using System.IdentityModel.Tokens;
using GetDataViaWebServices.DataEntityIO;
using WebDriverRunner.internals;
using Selene.Support.Attributes;

namespace GetDataViaWebServices
{
    public class WsExampleTests
    {
        //Note the service referances 
        //iSIMS.ApplicationServer.DataEntityIO
        //iSIMS.SecurityServer.UnsignedUserName
        //
        //Will need to be kept uptodate

        private const string SecurityServerAddress =
            "https://lab-two.sims8.com/iSIMSAuthorisationServerFarm1/iSIMSSTS.UnsignedUserName.svc";
        private const string ApplicationServerAddress =
            "https://lab-two.sims8.com/iSIMSApplicationServerFarm1/DataEntityIO.svc/Transport";
        
        private const string SchoolId = "d40f4b3b-101c-40e8-9f11-8faa1ed242c8";
        private const string TenantId = "1019999";
        private const string UserName = "testuser@capita.co.uk";
        private const string Password = "Pa$$w0rd";
        private const string Learnerid = "3cb5791c-95eb-4d06-a5c9-60c32751418e";
        [UnitTest(Enabled = true, Groups = new[] {"DataExample"})]
        public void GetDataFromTheWebService()
        {
            var sq = new StudentQuery(ApplicationServerAddress, SecurityServerAddress);
            //Login and get session token
            SecurityToken sessionToken = sq.Login(UserName, Password, SchoolId, TenantId);
            //Retrieve student entity based on id
            DataEntityCollectionDTO students = sq.RetrieveSingleLearnerById(Learnerid, sessionToken);
            //Display Student
            sq.DisplayDataInConsoleWindow(students);
            //Logoff
            sq.Logoff(UserName, Password, sessionToken);
        }

        [UnitTest(Enabled = true, Groups = new[] { "DataExample" })]
        public void SaveDataUsingTheWeservice()
        {
            var sq = new StudentQuery(ApplicationServerAddress, SecurityServerAddress);
            //Logon
            SecurityToken sessionToken = sq.Login(UserName, Password, SchoolId, TenantId);
            //Retrieve student entity based on id
            DataEntityCollectionDTO students = sq.RetrieveSingleLearnerById(Learnerid, sessionToken);
            //we only bring back 1 student so 
            DataEntityCollectionDTO changedStudents = sq.MakeModificatons(students, "Dude");
            
            //Save the student
            sq.Save(changedStudents, sessionToken);
            //Retrive student with new details
            students = sq.RetrieveSingleLearnerById(Learnerid, sessionToken);
            //Display Student
            sq.DisplayDataInConsoleWindow(students);
            //Logoff
            sq.Logoff(UserName, Password, sessionToken);
        }
    }

}
 

