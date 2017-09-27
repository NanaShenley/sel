using System.Threading;
using Assessment.Components.PageObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.HomePages;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using Assessment.Components;
using Assessment.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using SharedComponents;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using SeSugar.Automation;
using Selene.Support.Attributes;


namespace Assessment.CNR.Other.Assessment.Screens.Tests
{
    [TestClass]
    public class Gradesets : BaseSeleniumComponents
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

        /// <summary>
        /// Stroy 8103 : 1. Verify Total GradeSet Count on click of search
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "GradesetSearch", "Assessment CNR", "GridFailed", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void GradesetSearch()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Create page object of marksheet Gradeset Search Panel
            GradesetSearchPanel gradesetsearchpanel = new GradesetSearchPanel();
            //Click on Search Button
            gradesetsearchpanel = gradesetsearchpanel.Search(false);
            //Get The Search ResultCount
            gradesetsearchpanel.GetSearchResultCount();
            //Compare the Actual Result with the Expected Result
            List<string> GradesetList = new List<string>();
            GradesetList = TestData.CreateDataList("Select Name From AssessmentGradeset Where AssessmentGradesetType Not IN (Select ID From AssessmentGradesetType Where Description ='Comment') AND TenantID=" + TestDefaults.Default.TenantId, "Name");
          //  Assert.AreEqual(GradesetList.Count.ToString(), gradesetsearchpanel.GetSearchResultCount());
            Assert.IsTrue(gradesetsearchpanel.GetSearchResultCount().Contains(GradesetList.Count.ToString()));
        }

        /// <summary>
        /// Stroy 8103 : 2. Search By Gradeset Name
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "SearchByGradesetName", "Assessment CNR", "GridFailed", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SearchByGradesetName()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Create page object of marksheet Gradeset Search Panel
            GradesetSearchPanel gradesetsearchpanel = new GradesetSearchPanel();
            //Enter Gradeset Name
            gradesetsearchpanel.SetGradeSetName("MIST");
            //Click on Search Button
            gradesetsearchpanel = gradesetsearchpanel.Search(false);
            //Get The Searched GradesetName List
            List<string> SearchResults = gradesetsearchpanel.GetGradeSetNameResult();
            //Compare the Actual Result with the Expected Result
            List<string> GradesetResults = TestData.CreateDataList("Select Name From AssessmentGradeset Where Name Like '%MIST%'", "Name");
            foreach (string eachelement in SearchResults)
            {
                Assert.IsTrue(GradesetResults.Contains(eachelement));
            }
        }

        /// <summary>
        /// Stroy 8103 : 3. Search By Gradeset Type
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "SearchByGradesetType", "Assessment CNR", "GridFailed", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SearchByGradesetType()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Create page object of marksheet Gradeset Search Panel
            GradesetSearchPanel gradesetsearchpanel = new GradesetSearchPanel();
            //Enter Gradeset Type
            gradesetsearchpanel.SetGradeSetType("Age");
            //Click on Search Button
            gradesetsearchpanel = gradesetsearchpanel.Search(false);
            //Get The Searched GradesetName List
            List<string> SearchResults = gradesetsearchpanel.GetGradeSetNameResult();
            //Compare the Actual Result with the Expected Result
            List<string> GradesetTypeResults = TestData.CreateDataList("Select Name From AssessmentGradeset Where AssessmentGradesetType IN (Select ID From AssessmentGradesetType Where Description = 'Age')", "Name");
            foreach (string eachelement in SearchResults)
            {
                Assert.IsTrue(GradesetTypeResults.Contains(eachelement));
            }
        }
        /// <summary>
        /// Stroy 8103 : 4. Verifying the selected grade details on the gradeset details data maintainance screen
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "VerifyGradeSetDetails", "Assessment CNR", "GridFailed", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyGradeSetDetails()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Create page object of marksheet Gradeset Search Panel
            GradesetSearchPanel gradesetsearchpanel = new GradesetSearchPanel();
            //Create page object of marksheet Gradeset Search Panel
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
            //Enter Gradeset Name
            gradesetsearchpanel.SetGradeSetName("MIST");
            //Enter Gradeset Type
            gradesetsearchpanel.SetGradeSetType("Grade");
            //Click on Search Button
            gradesetsearchpanel = gradesetsearchpanel.Search(false);
            //Get The Searched GradesetName List
            gradesetdatamaintenance = gradesetsearchpanel.SelectGradesetByName("MIST Grades");
            //List for storeing all the details of MIST Grades on the details screen
            List<string> GradesetDetails = new List<string>();
            //Adding each field on the Gradeset Details to the list
            GradesetDetails.Add(gradesetdatamaintenance.GetGradeSetName());
            //GradesetDetails.Add(gradesetdatamaintenance.GetGradeSetCode());
            GradesetDetails.Add(gradesetdatamaintenance.GetGradeSetType());
            gradesetdatamaintenance = gradesetdatamaintenance.ClickVersionLink();
            //GradesetDetails.Add(gradesetdatamaintenance.GetMinimumValue());
            //GradesetDetails.Add(gradesetdatamaintenance.GetMaximumValue());
            GradesetDetails.Add(gradesetdatamaintenance.GetStartDate());
            GradesetDetails.Add(gradesetdatamaintenance.GetEndDate());
            //Verifying all the gradeset details
            foreach (string eachelement in GradesetDetails)
            {
                Console.WriteLine(eachelement);
                Assert.IsTrue(TestData.GradeDetails.Contains(eachelement));
            }
        }

        /// <summary>
        /// Stroy 8105 : Hooking of the Assign gradeset in marksheet template properties
        /// Check the popover appears after click of Ok button in Assessment period through subject flow
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "AssignGradesetPopoverMessage", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AssignGradesetPopoverMessage()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            //Adding Marksheet Template Name
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            string TemplateName = marksheetBuilder.RandomString(10);
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);

            AddAssessments addassessment = marksheetBuilder.NavigateAssessments();

            //Adding Subject to the Marksheet Template
            AddSubjects addSubjects = addassessment.NavigateAssessmentsviaSubject();

            //Selecting a Subject
            addSubjects.EnterSubjectSearchCriteria("Language and Literacy");
            addSubjects = addSubjects.ClickSubjectSearchButton();
            addSubjects.SelectSubjectResult(1);
            AddModeMethodPurpose addmodemethodpurpose = addSubjects.SubjectNextButton();

            //Selecting a Mode Method and Purpose for that Subject
            addmodemethodpurpose.purposeAssessmentPeriodSelection(1);
            addmodemethodpurpose.modeAssessmentPeriodSelection(1);
            addmodemethodpurpose.methodAssessmentPeriodSelection(1);

            //Selecting an Assessment Period
            AddAssessmentPeriod addassessmentperiod = addmodemethodpurpose.modeMethodPurposeNextButton();
            addassessmentperiod.subjectAssessmentPeriodSelection(1);
            marksheetBuilder = addassessmentperiod.ClickSubjectAssessmentPeriodDone();

            MarksheetTemplateProperties marksheetTemplateProperties = new MarksheetTemplateProperties();
            marksheetTemplateProperties.CheckAssignGradepopover();
        }

        /// <summary>
        /// Stroy 8105 : Hooking of the Assign gradeset in marksheet template properties
        /// Check the popover appears after click of Ok button in Assessment period through subject flow
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "AssignGradesetPopoverMessagethroughSave", "Assessment CNR", "Grid No Run" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AssignGradesetPopoverMessagethroughSave()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addassessment = marksheetBuilder.NavigateAssessments();

            //Adding Subject to the Marksheet Template
            AddSubjects addSubjects = addassessment.NavigateAssessmentsviaSubject();

            //Selecting a Subject
            addSubjects.EnterSubjectSearchCriteria("Language and Literacy");
            addSubjects = addSubjects.ClickSubjectSearchButton();
            addSubjects.SelectSubjectResult(1);
            AddModeMethodPurpose addmodemethodpurpose = addSubjects.SubjectNextButton();

            //Selecting a Mode Method and Purpose for that Subject
            addmodemethodpurpose.purposeAssessmentPeriodSelection(1);
            addmodemethodpurpose.modeAssessmentPeriodSelection(1);
            addmodemethodpurpose.methodAssessmentPeriodSelection(1);

            //Selecting an Assessment Period
            AddAssessmentPeriod addassessmentperiod = addmodemethodpurpose.modeMethodPurposeNextButton();
            addassessmentperiod.subjectAssessmentPeriodSelection(1);
            marksheetBuilder = addassessmentperiod.ClickSubjectAssessmentPeriodDone();

            //Check Assign gradeset popover is displayed and the close it
            MarksheetTemplateProperties marksheetTemplateProperties = new MarksheetTemplateProperties();
            marksheetTemplateProperties.CheckAssignGradepopover();
            marksheetTemplateProperties.CloseAssignGradepopover();

            //Adding Marksheet Template Name
            marksheetTemplateProperties.OpenDetailsTab();
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            string TemplateName = marksheetBuilder.RandomString(10);
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);
            SaveMarksheetTemplate savemarksheettemplate = new SaveMarksheetTemplate();

            savemarksheettemplate.ClickSaveButton();
            //Check Assign gradeset popover message is displayed on click on Save
            marksheetTemplateProperties.CheckAssignGradepopover();
        }

        /// <summary>
        /// Stroy 3254 : MCE - Clone  Marksheet Template (New from existing option)
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "GradesetBulkAllocate", "Assessment CNR", "GridFailed", "Gradeset BulkAllocate" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void GradesetBulkAllocate()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            //Adding Marksheet Template Name
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            string TemplateName = marksheetBuilder.RandomString(10);
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);

            //Adding Subject to the Marksheet Template
            AddAssessments addassessment = marksheetBuilder.NavigateAssessments();
            AddSubjects addSubjects = addassessment.NavigateAssessmentsviaSubject();

            //Selecting a Subject
            int noofselectedsubject = 2;
            addSubjects.SelectSubjectResult(noofselectedsubject);
            AddModeMethodPurpose addmodemethodpurpose = addSubjects.SubjectNextButton();

            //Scenario 1 : One Column Definition

            int modeSelected = 1, methodSected = 1, purposeSelected = 1;

            //Selecting a Mode Method and Purpose for that Subject
            addmodemethodpurpose.purposeAssessmentPeriodSelection(purposeSelected);
            addmodemethodpurpose.modeAssessmentPeriodSelection(modeSelected);
            addmodemethodpurpose.methodAssessmentPeriodSelection(methodSected);

            AddAssessmentPeriod addassessmentperiod = addmodemethodpurpose.modeMethodPurposeNextButton();

            addassessmentperiod.subjectAssessmentPeriodSelection(1);
            marksheetBuilder = addassessmentperiod.ClickSubjectAssessmentPeriodDone();

            //Assigning a Gradeset to a Subject
            MarksheetTemplateProperties marksheettemplateproperties = new MarksheetTemplateProperties();

            marksheettemplateproperties.SelectGridRows();

            BulkAssignGradeset bulkassigngradeset = marksheettemplateproperties.OpenBulkGradesetAllocationMenu();
            GradesetSearchPanel gradesetsearchpanel = bulkassigngradeset.ClickAddGradeSetButton();
            gradesetsearchpanel = gradesetsearchpanel.Search(true);
            string selectedgradesetname = gradesetsearchpanel.GetFirstGradesetName();

            GradesetDataMaintenance gradesetdatamaintenance = gradesetsearchpanel.SelectGradeset();

            marksheettemplateproperties = gradesetdatamaintenance.ClickOkButton();

            bulkassigngradeset.ApplyGradeset();
            List<string> AssessmentGradesetNameList = marksheettemplateproperties.GetAllGradesetNames();
            foreach (string eachvalue in AssessmentGradesetNameList)
            {
                Assert.AreEqual(selectedgradesetname, eachvalue);
            }
            //Apply the gradeset of type Comment
            marksheettemplateproperties.SelectGridRows();
            bulkassigngradeset = marksheettemplateproperties.OpenBulkGradesetAllocationMenu();
            marksheettemplateproperties = bulkassigngradeset.SelectComment();
            //Apply the overwrite functionality 
            marksheettemplateproperties = marksheettemplateproperties.SelectMultipleRows(4);
            marksheettemplateproperties = marksheettemplateproperties.SelectMultipleRows(2);
            bulkassigngradeset = marksheettemplateproperties.OpenBulkGradesetAllocationMenu();
            gradesetsearchpanel = bulkassigngradeset.ClickAddGradeSetButton();
            gradesetsearchpanel = gradesetsearchpanel.Search(true);

            gradesetdatamaintenance = gradesetsearchpanel.SelectGradeset(2);
            gradesetdatamaintenance.ClickOkButton();
            bulkassigngradeset.SelectOverwrite();
            bulkassigngradeset.ApplyGradeset();

        }

        ///// <summary>
        ///// Stroy 3711 : Create  and delete operations for Assessment Gradeset
        ///// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "CreatenDeleteGradesetType", "Assessment CNR", "GridFailed", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CreatenDeleteGradesetType()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Gradeset Data Maintainance Screen Page Object
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
            gradesetdatamaintenance = gradesetdatamaintenance.ClickCreateButton();

            string gradeSetName = gradesetdatamaintenance.GenerateRandomString(20);
            string gradeSetCode = gradesetdatamaintenance.GenerateRandomString(2);

            //Selecting the gradeset type
            gradesetdatamaintenance = gradesetdatamaintenance.SelectGradesetOption("Grade");

            // Setting up the Gradeset Name & Description
            gradesetdatamaintenance.SetGradeSetName(gradeSetName);
            gradesetdatamaintenance.SetGradeSetDescription(gradeSetName + " Description");

            //Setting up the Grades
            string instanceID = gradesetdatamaintenance.FindDefaultInstanceFieldPrefix();
            gradesetdatamaintenance.SetGradeRow(0, instanceID, gradeSetCode, gradeSetCode + " Description", "20");

            ////Saving the Gradeset
            gradesetdatamaintenance = gradesetdatamaintenance.ClickSaveButton();
            WaitUntillAjaxRequestCompleted();
            gradesetdatamaintenance.SaveMarksheetAssertionSuccess();

            //// Deleting the Gradeset.
            gradesetdatamaintenance = gradesetdatamaintenance.DeleteButtonClick();
            gradesetdatamaintenance = gradesetdatamaintenance.ContinueButtonClick();
            gradesetdatamaintenance.DeleteGrateSetAssertionSuccess();
        }

        /// <summary>
        /// Validate the options that appear as grade type options.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "CheckGradesetOptions", "Assessment CNR", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckGradesetOptions()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Gradeset Data Maintainance Screen Page Object
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
            gradesetdatamaintenance = gradesetdatamaintenance.ClickCreateButton();

            bool alltypesPresent = gradesetdatamaintenance.ValidateAllExpectedTypesArePresent();

            Assert.IsTrue(alltypesPresent, "All expected types are not present in dropdown selector");
        }

        /// <summary>
        /// Validates that the Default gradeset version is created when a grade type is selected.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "DefaultGradesetVersionCreated", "Assessment CNR", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DefaultGradesetVersionCreated()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
            gradesetdatamaintenance = gradesetdatamaintenance.ClickCreateButton();

            string gradeSetName = gradesetdatamaintenance.GenerateRandomString(20);
            string gradeSetCode = gradesetdatamaintenance.GenerateRandomString(2);

            //Selecting the gradeset type
            gradesetdatamaintenance = gradesetdatamaintenance.SelectGradesetOption("Grade");

            // Setting up the Gradeset Name & Description
            gradesetdatamaintenance.SetGradeSetName(gradeSetName);
            gradesetdatamaintenance.SetGradeSetDescription(gradeSetName + " Description");

            //Setting up the Grades
            string instancePrefix = gradesetdatamaintenance.FindDefaultInstanceFieldPrefix();
            string startDate = gradesetdatamaintenance.GetStartDate();
            Assert.IsNotNull(startDate, "Start Date cannot be null for gradeset version created.");
        }

        /// <summary>
        /// Validates that the Cancel button click closes the data maintainence.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "CancelButton", "Assessment CNR", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CancelButton()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
            gradesetdatamaintenance = gradesetdatamaintenance.ClickCreateButton();
            //Selecting the gradeset type
            gradesetdatamaintenance = gradesetdatamaintenance.SelectGradesetOption("Grade");
            //Click on Cancel
            gradesetdatamaintenance = gradesetdatamaintenance.ClickCancelButton();
            Assert.IsNull(gradesetdatamaintenance);
        }

        /// <summary>
        /// Validates that the grade values can not be saved without code or values.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "ValidateGradeValues", "Assessment CNR", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidateGradeValues()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Gradeset Data Maintainance Screen Page Object
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
            gradesetdatamaintenance = gradesetdatamaintenance.ClickCreateButton();

            string gradeSetName = gradesetdatamaintenance.GenerateRandomString(20);
            string gradeSetCode = string.Empty;

            //Selecting the gradeset type
            gradesetdatamaintenance = gradesetdatamaintenance.SelectGradesetOption("Grade");

            // Setting up the Gradeset Name & Description
            gradesetdatamaintenance.SetGradeSetName(gradeSetName);
            gradesetdatamaintenance.SetGradeSetDescription(gradeSetName + " Description");

            //Setting up the Grades
            string instancePrefix = gradesetdatamaintenance.FindDefaultInstanceFieldPrefix();
            gradesetdatamaintenance.SetGradeRow(0, instancePrefix, gradeSetCode, gradeSetCode + " Description", "20");

            ////Saving the Gradeset
            gradesetdatamaintenance = gradesetdatamaintenance.ClickSaveButton();
            WaitUntillAjaxRequestCompleted();
            bool validationMessageExists = gradesetdatamaintenance.ValidationMessageAssertion();
            Assert.IsTrue(validationMessageExists, "Warning is expected");
        }

        /// <summary>
        /// Confirm that a colour style can be associated with a grade value.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "SaveColourWithGrade", "Assessment CNR", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SaveColourWithGrade()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator, true);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Gradeset Data Maintainance Screen Page Object
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
          //  gradesetdatamaintenance = gradesetdatamaintenance.ClickToHideSearchCriateriaButton();
          //  Thread.Sleep(2000);
            gradesetdatamaintenance = gradesetdatamaintenance.ClickCreateButton();

            string gradeSetName = gradesetdatamaintenance.GenerateRandomString(20);
            string gradeSetCode = gradesetdatamaintenance.GenerateRandomString(2);

            //Selecting the gradeset type
            gradesetdatamaintenance = gradesetdatamaintenance.SelectGradesetOption("Grade");

            // Setting up the Gradeset Name & Description
            gradesetdatamaintenance.SetGradeSetName(gradeSetName);
            gradesetdatamaintenance.SetGradeSetDescription(gradeSetName + " Description");

            //Setting up the Grades
            string instancePrefix = gradesetdatamaintenance.FindDefaultInstanceFieldPrefix();
            gradesetdatamaintenance.SetGradeRow(0, instancePrefix, gradeSetCode, gradeSetCode + " Description", "20", true);

            ////Saving the Gradeset
            gradesetdatamaintenance = gradesetdatamaintenance.ClickSaveButton();
            WaitUntillAjaxRequestCompleted();
            gradesetdatamaintenance.SaveMarksheetAssertionSuccess();

            //// Deleting the Gradeset.
            gradesetdatamaintenance = gradesetdatamaintenance.DeleteButtonClick();
            gradesetdatamaintenance = gradesetdatamaintenance.ContinueButtonClick();
            gradesetdatamaintenance.DeleteGrateSetAssertionSuccess();
        }

        /// <summary>
        /// Confirm that a editing a gradset and choosing to update existing version does not create a new instance.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "EditGradesetUpdateExistingVersion", "Assessment CNR", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EditGradesetUpdateExistingVersion()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Gradeset Data Maintainance Screen Page Object
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
            gradesetdatamaintenance = gradesetdatamaintenance.ClickCreateButton();

            string gradeSetName = gradesetdatamaintenance.GenerateRandomString(20);
            string gradeSetCode = gradesetdatamaintenance.GenerateRandomString(2);

            //Selecting the gradeset type
            gradesetdatamaintenance = gradesetdatamaintenance.SelectGradesetOption("Grade");

            // Setting up the Gradeset Name & Description
            gradesetdatamaintenance.SetGradeSetName(gradeSetName);
            gradesetdatamaintenance.SetGradeSetDescription(gradeSetName + " Description");

            //Setting up the Grades
            string instancePrefix = gradesetdatamaintenance.FindDefaultInstanceFieldPrefix();
            gradesetdatamaintenance.SetGradeRow(0, instancePrefix, gradeSetCode, gradeSetCode + " Description", "20", true);

            ////Saving the Gradeset
            gradesetdatamaintenance = gradesetdatamaintenance.ClickSaveButton();
            WaitUntillAjaxRequestCompleted();
            gradesetdatamaintenance.SaveMarksheetAssertionSuccess();

            //Add another Gradevalue.
            gradesetdatamaintenance = gradesetdatamaintenance.ClickAddGradeLink(instancePrefix);
            //Thread.Sleep(1000);
            gradesetdatamaintenance.SetGradeRow(1, instancePrefix, gradeSetCode + "_NEW", gradeSetCode + " Description 2", "5");
            ////Saving the Gradeset
            gradesetdatamaintenance = gradesetdatamaintenance.ClickSaveButton();
            // button click to update existing instance.
            gradesetdatamaintenance = gradesetdatamaintenance.ConfirmUpdateToExistingInstance();

            WaitUntillAjaxRequestCompleted();
            gradesetdatamaintenance.SaveMarksheetAssertionSuccess();

            //// Deleting the Gradeset.
            gradesetdatamaintenance = gradesetdatamaintenance.DeleteButtonClick();
            gradesetdatamaintenance = gradesetdatamaintenance.ContinueButtonClick();
            gradesetdatamaintenance.DeleteGrateSetAssertionSuccess();

        }

        /// <summary>
        /// Script to validate that capita supplied grade can be edited by adding a grade value and associating with parent grade.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "AddGradesAcrossBaseDataSuppliedGradeset", "Assessment CNR", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AddGradesAcrossBaseDataSuppliedGradeset()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Create page object of marksheet Gradeset Search Panel
            GradesetSearchPanel gradesetsearchpanel = new GradesetSearchPanel();
            //Create page object of marksheet Gradeset Search Panel
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
            //Enter Gradeset Name
            gradesetsearchpanel.SetGradeSetName("MIST");
            //Enter Gradeset Type
            gradesetsearchpanel.SetGradeSetType("Grade");
            //Click on Search Button
            gradesetsearchpanel = gradesetsearchpanel.Search(false);
            //Get The Searched GradesetName List
            gradesetdatamaintenance = gradesetsearchpanel.SelectGradesetByName("MIST Grades");
            //Setting up the Grades
            string instancePrefix = gradesetdatamaintenance.FindDefaultInstanceFieldPrefix();
            //Add another Gradevalue.
            gradesetdatamaintenance = gradesetdatamaintenance.ClickAddGradeLink(instancePrefix);
            string rowCount = gradesetdatamaintenance.getGradesetGradesRowCount();
            int count = Convert.ToInt16(rowCount);
            // Add a new grade and map it to an existing supplied grade.
            gradesetdatamaintenance.SetGradeRow(count-1, instancePrefix, "Z", "F- Failed", string.Empty,false,"Average");
            //Saving the Gradeset
            gradesetdatamaintenance = gradesetdatamaintenance.ClickSaveButton();
            WaitUntillAjaxRequestCompleted();
            gradesetdatamaintenance.SaveMarksheetAssertionSuccess();
            rowCount = gradesetdatamaintenance.getGradesetGradesRowCount();
            // Assert that the new grade has been added to the grade collection.
            Assert.IsTrue(rowCount == "5");   
            //delete the newly added grade
            gradesetdatamaintenance.ClickDeleteRowButton(count-1);
            gradesetdatamaintenance = gradesetdatamaintenance.ClickSaveButton();
            WaitUntillAjaxRequestCompleted();
            gradesetdatamaintenance.SaveMarksheetAssertionSuccess();
            rowCount = gradesetdatamaintenance.getGradesetGradesRowCount();
            //Assert that the number of grades after deletion match the original grade count.
            Assert.IsTrue(rowCount == "4"); 
        }

        /// <summary>
        /// Confirm that a editing a gradset and choosing to create new version will create a new instance.
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "EditGradesetCreateNewVersion", "Assessment CNR", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EditGradesetCreateNewVersion()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Gradeset Data Maintainance Screen Page Object
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
            gradesetdatamaintenance = gradesetdatamaintenance.ClickCreateButton();

            string gradeSetName = gradesetdatamaintenance.GenerateRandomString(20);
            string gradeSetCode = gradesetdatamaintenance.GenerateRandomString(2);

            //Selecting the gradeset type
            gradesetdatamaintenance = gradesetdatamaintenance.SelectGradesetOption("Grade");

            // Setting up the Gradeset Name & Description
            gradesetdatamaintenance.SetGradeSetName(gradeSetName);
            gradesetdatamaintenance.SetGradeSetDescription(gradeSetName + " Description");

            //Setting up the Grades
            string instancePrefix = gradesetdatamaintenance.FindDefaultInstanceFieldPrefix();
            gradesetdatamaintenance.SetGradeRow(0, instancePrefix, gradeSetCode, gradeSetCode + " Description", "20", true);

            ////Saving the Gradeset
            gradesetdatamaintenance = gradesetdatamaintenance.ClickSaveButton();
            WaitUntillAjaxRequestCompleted();
            gradesetdatamaintenance.SaveMarksheetAssertionSuccess();

            //Add another Gradevalue.
            gradesetdatamaintenance = gradesetdatamaintenance.ClickAddGradeLink(instancePrefix);
            //Thread.Sleep(1000);
            gradesetdatamaintenance.SetGradeRow(1, instancePrefix, gradeSetCode + "_NEW", gradeSetCode + " Description 2", "5");
            ////Saving the Gradeset
            gradesetdatamaintenance = gradesetdatamaintenance.ClickSaveButton();
            gradesetdatamaintenance = gradesetdatamaintenance.SelectNewInstance();
            string newVersionStartDate = DateTime.Today.AddDays(2).ToShortDateString();
            //set start date
            gradesetdatamaintenance.SetNewVersionStartdate(newVersionStartDate);
            // button click to create new instance.
            gradesetdatamaintenance = gradesetdatamaintenance.ConfirmUpdateToExistingInstance();

            WaitUntillAjaxRequestCompleted();
            gradesetdatamaintenance.SaveMarksheetAssertionSuccess();
                        
            string rowCount = gradesetdatamaintenance.GetGradesetInstanceRowCount();
            Assert.IsTrue(rowCount == "2");          

        }

        #region Marks Result Type
        /// <summary>        
        /// Story - 31092 - Marks-Integer/Decimal to Marks Result type changes
        /// </summary>  
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "CreatenDeleteMarksResultType", "Assessment CNR", "t1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CreatenDeleteMarksResultType()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Result Types");
            //Gradeset Data Maintainance Screen Page Object
            GradesetDataMaintenance gradesetdatamaintenance = new GradesetDataMaintenance();
            gradesetdatamaintenance = gradesetdatamaintenance.ClickCreateButton();

            string gradeSetName = gradesetdatamaintenance.GenerateRandomString(20);
            string gradeSetCode = gradesetdatamaintenance.GenerateRandomString(2);

            //Selecting the result type
            gradesetdatamaintenance = gradesetdatamaintenance.SelectGradesetOption("Marks");

            // Setting up the result type Name, Description, Minimum & Maximum values
            gradesetdatamaintenance.SetGradeSetName(gradeSetName);
            gradesetdatamaintenance.SetGradeSetDescription(gradeSetName + " Description");
            gradesetdatamaintenance.SetMinimumValue("1");
            gradesetdatamaintenance.SetMaximumValue("100");

            ////Saving the result type
            gradesetdatamaintenance = gradesetdatamaintenance.ClickSaveButton();
            WaitUntillAjaxRequestCompleted();
            gradesetdatamaintenance.SaveMarksheetAssertionSuccess();

            //// Deleting the result type.
            gradesetdatamaintenance = gradesetdatamaintenance.DeleteButtonClick();
            gradesetdatamaintenance = gradesetdatamaintenance.ContinueButtonClick();
            gradesetdatamaintenance.DeleteGrateSetAssertionSuccess();
        }

        #endregion

    }
}
