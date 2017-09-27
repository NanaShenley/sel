using AddressBook.Components;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
using POM.Components.Pupil;
using System.Threading;
using System;
using System.Globalization;
using SeSugar.Automation;
using Attendance.POM.DataHelper;
using SeSugar.Data;
using System.Collections.Generic;
using POM.Components.HomePages;

namespace AddressBook.CurrentPupil.Test.Permissions
{
    class SENCoordinator
    {
        private readonly string textForSearch = "ad";

        #region SENCoordinatorQuickControlAccess()
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SENCoordinatorQuickControlAccess()
        {
            Assert.IsTrue(QuickControlAccess.isQuickControlAccess(SeleniumHelper.iSIMSUserType.SENCoordinator));
        }
        #endregion

        #region SENCoordinatorSearchCurrentPupilsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SENCoordinatorSearchCurrentPupilsAccess()
        {
            Assert.That(QuickControlAccess.hasPermissionToSearchCurrentPupil(SeleniumHelper.iSIMSUserType.SENCoordinator, textForSearch) >= 0);
        }
        #endregion

        #region SENCoordinatorPupilBasicDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SENCoordinatorPupilBasicDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewBasicDetailsCurrentPupil(SeleniumHelper.iSIMSUserType.SENCoordinator, textForSearch));
        }
        #endregion

        #region SENCoordinatorPupilTelephoneDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SENCoordinatorPupilTelephoneDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewPupilTelephoneDetails(SeleniumHelper.iSIMSUserType.SENCoordinator, textForSearch));
        }
        #endregion

        #region SENCoordinatorPupilEmailDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SENCoordinatorPupilEmailDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewPupilEmailDetails(SeleniumHelper.iSIMSUserType.SENCoordinator, textForSearch));
        }
        #endregion

        #region Data Provider for DisplayPupilAddressDetails for system Manager
        public List<object[]> PupilsWithAddressDataForSencoordinator()
        {
            string pattern = "M/d/yyyy";
            string pupilSurname = "SencoordPupilsWithAddressData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string pupilForename = "SencoordPupilsWithAddressData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string dateOfBirth = DateTime.ParseExact("10/02/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/11/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);


            var res = new List<object[]>
            {
                new object[] {pupilSurname, pupilForename, "Female", dateOfBirth,
                DateOfAdmission, "Year 2",new string[]{"222567", "SC House Name12", "SCFlat312", "SCStreet312", "SCDistrict31", "SCCity31", "SCCounty21", "ET2 8BD", "United Kingdom"},}
            };
            return res;
        }
        #endregion

        #region SENCoordinatorPupilAddressDetailsAccess
        [WebDriverTest(DataProvider = "PupilsWithAddressDataForSencoordinator", Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SENCoordinatorPupilAddressDetailsAccess(string forenameSetup, string surnameSetup, string gender, string dateOfBirth, string DateOfAdmission, string yearGroup, string[] currentAddress)
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
            SharedComponents.HomePages.TaskMenuBar menu = new SharedComponents.HomePages.TaskMenuBar();
            menu.WaitForTaskMenuBarButton();
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
            

            Assert.IsTrue(QuickControlAccess.canViewPupilAddressDetails(SeleniumHelper.iSIMSUserType.SENCoordinator, surnameSetup));
        }
        #endregion

        #region SENCoordinatorLinkToPupilRecordFromPupilInfoAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SENCoordinatorLinkToPupilRecordFromPupilInfoAccess()
        {
            Assert.IsTrue(QuickControlAccess.hasAccessLinkToPupilRecordFromPupilInfo(SeleniumHelper.iSIMSUserType.SENCoordinator, textForSearch));
        }
        #endregion
    }
}
