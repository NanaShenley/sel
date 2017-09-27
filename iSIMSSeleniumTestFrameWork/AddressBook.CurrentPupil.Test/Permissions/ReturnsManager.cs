using AddressBook.Components;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
using AddressBook.Test;
using SharedComponents.HomePages;
using System.Collections.Generic;
using POM.Components.Pupil;
using System.Threading;
using System;
using System.Globalization;
using SeSugar.Automation;
using Attendance.POM.DataHelper;
using SeSugar.Data;

namespace AddressBook.CurrentPupil.Test.Permissions
{
    class ReturnsManager
    {
        private readonly string textForSearch = "Laura";

        #region ReturnsManagerQuickControlAccess()
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ReturnsManagerQuickControlAccess()
        {
            Assert.IsTrue(QuickControlAccess.isQuickControlAccess(SeleniumHelper.iSIMSUserType.ReturnsManager));
        }
        #endregion

        #region ReturnsManagerSearchCurrentPupilsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ReturnsManagerSearchCurrentPupilsAccess()
        {
            Assert.That(QuickControlAccess.hasPermissionToSearchCurrentPupil(SeleniumHelper.iSIMSUserType.ReturnsManager, textForSearch) >= 0);
        }
        #endregion

        #region ReturnsManagerPupilBasicDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ReturnsManagerPupilBasicDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewBasicDetailsCurrentPupil(SeleniumHelper.iSIMSUserType.ReturnsManager, textForSearch));
        }
        #endregion

        #region ReturnsManagerPupilTelephoneDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ReturnsManagerPupilTelephoneDetailsAccess()
        {
            Assert.IsFalse(QuickControlAccess.canViewPupilTelephoneDetails(SeleniumHelper.iSIMSUserType.ReturnsManager, textForSearch));
        }
        #endregion

        #region ReturnsManagerPupilEmailDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ReturnsManagerPupilEmailDetailsAccess()
        {
            Assert.IsFalse(QuickControlAccess.canViewPupilEmailDetails(SeleniumHelper.iSIMSUserType.ReturnsManager, textForSearch));
        }
        #endregion

        #region Data Provider for DisplayPupilAddressDetails for Return Manager
        public List<object[]> PupilsWithAddressDataForReturnManager()
        {
            string pattern = "M/d/yyyy";
            string pupilSurname = "ReturnManagerPupilsWithAddressData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string pupilForename = "ReturnManagerPupilsWithAddressData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string dateOfBirth = DateTime.ParseExact("10/02/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/11/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);


            var res = new List<object[]>
            {
                new object[] {pupilSurname, pupilForename, "Female", dateOfBirth,
                DateOfAdmission, "Year 2",new string[]{"222567", "RM House Name12", "RMFlat312", "RMStreet312", "RMDistrict31", "RMCity31", "RMCounty21", "ET2 8CD", "United Kingdom"},}
            };
            return res;
        }
        #endregion

        #region Story 1235 - Check if address are displayed for current Pupils
        [WebDriverTest(DataProvider = "PupilsWithAddressDataForReturnManager", Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void RMCanAccessCurrentPupilAddress(string forenameSetup, string surnameSetup, string gender, string dateOfBirth, string DateOfAdmission, string yearGroup, string[] currentAddress)
        {
            #region Data Preparation

            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();
            #endregion
            BuildPupilRecord.AddBasicLearner(learnerIdSetup, surnameSetup, forenameSetup, dobSetup, dateOfAdmissionSetup, genderCode: "1", enrolStatus: "C");
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);
            //Address Add
            #region
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            AutomationSugar.WaitForAjaxCompletion();

            var pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surnameSetup, forenameSetup);
            pupilRecords.SearchCriteria.IsCurrent = true;
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();
            AutomationSugar.WaitForAjaxCompletion();

            // This sometimes takes an eternity
            PupilRecordTriplet.PupilRecordSearchResultTile pupilTile = null;
            for (var cnt = 0; cnt < 10; cnt++)
            {
                Thread.Sleep(5000);
                pupilTile =
                    pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surnameSetup, forenameSetup)));
                if (pupilTile != null) break;

            }
            var pupilRecord = pupilTile.Click<PupilRecordPage>();
            AutomationSugar.WaitForAjaxCompletion();
            pupilRecord.SelectAddressesTab();

            var addAddress = pupilRecord.ClickAddAddress();
            addAddress.ClickManualAddAddress();
            addAddress.BuildingNo = currentAddress[0];
            addAddress.BuildingName = currentAddress[1];
            addAddress.Flat = currentAddress[2];
            addAddress.Street = currentAddress[3];
            addAddress.District = currentAddress[4];
            addAddress.City = currentAddress[5];
            addAddress.County = currentAddress[6];
            addAddress.PostCode = currentAddress[7];
            addAddress.CountryPostCode = currentAddress[8];
            addAddress.ClickOk(5);

            //Save
            pupilRecord.SavePupil();
            AutomationSugar.WaitForAjaxCompletion();
            #endregion

            POM.Helper.SeleniumHelper.Logout();

            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationByUserType(SeleniumHelper.iSIMSUserType.ReturnsManager);
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(surnameSetup);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsTrue(popup.IsAddressDisplayed());
        }
        #endregion

        #region ReturnsManagerLinkToPupilRecordFromPupilInfoAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ReturnsManagerLinkToPupilRecordFromPupilInfoAccess()
        {
            Assert.IsTrue(QuickControlAccess.hasAccessLinkToPupilRecordFromPupilInfo(SeleniumHelper.iSIMSUserType.ReturnsManager, textForSearch));
        }
        #endregion


        #region Story 1235 - Check if image are displayed for current Pupils
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CannotAccessCurrentPupilImage()
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationByUserType(SeleniumHelper.iSIMSUserType.ReturnsManager);
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsFalse(popup.IsPupilImageDisplayed());
        }
        #endregion

    }
}
