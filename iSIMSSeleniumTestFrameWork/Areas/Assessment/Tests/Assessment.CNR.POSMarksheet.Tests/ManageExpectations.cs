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
using OpenQA.Selenium;
using Assessment.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Assessment.CNR.Other.Assessment.Screens.Tests
{
    [TestClass]
    public class ManageExpectations : BaseSeleniumComponents
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

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='manage_expectations_header_title']")]
        private OpenQA.Selenium.IWebElement ManageExpectationsTitle;

        private static By thresold = By.CssSelector("div[name='Expectations'] div[column='2'] div.webix_cell");
        private static By SetSchoolStatementExpectationslink = By.CssSelector("[data-automation-id='manage_statement_sub_menu_manage_expectations_details']");

        /// <summary>
        /// Story - 20759 - Manage Expectations
        /// Navigate to the Manage Expecattions Screen
        /// </summary>
        ///         
        /// 

        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Expectations", "Assessment CNR", "NavigateToManageStatements" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void NavigateToManageStatements()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            String[] featureList = { "Curriculum" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Statements");

            SeleniumHelper.WaitForElementClickableThenClick(SetSchoolStatementExpectationslink);
        }



        /// <summary>
        /// Story - 20761 - Display Statements Based on selection
        /// Display statements based on the search criteria selection
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Expectations", "Assessment CNR", "DisplayStatements" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DisplayPOSStatementsForSchoolExpectations()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            NavigateToManageStatements();
            
            ManageSchoolExpecations manageSchoolExpecations = new ManageSchoolExpecations();

            //Select a level
            manageSchoolExpecations.SelectGroup("Year 2");
            //Select a Subject
            manageSchoolExpecations.SelectSubject("English: Reading");
            //Select a Strand
            manageSchoolExpecations.SelectStrand("Comprehension");

            //Search for the statemenst based on the strand selected
            manageSchoolExpecations = manageSchoolExpecations.Search();

            MarksheetGridHelper.FindColumnByColumnName("Statement Name");
            MarksheetGridHelper.FindColumnByColumnName("Description");
            MarksheetGridHelper.FindColumnByColumnName("DFE Expected");
            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnNameForPOS("Threshold");

            columnList.First().Click();
            List<string> GradeDetails = new List<string>
            { "M"};
            for (int i = 0; i < GradeDetails.Count; i++)
            {
                MarksheetGridHelper.GetEditor().SendKeys(GradeDetails[i]);
                MarksheetGridHelper.PerformEnterKeyBehavior();

            }


        }

        /// <summary>
        /// Story - 3350 - (PrePOST) - Save school expectation across statements
        /// Set a thresold and save the school expectation
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Expectations", "Assessment CNR", "SaveSchoolExpectations" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SaveSchoolExpectations()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            NavigateToManageStatements();
            ManageSchoolExpecations manageSchoolExpecations = new ManageSchoolExpecations();

            //Select a level
            manageSchoolExpecations.SelectGroup("Year 2");
            //Select a Subject
            manageSchoolExpecations.SelectSubject("English: Reading");
            //Select a Strand
            manageSchoolExpecations.SelectStrand("Comprehension");

            //Search for the statemenst based on the strand selected
            manageSchoolExpecations = manageSchoolExpecations.Search();

            MarksheetGridHelper.FindColumnByColumnName("Statement Name");
            MarksheetGridHelper.FindColumnByColumnName("Description");
            MarksheetGridHelper.FindColumnByColumnName("DFE Expected");
            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnNameForPOS("Threshold");

            columnList.First().Click();
            List<string> GradeDetails = new List<string>
            { "S"};
            for (int i = 0; i < GradeDetails.Count; i++)
            {
                String presentValue = MarksheetGridHelper.GetEditor().GetValue();
                if (presentValue != "")
                    break;
                else
                {
                    MarksheetGridHelper.GetEditor().Clear();
                    MarksheetGridHelper.GetEditor().SendKeys(GradeDetails[i]);
                    MarksheetGridHelper.PerformEnterKeyBehavior();
                    manageSchoolExpecations.Save();
                    manageSchoolExpecations.waitforSavemessagetoAppear();

                }

            }

            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Programme of Study");
            //Search for a POS Marksheet
            POSSearchPannel possearchpanel = new POSSearchPannel();
            //Select a Group
            possearchpanel = possearchpanel.SelectGroup("Year 2");
            WaitUntillAjaxRequestCompleted();
            //Select a Subject
            possearchpanel = possearchpanel.SelectSubject("English: Reading");
            WaitUntillAjaxRequestCompleted();
            //Select a Strand
            possearchpanel = possearchpanel.SelectStrand("Comprehension");
 
            ////Select a Assessment Period
            //possearchpanel = possearchpanel.SelectAssessmentPeriod("Year 2 Autumn");
            ////Select a Year Group
            //possearchpanel = possearchpanel.OpenYearGroupSelectionDropdown("Year  2");
            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();
            Thread.Sleep(3000);

            //It checks whether a column by below name with School Expecattion exists
            List<IWebElement> columnListPOS1 = MarksheetGridHelper.FindCellsOfColumnByColumnNamePOSExpectation("En Compre S 2.01", "PoS & School Expectation");

        }
    }
}

