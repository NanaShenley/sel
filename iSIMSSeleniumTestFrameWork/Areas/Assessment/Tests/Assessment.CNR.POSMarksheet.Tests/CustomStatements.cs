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


namespace Assessment.CNR.Other.Assessment.Screens.Tests
{
    [TestClass]
    public class CustomStatements : BaseSeleniumComponents
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
        private static By CustomStatementslink = By.CssSelector("[data-automation-id='manage_statement_sub_menu_manage_StatementDescription_details']");

        /// <summary>
        /// Story - 16717 - Assessment:POS Customization - Define School specific description for Statements
        /// Display statements based on the search criteria selection
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Custom Statements", "Assessment CNR", "DisplayCustomStatements" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DisplayCustomStatements()
        {
            DisplayStatements();
        }

        /// <summary>
        /// Story - 3350 - (PrePOST) - Save school expectation across statements
        /// Set a thresold and save the school expectation
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Custom Statements", "Assessment CNR", "SaveCustomSchoolDescription" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SaveCustomSchoolDescription()
        {
            Customstatements customStatements = DisplayStatements();
            customStatements.Save();
            customStatements.waitforSavemessagetoAppear();
        }

        /// <summary>
        /// Story - 16720 - Assessment:Customization - Template with statements to display school customized statement descriptions
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Custom Statements", "Assessment CNR", "VerifyCustomSchoolDescription" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyCustomSchoolDescription()
        {
            NavigateToManageStatements();

            Customstatements customStatements = new Customstatements();

            //Select a level
            customStatements.SelectGroup("Year 2");
            //Select a Subject
            customStatements.SelectSubject("English: Reading");
            //Select a Strand
            customStatements.SelectStrand("Word Reading");

            //Search for the statemenst based on the strand selected
            customStatements = customStatements.Search();

            MarksheetGridHelper.FindColumnByColumnName("Name");
            MarksheetGridHelper.FindColumnByColumnName("Description");
            MarksheetGridHelper.FindColumnByColumnName("Custom Description");
            MarksheetGridHelper.FindColumnByColumnName("Use Custom Description");
            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnNameForPOS("Custom Description");

            string entertext = MarksheetGridHelper.GenerateRandomString(10);
            columnList.First().Click();
            MarksheetGridHelper.GetTextAreEditor().SendKeys(entertext);
            MarksheetGridHelper.PerformEnterKeyBehavior();

            List<IWebElement> useSchoolDescriptionlist = MarksheetGridHelper.FindCellsOfColumnByColumnNameForPOS("Use Custom Description");
            useSchoolDescriptionlist.First().Click();
            customStatements.UseSchoolDescriptionClick(true);
            customStatements.Save();
            customStatements.waitforSavemessagetoAppear();

            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Programme of Study");
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
            possearchpanel = possearchpanel.SelectStrand("Word Reading");

            //Click on Search Button
            POSDataMaintainanceScreen posdatamaintainance = possearchpanel.Search();

            IWebElement columnName = MarksheetGridHelper.FindColumnByColumnName("En Word Read S 2.01");

            string[] parts = columnName.Text.Split('\n');
            if (parts != null && parts[1] != null)
            {
                string[] columnDesc = parts[1].Split('\r');
                string textTocompare = entertext.Replace(" ", string.Empty).ToLower();

                Assert.IsTrue(columnDesc[0].Replace(" ", string.Empty).ToLower().Contains(textTocompare));
            }

        }

        public void NavigateToManageStatements()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            String[] featureList = { "Curriculum" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser);
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Statements");

            SeleniumHelper.WaitForElementClickableThenClick(CustomStatementslink);
        }

        private Customstatements DisplayStatements()
        {
            NavigateToManageStatements();

            Customstatements customStatements = new Customstatements();

            //Select a level
            customStatements.SelectGroup("Year 2");
            //Select a Subject
            customStatements.SelectSubject("English: Writing");
            //Select a Strand
            customStatements.SelectStrand("Handwriting & Presentation");

            //Search for the statemenst based on the strand selected
            customStatements = customStatements.Search();

            MarksheetGridHelper.FindColumnByColumnName("Name");
            MarksheetGridHelper.FindColumnByColumnName("Description");
            MarksheetGridHelper.FindColumnByColumnName("Custom Description");
            MarksheetGridHelper.FindColumnByColumnName("Use Custom Description");
            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnNameForPOS("Custom Description");

            string entertext = "learning the grammar for years 2  English Appendix 2";
            columnList.First().Click();
            MarksheetGridHelper.GetTextAreEditor().SendKeys(entertext);
            MarksheetGridHelper.PerformEnterKeyBehavior();

            List<IWebElement> useSchoolDescriptionlist = MarksheetGridHelper.FindCellsOfColumnByColumnNameForPOS("Use Custom Description");
            useSchoolDescriptionlist.First().Click();
            customStatements.UseSchoolDescriptionClick(true);
            return customStatements;
        }
    }
}

