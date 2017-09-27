using DataExchange.Components.Common;
using DataExchange.POM.Base;
using DataExchange.POM.Components.Census;
using DataExchange.POM.Components.Common;
using DataExchange.POM.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Selene.Support.Attributes;
using SeSugar.Automation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.webdriver;

namespace DataExchange.Tests.Census
{
    public class Return
    {       

        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void WidgetCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            Wait.WaitTillAllAjaxCallsComplete();

            bool isContainsWidget = false;

            IWebElement widgetElement = WebContext.WebDriver.FindElementSafe(By.CssSelector(SeleniumHelper.AutomationId("StatutoryReturnWidget")));
            if (widgetElement.IsElementExists())
            {
                var widgetDesc = widgetElement.FindChild(By.CssSelector(".hp-tile-desc")).Text;
                if (Regex.IsMatch(widgetDesc, @"[A-Za-z]"))
                    isContainsWidget = true;
            }
            Assert.IsTrue(isContainsWidget);

        }
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DataErrorCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            CensusDataChecks objCensusDataChecks = new CensusDataChecks();
            objCensusDataChecks.OpenCensusDataChecksTab();
            objCensusDataChecks.ClickShowIssueGroupedbyTypeCheckBox();
            Assert.IsTrue(objCensusDataChecks.HasRecords());
        }
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
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
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShowEarlyYearsPupilCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            ShowEarlyYearsPupil objShowEarlyYearsPupil = new ShowEarlyYearsPupil();
            objShowEarlyYearsPupil.OpenPupilInformationTab();
            //objShowEarlyYearsPupil.ClickShowAllPupilCheckBox();
            objShowEarlyYearsPupil.ClickShowEarlyYearsPupilCheckBox();
            Assert.IsTrue(objShowEarlyYearsPupil.HasRecords());
        }
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SchoolLunchTakenCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            CensusDaySnapshot objCensusDaySnapshot = new CensusDaySnapshot();
            objCensusDaySnapshot.OpenCensusDataChecksTab();
            Assert.IsTrue(objCensusDaySnapshot.SchoolLunchTakenGridHasRecords());
        }
        
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckTabExists()
        {
            bool tabExists = false;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();

            string[] strTabs = {  Constant.SchoolSummaryTab
                                  ,Constant.CensusDataCheckTab
                                  ,Constant.PupilInformationTab
                                  ,Constant.CensusDaySnapshotTab};

            for (int i = 0; i < strTabs.Length; i++)
            {
                tabExists = objTab.CheckTabExists(strTabs[i]);

                if (!tabExists)
                {
                    Assert.IsTrue(false);
                }
            }
        }        
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckSchoolCensusSectionExists()
        {
            bool isExists = false;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();
            isExists = objTab.CheckTabExists(Constant.SchoolSummaryTab);

            if (isExists)
            {
                objTab.ClickTab(Constant.SchoolSummaryTab);
                Wait.WaitTillAllAjaxCallsComplete();

                TabComponent objSection = new TabComponent();
                isExists = objSection.CheckTabElementExists(SeleniumHelper.AutomationId(Constant.SchoolCensusSection));

                if (isExists)
                {
                    IWebElement dateElement = objSection.GetTabElement(SeleniumHelper.NameAttribute(Constant.CensusDate));

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
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckSaveProcess()
        {
            bool isExists = false;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();
            IWebElement saveButton = objTab.GetTabElement(SeleniumHelper.AutomationId(Constant.Save_Button));

            if (saveButton.IsElementExists())
            {
                //saveButton.Click();
                SeleniumHelper.WaitForElementClickableThenClick(saveButton);

                isExists = WaitForDetailsViewAutoRefresh(Constant.Save_Button, 180);

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
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckValidateProcess()
        {
            bool isExists = false;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();
            IWebElement validateButton = objTab.GetTabElement(SeleniumHelper.AutomationId(Constant.Validate_Button));

            if (validateButton.IsElementExists())
            {
                //validateButton.Click();
                SeleniumHelper.WaitForElementClickableThenClick(validateButton);
                Thread.Sleep(200);
                Wait.WaitTillAllAjaxCallsComplete();
                isExists = WaitForDetailsViewAutoRefresh(Constant.Validate_Button, 299);

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
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckSignOffProcess()
        {
            bool isExists = false;

            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();
            isExists = objTab.CheckTabExists(Constant.SchoolSummaryTab);

            if (isExists)
            {
                objTab.ClickTab(Constant.SchoolSummaryTab);
                Wait.WaitTillAllAjaxCallsComplete();

                TabComponent objSection = new TabComponent();
                IWebElement signOffButton = objSection.GetTabElement(SeleniumHelper.AutomationId(Constant.Sign_Off_Button));

                if (signOffButton.IsElementExists())
                {
                    // signOffButton.Click();
                    SeleniumHelper.WaitForElementClickableThenClick(signOffButton);
                    Thread.Sleep(200);
                    Wait.WaitTillAllAjaxCallsComplete();

                    IWebElement signOffReturnButton = objSection.GetTabElement(SeleniumHelper.AutomationId(Constant.Sign_off_return_button));

                    if (signOffReturnButton.IsElementExists())
                    {
                        // signOffReturnButton.Click();
                        SeleniumHelper.WaitForElementClickableThenClick(signOffReturnButton);
                        Thread.Sleep(200);
                        Wait.WaitTillAllAjaxCallsComplete();

                        isExists = WaitForDetailsViewAutoRefresh(Constant.Sign_Off_Button, 299);

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
        public void NavigateToReturnPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager,enabledFeatures: "Stats_Return_Autumn2017");
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            //censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.SummerCensusTestTargetVersion;
            //var censusSearchTiles = censusTripletPage.SearchCriteria.Search();

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
        private bool WaitForDetailsViewAutoRefresh(string action, int timeOutInSeconds)
        {
            System.Console.WriteLine("Waiting for detail view refresh");

            string jsPredicate = string.Empty;

            switch (action)
            {
                case Constant.Sign_Off_Button:
                case Constant.Validate_Button:
                    jsPredicate = "return $(\"[id = '" + Constant.CensusDataCheckTab + "']\").length > 0;";
                    break;
                case Constant.Save_Button:
                    jsPredicate = "return $(\"[data-automation-id = '" + Constant.status_success + "']\").length > 0;";
                    break;
            }

            return Wait.WaitTillConditionIsMet(jsPredicate, timeOutInSeconds);
        }
        
    }
}
