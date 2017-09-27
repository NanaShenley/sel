using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;
using DataExchange.POM.Helper;
using SeSugar.Automation;
using DataExchange.POM.Components.Census;
using NUnit.Framework;
using DataExchange.Components.Common;
using OpenQA.Selenium;
using Selene.Support.Attributes;

namespace Census.Tests
{
    public class TestCensus
    {
        /// <summary>
        /// BeforeSuiteWebTest
        /// </summary>
        //[BeforeSuiteWebTest(Groups = new[] { DataExchangeElement.CensusGroup })]
        //public void BeforeSuite()
        //{
        //    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);
        //    AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
        //    var censusTripletPage = new CensusPage();
        //    censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = "2017";
        //    var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
        //    if (censusSearchTiles.Count() == 0)
        //    {
        //        var createCensusDialog = censusTripletPage.CreateCensus();
        //        createCensusDialog.ReturnTypeDropdown = "DENI Census Return";
        //        createCensusDialog.ReturnTypeVersionDropdown = "2015";
        //        createCensusDialog.OKButton();
        //    }
        //}


        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void OnRollPupilSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            OnRollPupilSection onroll = new OnRollPupilSection();
            onroll.OpenSection();
            Assert.IsTrue(onroll.HasRecords());
        }

        //Test to authorise a return and check if sign off section successfully exists.
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void GetSignOffSectionOpened()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            //Need to check how the below selection of dropdown works in lab if no value is present
            //censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;  
                    
            IWebElement searchResultTile = censusTripletPage.GetSearchResults("Validated with Issues");            
            if (searchResultTile == null)
            {
                censusTripletPage.CreateReturn();
            }
            else
            {               
                searchResultTile.Click();               
            }            
            //to authorise the return
            AuthoriseCensus authorise = new AuthoriseCensus();
            authorise.ClickAuthoriseButton();
            authorise.AuthoriseDialog();
                        
            //Sign off section
            SignOffsection signOff = new SignOffsection();
            signOff.OpenSection();
            Assert.IsTrue(signOff.HasSectiondisplayed());
        }

        //OnRoll Pupil  Home Test
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void OnRollPupilHomeInfoSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            OnRollPupilHomeInfoSection onRollPupilHomeInfoSection = new OnRollPupilHomeInfoSection();
            onRollPupilHomeInfoSection.OpenSection();
            Assert.IsTrue(onRollPupilHomeInfoSection.HasRecords());
        }

        //OnRoll Pupil  FSM Test
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void OnRollPupilFSMSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            OnRollPupilFreeSchoolMeal onRollPupilFSMSectionCheck = new OnRollPupilFreeSchoolMeal();
            onRollPupilFSMSectionCheck.OpenSection();
            Assert.IsTrue(onRollPupilFSMSectionCheck.HasRecords());
        }

        //OnRoll Exclusions Test
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void OnRollExclusionSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            OnRollPupilExclusionSection onRollPupilExclusionSection = new OnRollPupilExclusionSection();
            onRollPupilExclusionSection.OpenSection();
            Assert.IsTrue(onRollPupilExclusionSection.HasRecords());
        }

        //school lunch
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void SchoolLunchTakenSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            SchoolLunchTakenSection schoolLunchTakenSection = new SchoolLunchTakenSection();
            schoolLunchTakenSection.OpenSection();
            Assert.IsTrue(schoolLunchTakenSection.HasRecords());
        }

        //OnRoll Pupil Class Type
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void OnRollPupilClassTypeSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            OnRollPupilClassTypeSection onRollPupilClassTypeSection = new OnRollPupilClassTypeSection();
            onRollPupilClassTypeSection.OpenSection();
            Assert.IsTrue(onRollPupilClassTypeSection.HasRecords());
        }

        //OnRoll Pupil Attendance Basic Details
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void OnRollPupilAttendanceBasicDetailsSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            OnRollPupilAttendanceBasicDetailsSection onRollPupilAttendanceBasicDetailsSection = new OnRollPupilAttendanceBasicDetailsSection();
            onRollPupilAttendanceBasicDetailsSection.OpenSection();
            Assert.IsTrue(onRollPupilAttendanceBasicDetailsSection.HasRecords());
        }

        //OnRoll Pupil Manual Attendance
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void OnRollPupilManualAttendanceSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            OnRollPupilManulAttendanceSection onRollPupilManulAttendanceSection = new OnRollPupilManulAttendanceSection();
            onRollPupilManulAttendanceSection.OpenSection();
            Assert.IsTrue(onRollPupilManulAttendanceSection.HasRecords());
        }

        //EarlyYears Basic Details
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void EarlyYearsBasicDetailsSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            EarlyYearsProvisionSection earlyYearsProvisionSection = new EarlyYearsProvisionSection();
            earlyYearsProvisionSection.OpenSection();
            Assert.IsTrue(earlyYearsProvisionSection.HasRecords());
        }

        //Early Years Pupil Premium

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void EarlyYearsPupilPremiumSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            EarlyYearsPupilPremium earlyYearsPupilPremium = new EarlyYearsPupilPremium();
            earlyYearsPupilPremium.OpenSection();
            Assert.IsTrue(earlyYearsPupilPremium.HasRecords());
        }

        //Census Details
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void CensusDetailsSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            CensusDetailsSection censusDetailsSection = new CensusDetailsSection();
            censusDetailsSection.OpenSection();
            Assert.IsTrue(censusDetailsSection.HasRecords());
        }


        //School Information
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void SchoolInfoSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            SchoolInformationSection schoolInformationSection = new SchoolInformationSection();
            schoolInformationSection.OpenSection();
            Assert.IsTrue(schoolInformationSection.HasRecords());
        }


        //Classes Section
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void ClassesSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            ClassesSection classesSection = new ClassesSection();
            classesSection.OpenSection();
            Assert.IsTrue(classesSection.HasRecords());
        }

        //Leaver Basic Section
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void LeaverBasicSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            LeaverBasicDetailSection leaverBasicDetailSection = new LeaverBasicDetailSection();
            leaverBasicDetailSection.OpenSection();
            Assert.IsTrue(leaverBasicDetailSection.HasRecords());
        }

        //Leaver Exclusion Section
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void LeaverExclusionSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            LeaverExclusionSection leaverExclusionSection = new LeaverExclusionSection();
            leaverExclusionSection.OpenSection();
            Assert.IsTrue(leaverExclusionSection.HasRecords());
        }

        //Leaver Pupil Attendance Section
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { DataExchangeElement.CensusGroup })]
        public void LeaverAttendanceSectionCheck()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.CensusTestTargetVersion;
            var censusSearchTiles = censusTripletPage.SearchCriteria.Search();
            var censusPage = censusSearchTiles.SingleOrDefault(x => true).Click<CensusPage>();
            LeaverPupilAttendanceSection leaverPupilAttendanceSection = new LeaverPupilAttendanceSection();
            leaverPupilAttendanceSection.OpenSection();
            Assert.IsTrue(leaverPupilAttendanceSection.HasRecords());
        }
    }
}
