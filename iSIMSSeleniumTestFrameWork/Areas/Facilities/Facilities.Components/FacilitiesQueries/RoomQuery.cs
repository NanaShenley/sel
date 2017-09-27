using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.Text;
using GetDataViaWebServices.DataEntityIO;
using TestSettings;

namespace Facilities.Components.FacilitiesQueries
{
    public class RoomQuery : WebServiceBase
    {
        private readonly string _applicationServerAddress;

        public RoomQuery()
        {
            _applicationServerAddress = Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath;
        }

        public DataEntityDTO CreateRoom(string roomShortName, string roomLongName)
        {
            return null;
        }

        public DataEntityCollectionDTO RetrieveEntityByNameQuery(string queryName, Dictionary<string, object> parameters,
            SecurityToken sessionToken)
        {
            SecuritySummary securitySummary = new SecuritySummary();
            CallStatus callStatus = new CallStatus();

            using (ChannelFactory<IDataEntityIO> factory = GetApplicationServerChannelFactory(_applicationServerAddress)
                )
            {
                factory.Credentials.UseIdentityConfiguration = true;
                IDataEntityIO secureConnection = factory.CreateChannelWithIssuedToken(sessionToken);

                DataEntityCollectionDTO entities = secureConnection.RunNamedQuery(queryName, parameters,
                    ref securitySummary, ref callStatus);

                if (callStatus.Result == CallStatusenumCallResult.Success) return entities;

                StringBuilder sb = new StringBuilder();
                foreach (ValidationError error in callStatus.Messages)
                {
                    sb.AppendLine(error.Message);
                }
                throw new Exception("Call did not complete successfully" + Environment.NewLine + sb);
            }

            //TODO: CONTINUE WITH THIS METHOD SIMILAR TO StaffQuery
            return null;
        }
    }
}