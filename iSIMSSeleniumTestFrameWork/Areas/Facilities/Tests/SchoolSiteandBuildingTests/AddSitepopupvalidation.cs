using Facilities.Components.Common;
using Facilities.Components.Facilities_Pages;
using NUnit.Framework;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
using Facilities.Components.FacilitiesTestData;
using SharedComponents.CRUD;
using OpenQA.Selenium;
using System;
using Selene.Support.Attributes;

namespace SchoolSiteandBuildingTests
{
   class AddSitepopupvalidation
    {
        #region Story ID : 928 :- Validation Site Long Name is required.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationSiteLongName()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            sitepopup.EnterSiteShortName("ST1");
            sitepopup.EnterSiteLongName("");
            sitepopup.ClickOkButton();
            var ValidationWarning = SeleniumHelper.Get(MySchoolDetailsElements.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story ID :- Validation for site contact Email Address.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationContactEmailAddress()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            sitepopup.EnterSiteShortName("ST1");
            sitepopup.EnterSiteLongName("North");
            sitepopup.EnterEmailAddress("rajesh");
            sitepopup.ClickOkButton();
            var ValidationWarning = SeleniumHelper.Get(MySchoolDetailsElements.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story ID :- Validation for site contact Email Address.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationContactWebSiteAddress()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage popup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            popup.EnterSiteShortName("ST1");
            popup.EnterSiteLongName("North");
            popup.EnterWebsiteAddress("XXXXXXX");
            popup.ClickOkButton();
            var ValidationWarning = SeleniumHelper.Get(MySchoolDetailsElements.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story ID :- 928 : AC: Site Short name Max Field valiation
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void SiteShortNameMaxFieldLength()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            Assert.IsTrue(sitepopup.SiteShortName.GetAttribute("maxlength") == "5");
        }
        #endregion

        #region Story ID :- 928 : AC: Site Long name Max Field valiation
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SiteLongNameMaxFieldLength()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            Assert.IsTrue(sitepopup.SiteLongName.GetAttribute("maxlength") == "200");
        }
        #endregion

        #region Story ID :- 928 : AC: Site Contact Name Max Field valiation
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SiteContactNameMaxFieldLength()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            Assert.IsTrue(sitepopup.SiteContactName.GetAttribute("maxlength") == "255");
        }
        #endregion

        #region Story ID :- 928 : AC: Site Telephone Number Max Field valiation
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SiteTelephoneNumberMaxFieldLength()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            Assert.IsTrue(sitepopup.SiteTelephoneNumber.GetAttribute("maxlength") == "35");
        }
        #endregion

        #region Story ID :- 928 : AC: Site Mobile Number Max Field valiation
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SiteMobileNumberMaxFieldLength()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            Assert.IsTrue(sitepopup.SiteMobileNumber.GetAttribute("maxlength") == "35");
        }
        #endregion

        #region Story ID :- 928 : AC: Site Fax Number Max Field valiation
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SiteFaxNumberMaxFieldLength()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            Assert.IsTrue(sitepopup.SiteFaxNumber.GetAttribute("maxlength") == "35");
        }
        #endregion

        #region Story ID :- 928 : AC: Site Email Address Max Field valiation
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SiteEmailAddressrMaxFieldLength()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            Assert.IsTrue(sitepopup.SiteEmailAddress.GetAttribute("maxlength") == "254");
        }
        #endregion

        #region Story ID :- 928 : AC: Site Website Address Max Field valiation
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SiteWebsiteAddressrMaxFieldLength()
        {
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            Assert.IsTrue(sitepopup.SiteWebsiteAddress.GetAttribute("maxlength") == "100");
        }
        #endregion

        #region Story ID :-1344 :- AC: Building Short Name Max field validation.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void BldngShortNameMaxFieldLength()
        {
            Random rns = new Random();
            var stsnum = rns.Next(100);
            var siteshortname = "ST" + stsnum;
            Random rnl = new Random();
            var stlnum = rnl.Next(1000);
            var sitelongname = "SL" + stlnum;
            MySchoolDetailsPage schoolPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolPage.ClickAddSchoolSitebldnglink();
            sitepopup.EnterSiteShortName(siteshortname);
            sitepopup.EnterSiteLongName(sitelongname);
            AddSchoolBuildingPopupPage BuildingPopup = sitepopup.ClickAddBuilding();
            Assert.IsTrue(BuildingPopup.Shortname.GetAttribute("maxlength") == "5");
        }
        #endregion

        #region Story ID :-1344 :- AC: Building Long Name Max field validation.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void BldngLongNameMaxFieldLength()
        {
            Random rns = new Random();
            var stsnum = rns.Next(100);
            var siteshortname = "ST" + stsnum;
            Random rnl = new Random();
            var stlnum = rnl.Next(1000);
            var sitelongname = "SL" + stlnum;
            MySchoolDetailsPage schoolPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolPage.ClickAddSchoolSitebldnglink();
            sitepopup.EnterSiteShortName(siteshortname);
            sitepopup.EnterSiteLongName(sitelongname);
            AddSchoolBuildingPopupPage BuildingPopup = sitepopup.ClickAddBuilding();
            Assert.IsTrue(BuildingPopup.Longname.GetAttribute("maxlength") == "26");
        }
        #endregion

        #region Story ID :-1344 :- AC: Building Telephone number Max field validation.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void BldnTelNumberMaxFieldValidation()
        {
            Random rns = new Random();
            var stsnum = rns.Next(100);
            var siteshortname = "ST" + stsnum;
            Random rnl = new Random();
            var stlnum = rnl.Next(1000);
            var sitelongname = "SL" + stlnum;
            MySchoolDetailsPage schoolPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolPage.ClickAddSchoolSitebldnglink();
            sitepopup.EnterSiteShortName(siteshortname);
            sitepopup.EnterSiteLongName(sitelongname);
            AddSchoolBuildingPopupPage BuildingPopup = sitepopup.ClickAddBuilding();
            Assert.IsTrue(BuildingPopup.Telephonenumber.GetAttribute("maxlength") == "35");
        }
        #endregion

        #region Story ID :-1344 :- AC: Building Fax Number Max field validation.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void BldnFaxNumberMaxFieldValidation()
        {
            Random rns = new Random();
            var stsnum = rns.Next(100);
            var siteshortname = "ST" + stsnum;
            Random rnl = new Random();
            var stlnum = rnl.Next(1000);
            var sitelongname = "SL" + stlnum;
            MySchoolDetailsPage schoolPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolPage.ClickAddSchoolSitebldnglink();
            sitepopup.EnterSiteShortName(siteshortname);
            sitepopup.EnterSiteLongName(sitelongname);
            AddSchoolBuildingPopupPage BuildingPopup = sitepopup.ClickAddBuilding();
            Assert.IsTrue(BuildingPopup.Faxnumber.GetAttribute("maxlength") == "35");
        }
        #endregion

        #region Story ID :-1344 :- AC: Building Email Address Max field validation.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void BldnEmailMaxFieldValidation()
        {
            Random rns = new Random();
            var stsnum = rns.Next(100);
            var siteshortname = "ST" + stsnum;
            Random rnl = new Random();
            var stlnum = rnl.Next(1000);
            var sitelongname = "SL" + stlnum;
            MySchoolDetailsPage schoolPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolPage.ClickAddSchoolSitebldnglink();
            sitepopup.EnterSiteShortName(siteshortname);
            sitepopup.EnterSiteLongName(sitelongname);
            AddSchoolBuildingPopupPage BuildingPopup = sitepopup.ClickAddBuilding();
            Assert.IsTrue(BuildingPopup.Emailaddress.GetAttribute("maxlength") == "254");
        }
        #endregion

    }
}
