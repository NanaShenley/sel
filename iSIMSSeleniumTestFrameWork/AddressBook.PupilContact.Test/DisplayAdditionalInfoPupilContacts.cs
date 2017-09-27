using AddressBook.Components;
using AddressBook.Components.Pages;
using AddressBook.Test;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;
using Attendance.POM.DataHelper;
using Attendance.POM.Entities;
using System.Globalization;
using SeSugar.Automation;
using SeSugar.Data;
using SeSugar;
using System.Threading;
using WebDriverRunner.webdriver;
using OpenQA.Selenium;
using Attendance.Components;
using POM.Components.Pupil;
using POM.Helper;




namespace AddressBook.PupilContact.Test
{
    public class DisplayAdditionalInfoPupilContacts
    {


        #region Data Provider for Current Pupil Name options
        public List<object[]> PupilContactsWithEmailAndTelephoneData()
        {
            string pattern = "M/d/yyyy";

            string dateOfBirth = DateTime.ParseExact("10/06/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string telNo = "+44 0123456789";
            //    string location = "Other";
            string email = "learnercontact@gmail.com";

            var res = new List<object[]>
            {
                new object[] {dateOfBirth,
                DateOfAdmission, "Year 2",telNo,email}
            };
            return res;
        }
        #endregion


        [WebDriverTest(DataProvider = "PupilContactsWithEmailAndTelephoneData", Enabled = true, Groups = new[] { AddressBookTestGroups.PupilContactsQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DisplayPupilContactTelephoneDetails(string dateOfBirth, string DateOfAdmission, string yearGroup, string telNo, string email)
        {
            #region Data Pupil contact Setup



            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "TestData_Pupil");
            var pupilForename = Utilities.GenerateRandomString(10, "TestData_Pupil" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));

            #endregion

            #region Pre-Condition: Create new contact 1 and refer to pupil

            //Arrange
            AutomationSugar.Log("***Pre-Condition: Create new contact1 and refer to pupil");
            Guid pupilContactId1 = Guid.NewGuid();
            Guid pupilContactRelationshipId1 = Guid.NewGuid();
            //Add pupil contact
            var contactSurname1 = Utilities.GenerateRandomString(10, "Suvarna" + Thread.CurrentThread.ManagedThreadId);
            var contactForename1 = Utilities.GenerateRandomString(10, "Gill" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddPupilContact(pupilContactId1, contactSurname1, contactForename1);
            dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);
            #endregion

            #region Add email and Tel

            //  Create a new pupil, so that it can be attached later to the newly created contact1
            SharedComponents.Helpers.SeleniumHelper.Login(SharedComponents.Helpers.SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");
            var pupilContactTriplet = new PupilContactTriplet();
            pupilContactTriplet.SearchCriteria.ContactName = String.Format("{0}, {1}", contactSurname1, contactForename1);
            Thread.Sleep(2);
            pupilContactTriplet.SearchCriteria.Search();
            pupilContactTriplet.SearchCriteria.Search();
            var resultPupils = pupilContactTriplet.SearchCriteria.Search();
            int count = resultPupils.Count();
            if (count == 1)
            {
                var pupilcontactSearchTile = resultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", contactSurname1, contactForename1)));
                var pupilcontactRecord = pupilcontactSearchTile == null ? null : pupilcontactSearchTile.Click<PupilContactPage>();
                pupilcontactSearchTile.Click<PupilContactPage>();


                // Add Pupil Telephone number


                Wait.WaitUntilDisplayed(By.CssSelector("[data-maintenance-container='LearnerContactTelephones']"));
                pupilcontactRecord = new PupilContactPage();
                pupilcontactRecord.ClickAddTelephoneNumber();
                pupilcontactRecord.TelephoneNumberTable[0].TelephoneNumber = telNo;

                // Add Email Address
                pupilcontactRecord.ClickAddEmailAddress();
                pupilcontactRecord.EmailTable[0].EmailAddress = email;
                pupilContactTriplet.ClickSave();

                #endregion


                AddressBookSearchPage searchBox = new AddressBookSearchPage();
                searchBox.ClearText();
                searchBox.EnterSearchTextForPupilContacts(contactForename1);
                searchBox.ClickOnFirstPupilContactRecord();
                AddressBookPopup popup = new AddressBookPopup();
                popup.GetPupilContactBasicDetails();
                Assert.IsTrue(popup.IsPupilTelephoneDisplayed());
            }
            else throw new Exception();
        }




        #region Story 5660-Display Email Addresses and Email Addresses Icons for Pupil Contacts if recorded
        [WebDriverTest(DataProvider = "PupilContactsWithEmailAndTelephoneData", Enabled = true, Groups = new[] { AddressBookTestGroups.PupilContactsQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DisplayPupilContactEmailDetails(string dateOfBirth, string DateOfAdmission, string yearGroup, string telNo, string email)
        {
            #region Data Pupil contact Setup



            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "Aarav");
            var pupilForename = Utilities.GenerateRandomString(10, "Kumar" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));

            #endregion

            #region Pre-Condition: Create new contact 1 and refer to pupil

            //Arrange
            AutomationSugar.Log("***Pre-Condition: Create new contact1 and refer to pupil");
            Guid pupilContactId1 = Guid.NewGuid();
            Guid pupilContactRelationshipId1 = Guid.NewGuid();
            //Add pupil contact
            var contactSurname1 = Utilities.GenerateRandomString(10, "Sonakshi" + Thread.CurrentThread.ManagedThreadId);
            var contactForename1 = Utilities.GenerateRandomString(10, "Roshan" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddPupilContact(pupilContactId1, contactSurname1, contactForename1);
            dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);
            #endregion

            #region Add email and Tel

            //  Create a new pupil, so that it can be attached later to the newly created contact1
            SharedComponents.Helpers.SeleniumHelper.Login(SharedComponents.Helpers.SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");
            var pupilContactTriplet = new PupilContactTriplet();
            pupilContactTriplet.SearchCriteria.ContactName = String.Format("{0}, {1}", contactSurname1, contactForename1);
            Thread.Sleep(2);
            pupilContactTriplet.SearchCriteria.Search();
            pupilContactTriplet.SearchCriteria.Search();
            var resultPupils = pupilContactTriplet.SearchCriteria.Search();
            int count = resultPupils.Count();
            if (count == 1)
            {
                var pupilcontactSearchTile = resultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", contactSurname1, contactForename1)));
                var pupilcontactRecord = pupilcontactSearchTile == null ? null : pupilcontactSearchTile.Click<PupilContactPage>();
                pupilcontactSearchTile.Click<PupilContactPage>();


                // Add Pupil Telephone number


                Wait.WaitUntilDisplayed(By.CssSelector("[data-maintenance-container='LearnerContactTelephones']"));
                pupilcontactRecord = new PupilContactPage();
                pupilcontactRecord.ClickAddTelephoneNumber();
                pupilcontactRecord.TelephoneNumberTable[0].TelephoneNumber = telNo;

                // Add Email Address
                pupilcontactRecord.ClickAddEmailAddress();
                pupilcontactRecord.EmailTable[0].EmailAddress = email;
                pupilContactTriplet.ClickSave();

                #endregion
                AddressBookSearchPage searchBox = new AddressBookSearchPage();
                searchBox.ClearText();
                searchBox.EnterSearchTextForPupilContacts(contactSurname1);
                searchBox.ClickOnFirstPupilContactRecord();
                AddressBookPopup popup = new AddressBookPopup();
                popup.GetPupilContactBasicDetails();
                Assert.IsTrue(popup.IsEmailDisplayed());
            }
            else
            {
                throw new Exception();
            }
        }
        #endregion


        public List<object[]> Add_Contact()
        {
            var randomName = Thread.CurrentThread.ManagedThreadId + "Contact_WithAddress" + POM.Helper.SeleniumHelper.GenerateRandomString(5) + POM.Helper.SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<object[]>
            {
                new object[]
                {
                    // Forename contact
                    randomName,
                    // Surname contact
                    randomName,
                    // Title
                    "Mr",
                    // Gender
                    "Male",
                    // Salutation,
                    "Prof Billu",
                    // Addressee
                    "Prof Dhillon",
                    // BuildingNo
                    "20",
                    // Street
                    "Bushfoot Road",
                    // District
                    "Portballintrae",
                    // City
                    "Bushmills",
                    // County
                    "",
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

        [WebDriverTest(DataProvider = "Add_Contact", Enabled = true, Groups = new[] { AddressBookTestGroups.PupilContactsQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DisplayPupilContactAddressDetails(string forenameContact, string surnameContact, string title, string gender, string salutation,
            string addressee, string buildingNo, string street, string district, string city,
            string county, string postCode, string countryPostCode, string language, string placeOfWork,
            string jobTitle, string occupation, string priority, string relationship)
        {

            //Create a new pupil, so that it can be attached later to the newly created contact
            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "TestPupil_Surname" + Thread.CurrentThread.ManagedThreadId);
            var pupilForename = Utilities.GenerateRandomString(10, "TestPupil_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 01, 01), new DateTime(2012, 09, 01));

            #region Pre-Condition: Create new contact 1 and refer to pupil

            //Arrange
            AutomationSugar.Log("***Pre-Condition: Create new contact1 and refer to pupil");
            Guid pupilContactId1 = Guid.NewGuid();
            Guid pupilContactRelationshipId1 = Guid.NewGuid();
            //Add pupil contact

            dataPackage.AddPupilContact(pupilContactId1, forenameContact, surnameContact);
            dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);
            #endregion


            AutomationSugar.Log("Data Creation Finished");
            //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            //AutomationSugar.Log("Logged in to the system as School Administrator");
            SharedComponents.Helpers.SeleniumHelper.Login(SharedComponents.Helpers.SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.Log("Logged in to the system as Test User");

            // Navigate to Pupil Contracts
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

            AutomationSugar.Log("Navigated to Pupil Contacts");

            var pupilContactTriplet = new PupilContactTriplet();
            pupilContactTriplet.SearchCriteria.ContactName = String.Format("{0}, {1}", surnameContact, forenameContact);
            Thread.Sleep(4);
            pupilContactTriplet.SearchCriteria.Search();
            var resultPupils = pupilContactTriplet.SearchCriteria.Search();
            int count = resultPupils.Count();
            if (count == 1)
            {
                var pupilcontactSearchTile = resultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surnameContact, forenameContact)));
                var pupilcontactRecord = pupilcontactSearchTile == null ? null : pupilcontactSearchTile.Click<PupilContactPage>();
                pupilcontactSearchTile.Click<PupilContactPage>();

                // Select Addresses Tab
                pupilcontactRecord.SelectAddressesTab();
                var addAddressDialog = pupilcontactRecord.ClickAddanAdditionalAddressLink();
                addAddressDialog.ClickManualAddAddress();
                // Add new address
                addAddressDialog.BuildingNo = buildingNo;
                addAddressDialog.Street = street;
                addAddressDialog.District = district;
                addAddressDialog.City = city;
                addAddressDialog.County = county;
                addAddressDialog.PostCode = postCode;
                addAddressDialog.CountryPostCode = countryPostCode;
                addAddressDialog.ClickOk();
                AutomationSugar.Log("Created a new address to the pupil contact");


                // Save
                pupilContactTriplet.ClickSave();

                AddressBookSearchPage searchBox = new AddressBookSearchPage();
                searchBox.ClearText();
                searchBox.EnterSearchTextForPupilContacts(surnameContact);
                searchBox.ClickOnFirstPupilContactRecord();
                AddressBookPopup popup = new AddressBookPopup();
                Assert.IsTrue(popup.IsAddressDisplayed());

            }
        }
    }
}
