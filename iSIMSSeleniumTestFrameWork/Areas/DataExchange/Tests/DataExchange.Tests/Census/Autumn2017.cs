using DataExchange.Components.Common;
using DataExchange.POM.Base;
using DataExchange.POM.Components.Census;
using DataExchange.POM.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;

namespace DataExchange.Tests.Census
{
    public class Autumn2017
    {
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckReportsExists()
        {
            bool tabExists;
            new Return().NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();

            TabComponent objTab = new TabComponent();
            tabExists = objTab.CheckTabExists(Constant.SchoolSummaryTab);

            if (tabExists)
            {
                objTab.ClickTab(Constant.SchoolSummaryTab);
                Wait.WaitTillAllAjaxCallsComplete();

                string[] strReports = { Constant.Absentee_Det_Rpt
                                        ,Constant.Address_Det_Rpt
                                        ,Constant.Adopt_From_Care_Rpt
                                        ,Constant.Attendance_Det_Rpt
                                        ,Constant.Exclusion_Det_Rpt
                                        ,Constant.Free_School_Meals_Rpt
                                        ,Constant.Leaver_Basic_Det_Rpt                                        
                                        ,Constant.All
                                        ,Constant.Pupils_Basic_Det_Rpt
                                        ,Constant.School_Dinners_Rpt
                                        ,Constant.SEN_Det_Rpt
                                        ,Constant.Top_Up_Funding_Rpt
                                        ,Constant.Attendance_Det_Rpt_2nd_Half};


                bool reportExists = false;

                ReportComponent objReport = new ReportComponent();

                for (int i = 0; i < strReports.Length; i++)
                {
                    reportExists = objReport.CheckReportExists(SeleniumHelper.AutomationId(strReports[i] + Constant.periodAutumn));

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
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckOnlyOneReturnExists()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager,enabledFeatures: "Stats_Return_Autumn2017");
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = Constant.AutumnTarget;
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
        /*[Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void WidgetCheckAutumn2017()
        {
            WidgetCheck();
        }
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DataErrorCheckAutumn2017()
        {
            DataErrorCheck();
        }
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShowAllPupilCheckAutumn2017()
        {
            ShowAllPupilCheck();
        }
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShowEarlyYearsPupilCheckAutumn2017()
        {
            ShowEarlyYearsPupilCheck();
        }
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SchoolLunchTakenCheckAutumn2017()
        {
            SchoolLunchTakenCheck();
        }

        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckOnlyOneReturnExistsAutumn2017()
        {

        }*/
    }
}
