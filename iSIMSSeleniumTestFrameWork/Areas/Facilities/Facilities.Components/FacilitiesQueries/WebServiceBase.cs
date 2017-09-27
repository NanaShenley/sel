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

namespace Facilities.Components.FacilitiesQueries
{
    public class WebServiceBase
    {
        private readonly List<string> _changedFields = new List<string>();
        private const string SimsLoginContextSchoolClaimUrl = "http://www.capita.co.uk/InternationalSIMS/claims/context/School";
        private const string SimsLoginContextTenantClaimUrl = "http://www.capita.co.uk/InternationalSIMS/claims/context/TenantID";
        
        public ChannelFactory<IDataEntityIO> GetApplicationServerChannelFactory(string applicationServerAddress)
        {           
            var binding = new WS2007FederationHttpBinding(WSFederationHttpSecurityMode.TransportWithMessageCredential)
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

            var factory = new ChannelFactory<IDataEntityIO>(binding, new EndpointAddress(applicationServerAddress));

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

        public void Save(DataEntityCollectionDTO entitys, SecurityToken sessionToken, string applicationServerUrl, List<string> saveScope)
        {
            var callStatus = new CallStatus();

            using (var factory = GetApplicationServerChannelFactory(applicationServerUrl))
            {                            
                var changes = new DataEntityDTOChangeBatchCollection
                {
                    Batches = new List<DataEntityDTOChangeBatch>()
                };
                var dataModelType = new DataEntityDTO.DataModelTypeDTO { SchemaName = "dbo", DataModelPurpose = "BusinessDataModel" };
                changes.DataModelType = dataModelType;
                var batch = new DataEntityDTOChangeBatch { EntitiesToSave = entitys };
                var batchSaveContext = new DataEntitySaveContext();
                var alternateKeys = new List<string>();
                var batchSaveScope = new List<string>();
                if (_changedFields.Any())
                {
                    batchSaveScope = _changedFields;
                }
                else if(saveScope.Any())
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
                var secureConnection = factory.CreateChannelWithIssuedToken(sessionToken);
                secureConnection.SaveEntityCollection(changes, ref callStatus);

                if (callStatus.Result == CallStatusenumCallResult.Success) return;

                var sb = new StringBuilder();
                foreach (var error in callStatus.Messages)
                {
                    sb.AppendLine(error.Message);
                }

                throw new Exception("Call did not complete successfully" + Environment.NewLine + sb);
            }

        }

        public SecurityToken Login(string userName, string password, string schoolId, string tenantId, string applicationServerUrl, string securityServerUrl)
        {
            SecurityToken identityToken;

            using (WSTrustChannelFactory factory = GetTrustChannelFactory(securityServerUrl))
            {
                if (factory.Credentials == null)
                {
                    throw new Exception("Credentails are null");
                }
                factory.Credentials.UserName.UserName = userName;
                factory.Credentials.UserName.Password = password;
                factory.Credentials.SupportInteractive = false;

                var rst = new RequestSecurityToken(
                    RequestTypes.Issue,            
                    KeyTypes.Bearer) {AppliesTo = new EndpointReference(applicationServerUrl)};

                IWSTrustChannelContract channel = factory.CreateChannel();

                RequestSecurityTokenResponse response;
                identityToken = channel.Issue(rst, out response);
            }

            using (WSTrustChannelFactory factory = GetTrustChannelFactory(securityServerUrl))
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

                var channel = factory.CreateChannel();

                var rst = new RequestSecurityToken(
                    RequestTypes.Issue,
                    KeyTypes.Bearer) {AdditionalContext = new AdditionalContext()};

                rst.AdditionalContext.Items.Add(new ContextItem(new Uri(SimsLoginContextSchoolClaimUrl), schoolId));
                rst.AdditionalContext.Items.Add(new ContextItem(new Uri(SimsLoginContextTenantClaimUrl), tenantId));
                rst.AppliesTo = new EndpointReference(applicationServerUrl);
                rst.ActAs = new SecurityTokenElement(identityToken);

                RequestSecurityTokenResponse response;
                return channel.Issue(rst, out response);
            }

        }

        public bool Logoff(string userName, string password, SecurityToken sessionToken, string securityServerUrl)
        {
            using (var factory = GetTrustChannelFactory(securityServerUrl))
            {
                if (factory.Credentials == null)
                {
                    throw new Exception("Credentails are null");
                }
                factory.Credentials.UserName.UserName = userName;
                factory.Credentials.UserName.Password = password;
                factory.Credentials.SupportInteractive = false;

                var rst = new RequestSecurityToken(RequestTypes.Cancel)
                {
                    CancelTarget = new SecurityTokenElement(sessionToken)
                };

                var channel = factory.CreateChannel();

                var response = channel.Cancel(rst);

                return response.RequestedTokenCancelled;
            }
        }

        private WSTrustChannelFactory GetTrustChannelFactory(string securityServerUrl)
        {
            var binding = new WS2007HttpBinding
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

            var endpointAddress = new EndpointAddress(new Uri(securityServerUrl));

            var channelFactory = new WSTrustChannelFactory(binding, endpointAddress);
            return channelFactory;
        }
    }
}
