using System.Threading;
using Assessment.Components.PageObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TestSettings;
using WebDriverRunner.webdriver;
using Assessment.Components;
using Assessment.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using SeSugar.Automation;
using Selene.Support.Attributes;


namespace Assessment.CNR.Tests
{
    [TestClass]
    public class AssessmentCreateMarksheet : BaseSeleniumComponents
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
        /// Validate the different options provided as part of create marksheet.
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "FindTypeOfMarksheets", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void FindTypeOfMarksheets()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            //Console.WriteLine(WebContext.WebDriver.FindElement(MarksheetConstants.NewFromExisting).Text);
            Console.WriteLine(WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetWithLevels).Text);
            //Assert.AreEqual(MarksheetConstants.NewFromExistingLabel, WebContext.WebDriver.FindElement(MarksheetConstants.NewFromExisting).Text);
            Assert.AreEqual(MarksheetConstants.MarksheetWithLevelsLabel, WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetWithLevels).Text);

            //Not required as of now
            //Assert.AreEqual(MarksheetConstants.ProgrammeOfStudyTrackingLabel, WebContext.WebDriver.FindElement(MarksheetConstants.ProgrammeOfStudyTracking).GetAttribute("title"));
            //Assert.AreEqual(MarksheetConstants.TrackingGridLabel, WebContext.WebDriver.FindElement(MarksheetConstants.TrackingGrid).GetAttribute("title"));
        }


        /// <summary>
        /// Validate create marksheet with levels page is opened display Marksheet Builder slider control.
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "CreateMarksheetWithLevelsNew", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void CreateMarksheetWithLevelsNew()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            Assert.AreEqual(MarksheetConstants.MarksheetBuilderTitleLabel, WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetBuilderTitle).Text);

        }

        /// <summary>
        /// Story - 3184 - Verify the search for Assessment Period - contains 2 test cases one with valid search and other with invalid one.
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "SearchAPForCreateMarksheet", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchAPForCreateMarksheet()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(2);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();

            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.PeriodResult));

            //Search for a specific Aspect and view the result
            var ResultText = "";
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AspectAssessmentPeriodElementSelection));

            //Search for aspect that is not valid
            addAssessmentPeriod.EnterSearchName(":");
            addAssessmentPeriod.ClickSearch();

            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.APSearchResult, "No Matches"));
            ReadOnlyCollection<IWebElement> Noresultsearch = WebContext.WebDriver.FindElements(MarksheetConstants.APSearchResult);

            foreach (IWebElement eachresult in Noresultsearch)
            {
                if (eachresult.Text == "No Matches")
                    ResultText = eachresult.Text;
            }

            Assert.AreEqual(ResultText, "No Matches");

            ResultText = "";
            addAssessmentPeriod.EnterSearchName("Year 1 Annual");
            addAssessmentPeriod.ClickSearch();

            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AspectAssessmentPeriodElementSelection));
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.APSearchResult, "1 Match"));

            ReadOnlyCollection<IWebElement> codeElems = WebContext.WebDriver.FindElements(MarksheetConstants.APSearchResult);

            foreach (IWebElement codeElem in codeElems)
            {
                if (codeElem.Text == "1 Match")
                    ResultText = codeElem.Text;
            }

            Assert.AreEqual(ResultText, "1 Match");
        }

        /// <summary>
        /// Story 3184 - Add Assessment Periods in the create marksheet with levels page using Marksheet Builder
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "AddAPForNewMarksheetwithLevels", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AddAPForNewMarksheetwithLevels()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            //Selct Aspect
            addAspects.SelectNReturnSelectedAssessments(2);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();

            //Select Assessment period
            addAssessmentPeriod.AspectAssessmentPeriodSelection(2);
            marksheetBuilder = addAssessmentPeriod.ClickAspectAssessmentPeriodDone();
            marksheetBuilder.IsMarksheetPreviewColumnsPresent();
        }

        /// <summary>
        /// Story - 3185 - Verify the search for Aspects - contains 2 test cases one with valid search and other with invalid one.
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet Builder", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchAspectForCreateMarksheet()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.EnterAssessmentName(":");
            addAspects = addAspects.AspectSearch();

            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.AspectSearchResult, "No Matches"));
            ReadOnlyCollection<IWebElement> Noresultsearch = WebContext.WebDriver.FindElements(MarksheetConstants.AspectSearchResult);

            var ResultText = "";

            foreach (IWebElement eachresult in Noresultsearch)
            {
                if (eachresult.Text == "No Matches")
                {
                    waiter.Until(ExpectedConditions.TextToBePresentInElement(eachresult, "No Matches"));
                    ResultText = eachresult.Text;
                }
            }

            Assert.AreEqual(ResultText, "No Matches");
            //Search for a specific Aspect and view the result
            ResultText = "";
            addAspects.EnterAssessmentName("");
            addAspects = addAspects.AspectSearch();

            ReadOnlyCollection<IWebElement> codeElems = WebContext.WebDriver.FindElements(MarksheetConstants.AspectElementSelection);
            string itemSelection = codeElems[1].Text;
            addAspects.EnterAssessmentName(itemSelection);
            addAspects = addAspects.AspectSearch();

            //  WebContext.WebDriver.FindElement(MarksheetConstants.AspectCloseButton).Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.AspectElementSelection, itemSelection));
            // Thread.Sleep(1000);
            ReadOnlyCollection<IWebElement> newcodeElems = WebContext.WebDriver.FindElements(MarksheetConstants.AspectElementSelection);

            foreach (IWebElement codeElem in newcodeElems)
            {
                if (codeElem.Text == itemSelection)
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(codeElem));
                    ResultText = codeElem.Text;
                }
            }
            Assert.AreEqual(ResultText, itemSelection);
        }



        /// <summary>
        /// Story - 7561 - Back button on slider control. Additional column Back button
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "AdditioanlColumnCloseTest", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AdditionalColumnBackButtonCreateMarksheet()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            //Create page object of Additional Column Panel
            AdditionalColumn additionalcolumn = new AdditionalColumn();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();

            additionalcolumn.SelectNoOfAdditionalColumn(2);

            // Check columns displayed in preview

            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            marksheetBuilder = additionalcolumn.ClickBackButton();

            // Check columns removed in preview
            marksheetBuilder.IsMarksheetPreviewColumnsNotPresent();
            marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();
            additionalcolumn.AdditionalColumnsAreUnSelected();
        }


        /// <summary>
        /// Story - 7460 - Back button on slider control. Additional column Back button
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "AdditionalColumnDeleteTest", "Assessment CNR", "Done" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AdditionalColumnDeleteButton()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            //Create page object of Additional Column Panel

            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //marksheetBuilder.NavigateAssessments();
            AdditionalColumn additionalcolumn = marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();

            additionalcolumn.SelectNoOfAdditionalColumn(1);

            List<string> additionalColumnSelected = additionalcolumn.GetSelectedAdditionalColumnList();

            // Check columns displayed in preview
            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            marksheetBuilder = additionalcolumn.ClickDoneButton();

            // Check columns are retained in preview
            marksheetBuilder.IsMarksheetPreviewColumnsPresent();
            Thread.Sleep(2000);
            marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();
            marksheetBuilder.clickPropertiesTab();
            marksheetBuilder.ColumnPropertiesCollapseButton();
            marksheetBuilder.ClickDeleteButtonInPropertiesTab();

            List<string> checkSelected = additionalcolumn.GetSelectedAdditionalColumnList();

            //Assert Logic
            foreach (String eachitem in checkSelected)
            {
                Assert.IsFalse(additionalColumnSelected.Contains(eachitem));
            }
        }

        /// <summary>
        /// Story - 7561 - Back button on slider control for subject. 
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "SubjectBackButton", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SubjectBackButtonCreateMarksheet()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();


            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            addSubjects.SelectSubjectResult(2);

            // Check columns displayed in preview

            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            addAssessments = addSubjects.SubjectBackButton();

            // Check columns removed in preview
            marksheetBuilder.IsMarksheetPreviewColumnsNotPresent();
            //Navigate to Assessment screen Aspects are not selcted any more
            addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            addSubjects.checkSubjectIsNotSelcted();
        }

        /// <summary>
        /// Story - 7561 - Back button on slider control for Mode, method and properties. 
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "modeMethodPurposeBackButton", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void ModeBackButtonCreateMarksheet()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            //Select Subject along with Mode, Method and Purpose
            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();
            addSubjects.SelectSubjectResult(2);
            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();

            addModeMethodPurpose.purposeAssessmentPeriodSelection(2);
            addModeMethodPurpose.modeAssessmentPeriodSelection(
                2);
            addModeMethodPurpose.methodAssessmentPeriodSelection(2);


            // Check columns displayed in preview

            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            addSubjects = addModeMethodPurpose.modeMethodPurposeBackButton();
            //     WebContext.WebDriver.FindElement(MarksheetConstants.ModeBackButton).Click();

            // Check columns removed in preview
            marksheetBuilder.IsMarksheetPreviewColumnsNotPresent();

            //Navigate to Assessment screen Aspects are not selcted any more
            //  waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SubjectResult));

            addModeMethodPurpose = addSubjects.SubjectNextButton();

            //Check Mode, Method and Purpose are not selected
            addModeMethodPurpose.checkPurposeIsNotSelcted();
            addModeMethodPurpose.checkModeIsNotSelcted();
            addModeMethodPurpose.checkMethodIsNotSelcted();

        }

        /// <summary>
        /// Story - 7561 - Back button on slider control for Subject Assessment Period. 
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "SubjectAssessmentPeriodBackButtonCreateMarksheet", "Assessment CNR", "GridFailed", "PeriodBackButton" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SubjectAssessmentPeriodBackButtonCreateMarksheet()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            addSubjects.SelectSubjectResult(2);
            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();

            addModeMethodPurpose.purposeAssessmentPeriodSelection(2);
            addModeMethodPurpose.modeAssessmentPeriodSelection(2);
            addModeMethodPurpose.methodAssessmentPeriodSelection(2);


            // Check columns displayed in preview
            marksheetBuilder.IsMarksheetPreviewColumnsPresent();
            AddAssessmentPeriod addAssessmentPeriod = addModeMethodPurpose.modeMethodPurposeNextButton();

            addAssessmentPeriod.subjectAssessmentPeriodSelection(2);

            marksheetBuilder.IsMarksheetPreviewColumnsPresent();
            addModeMethodPurpose = addAssessmentPeriod.ClickSubjectAssessmentPeriodBack();

            addAssessmentPeriod = addModeMethodPurpose.modeMethodPurposeNextButton();
            //Navigate to Assessment period screen to check Assessment period through subject are not selected any more

            ReadOnlyCollection<IWebElement> SubjectAPElementSelected = addAssessmentPeriod.SubjectAssessmentPeriodList();
            String checkAPSelected = addAssessmentPeriod.checkAssessmentPeriodIsNotSelcted(SubjectAPElementSelected);

            Assert.AreEqual(checkAPSelected, "false");
        }


        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "BackButton", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AspectBackButtonCreateMarksheet()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            //   marksheetBuilder.SearchByAspect();
            addAspects.SelectNReturnSelectedAssessments(2);

            // Check columns displayed in preview

            marksheetBuilder.IsMarksheetPreviewColumnsPresent();
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AspectBackButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.AspectBackButton).Click();

            //Navigate to Assessment screen Aspects are not selcted any more

            AddAspects addAspects1 = addAssessments.NavigateAssessmentsviaAssessment();
            waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.AspectElementSelection));
            addAspects1.checkAspectIsNotSelcted();
            //CheckItemsIsNotSelcted(MarksheetConstants.AspectElementSelection);

        }

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "AspectAPBackButtonCreateMarksheet", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AspectAPBackButtonCreateMarksheet()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(2);

            // Check columns displayed in preview
            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();
            addAssessmentPeriod.AspectAssessmentPeriodSelection(2);
            addAspects = addAssessmentPeriod.ClickAspectAssessmentPeriodBack();

            addAssessmentPeriod = addAspects.AspectNextButton();
            ReadOnlyCollection<IWebElement> AspectAPElementSelected = addAssessmentPeriod.AspectAssessmentPeriodList();
            String checkAPSelected = addAssessmentPeriod.checkAssessmentPeriodIsNotSelcted(AspectAPElementSelected);
            Assert.AreEqual(checkAPSelected, "false");

        }


        /// <summary>
        /// Story - 7561 -  MCN - UI - Back button on the slider control. Groups Back button
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 8000, Groups = new[] { "Groups", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void GroupsBackButtonCreateMarksheet()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddGroups addgroups = marksheetBuilder.NavigateGroups();
            //Select an year group
            addgroups.SelectNoOfYearGroups(1);
            //Select a Class
            addgroups.SelectNoOfClasses(1);

            //checks if the YearGroups and Classes are Selected
            Assert.AreEqual(true, marksheetBuilder.ChecksIfYearGroupsSelected());
            Assert.AreEqual(true, marksheetBuilder.ChecksIfClassesSelected());

            List<string> selectedGroups = addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);
            selectedGroups.AddRange(addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectClasses));

            MarksheetTemplateDetails mrkTemplateDetails = new MarksheetTemplateDetails();
            List<string> marksheetnames = mrkTemplateDetails.GetExpectedMarksheetNameList(mrkTemplateDetails.GetMarksheetTemplateName(), selectedGroups);

            foreach (string name in mrkTemplateDetails.GetMarksheetName())
            {
                Assert.IsTrue(marksheetnames.Contains(name));
            }

            //clicks on the back button
            addgroups.ClickBackButton();

            //goes back to Groups slider
            marksheetBuilder.NavigateGroups();

            bool isYearGroupSelected = marksheetBuilder.ChecksIfYearGroupsSelected();
            Assert.AreEqual(false, isYearGroupSelected);

            bool isClassesSelected = marksheetBuilder.ChecksIfClassesSelected();
            Assert.AreEqual(false, isClassesSelected);

            marksheetnames.Clear();
            marksheetnames.Add("No Marksheet Name Found");
            List<string> marksheetList = mrkTemplateDetails.GetMarksheetName();
            foreach (string name in marksheetList)
            {
                Assert.IsTrue(marksheetnames.Contains(name));
                Assert.AreEqual(marksheetList.Count, 1);
            }

        }

        /// <summary>
        /// Story - 8208 - Back and Close button on slider control after click of Done button.
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet Builder", "Assessment CNR", "Done" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AspectDoneButton()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();
            //Select Aspect
            addAspects.SelectNReturnSelectedAssessments(2);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();

            //Select Assessment period
            addAssessmentPeriod.AspectAssessmentPeriodSelection(2);
            //Get the selected Asessment Period in a list
            List<string> selectedAssessmentperiod = addAssessmentPeriod.getAspectAssessmentPeriodSelectedItems();

            // Check columns displayed in preview

            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            marksheetBuilder = addAssessmentPeriod.ClickAspectAssessmentPeriodDone();

            // Check columns removed in preview
            marksheetBuilder.IsMarksheetPreviewColumnsNotPresent();

            AddAssessments addAssessments1 = marksheetBuilder.NavigateAssessments();

            AddAspects addAspects1 = addAssessments1.NavigateAssessmentsviaAssessment();
            String aspectSelction = addAspects.checkAspectIsNotSelcted();
            Assert.AreEqual(aspectSelction, "false");

            //check if assessment period selections are not retained
            addAspects1.SelectNReturnSelectedAssessments(2);
            addAssessmentPeriod = addAspects.AspectNextButton();

            List<string> APselectedItems = new List<string>();
            APselectedItems = addAssessmentPeriod.getAspectAssessmentPeriodSelectedItems();

            int i = 0;

            foreach (String eachitem in selectedAssessmentperiod)
            {
                Assert.AreEqual(eachitem, APselectedItems[i]);
                i++;
            }
        }


        /// <summary>
        /// Story - 8208 - Back and Close button on slider control after click of Done button.
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet Builder", "Assessment CNR", "GridFailed", "AspectBack" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AspectBackCloseButton()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();
            //Select Aspect
            addAspects.SelectNReturnSelectedAssessments(2);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();

            //Select Assessment period

            addAssessmentPeriod.AspectAssessmentPeriodSelection(2);
            //Get the selected Asessment Period in a list
            List<string> selectedAssessmentperiod = addAssessmentPeriod.getAspectAssessmentPeriodSelectedItems();

            marksheetBuilder = addAssessmentPeriod.ClickAspectAssessmentPeriodDone();
            // Check columns displayed in preview

            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            addAssessments = marksheetBuilder.NavigateAssessments();

            addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            String aspectSelction = addAspects.checkAspectIsNotSelcted();
            Assert.AreEqual(aspectSelction, "false");
            //check if assessment period selections are not retained


            addAspects.SelectNReturnSelectedAssessments(2);
            addAssessmentPeriod = addAspects.AspectNextButton();
            addAssessmentPeriod.AspectAssessmentPeriodSelection(2);

            //WebContext.WebDriver.FindElement(MarksheetConstants.AspectAPBackButton).Click();
            addAspects = addAssessmentPeriod.ClickAspectAssessmentPeriodBack();

            addAssessments = addAspects.AspectBackButton();

            Thread.Sleep(2000);

            addAspects = addAssessments.NavigateAssessmentsviaAssessment();
            aspectSelction = addAspects.checkAspectIsNotSelcted();
            Assert.AreEqual(aspectSelction, "false");
            //CheckItemsIsNotSelcted(MarksheetConstants.AspectElementSelection);
            addAspects.SelectNReturnSelectedAssessments(2);
            addAssessmentPeriod = addAspects.AspectNextButton();

            List<string> APselectedItems = new List<string>();
            APselectedItems = addAssessmentPeriod.getAspectAssessmentPeriodSelectedItems();

            //Check assessment period selction before done are retained

            int i = 0;
            foreach (String eachitem in APselectedItems)
            {
                Assert.AreEqual(eachitem, selectedAssessmentperiod[i]);
                i++;
            }
        }

        /// <summary>
        /// Story - 8208 - Back and Close button on slider control after click of Done button.
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet Builder", "Assessment CNR", "Subject Done" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SubjectDoneButton()
        {

            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            addSubjects.SelectSubjectResult(2);
            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();

            //Select Subject and its properties
            //    marksheetBuilder.SelectSearchedSubjectResultAndMoveNext(2);

            addModeMethodPurpose.purposeAssessmentPeriodSelection(2);
            addModeMethodPurpose.modeAssessmentPeriodSelection(2);
            addModeMethodPurpose.methodAssessmentPeriodSelection(2);

            AddAssessmentPeriod addassessmentperiod = addModeMethodPurpose.modeMethodPurposeNextButton();

            //Select Assessment period
            List<string> selectedAssessmentperiod = addassessmentperiod.subjectAssessmentPeriodSelection(2);

            // Check columns displayed in preview

            marksheetBuilder.IsMarksheetPreviewColumnsPresent();
            //Click on DOne button
            // WebContext.WebDriver.FindElement(MarksheetConstants.SubjectDoneButton).Click();
            marksheetBuilder = addassessmentperiod.ClickSubjectAssessmentPeriodDone();
            // Check columns removed in preview
            marksheetBuilder.IsMarksheetPreviewColumnsNotPresent();

            //Navigate to Assessment screen Aspects are not selcted any more
            Thread.Sleep(2000);
            addAssessments = marksheetBuilder.NavigateAssessments();
            addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            addSubjects.checkSubjectIsNotSelcted();

            addSubjects.SelectSubjectResult(2);
            addModeMethodPurpose = addSubjects.SubjectNextButton();

            String modeSelected = addModeMethodPurpose.checkModeIsNotSelcted();
            Assert.AreEqual(modeSelected, "false");

            String methodSelected = addModeMethodPurpose.checkMethodIsNotSelcted();
            Assert.AreEqual(methodSelected, "false");

            String purposeSelected = addModeMethodPurpose.checkPurposeIsNotSelcted();
            Assert.AreEqual(purposeSelected, "false");

            // select subjects and then assessment period other than those retained
            addModeMethodPurpose.purposeAssessmentPeriodSelection(2);
            addModeMethodPurpose.modeAssessmentPeriodSelection(2);
            addModeMethodPurpose.methodAssessmentPeriodSelection(2);

            addassessmentperiod = addModeMethodPurpose.modeMethodPurposeNextButton();

            //Check the newly selected assessment periods after click of done button are not retained

            List<string> APselectedItems = new List<string>();
            APselectedItems = addassessmentperiod.getSubjectAssessmentPeriodSelectedItems();

            int i = 0;
            foreach (String eachitem in APselectedItems)
            {
                Assert.AreEqual(eachitem, selectedAssessmentperiod[i]);
                i++;
            }
        }


        /// <summary>
        /// Story - 8208 - Done button on slider control. Additional column Done button
        /// Story-5889 -Save Supplier Marksheet Additional Column Preferences in Save Marksheet Transaction
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, InvocationCount = 1, Groups = new[] { "AdditionalColumnDoneTest", "Assessment CNR", "Done" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AdditionalColumnDoneButton()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //marksheetBuilder.NavigateAssessments();
            AdditionalColumn additionalcolumn = marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();

            additionalcolumn.SelectNoOfAdditionalColumn(2);

            List<string> additionalColumnSelected = additionalcolumn.GetSelectedAdditionalColumnList();

            marksheetBuilder = additionalcolumn.ClickDoneButton();

            // Check columns displayed in preview
            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            Thread.Sleep(2000);
            marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();

            List<string> checkSelected = additionalcolumn.GetSelectedAdditionalColumnList();

            //Assert Logic
            foreach (String eachitem in checkSelected)
            {
                Assert.IsTrue(additionalColumnSelected.Contains(eachitem));
            }
            marksheetBuilder.setMarksheetProperties("Marksheet Properties Test", "Description", false);
            marksheetBuilder.Save();
        }


        /// <summary>
        /// Story - 8208 - Done button on slider control. Additional column Done button
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "AdditionalColumnDoneBackButton", "Assessment CNR", "Done" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AdditionalColumnDoneBackButton()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            //Create page object of Additional Column Panel
            AdditionalColumn additionalcolumn = new AdditionalColumn();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //marksheetBuilder.NavigateAssessments();
            marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();

            additionalcolumn.SelectNoOfAdditionalColumn(2);

            List<string> additionalColumnSelected = additionalcolumn.GetSelectedAdditionalColumnList();

            // Check columns displayed in preview
            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            marksheetBuilder = additionalcolumn.ClickDoneButton();

            // Check columns are retained in preview
            marksheetBuilder.IsMarksheetPreviewColumnsPresent();
            marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();
            additionalcolumn.SelectNoOfAdditionalColumn(2);
            marksheetBuilder = additionalcolumn.ClickBackButton();
            marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();

            List<string> checkSelected = additionalcolumn.GetSelectedAdditionalColumnList();

            foreach (String eachitem in checkSelected)
            {
                Assert.IsTrue(additionalColumnSelected.Contains(eachitem));
            }
        }

        /// <summary>
        /// Story 8208 - MCN - close and back button after click of done
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "SelectedGroupDoneTest", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SelectedGroupDoneTest()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddGroups addgroups = marksheetBuilder.NavigateGroups();
            //Selects year group and moves to next screen

            addgroups.SelectNoOfYearGroups(1);
            List<String> selectedYearGroupBeforeDone = addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);

            //waiter.Until(ExpectedConditions.TextToBePresentInElement(marksheetBuilder.GroupNextButton, "Next"));
            marksheetBuilder = addgroups.ClickDoneButton();

            addgroups = marksheetBuilder.NavigateGroups();
            List<string> selectedGroup = addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);

            foreach (string eachstring in selectedGroup)
            {
                Assert.IsTrue(selectedYearGroupBeforeDone.Contains(eachstring));
            }

            marksheetBuilder = addgroups.ClickDoneButton();

        }



        /// <summary>
        /// Story 8208 - MCN - close and back button after click of done
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "SelectedGroupDoneBackTest", "Assessment CNR", "Done" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SelectedGroupDoneBackTest()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Select & Store the Selected Year Group
            AddGroups addgroups = marksheetBuilder.NavigateGroups();
            addgroups.SelectNoOfYearGroups(1);
            string selectedGroupBeforeDone = marksheetBuilder.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);
            marksheetBuilder = addgroups.ClickDoneButton();

            //Select & Store the Selected Group Filter
            GroupFilters groupfilters = marksheetBuilder.NavigateAdditionalFilter();
            groupfilters.SelectYearGroupsFilterName(selectedGroupBeforeDone);
            string selectedFilterBeforeDone = groupfilters.GetSelctedYearGroupsFilterName();
            marksheetBuilder = groupfilters.ClickDoneButton();

            //Navigate to the Group Selection Page
            addgroups = marksheetBuilder.NavigateGroups();
            addgroups.SelectNoOfYearGroups(1);
            marksheetBuilder = addgroups.ClickBackButton();
            addgroups = marksheetBuilder.NavigateGroups();
            string selectedGroup = marksheetBuilder.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);
            Assert.AreEqual(selectedGroupBeforeDone, selectedGroup);
            marksheetBuilder = addgroups.ClickDoneButton();

            //Navigate to the Group Filter Selection Page
            groupfilters = marksheetBuilder.NavigateAdditionalFilter();
            groupfilters.SelectYearGroupsFilterName(selectedGroupBeforeDone);
            marksheetBuilder = groupfilters.ClickBackButton();
            groupfilters = marksheetBuilder.NavigateAdditionalFilter();
            string selectedFilterGroup = groupfilters.GetSelctedYearGroupsFilterName();
            Assert.AreEqual(selectedFilterBeforeDone, selectedFilterGroup);
            marksheetBuilder = groupfilters.ClickDoneButton();
        }

        /// <summary>
        /// Story - 4918 - Verifies 81 Aspects are preset on the search panel.
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet Builder", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AspectPanelValidation()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            //Search for all the aspects
            addAspects.AspectSearch();

        }

        /// <summary>
        /// Add Aspects in the create marksheet with levels page using Marksheet Builder
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet Builder", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AddAssessmentForNewMarksheetwithLevels()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            //Select Aspect
            addAspects.SelectNReturnSelectedAssessments(3);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();

        }

        /// <summary>
        /// Story 4923 - Add Aspects through subjects in the create marksheet with levels page using Marksheet Builder
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet Builder", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AddAspectThroughSubject()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            //Create a aspect by selecting subjects and its properties
            addSubjects.SelectSubjectResult(2);

            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();

            addModeMethodPurpose.purposeAssessmentPeriodSelection(2);
            addModeMethodPurpose.modeAssessmentPeriodSelection(2);
            addModeMethodPurpose.methodAssessmentPeriodSelection(2);

            AddAssessmentPeriod addAssessmentPeriod = addModeMethodPurpose.modeMethodPurposeNextButton();
            addAssessmentPeriod.subjectAssessmentPeriodSelection(2);
            marksheetBuilder = addAssessmentPeriod.ClickSubjectAssessmentPeriodDone();
        }


        /// <summary>
        /// Story 4923 - Search Subject - covered 2 cases one for no match and other for a single match
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet Builder", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchSubject()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            //Search for aspect that is not valid
            addSubjects.EnterSubjectSearchCriteria(":$");
            addSubjects = addSubjects.ClickSubjectSearchButton();
            var ResultText = "";

            ResultText = addSubjects.GetSubjectSearchResultCount();

            Assert.AreEqual(ResultText, "No Matches");

            //Search for a specific Subject and view the result
            ResultText = "";
            addSubjects = new AddSubjects();
            addSubjects.EnterSubjectSearchCriteria("");
            addSubjects = addSubjects.ClickSubjectSearchButton();
            List<string> SubjectSearchResultsList = new List<string>();
            SubjectSearchResultsList = addSubjects.GetSubjectSearchResults();
            addSubjects = new AddSubjects();
            string itemSelection = SubjectSearchResultsList[1];
            addSubjects.EnterSubjectSearchCriteria(itemSelection);
            addSubjects = addSubjects.ClickSubjectSearchButton();

            SubjectSearchResultsList = new List<string>();
            SubjectSearchResultsList = addSubjects.GetSubjectSearchResults();
            Assert.IsTrue(SubjectSearchResultsList.Contains(itemSelection));
        }

        /// <summary>
        /// Story 7704 - MCN - UI - Integration to Marksheet Properties
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveMarksheetProperties()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Verifying the validation message
            marksheetBuilder.Save();
            marksheetBuilder.SaveMarksheetAssertionFailure();

            //Verifying the saved marksheet     
            marksheetBuilder.setMarksheetProperties("Marksheet Properties Test", "Description", false);
            //marksheetBuilder.Save();
            marksheetBuilder.SaveMarksheetAssertionSuccess();
            //var SearchedMarksheetName = marksheetBuilder.SearchByName("Marksheet Properties Test", false, false);
            //Assert.AreEqual("Marksheet Properties Test", SearchedMarksheetName);
        }

        /// <summary>
        /// Story 7152 - MCN - Save Marksheet Template
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "Marksheet", "Assessment CNR", "SaveMarksheetTemplate" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveMarksheetTemplate()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Randomly generate Unique Marksheet Name
            var MarksheetTemplateName = marksheetBuilder.RandomString(8);
            //Verifying the saved marksheet     
            marksheetBuilder.setMarksheetProperties(MarksheetTemplateName, "Description", true);
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(2);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();
            addAssessmentPeriod.AspectAssessmentPeriodSelection(2);
            addAssessmentPeriod.ClickAspectAssessmentPeriodDone();
            AddGroups addgroups = marksheetBuilder.NavigateGroups();
            addgroups.SelectNoOfYearGroups(1);
            List<string> selectedGroups = addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);
            marksheetBuilder = addgroups.ClickDoneButton();

            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            marksheetBuilder = groupfilter.ClickDoneButton();
            //MarksheetTemplateProperties marksheettemplateproperties = new MarksheetTemplateProperties();
            //MarksheetTemplateDetails marksheettemplatedetails = marksheettemplateproperties.OpenDetailsTab();
            //marksheetBuilder.CheckMarksheetIsAvailable();                    
            marksheetBuilder.Save();
            //marksheetBuilder.SaveMarksheetAssertionSuccess();

            //Navigate to Marksheet Data Entry and check the marksheet is displayed
            SeleniumHelper.NavigatebackToMenu("Tasks", "Assessment", "Marksheets");
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(MarksheetTemplateName + " - " + selectedGroups[0]));
            Thread.Sleep(2000);
        }

        /// <summary>
        /// 3146 - Test to Save the marksheet Properties Name,Description
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 8000, Groups = new[] { "preview", "Assessment CNR", "Done" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void Preview()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(1);

            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();

            marksheetBuilder.SelectSearchedAssessmentPeriodAndDone(1);

            AddAssessments addAssessments1 = marksheetBuilder.NavigateAssessments();

            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            addSubjects.SelectSubjectResult(1);
            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();

            addModeMethodPurpose.purposeAssessmentPeriodSelection(2);
            addModeMethodPurpose.modeAssessmentPeriodSelection(2);
            addModeMethodPurpose.methodAssessmentPeriodSelection(2);


            addAssessmentPeriod = addModeMethodPurpose.modeMethodPurposeNextButton();
            addAssessmentPeriod.subjectAssessmentPeriodSelection(2);
            marksheetBuilder = addAssessmentPeriod.ClickSubjectAssessmentPeriodDone();

            //columns.Clear();
            //foreach (var col in MarksheetGridHelper.FindAllColumns())
            //{
            //    if(col.Text!= "")
            //    columns.Add(col.Text);
            //}

            //foreach (string col in selectedResults)
            //{
            //    Assert.IsTrue(columns.Contains(col));
            //}
        }


        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, InvocationCount = 1, Groups = new[] { "Assessment CNR", "preview", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void PropertyPreview()
        {

            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            marksheetBuilder.SelectPropertiesTab();

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            AddAssessmentPeriod addAssessmentPeriod = addAspects.SelectAssessmentsAndMoveNext(2);

            marksheetBuilder.SelectSearchedAssessmentPeriodAndDone(1);



            marksheetBuilder.NavigateAssessments();

            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            addSubjects.SelectSubjectResult(1);
            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();

            addModeMethodPurpose.purposeAssessmentPeriodSelection(2);
            addModeMethodPurpose.modeAssessmentPeriodSelection(2);
            addModeMethodPurpose.methodAssessmentPeriodSelection(2);

            addAssessmentPeriod = addModeMethodPurpose.modeMethodPurposeNextButton();
            addAssessmentPeriod.subjectAssessmentPeriodSelection(2);
            marksheetBuilder = addAssessmentPeriod.ClickSubjectAssessmentPeriodDone();

            AdditionalColumn additionalcolumn = marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();

            additionalcolumn.SelectNoOfAdditionalColumn(2);

            List<string> additionalColumnSelected = additionalcolumn.GetSelectedAdditionalColumnList();
            IWebElement openElement = WebContext.WebDriver.FindElement(MarksheetConstants.TreeOpenGrid);
            openElement.Click();
            WebContext.Screenshot();


        }

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 600, Groups = new[] { "Assessment CNR", "IntegratingPropertieswithPreview" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void IntegratingPropertieswithPreview()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator, true);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            marksheetBuilder.SelectPropertiesTab();

            // Assessment Flow
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            AddAssessmentPeriod addAssessmentPeriod = addAspects.SelectAssessmentsAndMoveNext(2);

            marksheetBuilder.SelectSearchedAssessmentPeriodAndDone(1);


            WebContext.Screenshot();
            //Make readonly
            ReadOnlyCollection<IWebElement> readOnlyElementsCollection = WebContext.WebDriver.FindElements(By.CssSelector("[data-createmarksheet-readonly='']"));
            foreach (IWebElement readonlyElement in readOnlyElementsCollection)
            {
                readonlyElement.Click();

            }


            //Make hidden
            ReadOnlyCollection<IWebElement> hiddenElementsCollection = WebContext.WebDriver.FindElements(By.CssSelector("[data-createmarksheet-hidden='']"));
            foreach (IWebElement hiddenElement in hiddenElementsCollection)
            {
                hiddenElement.Click();

            }


            WebContext.Screenshot();

            //delete from preview
            ReadOnlyCollection<IWebElement> DeleteList = WebContext.WebDriver.FindElements(By.CssSelector("span[class='webix_icon fa-trash']"));

            int count = 0;
            foreach (IWebElement deleteElement in DeleteList)
            {
                if (count == 2)
                {
                    deleteElement.Click();
                    Thread.Sleep(1000);
                }
                count++;


            }


            WebContext.Screenshot();

            //From Subject Path
            addAssessments = marksheetBuilder.NavigateAssessments();
            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            addSubjects.SelectSubjectResult(1);
            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();

            addModeMethodPurpose.purposeAssessmentPeriodSelection(2);
            addModeMethodPurpose.modeAssessmentPeriodSelection(2);
            addModeMethodPurpose.methodAssessmentPeriodSelection(2);


            addAssessmentPeriod = addModeMethodPurpose.modeMethodPurposeNextButton();
            addAssessmentPeriod.subjectAssessmentPeriodSelection(2);
            marksheetBuilder = addAssessmentPeriod.ClickSubjectAssessmentPeriodDone();

            //delete from preview
            DeleteList = WebContext.WebDriver.FindElements(By.CssSelector("span[class='webix_icon fa-trash']"));

            Thread.Sleep(1000);
            DeleteList[4].Click();
            Thread.Sleep(1000);


            DeleteList = WebContext.WebDriver.FindElements(By.CssSelector("span[class='webix_icon fa-trash']"));

            Thread.Sleep(1000);
            DeleteList[3].Click();
            Thread.Sleep(1000);

            DeleteList = WebContext.WebDriver.FindElements(By.CssSelector("span[class='webix_icon fa-trash']"));

            Thread.Sleep(1000);
            DeleteList[3].Click();
            Thread.Sleep(1000);

            WebContext.Screenshot();

        }



        /// <summary>
        /// Story 7276 MCN - UI - Validation on Subject, Method, Mode and Purpose    
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "Assessment CNR", "SubjectValidations" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SubjectValidations()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();
            addSubjects.SelectSubjectResult(0);
            addSubjects.SubjectNextButton();
            ValidationPopUpMessages validationpopup = new ValidationPopUpMessages();
            //Assertion Method
            Assert.AreEqual("Please select at least one subject before proceeding to the next step", validationpopup.GetPopUpValidationMessageText());
        }

        /// <summary>
        /// Story 7276 MCN - UI - Validation on Subject, Method, Mode and Purpose

        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "Assessment CNR", "SubjectValidations12" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void ModeMethodPurposeValidations()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();
            addSubjects.SelectSubjectResult(2);
            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();
            waiter.Until(ExpectedConditions.ElementToBeClickable(addModeMethodPurpose._btnSubjectNext));
            addModeMethodPurpose._btnSubjectNext.Click();
            ValidationPopUpMessages validationpopup = new ValidationPopUpMessages();
            //Assertion Method
            Assert.AreEqual("Please select from each section to indicate the following:" +
                                   "What do you want to measure..." +
                                   "Are you recording a..." +
                                   "Is this a..." +
                                   "before proceeding to next step", validationpopup.GetPopUpValidationMessageText().Replace("\r\n", ""));

        }

        /// <summary>
        /// Story 7294 MCN - UI - Validation on Assessment Period
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "Assessment CNR", "SubjectValidations" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AssessmentPeriodValidations()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();
            //selects a subject and moves to next screen
            addSubjects.SelectSubjectResult(2);
            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();

            //selects a assessment purpose and moves to next screen
            addModeMethodPurpose.purposeAssessmentPeriodSelection(2);
            //selects a assessment mode
            addModeMethodPurpose.modeAssessmentPeriodSelection(2);
            //selects a assessment method
            addModeMethodPurpose.methodAssessmentPeriodSelection(2);

            AddAssessmentPeriod addAssessmentPeriod = addModeMethodPurpose.modeMethodPurposeNextButton();
            addAssessmentPeriod.subjectAssessmentPeriodSelection(0);
            //Validation for No Assessment Period selected on the Subject Selection Flow
            addAssessmentPeriod._btnSubjectAssessmentPeriodDone.Click();
            //Assertion Method
            ValidationPopUpMessages validationpopup = new ValidationPopUpMessages();
            //Assertion Method
            Assert.AreEqual("Please select at least one assessment period before proceeding to the next step", validationpopup.GetPopUpValidationMessageText());


        }


        /// <summary>
        /// Story 6053 - MCN - Population of group filters on slider control
        /// Script for story 6053 is further divided into 10 Test Scripts to verify each Group Filter Scenarios.
        /// </summary> 

        /// <summary>
        /// Story 6053 - 1. Verification of NC Year Group Filter
        /// </summary>

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "Group Filter" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifyNCYearGroupFilter()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();

            //Below code can be used only if the NC Years are available\visible under Group Filter list
            List<string> NCYearDataList = new List<string>();
            NCYearDataList = TestData.CreateDataList("Select distinct FullName from NCYear NC inner join SchoolNCYear SC on  Sc.NCYear=nc.id inner join LearnerNCYearMembership LNC on LNC.SchoolNCYear=sc.id inner join  learner l on LNC.Learner=l.ID where SC.School= '" + TestData.GetSchoolID() + "'  and l.School= '" + TestData.GetSchoolID() + "' and nc.IsVisible=1", "FullName");
            Assert.AreEqual(NCYearDataList.Count(), groupfilter.GetNCYearFilterOptionsCount());
            groupfilter = groupfilter.NewGroupFiltersPageObject();
            if (NCYearDataList.Count() > 0)
            {
                groupfilter.SelectNCYearFilterName(NCYearDataList[NCYearDataList.Count() - 1]);
                groupfilter = groupfilter.NewGroupFiltersPageObject();
                Assert.AreEqual(NCYearDataList[NCYearDataList.Count() - 1], groupfilter.GetSelctedNCYearFilterName());
            }
        }

        /// <summary>
        /// Story 6053 - 2. Verification of Ethnicity Group Filter
        /// </summary>

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "GridFailed", "Group Filter" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifyEthnicityGroupFilter()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();

            //Selects all the Filters and also verifies that all the; group filters options are available on the Group Filter Selection Screen
            List<string> EthnicityDataList = new List<string>();
            EthnicityDataList = TestData.CreateDataList("Select distinct e.Description from Ethnicity e inner join  learner l on e.id=l.Ethnicity where (e.ResourceProvider  = '" + TestData.GetSchoolID() + "' OR e.ResourceProvider='" + TestData.GetDENIID() + "') and l.School = '" + TestData.GetSchoolID() + "' and e.IsVisible=1", "Description");
            Assert.AreEqual(EthnicityDataList.Count(), groupfilter.GetEthnicityFilterOptionsCount());
            groupfilter = groupfilter.NewGroupFiltersPageObject();
            if (EthnicityDataList.Count() > 0)
            {
                groupfilter.SelectEthnicityFilterName(EthnicityDataList[EthnicityDataList.Count() - 1]);
                groupfilter = groupfilter.NewGroupFiltersPageObject();
                Assert.AreEqual(EthnicityDataList[EthnicityDataList.Count() - 1], groupfilter.GetSelctedEthnicityFilterName());
            }
        }

        /// <summary>
        /// Story 6053 - 3. Verification of Language Group Filter
        /// </summary>

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "VerifyLanguageGroupFilter", "Assessment CNR", "GridFailed", "Group Filter" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifyLanguageGroupFilter()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            //Selects all the Filters and also verifies that all the; group filters options are available on the Group Filter Selection Screen
            List<string> LanguageDataList = new List<string>();
            LanguageDataList = TestData.CreateDataList("Select distinct e.Description from Language e inner join  learner l on e.id=l.Language where (e.ResourceProvider  = '" + TestData.GetSchoolID() + "' OR e.ResourceProvider='" + TestData.GetDENIID() + "') and l.School = '" + TestData.GetSchoolID() + "' and e.IsVisible=1", "Description");
            Assert.AreEqual(LanguageDataList.Count(), groupfilter.GetLanguageFilterOptionsCount());
            groupfilter = groupfilter.NewGroupFiltersPageObject();
            if (LanguageDataList.Count() > 0)
            {
                groupfilter.SelectLanguageFilterName(LanguageDataList[LanguageDataList.Count() - 1]);
                groupfilter = groupfilter.NewGroupFiltersPageObject();
                Assert.AreEqual(LanguageDataList[LanguageDataList.Count() - 1], groupfilter.GetSelctedLanguageFilterName());
            }
        }

        /// <summary>
        /// Story 6053 - 4. Verification of New Intake Group Filter
        /// </summary>

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "GridFailed", "Group Filter" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifyNewIntakeGroupFilter()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            //Selects all the Filters and also verifies that all the; group filters options are available on the Group Filter Selection Screen
            List<string> NewIntakeDataList = new List<string>();
            NewIntakeDataList = TestData.CreateDataList("Select distinct SI.Name from SchoolIntake SI inner join  YearGroup YG on YG.ID=SI.YearGroup inner join  Learner L on L.YearGroup=YG.ID where (SI.School  = '" + TestData.GetSchoolID() + "') and L.School = '" + TestData.GetSchoolID() + "' and SI.IsActive=1 ", "Name");
            Assert.AreEqual(NewIntakeDataList.Count(), groupfilter.GetSchoolIntakeFilterOptionsCount());
            groupfilter = groupfilter.NewGroupFiltersPageObject();
            if (NewIntakeDataList.Count() > 0)
            {
                groupfilter.SelectSchoolIntakeFilterName(NewIntakeDataList[NewIntakeDataList.Count() - 1]);
                groupfilter = groupfilter.NewGroupFiltersPageObject();
                Assert.AreEqual(NewIntakeDataList[NewIntakeDataList.Count() - 1], groupfilter.GetSelctedSchoolIntakeFilterName());
            }
        }

        /// <summary>
        /// Story 6053 - 5. Verification of Classes Group Filter
        /// </summary>

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "GridFailed", "Group Filter" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifyClassGroupFilter()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");


            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            //Selects all the Filters and also verifies that all the; group filters options are available on the Group Filter Selection Screen
            List<string> ClassesDataList = new List<string>();
            ClassesDataList = TestData.CreateDataList("select distinct PC.FullName from PrimaryClass PC inner join  Learner L on L.PrimaryClass=PC.ID inner join PrimaryClassSetMembership YM on YM.PrimaryClass=PC.ID where (PC.School  = '" + TestData.GetSchoolID() + "') and L.School = '" + TestData.GetSchoolID() + "'  and YM.StartDate <= GETDATE()  and (isnull(YM.EndDate,GETDATE()) >=GETDATE() OR YM.EndDate IS Null)", "FullName");
            Assert.AreEqual(ClassesDataList.Count(), groupfilter.GetClassFilterOptionsCount());
            groupfilter = groupfilter.NewGroupFiltersPageObject();
            if (ClassesDataList.Count() > 0)
            {
                groupfilter.SelectClassFilterName(ClassesDataList[ClassesDataList.Count() - 1]);
                groupfilter = groupfilter.NewGroupFiltersPageObject();
                Assert.AreEqual(ClassesDataList[ClassesDataList.Count() - 1], groupfilter.GetSelctedClassFilterName());
            }
        }


        /// <summary>
        /// Story 6053 - 6. Verification of Year Groups Group Filter
        /// </summary>

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "VerifyYearGroupsGroupFilter", "Assessment CNR", "GridFailed", "Group Filter" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifyYearGroupsGroupFilter()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");


            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            //Selects all the Filters and also verifies that all the; group filters options are available on the Group Filter Selection Screen
            List<string> YearGroupDataList = new List<string>();
            YearGroupDataList = TestData.CreateDataList("Select distinct PC.FullName from YearGroup PC inner join  Learner L on L.YearGroup=PC.ID inner join YearGroupSetMembership YM on YM.YearGroup=PC.ID where (PC.School  = '" + TestData.GetSchoolID() + "'  ) and L.School = '" + TestData.GetSchoolID() + "' and YM.StartDate <= GETDATE()  and (isnull(YM.EndDate,GETDATE()) >=GETDATE() OR YM.EndDate IS Null)", "FullName");
            Assert.AreEqual(YearGroupDataList.Count(), groupfilter.GetYearGroupsFilterOptionsCount());
            groupfilter = groupfilter.NewGroupFiltersPageObject();
            if (YearGroupDataList.Count() > 0)
            {
                groupfilter.SelectYearGroupsFilterName(YearGroupDataList[YearGroupDataList.Count() - 1]);
                groupfilter = groupfilter.NewGroupFiltersPageObject();
                Assert.AreEqual(YearGroupDataList[YearGroupDataList.Count() - 1], groupfilter.GetSelctedYearGroupsFilterName());
            }
        }

        /// <summary>
        /// Story 6053 - 7. Verification of SEN Need Type Group Filter
        /// </summary>

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "GridFailed", "Group Filter" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifySENNeedTypeGroupFilter()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");


            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            //Selects all the Filters and also verifies that all the; group filters options are available on the Group Filter Selection Screen
            List<string> SENNeedTypeDataList = new List<string>();
            SENNeedTypeDataList = TestData.CreateDataList("Select distinct T.Description from SENNeedType T inner join  LearnerSENNeedType TG on TG.NeedType=T.ID inner join  Learner GM on GM.ID=TG.Learner where (T.ResourceProvider  = '" + TestData.GetSchoolID() + "'  OR T.ResourceProvider='" + TestData.GetDENIID() + "') and GM.School = '" + TestData.GetSchoolID() + "'  and T.IsVisible=1 ", "Description");
            Assert.AreEqual(SENNeedTypeDataList.Count(), groupfilter.GetSenNeedTypeFilterOptionsCount());
            groupfilter = groupfilter.NewGroupFiltersPageObject();
            if (SENNeedTypeDataList.Count() > 0)
            {
                groupfilter.SelectSenNeedTypeFilterName(SENNeedTypeDataList[SENNeedTypeDataList.Count() - 1]);
                groupfilter = groupfilter.NewGroupFiltersPageObject();
                Assert.AreEqual(SENNeedTypeDataList[SENNeedTypeDataList.Count() - 1], groupfilter.GetSelctedSenNeedTypeFilterName());
            }
        }

        /// <summary>
        /// Story 6053 - 8. Verification of User Defined Groups Group Filter
        /// </summary>

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "GridFailed", "Group Filter" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifyUserDefinedGroupsGroupFilter()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");


            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            //Selects all the Filters and also verifies that all the; group filters options are available on the Group Filter Selection Screen
            List<string> UserDefinedGroupsDataList = new List<string>();
            UserDefinedGroupsDataList = TestData.CreateDataList("Select distinct FullName from UserDefinedGroup u inner join  UserDefinedGroupMembership UG on UG.UserDefinedGroup=u.ID inner join  GroupMember GM on GM.ID=UG.GroupMember where (u.School  = '" + TestData.GetSchoolID() + "' ) and GM.School = '" + TestData.GetSchoolID() + "'  and u.IsVisible=1 ", "FullName");
            Assert.AreEqual(UserDefinedGroupsDataList.Count(), groupfilter.GetUserDefinedGroupFilterOptionsCount());
            groupfilter = groupfilter.NewGroupFiltersPageObject();
            if (UserDefinedGroupsDataList.Count() > 0)
            {
                groupfilter.SelectUserDefinedGroupFilterName(UserDefinedGroupsDataList[UserDefinedGroupsDataList.Count() - 1]);
                groupfilter = groupfilter.NewGroupFiltersPageObject();
                Assert.AreEqual(UserDefinedGroupsDataList[UserDefinedGroupsDataList.Count() - 1], groupfilter.GetSelctedUserDefinedGroupFilterName());
            }
        }

        /// <summary>
        /// Story 6053 - 9. Verification of Teaching Groups Group Filter
        /// </summary>

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "VerifyTeachingGroupsGroupFilter", "Assessment CNR", "Group Filter" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifyTeachingGroupsGroupFilter()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            //Selects all the Filters and also verifies that all the; group filters options are available on the Group Filter Selection Screen
            List<string> TeachingGroupsDataList = new List<string>();
            TeachingGroupsDataList = TestData.CreateDataList("Select distinct FullName from TeachingGroup T inner join  LearnerTeachingGroupMembership TG on TG.TeachingGroup=T.ID inner join  Learner GM on GM.ID=TG.Learner where (T.School  = '" + TestData.GetSchoolID() + "' ) and GM.School = '" + TestData.GetSchoolID() + "'  and T.IsVisible=1", "FullName");
            Assert.AreEqual(TeachingGroupsDataList.Count(), groupfilter.GetTeachingGroupFilterOptionsCount());
            groupfilter = groupfilter.NewGroupFiltersPageObject();
            if (TeachingGroupsDataList.Count() > 0)
            {
                groupfilter.SelectTeachingGroupFilterName(TeachingGroupsDataList[TeachingGroupsDataList.Count() - 1]);
                groupfilter = groupfilter.NewGroupFiltersPageObject();
                Assert.AreEqual(TeachingGroupsDataList[TeachingGroupsDataList.Count() - 1], groupfilter.GetSelctedTeachingGroupFilterName());
            }
        }

        /// <summary>
        /// Story 6053 - 10. Verification of SEN Status Group Filter
        /// </summary>

        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "VerifySENStatusGroupFilter", "Assessment CNR", "Group Filter" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifySENStatusGroupFilter()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Navigate to Additional Filters
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            //Selects all the Filters and also verifies that all the; group filters options are available on the Group Filter Selection Screen
            List<string> SENStatusDataList = new List<string>();
            SENStatusDataList = TestData.CreateDataList("Select distinct Description from SENStatus T inner join  LearnerSENStatus TG on TG.SENStatus=T.ID inner join  Learner GM on GM.ID=TG.Learner where (T.ResourceProvider  = '" + TestData.GetSchoolID() + "'  OR T.ResourceProvider='" + TestData.GetDENIID() + "') and GM.School = '" + TestData.GetSchoolID() + "'  and T.IsVisible=1", "Description");
            Assert.AreEqual(SENStatusDataList.Count(), groupfilter.GetSENStatusFilterOptionsCount());
            groupfilter = groupfilter.NewGroupFiltersPageObject();
            if (SENStatusDataList.Count() > 0)
            {
                groupfilter.SelectSENStatusFilterName(SENStatusDataList[SENStatusDataList.Count() - 1]);
                groupfilter = groupfilter.NewGroupFiltersPageObject();
                Assert.AreEqual(SENStatusDataList[SENStatusDataList.Count() - 1], groupfilter.GetSelctedSENStatusFilterName());
            }
        }


        /// <summary>
        /// Story 7293 & 7294 - MCN - UI - Validation on Aspect & Assessment Period Selection
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "Assessment CNR", "AssessmentValidations1" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AssessmentValidations()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //**1**
            //Validation for NO Aspects or Assessment selected
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(0);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();
            ValidationPopUpMessages validationpopup = new ValidationPopUpMessages();
            //Assertion Method
            Assert.AreEqual("Please select at least one assessment before proceeding to the next step", validationpopup.GetPopUpValidationMessageText());
        }

        /// <summary>
        /// Story 7293 & 7294 - MCN - UI - Validation on Aspect & Assessment Period Selection
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "Assessment CNR", "AspectAssessmentPeriodValidations" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AspectAssessmentPeriodValidations()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //**1**
            //Validation for NO Aspects or Assessment selected
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            //Validation for NO Assessment Period selected on the Aspect Selection Flow
            //addAssessments = marksheetBuilder.NavigateAssessments();
            addAspects.SelectNReturnSelectedAssessments(2);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();

            addAssessmentPeriod.AspectAssessmentPeriodSelection(0);
            addAssessmentPeriod._btnAspectAssessmentPeriodDone.Click();

            ValidationPopUpMessages validationpopup = new ValidationPopUpMessages();
            //Assertion Method
            Assert.AreEqual("Please select at least one assessment period before proceeding to the next step", validationpopup.GetPopUpValidationMessageText());


        }

        /// <summary>
        /// Story 7295 - MCN - UI - Validation on Group Selection
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "Marksheet", "Assessment CNR", "GroupValidation" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void GroupValidation()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Validation for NO Groups And Classes selected
            AddGroups addgroups = marksheetBuilder.NavigateGroups();
            //Select the No. of Groups & Classes and moves to next screen
            // addgroups.SelectNoOfYearGroups(0);
            //addgroups.SelectNoOfClasses(0);
            marksheetBuilder = addgroups.ClickDoneButton();

            //Assertion Method
            ValidationPopUpMessages validationpopup = new ValidationPopUpMessages();
            //Assertion Method
            Assert.AreEqual("Please select at least one group before proceeding to the next step", validationpopup.GetPopUpValidationMessageText());
        }

        /// <summary>
        /// Story 6055 - MCN - Selected group filters to be displayed on slider control
        /// Story 3142 -MCN - Save selected group filter in marksheet preferences (Click of Save Marksheet )
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "SelectedGroupFilterTest", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SelectedGroupFilterTest()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            GroupFilters groupfilters = marksheetBuilder.NavigateAdditionalFilter();
            groupfilters.GetSelctedYearGroupsFilterName();

            List<string> YearGroupDataList = new List<string>();
            YearGroupDataList = TestData.CreateDataList("Select distinct PC.FullName from YearGroup PC inner join  Learner L on L.YearGroup=PC.ID inner join YearGroupSetMembership YM on YM.YearGroup=PC.ID where (PC.School  = '" + TestData.GetSchoolID() + "'  ) and L.School = '" + TestData.GetSchoolID() + "' and YM.StartDate <= GETDATE()  and (isnull(YM.EndDate,GETDATE()) >=GETDATE() OR YM.EndDate IS Null)", "FullName");
            groupfilters = groupfilters.NewGroupFiltersPageObject();
            if (YearGroupDataList.Count() > 0)
            {
                groupfilters.SelectYearGroupsFilterName(YearGroupDataList[YearGroupDataList.Count() - 1]);
                groupfilters = groupfilters.NewGroupFiltersPageObject();
            }

            marksheetBuilder = groupfilters.ClickDoneButton();
            waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.MarksheetFilteredBy));
            string filteredby = WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetFilteredBy).GetAttribute("value");
            try
            {
                Assert.AreEqual(filteredby, YearGroupDataList[YearGroupDataList.Count() - 1]);
            }
            catch (Exception exception)
            {

                Assert.IsTrue(true, "Value is not present");
            }
        }


        /// <summary>
        /// Story 7979 - Searched results and selected items to be retained
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "SubjectAPSelectionRetentionAfterSearch", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SubjectSelectionRetentionAfterSearch()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            var ResultText = "";
            //  marksheetBuilder.SelectSubjectSearchCriteria("");

            //Select a subject
            ReadOnlyCollection<IWebElement> subjectList = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectElementSelection);
            foreach (IWebElement eachSubject in subjectList)
            {
                if (eachSubject.Text != "")
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(eachSubject));
                    eachSubject.Click();
                    ResultText = eachSubject.Text;
                    break;
                }
            }


            // Search through subject which is not selected above
            String itemSelection = "";

            foreach (IWebElement eachSubject1 in subjectList)
            {
                if (eachSubject1.Text != "" && ResultText != eachSubject1.Text)
                {
                    itemSelection = eachSubject1.Text;
                    marksheetBuilder.SelectSubjectSearchCriteria(itemSelection);
                    break;
                }
            }

            //waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(MarksheetConstants.AspectSearchResult));

            // search with empty string so that all subjects are displayed
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.SubjectElementSelection, itemSelection));
            itemSelection = "";
            marksheetBuilder.SelectSubjectSearchCriteria(itemSelection);

            waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(MarksheetConstants.SubjectElementSelection));
            String subjectRetention = "";

            Thread.Sleep(2000);
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.SubjectElementSelection, ResultText));

            //Verify the subject selected earlier is still retained
            ReadOnlyCollection<IWebElement> SubjectAPElementSelected = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectElementSelection);

            foreach (IWebElement subjectElem in SubjectAPElementSelected)
            {
                if (subjectElem.Text != "")
                {
                    String CheckSelction = subjectElem.GetAttribute("data-selected");
                    if (CheckSelction == "true")
                    {
                        subjectRetention = subjectElem.Text;
                        break;
                    }
                }
            }
            Assert.AreEqual(ResultText, subjectRetention);

        }

        /// <summary>
        /// Story 7979 - Searched results and selected items to be retained
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "SubjectAPSelectionRetentionAfterSearch", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SubjectAssessmentPeriodSelectionRetentionAfterSearch()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();

            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            addSubjects.SelectSubjectResult(2);
            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();
            addModeMethodPurpose.purposeAssessmentPeriodSelection(2);
            addModeMethodPurpose.modeAssessmentPeriodSelection(2);
            addModeMethodPurpose.methodAssessmentPeriodSelection(2);

            AddAssessmentPeriod addAssessmentPeriod = addModeMethodPurpose.modeMethodPurposeNextButton();
            var ResultText = "";

            //Select a Assessment Period through Subject
            ReadOnlyCollection<IWebElement> subjectAPList = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectAssessmentPeriodElementSelection);
            foreach (IWebElement eachAP in subjectAPList)
            {
                if (eachAP.Text != "")
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(eachAP));
                    eachAP.Click();
                    ResultText = eachAP.Text;
                    break;
                }
            }

            // Search through Assessment period which is not selected above
            String itemSelection = "";
            foreach (IWebElement eachAP1 in subjectAPList)
            {
                if (eachAP1.Text != "" && ResultText != eachAP1.Text)
                {
                    itemSelection = eachAP1.Text;
                    marksheetBuilder.SelectSubjectAPSearchCriteria(itemSelection);
                    break;
                }
            }


            // search with empty string so that all subjects are displayed
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.SubjectAssessmentPeriodElementSelection, itemSelection));
            itemSelection = "";
            marksheetBuilder.SelectSubjectAPSearchCriteria(itemSelection);

            waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(MarksheetConstants.SubjectAssessmentPeriodElementSelection));
            String subjectRetention = "";
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.SubjectAssessmentPeriodElementSelection, ResultText));

            //Verify the Assessment Period selected earlier is still retained
            ReadOnlyCollection<IWebElement> SubjectAPElementSelected = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectAssessmentPeriodElementSelection);

            foreach (IWebElement subjectAPElem in SubjectAPElementSelected)
            {
                if (subjectAPElem.Text != "")
                {
                    String CheckSelction = subjectAPElem.GetAttribute("data-selected");
                    if (CheckSelction == "true")
                    {
                        subjectRetention = subjectAPElem.Text;
                        break;
                    }
                }
            }
            Assert.AreEqual(ResultText, subjectRetention);
        }

        /// <summary>
        /// Story 7749 - 1. Search Marksheet Template by Year Group
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "SearchMTByYearGroup", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchMTByYearGroup()
        {


            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create Page Object for Marksheet Search Panel
            MarksheetTemplateSearchPanel MTSPObj = new MarksheetTemplateSearchPanel();
            bool YearGroupFound = false;
            //Open Marksheet Search Panel
            MTSPObj.OpenSearchMarksheetPanel();
            //Expands the Year Group Selection List
            MTSPObj.ExpandYearGroupList();
            //Getting the List OF Year Groups
            List<string> yeargroup = new List<string>();
            yeargroup = TestData.CreateDataList("Select FullName From YearGroup", "FullName");
            //Selects the desired Year Group and Returns true in case the desired Year Group is found or else will return false
            YearGroupFound = MTSPObj.SelectYearGroup(yeargroup[2]);
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();
            //Selection of Active or In Active Templates
            MTSPObj.IsActive(true);
            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj.ClickOnSearch();
            List<Guid> TemplateName = new List<Guid>();
            //Getting the List of Marksheet Templates that are actually present in the database for the given Year Group
            TemplateName = TestData.CreateGuidList("Select distinct MarksheetTemplate From AssessmentMarksheet Where ID IN (Select AssessmentMarksheet From AssessmentMarksheetGroupLink Where AssessmentGroup IN (Select ID from YearGroup Where FullName = '" + yeargroup[2] + "'))", "MarksheetTemplate");
            //Asserting the Search Result count againt the count in the database
            if (TemplateName.Count() == 0)
                Assert.AreEqual("No Marksheet Templates Found", MTSPObj.GetMarksheetTemplateCount());
            else
                Assert.AreEqual(TemplateName.Count().ToString(), MTSPObj.GetMarksheetTemplateCount());
        }

        /// <summary>
        /// Story 7749 - 2. Search Marksheet Template by Class
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "SearchMTByClass" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchMTByClass()
        {

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create Page Object for Marksheet Search Panel
            MarksheetTemplateSearchPanel MTSPObj = new MarksheetTemplateSearchPanel();
            bool ClassFound = false;
            //Open Marksheet Search Panel
            MTSPObj.OpenSearchMarksheetPanel();
            //Expands the Class Selection List
            MTSPObj.ExpandClassList();
            //Getting the List OF Classes
            List<string> classname = new List<string>();
            classname = TestData.CreateDataList("Select FullName From PrimaryClass", "FullName");
            //Selects the desired Class and Returns true in case the desired Class is found or else will return false
            ClassFound = MTSPObj.SelectClass(classname[0]);
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();
            //Selection of Active or In Active Templates
            MTSPObj.IsActive(true);
            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj.ClickOnSearch();
            List<Guid> TemplateName = new List<Guid>();
            //Getting the List of Marksheet Templates that are actually present in the database for the given Year Group
            TemplateName = TestData.CreateGuidList("Select distinct MarksheetTemplate From AssessmentMarksheet Where ID IN (Select AssessmentMarksheet From AssessmentMarksheetGroupLink Where AssessmentGroup IN (Select ID from PrimaryClass Where FullName = '" + classname[0] + "'))", "MarksheetTemplate");
            //Asserting the Search Result count againt the count in the database
            if (TemplateName.Count() == 0)
                Assert.AreEqual("No Marksheet Templates Found", MTSPObj.GetMarksheetTemplateCount());
            else
                Assert.AreEqual(TemplateName.Count().ToString(), MTSPObj.GetMarksheetTemplateCount());
        }

        /// <summary>
        /// Story 7749 - 3. Search Marksheet Template by Owner
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "SearchMTByOwnerMyTemplates" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchMTByOwnerMyTemplates()
        {

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Create Page Object for Marksheet Search Panel
            MarksheetTemplateSearchPanel MTSPObj = new MarksheetTemplateSearchPanel();
            //Open Marksheet Search Panel
            MTSPObj.OpenSearchMarksheetPanel();
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();

            //Verify if the required owners are present in the dropdown
            String OwnerName = MTSPObj.VerifyIteminDropdown("My Templates");
            Assert.AreEqual(OwnerName, "My Templates");

            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj.ClickOnSearch();
            List<string> TemplateName = new List<string>();
            //Getting the List of Marksheet Templates that are actually present in the database for the given user
            TemplateName = TestData.CreateDataList("Select Name from MarksheetTemplate where Owner IN (Select ID from app.AuthorisedUser Where UserName = 'SchoolAdmin@capita.co.uk')", "Name");
            //Asserting the Search Result count againt the count in the database
            if (TemplateName.Count() == 0)
                Assert.AreEqual("No Marksheet Templates Found", MTSPObj.GetMarksheetTemplateCount());
            else
                Assert.AreEqual(TemplateName.Count().ToString(), MTSPObj.GetMarksheetTemplateCount());

        }

        /// <summary>
        /// Story 7749 - 3. Search Marksheet Template by Owner
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "MTBOwner" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchMTByOwnerNoOwner()
        {

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Create Page Object for Marksheet Search Panel
            MarksheetTemplateSearchPanel MTSPObj = new MarksheetTemplateSearchPanel();
            //Open Marksheet Search Panel
            MTSPObj.OpenSearchMarksheetPanel();
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();

            //Verify if the required owners are present in the dropdown
            String OwnerName = MTSPObj.VerifyIteminDropdown("No Owner");
            Assert.AreEqual(OwnerName, "No Owner");

            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj.ClickOnSearch();
            List<string> TemplateName = new List<string>();
            //Getting the List of Marksheet Templates that are actually present in the database for the given user
            TemplateName = TestData.CreateDataList("Select distinct Name from MarksheetTemplate where Owner is null", "Name");
            //Asserting the Search Result count againt the count in the database
            if (TemplateName.Count() == 0)
                Assert.AreEqual("No Marksheet Templates Found", MTSPObj.GetMarksheetTemplateCount());
            else
                Assert.AreEqual(TemplateName.Count().ToString(), MTSPObj.GetMarksheetTemplateCount());


        }

        /// <summary>
        /// Story 7749 - 3. Search Marksheet Template by Owner
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, InvocationCount = 1, Groups = new[] { "Marksheet_Test", "Assessment CNR", "SearchMTByOwnerAnyTemplate" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchMTByOwnerAnyTemplate()
        {

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Create Page Object for Marksheet Search Panel
            MarksheetTemplateSearchPanel MTSPObj = new MarksheetTemplateSearchPanel();
            //Open Marksheet Search Panel
            MTSPObj.OpenSearchMarksheetPanel();
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();

            //Verify if the required owners are present in the dropdown
            String OwnerName = MTSPObj.VerifyIteminDropdown("Any Template");
            Assert.AreEqual(OwnerName, "Any Template");

            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj.ClickOnSearch();
            List<string> TemplateName = new List<string>();
            //Getting the List of Marksheet Templates that are actually present in the database for the given user
            TemplateName = TestData.CreateDataList("Select distinct Name from MarksheetTemplate", "Name");
            //Asserting the Search Result count againt the count in the database
            if (TemplateName.Count() == 0)
                Assert.AreEqual("No Marksheet Templates Found", MTSPObj.GetMarksheetTemplateCount());
            else
                Assert.AreEqual(TemplateName.Count().ToString(), MTSPObj.GetMarksheetTemplateCount());
        }

        /// <summary>
        /// Story 7749 - 4. Search Marksheet Template by Subject
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "SearchMTBySubject" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchMTBySubject()
        {

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Create Page Object for Marksheet Search Panel
            MarksheetTemplateSearchPanel MTSPObj = new MarksheetTemplateSearchPanel();
            bool SubjectFound = false;
            //Open Marksheet Search Panel
            MTSPObj.OpenSearchMarksheetPanel();
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();
            //Getting the List OF Year Groups
            List<string> subjectname = new List<string>();
            subjectname = TestData.CreateDataList("Select Name From AssessmentSubject ", "Name");
            //Selects the desired Class and Returns true in case the desired Class is found or else will return false
            SubjectFound = MTSPObj.SelectSubject(subjectname[14]);
            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj.ClickOnSearch();
            List<string> TemplateName = new List<string>();
            //Getting the List of Marksheet Templates that are actually present in the database for the given user
            TemplateName = TestData.CreateDataList("Select Name from MarksheetTemplate where id in (Select parent from MarksheetTemplateItem where id in (Select parent from MarksheetTemplateItem where id in (Select MarksheetTemplateItem from MarksheetTemplateColumn where ColumnDefinition in (Select id from ColumnDefinition where Aspect in (Select id from aspect where LearningProject in (Select ID from LearningActivity where AssessmentSubject in (Select id from AssessmentSubject Where Name Like '" + subjectname[14] + "')))))))", "Name");
            //Asserting the Search Result count againt the count in the database
            if (TemplateName.Count() == 0)
                Assert.AreEqual("No Marksheet Templates Found", MTSPObj.GetMarksheetTemplateCount());
            else
                Assert.AreEqual(TemplateName.Count().ToString(), MTSPObj.GetMarksheetTemplateCount());
        }

        /// <summary>
        /// Story 7761 - Dispaly Marksheet Template details for selected marksheet template
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "DisplayMarksheetTemplateDetails", "Assessment CNR", "EditMarksheetTemplate" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void DisplayMarksheetTemplateDetails()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, true);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Create Page Object for Marksheet Search Panel
            MarksheetTemplateSearchPanel MTSPObj = new MarksheetTemplateSearchPanel();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            //Randomly generate Unique Marksheet Name
            var MarksheetTemplateName = marksheetBuilder.RandomString(8);
            //Verifying the saved marksheet     
            marksheetBuilder.setMarksheetProperties(MarksheetTemplateName, "Description", true);
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(2);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();
            addAssessmentPeriod.AspectAssessmentPeriodSelection(2);

            List<string> selectedAssessmentperiod = addAssessmentPeriod.getAspectAssessmentPeriodSelectedItems();

            addAssessmentPeriod.ClickAspectAssessmentPeriodDone();

            AdditionalColumn additionalcolumn = marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();
            additionalcolumn.SelectNoOfAdditionalColumn(2);

            List<string> additionalColumnSelected = additionalcolumn.GetSelectedAdditionalColumnList();

            marksheetBuilder = additionalcolumn.ClickDoneButton();

            AddGroups addgroups = marksheetBuilder.NavigateGroups();
            addgroups.SelectNoOfYearGroups(1);
            List<string> selectedGroups = addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);
            marksheetBuilder = addgroups.ClickDoneButton();
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            marksheetBuilder = groupfilter.ClickDoneButton();

            marksheetBuilder.Save();
            marksheetBuilder.SaveMarksheetAssertionSuccess();

            //         string MarksheetTemplateName = marksheetName;
            //Open Marksheet Search Panel
            MTSPObj.OpenSearchMarksheetPanel();
            //Enter Marksheet Template Name to find that Template
            MTSPObj.EnterMarksheetTemplateName(MarksheetTemplateName);
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();
            //Selection of Active or In Active Templates :: If Active then pass "True" in the method or else pass "False"
            MTSPObj.IsActive(true);
            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj.ClickOnSearch();
            //Search again as it takes some time for the marksheet to appear after save
            MTSPObj.ClickOnSearch();
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(MarksheetTemplateName));
            Thread.Sleep(2000);
            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            //Check Marksheet Name and description populated are correct
            var getMarksheetTemplateName = marksheetBuilder.getMarksheetTemplateName();
            var getMarksheetTemplateDescription = marksheetBuilder.getMarksheetTemplateDescription();
            Assert.AreEqual(getMarksheetTemplateName, MarksheetTemplateName);
            Assert.AreEqual("Description", getMarksheetTemplateDescription);

            marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();
            List<string> additionalColumnRetention = additionalcolumn.GetSelectedAdditionalColumnList();

            //Check the earlier Assessment period selected are still retained on the screen
            int i = 0;
            foreach (String eachitem in additionalColumnSelected)
            {
                Assert.AreEqual(eachitem, additionalColumnRetention[i]);
                i++;
            }

            additionalcolumn.ClickDoneButton();

            i = 0;
            addgroups = marksheetBuilder.NavigateGroups();
            List<string> selectedGroupsRetained = addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);

            foreach (String eachitem in selectedGroups)
            {
                Assert.AreEqual(eachitem, selectedGroupsRetained[i]);
                i++;
            }
            marksheetBuilder = addgroups.ClickDoneButton();
            GroupFilters groupfilters = marksheetBuilder.NavigateAdditionalFilter();
            marksheetBuilder = groupfilters.ClickDoneButton();
            //Navigate to Assessment Aspect seelction and selct aspects
            addAssessments = marksheetBuilder.NavigateAssessments();
            addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(2);
            addAssessmentPeriod = addAspects.AspectNextButton();
            marksheetBuilder.clickPropertiesTab();

            //Get the current Selected Assessment period selected
            List<string> APselectedItems = new List<string>();
            APselectedItems = addAssessmentPeriod.getAspectAssessmentPeriodSelectedItems();

            i = 0;
            //Check the earlier Assessment period selected are still retained on the screen
            foreach (String eachitem in selectedAssessmentperiod)
            {
                Assert.AreEqual(eachitem, APselectedItems[i]);
                i++;
            }
        }

        /// <summary>
        /// Story 3242 - MCE - Edit marksheet template  properties
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "Marksheet_Test", "Assessment CNR", "EditMarksheetTemplateProperties" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void EditMarksheetTemplateProperties()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, true);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Create Page Object for Marksheet Search Panel
            MarksheetTemplateSearchPanel MTSPObj = new MarksheetTemplateSearchPanel();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            //Randomly generate Unique Marksheet Name
            var MarksheetTemplateName = marksheetBuilder.RandomString(8);
            //Verifying the saved marksheet     
            marksheetBuilder.setMarksheetProperties(MarksheetTemplateName, "Description", true);
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(1);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();
            addAssessmentPeriod.AspectAssessmentPeriodSelection(1);

            addAssessmentPeriod.ClickAspectAssessmentPeriodDone();

            AddGroups addgroups = marksheetBuilder.NavigateGroups();
            addgroups.SelectNoOfYearGroups(1);

            marksheetBuilder = addgroups.ClickDoneButton();
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            marksheetBuilder = groupfilter.ClickDoneButton();

            marksheetBuilder.Save();
            marksheetBuilder.SaveMarksheetAssertionSuccess();

            //         string MarksheetTemplateName = marksheetName;
            //Open Marksheet Search Panel
            MTSPObj = MTSPObj.OpenSearchMarksheetPanel();
            //Enter Marksheet Template Name to find that Template
            MTSPObj.EnterMarksheetTemplateName(MarksheetTemplateName);
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();
            //Selection of Active or In Active Templates :: If Active then pass "True" in the method or else pass "False"
            MTSPObj.IsActive(true);
            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj = MTSPObj.ClickOnSearch();
            //Search again as it takes some time for the marksheet to appear after save
            MTSPObj = MTSPObj.ClickOnSearch();
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(MarksheetTemplateName));
            Thread.Sleep(2000);

            marksheetBuilder.IsMarksheetPreviewColumnsPresent();

            var newname = "Amend Test" + DateTime.Now;
            marksheetBuilder.setMarksheetProperties(newname, "No", true);

            addgroups = marksheetBuilder.NavigateGroups();
            addgroups.SelectNoOfClasses(1);

            marksheetBuilder = addgroups.ClickDoneButton();
            groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            marksheetBuilder = groupfilter.ClickDoneButton();

            marksheetBuilder.Save();
            marksheetBuilder.SaveMarksheetAssertionSuccess();

            //Gettting the Create Marksheet Name 
            MarksheetTemplateDetails marksheetemplatedetails = new MarksheetTemplateDetails();
            List<string> MarksheetNameBeforeSearch = new List<string>();
            MarksheetNameBeforeSearch = marksheetemplatedetails.GetMarksheetName();

            //string MarksheetTemplateName = marksheetName;
            //Open Marksheet Search Panel
            MTSPObj = MTSPObj.OpenSearchMarksheetPanel();
            //Enter Marksheet Template Name to find that Template
            MTSPObj.EnterMarksheetTemplateName(newname);
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();
            //Selection of Active or In Active Templates :: If Active then pass "True" in the method or else pass "False"
            MTSPObj.IsActive(true);
            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj = MTSPObj.ClickOnSearch();
            //Search again as it takes some time for the marksheet to appear after save
            MTSPObj = MTSPObj.ClickOnSearch();
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(newname));
            Thread.Sleep(2000);

            List<string> MarksheetNameAfterSearch = new List<string>();
            MarksheetNameAfterSearch = marksheetemplatedetails.GetMarksheetName();

            //Verifying the Marksheet Name that we got before opening the marksheet in the edit mode
            foreach (string eachmarksheetname in MarksheetNameAfterSearch)
            {
                Assert.IsTrue(MarksheetNameBeforeSearch.Contains(eachmarksheetname));
            }


            //Open Marksheet Search Panel
            MTSPObj = MTSPObj.OpenSearchMarksheetPanel();
            //Enter Marksheet Template Name to find that Template
            MTSPObj.EnterMarksheetTemplateName(MarksheetTemplateName);
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();
            //Selection of Active or In Active Templates :: If Active then pass "True" in the method or else pass "False"
            MTSPObj.IsActive(true);
            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj = MTSPObj.ClickOnSearch();
            //Verify that the eatlier marksheet doesn't exists .i.e the new marksheet as replaced the earlier one
            Assert.AreEqual("No Marksheet Templates Found", MTSPObj.GetMarksheetTemplateCount());
        }


        /// <summary>
        /// Story 7716 - Display marksheet template details for selected marksheet template
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet_Test", "Assessment CNR", "SearchMTByTemplateName" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchMTByTemplateName()
        {

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Create Page Object for Marksheet Search Panel
            MarksheetTemplateSearchPanel MTSPObj = new MarksheetTemplateSearchPanel();
            //Getting the List OF Year Groups
            List<string> marksheettemplatename = new List<string>();
            marksheettemplatename = TestData.CreateDataList("Select Name From MarksheetTemplate", "Name");
            string MarksheetTemplateName = marksheettemplatename[0];
            //Open Marksheet Search Panel
            MTSPObj.OpenSearchMarksheetPanel();
            //Enter Marksheet Template Name to find that Template
            MTSPObj.EnterMarksheetTemplateName(MarksheetTemplateName);
            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();
            //Selection of Active or In Active Templates :: If Active then pass "True" in the method or else pass "False"
            MTSPObj.IsActive(true);
            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj.ClickOnSearch();
            List<string> TemplateName = new List<string>();
            //Getting the List of Marksheet Templates that are actually present in the database for the given user
            TemplateName = TestData.CreateDataList("Select Name From MarksheetTemplate Where Name Like '%" + MarksheetTemplateName + "%'", "Name");
            //Asserting the Search Result count againt the count in the database
            List<string> MarksheetTemplateNameResults = MTSPObj.TemplateResult();
            foreach (string eachresult in MarksheetTemplateNameResults)
            {
                //Verifying all the marksheet Template Name obtained in the Search Result against the data that is present in the database
                Assert.IsTrue(TemplateName.Contains(eachresult));
            }

        }

        /// <summary>
        /// Story 7749 - 6. Search Panel Hydration Logic
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "SearchPanelHydrationLogic", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SearchPanelHydrationLogic()
        {

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create Page Object for Marksheet Search Panel
            MarksheetTemplateSearchPanel MTSPObj = new MarksheetTemplateSearchPanel();

            string MarksheetTemplateCountBeforeWindowSwitching = "";
            string MarksheetTemplateCountAfterWindowSwitching = "";
            bool YearGroupFound = false;
            string MarksheetTemplateName = "Recording";

            //Open Marksheet Search Panel
            MTSPObj.OpenSearchMarksheetPanel();

            //Expands the Year Group Selection List
            MTSPObj.ExpandYearGroupList();

            //Selects the desired Year Group and Returns true in case the desired Year Group is found or else will return false
            YearGroupFound = MTSPObj.SelectYearGroup("Year 1");

            //Enter Marksheet Template Name to find that Template
            MTSPObj.EnterMarksheetTemplateName(MarksheetTemplateName);

            //Shows the Advance Search Options
            MTSPObj.OpenAdvanceSearchOptions();

            //Selection of Active or In Active Templates :: If Active then pass "True" in the method or else pass "False"
            MTSPObj.IsActive(true);

            //Clicks on the Marksheet Template Search Panel Search Button
            MTSPObj.ClickOnSearch();

            //Gets the Marksheet Template Count before Navigating away from the Marksheet Template Search Panel
            MarksheetTemplateCountBeforeWindowSwitching = MTSPObj.GetMarksheetTemplateCount();

            //Executing the Hydration Logic - That Navigates to the Pupil Record Screen and Comes back to the Marksheet Tamplate Screen and returns the Count of the Searched Templates again
            MTSPObj.HydrationLogic();

            //Gets the Marksheet Template Count after Navigating away from the marksheet template search panel and return back to the marksheet search screen
            MarksheetTemplateCountAfterWindowSwitching = MTSPObj.GetMarksheetTemplateCount();

            //Comparing the Marksheet Template Count before Navigation with the Marksheet Template Count After Navigation
            Assert.AreEqual(MarksheetTemplateCountBeforeWindowSwitching, MarksheetTemplateCountAfterWindowSwitching);
        }

        /// <summary>
        /// Story 7979 : For Aspect Search
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "AspectSelectionRetentionAfterSearch", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AspectSelectionRetentionAfterSearch()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            List<String> selectedAspectsList = addAspects.SelectNReturnSelectedAssessments(2);

            //String ResultText = "";

            //waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.AspectNextButton));

            ////Select a aspect
            //ReadOnlyCollection<IWebElement> aspectList = WebContext.WebDriver.FindElements(MarksheetConstants.AspectElementSelection);
            //foreach (IWebElement eachAspect in aspectList)
            //{
            //    if (eachAspect.Text != "")
            //    {
            //        waiter.Until(ExpectedConditions.ElementToBeClickable(eachAspect));
            //        eachAspect.Click();
            //        ResultText = eachAspect.Text;
            //        break;
            //    }
            //}

            addAspects.EnterAssessmentName(selectedAspectsList[1]);
            addAspects = addAspects.AspectSearch();


            addAspects.EnterAssessmentName("");
            addAspects = addAspects.AspectSearch();
            List<String> selectedAspect = addAspects.getSelectedAspectList();


            int i = 0;

            foreach (String eachitem in selectedAspect)
            {
                Assert.AreEqual(eachitem, selectedAspectsList[i]);
                i++;
            }
        }

        /// <summary>
        /// Story 7979 : For Aspect Assessment Period Search
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 8000, Groups = new[] { "AspectAssessmentPeriodSelectionRetentionAfterSearch", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void AspectAssessmentPeriodSelectionRetentionAfterSearch()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            //select an aspect and move to next slider
            addAspects.SelectNReturnSelectedAssessments(1);
            Thread.Sleep(2000);
            WebContext.WebDriver.FindElement(MarksheetConstants.AspectNextButton).Click();

            String ResultText = "";

            //Select a assessment period
            ReadOnlyCollection<IWebElement> aspectapList = WebContext.WebDriver.FindElements(MarksheetConstants.AspectAssessmentPeriodElementSelection);
            foreach (IWebElement eachAspect in aspectapList)
            {
                if (eachAspect.Text != "")
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(eachAspect));
                    eachAspect.Click();
                    ResultText = eachAspect.Text;
                    break;
                }
            }


            // Search through assessment period which is not selected above
            String itemSelection = "";

            foreach (IWebElement eachAssessmentPeriod in aspectapList)
            {
                if (ResultText != null && (eachAssessmentPeriod.Text != "" && ResultText != eachAssessmentPeriod.Text))
                {
                    itemSelection = eachAssessmentPeriod.Text;
                    WebContext.WebDriver.FindElement(MarksheetConstants.AspectAssessmentPeriodNameInput).SetText(itemSelection);
                    waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AssessmentPeriodSearchButton));
                    WebContext.WebDriver.FindElement(MarksheetConstants.AssessmentPeriodSearchButton).Click();
                    break;
                }
            }

            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.AssessmentPeriodNameInput));
            WebContext.WebDriver.FindElement(MarksheetConstants.AspectAssessmentPeriodNameInput).SetText("");
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AssessmentPeriodSearchButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.AssessmentPeriodSearchButton).Click();


            String assessmentPeriodRetention = "";
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.AspectAssessmentPeriodElementSelection, ResultText));



            //Verify the assessment period selected earlier is still retained
            ReadOnlyCollection<IWebElement> aspectapList1 = WebContext.WebDriver.FindElements(MarksheetConstants.AspectAssessmentPeriodElementSelection);

            foreach (IWebElement aspectElem in aspectapList1)
            {
                if (aspectElem.Text != "")
                {
                    String CheckSelction = aspectElem.GetAttribute("data-selected");
                    if (CheckSelction == "true")
                    {
                        assessmentPeriodRetention = aspectElem.Text;
                        break;
                    }
                }
            }
            Assert.AreEqual(ResultText, assessmentPeriodRetention);
        }

        /// <summary>
        /// Stroy 7186 : 1. Verifying the Marksheet Name when Marksheet Template Name is Present
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 8000, Groups = new[] { "VerifyMarksheetTemplateName", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void VerifyMarksheetTemplateName()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            AddGroups addgroups = new AddGroups();
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            marksheetBuilder.NavigateGroups();
            addgroups.SelectNoOfYearGroups(1);
            addgroups.SelectNoOfClasses(1);

            List<string> selectedGroups = addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);
            selectedGroups.AddRange(addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectClasses));

            marksheetBuilder = addgroups.ClickDoneButton();
            marksheettemplatedetails.SetMarksheetTemplateName("Test Template");
            List<string> MarksheetTemplateNameResults = marksheettemplatedetails.GetMarksheetName();

            MarksheetTemplateDetails mrkTemplateDetails = new MarksheetTemplateDetails();
            List<string> marksheetnames = mrkTemplateDetails.GetExpectedMarksheetNameList(mrkTemplateDetails.GetMarksheetTemplateName(), selectedGroups);
            Thread.Sleep(2000);
            foreach (string eachresult in MarksheetTemplateNameResults)
            {
                //Verifying the Marksheet template Count for the desired class
                Assert.IsTrue(marksheetnames.Contains(eachresult));
            }
        }

        /// <summary>
        /// Stroy 7706 : MCN - Validation on Save Marksheet
        /// 1. Save Validations on the Aspect Flow
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Save Validations", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveValidationAspectFlow()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Add Marksheet Template Name
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            string TemplateName = marksheetBuilder.RandomString(10);
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);
            AddAssessments addassessment = marksheetBuilder.NavigateAssessments();

            //Navigate to Assessment Selections
            AddAspects addaspects = addassessment.NavigateAssessmentsviaAssessment();
            SaveMarksheetTemplate savemarksheettemplate = new SaveMarksheetTemplate();
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            ValidationPopUpMessages validationpopupmessage = new ValidationPopUpMessages();

            //Assertion for the save validation without aspects selected
            Assert.AreEqual("At least one column should be present.", validationpopupmessage.GetInlineValidationMessageText());


            //Add Aspect and click save
            addaspects.SelectNReturnSelectedAssessments(1);
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            validationpopupmessage = validationpopupmessage.ValidationPopUpMessagePageObject();

            //Assertion for the save validation with only aspects selected
            Assert.AreEqual("Record is not saved for incomplete column definition kindly complete the marksheet builder wizard and click on done." +
                            "Invalid assessment period.", validationpopupmessage.GetInlineValidationMessageText().Replace("\r\n", ""));

            AddAssessmentPeriod addassessmentperiod = new AddAssessmentPeriod();
            addassessmentperiod = addaspects.AspectNextButton();
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();


            //Assertion for the save validation without assessment period selected
            Assert.AreEqual("Record is not saved for incomplete column definition kindly complete the marksheet builder wizard and click on done." +
                            "Invalid assessment period.", validationpopupmessage.GetInlineValidationMessageText().Replace("\r\n", ""));

            addassessmentperiod.AspectAssessmentPeriodSelection(1);
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();

            //Assertion for the save validation with only assessment period selected
            Assert.AreEqual("Record is not saved for incomplete column definition kindly complete the marksheet builder wizard and click on done.", validationpopupmessage.GetInlineValidationMessageText().Replace("\r\n", ""));


            //Verifying the Template Saved in Database
            List<string> SavedTemplateName = new List<string>();
            SavedTemplateName = TestData.CreateDataList("Select Name From MarksheetTemplate Where Name ='" + TemplateName + "'", "Name");
            Assert.IsFalse(SavedTemplateName.Contains(TemplateName));
        }

        /// <summary>
        /// Stroy 7706 : MCN - Validation on Save Marksheet
        /// 2. Save Validations on the Subject Flow
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Save Validations", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveValidationSubjectFlow()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Add Marksheet Template Name
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            string TemplateName = marksheetBuilder.RandomString(10);
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);
            AddAssessments addassessment = marksheetBuilder.NavigateAssessments();

            //Navigate to Assessment Selections
            AddSubjects addsubject = addassessment.NavigateAssessmentsviaSubject();
            SaveMarksheetTemplate savemarksheettemplate = new SaveMarksheetTemplate();
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            ValidationPopUpMessages validationpopupmessage = new ValidationPopUpMessages();

            //Assertion for the save validation without subject selected
            Assert.AreEqual("At least one column should be present.", validationpopupmessage.GetInlineValidationMessageText());

            //Add subject and click save
            addsubject.SelectSubjectResult(1);
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            validationpopupmessage = validationpopupmessage.ValidationPopUpMessagePageObject();

            //Assertion for the save validation with only Subjects selected
            Assert.AreEqual("Record is not saved for incomplete column definition kindly complete the marksheet builder wizard and click on done." +
                            "Invalid assessment period." +
                            "Invalid aspect for selected subject" +
                            "Select assessment mode, method, purpose and gradeset.", validationpopupmessage.GetInlineValidationMessageText().Replace("\r\n", ""));

            AddModeMethodPurpose addmodemethodpurpose = new AddModeMethodPurpose();
            addmodemethodpurpose = addsubject.SubjectNextButton();
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            validationpopupmessage = validationpopupmessage.ValidationPopUpMessagePageObject();

            //Assertion for the save validation without Mode Method & Purpose selected
            Assert.AreEqual("Record is not saved for incomplete column definition kindly complete the marksheet builder wizard and click on done." +
                            "Invalid assessment period." +
                            "Invalid aspect for selected subject" +
                            "Select assessment mode, method, purpose and gradeset.", validationpopupmessage.GetInlineValidationMessageText().Replace("\r\n", ""));

            addmodemethodpurpose.modeAssessmentPeriodSelection(1);
            addmodemethodpurpose.methodAssessmentPeriodSelection(1);
            addmodemethodpurpose.purposeAssessmentPeriodSelection(1);
            AddAssessmentPeriod addassessmentperiod = new AddAssessmentPeriod();
            addassessmentperiod = addmodemethodpurpose.modeMethodPurposeNextButton();
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            validationpopupmessage = validationpopupmessage.ValidationPopUpMessagePageObject();

            //Assertion for the save validation with Subject, Mode, Method, Purpose selected
            Assert.AreEqual("Record is not saved for incomplete column definition kindly complete the marksheet builder wizard and click on done." +
                            "Invalid assessment period." +
                            "Invalid aspect for selected subject" +
                            "Select assessment gradeset.", validationpopupmessage.GetInlineValidationMessageText().Replace("\r\n", ""));


            addassessmentperiod.subjectAssessmentPeriodSelection(1);
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            validationpopupmessage = validationpopupmessage.ValidationPopUpMessagePageObject();

            //Assertion for the save validation with only assessment period selected
            Assert.AreEqual("Record is not saved for incomplete column definition kindly complete the marksheet builder wizard and click on done." +
                            "Invalid aspect for selected subject" +
                            "Select assessment gradeset.", validationpopupmessage.GetInlineValidationMessageText().Replace("\r\n", ""));


            marksheetBuilder = addassessmentperiod.ClickSubjectAssessmentPeriodDone();
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            validationpopupmessage = validationpopupmessage.ValidationPopUpMessagePageObject();

            //Assertion for the save validation with No GradeSet Assigned to the column definition under save
            Assert.AreEqual("Invalid aspect for selected subject" +
                            "Select assessment gradeset.", validationpopupmessage.GetInlineValidationMessageText().Replace("\r\n", ""));


            //Verifying the Template Saved in Database
            List<string> SavedTemplateName = new List<string>();
            SavedTemplateName = TestData.CreateDataList("Select Name From MarksheetTemplate Where Name ='" + TemplateName + "'", "Name");
            Assert.IsFalse(SavedTemplateName.Contains(TemplateName));
        }

        /// <summary>
        /// Stroy 7706 : MCN - Validation on Save Marksheet
        /// 3. Save Validations on the Additional Column Flow
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Save Validations", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveValidationAdditionalColumnFlow()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Add Marksheet Template Name
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            string TemplateName = marksheetBuilder.RandomString(10);
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);

            //Navigate to Additional Columns
            AdditionalColumn additionalcolumn = marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();

            //Adding Additional Columns
            additionalcolumn.SelectNoOfAdditionalColumn(2);
            //Saving the Marksheet Template
            SaveMarksheetTemplate savemarksheettemplate = new SaveMarksheetTemplate();
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            ValidationPopUpMessages validationpopupmessage = new ValidationPopUpMessages();

            //Assertion for the save validation with only Additional Columns selected
            Assert.AreEqual("At least one column should be present.", validationpopupmessage.GetInlineValidationMessageText());

            marksheetBuilder = additionalcolumn.ClickDoneButton();
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            validationpopupmessage = validationpopupmessage.ValidationPopUpMessagePageObject();

            //Assertion for the save validation with only Additional Columns selected and after click of done button
            Assert.AreEqual("At least one column should be present.", validationpopupmessage.GetInlineValidationMessageText());


            //Verifying the Template Saved in Database
            List<string> SavedTemplateName = new List<string>();
            SavedTemplateName = TestData.CreateDataList("Select Name From MarksheetTemplate Where Name ='" + TemplateName + "'", "Name");
            Assert.IsFalse(SavedTemplateName.Contains(TemplateName));
        }

        /// <summary>
        /// Stroy 7706 : MCN - Validation on Save Marksheet
        /// 4. Save Validations on save of atleast one complete and an incomplete column definition
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "Save Validations", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveValidationWithComletedColumnDefinition()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Add Marksheet Template Name
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            string TemplateName = marksheetBuilder.RandomString(10);
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);
            AddAssessments addassessment = marksheetBuilder.NavigateAssessments();

            //Navigate to Assessment Selections
            AddAspects addaspects = addassessment.NavigateAssessmentsviaAssessment();

            //Adding an aspect or a complete column definition
            addaspects.SelectNReturnSelectedAssessments(1);
            AddAssessmentPeriod addassessmentperiod = addaspects.AspectNextButton();
            addassessmentperiod.AspectAssessmentPeriodSelection(1);
            marksheetBuilder = addassessmentperiod.ClickAspectAssessmentPeriodDone();

            //Adding incomplete column definition (Subject)
            addassessment = marksheetBuilder.NavigateAssessments();

            //Navigate to Assessment Selections
            AddSubjects addsubject = addassessment.NavigateAssessmentsviaSubject();
            addsubject.SelectSubjectResult(1);

            //Saving the Marksheet Template
            SaveMarksheetTemplate savemarksheettemplate = new SaveMarksheetTemplate();
            savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            ValidationPopUpMessages validationpopupmessage = new ValidationPopUpMessages();

            //Assertion for the save validation with only Additional Columns selected
            Assert.AreEqual("Record is not saved for incomplete column definition kindly complete the marksheet builder wizard and click on done." +
                            "Invalid aspect for selected subject" +
                            "Select assessment mode, method, purpose and gradeset.", validationpopupmessage.GetInlineValidationMessageText().Replace("\r\n", ""));

            //Verifying the Template Saved in Database
            List<string> SavedTemplateName = new List<string>();
            SavedTemplateName = TestData.CreateDataList("Select Name From MarksheetTemplate Where Name ='" + TemplateName + "'", "Name");
            Assert.IsFalse(SavedTemplateName.Contains(TemplateName));
        }



        /// <summary>
        /// Story 10544 : MCE - Save and Continue in Manage Template
        /// 1. Save & Continue behaviour verification on click of "Cancel" and "Dont Save"
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 600, Groups = new[] { "SaveNContinueOnCancel", "Assessment CNR", "Grid No Run" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveNContinueOnCancel()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Add Marksheet Template Name
            AddAssessments addassessment = marksheetBuilder.NavigateAssessments();

            //Navigate to Assessment Selections
            AddAspects addaspects = addassessment.NavigateAssessmentsviaAssessment();

            //Adding an aspect or a complete column definition
            addaspects.SelectNReturnSelectedAssessments(1);
            AddAssessmentPeriod addassessmentperiod = addaspects.AspectNextButton();

            //Adding an Assessment Period
            addassessmentperiod.AspectAssessmentPeriodSelection(1);
            marksheetBuilder = addassessmentperiod.ClickAspectAssessmentPeriodDone();

            SaveNContinuePage savencontinue = new SaveNContinuePage();

            //Navigating to the Pupil Record Screen
            savencontinue = savencontinue.NavigatetoPupilRecordScreen();

            //Choose the Cancel Option on the Save & Continue button
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            savencontinue.WaitUntilSaveNContinuePopUpOpens();
            savencontinue.ClickCancelButton();
            marksheettemplatedetails = marksheettemplatedetails.MarksheetTemplateDetailsPageObject();

            //Adding Marksheet Template Name
            string TemplateName = marksheetBuilder.RandomString(10);
            MarksheetTemplateProperties marksheettemplateproperties = new MarksheetTemplateProperties();
            marksheettemplatedetails = marksheettemplateproperties.OpenDetailsTab();
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);

            //Navigating to the Pupil Record Screen
            savencontinue = savencontinue.NavigatetoPupilRecordScreen();

            //Choose the Cancel Option on the Save & Continue button
            savencontinue.WaitUntilSaveNContinuePopUpOpens();
            savencontinue.ClickCancelButton();
            marksheettemplatedetails = marksheettemplatedetails.MarksheetTemplateDetailsPageObject();

            //Verifying the Template Saved in Database
            List<string> SavedTemplateName = new List<string>();
            SavedTemplateName = TestData.CreateDataList("Select Name From MarksheetTemplate Where Name ='" + TemplateName + "'", "Name");



            //Verifying IF we are still on the Marksheet Template Screen and the Marksheet Temlate Name entered by us is not changed
            //Assert Logic in case the user clicks on cancel button
            Assert.AreEqual(SavedTemplateName.Count(), 0);
            Assert.AreEqual(TemplateName, marksheettemplatedetails.GetMarksheetTemplateName());

        }

        /// <summary>
        /// Story 10544 : MCE - Save and Continue in Manage Template
        /// 1. Save & Continue behaviour verification on click of "Cancel" and "Dont Save"
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 600, Groups = new[] { "SaveNContinueOnDontSave", "Assessment CNR", "Grid No Run" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveNContinueOnDontSave()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Add Marksheet Template Name
            AddAssessments addassessment = marksheetBuilder.NavigateAssessments();

            //Navigate to Assessment Selections
            AddAspects addaspects = addassessment.NavigateAssessmentsviaAssessment();

            //Adding an aspect or a complete column definition
            addaspects.SelectNReturnSelectedAssessments(1);
            AddAssessmentPeriod addassessmentperiod = addaspects.AspectNextButton();

            //Adding an Assessment Period
            addassessmentperiod.AspectAssessmentPeriodSelection(1);
            marksheetBuilder = addassessmentperiod.ClickAspectAssessmentPeriodDone();

            SaveNContinuePage savencontinue = new SaveNContinuePage();

            //Choose the Cancel Option on the Save & Continue button
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();

            //Adding Marksheet Template Name
            string TemplateName = marksheetBuilder.RandomString(10);
            MarksheetTemplateProperties marksheettemplateproperties = new MarksheetTemplateProperties();
            marksheettemplatedetails = marksheettemplateproperties.OpenDetailsTab();
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);

            //Navigating to the Pupil Record Screen
            savencontinue = savencontinue.NavigatetoPupilRecordScreen();

            //Choose the Cancel Option on the Save & Continue button
            savencontinue.WaitUntilSaveNContinuePopUpOpens();
            savencontinue.ClickDontSaveButton();

            //wait until you are navigated to Pupil Record Screen
            By AddNewPupilButton = By.CssSelector("a[data-automation-id='add_new_pupil_button']");
            WaitUntilDisplayed(AddNewPupilButton);

            //Navigate back to Create Marksheet Template Screen
            marksheettemplatedetails = savencontinue.NavigatetoCreateMarksheetTemplateScreen();

            //Verifying the Template Saved in Database
            List<string> SavedTemplateName = new List<string>();
            SavedTemplateName = TestData.CreateDataList("Select Name From MarksheetTemplate Where Name ='" + TemplateName + "'", "Name");

            //Verifying IF we are still on the Marksheet Template Screen and the Marksheet Temlate Name entered by us is not changed
            //Assert Logic in case the user clicks on cancel button
            Assert.AreEqual(SavedTemplateName.Count(), 0);

        }

        /// <summary>
        /// Stroy 10544 : MCE - Save and Continue in Manage Template
        /// 2. Save & Continue behaviour verification on click of Save & Continue button
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 600, Groups = new[] { "SaveNContinueOnSave", "Assessment CNR", "Grid No Run" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveNContinueOnSave()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Adding Subject to the Marksheet Template
            AddAssessments addassessment = marksheetBuilder.NavigateAssessments();
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

            //Adding Marksheet Template Name
            MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            string TemplateName = marksheetBuilder.RandomString(10);
            MarksheetTemplateProperties marksheettemplateproperties = new MarksheetTemplateProperties();
            marksheettemplatedetails = marksheettemplateproperties.OpenDetailsTab();
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);

            //Assigning a Gradeset to a Subject
            marksheettemplateproperties = marksheettemplatedetails.OpenPropertiesTab();
            GradesetSearchPanel gradesetsearchpanel = marksheettemplateproperties.ClickAssignGradeSet("Language and Literacy");

            //Selecting a Gradeset
            gradesetsearchpanel = gradesetsearchpanel.Search(true);
            GradesetDataMaintenance gradesetdatamaintenance = gradesetsearchpanel.SelectGradesetByName("Marks 49-151");
            marksheettemplateproperties = gradesetdatamaintenance.ClickOkButton();

            SaveNContinuePage savencontinue = new SaveNContinuePage();

            //Navigating to the Pupil Record Screen
            savencontinue = savencontinue.NavigatetoPupilRecordScreen();

            //Choose the Cancel Option on the Save & Continue button
            savencontinue.WaitUntilSaveNContinuePopUpOpens();
            savencontinue.ClickSaveNContinueButton();

            //wait until you are navigated to Pupil Record Screen
            By AddNewPupilButton = By.CssSelector("a[data-automation-id='add_new_pupil_button']");
            WaitUntilDisplayed(AddNewPupilButton);

            //Navigate back to Create Marksheet Template Screen
            marksheettemplatedetails = savencontinue.NavigatetoCreateMarksheetTemplateScreen();

            //Verifying the Template Saved in Database
            List<string> SavedTemplateName = new List<string>();
            SavedTemplateName = TestData.CreateDataList("Select Name From MarksheetTemplate Where Name ='" + TemplateName + "'", "Name");

            //Verifying IF we are still on the Marksheet Template Screen and the Marksheet Temlate Name entered by us is not changed
            //Assert Logic in case the user clicks on cancel button
            Assert.AreEqual(SavedTemplateName.Count(), 1);
            Assert.AreEqual(TemplateName, marksheettemplatedetails.GetMarksheetTemplateName());
        }

        /// <summary>
        /// Stroy 10544 : MCE - Save and Continue in Manage Template
        /// 3. Save & Continue behaviour verification on click of close button
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 600, Groups = new[] { "SaveNContinueOnClose", "Assessment CNR", "Grid No Run" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SaveNContinueOnClose()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Add Marksheet Template Name

            AddAssessments addassessment = marksheetBuilder.NavigateAssessments();

            //Navigate to Assessment Selections
            AddAspects addaspects = addassessment.NavigateAssessmentsviaAssessment();

            //Adding an aspect or a complete column definition
            addaspects.SelectNReturnSelectedAssessments(1);
            AddAssessmentPeriod addassessmentperiod = addaspects.AspectNextButton();

            //Adding an Assessment Period to the selected Aspect
            addassessmentperiod.AspectAssessmentPeriodSelection(1);
            marksheetBuilder = addassessmentperiod.ClickAspectAssessmentPeriodDone();

            SaveNContinuePage savencontinue = new SaveNContinuePage();

            //Adding Marksheet Template Name
            MarksheetTemplateProperties marksheettemplateproperties = new MarksheetTemplateProperties();
            MarksheetTemplateDetails marksheettemplatedetails = marksheettemplateproperties.OpenDetailsTab();
            string TemplateName = marksheetBuilder.RandomString(10);
            marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);

            //Clicks the Close button
            savencontinue.ClickCreateMarksheetCloseButton();

            //Choose the Cancel Option on the Save & Continue button
            savencontinue.WaitUntilSaveNContinuePopUpOpens();
            savencontinue.ClickSaveNContinueButton();

            //Verifying the Template Saved in Database
            List<string> SavedTemplateName = new List<string>();
            SavedTemplateName = TestData.CreateDataList("Select Name From MarksheetTemplate Where Name ='" + TemplateName + "'", "Name");


            //Verifying IF we are still on the Marksheet Template Screen and the Marksheet Temlate Name entered by us is not changed
            //Assert Logic in case the user clicks on cancel button
            Assert.AreEqual(TemplateName, SavedTemplateName[0]);
        }

        /// <summary>
        /// Stroy 13559 : Creation of the column definition based on the selection of Mode and Method
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "ColumnDefBasedOnPurposeModeNMethod", "Assessment CNR", "Grid No Run" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void ColumnDefBasedOnPurposeModeNMethod()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, true);
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
            addSubjects.EnterSubjectSearchCriteria("Language and Literacy");
            addSubjects = addSubjects.ClickSubjectSearchButton();
            addSubjects.SelectSubjectResult(1);
            AddModeMethodPurpose addmodemethodpurpose = addSubjects.SubjectNextButton();

            //Scenario 1 : One Column Definition

            int modeSelected = 1, methodSected = 1, purposeSelected = 1;

            //Selecting a Mode Method and Purpose for that Subject
            addmodemethodpurpose.purposeAssessmentPeriodSelection(purposeSelected);
            addmodemethodpurpose.modeAssessmentPeriodSelection(modeSelected);
            addmodemethodpurpose.methodAssessmentPeriodSelection(methodSected);

            MarksheetTemplatePreview marksheettemplatepreview = new MarksheetTemplatePreview();

            int finalResult = modeSelected * methodSected * purposeSelected;
            //Assert Logic to Check the Number of columns created
            Assert.AreEqual(finalResult, marksheettemplatepreview.GetColumnCount());

            //Scenario 2 : Multiple Column Definition

            addSubjects = addmodemethodpurpose.modeMethodPurposeBackButton();
            addmodemethodpurpose = addSubjects.SubjectNextButton();

            modeSelected = 2; methodSected = 1; purposeSelected = 3;
            finalResult = modeSelected * methodSected * purposeSelected;

            //Selecting a Mode Method and Purpose for that Subject
            addmodemethodpurpose.purposeAssessmentPeriodSelection(purposeSelected);
            addmodemethodpurpose.modeAssessmentPeriodSelection(modeSelected);
            addmodemethodpurpose.methodAssessmentPeriodSelection(methodSected);

            //Assert Logic to Check the Number of columns created
            Assert.AreEqual(finalResult, marksheettemplatepreview.GetColumnCount());
            Thread.Sleep(3000);
            //Scenario 3 : No Column Definition

            addSubjects = addmodemethodpurpose.modeMethodPurposeBackButton();
            addmodemethodpurpose = addSubjects.SubjectNextButton();

            modeSelected = 3; methodSected = 2; purposeSelected = 1;
            finalResult = modeSelected * methodSected * purposeSelected;

            //Selecting a Mode Method and Purpose for that Subject
            addmodemethodpurpose.purposeAssessmentPeriodSelection(purposeSelected);
            addmodemethodpurpose.modeAssessmentPeriodSelection(modeSelected);
            addmodemethodpurpose.methodAssessmentPeriodSelection(methodSected);

            //Assert Logic to Check the Number of columns created
            Assert.AreEqual(finalResult, marksheettemplatepreview.GetColumnCount());
            Thread.Sleep(3000);
            //Scenario 4 : No Column Definition

            addSubjects = addmodemethodpurpose.modeMethodPurposeBackButton();
            addmodemethodpurpose = addSubjects.SubjectNextButton();

            modeSelected = 1; methodSected = 2; purposeSelected = 3;
            finalResult = modeSelected * methodSected * purposeSelected;

            //Selecting a Mode Method and Purpose for that Subject
            addmodemethodpurpose.purposeAssessmentPeriodSelection(purposeSelected);
            addmodemethodpurpose.modeAssessmentPeriodSelection(modeSelected);
            addmodemethodpurpose.methodAssessmentPeriodSelection(methodSected);

            //Assert Logic to Check the Number of columns created
            Assert.AreEqual(finalResult, marksheettemplatepreview.GetColumnCount());
            Thread.Sleep(3000);
        }


        /// <summary>
        /// Stroy 8105 : Hooking of the Assign gradeset in marksheet template properties
        /// Check the popover appears after click of Ok button in Assessment period through subject flow
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "DefaultSelectionOfModeMethod", "Assessment CNR", "Grid No Run" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void DefaultSelectionOfModeMethod()
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

            //Check default selction of mode and method for multiple selection of purpose
            int i = 0;
            int PurposeCount = 0;
            if (TestData.PurposeList.Count > 2)
            {
                PurposeCount = 2;
            }
            while (i < PurposeCount)
            {
                addmodemethodpurpose.purposeAssessmentPeriodSelectionByName(TestData.PurposeList[i]);

                //Assert the default mode selected
                List<String> selectedmodelist = addmodemethodpurpose.modeSelctedList();
                Assert.AreEqual(selectedmodelist.Count, 1);
                Assert.IsTrue(selectedmodelist.Contains(TestData.ModeList[0]));

                //Assert the default moethod selected
                List<String> selectedmethodlist = addmodemethodpurpose.methodSelctedList();
                Assert.AreEqual(selectedmethodlist.Count, 1);
                Assert.IsTrue(selectedmethodlist.Contains(TestData.MethodList[0]));
                i++;
                addSubjects = addmodemethodpurpose.modeMethodPurposeBackButton();

                // Check columns removed in preview
                marksheetBuilder.IsMarksheetPreviewColumnsNotPresent();

                addmodemethodpurpose = addSubjects.SubjectNextButton();

            }

        }

        /// <summary>
        /// Stroy 3254 : MCE - Clone  Marksheet Template (New from existing option)
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, Groups = new[] { "CloneMarksheet", "Assessment CNR", "Grid No Run" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void CloneMarksheet()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            ////Create page object of marksheet home
            //CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            //MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            ////Adding Marksheet Template Name
            //MarksheetTemplateDetails marksheettemplatedetails = new MarksheetTemplateDetails();
            //string TemplateName = marksheetBuilder.RandomString(10);
            //marksheettemplatedetails.SetMarksheetTemplateName(TemplateName);

            ////Adding Subject to the Marksheet Template
            //AddAssessments addassessment = marksheetBuilder.NavigateAssessments();
            //AddSubjects addSubjects = addassessment.NavigateAssessmentsviaSubject();

            ////Selecting a Subject
            //addSubjects.EnterSubjectSearchCriteria("Language and Literacy");
            //addSubjects = addSubjects.ClickSubjectSearchButton();
            //addSubjects.SelectSubjectResult(1);
            //AddModeMethodPurpose addmodemethodpurpose = addSubjects.SubjectNextButton();

            ////Scenario 1 : One Column Definition

            //int modeSelected = 1, methodSected = 1, purposeSelected = 1;

            ////Selecting a Mode Method and Purpose for that Subject
            //addmodemethodpurpose.purposeAssessmentPeriodSelection(purposeSelected);
            //addmodemethodpurpose.modeAssessmentPeriodSelection(modeSelected);
            //addmodemethodpurpose.methodAssessmentPeriodSelection(methodSected);

            //AddAssessmentPeriod addassessmentperiod = addmodemethodpurpose.modeMethodPurposeNextButton();

            //addassessmentperiod.subjectAssessmentPeriodSelection(1);
            //marksheetBuilder = addassessmentperiod.ClickSubjectAssessmentPeriodDone();

            ////Assigning a Gradeset to a Subject
            //MarksheetTemplateProperties marksheettemplateproperties = marksheettemplatedetails.OpenPropertiesTab();
            //GradesetSearchPanel gradesetsearchpanel = marksheettemplateproperties.ClickAssignGradeSet("Language and Literacy");

            ////Selecting a Gradeset
            //gradesetsearchpanel = gradesetsearchpanel.Search(true);
            //GradesetDataMaintenance gradesetdatamaintenance = gradesetsearchpanel.SelectGradesetByName("Marks 49-151");
            //marksheettemplateproperties = gradesetdatamaintenance.ClickOkButton();

            ////Adding a Aspect
            //addassessment = marksheetBuilder.NavigateAssessments();
            //AddAspects addaspects = addassessment.NavigateAssessmentsviaAssessment();
            //addaspects.SelectNReturnSelectedAssessments(1);
            //addassessmentperiod = addaspects.AspectNextButton();
            //marksheetBuilder = addassessmentperiod.ClickAspectAssessmentPeriodDone();

            ////Adding Additional Column
            //AdditionalColumn additionalcolumn = marksheetBuilder.NavigateAdditionalColumnsviaAdditionalColumn();
            //additionalcolumn.SelectNoOfAdditionalColumn(2);
            //marksheetBuilder = additionalcolumn.ClickDoneButton();


            ////Adding Groups

            //AddGroups addgroups = marksheetBuilder.NavigateGroups();
            //addgroups.SelectNoOfYearGroups(1);
            //addgroups.SelectNoOfClasses(1);

            //marksheetBuilder = addgroups.ClickDoneButton();
            //GroupFilters groupfilters = marksheetBuilder.NavigateAdditionalFilter();
            //marksheetBuilder = groupfilters.ClickDoneButton();

            //// Marksheet Preview

            //MarksheetTemplatePreview marksheettemplatepreview = new MarksheetTemplatePreview();
            //List<string> OldColumnHeaders = marksheettemplatepreview.GetColumnHeaders(1);
            //for (int i = 1; i < 4; i++)
            //{
            //    OldColumnHeaders.AddRange(marksheettemplatepreview.GetColumnHeaders(i + 1));
            //}

            ////Save Marksheet Template
            //SaveMarksheetTemplate savemarksheettemplate = new SaveMarksheetTemplate();
            //savemarksheettemplate = savemarksheettemplate.ClickSaveButton();
            //savemarksheettemplate.GetSaveSuccessMessage();

            //Create page object of Marksheet Template Search Panel
            MarksheetTemplateSearchPanel marksheettemplatesearchpanel = new MarksheetTemplateSearchPanel();

            marksheettemplatesearchpanel = marksheettemplatesearchpanel.OpenSearchMarksheetPanel();
            marksheettemplatesearchpanel = marksheettemplatesearchpanel.ClickOnSearch();

            //Getting the list of Marksheet Templates
            List<string> marksheettemplatelist = new List<string>();
            marksheettemplatelist = marksheettemplatesearchpanel.TemplateResult();

            //Select the first marksheet template
            MarksheetTemplateDetails marksheettemplatedetails = marksheettemplatesearchpanel.SelectMarksheetTemplate(marksheettemplatelist[1]);

            MarksheetTemplatePreview marksheettemplatepreview = new MarksheetTemplatePreview();
            List<string> OldColumnHeaders = marksheettemplatepreview.GetColumnHeaders(1);
            for (int i = 1; i < 4; i++)
            {
                OldColumnHeaders.AddRange(marksheettemplatepreview.GetColumnHeaders(i + 1));
            }

            //Clone Marksheet
            CloneMarksheetTemplate clonemarksheettemplate = new CloneMarksheetTemplate();
            clonemarksheettemplate = clonemarksheettemplate.ClickCloneButton();
            Assert.AreEqual(marksheettemplatelist[1] + " has been copied", clonemarksheettemplate.GetCloneSuccessMessage());

            marksheettemplatedetails = marksheettemplatedetails.MarksheetTemplateDetailsPageObject();

            //Verifying that the Marksheet Template Name is not copied as a part of clone
            Assert.AreEqual("", marksheettemplatedetails.GetMarksheetTemplateName());

            //Getting all the column definitions
            marksheettemplatepreview = marksheettemplatepreview.NewMarksheetTemplatePreviewPageObject();
            List<string> NewColumnHeaders = marksheettemplatepreview.GetColumnHeaders(1);
            for (int i = 1; i < 4; i++)
            {
                NewColumnHeaders.AddRange(marksheettemplatepreview.GetColumnHeaders(i + 1));
            }

            //Verifying that all the columns are present on the preview
            foreach (string eachstring in NewColumnHeaders)
            {
                Assert.IsTrue(OldColumnHeaders.Contains(eachstring));
            }
        }


        /// <summary>
        /// Story 8744 - MCN - Delete Marksheet Template
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "Marksheet", "Assessment CNR", "DeleteMarksheetTemplate" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void DeleteMarksheetTemplate()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Randomly generate Unique Marksheet Name
            var MarksheetTemplateName = marksheetBuilder.RandomString(8);
            //Verifying the saved marksheet     
            marksheetBuilder.setMarksheetProperties(MarksheetTemplateName, "Description", true);
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(2);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();
            addAssessmentPeriod.AspectAssessmentPeriodSelection(2);
            addAssessmentPeriod.ClickAspectAssessmentPeriodDone();
            AddGroups addgroups = marksheetBuilder.NavigateGroups();
            addgroups.SelectNoOfYearGroups(1);
            List<string> selectedGroups = addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);
            marksheetBuilder = addgroups.ClickDoneButton();

            marksheetBuilder.Save();
            marksheetBuilder.SaveMarksheetAssertionSuccess();

            //Add code to delete the marksheet template

            //CLick on delete button
            marksheetBuilder.DeleteMarksheetTemplateButton();

            //Check for confirmation message
            marksheetBuilder.WaitUntilSaveNContinuePopUpOpens();

            marksheetBuilder.ClickNoButton();

            MarksheetTemplateProperties marksheettemplateproperties = new MarksheetTemplateProperties();
            marksheettemplateproperties.OpenDetailsTab();

            Assert.AreEqual(marksheetBuilder.getMarksheetTemplateName(), MarksheetTemplateName);

            //CLick on delete button
            marksheetBuilder.DeleteMarksheetTemplateButton();

            marksheetBuilder.ClickYesButton();

            //Check for deleted message
            marksheetBuilder.DeleteMarksheetAssertionSuccess();

        }

        /// <summary>
        /// Story 13584 : MCE- Clone row within column properties of the marksheet Template(Technically New functionality need to implement)
        /// </summary>
        [TestMethod]
        [Ignore]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "CloneRowsinMarksheetPopup", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void CloneRowsinMarksheetPopup()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, true);
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
            AddAspects addAspects = addassessment.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(1);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();
            addAssessmentPeriod.AspectAssessmentPeriodSelection(1);
            addAssessmentPeriod.ClickAspectAssessmentPeriodDone();

            marksheetBuilder.SelectPropertiesTab();
            MarksheetTemplateProperties marksheetTemplateProperties = new MarksheetTemplateProperties();
            marksheetTemplateProperties.ClickCloneRowMarksheetProperties();

            List<String> marksheetRows = marksheetTemplateProperties.GetMarksheetTemplateRowsList();


            // click on clone icon in properties grid and check a new row with same details is copied


        }



    }
}
