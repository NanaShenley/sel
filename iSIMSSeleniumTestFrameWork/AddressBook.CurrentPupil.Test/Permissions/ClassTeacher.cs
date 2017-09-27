using AddressBook.Components;
using NUnit.Framework;
using Selene.Support.Attributes;
using SeSugar.Data;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using TestSettings;
using WebDriverRunner.internals;
using Attendance.POM.DataHelper;
using POM.Components.HomePages;
using SeSugar.Automation;
using POM.Components.Pupil;
using System.Threading;

namespace AddressBook.CurrentPupil.Test.Permissions
{
    class ClassTeacher
    {
        private readonly string textForSearch = "Laura";

        #region ClassTeacherQuickControlAccess()
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ClassTeacherQuickControlAccess()
        {
            Assert.IsTrue(QuickControlAccess.isQuickControlAccess(SeleniumHelper.iSIMSUserType.ClassTeacher));
        }
        #endregion

        #region ClassTeacherSearchCurrentPupilsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ClassTeacherSearchCurrentPupilsAccess()
        {
            Assert.That(QuickControlAccess.hasPermissionToSearchCurrentPupil(SeleniumHelper.iSIMSUserType.ClassTeacher, textForSearch) >= 0);
        }
        #endregion

        #region ClassTeacherPupilBasicDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ClassTeacherPupilBasicDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewBasicDetailsCurrentPupil(SeleniumHelper.iSIMSUserType.ClassTeacher, textForSearch));
        }
        #endregion

        #region ClassTeacherPupilTelephoneDetailsAccess
        [WebDriverTest(Enabled = false, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ClassTeacherPupilTelephoneDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewPupilTelephoneDetails(SeleniumHelper.iSIMSUserType.ClassTeacher, textForSearch));
        }
        #endregion

        #region ClassTeacherPupilEmailDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ClassTeacherPupilEmailDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewPupilEmailDetails(SeleniumHelper.iSIMSUserType.ClassTeacher, textForSearch));
        }
        #endregion

        #region Data Provider for DisplayPupilAddressDetails for Class teacher
        public List<object[]> PupilsWithAddressDataForClassTeacher()
        {
            string pattern = "M/d/yyyy";
            string pupilSurname = "ClassTeacherPupilsWithAddressData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string pupilForename = "ClassTeacherPupilsWithAddressData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string dateOfBirth = DateTime.ParseExact("10/02/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/11/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);


            var res = new List<object[]>
            {
                new object[] {pupilSurname, pupilForename, "Female", dateOfBirth,
                DateOfAdmission, "Year 2",new string[]{"222567", "CT House Name12", "CTFlat312", "CTStreet312", "CTDistrict31", "CTCity31", "CTCounty21", "BT2 8EB", "United Kingdom"},}
            };
            return res;
        }
        #endregion

        #region ClassTeacherPupilAddressDetailsAccess
        [WebDriverTest(DataProvider = "PupilsWithAddressDataForClassTeacher", Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ClassTeacherPupilAddressDetailsAccess(string forenameSetup, string surnameSetup, string gender, string dateOfBirth, string DateOfAdmission, string yearGroup, string[] currentAddress)
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

         
            Assert.IsTrue(QuickControlAccess.canViewPupilAddressDetails(SeleniumHelper.iSIMSUserType.ClassTeacher, surnameSetup));
        }
        #endregion

        #region ClassTeacherLinkToPupilRecordFromPupilInfoAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void ClassTeacherLinkToPupilRecordFromPupilInfoAccess()
        {
            Assert.IsTrue(QuickControlAccess.hasAccessLinkToPupilRecordFromPupilInfo(SeleniumHelper.iSIMSUserType.ClassTeacher, textForSearch));
        }
        #endregion
    }
}
