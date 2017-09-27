using System.Threading;
using TestSettings;
using DataExchange.POM.Helper;
using SeSugar.Automation;
using NUnit.Framework;
using DataExchange.POM.Components.DENI;
using OpenQA.Selenium.Support.UI;
using OnRollPupilSection = DataExchange.POM.Components.DENI.OnRollPupilSection;
using Selene.Support.Attributes;
using SeSugar;
using DataExchange.Components.Common;
using DataExchange.POM.Components.Common;

namespace DataExchange.Tests
{
    public class DENI2015
    {
        private readonly int _tenantId = Environment.Settings.TenantId;

        private void NavigateToReturnPage()
        {
            if (_tenantId == 1019999)
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
                AutomationSugar.NavigateMenu(DataExchangeElement.MenuTopLevel, DataExchangeElement.MenuCategory, DataExchangeElement.MenuItem);
                var deniTripletPage = new DeniTripletPage();
                deniTripletPage.SearchCriteria.ReturnTypeVersionDropdown = DataExchangeElement.DENIVersion;
                deniTripletPage.SearchCriteria.Search();

                if (deniTripletPage.ClickSearchResultItemIfAny())
                {
                    //wait till details are loaded
                    Wait.WaitTillAllAjaxCallsComplete();
                }
                else
                {
                    var createDeniDialog = deniTripletPage.CreateDeni();
                    createDeniDialog.OKButton();
                }
            }
        }

        [WebDriverTest(Enabled = true, Browsers = new[] {  BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.DENI2015, BugPriority.P2 })]
        public void OnRollPupilSectionCheck()
        {
            NavigateToReturnPage();

            OnRollPupilSection onroll = new OnRollPupilSection();

            WebDriverWait wait = new WebDriverWait(SeSugar.Environment.WebContext.WebDriver, System.TimeSpan.FromMinutes(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(onroll.OnrollPupilSection));
            onroll.OpenSection();
            Assert.IsTrue(onroll.HasRecords());
        }

        [WebDriverTest(Enabled = true, Browsers = new[] {  BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.DENI2015, BugPriority.P2 })]
        public void ProjectedPupilNumbersSectionCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            ProjectedPupilNumbersSection projectedPupilNumbersSection = new ProjectedPupilNumbersSection();

            projectedPupilNumbersSection.OpenSection();
            Wait.WaitTillAllAjaxCallsComplete();
            Assert.IsTrue(projectedPupilNumbersSection.HasRecords());
        }

        [WebDriverTest(Enabled = true, Browsers = new[] {  BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.DENI2015, BugPriority.P2 })]
        public void ProjectedNurserySectionCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            ProjectedNurserySection projectedNurserySectionSection = new ProjectedNurserySection();

            projectedNurserySectionSection.OpenSection();
            Wait.WaitTillAllAjaxCallsComplete();
            Assert.IsTrue(projectedNurserySectionSection.HasRecords());
        }

        [WebDriverTest(Enabled = true, Browsers = new[] {  BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.DENI2015, BugPriority.P2 })]
        public void EarlyYearsProvisionSectionCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            EarlyYearsProvisionSection earlyYearsProvision = new EarlyYearsProvisionSection();

            earlyYearsProvision.OpenSection();
            Wait.WaitTillAllAjaxCallsComplete();
            Assert.IsTrue(earlyYearsProvision.HasRecords());
        }

        [WebDriverTest(Enabled = true, Browsers = new[] {  BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.DENI2015, BugPriority.P2 })]
        public void OnrollPupilAttendanceSectionCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            OnrollPupilAttendanceSection onrollPupilAttendance = new OnrollPupilAttendanceSection();

            onrollPupilAttendance.OpenSection();
            Wait.WaitTillAllAjaxCallsComplete();
            Assert.IsTrue(onrollPupilAttendance.HasRecords());
        }

        [WebDriverTest(Enabled = true, Browsers = new[] {  BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.DENI2015, BugPriority.P2 })]
        public void LeaverSectionCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            LeaverSection leaverSection = new LeaverSection();

            leaverSection.OpenSection();
            Wait.WaitTillAllAjaxCallsComplete();
            Assert.IsTrue(leaverSection.HasRecords());
        }
        [WebDriverTest(Enabled = true, Browsers = new[] {  BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.DENI2015, BugPriority.P2 })]
        public void LeaverAttendanceSectionCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            LeaverAttendance leaverAtt = new LeaverAttendance();

            leaverAtt.OpenSection();
            Wait.WaitTillAllAjaxCallsComplete();
            Assert.IsTrue(leaverAtt.HasRecords());
        }

        [WebDriverTest(Enabled = true, Browsers = new[] {  BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.DENI2015, BugPriority.P2 })]
        public void LeaverSENSectionCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            LeaverSEN leaverSEN = new LeaverSEN();

            leaverSEN.OpenSection();
            Wait.WaitTillAllAjaxCallsComplete();
            Assert.IsTrue(leaverSEN.HasRecords());
        }
        [WebDriverTest(Enabled = true, Browsers = new[] {  BrowserDefaults.Chrome }, Groups = new[] { ReturnVersion.DENI2015, BugPriority.P2  })]
        public void PupilsNotResidentInNISectionCheck()
        {
            NavigateToReturnPage();
            Wait.WaitTillAllAjaxCallsComplete();
            PupilsNotResidentInNISection pupilsNotResidentInNISection = new PupilsNotResidentInNISection();

            pupilsNotResidentInNISection.OpenSection();
            Wait.WaitTillAllAjaxCallsComplete();
            Assert.IsTrue(pupilsNotResidentInNISection.HasRecords());
        }

    }

}

