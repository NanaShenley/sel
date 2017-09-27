using Assessment.Components;
using Assessment.Components.Common;
using Assessment.Components.PageObject;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.PageObjects;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using POM.Components.HomePages;
using TestSettings;
using WebDriverRunner.webdriver;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using System.Security.Principal;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace Assessment.DEA.Tests
{
    [TestClass]
    public class Assessment_DEA : BaseSeleniumComponents
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

        #region Group Membership

        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "GroupMembershipChange" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void GroupMembershipDefaultsToTodaysDate()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);

            WaitUntillAjaxRequestCompleted();
            string gmSelectedValue = detail.GroupMembershipSelectedValue();
            Assert.IsTrue(!string.IsNullOrEmpty(gmSelectedValue));
        }


        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AssessmentYearsShowMoreLess()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            detail.OpenAssessmentYear();
            WaitUntillAjaxRequestCompleted();
            detail.AssessmentYearShowMoreLess();
            Assert.IsTrue(string.Equals(detail.AssessmentYearSelectedValue().ToString(), "2015/2016"));
        }

        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "GroupMembershipChange" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void GroupMembershipChangeToStandardAssessmentYears()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            detail.OpenGroupMembership();
            WaitUntillAjaxRequestCompleted();
            detail.GroupMembershipEnterEffectiveDate("01/01/2012").GroupMembershipApply();
            detail.OpenAssessmentYear();
            WaitUntillAjaxRequestCompleted();
            Assert.IsTrue(string.Equals(detail.AssessmentYearSelectedValue().ToString(), "2011/2012"));
            WebContext.Screenshot();
        }

        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "GroupMembershipChange" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void GroupMembershipChangeToNonStandardAssessmentYears()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            detail.OpenGroupMembership();
            detail.GroupMembershipEnterEffectiveDate("01/01/2009").GroupMembershipApply();
            detail.OpenAssessmentYear();
            Assert.IsTrue(string.Equals(detail.AssessmentYearSelectedValue().ToString(), "2008/2009"));
            WebContext.Screenshot();
        }

        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "GroupMembershipChange" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void GroupMembershipChangeToNonExistingAssessmentYears()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            detail.OpenGroupMembership();
            detail.GroupMembershipEnterEffectiveDate("01/01/2024").GroupMembershipApply();
            WaitUntillAjaxRequestCompleted();
            detail.OpenAssessmentYear();
            Assert.IsTrue(string.Equals(detail.AssessmentYearSelectedValue().ToString(), "2015/2016"));
            //Assert.IsTrue(string.Equals(detail.GetValidationMessage().ToString(), "No corresponding assessment year found for the group membership dates 01/01/2024 to 02/01/2024, current assessment year is selected."));
            Assert.IsTrue(detail.GetValidationMessage().Contains("No corresponding assessment year found for the group membership dates"));
        }

        #endregion

        #region FormulaBar Previous/Next

        /// <summary>
        /// Test to be run specifically in Chrome, as they will fail in IE unless started on IE with nativeEvents attribute set to false.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "FormulaBar" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void FormulaBarEnterIntegerMovePreviousNext()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            List<IWebElement> selectableElements = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Terms Completed");
            if (selectableElements != null && selectableElements.Count() > 1)
            {
                //Select any cell which is not first or last
                selectableElements.First().Click();
                detail.EnterInFormulaBar("0").FormulaBarNextClick();
                detail.EnterInFormulaBar("1").FormulaBarNextClick();
                detail.EnterInFormulaBar("2").FormulaBarNextClick();
                detail.EnterInFormulaBar("0").FormulaBarNextClick();
                detail.EnterInFormulaBar("1").FormulaBarNextClick();
                detail.EnterInFormulaBar("2").FormulaBarNextClick();
                detail.EnterInFormulaBar("3").FormulaBarPreviousClick();
                detail.EnterInFormulaBar("4").FormulaBarPreviousClick();
                detail.EnterInFormulaBar("5").FormulaBarPreviousClick();
                detail.EnterInFormulaBar("3").FormulaBarPreviousClick();
                detail.EnterInFormulaBar("4").FormulaBarPreviousClick();
                detail.EnterInFormulaBar("5").FormulaBarPreviousClick();
                Assert.IsTrue(detail.GetFormulaBarText().Equals("0"));
            }
        }

        /// <summary>
        /// Test to be run specifically in Chrome, as they will fail in IE unless started on IE with nativeEvents attribute set to false.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "FormulaBarEnterGradeMovePreviousNext" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void FormulaBarEnterGradeMovePreviousNext()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.LoPYear4Marksheet);
            List<IWebElement> selectableElements = MarksheetGridHelper.FindCellsOfColumnByColumnName("KS1 Using ICT");
            if (selectableElements != null && selectableElements.Count() > 1)
            {
                //Select any cell which is not first or last
                selectableElements.First().Click();
                detail.EnterInFormulaBar("03").FormulaBarNextClick();
                detail.EnterInFormulaBar("02").FormulaBarNextClick();
                detail.EnterInFormulaBar("01").FormulaBarNextClick();
                detail.EnterInFormulaBar("E2").FormulaBarNextClick();
                detail.EnterInFormulaBar("E3").FormulaBarNextClick();
                detail.EnterInFormulaBar("E5").FormulaBarNextClick();
                detail.EnterInFormulaBar("E6").FormulaBarPreviousClick();
                detail.EnterInFormulaBar("E1").FormulaBarPreviousClick();
                detail.EnterInFormulaBar("QQ").FormulaBarPreviousClick();
                detail.EnterInFormulaBar("NR").FormulaBarPreviousClick();
                detail.EnterInFormulaBar("02").FormulaBarPreviousClick();
                detail.EnterInFormulaBar("03").FormulaBarPreviousClick();
                Assert.IsTrue(detail.GetFormulaBarText().Equals("03"));
            }
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "Singleview" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void FormulaBarPreviousNextDisabledOnFormulaBar()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.AnalysisEnglishYear2Marksheet);
            List<IWebElement> columnList = new List<IWebElement>();
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("Eng Stan Score");
            columnList.First().Click();

            for (int i = 0; i < 7; i++)
            {
                MarksheetGridHelper.PerformTabKeyBehavior();
                System.Threading.Thread.Sleep(1000);
            }

            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("Eng Diff 3-2");
            if (columnList != null && columnList.Count() > 1)
            {
                columnList[1].Click();
                Assert.IsTrue(detail.IsFormulaBarPreviousDisabled());
                Assert.IsTrue(detail.IsFormulaBarNextDisabled());
            }
        }

        #endregion

        #region Distribution Graph
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "DistributionGraphExportTest" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DistributionGraphExportTest()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.LoPYear4Marksheet);
            MarksheetGridHelper.OpenDistributionDetails("KS1 Using ICT", string.Empty);
            WaitForAndClick(BrowserDefaults.TimeOut, By.ClassName("highcharts-button"));
        }

        #endregion

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "AdditionalPupilDetail" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AdditionalPupilDetailTest()
        {
            MarsksheetPupilDetail dea = new MarsksheetPupilDetail(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            dea.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            AutomationSugar.WaitForAjaxCompletion();
            dea.PupilLink();
            WaitUntillAjaxRequestCompleted();
            dea.AssessmentNote();
            WaitUntillAjaxRequestCompleted();
            dea.AssessmentNoteTextArea("assessment note");
            WaitUntillAjaxRequestCompleted();
            dea.AssessmentNoteTextSave();
            dea.PupilLink();
            WaitUntillAjaxRequestCompleted();
            dea.ViewPupilLogNote();
        }

        #region Additional Columns

        public void AdditionalColumnDataTest(string additionalColumnText)
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            AdditionalColumns additionalColumn = detail.OpenAdditionalColumns();
            bool checkedAlready = additionalColumn.SelectAdditonalColumnCheckBox(additionalColumnText);
            additionalColumn.ClickOk();
        }

        //Test to verify additional columns work.
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "AdditionalColumnTests" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AdditionalColumnTests()
        {
            AdditionalColumnDataTest("Term of Birth");
        }

        #endregion

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "MarksValidationTest" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void MarksValidationTest()
        {
            MarksheetGridHelper.OpenMarksheet("Recording MIST Year 2 - Year 2");

            //Integer Column Validation Test
            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Age (YY/MM)");
            columnList.First().Click();
            MarksheetGridHelper.GetEditor().SendKeys("11");
            MarksheetGridHelper.PerformTabKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("1");
            MarksheetGridHelper.PerformTabKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("201");
            MarksheetGridHelper.PerformEnterKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("200");
            MarksheetGridHelper.PerformEnterKeyBehavior();
            //Age Column Validation Test
            MarksheetGridHelper.GetEditor().SendKeys("88/11");
            MarksheetGridHelper.PerformEnterKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("77/11");
            MarksheetGridHelper.PerformEnterKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("77/5");
            MarksheetGridHelper.PerformEnterKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("77/11");

        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "GradeTypeMarksValidationTest" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void GradeTypeMarksValidationTest()
        {

            MarksheetGridHelper.OpenMarksheet(MarksheetGridHelper.LoPYear4Marksheet);

            //Grade Column Validation Test
            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("KS1 Using Mathematics");
            columnList.First().Click();
            MarksheetGridHelper.GetEditor().SendKeys("03");
            MarksheetGridHelper.PerformEnterKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("QQ");
            MarksheetGridHelper.PerformEnterKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("E5");
            MarksheetGridHelper.PerformEnterKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("E4");
            MarksheetGridHelper.PerformEnterKeyBehavior();
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "CommentTypeMarksValidationTest" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CommentTypeMarksValidationTest()
        {
            MarksheetGridHelper.OpenMarksheet(MarksheetGridHelper.AnnualCommentsYear1Markshett);

            //Comment Column Validation
            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("Communication");
            columnList.First().Click();
            MarksheetGridHelper.GetEditor().SendKeys("This is a comment column");
            MarksheetGridHelper.PerformEnterKeyBehavior();
            MarksheetGridHelper.GetEditor().SendKeys("This is a second comment column");
            MarksheetGridHelper.PerformEnterKeyBehavior();
        }

        /// <summary>
        /// EYFS Templates
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "EYFSPOSMarksheetDisplay" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EYFSPOSMarksheetDisplay()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Programme of Study");
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Scheme
            possearchpanel = possearchpanel.SelectScheme("Early Years Foundation Stage");
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Early Years Foundation");
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("Physical Development");
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Moving and Handling");
            //Select a Assessment Period
            possearchpanel = possearchpanel.SelectAssessmentPeriod("Year R1 On Entry");
            //Select a Year Group
            possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  R");
            Thread.Sleep(1500);

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            Thread.Sleep(2000);
            //Verify if % POS Expectations Achieved Column is present on the Maintainance Screen
            Assert.IsTrue(posdatamaintainance.VerifyColumnPresent("PD Moving and Handling Overall"));
        }


        #region FormulaBar Entry
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "EnterAgeViaFormulaBarTest" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EnterAgeViaFormulaBarTest()
        {
            FormulaBarPage formulabardetail = new FormulaBarPage(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            formulabardetail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);

            WaitUntillAjaxRequestCompleted();

            formulabardetail.AddValue("MIST Age (YY/MM)\r\nYear 2 Annual", "01/02");

            //update cell
            formulabardetail.AddValue("MIST Age (YY/MM)\r\nYear 2 Annual", "09/10");

            //validate formula bar
            formulabardetail.AddValue("MIST Age (YY/MM)\r\nYear 2 Annual", "99/12");

            //multiple cell - updates only last cell
            formulabardetail.AddValueToColumn("MIST Age (YY/MM)\r\nYear 2 Annual", "99/10");
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "EnterIntegerViaFormulaBarTest1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EnterIntegerViaFormulaBarTest1()
        {
            FormulaBarPage formulabardetail = new FormulaBarPage(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            formulabardetail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            WaitUntillAjaxRequestCompleted();
            formulabardetail.AddValue("MIST Terms Completed\r\nYear 2 Annual", "2");

            //update cell
            formulabardetail.AddValue("MIST Terms Completed\r\nYear 2 Annual", "3");

            //validate formula bar
            formulabardetail.AddValue("MIST Terms Completed\r\nYear 2 Annual", "4");

            //multiple cell - updates only last cell
            formulabardetail.AddValueToColumn("MIST Terms Completed\r\nYear 2 Annual", "5");
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EnterGradeViaFormulaBarTest()
        {
            FormulaBarPage formulabardetail = new FormulaBarPage(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            formulabardetail.OpenMarksheet(MarksheetGridHelper.LoPYear4Marksheet);
            formulabardetail.AddValue("KS1 Using Mathematics", "01");

            //update cell
            formulabardetail.AddValue("KS1 Using Mathematics", "03");

            //validate formula bar
            formulabardetail.AddValue("KS1 Using Mathematics", "tt");

            //multiple cell - updates only last cell
            formulabardetail.AddValueToColumn("KS1 Using Mathematics", "QQ");
        }


        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EnterCommentViaFormulaBarTest()
        {
            FormulaBarPage formulabardetail = new FormulaBarPage(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            WaitUntillAjaxRequestCompleted();
            formulabardetail.OpenMarksheet(MarksheetGridHelper.AnnualCommentsYear1Markshett);
            WaitUntillAjaxRequestCompleted();
            formulabardetail.AddComment("Using Mathematics", "test comment", true);

            //update cell
            formulabardetail.AddComment("Using Mathematics", "update comment", false);

            //expand
            formulabardetail.ExpandCommantBar();
            formulabardetail.AddComment("Using Mathematics", "test comment test commenttest commenttest commenttest commenttest commenttest commenttest " +
                                    "commenttest commenttest commenttest commenttest commenttest commenttest commenttest commenttest " +
                                    "commenttest commenttest commenttest commenttest commenttest commenttest commenttest commenttest commenttest " +
                                    "commenttest commenttest commenttest commenttest commenttest commenttest commenttest commenttest commenttest " +
                                    "commenttest commenttest commenttest commenttest commenttest commenttest commenttest commenttest commenttest " +
                                    "commenttest commenttest commenttest commenttest commenttest commenttest commenttest commenttest " +
                                    "commenttest commenttest commenttest commenttest commenttest commenttest commenttest commenttest" +
                                    " commenttest commenttest commenttest commenttest commenttest commenttest commenttest" +
                                    " commenttest commenttest commenttest commenttest commenttest commenttest commenttest" +
                                    " commenttest commenttest commenttest commenttest commenttest commenttest commenttest" +
                                    " commenttest commenttest commenttest commenttest commenttest", true);

            //multiple cell - updates only last cell
            formulabardetail.AddCommentToColumn("Using Mathematics", "multiple cells");
        }

        #endregion

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "CheckEntryforAllColumns" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckEntryforAllColumns()
        {
            MarksheetGridHelper.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);

            string[] ageValues = { "77/11", "88/11", "99/11", "11/11", "22/10", "12/10" };
            var columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Age (YY/MM)");
            columnList.First().Click();
            MarksheetGridHelper.EnterValueinLoop(1, ageValues);

            string[] integerValues = { "3", "5", "6", "0", "1", "5" };
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Terms Completed");
            columnList.First().Click();
            MarksheetGridHelper.EnterValueinLoop(1, integerValues);

            for (int i = 0; i < 9; i++)
            {
                MarksheetGridHelper.PerformTabKeyBehavior();
                System.Threading.Thread.Sleep(1000);
            }

            string[] gradeValues = { "A", "AA", "WBA", "BA", "WBA", "A" };
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Reading Assessment");
            columnList.First().Click();
            MarksheetGridHelper.EnterValueinLoop(1, gradeValues);


            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Handwriting Assess");
            columnList.First().Click();
            MarksheetGridHelper.EnterValueinLoop(1, gradeValues);

            string[] columnValues = { "This is a comment column", "Second line in a column", "Third line in a column", "Fourth Line in a column", "Fifth Line in column", "Last Line in column" };
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Recommendations");
            columnList.First().Click();
            MarksheetGridHelper.EnterValueinLoop(1, columnValues);


        }

        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "Narrow" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ColumnNarrowFeatureTest()
        {
            string checkstete = "";
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            IJavaScriptExecutor js = WebContext.WebDriver as IJavaScriptExecutor;

            detail.ClickMarksheetExtendedDropDownIcon();

            checkstete = detail.CheckStateofColumn();

            if (checkstete == "Default")
            {
                detail.ClickCompactView();
            }
            else
            {
                detail.ClickDefaultView();
            }

            MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Terms Completed")[2].Click();
            MarksheetGridHelper.GetEditor().SendKeys("5");

            detail.ClickMarksheetExtendedDropDownIcon();
            checkstete = detail.CheckStateofColumn();

            if (checkstete == "Default")
            {
                detail.ClickCompactView();
            }
            else
            {
                detail.ClickDefaultView();
            }

            IWebElement elem = WebContext.WebDriver.FindElement(By.CssSelector(MarksheetGridHelper.FiltersButton));


            if (elem.Displayed)
            {
                elem.Click();
            }

            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);

            //Save Dialog Ok
            IWebElement savebutton = WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID("save_continue_commit_dialog"));

            if (savebutton.Displayed)
                savebutton.Click();

        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "CellNavigationOrientation" }, Browsers = new[] { BrowserDefaults.Ie })]
        public void CellNavigationOrientationTest()
        {
            bool isVerticalState = true;
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail(SeleniumHelper.iSIMSUserType.TestUser);
            //detail.OpenMarksheet(MarksheetGridHelper.FT0102201701); //Local
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet); //Server
            IJavaScriptExecutor js = WebContext.WebDriver as IJavaScriptExecutor;

            detail.ClickMarksheetCellNavigationDropdown();

            isVerticalState = detail.CheckStateofCellNavigation();

            if (isVerticalState)
            {
                detail.ClickHorizontalOrientation();
            }
            else
            {
                detail.ClickVerticalOrientation();
            }

            MarksheetGridHelper.FindCellsOfColumnByColumnName(string.Empty)[0].Click();
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);
            Thread.Sleep(5);
            MarksheetGridHelper.GetEditor().SendKeys(Keys.Return);

            detail.ClickMarksheetCellNavigationDropdown();
            isVerticalState = detail.CheckStateofCellNavigation();

            if (isVerticalState)
            {
                detail.ClickHorizontalOrientation();
            }
            else
            {
                detail.ClickVerticalOrientation();
            }

            MarksheetGridHelper.FindCellsOfColumnByColumnName(string.Empty)[0].Click();
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

        #region Marksheet Description

        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "MarksheetDescription" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void MarksheetDescription_Story2886_Story2887()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            detail.OpenMarksheet(MarksheetGridHelper.LoPYear4Marksheet);
            detail.ClickMarksheetDescriptionIcon();
            Assert.IsNotNull(detail._marksheetDescription);
        }

        #endregion

        #region column details
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "OpenColumnDetails" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void OpenColumnDetailsForAge()
        {
            ColumnDetails detail = new ColumnDetails();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);

            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector(detail.GetHearderMenuId("MIST Age (YY/MM)")));
            detail.OpenColumnDetailsPopover("MIST Age (YY/MM)");
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "OpenColumnDetails" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void OpenColumnDetailsForInteger()
        {
            ColumnDetails detail = new ColumnDetails();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);

            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector(detail.GetHearderMenuId("MIST Terms Completed")));
            detail.OpenColumnDetailsPopover("MIST Terms Completed");
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "OpenColumnDetails" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void OpenColumnDetailsForComment()
        {
            ColumnDetails detail = new ColumnDetails();
            detail.OpenMarksheet(MarksheetGridHelper.AnnualCommentsYear1Markshett);

            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector(detail.GetHearderMenuId("Communication")));
            detail.OpenColumnDetailsPopover("Communication");
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "OpenColumnDetails" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void OpenColumnDetailsForGrade()
        {
            ColumnDetails detail = new ColumnDetails();
            detail.OpenMarksheet(MarksheetGridHelper.LoPYear4Marksheet);
            AutomationSugar.WaitForAjaxCompletion();
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector(detail.GetHearderMenuId("KS1 Using ICT")));

            detail.OpenColumnDetailsPopover("KS1 Using ICT");
            AutomationSugar.WaitForAjaxCompletion();
            detail.ClosePopover();
        }
        #endregion

        #region result history

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "ViewEditResultHistoryTest" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ViewResultHistoryTest()
        {
            ResultHistory detail = new ResultHistory(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            detail.EnterValueInCell("1");
            detail.SaveMarksheet();
            detail.SaveMarksheet();
            detail.ClickCellMenu();
            detail.ViewResultHistory();
            detail.CloseResultHistory();
        }


        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ViewEditResultHistoryTest()
        {
            ResultHistory detail = new ResultHistory(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            detail.EnterValueInCell("1");
            detail.SaveMarksheet();
            detail.SaveMarksheet();
            detail.ClickCellMenu();
            detail.ViewResultHistory();

            List<IWebElement> historyrows = detail.GetResultelements();

            historyrows.FirstOrDefault().Clear();
            historyrows.FirstOrDefault().SendKeys("5");

            detail.ClickOkButton();

        }
        #endregion

        #region Search Criteria for Marksheet

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "UpdateSearchPanelWithClassGroupFilter" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void UpdateSearchPanelWithClassGroupFilter_Story8711()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            Assert.IsNotNull(detail._ClassGroupSearchElement);
            Assert.IsNotNull(detail._SearchCriteriaButtonElement);
            detail.ClickMarksheetClassGroupFilter().CheckClassGroup().ClickSearchCriteriaButton();
        }

        /// <summary>
        /// Story - 3006 - Add subject to Assessment Search Criteria and search for marksheets
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet", "SubjectSearchForMarksheet" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SubjectSearchForMarksheet()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail(SharedComponents.Helpers.SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            string BeforeSearch = detail.NumberofSearchResults;
            detail.ShowMoreFilters();
            detail.SubjectFilter = "English";
            detail.ClickSearchCriteriaButton();
            WaitUntillAjaxRequestCompleted();
            string AfterSearch = detail.NumberofSearchResults;
            Assert.IsFalse((String.Compare(BeforeSearch, AfterSearch, true) == 0));

        }

        #endregion

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "copycolumn" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CopyColumnFeatureTypeA()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Marksheets");
            CommonFunctions.GotToMarksheetMenu();
            CopyColumnDialog dialog = new CopyColumnDialog();
            dialog.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            List<IWebElement> columnList = new List<IWebElement>();

            string[] IntegerValues = new string[] { "3", "5", "6", "0", "1", "5" };
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Terms Completed");
            columnList.First().Click();
            MarksheetGridHelper.EnterValueinLoop(1, IntegerValues);
            dialog.OpenCopyColumnsPopover("MIST Terms Completed");
            dialog.SelectTargetColumn(false, "one");
            IntegerValues = new string[] { "1", "2", "3", "4", "5", "6" };
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Terms Completed");
            columnList.First().Click();
            MarksheetGridHelper.EnterValueinLoop(1, IntegerValues);
            WaitUntillAjaxRequestCompleted();
            dialog.OpenCopyColumnsPopover("MIST Terms Completed");
            WaitUntillAjaxRequestCompleted();
            dialog.SelectTargetColumn(true, "multiple");
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "copycolumn" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CopyColumnFeatureTypeB()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            CommonFunctions.GotToMarksheetMenu();
            WaitUntillAjaxRequestCompleted();
            CopyColumnDialog dialog = new CopyColumnDialog();
            List<IWebElement> columnList = new List<IWebElement>();

            dialog.OpenMarksheet(MarksheetGridHelper.LoPYear4Marksheet);
            WaitUntillAjaxRequestCompleted();
            string[] gradeValues = new string[] { "03", "01", "QQ", "E5", "E1", "NR" };
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("KS1 Using ICT");
            columnList.First().Click();
            MarksheetGridHelper.EnterValueinLoop(1, gradeValues);
            WaitUntillAjaxRequestCompleted();
            dialog.OpenCopyColumnsPopover("KS1 Using ICT");
            WaitUntillAjaxRequestCompleted();
            dialog.SelectTargetColumn(true, "multiple");
        }

        /// <summary>
        /// Story - 19566 - Single Column View
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SingleViewOfMarksheetAge()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);

            List<IWebElement> columnList = new List<IWebElement>();
            string[] ageValues = { "77/11", "88/11", "99/11", "11/11", "22/10", "12/10" };
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Age (YY/MM)");
            columnList.First().Click();
            MarksheetGridHelper.EnterValueinLoop(1, ageValues);
            WaitUntillAjaxRequestCompleted();

            //click on Single View
            detail.SingleViewClick();
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            Assert.IsTrue(allColumns.Count == 2);

            //Click on OverView 
            detail.OverviewClick();
            PageFactory.InitElements(WebContext.WebDriver, this);
            allColumns = MarksheetGridHelper.FindAllColumns();
            Assert.IsTrue(allColumns.Count > 2);
        }

        /// <summary>
        /// Story - 19566 - Single Column View
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SingleViewOfMarksheetInt()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);

            List<IWebElement> columnList = new List<IWebElement>();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Terms Completed");
            columnList.First().Click();


            //click on Single View
            detail.SingleViewClick();
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            Assert.IsTrue(allColumns.Count == 2);

            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Terms Completed");
            columnList.First().Click();
            MarksheetGridHelper.PerformEnterKeyBehavior();

            string[] IntegerValues = new string[] { "3", "5", "6", "2", "1", "5" };
            PageFactory.InitElements(WebContext.WebDriver, this);
            MarksheetGridHelper.EnterValueinLoop(1, IntegerValues);
            WaitUntillAjaxRequestCompleted();

            //Click on OverView 
            detail.OverviewClick();
            PageFactory.InitElements(WebContext.WebDriver, this);
            allColumns = MarksheetGridHelper.FindAllColumns();
            Assert.IsTrue(allColumns.Count > 2);
            WaitUntillAjaxRequestCompleted();

        }

        /// <summary>
        /// Story - 19566 - Single Column View
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SingleViewOfMarksheetGrade()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);

            List<IWebElement> columnList = new List<IWebElement>();
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Terms Completed");
            columnList.First().Click();

            for (int i = 0; i < 9; i++)
            {
                MarksheetGridHelper.PerformTabKeyBehavior();
                System.Threading.Thread.Sleep(1000);
            }

            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Reading Assessment");
            columnList.First().Click();

            //click on Single View
            detail.SingleViewClick();
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            Assert.IsTrue(allColumns.Count == 2);

            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Reading Assessment");
            columnList.First().Click();
            MarksheetGridHelper.PerformEnterKeyBehavior();

            string[] gradeValues = new string[] { "AA", "AA", "A", "BA", "WBA", "A" };
            MarksheetGridHelper.EnterValueinLoop(1, gradeValues);
            WaitUntillAjaxRequestCompleted();

            //Click on OverView 
            detail.OverviewClick();
            PageFactory.InitElements(WebContext.WebDriver, this);
            allColumns = MarksheetGridHelper.FindAllColumns();
            Assert.IsTrue(allColumns.Count > 2);

        }

        /// <summary>
        /// Story - 19566 - Single Column View
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "Marksheet" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SingleViewOfMarksheetComment()
        {
            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);

            List<IWebElement> columnList = new List<IWebElement>();
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Terms Completed");
            columnList.First().Click();

            for (int i = 0; i < 9; i++)
            {
                MarksheetGridHelper.PerformTabKeyBehavior();
                System.Threading.Thread.Sleep(1000);
            }


            //click on Single View
            detail.SingleViewClick();
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            Assert.IsTrue(allColumns.Count == 2);

            System.Threading.Thread.Sleep(1000);
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Recommendations");
            System.Threading.Thread.Sleep(1000);
            columnList.First().Click();
            MarksheetGridHelper.PerformEnterKeyBehavior();

            string[] columnValues = { "This is a comment column", "Second line in a column", "Third line in a column", "Fourth Line in a column", "Fifth Line in column", "Last Line in column" };
            MarksheetGridHelper.EnterValueinLoop(1, columnValues);
            WaitUntillAjaxRequestCompleted();

            //Click on OverView 
            detail.OverviewClick();
            PageFactory.InitElements(WebContext.WebDriver, this);
            allColumns = MarksheetGridHelper.FindAllColumns();
            Assert.IsTrue(allColumns.Count > 2);
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "copycolumn" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CopyColumnFeatureTypeC()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Marksheets");
            CommonFunctions.GotToMarksheetMenu();
            CopyColumnDialog dialog = new CopyColumnDialog();
            List<IWebElement> columnList = new List<IWebElement>();

            dialog.OpenMarksheet(MarksheetGridHelper.AnnualCommentsYear1Markshett);

            string[] columnValues = new string[] { "This is a comment column", "Second line in a column", "Third line in a column", "Fourth Line in a column", "Fifth Line in column", "Last Line in column" };
            columnList.Clear();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("Communication");
            columnList.First().Click();
            MarksheetGridHelper.EnterValueinLoop(1, columnValues);
            WaitUntillAjaxRequestCompleted();
            dialog.OpenCopyColumnsPopover("Communication");
            WaitUntillAjaxRequestCompleted();
            dialog.SelectTargetColumn(true, "multiple");

        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void MarksheetExportToExcellTest()
        {
            MarsksheetPupilDetail dea = new MarsksheetPupilDetail(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            dea.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            List<IWebElement> columnList = new List<IWebElement>();
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("MIST Age (YY/MM)");
            columnList.First().Click();
            dea.ExportMarksheet();
        }

        #region"Creeate Marksheet"

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "AssignTemplateGroupAndFilter" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AssignTemplateGroupAndFilter()
        {
            //Login
            //FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");

            WaitUntillAjaxRequestCompleted();

            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            AssignTemplateGroupAndFilter assignTemplateGroupAndFilter = (AssignTemplateGroupAndFilter)marksheetTypeMenuPage.MarksheetTypeSelection("Create Marksheet(s)");
            WaitUntillAjaxRequestCompleted();
            //assignTemplateGroupAndFilter.SelectResult();
            new CopyColumnDialog().OpenMarksheet("Annual Pupil Profile Comments Year 3");

            //wait for filter value execution to complete
            WaitUntillAjaxRequestCompleted();

            assignTemplateGroupAndFilter.AssigGroupClick();
            //WaitUntillAjaxRequestCompleted();
            var groupList = assignTemplateGroupAndFilter.AssignGroup();

            //assignTemplateGroupAndFilter = new AssignTemplateGroupAndFilter();
            int position = 1;
            assignTemplateGroupAndFilter.Save();
            WaitUntillAjaxRequestCompleted();
            string marksheetName = assignTemplateGroupAndFilter.GetValueFromMarksheetDetails(position);
            Assert.IsTrue(marksheetName.Contains(groupList[0]));
            assignTemplateGroupAndFilter.clickElementAtPosition(position);

            WaitUntillAjaxRequestCompleted();
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitUntilDisplayed(MarksheetConstants.MarksheetTitle);
            IWebElement marksheetEntryTitle = WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetTitle);
            Assert.IsTrue(marksheetName.Contains(marksheetEntryTitle.Text));
        }

        #endregion

        #region"Create Template"

        /// <summary>
        /// Story - 18433 - Add Pupil details.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "Add_Pupil_details" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Pupil_details()
        {
            //Login
            //FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");

            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();
            createMarksheet.FillTemplateDetails("First Template", "Template Description");
            WaitUntillAjaxRequestCompleted();
            createMarksheet.AddPupilInformation();
        }

        /// <summary>
        /// Story - 17926 - Add Predefined Assessment- Search.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "AddPredefinedAssessment" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Predefined_Assessment()
        {
            //Login
            //FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");

            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();
            createMarksheet.FillTemplateDetails("First Template", "Template Description");
            WaitUntillAjaxRequestCompleted();
            createMarksheet.AddPredefinedAssessment();
        }

        /// <summary>
        /// Story - 17842 - Adhoc - Predefined columns.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "Add_AdHocColumn" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_AdHocColumn()
        {
            //Login
            //FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();
            createMarksheet.FillTemplateDetails("First Template", "Template Description");
            createMarksheet.AddAdHoc();
        }

        /// <summary>
        /// Story - 17842 - Adhoc - Predefined columns.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "Add_AdHocColumn_With_Assessment_Period" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_AdHocColumnWithAssessment()
        {
            //Login
            //FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();
            createMarksheet.FillTemplateDetails("First Template", "Template Description");
            createMarksheet.AddAdHocWithAssessment();
        }

        /// <summary>
        /// Story - 22059 - View Template - Preview - Basic Implementation.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "MarksheetPreview" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void MarksheetPreview()
        {
            //Login
            //FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();
            createMarksheet.FillTemplateDetails("First Template", "Template Description");
            createMarksheet.AddPredefinedAssessment();
            WaitUntillAjaxRequestCompleted();
            System.Threading.Thread.Sleep(1000);
            createMarksheet.AddPupilInformation();
            WaitUntillAjaxRequestCompleted();
            createMarksheet = new CreateMarksheet();
            createMarksheet.Preview();
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "MarksheetSave" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SaveMarksheet()
        {
            //Login
            //FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "NewMarksheet" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WaitUntillAjaxRequestCompleted();
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");
            WaitUntillAjaxRequestCompleted();
            createMarksheet.FillTemplateDetails(createMarksheet.RandomString(5), "Template Description");
            createMarksheet.AddPredefinedAssessment();
            WaitUntillAjaxRequestCompleted();
            System.Threading.Thread.Sleep(1000);
            createMarksheet.AddPupilInformation();
            System.Threading.Thread.Sleep(1000);
            createMarksheet.AddAdHoc();
            System.Threading.Thread.Sleep(1000);
            createMarksheet.SaveTemplate();
            WaitUntillAjaxRequestCompleted();
            createMarksheet.goBack();
            WaitUntillAjaxRequestCompleted();
            createMarksheet.editColumn();
            createMarksheet.SaveTemplate();
            WaitUntillAjaxRequestCompleted();
        }


        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "SaveSearchTemplateWithKeywords" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SaveSearchTemplateWithKeywords()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");

            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            CreateMarksheet createMarksheet = (CreateMarksheet)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            CurriculumMarksheetMaintainance curriculummarksheetmaintainance = new CurriculumMarksheetMaintainance();
            string TemplateName = createMarksheet.RandomString(10);
            string TemplateDescription = createMarksheet.RandomString(10);
            string Keyword = "Keywords Test";
            string SearchKeyword = "Keywords";
            createMarksheet.FillTemplateDetails(TemplateName, TemplateDescription, Keyword);

            createMarksheet.AddPredefinedAssessment();
            WaitUntillAjaxRequestCompleted();
            //System.Threading.Thread.Sleep(1000);
            createMarksheet.SaveTemplate();
            WaitUntillAjaxRequestCompleted();

            // Edit Template Started
            createMarksheet = createMarksheet.CancelTemplate();
            createMarksheet = createMarksheet.CreateTemplate();
            EditMarksheetTemplate editTemplate = createMarksheet.ClickModifyExistingButton();
            editTemplate.SearchTemplateByKeywords(SearchKeyword);
            editTemplate.Search();

            editTemplate.SelectTemplateByName(TemplateName);
            editTemplate.OpenTemplate();

            string marksheetTemplateName = createMarksheet.getMarksheetTemplateName();
            Assert.AreEqual(TemplateName, marksheetTemplateName);

            string marksheetTemplateDescription = createMarksheet.getMarksheetTemplateDescription();
            Assert.AreEqual(TemplateDescription, marksheetTemplateDescription);

            string marksheetTemplateKeywords = createMarksheet.getMarksheetTemplateKeywords();
            Assert.AreEqual(Keyword, marksheetTemplateKeywords);

            String GetTemplateName = editTemplate.GetExistingTemplateNameInEditDialog(TemplateName);


            // Copy Template Started
            createMarksheet = createMarksheet.CancelTemplate();
            createMarksheet = createMarksheet.CreateTemplate();
            editTemplate = createMarksheet.ClickCopyFromExistingButton();
            Assert.IsTrue(editTemplate.IsNewFromExistingDialogVisible());
            editTemplate.SearchTemplateByKeywords(SearchKeyword);
            editTemplate.Search();
            Assert.IsTrue(editTemplate.IsSearchedMarksheetTemplatePresent(TemplateName));
            editTemplate.SelectTemplateByName(TemplateName);
            Assert.AreEqual(TemplateName, editTemplate.getMarksheetTemplateNameFromNewFromExistingDialog());
            editTemplate.SelectTemplate();

            Assert.AreEqual("Copy of " + TemplateName, createMarksheet.getMarksheetTemplateName());

            marksheetTemplateDescription = createMarksheet.getMarksheetTemplateDescription();
            Assert.AreEqual(TemplateDescription, marksheetTemplateDescription);

            marksheetTemplateKeywords = createMarksheet.getMarksheetTemplateKeywords();
            Assert.AreEqual(Keyword, marksheetTemplateKeywords);

            GetTemplateName = editTemplate.GetExistingTemplateNameInEditDialog(TemplateName);
        }



        #endregion

        #region Assessments Quick Links

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "AssessmentQuickLinks" },
            Browsers = new[] { BrowserDefaults.Chrome })]
        public void AssessmentQuickLinks_AssessmentCoordinator()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            // Check if coordinator quick links exist
            var assessmentQuickLinks = new AssessmentQuickLinks();
            assessmentQuickLinks.CheckIfAssessmentQuickLinkExists(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            // Wait for the ajax request to complete
            WaitUntillAjaxRequestCompleted();

            // Open Assessment Quick Links Dropdown for assessment coordinator
            assessmentQuickLinks.OpenAssessmentQuickLinksDropdown(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            // Wait for the ajax request to complete
            WaitUntillAjaxRequestCompleted();

            // Click on "Set Up Marksheets and Parental Reports" quick link
            assessmentQuickLinks.ClickAndVerifyManageTemplatesLink(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "AssessmentQuickLinks" },
            Browsers = new[] { BrowserDefaults.Chrome })]
        public void AssessmentQuickLinks_ClassTeacher()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher, false);

            // Check if coordinator quick links exist
            var assessmentQuickLinks = new AssessmentQuickLinks();
            assessmentQuickLinks.CheckIfAssessmentQuickLinkExists(SeleniumHelper.iSIMSUserType.ClassTeacher);

            // Wait for the ajax request to complete
            WaitUntillAjaxRequestCompleted();

            // Open Assessment Quick Links Dropdown for class teacher
            assessmentQuickLinks.OpenAssessmentQuickLinksDropdown(SeleniumHelper.iSIMSUserType.ClassTeacher);

            // Wait for the ajax request to complete
            WaitUntillAjaxRequestCompleted();

            // Click on "Set Up Marksheets and Parental Reports" quick link
            assessmentQuickLinks.ClickAndVerifyMyMarksheetsLink(SeleniumHelper.iSIMSUserType.ClassTeacher);
        }


        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "FilterHybridMarksheet" },
          Browsers = new[] { BrowserDefaults.Chrome })]
        public void FilterHybridMarksheet()
        {

            AssessmentMarksheetDetail detail = new AssessmentMarksheetDetail();
            detail.OpenMarksheet(MarksheetGridHelper.RecordingMistYear2Marksheet);
            detail.clickFilterbutton();
            detail.selectFilterOptions();
            detail.ClickAdditionalColumnDetails();
            detail.clickFilterbutton();
            detail.ClickAdditionalColumnSwitch();
            detail.ClickApplyFilter();
            detail.ClickClearAllFilter();
        }
        #endregion

        #region"Enter Historical Result"

        //For NI Region
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "EnterHistoricalResult_NI" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EnterHistoricalResult_For_NI()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Assessment Historical Results" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Enter Historical Results");

            WaitUntillAjaxRequestCompleted();

            //Create page object of gistorical Result home
            EnterHistoricalResult enterHistoricalResult = new EnterHistoricalResult();

            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByAcademicYear, "2015/2016");
            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByYearGroup, "Year 2");
            //SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByView, "Template");
            
            enterHistoricalResult.Search();

            CopyColumnDialog dialog = new CopyColumnDialog();
            dialog.OpenMarksheet("Analysis English Year on Year");
            List <IWebElement> columnList = new List<IWebElement>();

            string[] IntegerValues = new string[] { "50", "51", "52" };
                        
            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("Eng Stan Score");
            columnList.First().Click();

            MarksheetGridHelper.EnterValueinLoop(1, IntegerValues);

            IWebElement savebutton = WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID("save_continue_commit_dialog"));

            if (savebutton.Displayed)
                savebutton.Click();
        }

        //For England Region with Template View
        [TestMethod]
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "EnterHistoricalResult_Eng_Template" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EnterHistoricalResult_For_EnglandWithTemplate()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Assessment Historical Results" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Enter Historical Results");

            WaitUntillAjaxRequestCompleted();

            //Create page object of gistorical Result home
            EnterHistoricalResult enterHistoricalResult = new EnterHistoricalResult();

            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByAcademicYear, "2015/2016");
            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByYearGroup, "Y3");
            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByView, "Template");

            enterHistoricalResult.Search();

            CopyColumnDialog dialog = new CopyColumnDialog();
            dialog.OpenMarksheet("KS1 A. Teacher Assessments 2016");
            List<IWebElement> columnList = new List<IWebElement>();

            string[] GradeValues = new string[] { "GDS", "A", "D" };

            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("KS1 Reading TA");
            columnList.First().Click();

            MarksheetGridHelper.EnterValueinLoop(1, GradeValues);

            IWebElement savebutton = WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID("save_continue_commit_dialog"));

            if (savebutton.Displayed)
                savebutton.Click();
        }

        //For England Region with Scheme View
        [TestMethod]
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet", "EnterHistoricalResult_Eng_Scheme" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EnterHistoricalResult_For_EnglandWithScheme()
        {
            //Login
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Assessment Historical Results" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Enter Historical Results");

            WaitUntillAjaxRequestCompleted();

            //Create page object of gistorical Result home
            EnterHistoricalResult enterHistoricalResult = new EnterHistoricalResult();

            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByAcademicYear, "2015/2016");
            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByYearGroup, "Y3");
            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByView, "Scheme");
            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByPhase, "Year 4");
            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchBySubject, "Art & Design");
            SeleniumHelper.ChooseSelectorOption(enterHistoricalResult.SearchByStrand, "Art & Design");

            enterHistoricalResult.Search();
            
            List<IWebElement> columnList = new List<IWebElement>();

            string[] GradeValues = new string[] { "M", "S", "D" };

            columnList = MarksheetGridHelper.FindCellsOfColumnByColumnName("Fn Art Design KS2 01");
            columnList.First().Click();

            MarksheetGridHelper.EnterValueinLoop(1, GradeValues);

            IWebElement savebutton = WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID("save_continue_commit_dialog"));

            if (savebutton.Displayed)
                savebutton.Click();
        }
        #endregion
    }
}