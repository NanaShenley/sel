using Facilities.Components.Common;
using Facilities.Components.FacilitiesPages;
using Facilities.Components.FacilitiesTestData;
using NUnit.Framework;
using SharedComponents.CRUD;
using System;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using Facilities.Components.Facilities_Pages;
using Selene.Support.Attributes;

namespace SchoolSiteandBuildingTests
{
    class SchoolSiteTest
    {
        #region Story ID :- 928 :- Can I create My School Site?
        [WebDriverTest(Enabled = true, Groups = new[] { "All" , "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AddSchoolsite()
        {
            Random rns = new Random();
            var stsnum = rns.Next(100);
            var siteshortname = "ST01" + stsnum;
            Random rnl = new Random();
            var stlnum = rnl.Next(100);
            var sitelongname = "ST01" + stlnum;
            MySchoolDetailsPage schoolPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolPage.ClickAddSchoolSitebldnglink();
            sitepopup.EnterSiteShortName(siteshortname);
            sitepopup.EnterSiteLongName(sitelongname);
            sitepopup.EnterContactName("Adair");
            sitepopup.EnterTelephoneNumber("9527725936");
            sitepopup.EnterMobileNumber("9527725936");
            sitepopup.EnterFaxNumber("5246");
            sitepopup.EnterEmailAddress("Adair.kumar@capita.co.uk");
            sitepopup.EnterWebsiteAddress("www.capita.co.uk");
            sitepopup.ClickOkButton();
            schoolPage.Save();
            Assert.IsTrue(schoolPage.HasConfirmedSave("My School Details Saved"));
        }
        #endregion

        #region Story ID :- 928 :- Can I edit My School Site?
        [WebDriverTest(Enabled = true, Groups = new[] { "All" , "P2"}, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EditSchoolsite()
        {
            Random rns = new Random();
            var stsnum = rns.Next(1000);
            var siteshortname = "SE" + stsnum;
            Random rnl = new Random();
            var stlnum = rnl.Next(1000);
            var sitelongname = "SLE" + stlnum;
            MySchoolDetailsPage schoolRoomPage = FacilitiesNavigation.NavigatetoMySchoolDetailPage();
            schoolRoomPage.ExpandSchoolSitebldng();
            AddSchoolSitepopupPage sitepopup = schoolRoomPage.ClickAddSchoolSitebldnglink();
            sitepopup.EnterSiteShortName(siteshortname);
            sitepopup.EnterSiteLongName(sitelongname);
            sitepopup.EnterContactName("Adair");
            sitepopup.EnterTelephoneNumber("9527725936");
            sitepopup.EnterMobileNumber("9527725936");
            sitepopup.EnterFaxNumber("5246");
            sitepopup.EnterEmailAddress("Adair.kumar@capita.co.uk");
            sitepopup.EnterWebsiteAddress("www.capita.co.uk");
            sitepopup.ClickOkButton();
            schoolRoomPage.Save();
            schoolRoomPage.ClickEditButton();
            sitepopup.EnterSiteShortName("01");
            sitepopup.EnterSiteLongName("01 Modify Long Name Site Test");
            sitepopup.EnterContactName("Benson");
            sitepopup.EnterTelephoneNumber("98156557397");
            sitepopup.EnterMobileNumber("98156557397");
            sitepopup.EnterFaxNumber("9878");
            sitepopup.EnterEmailAddress("Benson.kumar@capita.co.uk");
            sitepopup.EnterWebsiteAddress("www.capitalone.co.uk");
            sitepopup.ClickOkButton();
            schoolRoomPage.Save();
            Assert.IsTrue(schoolRoomPage.HasConfirmedSave("My School Details Saved"));
            sitepopup.DeleteSiteDetails();
        }
        #endregion

        #region Story ID :-1344 :- Can I Search the Building Address by Postal Code.
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SearchSiteAddressPostCode()
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
            BuildingPopup.ClickAddNewAddress();
            AddressSearchPage SearchPage = new AddressSearchPage();
            SearchPage.EnterPostNumber("BT57 8RR");
            SearchPage.ClickSearchButton();
            SearchResults.WaitForResults();
            Assert.IsTrue(SearchResults.HasResults(1));
        }
        #endregion

        #region Story ID :- 1344 :- Can I Search the Building Address by House Number.
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P3" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SearchSiteAddressHouseNo()
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
            BuildingPopup.ClickAddNewAddress();
            AddressSearchPage SearchPage = new AddressSearchPage();
            SearchPage.EnterPostNumber("BT57 8RR");
            SearchPage.EnterHouseNumber("20");
            SearchPage.ClickSearchButton();
            SearchResults.WaitForResults();
            Assert.IsTrue(SearchResults.HasResults(1));
        }
        #endregion
    }
}
