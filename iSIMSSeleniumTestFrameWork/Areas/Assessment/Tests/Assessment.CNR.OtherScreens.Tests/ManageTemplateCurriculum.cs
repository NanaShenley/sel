using TestSettings;
using WebDriverRunner.webdriver;
using Assessment.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using Selene.Support.Attributes;
using OpenQA.Selenium.Support.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment.Components.PageObject;
using System.Collections.Generic;
using System.Threading;
using Assessment.Components;
using OpenQA.Selenium;

namespace Assessment.CNR.Other.Assessment.Screens.Tests
{
    [TestClass]
    public class ManageTemplateCurriculum : BaseSeleniumComponents
    {

        #region MS Unit Testing support
        public TestContext TestContext { get; set; }
        [TestInitialize]
        public void Init()
        {
            TestRunner.VSSeleniumTest.Init(this, TestContext);
        }
        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.VSSeleniumTest.Cleanup(TestContext);
        }
        #endregion

        public const string TopicSetup = "setuptopic";
        public const string YearGroupSelected = "Year 1";
        public const string TermSelected = "Autumn";
        #region New Curriculum Marksheet Template

        //This is to demonstrate Expand and collapse features in the Add columsn screen
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "ExpandCollapseCurriculumStatementInTreeControl", "NewCurriculumTemplate", "ManageTemplate" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ExpandCollapseCurriculumStatementInTreeControl()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //Going to desired path

            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WaitUntillAjaxRequestCompleted();

            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();

            //Enter Template name and Description
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            //String CurriculumToolTip = curriculummarksheetmaintainance.GetCurriculumButtonTooltip();

            //Select Curriculum option and then select a statement from the tree
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            //Select 4th Node in the tree control which will be first statement
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Expand();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Collapse();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Edit();

        }

        // This will save the template for curriculum columns
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "SaveTemplate", "NewCurriculumTemplate", "ManageTemplate", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SaveTemplate()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            //Select 4th Node in the tree control which will be first statement
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
        }

        // This will save the template for curriculum columns
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "SaveTemplateCreateMarksheet", "NewCurriculumTemplate", "ManageTemplate", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SaveTemplateCreateMarksheet()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            //Select 4th Node in the tree control which will be first statement
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            createMarksheet.IsCreateMarksheetButtonVisibleAfterSavingTemplate();
            createMarksheet.Marksheetcreate();
            WaitUntillAjaxRequestCompleted();

            AssignTemplateGroupAndFilter assignTemplateGroupAndFilter = new AssignTemplateGroupAndFilter();
            assignTemplateGroupAndFilter.HideSearchButtonClick();
            assignTemplateGroupAndFilter.AssigGroupClick();
            assignTemplateGroupAndFilter.SelectYearGroup("Year 1");
            assignTemplateGroupAndFilter.OkButtonClick();
            assignTemplateGroupAndFilter.SaveButton();
            int position = 1;
            string marksheetName = assignTemplateGroupAndFilter.GetValueFromMarksheetDetails(position);
            Assert.IsTrue(marksheetName.Contains(TemplateName + " - " + "Year 1"));
            assignTemplateGroupAndFilter.clickElementAtPosition(position);
            WaitUntillAjaxRequestCompleted();
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitUntilDisplayed(MarksheetConstants.MarksheetTitle);
            IWebElement marksheetEntryTitle = WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetTitle);
            Assert.IsTrue(marksheetName.Contains(marksheetEntryTitle.Text));

        }

        //This is to verify if the statement description appears once a statement is selected in the tree control
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "ViewSelectDescription", "NewCurriculumTemplate", "ManageTemplate" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ViewSelectDescription()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            Thread.Sleep(8000);
            //Going to desired path

            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            Thread.Sleep(8000);
            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            //Select 5th, 6th and 7th Node in the tree control which will be first, second and third statement
            int[] checkedboxIndex = { 5, 6, 7 };
            foreach (int index in checkedboxIndex)
            {
                curriculummarksheetmaintainance.CheckStatement(index);
            }

            int SelectedTextCount = curriculummarksheetmaintainance.GetSelectedCount();
            int GetCheckedCount = curriculummarksheetmaintainance.GetCheckedCount();
            Assert.AreEqual(SelectedTextCount, GetCheckedCount);

        }

        #endregion

        #region Edit Marksheet Template

        //This will search for an existing template open it an dverify details such as Name and Description
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "SearchForExistingTemplate", "EditTemplate", "ManageTemplate" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SearchForExistingTemplate()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");

            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            //Select 4th Node in the tree control which will be first statement
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            // Edit Template Started
            WaitUntillAjaxRequestCompleted();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickModifyExistingButton();
            WaitUntillAjaxRequestCompleted();
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            String GetTemplateName = editTemplate.GetExistingTemplateNameInEditDialog(TemplateName);
            Assert.AreEqual(GetTemplateName, TemplateName);
        }

        //Select a template to view the template in edit mode. Validate the columns , name and description
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "VerifyExistingTemplateDetails", "EditTemplate", "ManageTemplate" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyExistingTemplateDetails()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");

            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            //Select 4th Node in the tree control which will be first statement
            curriculummarksheetmaintainance.SelectStatement(4);
            int GetStatementCount = curriculummarksheetmaintainance.GetCheckedCount();
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            // Edit Template Started
            WaitUntillAjaxRequestCompleted();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickModifyExistingButton();
            WaitUntillAjaxRequestCompleted();
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            editTemplate.SelectTemplateByName(TemplateName);
            editTemplate.OpenTemplate();
            //Verify Template Name
            string marksheetTemplateName = createMarksheet.getMarksheetTemplateName();
            Assert.AreEqual(TemplateName, marksheetTemplateName);

            //Verify Template Description
            string marksheetTemplateDescription = createMarksheet.getMarksheetTemplateDescription();
            Assert.AreEqual(TemplateDescription, marksheetTemplateDescription);
            createMarksheet = createMarksheet.NextButtonClick();

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();

            int GetCheckedCount = curriculummarksheetmaintainance.GetCheckedCount();
            Assert.AreEqual(GetStatementCount, GetCheckedCount);

        }


        //As part of Editing a atemplate it will add a new statement from the tree control
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "EditCurriculumStatementInTreeControl", "EditTemplate", "ManageTemplate" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EditCurriculumStatementInTreeControl()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //Going to desired path

            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WaitUntillAjaxRequestCompleted();

            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();

            //Enter Template name and Description
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            //Select Curriculum option and then select a statement from the tree
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            //Select 4th Node in the tree control which will be first statement
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Edit();
            Thread.Sleep(3000);
            //Select 5th Node in the tree control which will be second statements
            curriculummarksheetmaintainance.SelectStatement(5);
            int SelectedTextCount = curriculummarksheetmaintainance.GetSelectedCount();
            int GetCheckedCount = curriculummarksheetmaintainance.GetCheckedCount();
            Assert.AreEqual(SelectedTextCount, GetCheckedCount);

        }

        //As part of Editing a atemplate it will delete a statement from the tree control
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "DeleteCurriculumStatementInTreeControl", "EditTemplate", "ManageTemplate", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DeleteCurriculumStatementInTreeControl()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //Going to desired path

            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WaitUntillAjaxRequestCompleted();

            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();

            //Enter Template name and Description
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            //Select Curriculum option and then select a statement from the tree
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            //Select 4th Node in the tree control which will be first statement
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Delete();
            Thread.Sleep(2000);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();

            int GetCheckedCount = curriculummarksheetmaintainance.GetCheckedCount();
            Assert.AreEqual(0, GetCheckedCount);

        }


        //Edit template details such Name, Description and statments and verify
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "EditTemplateDetails", "EditTemplate", "ManageTemplate", "EndToEnd", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EditTemplateDetails()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");

            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            //Select 4th Node in the tree control which will be first statement
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            // Edit Template Started
            WaitUntillAjaxRequestCompleted();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickModifyExistingButton();
            WaitUntillAjaxRequestCompleted();
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            editTemplate.SelectTemplateByName(TemplateName);
            editTemplate.OpenTemplate();

            //Edit Name and Description
            TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();

            //Select another statement
            //Select 6th Node in the tree control which will be Third statement
            curriculummarksheetmaintainance.SelectStatement(6);
            int GetStatementSelected = curriculummarksheetmaintainance.GetCheckedCount();

            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            WaitUntillAjaxRequestCompleted();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            editTemplate = curriculummarksheetmaintainance.ClickModifyExistingButton();
            WaitUntillAjaxRequestCompleted();
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            editTemplate.SelectTemplateByName(TemplateName);
            editTemplate.OpenTemplate();

            string marksheetTemplateName = createMarksheet.getMarksheetTemplateName();
            Assert.AreEqual(TemplateName, marksheetTemplateName);

            string marksheetTemplateDescription = createMarksheet.getMarksheetTemplateDescription();
            Assert.AreEqual(TemplateDescription, marksheetTemplateDescription);
            createMarksheet.NextButtonClick();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();

            int GetCheckedCount = curriculummarksheetmaintainance.GetCheckedCount();
            Assert.AreEqual(GetStatementSelected, GetCheckedCount);


        }

        //Add additional columns in marksheet template
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "EditTemplateAndAddAdditionalColumns", "EditTemplate", "ManageTemplate", "EndToEnd" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EditTemplateAndAddAdditionalColumns()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");

            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            //Select 4th Node in the tree control which will be first statement
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            createMarksheet.PupilInformationClick();

            //Add additional columns
            PupilDetails pupilDetails = new PupilDetails();

            pupilDetails.SelectPupilDetailColumnCheckBox("Date Of Birth");
            createMarksheet.PupilDetailsDoneClick();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            WaitUntillAjaxRequestCompleted();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickModifyExistingButton();
            WaitUntillAjaxRequestCompleted();
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            editTemplate.SelectTemplateByName(TemplateName);
            editTemplate.OpenTemplate();
            createMarksheet.NextButtonClick();
            createMarksheet.PupilInformationClick();
            Assert.IsTrue(pupilDetails.SelectPupilDetailColumnCheckBox("Date Of Birth"));

        }

        #endregion

        #region New From Existing Template
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "ClickManageTemplate", "CopyTemplate", "ManageTemplate" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ClickManageTemplate()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WaitUntillAjaxRequestCompleted();
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            Assert.IsTrue(curriculummarksheetmaintainance.IsCreateMarksheetTemplateVisible());
        }

        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "ClickNewFromExistingTemplate", "CopyTemplate", "ManageTemplate" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ClickNewFromExistingTemplate()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickCopyFromExistingButton();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(editTemplate.IsNewFromExistingDialogVisible());
        }

        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "SearchTemplateToCopy", "CopyTemplate", "ManageTemplate", "EndToEnd" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SearchTemplateToCopy()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            // creating a new template
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            WaitUntillAjaxRequestCompleted();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            // Copy Template Started
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickCopyFromExistingButton();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(editTemplate.IsNewFromExistingDialogVisible());
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            Assert.IsTrue(editTemplate.IsSearchedMarksheetTemplatePresent(TemplateName));
        }

        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "SearchAndSelectTemplateOnDialog", "CopyTemplate", "ManageTemplate", "EndToEnd" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SearchAndSelectTemplateOnDialog()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            // creating a new template
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            WaitUntillAjaxRequestCompleted();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            // Copy Template Started
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickCopyFromExistingButton();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(editTemplate.IsNewFromExistingDialogVisible());
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            Assert.IsTrue(editTemplate.IsSearchedMarksheetTemplatePresent(TemplateName));
            editTemplate.SelectTemplateByName(TemplateName);
            Assert.AreEqual(TemplateName, editTemplate.getMarksheetTemplateNameFromNewFromExistingDialog());
        }

        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "SelectTemplateToCopy", "CopyTemplate", "ManageTemplate", "EndToEnd" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SelectTemplateToCopy()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            // creating a new template
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            curriculummarksheetmaintainance.SelectStatement(4);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            WaitUntillAjaxRequestCompleted();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            // Copy Template Started
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickCopyFromExistingButton();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(editTemplate.IsNewFromExistingDialogVisible());
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            Assert.IsTrue(editTemplate.IsSearchedMarksheetTemplatePresent(TemplateName));
            editTemplate.SelectTemplateByName(TemplateName);
            Assert.AreEqual(TemplateName, editTemplate.getMarksheetTemplateNameFromNewFromExistingDialog());
            editTemplate.SelectTemplate();
            Assert.AreEqual("Copy of " + TemplateName, createMarksheet.getMarksheetTemplateName());

        }

        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "EditCopiedTemplate", "CopyTemplate", "ManageTemplate", "EndToEnd" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EditCopiedTemplate()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            // creating a new template
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            curriculummarksheetmaintainance.SelectStatement(4);
            curriculummarksheetmaintainance.SelectStatement(6);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(createMarksheet.IsCreateMarksheetButtonVisibleAfterSavingTemplate());
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            // Copy Template Started
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickCopyFromExistingButton();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(editTemplate.IsNewFromExistingDialogVisible());
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            Assert.IsTrue(editTemplate.IsSearchedMarksheetTemplatePresent(TemplateName));
            editTemplate.SelectTemplateByName(TemplateName);
            Assert.AreEqual(TemplateName, editTemplate.getMarksheetTemplateNameFromNewFromExistingDialog());
            editTemplate.SelectTemplate();
            Assert.AreEqual("Copy of " + TemplateName, createMarksheet.getMarksheetTemplateName());
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.EditTemplateDescription(TemplateDescription + " . This template is copied from existing template.");
            createMarksheet = createMarksheet.NextButtonClick();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            curriculummarksheetmaintainance.UnSelectStatement(4);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(createMarksheet.IsCreateMarksheetButtonVisibleAfterSavingTemplate());
        }

        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "EditCopiedTemplateAndVerify", "CopyTemplate", "ManageTemplate", "EndToEnd" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EditCopiedTemplateAndVerify()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            // creating a new template
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.FillTemplateDetails("DFE");
            curriculummarksheetmaintainance.SelectStatement(1);
            curriculummarksheetmaintainance.SelectStatement(2);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(createMarksheet.IsCreateMarksheetButtonVisibleAfterSavingTemplate());
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            // Copy Template Started
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickCopyFromExistingButton();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(editTemplate.IsNewFromExistingDialogVisible());
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            Assert.IsTrue(editTemplate.IsSearchedMarksheetTemplatePresent(TemplateName));
            editTemplate.SelectTemplateByName(TemplateName);
            Assert.AreEqual(TemplateName, editTemplate.getMarksheetTemplateNameFromNewFromExistingDialog());
            editTemplate.SelectTemplate();
            Assert.AreEqual("Copy of " + TemplateName, createMarksheet.getMarksheetTemplateName());
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.EditTemplateDescription(TemplateDescription + " . This template is copied from existing template.");
            createMarksheet = createMarksheet.NextButtonClick();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.FillTemplateDetails("DFE");
            curriculummarksheetmaintainance.UnSelectStatement(4);
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(createMarksheet.IsCreateMarksheetButtonVisibleAfterSavingTemplate());
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            editTemplate = curriculummarksheetmaintainance.ClickModifyExistingButton();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(editTemplate.IsNewFromExistingDialogVisible());
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            Assert.IsTrue(editTemplate.IsSearchedMarksheetTemplatePresent(TemplateName));
            editTemplate.SelectTemplateByName(TemplateName);
            Assert.AreEqual(TemplateName, editTemplate.getMarksheetTemplateNameFromNewFromExistingDialog());
            editTemplate.OpenTemplate();
            Assert.AreEqual(TemplateName, createMarksheet.getMarksheetTemplateName());
            createMarksheet = createMarksheet.NextButtonClick();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();
            int selectedCount = curriculummarksheetmaintainance.GetCheckedCount();
            Assert.AreEqual(selectedCount, 2);
        }


        //Edit template details such Name, Description and statments and verify
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "EditTemplateDetails", "EditTemplate", "ManageTemplate", "EndToEnd", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CreateTemplateByTopic()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
                     
            //Going to desired path            
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Curriculum Topics");
            TopicDataMaintainance topicDataMaintainance = new TopicDataMaintainance();

            topicDataMaintainance = topicDataMaintainance.ClickCreateButton();
            // Generating Basice Details
            string topicName = "Selenium Test Topic " + topicDataMaintainance.GenerateRandomString(10);
            topicDataMaintainance.SetTopicName(topicName);

            topicDataMaintainance.SetTopicDescription(topicName + " Description");

            SeleniumHelper.ChooseSelectorOption(topicDataMaintainance.TopicYearDropdownInitiator, YearGroupSelected);
            SeleniumHelper.ChooseSelectorOption(topicDataMaintainance.TopicTermDropdownInitiator, TermSelected);

            //Saving the Topic
            topicDataMaintainance = topicDataMaintainance.ClickDialogOkButton();
            topicDataMaintainance.SaveTopicSuccess();
            //Select Statement   
            String selectedStatement = topicDataMaintainance.SelectStatement(5);

            //Allocate Statement
            topicDataMaintainance.AllocateStatmentByName(topicName);


            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");

            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = curriculummarksheetmaintainance.GenerateRandomString(10);
            string TemplateDescription = curriculummarksheetmaintainance.GenerateRandomString(20);
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription);

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectCurriculum();

            curriculummarksheetmaintainance = curriculummarksheetmaintainance.SelectTopic();


            //Select 4th Node in the tree control which will be first statement
            curriculummarksheetmaintainance.SelectTopicStatement(2);
            // Add Selected Columns
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.AddSelectedColumns();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.Save();
            // Edit Template Started
            WaitUntillAjaxRequestCompleted();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CancelTemplate();
            curriculummarksheetmaintainance = curriculummarksheetmaintainance.CreateMarksheet();
            EditMarksheetTemplate editTemplate = curriculummarksheetmaintainance.ClickModifyExistingButton();
            WaitUntillAjaxRequestCompleted();
            editTemplate.SearchTemplateByName(TemplateName);
            editTemplate.Search();
            editTemplate.SelectTemplateByName(TemplateName);
            editTemplate.OpenTemplate();


            string marksheetTemplateName = createMarksheet.getMarksheetTemplateName();
            Assert.AreEqual(TemplateName, marksheetTemplateName);

            string marksheetTemplateDescription = createMarksheet.getMarksheetTemplateDescription();
            Assert.AreEqual(TemplateDescription, marksheetTemplateDescription);
            createMarksheet.NextButtonClick();

            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Curriculum Topics");
            topicDataMaintainance = topicDataMaintainance.SelectTopicDropDownByName(topicName);
            topicDataMaintainance.ClickDeleteButton(topicName);
            topicDataMaintainance = topicDataMaintainance.ClickDeleteDialogOkButton();
            topicDataMaintainance.DeleteTopicSuccess();

        }

        #endregion

    }

}



