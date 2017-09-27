using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using GetDataViaWebServices.DataEntityIO;

using SecurityTokenElement = System.IdentityModel.Tokens.SecurityTokenElement;

namespace GetDataViaWebServices
{
    public class WebServiceBase
    {
        private readonly List<string> _changedFields = new List<string>();
        private const string SimsLoginContextSchoolClaimUrl = "http://www.capita.co.uk/InternationalSIMS/claims/context/School";
        private const string SimsLoginContextTenantClaimUrl = "http://www.capita.co.uk/InternationalSIMS/claims/context/TenantID";

        public ChannelFactory<IDataEntityIO> GetApplicationServerChannelFactory(string _applicationServerAddress)
        {
            WS2007FederationHttpBinding binding = new WS2007FederationHttpBinding(WSFederationHttpSecurityMode.TransportWithMessageCredential)
            {
                ReaderQuotas =
                {
                    MaxArrayLength = 2147483647,
                    MaxStringContentLength = 2147483647,
                    MaxDepth = int.MaxValue
                },
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue
            };

            binding.Security.Message.IssuedKeyType = SecurityKeyType.BearerKey;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.NegotiateServiceCredential = true;

            binding.UseDefaultWebProxy = true;
            binding.BypassProxyOnLocal = false;

            ChannelFactory<IDataEntityIO> factory = new ChannelFactory<IDataEntityIO>(binding, new EndpointAddress(_applicationServerAddress));

            return factory;
        }

        public DataEntityCollectionDTO UpdateEntity(DataEntityCollectionDTO entitys, string fieldName, string referencevalue)
        {
            foreach (int referenceId in entitys.TopLevelDtoIDs)
            {
                DataEntityDTO entity = entitys.DataEntityDtos[referenceId];
                ((DataEntityDTO.SimplePropertyDTOString)entity.Values[fieldName]).Value = referencevalue;
                _changedFields.Add(fieldName);
            }

            return entitys;
        }

        public void Save(DataEntityCollectionDTO entitys, SecurityToken sessionToken, string applicationServerURL, List<string> saveScope)
        {
            CallStatus callStatus = new CallStatus();

            using (ChannelFactory<IDataEntityIO> factory = GetApplicationServerChannelFactory(applicationServerURL))
            {
                DataEntityDTOChangeBatchCollection changes = new DataEntityDTOChangeBatchCollection
                {
                    Batches = new List<DataEntityDTOChangeBatch>()
                };
                DataEntityDTO.DataModelTypeDTO dataModelType = new DataEntityDTO.DataModelTypeDTO { SchemaName = "dbo", DataModelPurpose = "BusinessDataModel" };
                changes.DataModelType = dataModelType;
                DataEntityDTOChangeBatch batch = new DataEntityDTOChangeBatch { EntitiesToSave = entitys };
                DataEntitySaveContext batchSaveContext = new DataEntitySaveContext();
                List<string> alternateKeys = new List<string>();
                List<string> batchSaveScope = new List<string>();
                if (_changedFields.Any())
                {
                    batchSaveScope = _changedFields;
                }
                else if (saveScope.Any())
                {
                    batchSaveScope = saveScope;
                }
                batchSaveContext.SaveScope = batchSaveScope;
                batchSaveContext.AlternateKeyFields = alternateKeys;
                batchSaveContext.CustomWorkflows = new List<WorkflowPackage>();
                batchSaveContext.CustomDeleteWorkflows = new List<WorkflowPackage>();
                batch.SaveContext = batchSaveContext;
                changes.Batches.Add(batch);

                factory.Credentials.UseIdentityConfiguration = true;
                IDataEntityIO secureConnection = factory.CreateChannelWithIssuedToken(sessionToken);
                secureConnection.SaveEntityCollection(changes, ref callStatus);

                if (callStatus.Result == CallStatusenumCallResult.Success) return;

                StringBuilder sb = new StringBuilder();
                foreach (ValidationError error in callStatus.Messages)
                {
                    sb.AppendLine(error.Message);
                }

                throw new Exception("Call did not complete successfully" + Environment.NewLine + sb);
            }

        }

        public SecurityToken Login(string userName, string password, string schoolId, string tenantId, string applicationServerURL, string securityServerURL)
        {
            SecurityToken identityToken;

            using (WSTrustChannelFactory factory = GetTrustChannelFactory(securityServerURL))
            {
                if (factory.Credentials == null)
                {
                    throw new Exception("Credentails are null");
                }
                factory.Credentials.UserName.UserName = userName;
                factory.Credentials.UserName.Password = password;
                factory.Credentials.SupportInteractive = false;

                RequestSecurityToken rst = new RequestSecurityToken(
                    RequestTypes.Issue,
                    KeyTypes.Bearer) { AppliesTo = new EndpointReference(applicationServerURL) };

                IWSTrustChannelContract channel = factory.CreateChannel();

                RequestSecurityTokenResponse response;
                identityToken = channel.Issue(rst, out response);
            }

            using (WSTrustChannelFactory factory = GetTrustChannelFactory(securityServerURL))
            {
                if (factory.Credentials == null)
                {
                    throw new Exception("Credentails are null");
                }

                factory.Credentials.UserName.UserName = userName;
                factory.Credentials.UserName.Password = password;
                factory.Credentials.SupportInteractive = false;
                factory.Credentials.UseIdentityConfiguration = true;
                factory.CreateChannelWithIssuedToken(identityToken);

                IWSTrustChannelContract channel = factory.CreateChannel();

                RequestSecurityToken rst = new RequestSecurityToken(
                    RequestTypes.Issue,
                    KeyTypes.Bearer) { AdditionalContext = new AdditionalContext() };

                rst.AdditionalContext.Items.Add(new ContextItem(new Uri(SimsLoginContextSchoolClaimUrl), schoolId));
                rst.AdditionalContext.Items.Add(new ContextItem(new Uri(SimsLoginContextTenantClaimUrl), tenantId));
                rst.AppliesTo = new EndpointReference(applicationServerURL);
                rst.ActAs = new SecurityTokenElement(identityToken);

                RequestSecurityTokenResponse response;
                return channel.Issue(rst, out response);
            }

        }

        public bool Logoff(string userName, string password, SecurityToken sessionToken, string securityServerURL)
        {
            using (WSTrustChannelFactory factory = GetTrustChannelFactory(securityServerURL))
            {
                if (factory.Credentials == null)
                {
                    throw new Exception("Credentails are null");
                }
                factory.Credentials.UserName.UserName = userName;
                factory.Credentials.UserName.Password = password;
                factory.Credentials.SupportInteractive = false;

                RequestSecurityToken rst = new RequestSecurityToken(RequestTypes.Cancel)
                {
                    CancelTarget = new SecurityTokenElement(sessionToken)
                };

                IWSTrustChannelContract channel = factory.CreateChannel();

                RequestSecurityTokenResponse response = channel.Cancel(rst);

                return response.RequestedTokenCancelled;
            }
        }

        private WSTrustChannelFactory GetTrustChannelFactory(string securityServerURL)
        {
            WS2007HttpBinding binding = new WS2007HttpBinding
            {
                CloseTimeout = new TimeSpan(0, 10, 0),
                OpenTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
                ReaderQuotas =
                {
                    MaxArrayLength = 2147483647,
                    MaxStringContentLength = 2147483647,
                    MaxDepth = int.MaxValue
                },
                MaxReceivedMessageSize = int.MaxValue,
                Security =
                {
                    Mode = SecurityMode.TransportWithMessageCredential,
                    Message =
                    {
                        ClientCredentialType = MessageCredentialType.UserName,
                        EstablishSecurityContext = false,
                        NegotiateServiceCredential = true
                    }
                },
                UseDefaultWebProxy = true,
                BypassProxyOnLocal = false
            };

            EndpointAddress endpointAddress = new EndpointAddress(new Uri(securityServerURL));

            WSTrustChannelFactory channelFactory = new WSTrustChannelFactory(binding, endpointAddress);
            return channelFactory;
        }
    }
}
