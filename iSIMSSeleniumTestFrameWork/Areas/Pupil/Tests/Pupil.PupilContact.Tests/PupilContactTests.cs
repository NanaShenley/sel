using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using TestSettings;
using Selene.Support.Attributes;
using POM.Components.Pupil.Dialogs;
using POM.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POM.Components.Common;

namespace Pupil.SENRecords.Tests
{
    public class PupilContactTests
    {
        #region Private Parameters

        private readonly int tenantID = SeSugar.Environment.Settings.TenantId;
        private readonly DateTime startDate = DateTime.Today.AddDays(-1);
        private readonly string telephoneAutomationID = "update_button";
        private readonly string emailAutomationID = "update_button";
        private readonly string coresidentMatchedAutomationID = "update_button";

        #endregion

        #region Pupil Contact

        [WebDriverTest(TimeoutSeconds = 1200,
            Enabled = true,
            Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilContact.Create, PupilTestGroups.Priority.Priority1, },
            DataProvider = "ADD_NEW_PUPIL_CONTACT_DATA")]
        public void Can_Add_A_New_Pupil_Contact(
            string forenameContact, string surnameContact, string title, string gender, string salutation,
            string addressee, string buildingNo, string street, string district, string city,
            string county, string postCode, string countryPostCode, string language, string placeOfWork,
            string jobTitle, string occupation, string priority, string relationship)
        {
            #region DataSetup

            //Create a new pupil, so that it can be attached later to the newly created contact
            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "TestPupil_Surname" + Thread.CurrentThread.ManagedThreadId);
            var pupilForename = Utilities.GenerateRandomString(10, "TestPupil_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 01, 01), new DateTime(2012, 09, 01));

            #endregion

            #region Steps

            //Act and Clean
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps

                AutomationSugar.Log("Data Creation Finished");
                //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                //AutomationSugar.Log("Logged in to the system as School Administrator");
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                AutomationSugar.Log("Logged in to the system as Test User");

                // Navigate to Pupil Contracts
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

                AutomationSugar.Log("Navigated to Pupil Contacts");
                // Create new pupil contact
                var pupilContactTriplet = new PupilContactTriplet();
                var pupilContactPage = pupilContactTriplet.ClickCreate();

                // Add data for Personal Details
                pupilContactPage.SelectPersonalDetailsTab();
                pupilContactPage.Title = title;
                pupilContactPage.Forename = forenameContact;
                pupilContactPage.Surname = surnameContact;
                pupilContactPage.Gender = gender;
                pupilContactPage.Salutation = salutation;
                pupilContactPage.Addressee = addressee;

                // Save
                pupilContactTriplet.ClickSave();
                AutomationSugar.Log("Created a new pupil contact");

                // Select Addresses Tab
                pupilContactPage.SelectAddressesTab();
                pupilContactPage.ClickAddAddress();

                var addAddressDialog = new AddAddressDialog();

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

                // Select Ethnic/Cultural
                pupilContactPage.SelectEthnicCulturalTab();
                Thread.Sleep(1000);
                pupilContactPage.Language = language;

                // Select Job Details tab
                pupilContactPage.SelectJobDetailsTab();
                Thread.Sleep(1000);
                pupilContactPage.PlaceOfWork = placeOfWork;
                pupilContactPage.JobTitle = jobTitle;
                pupilContactPage.Occupation = occupation;
                AutomationSugar.Log("Updated language and job details of the pupil contact");

                // Save
                pupilContactTriplet.ClickSave();

                // Select Associated Pupils tab
                pupilContactPage.SelectAssociatedPupilsTab();
                var addAssociatedPupilsTripletDialog = pupilContactPage.ClickAddPupilLink();
                addAssociatedPupilsTripletDialog.SearchCriteria.PupilName = pupilForename;
                var associatedPupilResults = addAssociatedPupilsTripletDialog.SearchCriteria.Search();
                associatedPupilResults.SingleOrDefault(
                    x => x.Name.Trim().Equals(pupilSurname + ", " + pupilForename))
                    .Click();
                addAssociatedPupilsTripletDialog.ClickOk();

                // Record the data for the associated pupil
                var pupilTable = pupilContactPage.PupilTable;
                var pupilRow = pupilTable.Rows.SingleOrDefault(
                    x => x.LegalForename.Trim().Equals(pupilForename)
                    && x.LegalSurname.Trim().Equals(pupilSurname));

                if (pupilRow != null)
                {
                    pupilRow.Priority = priority;
                    pupilRow.Relationship = relationship;
                    pupilRow.ParentalResponsibility = true;
                    pupilRow.ReceivesCorrespondance = true;
                    pupilRow.SchoolReport = true;
                    pupilRow.CourtOrder = false;
                }

                // Save
                pupilContactTriplet.ClickSave();
                AutomationSugar.Log("Linked the newly created pupil with the new pupil contact");
                // Verify that New 'Pupil Contact' record has been added.
                Assert.AreEqual(true, pupilContactPage.IsSuccessMessageDisplayed(), "New 'Pupil Contact' record has been added unsuccessfully.");

                // Navigate to Pupil Records
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

                // Search a pupil
                var pupilRecordTriplet = new PupilSearchTriplet();
                pupilRecordTriplet.SearchCriteria.IsCurrent = true;
                pupilRecordTriplet.SearchCriteria.IsFuture = true;
                pupilRecordTriplet.SearchCriteria.IsLeaver = false;
                pupilRecordTriplet.SearchCriteria.PupilName = pupilSurname + ", " + pupilForename;
                var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
                var pupilRecordPage =
                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilSurname + ", " + pupilForename))
                        .Click<PupilRecordPage>();

                // Go to Family/Home section
                pupilRecordPage.SelectFamilyHomeTab();
                var contacts = pupilRecordPage.ContactTable;

                // Verify that Pupil Contact's record now displays
                // in the 'Contacts' grid in the 'Family/Contact' section.
                Assert.AreEqual(true, contacts.Rows.Any(x => x.Name.Trim().Contains(forenameContact + " " + surnameContact)), "Pupil Contact's record isn't displayed in the 'Contacts' grid");
                AutomationSugar.Log("Done the verifications");

                #endregion

                #region Post-Condition: Delete the contact

                Console.WriteLine("***Post-Condition: Delete the contact");
                // Navigate to Pupil Contracts
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");
                pupilContactPage.ClickDelete();
                pupilContactPage.ContinueDelete();
                #endregion

                AutomationSugar.Log("Deleted the newly pupil contact");
            }
            AutomationSugar.Log("Successfully Purged the created data");

            #endregion
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilContact.Page,
                PupilTestGroups.Priority.Priority2
            })]
        public void Can_Search_For_An_Existing_Pupil_Contact()
        {
            #region DataSetup

            AutomationSugar.Log("DataSetup Started.");
            //Arrange
            Guid pupilContactId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();

            //Add a pupil contact
            var contactSurname = Utilities.GenerateRandomString(10, "TestPupilContact_Surname" + Thread.CurrentThread.ManagedThreadId);
            var contactForename = Utilities.GenerateRandomString(10, "TestPupilContact_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddPupilContact(pupilContactId, contactSurname, contactForename);

            #endregion

            #region Steps

            //Act and Clean
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                string expectedPreferredName = contactSurname + ", " + contactForename;
                AutomationSugar.Log("DataSetup Completed.");
                //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                //AutomationSugar.Log("Logged in to the system as School Administrator");
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                AutomationSugar.Log("Logged in to the system as Test User");

                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");
                AutomationSugar.Log("Navigated to Pupil contacts successfully.");
                AutomationSugar.Log("Search with forename for pupil " + expectedPreferredName + " started.");
                var pupilContactTriplet = new PupilContactTriplet();
                pupilContactTriplet.SearchCriteria.ContactName = contactForename;
                var pupilContactResults = pupilContactTriplet.SearchCriteria.Search();
                Assert.AreNotEqual(null, pupilContactResults.FirstOrDefault(x => x.Name.Trim().Equals(expectedPreferredName)), "The contact isn't displayed.");
                AutomationSugar.Log("Search with forename for pupil " + expectedPreferredName + " finished.");
                AutomationSugar.Log("Search with surname for pupil " + expectedPreferredName + " started.");
                pupilContactTriplet.SearchCriteria.ContactName = contactSurname;
                pupilContactResults = pupilContactTriplet.SearchCriteria.Search();
                Assert.AreNotEqual(null, pupilContactResults.FirstOrDefault(x => x.Name.Trim().Equals(expectedPreferredName)), "The contact isn't displayed.");
                AutomationSugar.Log("Search with forename for pupil " + expectedPreferredName + " finished.");
                AutomationSugar.Log("Search with forename & surname for pupil " + expectedPreferredName + " started.");
                pupilContactTriplet.SearchCriteria.ContactName = expectedPreferredName;
                pupilContactResults = pupilContactTriplet.SearchCriteria.Search();
                Assert.AreNotEqual(null, pupilContactResults.FirstOrDefault(x => x.Name.Trim().Equals(expectedPreferredName)), "The contact isn't displayed.");
                AutomationSugar.Log("Search with forename & surname for pupil " + expectedPreferredName + " finished.");
            }

            #endregion
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true,
            Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilContact.Delete, PupilTestGroups.Priority.Priority2 })]
        public void Can_Delete_A_PupilContact_ViaPupilRecord()
        {
            #region Data Setup

            #region Create a new pupil, so that it can be attached later to the newly created contact1

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "TestPupil_Surname" + Thread.CurrentThread.ManagedThreadId);
            var pupilForename = Utilities.GenerateRandomString(10, "TestPupil_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 01, 01), new DateTime(2012, 09, 01));
            #endregion

            #region Pre-Condition: Create new contact 1 and refer to pupil

            //Arrange
            AutomationSugar.Log("***Pre-Condition: Create new contact1 and refer to pupil");
            Guid pupilContactId1 = Guid.NewGuid();
            Guid pupilContactRelationshipId1 = Guid.NewGuid();
            //Add pupil contact
            var contactSurname1 = Utilities.GenerateRandomString(10, "TestPupilContact1_Surname" + Thread.CurrentThread.ManagedThreadId);
            var contactForename1 = Utilities.GenerateRandomString(10, "TestPupilContact1_Forename" + Thread.CurrentThread.ManagedThreadId);
            var titleContact1 = "Mr";
            dataPackage.AddPupilContact(pupilContactId1, contactSurname1, contactForename1);
            dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1);
            #endregion


            #endregion

            #region Steps

            //Act and Clean
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region STEPS

                //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                //AutomationSugar.Log("Logged in to the system as School Administrator");
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                AutomationSugar.Log("Logged in to the system as Test User");

                // Navigate to Pupil Contracts
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

                // Search a pupil
                var pupilRecordTriplet = new PupilSearchTriplet();
                pupilRecordTriplet.SearchCriteria.IsCurrent = true;
                pupilRecordTriplet.SearchCriteria.IsFuture = true;
                pupilRecordTriplet.SearchCriteria.IsLeaver = false;
                pupilRecordTriplet.SearchCriteria.PupilName = pupilForename + ", " + pupilSurname;
                var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
                var pupilRecordPage =
                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilSurname + ", " + pupilForename))
                        .Click<PupilRecordPage>();

                // Go to Family/Home section
                pupilRecordPage.SelectFamilyHomeTab();

                // Delete a contact
                var contacts = pupilRecordPage.ContactTable;
                var contactRows = contacts.Rows;
                var contactDeleted =
                    contactRows.SingleOrDefault(
                        x => x.Name.Trim().Equals(titleContact1 + " " + contactForename1 + " " + contactSurname1));
                contactDeleted.DeleteRow();

                // Save
                pupilRecordPage.SavePupil();

                // Navigate to Pupil Contracts
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

                // Search the contact
                var pupilContactTriplet = new PupilContactTriplet();
                pupilContactTriplet.SearchCriteria.ContactName = contactForename1;
                var pupilContactResults = pupilContactTriplet.SearchCriteria.Search();
                var pupilContact =
                    pupilContactResults.SingleOrDefault(
                        x => x.Name.Trim().Equals(contactSurname1 + ", " + contactForename1));
                var pupilContactPage = pupilContact.Click<PupilContactPage>();

                // Select Associated Pupils tab
                pupilContactPage.SelectAssociatedPupilsTab();
                var pupils = pupilContactPage.PupilTable;

                // Verify that there is no longer a link to the pupil record
                // where the 'Pupil Contact' was deleted.
                Assert.AreEqual(false, pupils.Rows.Any(x => x.LegalForename.Trim().Equals(pupilSurname)
                                                            && x.LegalSurname.Trim().Equals(pupilForename)),
                    "Existing link to the pupil record.");

                #endregion
            }

            #endregion
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true,
            Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilContact.Delete, PupilTestGroups.Priority.Priority2 })]
        public void Can_Delete_A_PupilContact_ViaPupilContact()
        {
            #region Data Setup

            #region Create a new pupil, so that it can be attached later to the newly created contact1

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "TestPupil_Surname" + Thread.CurrentThread.ManagedThreadId);
            var pupilForename = Utilities.GenerateRandomString(10, "TestPupil_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 01, 01), new DateTime(2012, 09, 01));
            #endregion

            #region Pre-Condition: Create new contact 2 and refer to pupil

            AutomationSugar.Log("***Pre-Condition: Create new contact2 and refer to pupil");
            Guid pupilContactId2 = Guid.NewGuid();
            Guid pupilContactRelationshipId2 = Guid.NewGuid();
            //Add pupil contact
            var contactSurname2 = Utilities.GenerateRandomString(10, "TestPupilContact2_Surname" + Thread.CurrentThread.ManagedThreadId);
            var contactForename2 = Utilities.GenerateRandomString(10, "TestPupilContact2_Forename" + Thread.CurrentThread.ManagedThreadId);
            var titleContact2 = "Mr";
            dataPackage.AddPupilContact(pupilContactId2, contactSurname2, contactForename2);
            dataPackage.AddPupilContactRelationship(pupilContactRelationshipId2, pupilId, pupilContactId2);
            #endregion

            #endregion

            #region Steps

            //Act and Clean
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region STEPS

                //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                //AutomationSugar.Log("Logged in to the system as School Administrator");
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                AutomationSugar.Log("Logged in to the system as Test User");

                // Navigate to Pupil Contracts
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

                // Search the contact
                var pupilContactTriplet = new PupilContactTriplet();
                pupilContactTriplet.SearchCriteria.ContactName = contactForename2;
                var pupilContactResults = pupilContactTriplet.SearchCriteria.Search();
                var pupilContact =
                    pupilContactResults.SingleOrDefault(
                        x => x.Name.Trim().Equals(contactSurname2 + ", " + contactForename2));
                var pupilContactPage = pupilContact.Click<PupilContactPage>();

                // Delete contact
                pupilContactPage.ClickDelete();
                var warningConfirmDialog = new WarningConfirmationDialog();
                warningConfirmDialog.ConfirmDelete();

                // Navigate to Pupil Contracts
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

                // Search a pupil
                var pupilRecordTriplet = new PupilSearchTriplet();
                pupilRecordTriplet.SearchCriteria.IsCurrent = true;
                pupilRecordTriplet.SearchCriteria.IsFuture = true;
                pupilRecordTriplet.SearchCriteria.IsLeaver = false;
                pupilRecordTriplet.SearchCriteria.PupilName = pupilForename + ", " + pupilSurname;
                var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
                var pupilRecordPage =
                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilSurname + ", " + pupilForename))
                        .Click<PupilRecordPage>();

                // Go to Family/Home section
                pupilRecordPage.SelectFamilyHomeTab();

                // Verify that A Pupil Contact record is deleted in its entirety
                // and is no longer a contact for any pupil previously identified with this contact.
                var contacts = pupilRecordPage.ContactTable;
                Assert.AreEqual(false,
                    contacts.Rows.Any(
                        x => x.Name.Trim().Equals(titleContact2 + " " + contactForename2 + " " + contactSurname2)),
                    "Fail: Exist the contact");

                #endregion
            }

            #endregion
        }

        #endregion

        #region Shared Contact

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilContact.Page,
                PupilTestGroups.Priority.Priority1,
                "SharedContactDetails"
            })]
        public void Shared_Contact_Update_Matches_PupilContactScreen_Telephone()
        {
            //Arrange
            #region IDs

            Guid pupilID,
                staffID,
                pupilContactID,
                staffContactID;

            #endregion

            #region Values

            string oldTelephoneNumber = GenerateNumber();
            string newTelephoneNumber = oldTelephoneNumber + "new";

            #endregion

            #region Data

            DataPackage data = CreateData(out pupilID, out staffID, out pupilContactID, out staffContactID);
            CreateTelephoneData(data, oldTelephoneNumber, pupilID, staffID, pupilContactID, staffContactID);

            #endregion

            //Act
            using (new DataSetup(data))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                PupilContactPage pupilContact = LoadPupilContact(pupilContactID);

                //Update Pupil Contact Telephone
                pupilContact.SelectContactDetailTab();
                var gridRow = pupilContact.TelephoneNumberTable.Rows[0];
                gridRow.TelephoneNumber = newTelephoneNumber;

                //Matches
                var matchesDialog = new SharedContactDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.Matches.Rows[2].Selected = true;
                matchesDialog.ClickSave(telephoneAutomationID);

                //Pupil Contact
                gridRow = pupilContact.TelephoneNumberTable.Rows[0];
                Assert.AreEqual(newTelephoneNumber, gridRow.TelephoneNumber);

                //Pupil
                PupilRecordPage pupil = LoadPupil(pupilID);
                pupil.SelectPhoneEmailTab();
                var ptGridRow = pupil.TelephoneNumberTable.Rows[0];
                Assert.AreEqual(newTelephoneNumber, ptGridRow.TelephoneNumber);

                //Staff
                StaffRecordPage staff = LoadStaff(staffID);
                staff.SelectPhoneEmailTab();
                var sGridRow = staff.TelephoneNumberTable.Rows[0];
                Assert.AreEqual(newTelephoneNumber, sGridRow.TelephoneNumber);

                //Staff Contact
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                var sContactDialog = new EditStaffContactDialog();
                var sctGridRow = sContactDialog.StaffContactTelephones.Rows[0];
                Assert.AreEqual(newTelephoneNumber, sctGridRow.TelephoneNumber);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                        PupilTestGroups.PupilContact.Page,
                        PupilTestGroups.Priority.Priority1,
                        "SharedContactDetails"
            })]
        public void Shared_Contact_Update_Matches_PupilContactScreen_Email()
        {
            //Arrange
            #region IDs

            Guid pupilID,
                staffID,
                pupilContactID,
                staffContactID;

            #endregion

            #region Values

            var email = SeleniumHelper.GenerateRandomString(10);
            string oldEmail = email + "@capita.co.uk";
            string newEmail = email + "new@capita.co.uk";

            #endregion

            #region Data

            DataPackage data = CreateData(out pupilID, out staffID, out pupilContactID, out staffContactID);
            CreateEmailData(data, oldEmail, pupilID, staffID, pupilContactID, staffContactID);

            #endregion

            //Act
            using (new DataSetup(data))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                PupilContactPage pupilContact = LoadPupilContact(pupilContactID);

                //Update Pupil Contact Telephone
                pupilContact.SelectContactDetailTab();
                var gridRow = pupilContact.EmailTable.Rows[0];
                gridRow.EmailAddress = newEmail;

                //Matches
                var matchesDialog = new SharedContactDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.Matches.Rows[2].Selected = true;
                matchesDialog.ClickSave(emailAutomationID);

                //Pupil Contact
                gridRow = pupilContact.EmailTable.Rows[0];
                Assert.AreEqual(newEmail, gridRow.EmailAddress);

                //Pupil
                PupilRecordPage pupil = LoadPupil(pupilID);
                pupil.SelectPhoneEmailTab();
                var ptGridRow = pupil.EmailTable.Rows[0];
                Assert.AreEqual(newEmail, ptGridRow.EmailAddress);

                //Staff
                StaffRecordPage staff = LoadStaff(staffID);
                staff.SelectPhoneEmailTab();
                var sGridRow = staff.EmailTable.Rows[0];
                Assert.AreEqual(newEmail, sGridRow.EmailAddress);

                //Staff Contact
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                var sContactDialog = new EditStaffContactDialog();
                var sctGridRow = sContactDialog.StaffContactEmails.Rows[0];
                Assert.AreEqual(newEmail, sctGridRow.EmailAddress);
            }
        }

        #endregion

        #region Main Contact Screen

        #region Add Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Add", "Add_Pupil_Contact_Address_Local")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
         Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase)]
        public void Add_Pupil_Contact_Address_Local()
        {
            #region Arrange

            Guid pupilContactId;
            Guid addressID;

            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(SAON, _seperator,
                PAONDescription, _seperator,
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(SAON, _seperator,
                PAONDescription, _lineSeperator,
                PAONRange, _space, Street, _lineSeperator,
                Locality, _lineSeperator,
                Town, _lineSeperator,
                AdministrativeArea, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            #endregion

            using (new DataSetup(CreatePupilContact(out pupilContactId, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");

                //Get Pupil Contact record
                PupilContactPage pupilContactRecordPage = LoadPupilContact(pupilContactId);
                pupilContactRecordPage.SelectAddressesTab();
                pupilContactRecordPage.ClickAddAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONDescription, addAddress.BuildingName);
                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(SAON, addAddress.Flat);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Locality, addAddress.District);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(AdministrativeArea, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);
                Assert.AreEqual(Country, addAddress.Country);

                addAddress.ClickOk();
                pupilContactRecordPage.ClickSave();

                pupilContactRecordPage = new PupilContactPage();
                pupilContactRecordPage.SelectAddressesTab();

                var gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                Assert.AreEqual(addressDisplayLarge, gridRow.Address);
            }
        }

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Add", "Add_Pupil_Contact_Address_WAV")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Pupil_Contact_Address_WAV()
        {
            #region Arrange

            Guid pupilContactId;

            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string PAONRange = "22";
            string Street = "SUDELEY WALK";
            string Town = "BEDFORD";
            string PostCode = "MK41 8HS";
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(
                PAONRange, _space, Street, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            #endregion

            using (new DataSetup(CreatePupilContact(out pupilContactId, surname)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");

                //Get Pupil Contact record
                PupilContactPage pupilContactRecordPage = LoadPupilContact(pupilContactId);
                pupilContactRecordPage.SelectAddressesTab();
                pupilContactRecordPage.ClickAddAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(Town, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);

                addAddress.ClickOk();
                pupilContactRecordPage.ClickSave();

                pupilContactRecordPage = new PupilContactPage();
                pupilContactRecordPage.SelectAddressesTab();

                var gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                Assert.AreEqual(addressDisplayLarge, gridRow.Address);
            }
        }

        #endregion

        #region Edit Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Edit", "Edit_Pupil_Contact_Address_Fields")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Pupil_Contact_Address_Fields()
        {
            #region Arrange

            Guid pupilContactId;
            Guid addressID, pupilContactAddressID;

            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            #endregion

            using (new DataSetup(
                CreatePupilContact(out pupilContactId, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");

                //Get Pupil Contact record
                PupilContactPage pupilContactRecordPage = LoadPupilContact(pupilContactId);
                pupilContactRecordPage.SelectAddressesTab();
                var gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.District = newLocality;
                addAddress.ClickOk();
                pupilContactRecordPage.ClickSave();

                pupilContactRecordPage = new PupilContactPage();
                pupilContactRecordPage.SelectAddressesTab();
                gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                addAddress = new AddAddressPopup();
                Assert.AreEqual(newLocality, addAddress.District);
            }
        }

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Edit", "Edit_Pupil_Contact_Address_Fields_Coresident")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
           Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
           Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
           Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Pupil_Contact_Address_Fields_Coresident()
        {
            #region Arrange

            Guid pupilContactId_One,pupilContactId_Two;
            Guid addressID_One, addressID_Two, pupilContactAddressID_One, pupilContactAddressID_Two;

            string surname_one = Utilities.GenerateRandomString(6, "Selenium_One");
            string surname_two = Utilities.GenerateRandomString(6, "Selenium_Two");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            #endregion
                using (new DataSetup(
                CreatePupilContact(out pupilContactId_One, surname_one),
                GetAddress(out addressID_One, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID_One, DateTime.Today, "H", addressID_One, pupilContactId_One),
                CreatePupilContact(out pupilContactId_Two, surname_two),
                GetAddress(out addressID_Two, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID_Two, DateTime.Today, "H", addressID_Two, pupilContactId_Two)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");

                //Get Pupil Contact record
                PupilContactPage pupilContactRecordPage = LoadPupilContact(pupilContactId_One);
                pupilContactRecordPage.SelectAddressesTab();
                var gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.District = newLocality;
                addAddress.ClickOk();

                // co residents match dialog
                var matchesDialog = new SharedAddressDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.ClickSave(coresidentMatchedAutomationID);
                //pupilContactRecordPage.ClickSave();

                pupilContactRecordPage = new PupilContactPage();
                pupilContactRecordPage.SelectAddressesTab();
                gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                addAddress = new AddAddressPopup();
                Assert.AreEqual(newLocality, addAddress.District);
                addAddress.ClickCancel();

                pupilContactRecordPage = LoadPupilContact(pupilContactId_Two);
                pupilContactRecordPage = new PupilContactPage();
                pupilContactRecordPage.SelectAddressesTab();
                gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                addAddress = new AddAddressPopup();
                Assert.AreEqual(newLocality, addAddress.District);
            }
        }

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Edit", "Edit_Pupil_Contact_Address_New_Address")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Pupil_Contact_Address_New_Address()
        {
            #region Arrange

            Guid pupilContactId;
            Guid addressID, pupilContactAddressID;

            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string WAVPAONRange = "22";
            string WAVStreet = "SUDELEY WALK";
            string WAVTown = "BEDFORD";
            string WAVPostCode = "MK41 8HS";

            #endregion

            using (new DataSetup(
                CreatePupilContact(out pupilContactId, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");

                //Get Pupil Contact record
                PupilContactPage pupilContactRecordPage = LoadPupilContact(pupilContactId);
                pupilContactRecordPage.SelectAddressesTab();
                var gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();

                addAddress.ClickOk();
                pupilContactRecordPage.ClickSave();

                pupilContactRecordPage = new PupilContactPage();
                pupilContactRecordPage.SelectAddressesTab();
                gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                addAddress = new AddAddressPopup();

                Assert.AreEqual(WAVPAONRange, addAddress.BuildingNo);
                Assert.AreEqual(WAVStreet, addAddress.Street);
                Assert.AreEqual(WAVTown, addAddress.Town);
                Assert.AreEqual(WAVPostCode, addAddress.PostCode);
            }
        }

        #endregion

        #region Delete Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Delete", "Delete_Pupil_Contact_Address")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Delete_Pupil_Contact_Address()
        {
            #region Arrange

            Guid pupilContactId;
            Guid addressID, pupilContactAddressID;

            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            #endregion

            using (new DataSetup(
                CreatePupilContact(out pupilContactId, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");

                //Get Pupil Contact record
                PupilContactPage pupilContactRecordPage = LoadPupilContact(pupilContactId);
                pupilContactRecordPage.SelectAddressesTab();
                var gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                gridRow.DeleteRow();

                pupilContactRecordPage.ClickSave();

                pupilContactRecordPage = new PupilContactPage();
                pupilContactRecordPage.SelectAddressesTab();

                int count = pupilContactRecordPage.AddressTable.Rows.Count;

                Assert.AreEqual(0, count);
            }
        }

        #endregion

        #region Move Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Move", "Move_Pupil_Contact_Address")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Move_Pupil_Contact_Address()
        {
            #region Arrange

            Guid pupilContactId;
            Guid addressID, pupilContactAddressID;

            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string WAVPAONRange = "22";
            string WAVPostCode = "MK41 8HS";

            DateTime moveDate = DateTime.Today.AddDays(5);

            #endregion

            using (new DataSetup(
                CreatePupilContact(out pupilContactId, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");

                //Get Pupil Contact record
                PupilContactPage pupilContactRecordPage = LoadPupilContact(pupilContactId);
                pupilContactRecordPage.SelectAddressesTab();
                var gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                gridRow.ClickMoveAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.MoveDate = moveDate.ToShortDateString();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();

                addAddress.ClickOk();
                pupilContactRecordPage.ClickSave();

                pupilContactRecordPage = new PupilContactPage();
                pupilContactRecordPage.SelectAddressesTab();

                gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                var newGridRow = pupilContactRecordPage.AddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), newGridRow.StartDate);
            }
        }


        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Move", "Move_Pupil_Contact_Address_CoResident")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
       Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Move_Pupil_Contact_Address_CoResident()
        {
            #region Arrange

            Guid pupilContactId_One, pupilContactId_Two, pupilId_Three;
            Guid addressID_One, addressID_Two, pupilContactAddressID_One, pupilContactAddressID_Two, addressID_Three, pupilAddressID_Three;
            string name_One = Utilities.GenerateRandomString(6, "Name_One");
            string name_Two = Utilities.GenerateRandomString(6, "Name_Two");
            string surname_One = Utilities.GenerateRandomString(6, "Selenium_One");
            string surname_Two = Utilities.GenerateRandomString(6, "Selenium_Two");
            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";
            string WAVPAONRange = "22";
            string WAVPostCode = "MK41 8HS";
            DateTime moveDate = DateTime.Today.AddDays(5);

            #endregion

            using (new DataSetup(
                CreatePupilContact(out pupilContactId_One, surname_One),
                GetAddress(out addressID_One, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID_One, DateTime.Today, "H", addressID_One, pupilContactId_One),
                CreatePupilContact(out pupilContactId_Two, surname_Two),
                GetAddress(out addressID_Two, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID_Two, DateTime.Today, "H", addressID_Two, pupilContactId_Two),
                GetPupilRecord_current(out pupilId_Three),
                GetAddress(out addressID_Three, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilAddress(out pupilAddressID_Three, DateTime.Today, "H", addressID_Three, pupilId_Three)
                ))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");

                //Get Pupil Contact record
                PupilContactPage pupilContactRecordPage = LoadPupilContact(pupilContactId_One);
                pupilContactRecordPage.SelectAddressesTab();
                var gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                gridRow.ClickMoveAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.MoveDate = moveDate.ToShortDateString();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();
                addAddress.Addresses = WAVPAONRange;
                Wait.WaitLoading();
                addAddress.ClickOk();

                // co residents match dialog
                var matchesDialog = new SharedAddressDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.ClickSave(coresidentMatchedAutomationID);

                pupilContactRecordPage = new PupilContactPage();
                pupilContactRecordPage.SelectAddressesTab();

                gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                var newGridRow = pupilContactRecordPage.AddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), newGridRow.StartDate);

                pupilContactRecordPage = LoadPupilContact(pupilContactId_Two);
                pupilContactRecordPage.SelectAddressesTab();

                gridRow = pupilContactRecordPage.AddressTable.Rows[0];
                newGridRow = pupilContactRecordPage.AddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), newGridRow.StartDate);

                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                var pupilRecordPage = LoadPupil_Record(pupilId_Three);
                pupilRecordPage.SelectAddressesTab();
                var gridRow_Record = pupilRecordPage.AddressTable.Rows[0];
                var newGridRow_Record = pupilRecordPage.AddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow_Record.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), newGridRow_Record.StartDate);
            }
        }

        #endregion

        #endregion

        #region Edit Contact Popup

        #region Add Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Add", "Edit_Pupil_Contact_Add_Pupil_Contact_Address_Local")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
         Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase)]
        public void Edit_Pupil_Contact_Add_Pupil_Contact_Address_Local()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            Guid addressID;

            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(SAON, _seperator,
                PAONDescription, _seperator,
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(SAON, _seperator,
                PAONDescription, _lineSeperator,
                PAONRange, _space, Street, _lineSeperator,
                Locality, _lineSeperator,
                Town, _lineSeperator,
                AdministrativeArea, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            #endregion

            using (new DataSetup(CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname),
                CreatePupilContactRelationship(pupilId, pupilContactId),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil Contact record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                EditContactDialog pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();
                pupilContactPage.ClickAddAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(Town, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);

                addAddress.ClickOk();
                pupilContactPage.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();
                var gridRow2 = pupilContactPage.AddressTable.Rows[0];

                Assert.AreEqual(addressDisplayLarge, gridRow2.Address);
            }
        }

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Add", "Edit_Pupil_Contact_Add_Pupil_Contact_Address_WAV")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Pupil_Contact_Add_Pupil_Contact_Address_WAV()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string PAONRange = "22";
            string Street = "SUDELEY WALK";
            string Town = "BEDFORD";
            string PostCode = "MK41 8HS";
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(
                PAONRange, _space, Street, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            #endregion

            using (new DataSetup(CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname),
                CreatePupilContactRelationship(pupilId, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                EditContactDialog pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();
                pupilContactPage.ClickAddAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(Town, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);

                addAddress.ClickOk();
                pupilContactPage.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();
                var gridRow2 = pupilContactPage.AddressTable.Rows[0];

                Assert.AreEqual(addressDisplayLarge, gridRow2.Address);
            }
        }

        #endregion

        #region Edit Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Edit", "Edit_Pupil_Contact_Edit_Pupil_Contact_Address_Fields")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Pupil_Contact_Edit_Pupil_Contact_Address_Fields()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            Guid addressID, pupilContactAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            #endregion

            using (new DataSetup(
                CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname),
                CreatePupilContactRelationship(pupilId, pupilContactId),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil Contact record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                EditContactDialog pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();
                var gridRow2 = pupilContactPage.AddressTable.Rows[0];
                gridRow2.Edit();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.District = newLocality;

                addAddress.ClickOk();
                pupilContactPage.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();
                gridRow2 = pupilContactPage.AddressTable.Rows[0];
                gridRow2.Edit();

                addAddress = new AddAddressPopup();
                Assert.AreEqual(newLocality, addAddress.District);
            }
        }

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Edit", "Edit_Pupil_Contact_Edit_Pupil_Contact_Address_New_Address")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Pupil_Contact_Edit_Pupil_Contact_Address_New_Address()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            Guid addressID, pupilContactAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string WAVPAONRange = "22";
            string WAVStreet = "SUDELEY WALK";
            string WAVTown = "BEDFORD";
            string WAVPostCode = "MK41 8HS";

            #endregion

            using (new DataSetup(
                CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname),
                CreatePupilContactRelationship(pupilId, pupilContactId),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil Contact record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                EditContactDialog pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();
                var gridRow2 = pupilContactPage.AddressTable.Rows[0];
                gridRow2.Edit();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();

                addAddress.ClickOk();
                pupilContactPage.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();
                gridRow2 = pupilContactPage.AddressTable.Rows[0];
                gridRow2.Edit();

                addAddress = new AddAddressPopup();

                Assert.AreEqual(WAVPAONRange, addAddress.BuildingNo);
                Assert.AreEqual(WAVStreet, addAddress.Street);
                Assert.AreEqual(WAVTown, addAddress.Town);
                Assert.AreEqual(WAVPostCode, addAddress.PostCode);
            }
        }

        #endregion

        #region Delete Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Delete", "Edit_Pupil_Contact_Delete_Pupil_Contact_Address")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Pupil_Contact_Delete_Pupil_Contact_Address()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            Guid addressID, pupilContactAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            #endregion

            using (new DataSetup(
                CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname),
                CreatePupilContactRelationship(pupilId, pupilContactId),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil Contact record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                EditContactDialog pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();
                var gridRow2 = pupilContactPage.AddressTable.Rows[0];
                gridRow2.DeleteRow();

                pupilContactPage.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();

                int count = pupilContactPage.AddressTable.Rows.Count;

                Assert.AreEqual(0, count);
            }
        }

        #endregion

        #region Move Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Move", "Edit_Pupil_Contact_Move_Pupil_Contact_Address")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Pupil_Contact_Move_Pupil_Contact_Address()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            Guid addressID, pupilContactAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string WAVPAONRange = "22";
            string WAVPostCode = "MK41 8HS";

            DateTime moveDate = DateTime.Today.AddDays(5);

            #endregion

            using (new DataSetup(
                CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname),
                CreatePupilContactRelationship(pupilId, pupilContactId),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil Contact record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                EditContactDialog pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();
                var gridRow2 = pupilContactPage.AddressTable.Rows[0];
                gridRow2.Move();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.MoveDate = moveDate.ToShortDateString();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();

                addAddress.ClickOk();
                pupilContactPage.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                pupilContactPage = new EditContactDialog();
                pupilContactPage.ScrollToAddressPanel();

                gridRow2 = pupilContactPage.AddressTable.Rows[0];
                var gridRow3 = pupilContactPage.AddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow2.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), gridRow3.StartDate);
            }
        }

        #endregion

        #endregion

        #region Add Contact Palette

        #region Add Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Add", "Add_Pupil_Contact_Add_Pupil_Contact_Address_Local")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
         Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase)]
        public void Add_Pupil_Contact_Add_Pupil_Contact_Address_Local()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            Guid addressID;

            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(SAON, _seperator,
                PAONDescription, _seperator,
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(SAON, _seperator,
                PAONDescription, _lineSeperator,
                PAONRange, _space, Street, _lineSeperator,
                Locality, _lineSeperator,
                Town, _lineSeperator,
                AdministrativeArea, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            #endregion

            using (new DataSetup(CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname),
                CreatePupilContactRelationship(pupilId, pupilContactId),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil Contact record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                pupilRecordPage.ClickAddContact();

                AddPupilContactTripletDialog pupilContactTriplet = new AddPupilContactTripletDialog();
                pupilContactTriplet.SearchCriteria.NameSearchText = surname;
                SearchResultsComponent<AddPupilContactTripletDialog.PupilCotactSearchResultTile> contactSearchResultTiles = pupilContactTriplet.SearchCriteria.Search();
                AddPupilContactTripletDialog.PupilCotactSearchResultTile contactSearchResultTile = contactSearchResultTiles.Single();
                contactSearchResultTile.Click();

                AddPupilContactDialog pupilContactPage = new AddPupilContactDialog();
                pupilContactPage.HasSameAddressPupil = false;
                pupilContactPage.ClickAddAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(Town, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);

                addAddress.ClickOk();
                pupilContactTriplet.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                var editPupilContactPage = new EditContactDialog();
                editPupilContactPage.ScrollToAddressPanel();
                var gridRow2 = editPupilContactPage.AddressTable.Rows[0];

                Assert.AreEqual(addressDisplayLarge, gridRow2.Address);
            }
        }

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Add", "Add_Pupil_Contact_Add_Pupil_Contact_Address_WAV")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Pupil_Contact_Add_Pupil_Contact_Address_WAV()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string PAONRange = "22";
            string Street = "SUDELEY WALK";
            string Town = "BEDFORD";
            string PostCode = "MK41 8HS";
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(
                PAONRange, _space, Street, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            #endregion

            using (new DataSetup(CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                pupilRecordPage.ClickAddContact();

                AddPupilContactTripletDialog pupilContactTriplet = new AddPupilContactTripletDialog();
                pupilContactTriplet.SearchCriteria.NameSearchText = surname;
                SearchResultsComponent<AddPupilContactTripletDialog.PupilCotactSearchResultTile> contactSearchResultTiles = pupilContactTriplet.SearchCriteria.Search();
                AddPupilContactTripletDialog.PupilCotactSearchResultTile contactSearchResultTile = contactSearchResultTiles.Single();
                contactSearchResultTile.Click();

                AddPupilContactDialog pupilContactPage = new AddPupilContactDialog();
                pupilContactPage.HasSameAddressPupil = false;
                pupilContactPage.ClickAddAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(Town, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);

                addAddress.ClickOk();
                pupilContactTriplet.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow = pupilRecordPage.ContactTable.Rows[0];
                gridRow.ClickEdit();

                var editPupilContactPage = new EditContactDialog();
                editPupilContactPage.ScrollToAddressPanel();
                var gridRow2 = editPupilContactPage.AddressTable.Rows[0];

                Assert.AreEqual(addressDisplayLarge, gridRow2.Address);
            }
        }

        #endregion

        #region Edit

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Edit", "Add_Pupil_Contact_Edit_Pupil_Contact_Address_Fields")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Pupil_Contact_Edit_Pupil_Contact_Address_Fields()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            Guid addressID, pupilContactAddressID;

            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            #endregion

            using (new DataSetup(
                CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil Contact record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                pupilRecordPage.ClickAddContact();

                AddPupilContactTripletDialog pupilContactTriplet = new AddPupilContactTripletDialog();
                pupilContactTriplet.SearchCriteria.NameSearchText = surname;
                SearchResultsComponent<AddPupilContactTripletDialog.PupilCotactSearchResultTile> contactSearchResultTiles = pupilContactTriplet.SearchCriteria.Search();
                AddPupilContactTripletDialog.PupilCotactSearchResultTile contactSearchResultTile = contactSearchResultTiles.Single();
                contactSearchResultTile.Click();

                AddPupilContactDialog pupilContactPage = new AddPupilContactDialog();
                var gridRow = pupilContactPage.AddressTable.Rows[0];
                gridRow.Edit();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.District = newLocality;

                addAddress.ClickOk();
                pupilContactTriplet.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow2 = pupilRecordPage.ContactTable.Rows[0];
                gridRow2.ClickEdit();

                var editPupilContactPage = new EditContactDialog();
                editPupilContactPage.ScrollToAddressPanel();
                var gridRow3 = editPupilContactPage.AddressTable.Rows[0];
                gridRow3.Edit();

                addAddress = new AddAddressPopup();
                Assert.AreEqual(newLocality, addAddress.District);
            }
        }

        #endregion

        #region Delete Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Delete", "Add_Pupil_Contact_Delete_Pupil_Contact_Address")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Pupil_Contact_Delete_Pupil_Contact_Address()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            Guid addressID, pupilContactAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            #endregion

            using (new DataSetup(
                CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil Contact record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                pupilRecordPage.ClickAddContact();

                AddPupilContactTripletDialog pupilContactTriplet = new AddPupilContactTripletDialog();
                pupilContactTriplet.SearchCriteria.NameSearchText = surname;
                SearchResultsComponent<AddPupilContactTripletDialog.PupilCotactSearchResultTile> contactSearchResultTiles = pupilContactTriplet.SearchCriteria.Search();
                AddPupilContactTripletDialog.PupilCotactSearchResultTile contactSearchResultTile = contactSearchResultTiles.Single();
                contactSearchResultTile.Click();

                AddPupilContactDialog pupilContactPage = new AddPupilContactDialog();
                var gridRow = pupilContactPage.AddressTable.Rows[0];
                gridRow.DeleteRow();

                pupilContactTriplet.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow2 = pupilRecordPage.ContactTable.Rows[0];
                gridRow2.ClickEdit();

                var editPupilContactPage = new EditContactDialog();
                editPupilContactPage.ScrollToAddressPanel();

                int count = editPupilContactPage.AddressTable.Rows.Count;

                Assert.AreEqual(0, count);
            }
        }

        #endregion

        #region Move Address

        [TestMethod]
        [ChromeUiTest("PupilContactAddress", "P1", "Move", "Add_Pupil_Contact_Move_Pupil_Contact_Address")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Pupil_Contact_Move_Pupil_Contact_Address()
        {
            #region Arrange

            Guid pupilId, pupilContactId;
            Guid addressID, pupilContactAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string WAVPAONRange = "22";
            string WAVPostCode = "MK41 8HS";

            DateTime moveDate = DateTime.Today.AddDays(5);

            #endregion

            using (new DataSetup(
                CreatePupil(out pupilId),
                CreatePupilContact(out pupilContactId, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID, DateTime.Today, "H", addressID, pupilContactId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil Contact record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                pupilRecordPage.ClickAddContact();

                AddPupilContactTripletDialog pupilContactTriplet = new AddPupilContactTripletDialog();
                pupilContactTriplet.SearchCriteria.NameSearchText = surname;
                SearchResultsComponent<AddPupilContactTripletDialog.PupilCotactSearchResultTile> contactSearchResultTiles = pupilContactTriplet.SearchCriteria.Search();
                AddPupilContactTripletDialog.PupilCotactSearchResultTile contactSearchResultTile = contactSearchResultTiles.Single();
                contactSearchResultTile.Click();

                AddPupilContactDialog pupilContactPage = new AddPupilContactDialog();
                var gridRow = pupilContactPage.AddressTable.Rows[0];
                gridRow.Move();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.MoveDate = moveDate.ToShortDateString();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();

                addAddress.ClickOk();
                pupilContactTriplet.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectFamilyHomeTab();
                var gridRow2 = pupilRecordPage.ContactTable.Rows[0];
                gridRow2.ClickEdit();

                var editPupilContactPage = new EditContactDialog();
                editPupilContactPage.ScrollToAddressPanel();

                var gridRow3 = editPupilContactPage.AddressTable.Rows[0];
                var gridRow4 = editPupilContactPage.AddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow3.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), gridRow4.StartDate);
            }
        }

        #endregion

        #endregion

        #region Helpers

        private DataPackage CreateData(out Guid pupilID, out Guid staffID, out Guid pupilContactID, out Guid staffContactID)
        {
            #region IDs

            Guid learnerEnrolmentID;

            #endregion

            #region Values

            string staffForename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string staffSurname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string staffContactSurname = CoreQueries.GetColumnUniqueString("StaffContact", "Surname", 10, tenantID);

            string pupilForename = CoreQueries.GetColumnUniqueString("Learner", "LegalForename", 10, tenantID);
            string pupilSurname = CoreQueries.GetColumnUniqueString("Learner", "LegalSurname", 10, tenantID);
            string pupilContactSurname = CoreQueries.GetColumnUniqueString("LearnerContact", "Surname", 10, tenantID);

            string code = CoreQueries.GetColumnUniqueString("LocationType", "Code", 10, tenantID);
            string description = CoreQueries.GetColumnUniqueString("LocationType", "Description", 10, tenantID);

            #endregion

            DataPackage package = new DataPackage();

            #region Staff & StaffContact

            package.AddData("Staff", new
            {
                ID = staffID = Guid.NewGuid(),
                LegalForename = staffForename,
                LegalSurname = staffSurname,
                LegalMiddleNames = "Middle Names",
                PreferredForename = staffForename,
                PreferredSurname = staffSurname,
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            });
            package.AddData("StaffContact", new
            {
                ID = staffContactID = Guid.NewGuid(),
                Surname = staffContactSurname,
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            });
            package.AddData("StaffContactRelationship", new
            {
                ID = Guid.NewGuid(),
                Staff = staffID,
                StaffContact = staffContactID,
                TenantID = tenantID
            });

            #endregion

            #region Learner & LearnerContact 

            var yearGroup = Queries.GetFirstYearGroup();

            package.AddData("Learner", new
            {
                ID = pupilID = Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                LegalForename = pupilForename,
                LegalSurname = pupilSurname,
                DateOfBirth = startDate,
                TenantID = tenantID
            });
            package.AddData("LearnerEnrolment", new
            {
                ID = learnerEnrolmentID = Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Learner = pupilID,
                DOA = startDate,
                TenantID = tenantID

            });
            package.AddData("LearnerEnrolmentStatus", new
            {
                Id = Guid.NewGuid(),
                LearnerEnrolment = learnerEnrolmentID,
                StartDate = startDate,
                EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: "C"),
                TenantID = tenantID
            });
            package.AddData("LearnerYearGroupMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = pupilID,
                YearGroup = yearGroup.ID,
                StartDate = startDate,
                TenantID = tenantID
            });
            package.AddData("LearnerNCYearMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = pupilID,
                SchoolNCYear = yearGroup.SchoolNCYear,
                StartDate = startDate,
                TenantID = tenantID
            });
            package.AddData("LearnerContact", new
            {
                ID = pupilContactID = Guid.NewGuid(),
                Surname = pupilContactSurname,
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            });
            package.AddData("LearnerContactRelationship", new
            {
                ID = Guid.NewGuid(),
                Learner = pupilID,
                LearnerContact = pupilContactID,
                TenantID = tenantID
            });

            #endregion

            return package;
        }

        private DataPackage CreatePupil(out Guid pupilId)
        {
            #region IDs

            Guid learnerEnrolmentID;

            #endregion

            #region Values

            string pupilForename = CoreQueries.GetColumnUniqueString("Learner", "LegalForename", 10, tenantID);
            string pupilSurname = CoreQueries.GetColumnUniqueString("Learner", "LegalSurname", 10, tenantID);
            var yearGroup = Queries.GetFirstYearGroup();

            #endregion

            DataPackage package = new DataPackage();

            package.AddData("Learner", new
            {
                ID = pupilId = Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                LegalForename = pupilForename,
                LegalSurname = pupilSurname,
                DateOfBirth = startDate,
                TenantID = tenantID
            });
            package.AddData("LearnerEnrolment", new
            {
                ID = learnerEnrolmentID = Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Learner = pupilId,
                DOA = startDate,
                TenantID = tenantID

            });
            package.AddData("LearnerEnrolmentStatus", new
            {
                Id = Guid.NewGuid(),
                LearnerEnrolment = learnerEnrolmentID,
                StartDate = startDate,
                EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: "C"),
                TenantID = tenantID
            });
            package.AddData("LearnerYearGroupMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = pupilId,
                YearGroup = yearGroup.ID,
                StartDate = startDate,
                TenantID = tenantID
            });
            package.AddData("LearnerNCYearMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = pupilId,
                SchoolNCYear = yearGroup.SchoolNCYear,
                StartDate = startDate,
                TenantID = tenantID
            });

            return package;
        }

        private DataPackage CreatePupilContact(out Guid pupilContactId, string surname)
        {
            return new DataPackage()
                .AddData("LearnerContact", new
                {
                    ID = pupilContactId = Guid.NewGuid(),
                    Surname = surname,
                    School = CoreQueries.GetSchoolId(),
                    TenantID = tenantID
                });
        }

        private DataPackage GetPupilRecord_current(out Guid pupilID)
        {
            Guid learnerEnrolmentID;
            int tenantID = SeSugar.Environment.Settings.TenantId;
            DateTime startDate = DateTime.Today.AddDays(-1);
            var yearGroup = Queries.GetFirstYearGroup();

            string pupilForename = CoreQueries.GetColumnUniqueString("Learner", "LegalForename", 10, tenantID);
            string pupilSurname = CoreQueries.GetColumnUniqueString("Learner", "LegalSurname", 10, tenantID);

            return this.BuildDataPackage().AddData("Learner", new
            {
                ID = pupilID = Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                LegalForename = pupilForename,
                LegalSurname = pupilSurname,
                DateOfBirth = startDate,
                TenantID = tenantID
            })
            .AddData("LearnerEnrolment", new
            {
                ID = learnerEnrolmentID = Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Learner = pupilID,
                DOA = startDate,
                TenantID = tenantID

            })
            .AddData("LearnerEnrolmentStatus", new
            {
                Id = Guid.NewGuid(),
                LearnerEnrolment = learnerEnrolmentID,
                StartDate = startDate,
                EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: "C"),
                TenantID = tenantID
            })
            .AddData("LearnerYearGroupMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = pupilID,
                YearGroup = yearGroup.ID,
                StartDate = startDate,
                TenantID = tenantID
            })
            .AddData("LearnerNCYearMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = pupilID,
                SchoolNCYear = yearGroup.SchoolNCYear,
                StartDate = startDate,
                TenantID = tenantID
            });
        }

        private DataPackage CreatePupilContactRelationship(Guid pupilId, Guid pupilContactId)
        {
            return new DataPackage()
               .AddData("LearnerContactRelationship", new
               {
                   ID = Guid.NewGuid(),
                   Learner = pupilId,
                   LearnerContact = pupilContactId,
                   TenantID = tenantID
               });
        }

        private void CreateTelephoneData(DataPackage package, string oldTelephoneNumber, Guid pupilID, Guid staffID, Guid pupilContactID, Guid staffContactID)
        {
            Guid locationTypeID;
            package.AddData("LocationType", new
            {
                ID = locationTypeID = Guid.NewGuid(),
                TenantID = tenantID,
                Code = "",
                Description = "",
                IsVisible = 1,
                DisplayOrder = 1,
                ResourceProvider = CoreQueries.GetSchoolId()
            });
            package.AddData("LearnerTelephone", new
            {
                ID = Guid.NewGuid(),
                TelephoneNumber = oldTelephoneNumber,
                IsFirstPointOfContact = 1,
                LocationType = locationTypeID,
                Learner = pupilID,
                TenantID = tenantID
            });
            package.AddData("LearnerContactTelephone", new
            {
                ID = Guid.NewGuid(),
                TelephoneNumber = oldTelephoneNumber,
                IsFirstPointOfContact = 1,
                LocationType = locationTypeID,
                LearnerContact = pupilContactID,
                TenantID = tenantID
            });
            package.AddData("StaffTelephone", new
            {
                ID = Guid.NewGuid(),
                TelephoneNumber = oldTelephoneNumber,
                IsMainTelephone = 1,
                LocationType = locationTypeID,
                Staff = staffID,
                TenantID = tenantID
            });
            package.AddData("StaffContactTelephone", new
            {
                ID = Guid.NewGuid(),
                TelephoneNumber = oldTelephoneNumber,
                IsFirstPointOfContact = 1,
                LocationType = locationTypeID,
                StaffContact = staffContactID,
                TenantID = tenantID
            });
        }

        private void CreateEmailData(DataPackage package, string oldEmailAddress, Guid pupilID, Guid staffID, Guid pupilContactID, Guid staffContactID)
        {
            Guid emailLocationTypeID;
            package.AddData("EmailLocationType", new
            {
                ID = emailLocationTypeID = Guid.NewGuid(),
                TenantID = tenantID,
                Code = "",
                Description = "",
                IsVisible = 1,
                DisplayOrder = 1,
                ResourceProvider = CoreQueries.GetSchoolId()
            });
            package.AddData("LearnerEmail", new
            {
                ID = Guid.NewGuid(),
                EmailAddress = oldEmailAddress,
                IsMainEmail = 1,
                LocationType = emailLocationTypeID,
                Learner = pupilID,
                TenantID = tenantID
            });
            package.AddData("LearnerContactEmail", new
            {
                ID = Guid.NewGuid(),
                EmailAddress = oldEmailAddress,
                IsMainEmail = 1,
                LocationType = emailLocationTypeID,
                LearnerContact = pupilContactID,
                TenantID = tenantID
            });
            package.AddData("StaffEmail", new
            {
                ID = Guid.NewGuid(),
                EmailAddress = oldEmailAddress,
                IsMainEmail = 1,
                LocationType = emailLocationTypeID,
                Staff = staffID,
                TenantID = tenantID
            });
            package.AddData("StaffContactEmail", new
            {
                ID = Guid.NewGuid(),
                EmailAddress = oldEmailAddress,
                IsMainEmail = 1,
                LocationType = emailLocationTypeID,
                StaffContact = staffContactID,
                TenantID = tenantID
            });
        }

        public string GenerateNumber()
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 11; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }

        private static void LoginAndNavigate(SeleniumHelper.iSIMSUserType userType, string menuRoute = "Pupil Contacts", string enabledFeatures = null)
        {
            if (string.IsNullOrEmpty(enabledFeatures))
            {
                SeleniumHelper.Login(userType);
            }
            else
            {
                SeleniumHelper.Login(userType, enabledFeatures: enabledFeatures);
            }
            AutomationSugar.NavigateMenu("Tasks", "Pupils", menuRoute);
        }

        private static PupilRecordPage LoadPupil(Guid pupilId)
        {
            return PupilRecordPage.LoadPupilDetail(pupilId);
        }

        private static PupilRecordPage LoadPupil_Record(Guid pupilId)
        {
            return PupilRecordPage.LoadPupilDetail(pupilId);
        }

        private static PupilContactPage LoadPupilContact(Guid pupilContactId)
        {
            return PupilContactPage.LoadPupilContactDetail(pupilContactId);
        }

        private static StaffRecordPage LoadStaff(Guid staffId)
        {
            return StaffRecordPage.LoadStaffDetail(staffId);
        }

        private DataPackage GetAddress(out Guid addressID,
                            string UPRN,
                            string PAONDescription,
                            string PAONRange,
                            string SAON,
                            string Street,
                            string Locality,
                            string Town,
                            string AdministrativeArea,
                            string PostCode,
                            string Country)
        {
            return this.BuildDataPackage()
                .AddData("Address", new
                {
                    ID = addressID = Guid.NewGuid(),
                    UPRN = UPRN,
                    PAONDescription = PAONDescription,
                    PAONRange = PAONRange,
                    SAON = SAON,
                    Street = Street,
                    Locality = Locality,
                    Town = Town,
                    AdministrativeArea = AdministrativeArea,
                    PostCode = PostCode,
                    Country = CoreQueries.GetLookupItem("Country", description: Country),
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = SeSugar.Environment.Settings.TenantId
                });
        }

        private DataPackage GetPupilAddress(out Guid pupilAddressID, DateTime startDate, string addressTypeCode, Guid address, Guid pupilID, DateTime? endDate = null)
        {
            return this.BuildDataPackage()
            .AddData("LearnerAddress", new
            {
                ID = pupilAddressID = Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                AddressType = CoreQueries.GetLookupItem("AddressType", code: addressTypeCode),
                Address = address,
                Learner = pupilID,
                TenantID = SeSugar.Environment.Settings.TenantId
            });
        }

        private DataPackage GetPupilContactAddress(out Guid pupilAddressID,
                            DateTime startDate,
                            string addressTypeCode,
                            Guid address,
                            Guid pupilContactID,
                            DateTime? endDate = null)
        {
            return this.BuildDataPackage()
            .AddData("LearnerContactAddress", new
            {
                ID = pupilAddressID = Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                AddressType = CoreQueries.GetLookupItem("AddressType", code: addressTypeCode),
                Address = address,
                LearnerContact = pupilContactID,
                TenantID = SeSugar.Environment.Settings.TenantId
            });
        }

        #endregion

        #region DATA

        public List<object[]> ADD_NEW_PUPIL_CONTACT_DATA()
        {
            var randomName = Thread.CurrentThread.ManagedThreadId + "ADD_NEW_PUPIL_CONTACT" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

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
                    "Prof Aaron",
                    // Addressee
                    "Prof J Aaron",
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
                    "BT57 8RR",
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

        public List<object[]> TC_PU031a_Data()
        {
            var randomContact1 = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var randomContact2 = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Forename contact
                    "Stephen",
                    // Surname contact
                    "Baker",
                    // Forename contact 2
                    "Gareth",
                    // Surname contact 2
                    "Baker",
                    // Pupil name
                    "Baker, Fiona",
                    // Title
                    "Rev",
                    // Gender
                    "Male",
                    // Priority
                    "2",
                    // Relationship
                    "Parent",
                    // Salutation,
                    "Prof Aaron",
                    // Addressee
                    "Prof J Aaron"
                }
            };
            return res;
        }

        public List<object[]> TC_PU031b_Data()
        {
            var randomPupil = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var randomContact = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Forename pupil
                    randomPupil,
                    // Surname pupil
                    randomPupil,
                    // Full name contact 1
                    "Mr " + randomContact + " " + randomContact,
                    // Title Contact 1
                    "Mr",
                    // Forename contact 1
                    randomContact,
                    // Surname contact 1
                    randomContact,
                    // Gender contact 1
                    "Male"
                }
            };
            return res;
        }

        #endregion
    }
}