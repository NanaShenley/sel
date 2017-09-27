using Assessment.Components;
using Assessment.Components.PageObject;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using System.Threading;
using TestSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Assessment.POS.CreateCurriculamMarksheet
{
    public class CurriculumMarksheet : BaseSeleniumComponents
    {

        [WebDriverTest(Enabled = false, Groups = new[] { "Add_AdHocColumn", "Newmarksheet" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void Add_AdHocColumn()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser);

            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Templates");
            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template(Blank)");
            WaitUntillAjaxRequestCompleted();
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            curriculummarksheetmaintainance.SelectStatement();
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            Thread.Sleep(500);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Expand();
            Thread.Sleep(500);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Collapse();
            Thread.Sleep(500);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Edit();
            Thread.Sleep(2000);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Cancel();
            Thread.Sleep(2000);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Delete();
            Thread.Sleep(2000);
        }

        [WebDriverTest(Enabled = false, Groups = new[] { "SaveTemplate", "Newmarksheet" }, Browsers = new[] {BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveTemplate()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser);

            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Templates");
            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template(Blank)");
            WaitUntillAjaxRequestCompleted();
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            curriculummarksheetmaintainance.SelectStatement();
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            Thread.Sleep(500);

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            Thread.Sleep(2000);
        }

        [WebDriverTest(Enabled = false, Groups = new[] { "ViewSelectDescription", "Newmarksheet" }, Browsers = new[] {BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void ViewSelectDescription()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser);

            //Going to desired path

            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Templates");
            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template(Blank)");
            WaitUntillAjaxRequestCompleted();
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);
            Thread.Sleep(500);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            
            int[] checkedboxIndex = { 5,6,7 };
            foreach (var index in checkedboxIndex)
            {
                curriculummarksheetmaintainance = curriculummarksheetmaintainance.CheckStatement(index);
            }

            int SelectedTextCount = curriculummarksheetmaintainance.GetSelectedCount();
            int GetCheckedCount = curriculummarksheetmaintainance.GetCheckedCount();
            Assert.AreEqual(SelectedTextCount, GetCheckedCount);         

        }
    }
}
