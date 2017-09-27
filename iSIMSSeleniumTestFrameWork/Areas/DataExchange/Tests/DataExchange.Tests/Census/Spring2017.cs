using SeSugar.Automation;
using Selene.Support.Attributes;
using DataExchange.Components.Common;
using DataExchange.POM.Helper;
using DataExchange.POM.Components.Census;
using NUnit.Framework;
using TestSettings;
using DataExchange.POM.Base;
using OpenQA.Selenium;
using System.Threading;
using System;
using System.Collections.Generic;
using WebDriverRunner.webdriver;
using System.Diagnostics;
using DataExchange.POM.Components.Common;

namespace DataExchange.Tests.Census
{
    public class Spring2017
    {
        ///<summary>
        /// Page Constants
        /// </summary>
        /// 
        private const string period = "_Spring2017";

        private const string Absentee_Det_Rpt = "Absentee_Det_Rpt";
        private const string Address_Det_Rpt = "Address_Det_Rpt";
        private const string Adopt_From_Care_Rpt = "Adopt_From_Care_Rpt";
        private const string Attendance_Det_Rpt = "Attendance_Det_Rpt";
        private const string Class_Det_Rpt = "Class_Det_Rpt";
        private const string Early_Years_Rpt = "Early_Years_Rpt";
        private const string Exclusion_Det_Rpt = "Exclusion_Det_Rpt";
        private const string Free_School_Meals_Rpt = "Free_School_Meals_Rpt";
        private const string General_Det_Rpt = "General_Det_Rpt";
        private const string Leaver_Basic_Det_Rpt = "Leaver_Basic_Det_Rpt";
        private const string Preview_Summary_Report = "Preview_Summary_Report";
        private const string All = "All";
        private const string Pupils_Basic_Det_Rpt = "Pupils_Basic_Det_Rpt";
        private const string School_Dinners_Rpt = "School_Dinners_Rpt";
        private const string SEN_Det_Rpt = "SEN_Det_Rpt";
        private const string Top_Up_Funding_Rpt = "Top_Up_Funding_Rpt";

        private const string Sign_Off_Button = "sign_off_button";
        private const string Sign_off_return_button = "sign_off_return_button";
        private const string Save_Button = "well_know_action_save";
        private const string Validate_Button = "ActivateCustomBehaviourButton-Validate";

        private const string SchoolSummaryTab = "SchoolSummaryTab-label";
        private const string CensusDataCheckTab = "CensusDataCheckTab-label";
        private const string PupilInformationTab = "PupilInformationTab-label";
        private const string CensusDaySnapshotTab = "CensusDaySnapshotTab-label";

        private const string SchoolCensusSection = "SchoolCensusSection";
        private const string CensusDate = "CensusDate";

        private const string status_success = "status_success";

        private static List<string> _lstWidgetDesc;
        public static List<string> AllWidgetDesc
        {
            get
            {
                if (_lstWidgetDesc == null)
                {
                    _lstWidgetDesc = new List<string>();
                    _lstWidgetDesc.Add(" Is Census Day");
                    _lstWidgetDesc.Add("Days to " + DataExchangeElement.CensusTestTargetVersion + " Census");
                    _lstWidgetDesc.Add("No Census Data Available");
                    _lstWidgetDesc.Add("Days to " + DataExchangeElement.SummerCensusTestTargetVersion + " Census");
                }
                return _lstWidgetDesc;
            }
        }
        public void NavigateToReturnPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            // var censusSearchTiles = censusTripletPage.SearchCriteria.Search();

            TimeSpan maxDuration = TimeSpan.FromMinutes(3);
            Stopwatch sw = Stopwatch.StartNew();

            bool isReturnProcessed = false;

            while (sw.Elapsed < maxDuration && !isReturnProcessed)
            {
                isReturnProcessed = !isReturnProcessing(censusTripletPage);
            }

            sw.Stop();

            if (isReturnProcessed)
            {
                if (censusTripletPage.ClickSearchResultItemIfAny())
                {
                    return;
                }
                else
                {
                    Wait.WaitTillAllAjaxCallsComplete();
                }
            }
            else
            {
                Assert.IsTrue(false);
            }

        }

        private bool isReturnProcessing(CensusPage censusTripletPage)
        {
            bool isProcessing = false;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            CensusPage.CensusSearchResultTile censusTile = censusSearchTiles.FirstOrDefault();
            if (censusTile.Tile.Text.ToLower().Contains("processing"))
            {
                isProcessing = true;
            }
            return isProcessing;
        }

        ///<summary>
        /// Function to wait untill the view refresh
        /// </summary>
        public bool WaitForDetailsViewAutoRefresh(string action, int timeOutInSeconds)
        {
            System.Console.WriteLine("Waiting for detail view refresh");

            string jsPredicate = string.Empty;

            switch (action)
            {
                case Sign_Off_Button:
                case Validate_Button:
                    jsPredicate = "return $(\"[id = '" + CensusDataCheckTab + "']\").length > 0;";
                    break;
                case Save_Button:
                    jsPredicate = "return $(\"[data-automation-id = '" + status_success + "']\").length > 0;";
                    break;
            }

            return Wait.WaitTillConditionIsMet(jsPredicate, timeOutInSeconds);
        }

        ///<summary>
        /// Function to get report name appended with current period
        /// </summary>
        private string GetReportName(string report)
        {
            return report + period;
        }

        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void CensusWidgetCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            Wait.WaitTillAllAjaxCallsComplete();

            bool isContainsWidget = false;

            IWebElement widgetElement = WebContext.WebDriver.FindElementSafe(By.CssSelector(SeleniumHelper.AutomationId("StatutoryReturnWidget")));
            if (widgetElement.IsElementExists())
            {
                var widgetDesc = widgetElement.FindChild(By.CssSelector(".hp-tile-desc")).Text;
                if (AllWidgetDesc.Contains(widgetDesc))
                    isContainsWidget = true;
            }
            Assert.IsTrue(isContainsWidget);

        }


        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void CensusDataErrorCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            CensusDataChecks objCensusDataChecks = new CensusDataChecks();
            objCensusDataChecks.OpenCensusDataChecksTab();
            objCensusDataChecks.ClickShowIssueGroupedbyTypeCheckBox();
            Assert.IsTrue(objCensusDataChecks.HasRecords());
        }

        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void ShowAllPupilCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            ShowAllPupils objShowAllPupils = new ShowAllPupils();
            objShowAllPupils.OpenPupilInformationTab();
            objShowAllPupils.ClickShowAllPupilCheckBox();

            Assert.IsTrue(objShowAllPupils.HasRecords());
        }

        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void ShowEarlyYearsPupilCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            ShowEarlyYearsPupil objShowEarlyYearsPupil = new ShowEarlyYearsPupil();
            objShowEarlyYearsPupil.OpenPupilInformationTab();
            objShowEarlyYearsPupil.ClickShowEarlyYearsPupilCheckBox();
            Assert.IsTrue(objShowEarlyYearsPupil.HasRecords());
        }

        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void ShowEarlyYearsPupilPremiumCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            ShowEarlyYearsPupilPremium objShowEarlyYearsPupilPremium = new ShowEarlyYearsPupilPremium();
            objShowEarlyYearsPupilPremium.OpenPupilInformationTab();
            objShowEarlyYearsPupilPremium.ClickShowEarlyYearsPupilPremiumCheckBox();
            Assert.IsTrue(objShowEarlyYearsPupilPremium.HasRecords());
        }

        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void ClassesCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            CensusDaySnapshot objCensusDaySnapshot = new CensusDaySnapshot();
            objCensusDaySnapshot.OpenCensusDataChecksTab();
            Assert.IsTrue(objCensusDaySnapshot.ClassesGridHasRecords());
        }

        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void SchoolLunchTakenCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            CensusDaySnapshot objCensusDaySnapshot = new CensusDaySnapshot();
            objCensusDaySnapshot.OpenCensusDataChecksTab();
            Assert.IsTrue(objCensusDaySnapshot.SchoolLunchTakenGridHasRecords());
        }

        ///<summary>
        /// Test to check if only one return exist
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void CheckOnlyOneReturnExists()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            Wait.WaitTillAllAjaxCallsComplete();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            //WaitForDetailsViewAutoRefresh(Validate_Button, 90);
            int returnsCount = censusSearchTiles.Count();

            if (returnsCount == 1)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        ///<summary>
        /// Test to check if all the tabs exists
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void CheckTabExists()
        {
            bool tabExists = false;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();

            string[] strTabs = {  SchoolSummaryTab
                                  ,CensusDataCheckTab
                                  ,PupilInformationTab
                                  ,CensusDaySnapshotTab};

            for (int i = 0; i < strTabs.Length; i++)
            {
                tabExists = objTab.CheckTabExists(strTabs[i]);

                if (!tabExists)
                {
                    Assert.IsTrue(false);
                }
            }
        }

        ///<summary>
        /// Test to check if all the reports exists
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void CheckReportsExists()
        {
            bool tabExists;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();
            tabExists = objTab.CheckTabExists(SchoolSummaryTab);

            if (tabExists)
            {
                objTab.ClickTab(SchoolSummaryTab);
                Wait.WaitTillAllAjaxCallsComplete();

                string[] strReports = { Absentee_Det_Rpt
                                        ,Address_Det_Rpt
                                        ,Adopt_From_Care_Rpt
                                        ,Attendance_Det_Rpt
                                        ,Class_Det_Rpt
                                        ,Early_Years_Rpt
                                        ,Exclusion_Det_Rpt
                                        ,Free_School_Meals_Rpt
                                        ,General_Det_Rpt
                                        ,Leaver_Basic_Det_Rpt
                                        ,Preview_Summary_Report
                                        ,All
                                        ,Pupils_Basic_Det_Rpt
                                        ,School_Dinners_Rpt
                                        ,SEN_Det_Rpt
                                        ,Top_Up_Funding_Rpt
                                        ,Preview_Summary_Report};


                bool reportExists = false;

                ReportComponent objReport = new ReportComponent();

                for (int i = 0; i < strReports.Length; i++)
                {
                    reportExists = objReport.CheckReportExists(SeleniumHelper.AutomationId(GetReportName(strReports[i])));

                    if (!reportExists)
                    {
                        Assert.IsTrue(false);
                    }
                }

            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        ///<summary>
        /// Test to check if school section with census date exists
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void CheckSchoolCensusSectionExists()
        {
            bool isExists = false;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();
            isExists = objTab.CheckTabExists(SchoolSummaryTab);

            if (isExists)
            {
                objTab.ClickTab(SchoolSummaryTab);
                Wait.WaitTillAllAjaxCallsComplete();

                TabComponent objSection = new TabComponent();
                isExists = objSection.CheckTabElementExists(SeleniumHelper.AutomationId(SchoolCensusSection));

                if (isExists)
                {
                    IWebElement dateElement = objSection.GetTabElement(SeleniumHelper.NameAttribute(CensusDate));

                    if (string.IsNullOrEmpty(dateElement.GetValue()))
                    {
                        Assert.IsTrue(false);
                    }
                }
                else
                {
                    Assert.IsTrue(false);
                }
            }
            else
            {
                Assert.IsTrue(false);
            }
        }


        ///<summary>
        /// Test to check if save process flow works
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P2 })]
        public void CheckSaveProcess()
        {
            bool isExists = false;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();
            IWebElement saveButton = objTab.GetTabElement(SeleniumHelper.AutomationId(Save_Button));

            if (saveButton.IsElementExists())
            {
                //saveButton.Click();
                SeleniumHelper.WaitForElementClickableThenClick(saveButton);

                isExists = WaitForDetailsViewAutoRefresh(Save_Button, 180);

                if (!isExists)
                {
                    Assert.IsTrue(false);
                }
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        ///<summary>
        /// Test to check if validate process flow works
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P1 })]
        public void CheckValidateProcess()
        {
            bool isExists = false;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();
            IWebElement validateButton = objTab.GetTabElement(SeleniumHelper.AutomationId(Validate_Button));

            if (validateButton.IsElementExists())
            {
                //validateButton.Click();
                SeleniumHelper.WaitForElementClickableThenClick(validateButton);
                Thread.Sleep(200);
                Wait.WaitTillAllAjaxCallsComplete();
                isExists = WaitForDetailsViewAutoRefresh(Validate_Button, 299);

                if (!isExists)
                {
                    Assert.IsTrue(false);
                }
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        ///<summary>
        /// Test to check if sign-off process flow works
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.Spring2017, BugPriority.P1 })]
        public void CheckSignOffProcess()
        {
            bool isExists = false;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();
            isExists = objTab.CheckTabExists(SchoolSummaryTab);

            if (isExists)
            {
                objTab.ClickTab(SchoolSummaryTab);
                Wait.WaitTillAllAjaxCallsComplete();

                TabComponent objSection = new TabComponent();
                IWebElement signOffButton = objSection.GetTabElement(SeleniumHelper.AutomationId(Sign_Off_Button));

                if (signOffButton.IsElementExists())
                {
                    // signOffButton.Click();
                    SeleniumHelper.WaitForElementClickableThenClick(signOffButton);
                    Thread.Sleep(200);
                    Wait.WaitTillAllAjaxCallsComplete();

                    IWebElement signOffReturnButton = objSection.GetTabElement(SeleniumHelper.AutomationId(Sign_off_return_button));

                    if (signOffReturnButton.IsElementExists())
                    {
                        // signOffReturnButton.Click();
                        SeleniumHelper.WaitForElementClickableThenClick(signOffReturnButton);
                        Thread.Sleep(200);
                        Wait.WaitTillAllAjaxCallsComplete();

                        isExists = WaitForDetailsViewAutoRefresh(Sign_Off_Button, 299);

                        if (!isExists)
                        {
                            Assert.IsTrue(false);
                        }

                    }
                    else
                    {
                        Assert.IsTrue(false);
                    }

                }
                else
                {
                    Assert.IsTrue(false);
                }

            }
            else
            {
                Assert.IsTrue(false);
            }
        }

    }
}
