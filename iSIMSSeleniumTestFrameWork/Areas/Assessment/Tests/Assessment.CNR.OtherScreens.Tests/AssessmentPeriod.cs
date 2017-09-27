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
    public class AssessmentPeriod : BaseSeleniumComponents
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

        private static By AssessmentPeriodLink = By.CssSelector("[data-automation-id='manage_assessmentperiod_sub_menu_manage_assessmentperiods_details']");
        private static By AssessmentPeriodDateRangeLink = By.CssSelector("[data-automation-id='manage_assessmentperiod_sub_menu_assessmentperioddaterange_details']");


        #endregion

        /// <summary>
        /// Story - 3309 - MCP - Create New UI for Assessment period lookup screen
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "SearchAssessmentPeriodLookup", "Assessment CNR", "SearchAssessmentPeriodLookup" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SearchAssessmentPeriodLookup()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Task", "Assessment", "Manage Assessment Periods");

            //Create page object of marksheet Assessment Period Lookup Search Panel
            AssessmentPeriodLookupSearchPanel assessmentPeriodLookupSearchPanel = new AssessmentPeriodLookupSearchPanel();
            SeleniumHelper.WaitForElementClickableThenClick(AssessmentPeriodLink);

            //Click on Search Button
            AssessmentPeriodLookupDataMaintainanceScreen assessmentperiodlookupdatamaintainancescreen = assessmentPeriodLookupSearchPanel.Search();

            //List<string> AssessmentPeriodNameList = new List<string>();
            //AssessmentPeriodNameList = TestData.CreateDataList("Select Name From AssessmentPeriod Where ResourceProvider IN ('" + TestData.GetSchoolID() + "','" + TestData.GetCAPITASIMSIDByTenantId() + "') and TenantId ='" + TestDefaults.Default.TenantId + "'", "Name");
            //IJavaScriptExecutor js = WebContext.WebDriver as IJavaScriptExecutor;
            ////object gridLength = (int) js.ExecuteScript("var grid = $$('cxgridAssessmentPeriods'); return grid.config.data.length;"); 

            //Assert.AreEqual((Int64)AssessmentPeriodNameList.Count, js.ExecuteScript("var grid = $$('cxgridAssessmentPeriods'); return grid.config.data.length;"));
            ////Assert.AreEqual(AssessmentPeriodNameList.Count, assessmentperiodlookupdatamaintainancescreen.GetAllValuesForAColumn(2).Count);

          //  assessmentperiodlookupdatamaintainancescreen = assessmentPeriodLookupSearchPanel.FilterClick();

            //enter search criteria Assessment Period name as "annual"
            assessmentPeriodLookupSearchPanel.SetAssessmentPeriodName("Annual");
            assessmentperiodlookupdatamaintainancescreen = assessmentPeriodLookupSearchPanel.Search();

            ////Get the actual number of Assessment Period present in the database
            //AssessmentPeriodNameList = TestData.CreateDataList("Select Name From AssessmentPeriod Where Name Like '%annual%' AND ResourceProvider IN ('" + TestData.GetSchoolID() + "','" + TestData.GetCAPITASIMSIDByTenantId() + "') and TenantId ='" + TestDefaults.Default.TenantId + "'", "Name");
            ////Asserting the database number with the number of AP displayed on the UI
            //Assert.AreEqual((Int64)AssessmentPeriodNameList.Count, js.ExecuteScript("var grid = $$('cxgridAssessmentPeriods'); return grid.config.data.length;"));
        }


        /// <summary>
        /// Story - 3309 - MCP - Create New UI for Assessment period lookup screen
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 60000, Groups = new[] { "AssociateAssessmentPeriodDateRange", "Assessment CNR", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AssociateAssessmentPeriodDateRange()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Task", "Assessment", "Manage Assessment Periods");

            //Create page object of marksheet Assessment Period Lookup Search Panel
            AssessmentPeriodLookupSearchPanel assessmentPeriodLookupSearchPanel = new AssessmentPeriodLookupSearchPanel();
            SeleniumHelper.WaitForElementClickableThenClick(AssessmentPeriodDateRangeLink);
            assessmentPeriodLookupSearchPanel =  assessmentPeriodLookupSearchPanel.SelectAcademicYear("Academic Year 2016/2017");
            //Click on Search Button
            AssessmentPeriodLookupDataMaintainanceScreen assessmentperiodlookupdatamaintainancescreen = assessmentPeriodLookupSearchPanel.Search();
        }

        /// <summary>
        /// Story - 12422 - MCP - Reseting the Reference Day, Reference Month for Assessment period
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, TimeoutSeconds = 6000, Groups = new[] { "Marksheet Builder", "Assessment CNR", "ResetReferenceDateforAP", "Grid No Run", "EndToEnd" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ResetReferenceDateforAP()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path            
            AutomationSugar.NavigateMenu("Task", "Assessment", "Manage Assessment Periods");
            SeleniumHelper.WaitForElementClickableThenClick(AssessmentPeriodLink);
            Random random = new Random();
            //Create page object of marksheet Assessment Period Lookup Search Panel
            AssessmentPeriodLookupSearchPanel assessmentPeriodLookupSearchPanel = new AssessmentPeriodLookupSearchPanel();
            //Click on Search Button
            AssessmentPeriodLookupDataMaintainanceScreen assessmentperiodlookupdatamaintainancescreen = assessmentPeriodLookupSearchPanel.Search();

            string RefDateDay = "";
            RefDateDay = random.Next(1, 28).ToString();
            string RefDateMonth = "";
            RefDateMonth = random.Next(1, 12).ToString();

            //Setting Reference Date Day & Month
            assessmentperiodlookupdatamaintainancescreen = assessmentperiodlookupdatamaintainancescreen.SetCellValue(5, 1, RefDateDay);
            assessmentperiodlookupdatamaintainancescreen = assessmentperiodlookupdatamaintainancescreen.SetCellValue(6, 1, RefDateMonth);

            //Saving the Values
            assessmentperiodlookupdatamaintainancescreen = assessmentperiodlookupdatamaintainancescreen.ClickSaveButton();

            //Refreshing the screen
            assessmentperiodlookupdatamaintainancescreen = assessmentPeriodLookupSearchPanel.Search();

            //Getting the details for the newly created row
            //Assert.AreEqual(RefDateDay, assessmentperiodlookupdatamaintainancescreen.GetCellValue(5, 1));
            //Assert.AreEqual(RefDateMonth, assessmentperiodlookupdatamaintainancescreen.GetCellValue(6, 1));

            //Reseting the dates to default values
            assessmentperiodlookupdatamaintainancescreen = assessmentperiodlookupdatamaintainancescreen.ClickResetReferenceDateButton();

            List<int> defaultreferencedateday = TestData.CreateIntegerList("Select DefaultReferenceDateDay From AssessmentPeriod Where Name Like'%" + assessmentperiodlookupdatamaintainancescreen.GetCellValue(2, 1) + "%' and TenantID ='" + MarksheetConstants.TenantId + "'", "DefaultReferenceDateDay");
            List<int> defaultreferencedatemonth = TestData.CreateIntegerList("Select DefaultReferenceDateMonth From AssessmentPeriod Where Name Like'%" + assessmentperiodlookupdatamaintainancescreen.GetCellValue(2, 1) + "%' and TenantID ='" + MarksheetConstants.TenantId + "'", "DefaultReferenceDateMonth");

            //Asserting if the values are changed to its default values
            Assert.AreEqual(assessmentperiodlookupdatamaintainancescreen.GetCellValue(5, 1), defaultreferencedateday[0].ToString());
            Assert.AreEqual(assessmentperiodlookupdatamaintainancescreen.GetCellValue(6, 1), defaultreferencedatemonth[0].ToString());
        }

    }
}
