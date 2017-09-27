using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using GetDataViaWebServices.DataEntityIO;
using SecurityTokenElement = System.IdentityModel.Tokens.SecurityTokenElement;


namespace GetDataViaWebServices
{
    // ReSharper disable SuggestUseVarKeywordEvident
    /// <summary>
    /// Sample sequence for a typical login->fetch->modify>save->logoff for a third party consumer of SIMS data
    /// </summary>

    public class StudentQuery
    {
        private const string SimsLoginContextSchoolClaimUrl = "http://www.capita.co.uk/InternationalSIMS/claims/context/School";
        private const string SimsLoginContextTenantClaimUrl = "http://www.capita.co.uk/InternationalSIMS/claims/context/TenantID";
        
        private readonly string _applicationServerAddress;
        private readonly string _securityServerAddress;

        public StudentQuery(string applicationServerAddress, string securityServerAddress)
        {
            _applicationServerAddress = applicationServerAddress;
            _securityServerAddress = securityServerAddress;
        }

        /// <summary>
        /// Make some modifications to the Learner entity fetched by RetrieveSingleLearnerByID
        /// </summary>
        internal DataEntityCollectionDTO MakeModificatons(DataEntityCollectionDTO entitys, string editedvalue)
        {
            // The data structure returned may have many entities, (in our case a mixture of Address, LearnerAddress and Learner). The TopLevelDtoIDs collection
            // gives the list of unique IDs that represent the Learner entitities we requested.
            foreach (int referenceId in entitys.TopLevelDtoIDs)
            {
                // In our case there is only one expected TopLevel entity 
                DataEntityDTO entity = entitys.DataEntityDtos[referenceId];
                // SimplePropertyDTO
                ((DataEntityDTO.SimplePropertyDTOString)entity.Values["PreferredForename"]).Value = editedvalue;
            }

            return entitys;
        }

        internal void Save(DataEntityCollectionDTO entitys, SecurityToken sessionToken)
        {
            // Almost all iSIMS calls fill a callStatus response message in with status and error messages.
            CallStatus callStatus = new CallStatus();
            // Create a communication channel factory to communicate with the iSIMS application server
            using (ChannelFactory<IDataEntityIO> factory = GetApplicationServerChannelFactory())
            {
                // Because our communication now will be secured using our Identity Token we need to use some WIF extension methods to make sure the identity token
                // is sent as a Cookie on the SOAP call that WCF will be generating.
                factory.Credentials.UseIdentityConfiguration = true;
                IDataEntityIO secureConnection = factory.CreateChannelWithIssuedToken(sessionToken);

                DataEntityDTOChangeBatchCollection changes = new DataEntityDTOChangeBatchCollection
                {
                    Batches = new List<DataEntityDTOChangeBatch>()
                };
                DataEntityDTO.DataModelTypeDTO dataModelType = new DataEntityDTO.DataModelTypeDTO { SchemaName = "dbo", DataModelPurpose = "BusinessDataModel" };
                changes.DataModelType = dataModelType;
                DataEntityDTOChangeBatch batch = new DataEntityDTOChangeBatch { EntitiesToSave = entitys};
                DataEntitySaveContext batchSaveContext = new DataEntitySaveContext();
                List<string> alternateKeys = new List<string>();
                // Simplest thing to do is declare which field we believe we have changed
                List<string> batchSaveScope = new List<string> { "Learner.PreferredForename" };
                List<WorkflowPackage> customWorkflows = new List<WorkflowPackage>();
                batchSaveContext.SaveScope = batchSaveScope;
                batchSaveContext.AlternateKeyFields = alternateKeys;
                batchSaveContext.CustomWorkflows = customWorkflows;
                batchSaveContext.CustomDeleteWorkflows = new List<WorkflowPackage>();
                batch.SaveContext = batchSaveContext;
                changes.Batches.Add(batch);
                secureConnection.SaveEntityCollection(changes, ref callStatus);
              



                // Handle an unsuccessful call.
                if (callStatus.Result != CallStatusenumCallResult.Success)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ValidationError error in callStatus.Messages)
                    {
                        sb.AppendLine(error.Message);
                    }
                    throw new Exception("Call did not complete successfully" + Environment.NewLine + sb);
                }
            }
            
        }

        /// <summary>
        /// Example of retrieving some data from the application server. In this case we will use the ReadEnity call to retrieve data for a single Learner entity.
        /// 
        /// This illustrates the method of constructing a single entity query, and handling the results that are returned. It also illustrates the use of the CallStatus
        /// structure to retrieve information about the call, and any failures that occured.
        /// </summary>
        /// <param name="learnerid"></param>
        /// <param name="sessionToken"></param>
        internal DataEntityCollectionDTO RetrieveSingleLearnerById(string learnerid, SecurityToken sessionToken)
        {
            // The security summary will be filled with any notifications of data fields which were removed by the security protocols prior to being sent to the recipient. This 
            // enables you to see whether your data has been redacted before you got to see it.
            SecuritySummary securitySummary = new SecuritySummary();

            // Almost all iSIMS calls fill a callStatus response message in with status and error messages.
            CallStatus callStatus = new CallStatus();

            // Create a communication channel factory to communicate with the iSIMS application server
            using (ChannelFactory<IDataEntityIO> factory = GetApplicationServerChannelFactory())
            {
                // Because our communication now will be secured using our Identity Token we need to use some WIF extension methods to make sure the identity token
                // is sent as a Cookie on the SOAP call that WCF will be generating.
                factory.Credentials.UseIdentityConfiguration = true;
                IDataEntityIO secureConnection = factory.CreateChannelWithIssuedToken(sessionToken);
                Guid studentId = new Guid(learnerid);

                // Construct a query to read a specific entity from SIMS8
                DataEntityCollectionDTO entities = secureConnection.ReadEntity(// Tell it which specific unique entity we want to fetch
                    studentId, // Tell it what type of entity this is
                    "Learner", // Tell it what scope of data we want to get back from the call.
                    new List<string>(new[]
                    {
                        // The surname and forename
                        "Learner.LegalSurname",
                        "Learner.LegalForename",
                        "Learner.PreferredForename",
                        // The Unique Pupil Number
                        "Learner.UPN",
                        // The Learners Addresses, start date. Note that there are many Addresses attached to a single Learner
                        "Learner.LearnerAddresses.StartDate",
                        // The Learners Addresses, Post Code
                        "Learner.LearnerAddresses.Address.PostCode"
                    }), ref securitySummary, ref callStatus);

                // Handle an unsuccessful call.
                if (callStatus.Result != CallStatusenumCallResult.Success)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ValidationError error in callStatus.Messages)
                    {
                        sb.AppendLine(error.Message);
                    }
                    throw new Exception("Call did not complete successfully" + Environment.NewLine + sb);
                }
                return entities;
            }
        }

        /// <summary>
        /// Display the data in the passed DataEntityCollectionDTO in the console window. This gives an example of how the content 
        /// of a loosly typed entity property bag works.
        /// </summary>
        /// <param name="entities"></param>
        internal void DisplayDataInConsoleWindow(DataEntityCollectionDTO entities)
        {
            // The data structure returned may have many entities, (in our case a mixture of Address, LearnerAddress and Learner). The TopLevelDtoIDs collection
            // gives the list of unique IDs that represent the Learner entitities we requested.
            foreach (int referenceId in entities.TopLevelDtoIDs)
            {
                // In our case there is only one expected TopLevel entity - the Learner 8BCF76BF-C067-98AB-5DFC-88D62DE77450
                DataEntityDTO entity = entities.DataEntityDtos[referenceId];
                Console.WriteLine("Entity " + entity.EntityName + " " + entity.ID.ToString());

                // Extract the fields from the DataEntityDTO.Values dictionary
                // Dangerous assumption that these Values can be cast to a String - if they are Null they will be of type SimplePropertyDTONull, not SimplePropertyDTOString
                // programmers should construct a conversion function to prevent having to check this every time, returning a Nullable<Type> when passed an instance of a
                // SimplePropertyDTO
                Console.WriteLine("      Surname " + ((DataEntityDTO.SimplePropertyDTOString)entity.Values["LegalSurname"]).Value);
                Console.WriteLine("     Forename " + ((DataEntityDTO.SimplePropertyDTOString)entity.Values["LegalForename"]).Value);
                Console.WriteLine("     PreferredForename " + ((DataEntityDTO.SimplePropertyDTOString)entity.Values["PreferredForename"]).Value);
                // In theory any value might be returned as Null rather than the expected Type. Third party developers should assume that, due to security redaction or 
                // naturally occuring blanks in the data, that any Value property of an DataEntityDTO could be of Type SimplePropertyDTONull. 
                if (entity.Values.ContainsKey("UPN"))
                {
                    if (entity.Values["UPN"] is DataEntityDTO.SimplePropertyDTONull)
                    {
                        Console.WriteLine("          UPN NULL");
                    }
                    else
                    {
                        Console.WriteLine("          UPN " + ((DataEntityDTO.SimplePropertyDTOString)entity.Values["UPN"]).Value);
                    }
                }

                if (entity.Values.ContainsKey("LearnerAddresses"))
                {
                    // Spin round each of the possible addresses attached to this learner.
                    foreach (DataEntityDTO.ReferencePropertyDTO learerReference in ((DataEntityDTO.ReferencePropertyDTOArray)entity.Values["LearnerAddresses"]).ReferenceProperties)
                    {
                        if (learerReference.InternalReferenceID.HasValue)
                        {
                            // Get the learnerAddress entity
                            DataEntityDTO learnerAddressEntity = entities.DataEntityDtos[learerReference.InternalReferenceID.Value];
                            Date dateValue = ((DataEntityDTO.SimplePropertyDTODate)learnerAddressEntity.Values["StartDate"]).Value;
                            Console.WriteLine("      Address Start Date " + dateValue.internalDateTime.Date);

                            // Get the address unique ID
                            DataEntityDTO.ReferencePropertyDTO addressReference = ((DataEntityDTO.ReferencePropertyDTO)learnerAddressEntity.Values["Address"]);
                            // Get the address entity.
                            // ReSharper disable once AssignNullToNotNullAttribute
                            // ReSharper disable once PossibleInvalidOperationException
                            DataEntityDTO addressEntity = entities.DataEntityDtos[addressReference.InternalReferenceID.Value];
                            Console.WriteLine("      Address PostCode    " + ((DataEntityDTO.SimplePropertyDTOString)addressEntity.Values["PostCode"]).Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create the communication channel to the SIMS security server.
        /// Instead of relying on unweildy .config files full of WCF binding information, we will do this longhand as an example.
        /// </summary>
        /// <returns>A channel factory configured with WS2007HttpBinding</returns>
        private WSTrustChannelFactory GetTrustChannelFactory()
        {
            // Use WS2007HttpBinding
            WS2007HttpBinding binding = new WS2007HttpBinding();
            // Timeout settings etc
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            // Quotas
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;

            // SSL security mode
            binding.Security.Mode = SecurityMode.TransportWithMessageCredential;

            // The credentials we will pass will be username/password
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.NegotiateServiceCredential = true;

            binding.UseDefaultWebProxy = true;
            binding.BypassProxyOnLocal = false;

            EndpointAddress endpointAddress = new EndpointAddress(new Uri(_securityServerAddress));

            WSTrustChannelFactory channelFactory = new WSTrustChannelFactory(binding, endpointAddress);
            return channelFactory;
        }


        /// <summary>
        /// Create a communication channel to the SIMS application server.
        /// Instead of relying on unweildy .config files full of WCF binding information, we will do this longhand as an example.
        /// </summary>
        /// <returns></returns>
        private ChannelFactory<IDataEntityIO> GetApplicationServerChannelFactory()
        {
            // Manually set up the connection parameters to the application server using HTTPS to encrypt the message content and the HTTP stream.
            WS2007FederationHttpBinding binding = new WS2007FederationHttpBinding(WSFederationHttpSecurityMode.TransportWithMessageCredential);
            // Quotas
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferPoolSize = int.MaxValue;

            // Notify the connection that we will be providing a security token.
            binding.Security.Message.IssuedKeyType = SecurityKeyType.BearerKey;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.NegotiateServiceCredential = true;


            binding.UseDefaultWebProxy = true;
            binding.BypassProxyOnLocal = false;

            // Create a communication channel factory to communicate with the iSIMS application server
            ChannelFactory<IDataEntityIO> factory = new ChannelFactory<IDataEntityIO>(binding, new EndpointAddress(_applicationServerAddress));


            return factory;
        }


        #region LOGONandLOGOFF
        /// <summary>
        /// Login to the application.
        /// 
        /// The login process takes two steps - 
        /// First; establish the identity of the user using standard credentials.
        /// Second; use this identity to login to SIMS for the purpose of carrying out data querying.
        /// </summary>
        internal SecurityToken Login(string userName, string password, string schoolId, string tenantId)
        {
            // Temporaryily store the Identity Token issued by the identity server so that we can use it to prove our identity to the application login server.
            // Note that; although iSIMS provides both Identity and Application logins on the same service endpoint (iSIMSSTS) the Identity Service might have 
            // a different endpoint where iSIMS has been integrated with an external identity management system such as ADFS, Shibboleth etc
            // ReSharper disable once RedundantAssignment
            SecurityToken identityToken = null;

            // Step 1 : Pass credentials to the identity server to get a token bearing our verified identity.
            using (WSTrustChannelFactory factory = GetTrustChannelFactory())
            {
                // Set up credentials
                if (factory.Credentials == null)
                {
                    throw new Exception("Credentails are null");
                }
                factory.Credentials.UserName.UserName = userName;
                factory.Credentials.UserName.Password = password;
                factory.Credentials.SupportInteractive = false;
                // Create a "request for token" SOAP message.
                RequestSecurityToken rst = new RequestSecurityToken(
                    // We want the security server to Issue us with a token
                    RequestTypes.Issue,
                    // We want the token type to be a Bearer token (i.e. not symmetric key or other mutually guaranteed certificate based mechanism)
                    KeyTypes.Bearer);

                // We want a token that will permit us to communicate with the application server 
                rst.AppliesTo = new EndpointReference(_applicationServerAddress);

                // Create the communicaiton channel
                IWSTrustChannelContract channel = factory.CreateChannel();

                // Request an identity token
                RequestSecurityTokenResponse response;
                identityToken = channel.Issue(rst, out response);
            }

            // Step 2 : Use the identity token to get a further token which can be used to access resources within the system. This token is obtained from the same
            // location in this example, but the Application Identity Server (here) and the Identity Verification Server (above) might be different URLs in Single Sign-on 
            // implementations.
            using (WSTrustChannelFactory factory = GetTrustChannelFactory())
            {
                // Provide the credentials again.
                if (factory.Credentials == null)
                {
                    throw new Exception("Credentails are null");
                }
                factory.Credentials.UserName.UserName = userName;
                factory.Credentials.UserName.Password = password;
                factory.Credentials.SupportInteractive = false;

                // Because our communication now will be secured using our Identity Token we need to use some WIF extension methods to make sure the identity token
                // is sent as a Cookie on the SOAP call that WCF will be generating.
                factory.Credentials.UseIdentityConfiguration = true;
                factory.CreateChannelWithIssuedToken(identityToken);

                // Create the communication channel
                IWSTrustChannelContract channel = factory.CreateChannel();

                // create token request
                RequestSecurityToken rst = new RequestSecurityToken(
                    // We want the security server to Issue us with a token
                    RequestTypes.Issue,
                    // We want the token type to be a Bearer token (i.e. not symmetric key or other mutually guaranteed certificate based mechanism)
                    KeyTypes.Bearer);

                // Add the school ID that we claim to have access to.
                rst.AdditionalContext = new AdditionalContext();
                rst.AdditionalContext.Items.Add(new ContextItem(new Uri(SimsLoginContextSchoolClaimUrl), schoolId));
                rst.AdditionalContext.Items.Add(new ContextItem(new Uri(SimsLoginContextTenantClaimUrl), tenantId));
                // This is the endpoint for which the token will be valid.
                rst.AppliesTo = new EndpointReference(_applicationServerAddress);

                // Becuase we have already established our identity, we want this call to "Act As" us. This enables the receving web service to be passed our 
                // identity in the thread principal.
                rst.ActAs = new SecurityTokenElement(identityToken);

                // Request an application session token.
                RequestSecurityTokenResponse response;
                return channel.Issue(rst, out response);
            }

        }

        internal bool Logoff(string userName, string password, SecurityToken sessionToken)
        {
            using (WSTrustChannelFactory factory = GetTrustChannelFactory())
            {
                //// Set up credentials
                if (factory.Credentials == null)
                {
                    throw new Exception("Credentails are null");
                }
                factory.Credentials.UserName.UserName = userName;
                factory.Credentials.UserName.Password = password;
                factory.Credentials.SupportInteractive = false;

                // Create a "request for token" SOAP message.
                // We want the security server to Issue us with a token
                RequestSecurityToken rst = new RequestSecurityToken(RequestTypes.Cancel);
                // the token we want to cancel
                rst.CancelTarget = new SecurityTokenElement(sessionToken);


                // Create the communicaiton channel
                IWSTrustChannelContract channel = factory.CreateChannel();

                // Cancel token
                // ReSharper disable once UnusedVariable
                RequestSecurityTokenResponse response = channel.Cancel(rst);
                return response.RequestedTokenCancelled;

            }
        }
        #endregion
        // ReSharper restore SuggestUseVarKeywordEvident
    }
}
