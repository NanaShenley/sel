using AddressBook.Components;
using AddressBook.Components.Pages;
using AddressBook.Test;
using NUnit.Framework;
using POM.Components.Staff;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;

namespace AddressBook.CurrentStaff.Test
{
    public class DisplayAdditionalInfoStaff
    {
        private readonly string textForSearch = "Av";

        #region Story 5657- Display Phone Numbers and Telephone Icons for Staff if recorded
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]

        public void DisplayStaffTelephoneDetails()
        {

            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = SeSugar.Utilities.GenerateRandomString(10, "Staff_AddressBook");
            var surname = SeSugar.Utilities.GenerateRandomString(10, "Staff_AddressBook");
            var startDate = DateTime.Today.AddDays(-1);

            var staffRecordData = StaffRecordPage.CreateStaffRecord(out staffId, forename, surname, startDate);
            DataSetup DataSetStaff = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: staffRecordData);

            #endregion

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            POM.Helper.Wait.WaitLoading();
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
            var staffContactTriplet = new StaffRecordTriplet();
            staffContactTriplet.SearchCriteria.StaffName = String.Format("{0}, {1}", surname, forename);
            //staffContactTriplet.SearchCriteria.Search();
            //staffContactTriplet.SearchCriteria.Search();
            var resultPupils = staffContactTriplet.SearchCriteria.Search();
            int count = resultPupils.Count();
            if (count == 1)
            {
                var staffSearchTile = resultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));
                var staffRecord = staffSearchTile == null ? null : staffSearchTile.Click<StaffRecordPage>();
                staffSearchTile.Click<StaffRecordPage>();
            }
            // Add Pupil Telephone number
            string telNo = "6789867543";

            //   Wait.WaitUntilDisplayed(By.CssSelector("[data-maintenance-container='LearnerContactTelephones']"));
            var Record = new StaffRecordPage();
            Record.SelectPhoneEmailTab();
            Record.ClickAddTelephoneNumber();
            Record.TelephoneNumberTable[0].TelephoneNumber = telNo;
            staffContactTriplet.ClickSave();

            POM.Helper.SeleniumHelper.Logout();
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            searchBox.ClearText();
            searchBox.EnterSearchTextForStaff(surname);
            searchBox.ClickOnFirstStaffRecord();
            AddressBookPopup popup = new AddressBookPopup();
            popup.GetStaffBasicDetails();
            Assert.IsTrue(popup.IsPupilTelephoneDisplayed());
        }
        #endregion

        #region Story 5660-Display Email Addresses and Email Address Icons for staff if recorded
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void DisplayStaffEmailDetails()
        {
            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = SeSugar.Utilities.GenerateRandomString(10, "Staff_AddressBook_Email");
            var surname = SeSugar.Utilities.GenerateRandomString(10, "Staff_AddressBook_Email");
            var startDate = DateTime.Today.AddDays(-1);

            var staffRecordData = StaffRecordPage.CreateStaffRecord(out staffId, forename, surname, startDate);
            DataSetup DataSetStaff = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: staffRecordData);

            #endregion


            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
            POM.Helper.Wait.WaitLoading();
            var staffContactTriplet = new StaffRecordTriplet();
            staffContactTriplet.SearchCriteria.StaffName = String.Format("{0}, {1}", surname, forename);
            //staffContactTriplet.SearchCriteria.Search();
            //staffContactTriplet.SearchCriteria.Search();
            var resultPupils = staffContactTriplet.SearchCriteria.Search();
            int count = resultPupils.Count();
            if (count == 1)
            {
                var staffSearchTile = resultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));
                var staffRecord = staffSearchTile == null ? null : staffSearchTile.Click<StaffRecordPage>();
                staffSearchTile.Click<StaffRecordPage>();
            }


            string emailId = "parul.khullar@capita.co.uk";

            //   Wait.WaitUntilDisplayed(By.CssSelector("[data-maintenance-container='LearnerContactTelephones']"));
            var Record = new StaffRecordPage();
            Record.SelectPhoneEmailTab();
            Record.ClickAddEmailId();
            Record.EmailTable[0].EmailAddress = emailId;
            staffContactTriplet.ClickSave();

            POM.Helper.SeleniumHelper.Logout();
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            searchBox.ClearText();
            searchBox.EnterSearchTextForStaff(surname);
            searchBox.ClickOnFirstStaffRecord();
            AddressBookPopup popup = new AddressBookPopup();
            popup.GetStaffBasicDetails();
            Assert.IsTrue(popup.IsEmailDisplayed());
        }
        #endregion
        public List<object[]> Add_Address()
        {
            var randomName = Thread.CurrentThread.ManagedThreadId + "Contact_WithAddress" + POM.Helper.SeleniumHelper.GenerateRandomString(5) + POM.Helper.SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<object[]>
            {
                new object[]
                {
                   
                    // BuildingNo
                    "20",
                    // Street
                    "Bushfoot Road",
                    // District
                    "Portballintrae",
                    // City
                    "Bushmills",
                    // PostCode
                    "BT57 8QZ",
                    // CountryPostCode
                    "United Kingdom",
                    // Language
                    "English",
                    // PlaceOfWork
                    "Northern Ireland",
                    // JobTitle
                    "",
                    // Occupation
                    "",
                    // Priority
                    "1",
                    // Relationship
                    "Parent"
                }
            };
            return res;
        }
        #region Story 5661-Display Addresses and Addresses Icons for staff
        [WebDriverTest(DataProvider = "Add_Address", Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void DisplayStaffAddressDetails(string buildingNo, string street, string district, string city,
            string postCode, string countryPostCode, string language, string placeOfWork,
            string jobTitle, string occupation, string priority, string relationship)
        {
            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = SeSugar.Utilities.GenerateRandomString(10, "Staff_AddressBook_address");
            var surname = SeSugar.Utilities.GenerateRandomString(10, "Staff_AddressBook_address");
            var startDate = DateTime.Today.AddDays(-1);

            var staffRecordData = StaffRecordPage.CreateStaffRecord(out staffId, forename, surname, startDate);
            DataSetup DataSetStaff = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: staffRecordData);

            #endregion

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
            POM.Helper.Wait.WaitLoading();
            var staffContactTriplet = new StaffRecordTriplet();
            staffContactTriplet.SearchCriteria.StaffName = String.Format("{0}, {1}", surname, forename);
            //staffContactTriplet.SearchCriteria.Search();
            //staffContactTriplet.SearchCriteria.Search();
            var resultPupils = staffContactTriplet.SearchCriteria.Search();
            int count = resultPupils.Count();
            if (count == 1)
            {
                var staffSearchTile = resultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));
                var staffRecord = staffSearchTile == null ? null : staffSearchTile.Click<StaffRecordPage>();
                staffSearchTile.Click<StaffRecordPage>();


               staffRecord.SelectAddressesTab();
                var addAddressDialog = staffRecord.ClickAddAddress();
                addAddressDialog.ClickManualAddAddress();
                // Add new address
                addAddressDialog.BuildingNo = buildingNo;
                addAddressDialog.Street = street;
                addAddressDialog.District = district;
                addAddressDialog.City = city;
                addAddressDialog.PostCode = postCode;
                addAddressDialog.CountryPostCode = countryPostCode;
                addAddressDialog.ClickOk();
                AutomationSugar.Log("Created a new address to the staff record");
                staffContactTriplet.ClickSave();
            }

            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForStaff(surname);
            searchBox.ClickOnFirstStaffRecord();
            AddressBookPopup popup = new AddressBookPopup();
            popup.GetStaffBasicDetails();
            Assert.IsTrue(popup.IsAddressDisplayed());
        }
        #endregion
    }
}
