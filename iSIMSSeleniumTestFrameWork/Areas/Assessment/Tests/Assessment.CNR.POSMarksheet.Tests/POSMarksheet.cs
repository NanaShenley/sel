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
using POM.Components.HomePages;


namespace Assessment.CNR.POSMarksheet.Tests
{
    [TestClass]
    public class POSMarksheet : BaseSeleniumComponents
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
        /// Story - 11084 - (PrePOST): Display POS Marksheet with relevant sections.
        /// </summary>
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Assessment CNR", "NavigateToPOSMarksheet", "P1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void NavigateToPOSMarksheet()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Programme of Study");
        }


        /// <summary>
        /// Story - 11084 - (PrePOST): Display POS Marksheet with relevant sections.
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Assessment CNR", "POSMarksheetDisplay", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void POSMarksheetDisplay()
        {
            NavigateToPOSMarksheet();
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 1");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 1 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  1");
            Thread.Sleep(1500);

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickSummarycolumnsToggleOption(true);
            //Verify if % POS Expectations Achieved Column is present on the Maintainance Screen
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("% of PoS Expectations Achieved"));
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("% of School Expectations Achieved"));

        }


        /// <summary>
        /// Quick link for POS
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Groups = new[] { "QuickLinkForPOS", "POS" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void QuickLinkForPOS()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            String[] featureList = { "Curriculum" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);

            var assessmentQuickLinks = new AssessmentQuickLinks();
            // Check if Assessment Quick links exists
            assessmentQuickLinks.CheckIfAssessmentQuickLinkExists(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            // Wait for the ajax request to complete
            WaitUntillAjaxRequestCompleted();

            // Open Assessment Quick Links Dropdown for class teacher
            assessmentQuickLinks.OpenAssessmentQuickLinksDropdown(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            assessmentQuickLinks.ClickAndVerifyPOSMarksheetLink(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            POSSearchPannel posSearchPannel = new POSSearchPannel();
            string PosTitle = posSearchPannel.GetPOSTitle();
            Assert.AreEqual(PosTitle, "Programme of Study Tracking");
        }

        /// <summary>
        /// (PrePOST) - Summative overview per subject and strands
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet Builder", "Assessment CNR", "POSMarksheetDisplaySummative", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void POSMarksheetDisplaySummative()
        {
            NavigateToPOSMarksheet();
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();

            //Select a View
            possearchpanel = possearchpanel.SelectView("Summative");
            //Select a Scheme
            possearchpanel = possearchpanel.SelectScheme("DFE National Curriculum");

            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 1");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 1 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  1");
            Thread.Sleep(1500);

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            //MarksheetGridHelper.FindCellsOfColumnByColumnName("En Read Comp Overall");
            List<string> POSMarksheetColumnNames = posdatamaintainance.GetAllMarksheetColumnNames();
            //Verify if % POS Expectations Achieved Column is present on the Maintainance Screen
            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Comp Overall"));
            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Comp Strengths"));
            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Comp Next Steps"));

        }

        /// <summary>
        /// Story - 11084 - (PrePOST): Display POS Marksheet with relevant sections.
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Assessment CNR", "POSMarksheetDisplayForSchoolAdmin", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void POSMarksheetDisplayForSchoolAdmin()
        {
            NavigateToPOSMarksheet();
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 1");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 1 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  1");
            Thread.Sleep(1500);

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickSummarycolumnsToggleOption(true);
            //Verify if % POS Expectations Achieved Column is present on the Maintainance Screen
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("% of PoS Expectations Achieved"));
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("% of School Expectations Achieved"));

        }

        /// <summary>
        /// Story - 11084 - (PrePOST): Display POS Marksheet with relevant sections.
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Assessment CNR", "POSMarksheetDisplayForClassTeacher", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void POSMarksheetDisplayForClassTeacher()
        {
            NavigateToPOSMarksheet();
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 1");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 1 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  1");
            Thread.Sleep(1500);

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickSummarycolumnsToggleOption(true);
            //Verify if % POS Expectations Achieved Column is present on the Maintainance Screen
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("% of PoS Expectations Achieved"));
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("% of School Expectations Achieved"));

        }


        /// <summary>
        /// Story - 11304 - (PrePOST) - Hiding Summary columns on the POST marksheet template
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Assessment CNR", "POSToogleFunctionality" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void POSToogleFunctionality()
        {
            NavigateToPOSMarksheet();
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 2");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 2 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  2");

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            //Verify Summative and Summary Columns are present on the screen
            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickSummarycolumnsToggleOption(true);
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("% of PoS Expectations Achieved"));
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("% of School Expectations Achieved"));

            //Verifying Toogle Functionality for Summative Columns
            posdatamaintainance = postoolbar.ClickSubjectSummativecolumnsToggleOption(true);

            postoolbar.ToggleButton.Click();

            String Columnname = MarksheetGridHelper.GetColumnName("En Read Overall");
            Assert.IsTrue(Columnname.StartsWith("En Read Overall"));
            Columnname = MarksheetGridHelper.GetColumnName("En Read Strengths");
            Assert.IsTrue(Columnname.StartsWith("En Read Strengths"));
            Columnname = MarksheetGridHelper.GetColumnName("En Read Next Steps");
            Assert.IsTrue(Columnname.StartsWith("En Read Next Steps"));

        }

        /// <summary>
        /// Story - 18376 - (PrePOST) - Toggle options on the Summative view for next steps / Strengths
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Assessment CNR", "ToogleOverallSummativeColumnsFunctionality"}, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ToogleOverallSummativeColumnsFunctionality()
        {
            NavigateToPOSMarksheet();

            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();

            possearchpanel = possearchpanel.SelectView("Summative");
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 2");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");

            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 2 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  2");

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();


            //Verifying Toogle Functionality for  Strength and Next steps Summative Columns
            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();

            //hide Strengths and Next steps summative columns
            posdatamaintainance = postoolbar.ClickHideStrengthNextStepsToggleOption(true);

            postoolbar.ToggleButton.Click();

            List<string> POSMarksheetColumnNames = posdatamaintainance.GetAllMarksheetColumnNames();

            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Overall"));
            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Comp Overall"));

            Assert.IsTrue(!POSMarksheetColumnNames.Contains("En Read Strengths"));
            Assert.IsTrue(!POSMarksheetColumnNames.Contains("En Read Next Steps"));

            Assert.IsTrue(!POSMarksheetColumnNames.Contains("En Read Word Strengths"));
            Assert.IsTrue(!POSMarksheetColumnNames.Contains("En Read Word Next Steps"));

            Assert.IsTrue(!POSMarksheetColumnNames.Contains("En Read Comp Strengths"));
            Assert.IsTrue(!POSMarksheetColumnNames.Contains("En Read Comp Next Steps"));
            
            //unhide Strengths and Next steps summative columns
            postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickHideStrengthNextStepsToggleOption(false);

            postoolbar.ToggleButton.Click();

            POSMarksheetColumnNames = posdatamaintainance.GetAllMarksheetColumnNames();

            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Overall"));
            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Comp Overall"));

            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Strengths"));
            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Next Steps"));

            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Word Strengths"));
            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Word Next Steps"));

            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Comp Strengths"));
            Assert.IsTrue(POSMarksheetColumnNames.Contains("En Read Comp Next Steps"));

        }

        /// <summary>
        /// Story - 11309 - Single View POS marksheet
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Assessment CNR", "POSMarksheetSingleViewDisplay" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void POSMarksheetSingleViewDisplay()
        {
            NavigateToPOSMarksheet();
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Programme of Study");

            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 2");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 2 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  2");

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            Thread.Sleep(3000);
            POSSingleView posSingleView = new POSSingleView();
            posSingleView.ClickSingleViewButton();
            Assert.IsTrue(posSingleView.VerifySummaryRowsSingleView("Number of Pupils"));
            Assert.IsTrue(posSingleView.VerifySummaryRowsSingleView("Number of Results"));
            posSingleView.ClickStatementsViewButton();

        }

        /// <summary>
        /// Stroy 11302 : (PrePOST) - Search columns\results based on the search criteria
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]

        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "VerifyPOSMarksheetColumnHeaderonSearch", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyPOSMarksheetColumnHeaderonSearch()
        {
            NavigateToPOSMarksheet();

            ////Search for a POS Marksheet
            //POSSearchPannel possearchpanel = new POSSearchPannel();
            //List<string> temp = new List<string>();
            //List<string> learninglevelcode = new List<string>();
            //List<string> SubjectCode = new List<string>();
            //List<string> SubjectName = new List<string>();
            //List<Guid> StrandSubjectTypeID = new List<Guid>();
            //learninglevelcode = TestData.CreateDataList("Select Code From LearningLevel order by Name desc", "Code");
            //StrandSubjectTypeID = TestData.CreateGuidList("Select ID From AssessmentSubjectType Where Name = 'Strand' and TenantID = '" + TestDefaults.Default.TenantId + "'", "ID");
            //SubjectCode = TestData.CreateDataList("Select * From AssessmentSubject Where ID IN (Select AssessmentSubject From LearningActivity Where ID IN (Select ID From LearningProject Where Code Like '%" + learninglevelcode[11] + "%')) And AssessmentSubjectType != '" + StrandSubjectTypeID[0] + "' Order By Code", "Code");
            //SubjectName = TestData.CreateDataList("Select * From AssessmentSubject Where ID IN (Select AssessmentSubject From LearningActivity Where ID IN (Select ID From LearningProject Where Code Like '%" + learninglevelcode[11] + "%')) And AssessmentSubjectType != '" + StrandSubjectTypeID[0] + "' Order By Name", "Name");
            ////Select a Group
            //temp = TestData.CreateDataList("Select Name From LearningLevel order by Name desc", "Name");
            //possearchpanel = possearchpanel.SelectGroup(temp[11]);
            ////Select a Subject
            //possearchpanel = possearchpanel.SelectSubject(learninglevelcode[11] + ":" + SubjectCode[0] + "-" + SubjectName[0]);
            ////Select a Strand
            //temp = new List<string>();
            //temp = TestData.CreateDataList("Select Name From AssessmentSubject Where Name Like '%" + SubjectName[0] + "%' And AssessmentSubjectType = '" + StrandSubjectTypeID[0] + "'", "Name");
            //List<string> StrandCode = new List<string>();
            //StrandCode = TestData.CreateDataList("Select Code From LearningActivity Where AssessmentSubject IN (Select ID From AssessmentSubject Where Name Like '%" + SubjectName[0] + "%' And AssessmentSubjectType = '" + StrandSubjectTypeID[0] + "') And Code Like '%" + learninglevelcode[11] + "%'", "");
            //possearchpanel = possearchpanel.SelectStrand(StrandCode[0] + "-" + temp[0]);
            ////Select a Assessment Period
            //temp = new List<string>();
            //temp = TestData.CreateDataList("Select Name From AssessmentPeriod", "Name");
            //possearchpanel = possearchpanel.SelectAssessmentPeriod(temp[0]);
            ////Select a Year Group
            //possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown();
            //temp = new List<string>();
            //temp = TestData.CreateDataList("Select FullName From YearGroup", "FullName");
            //possearchpanel.SelectYearGroup(temp[0]);

            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a View
            possearchpanel = possearchpanel.SelectView("Scheme");
            //Select a Scheme
            possearchpanel = possearchpanel.SelectScheme("DFE National Curriculum");
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 2");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 2 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  2");

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();

            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickNextYearStatementsToggleOption(false);
            posdatamaintainance = postoolbar.ClickPreviousYearStatementsToggleOption(false);
            postoolbar.ToggleButton.Click();
            //Get all the Marksheet Columns
            List<string> POSMarksheetColumnNames = posdatamaintainance.GetAllMarksheetColumnNames();
            //Expected Marksheet Columns
            List<string> ExpectedPOSMarksheetColumnNames = new List<string>();
            ExpectedPOSMarksheetColumnNames.Add("% of PoS Expectations Achieved");
            ExpectedPOSMarksheetColumnNames.Add("% of School Expectations Achieved");

            //ExpectedPOSMarksheetColumnNames = TestData.CreateDataList("Select Heading From MarksheetTemplateItem Where ID IN (Select MarksheetTemplateItem From MarksheetTemplateColumn Where ColumnDefinition IN (Select ID From ColumnDefinition Where Aspect IN (Select ID From Aspect Where LearningProject IN (Select ID From LearningActivity Where Code = '" + StrandCode[0] + "'))))", "");
            ExpectedPOSMarksheetColumnNames.AddRange(TestData.CreateDataList("Select Heading From MarksheetTemplateItem Where ID IN (Select MarksheetTemplateItem From MarksheetTemplateColumn Where ColumnDefinition IN (Select ID From ColumnDefinition Where Aspect IN (select id from aspect where LearningActivity in (select id from LearningActivity where code in (select code from Statement where id in(select Statement from SubjectLevelStatement where strand in(select id from Strand where name = 'Comprehension') and SubjectLevel in (select id from SubjectLevel where LearningLevel in (select id from LearningLevel where code = 'YR2' And TenantID = '" + TestDefaults.Default.TenantId + "')) )))))) ", "Heading"));

            List<String> POSColumnsdisplayed = new List<string>();

            foreach (String eachitem in POSMarksheetColumnNames)
            {
                POSColumnsdisplayed.Add(eachitem.Replace(" ", string.Empty).ToLower());
            }

            foreach (String eachitem in ExpectedPOSMarksheetColumnNames)
            {

                Assert.IsTrue(POSColumnsdisplayed.Contains(eachitem.Replace(" ", string.Empty).ToLower()));
            }

        }

        ///// <summary>
        ///// Stroy 11302 : (PrePOST) - Search columns\results based on the search criteria
        ///// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "SavePOSMarksheet", "Assessment CNR", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SavePOSMarksheet()
        {
            NavigateToPOSMarksheet();

            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 1");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 1 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  1");
            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            Thread.Sleep(8000);

            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();

            posdatamaintainance = postoolbar.ClickSubjectSummativecolumnsToggleOption(false);
            posdatamaintainance = postoolbar.ClickSummativecolumnsToggleOption(false);
            posdatamaintainance = postoolbar.ClickSummarycolumnsToggleOption(false);


            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("En Compre S 1.02");

            columnList.First().Click();
            List<string> GradeDetails = new List<string>
            { "M", "S", "D", "U"};
            for (int i = 0; i < GradeDetails.Count; i++)
            {
                MarksheetGridHelper.GetEditor().SendKeys(GradeDetails[i]);
                MarksheetGridHelper.PerformEnterKeyBehavior();

            }

            postoolbar.ClickSaveButton();
            postoolbar.waitforSavemessagetoAppear();

            //Reload the results again by clicking on search to verify results are persisted
            columnList.Clear();
            posdatamaintainance.SearchFilterButtonClick();
            possearchpanel.Search();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("En Compre S 1.02");
            columnList.First().Click();

            for (int i = 0; i < GradeDetails.Count; i++)
            {
                String getCellValue = MarksheetGridHelper.GetEditor().GetValue();
                Assert.AreEqual(getCellValue, GradeDetails[i]);
                MarksheetGridHelper.GetEditor().Clear();
                MarksheetGridHelper.PerformEnterKeyBehavior();
            }
            postoolbar.ClickSaveButton();
            postoolbar.waitforSavemessagetoAppear();

        }
        ///// <summary>
        ///// Stroy 18308 - (PrePOST) - Display subject summative columns in the PoS marskheet
        ///// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "SubjectSummativeColumns", "Assessment CNR", "GridFailed" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SubjectSummativeColumns()
        {
            NavigateToPOSMarksheet();

            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 1");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 1 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  1");
            //possearchpanel.SelectYearGroup("Year 1");
            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            Thread.Sleep(3000);

            //Verifying Toogle Functionality for Summative Columns
            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickSubjectSummativecolumnsToggleOption(true);
            //Close the toggle dialog
            postoolbar.ToggleButton.Click();
            String Columnname = MarksheetGridHelper.GetColumnName("En Read Overall");
            Assert.IsTrue(Columnname.StartsWith("En Read Overall"));
            Columnname = MarksheetGridHelper.GetColumnName("En Read Strengths");
            Assert.IsTrue(Columnname.StartsWith("En Read Strengths"));
            Columnname = MarksheetGridHelper.GetColumnName("En Read Next Steps");
            Assert.IsTrue(Columnname.StartsWith("En Read Next Steps"));

            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnNameForPOS("En Read Overall");

            columnList.First().Click();
            List<string> GradeDetails = new List<string>
            { "1M", "2S", "3D", "4U", "1E", "2M", "2S", "2D", "2M", "2S"};
            for (int i = 0; i < GradeDetails.Count; i++)
            {
                MarksheetGridHelper.GetEditor().SendKeys(GradeDetails[i]);
                MarksheetGridHelper.PerformEnterKeyBehavior();

            }

            postoolbar.ClickSaveButton();
            //    postoolbar.SaveMarksheetAssertionSuccess();
            Thread.Sleep(3000);
        }


        ///// <summary>
        ///// Stroy 18308 - (PrePOST) - Display subject summative columns in the PoS marskheet
        ///// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "NextYearStatements", "Assessment CNR", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void NextYearStatements()
        {
            NavigateToPOSMarksheet();
            //   SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            //         SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Programme of Study");

            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 2");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 2 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  2");
            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            Thread.Sleep(3000);

            //Verifying Toogle Functionality for Summative Columns
            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickNextYearStatementsToggleOption(true);

            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("En Compre S 34.01"));
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("En Compre S 2.01"));
        }

        ///// <summary>
        ///// Stroy 18308 - (PrePOST) - Display subject summative columns in the PoS marskheet
        ///// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "PreviousYearStatements", "Assessment CNR", "P1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void PreviousYearStatements()
        {
            NavigateToPOSMarksheet();
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 3");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 3 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  3");
            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            Thread.Sleep(3000);

            //Verifying Toogle Functionality for Summative Columns
            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickPreviousYearStatementsToggleOption(true);
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("En Handwrite S 2.01"));
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("En Handwrite S 34.01"));

            //    List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("En Handwrite N 2.01");

            //    columnList.First().Click();
            //    List<string> GradeDetails = new List<string>
            //    { "M", "S", "D", "U", "E"};
            //    for (int i = 0; i < GradeDetails.Count; i++)
            //    {
            //        MarksheetGridHelper.GetEditor().SendKeys(GradeDetails[i]);
            //        MarksheetGridHelper.PerformEnterKeyBehavior();

            //    }

            //    postoolbar.ClickSaveButton();
            //    //    postoolbar.SaveMarksheetAssertionSuccess();
            //    Thread.Sleep(3000);
            //}

        }

        /// <summary>
        /// Story 21530 :Assessment POS- Default to My Class
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Groups = new[] { "VerifyDefaultMyClassFunctionality", "Assessment CNR" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyDefaultMyClassFunctionality()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            String[] featureList = { "ClassPicker" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.ClassTeacher);
            
            var assessmentQuickLinks = new AssessmentQuickLinks();
            // Check if class teacher quick links exist
            assessmentQuickLinks.CheckIfAssessmentQuickLinkExists(SeleniumHelper.iSIMSUserType.ClassTeacher);

            // Wait for the ajax request to complete
            WaitUntillAjaxRequestCompleted();

            string selectedClass = assessmentQuickLinks.SelectFirstClassFromQuickLink();

            // Open Assessment Quick Links Dropdown for class teacher
            assessmentQuickLinks.OpenAssessmentQuickLinksDropdown(SeleniumHelper.iSIMSUserType.ClassTeacher);

            assessmentQuickLinks.ClickAndVerifyPOSMarksheetLink(SeleniumHelper.iSIMSUserType.ClassTeacher);

            POSSearchPannel possearchpanel = new POSSearchPannel();
            //verifies that the default class set in cohort is the one selected from Class picker
            Assert.IsTrue(possearchpanel.VerifySelectedClass(selectedClass));
        }

        /// <summary>
        /// Story - 11084 - (PrePOST): Display POS Marksheet with relevant sections.
        /// </summary>
        [Variant(Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Assessment CNR", "WalesPOSMarksheetDisplay", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void WalesPOSMarksheetDisplay()
        {
            NavigateToPOSMarksheet();
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 1");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("Literacy : Reading : Response & analysis");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Responding to what has been read");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 1 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  1");
            Thread.Sleep(1500);

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickSummarycolumnsToggleOption(true);

            //Verify if % POS Expectations Achieved Column is present on the Maintainance Screen
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("% of PoS Expectations Achieved"));
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("% of School Expectations Achieved"));

        }
        ///// <summary>
        ///// Stroy 11302 : (PrePOST) - Search columns\results based on the search criteria
        ///// </summary>
        [Variant(Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "WalesSavePOSMarksheet", "Assessment CNR", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void WalesSavePOSMarksheet()
        {
            NavigateToPOSMarksheet();
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 1");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("Literacy : Reading : Response & analysis");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Responding to what has been read");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 1 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  1");
            Thread.Sleep(1500);
            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            Thread.Sleep(8000);
            POSToolbar postoolbar = new POSToolbar();
            postoolbar = postoolbar.OpenToggleMenuList();
            posdatamaintainance = postoolbar.ClickSubjectSummativecolumnsToggleOption(false);
            posdatamaintainance = postoolbar.ClickSummativecolumnsToggleOption(false);
            posdatamaintainance = postoolbar.ClickSummarycolumnsToggleOption(false);
            //      posdatamaintainance.SelectMarksheetColumnName("Lit - Reading - Response and analysis 1.RA2");

            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("Lit - Reading - Response and analysis 1.RA2");

            columnList.First().Click();
            List<string> GradeDetails = new List<string>
            { "Y", "D", "N"};
            for (int i = 0; i < GradeDetails.Count; i++)
            {
                MarksheetGridHelper.GetEditor().SendKeys(GradeDetails[i]);
                MarksheetGridHelper.PerformEnterKeyBehavior();

            }

         //   POSToolbar postoolbar = new POSToolbar();
            postoolbar.ClickSaveButton();
            postoolbar.waitforSavemessagetoAppear();

            //Reload the results again by clicking on search to verify results are persisted
            columnList.Clear();
            posdatamaintainance.SearchFilterButtonClick();
            possearchpanel.Search();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("Lit - Reading - Response and analysis 1.RA2");
            columnList.First().Click();

            for (int i = 0; i < GradeDetails.Count; i++)
            {
                String getCellValue = MarksheetGridHelper.GetEditor().GetValue();
                Assert.AreEqual(getCellValue, GradeDetails[i]);
                MarksheetGridHelper.GetEditor().Clear();
                MarksheetGridHelper.PerformEnterKeyBehavior();
            }
            postoolbar.ClickSaveButton();
            postoolbar.waitforSavemessagetoAppear();

        }

        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Assessment CNR", "CellNavigationOrientation", "P2" }, Browsers = new[] { BrowserDefaults.Ie })]
        public void CellNavigationOrientationTest()
        {
            bool isVerticalState = true;

            NavigateToPOSMarksheet();
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 1");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 1 Autumn");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  1");
            Thread.Sleep(1500);

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            POSToolbar postoolbar = new POSToolbar();

            IJavaScriptExecutor js = WebContext.WebDriver as IJavaScriptExecutor;

            postoolbar.ClickPOSMarksheetCellNavigationDropdown();

            isVerticalState = postoolbar.CheckStateofCellNavigation();

            if (isVerticalState)
            {
                postoolbar.ClickHorizontalOrientation();
            }
            else
            {
                postoolbar.ClickVerticalOrientation();
            }

            MarksheetGridHelper.FindCellsOfColumnByColumnNameForPOS(string.Empty)[0].Click();
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);

            postoolbar.ClickPOSMarksheetCellNavigationDropdown();
            isVerticalState = postoolbar.CheckStateofCellNavigation();

            if (isVerticalState)
            {
                postoolbar.ClickHorizontalOrientation();
            }
            else
            {
                postoolbar.ClickVerticalOrientation();
            }

            //MarksheetGridHelper.FindCellsOfColumnByColumnName("2 Decimal Column")[2].Click();
            MarksheetGridHelper.FindCellsOfColumnByColumnNameForPOS(string.Empty)[0].Click();
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
        }

    }
}