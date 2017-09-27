using AddressBook.Components;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
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
    class SystemManger
    {
        private readonly string textForSearch = "ad";

        #region SystemMangerQuickControlAccess()
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SystemMangerQuickControlAccess()
        {
            Assert.IsTrue(QuickControlAccess.isQuickControlAccess(SeleniumHelper.iSIMSUserType.SystemManger));
        }
        #endregion

        #region SystemMangerSearchCurrentPupilsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SystemMangerSearchCurrentPupilsAccess()
        {
            Assert.That(QuickControlAccess.hasPermissionToSearchCurrentPupil(SeleniumHelper.iSIMSUserType.SystemManger, textForSearch) >= 0);
        }
        #endregion

        #region SystemMangerPupilBasicDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SystemMangerPupilBasicDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewBasicDetailsCurrentPupil(SeleniumHelper.iSIMSUserType.SystemManger, textForSearch));
        }
        #endregion

        #region SystemMangerPupilTelephoneDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SystemMangerPupilTelephoneDetailsAccess()
        {
            Assert.IsFalse(QuickControlAccess.canViewPupilTelephoneDetails(SeleniumHelper.iSIMSUserType.SystemManger, textForSearch));
        }
        #endregion

        #region SystemMangerPupilEmailDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SystemMangerPupilEmailDetailsAccess()
        {
            Assert.IsFalse(QuickControlAccess.canViewPupilEmailDetails(SeleniumHelper.iSIMSUserType.SystemManger, textForSearch));
        }
        #endregion


        #region Data Provider for DisplayPupilAddressDetails for system Manager
        public List<object[]> PupilsWithAddressDataForSystemManager()
        {
            string pattern = "M/d/yyyy";
            string pupilSurname = "SystemManagerPupilsWithAddressData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string pupilForename = "SystemManagerPupilsWithAddressData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string dateOfBirth = DateTime.ParseExact("10/02/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/11/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);


            var res = new List<object[]>
            {
                new object[] {pupilSurname, pupilForename, "Female", dateOfBirth,
                DateOfAdmission, "Year 2",new string[]{"2221567", "SM House Name121", "SMFlat3121", "SMStreet3121", "RMDistrict311", "RMCity311", "RMCounty21", "ET2 8FD", "United Kingdom"},}
            };
            return res;
        }
        #endregion


        #region SystemMangerPupilAddressDetailsAccess
        [WebDriverTest(DataProvider = "PupilsWithAddressDataForSystemManager", Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SystemMangerPupilAddressDetailsAccess(string forenameSetup, string surnameSetup, string gender, string dateOfBirth, string DateOfAdmission, string yearGroup, string[] currentAddress)
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

            
            Assert.IsFalse(QuickControlAccess.canViewPupilAddressDetails(SeleniumHelper.iSIMSUserType.SystemManger, surnameSetup));
        }
        #endregion

        #region SystemMangerLinkToPupilRecordFromPupilInfoAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SystemMangerLinkToPupilRecordFromPupilInfoAccess()
        {
            Assert.IsFalse(QuickControlAccess.hasAccessLinkToPupilRecordFromPupilInfo(SeleniumHelper.iSIMSUserType.SystemManger, textForSearch));
        }
        #endregion
    }
}
