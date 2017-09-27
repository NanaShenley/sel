using Assessment.Components;
using TestSettings;
using WebDriverRunner.internals;

namespace Assessment.DEA.Tests
{
    public class AssessmentSmokeTest
    {
       // [WebDriverTest( Enabled = true, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { "SmokeTests","test" })]
        public void AssesmentSmokeTest()
        {
           // SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, false);
            //SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Marksheets");
            //string comments =
            //        "Comments column Comments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments columnComments c";
            MarksheetGridHelper.SignInAsUser(MarksheetGridHelper.SchoolAdminUser, TestDefaults.Default.SENCoordinatorPassword);
            MarksheetGridHelper helper = new MarksheetGridHelper();
            helper.NavigatetoMarksheets().SearchForMarksheet("Recording MIST Year 2 - Year 2");

            //Integer Column Validation Test
            MarksheetGridHelper.ClickFirstCellofColumn("3");
            MarksheetGridHelper.GetEditor().SendKeys("11");
            MarksheetGridHelper.PerformTabKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("1");
            MarksheetGridHelper.PerformTabKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("201");
            MarksheetGridHelper.PerformEnterKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("200");





        }
    }
}
