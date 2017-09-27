using System;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.Text;
using GetDataViaWebServices;
using GetDataViaWebServices.DataEntityIO;
using TestSettings;

namespace DataExchange.POM.Helper
{
    public class ApplicationServerHelper : WebServiceBase
    {
        private readonly string _applicationServerAddress;
        private readonly SecurityToken _sessionToken;

        public ApplicationServerHelper(Guid schoolId)
        {
            _applicationServerAddress = Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath;

            _sessionToken = Login(TestDefaults.Default.TestUser,
                TestDefaults.Default.TestUserPassword,
                Convert.ToString(schoolId),
                TestDefaults.Default.TenantId.ToString(CultureInfo.InvariantCulture),
                _applicationServerAddress,
                Configuration.GetSutUrl() + TestDefaults.Default.SecurityServerPath);
        }

        public string GetDocumentStoreUrl(string propertyName, Guid storeId)
        {
            var callStatus = new CallStatus();
            using (ChannelFactory<IDataEntityIO> factory = GetApplicationServerChannelFactory(_applicationServerAddress))
            {
                factory.Credentials.UseIdentityConfiguration = true;
                IDataEntityIO secureConnection = factory.CreateChannelWithIssuedToken(_sessionToken);

                string documentUrl = secureConnection.GetDocumentStoreURL(propertyName, storeId, ref callStatus);

                if (callStatus.Result == CallStatusenumCallResult.Success) return documentUrl;

                var sb = new StringBuilder();
                foreach (var error in callStatus.Messages)
                {
                    sb.AppendLine(error.Message);
                }
                throw new Exception("Document Store Directory Does not exists !!!" + Environment.NewLine + sb);
            }
        }

        public bool SaveFileToDocumentStore(string documentUrl, byte[] content)
        {
            var callStatus = new CallStatus();

            using (ChannelFactory<IDataEntityIO> factory = GetApplicationServerChannelFactory(_applicationServerAddress))
            {
                factory.Credentials.UseIdentityConfiguration = true;
                IDataEntityIO secureConnection = factory.CreateChannelWithIssuedToken(_sessionToken);

                secureConnection.SaveFileToDocumentStore(documentUrl, content, ref callStatus);

                if (callStatus.Result == CallStatusenumCallResult.Success) return true;

                var sb = new StringBuilder();
                foreach (var error in callStatus.Messages)
                {
                    sb.AppendLine(error.Message);
                }
                throw new Exception("Upload Document Failed !!!" + Environment.NewLine + sb);
            }
        }
    }
}