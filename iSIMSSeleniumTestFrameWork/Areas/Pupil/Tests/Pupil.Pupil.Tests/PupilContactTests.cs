using NUnit.Framework;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Pupil.Components.Common;
using TestSettings;
using WebDriverRunner.internals;

namespace Pupil.Pupil.Tests
{
    public class PupilContactTests
    {

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-029 : 
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Contact, PupilTestGroups.Priority.Priority1 }, DataProvider = "TC_PU029_Data")]
        public void TC_PU029_Exercise_ability_to_add_a_new_Pupil_Contact(
            string forenameContact, string surnameContact, string title, string gender, string salutation,
            string addressee, string buildingNo, string street, string district, string city,
            string county, string postCode, string countryPostCode, string language, string placeOfWork,
            string jobTitle, string occupation, string forenamePupil, string surenamePupil, string priority, string relationship)
        {

            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Steps
            Console.WriteLine("***Steps");

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

            // Create new pupil contract
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

            // Select Addresses Tab
            pupilContactPage.SelectAddressesTab();
            var addAddressDialog = pupilContactPage.ClickAddanAdditionalAddressLink();

            // Add new address
            addAddressDialog.BuildingNo = buildingNo;
            addAddressDialog.Street = street;
            addAddressDialog.District = district;
            addAddressDialog.City = city;
            addAddressDialog.County = county;
            addAddressDialog.PostCode = postCode;
            addAddressDialog.CountryPostCode = countryPostCode;
            addAddressDialog.ClickOk();

            // Select Ethnic/Cultural
            pupilContactPage.SelectEthnicCulturalTab();
            pupilContactPage.Language = language;

            // Select Job Details tab
            pupilContactPage.SelectJobDetailsTab();
            pupilContactPage.PlaceOfWork = placeOfWork;
            pupilContactPage.JobTitle = jobTitle;
            pupilContactPage.Occupation = occupation;

            // Save
            pupilContactTriplet.ClickSave();

            // Select Associated Pupils tab
            pupilContactPage.SelectAssociatedPupilsTab();
            var addAssociatedPupilsTripletDialog = pupilContactPage.ClickAddPupilLink();
            addAssociatedPupilsTripletDialog.SearchCriteria.LegalSurname = surenamePupil;
            addAssociatedPupilsTripletDialog.SearchCriteria.LegalForename = forenamePupil;
            var associatedPupilResults = addAssociatedPupilsTripletDialog.SearchCriteria.Search();
            associatedPupilResults.SingleOrDefault(x => x.Name.Trim().Equals(surenamePupil + ", " + forenamePupil)).Click(); ;
            addAssociatedPupilsTripletDialog.ClickOk(7);

            // Record the data for the associated pupil
            var pupilTable = pupilContactPage.PupilTable;
            var pupilRow = pupilTable.Rows.SingleOrDefault(x => x.LegalForename.Trim().Equals(forenamePupil)
                && x.LegalSurname.Trim().Equals(surenamePupil));
            pupilRow.Priority = priority;
            pupilRow.Relationship = relationship;
            pupilRow.ParentalResponsibility = true;
            pupilRow.ReceivesCorrespondance = true;
            pupilRow.SchoolReport = true;
            pupilRow.CourtOrder = false;

            // Save
            pupilContactTriplet.ClickSave();

            // Verify that New 'Pupil Contact' record has been added.
            Assert.AreEqual(true, pupilContactPage.IsSuccessMessageDisplayed()
                , "New 'Pupil Contact' record has been added unsuccessfully.");

            // Navigate to Pupil Records
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            // Search a pupil
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = surenamePupil + ", " + forenamePupil;
            var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            var pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surenamePupil + ", " + forenamePupil)).Click<PupilRecordPage>();

            // Go to Family/Home section
            pupilRecordPage.SelectFamilyHomeTab();
            var contacts = pupilRecordPage.ContactTable;

            // Verify that Pupil Contact's record now displays 
            // in the 'Contacts' grid in the 'Family/Contact' section.

            Assert.AreEqual(true, contacts.Rows
                .Any(x => x.Name.Trim().Contains(forenameContact + " " + surnameContact))
                , "Pupil Contact's record isn't displayed in the 'Contacts' grid");

            #endregion

            #region Post-Condition: Delete the contact
            Console.WriteLine("***Post-Condition: Delete the contact");

            pupilContactPage.ClickDelete();
            pupilContactPage.ContinueDelete();

            #endregion
        }

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-030 : 
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Contact, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU030_Data")]
        public void TC_PU030_Exercise_ability_to_search_for_an_existing_Pupil_Contact(
            string forenameContact, string surnameContact, string title, string gender, string salutation,
            string addressee)
        {
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition: Create new contact
            Console.WriteLine("***Pre-Condition: Create new contact");

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

            // Create new pupil contract
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

            #endregion

            #region Steps:
            Console.WriteLine("***Steps:");

            // Search the contact with Forname
            pupilContactTriplet = new PupilContactTriplet();
            pupilContactTriplet.SearchCriteria.Forename = forenameContact;
            var pupilContactResults = pupilContactTriplet.SearchCriteria.Search();

            // Verify that contact results are displayed
            Assert.AreNotEqual(null, pupilContactResults.FirstOrDefault(x => x.Name.Trim().Equals(surnameContact + ", " + forenameContact)), "The contact isn't displayed.");

            // Search the contact with Surname
            pupilContactTriplet.SearchCriteria.Surname = surnameContact;
            pupilContactResults = pupilContactTriplet.SearchCriteria.Search();

            // Verify that contact results are displayed
            Assert.AreNotEqual(null, pupilContactResults.FirstOrDefault(x => x.Name.Trim().Equals(surnameContact + ", " + forenameContact)), "The contact isn't displayed.");

            // Search the contact with Surname
            pupilContactTriplet.SearchCriteria.Forename = forenameContact;
            pupilContactTriplet.SearchCriteria.Surname = surnameContact;
            pupilContactResults = pupilContactTriplet.SearchCriteria.Search();

            // Verify that contact results are displayed
            Assert.AreNotEqual(null, pupilContactResults.FirstOrDefault(x => x.Name.Trim().Equals(surnameContact + ", " + forenameContact)), "The contact isn't displayed.");

            #endregion

            #region Post-Condition: Delete the contact
            Console.WriteLine("***Pre-Condition: Delete the contact");

            pupilContactPage.ClickDelete();
            pupilContactPage.ContinueDelete();

            #endregion
        }
        
        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-032 : 
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Contact, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU032_Data")]
        public void TC_PU032_Exercise_ability_to_delete_a_Pupil_Contact(
            string forenamePP, string surnamePP, string titleContact1, string forenameContact1, string surnameContact1,
            string titleContact2, string forenameContact2, string surnameContact2, string forenamePP2, string surnamePP2)
        {
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition: Create new contact 1 and refer to pupil
            Console.WriteLine("***Pre-Condition: Create new contact and refer to pupil");

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

            // Create new pupil contract
            var pupilContactTriplet = new PupilContactTriplet();
            var pupilContactPage = pupilContactTriplet.ClickCreate();

            // Add data for Personal Details
            pupilContactPage.SelectPersonalDetailsTab();
            pupilContactPage.Title = titleContact1;
            pupilContactPage.Forename = forenameContact1;
            pupilContactPage.Surname = surnameContact1;
            pupilContactPage.Gender = "Male";

            // Save
            pupilContactTriplet.ClickSave();

            // Select Associated Pupils tab
            pupilContactPage.SelectAssociatedPupilsTab();
            var addAssociatedPupilsTripletDialog = pupilContactPage.ClickAddPupilLink();
            addAssociatedPupilsTripletDialog.SearchCriteria.LegalSurname = surnamePP;
            addAssociatedPupilsTripletDialog.SearchCriteria.LegalForename = forenamePP;
            var associatedPupilResults = addAssociatedPupilsTripletDialog.SearchCriteria.Search();
            associatedPupilResults.SingleOrDefault(x => x.Name.Trim().Equals(surnamePP + ", " + forenamePP)).Click(); ;
            addAssociatedPupilsTripletDialog.ClickOk(7);

            // Record the data for the associated pupil
            var pupilTable = pupilContactPage.PupilTable;
            var pupilRow = pupilTable.Rows.SingleOrDefault(x => x.LegalForename.Trim().Equals(forenamePP)
                && x.LegalSurname.Trim().Equals(surnamePP));
            pupilRow.Priority = "1";
            pupilRow.Relationship = "Parent";
            pupilRow.ParentalResponsibility = true;
            pupilRow.ReceivesCorrespondance = true;
            pupilRow.SchoolReport = true;
            pupilRow.CourtOrder = false;

            // Save
            pupilContactTriplet.ClickSave();

            #endregion

            #region Pre-Condition: Create new contact 2 and refer to pupil
            Console.WriteLine("***Pre-Condition: Create new contact and refer to pupil");

            // Create new pupil contract
            pupilContactPage = pupilContactTriplet.ClickCreate();

            // Add data for Personal Details
            pupilContactPage.SelectPersonalDetailsTab();
            pupilContactPage.Title = titleContact2;
            pupilContactPage.Forename = forenameContact2;
            pupilContactPage.Surname = surnameContact2;
            pupilContactPage.Gender = "Male";

            // Save
            pupilContactTriplet.ClickSave();

            // Select Associated Pupils tab
            pupilContactPage.SelectAssociatedPupilsTab();
            addAssociatedPupilsTripletDialog = pupilContactPage.ClickAddPupilLink();
            addAssociatedPupilsTripletDialog.SearchCriteria.LegalSurname = surnamePP2;
            addAssociatedPupilsTripletDialog.SearchCriteria.LegalForename = forenamePP2;
            associatedPupilResults = addAssociatedPupilsTripletDialog.SearchCriteria.Search();
            associatedPupilResults.SingleOrDefault(x => x.Name.Trim().Equals(surnamePP2 + ", " + forenamePP2)).Click(); ;
            addAssociatedPupilsTripletDialog.ClickOk(7);

            // Record the data for the associated pupil
            pupilTable = pupilContactPage.PupilTable;
            pupilRow = pupilTable.Rows.SingleOrDefault(x => x.LegalForename.Trim().Equals(forenamePP2)
                && x.LegalSurname.Trim().Equals(surnamePP2));
            pupilRow.Priority = "1";
            pupilRow.Relationship = "Parent";
            pupilRow.ParentalResponsibility = true;
            pupilRow.ReceivesCorrespondance = true;
            pupilRow.SchoolReport = true;
            pupilRow.CourtOrder = false;

            // Save
            pupilContactTriplet.ClickSave();

            #endregion

            #region STEPS

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            // Search a pupil
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = forenamePP + ", " + surnamePP;
            var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            var pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surnamePP + ", " + forenamePP)).Click<PupilRecordPage>();

            // Go to Family/Home section
            pupilRecordPage.SelectFamilyHomeTab();

            // Delete a contact
            var contacts = pupilRecordPage.ContactTable;
            var contactRows = contacts.Rows;
            var contactDeleted = contactRows.SingleOrDefault(x => x.Name.Trim().Equals(titleContact1 + " " + forenameContact1 + " " + surnameContact1));
            contactDeleted.DeleteRow();

            // Save
            pupilRecordPage.SavePupil();

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

            // Search the contact
            pupilContactTriplet = new PupilContactTriplet();
            pupilContactTriplet.SearchCriteria.Forename = forenameContact1;
            pupilContactTriplet.SearchCriteria.Surname = surnameContact1;
            var pupilContactResults = pupilContactTriplet.SearchCriteria.Search();
            var pupilContact = pupilContactResults.SingleOrDefault(x => x.Name.Trim().Equals(surnameContact1 + ", " + forenameContact1));
            pupilContactPage = pupilContact.Click<PupilContactPage>();

            // Select Associated Pupils tab
            pupilContactPage.SelectAssociatedPupilsTab();
            var pupils = pupilContactPage.PupilTable;

            // Verify that there is no longer a link to the pupil record 
            // where the 'Pupil Contact' was deleted.
            Assert.AreEqual(false, pupils.Rows.Any(x => x.LegalForename.Trim().Equals(surnamePP)
                && x.LegalSurname.Trim().Equals(forenamePP)), "Existing link to the pupil record.");

            // Search other contact
            pupilContactTriplet.SearchCriteria.Forename = forenameContact2;
            pupilContactTriplet.SearchCriteria.Surname = surnameContact2;
            pupilContactResults = pupilContactTriplet.SearchCriteria.Search();
            pupilContact = pupilContactResults.SingleOrDefault(x => x.Name.Trim().Equals(surnameContact2 + ", " + forenameContact2));
            pupilContactPage = pupilContact.Click<PupilContactPage>();

            // Delete contact
            pupilContactPage.ClickDelete();
            var warningConfirmDialog = new WarningConfirmationDialog();
            warningConfirmDialog.ConfirmDelete();

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            // Search a pupil
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = forenamePP + ", " + surnamePP;
            pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surnamePP + ", " + forenamePP)).Click<PupilRecordPage>();

            // Go to Family/Home section
            pupilRecordPage.SelectFamilyHomeTab();

            // Verify that A Pupil Contact record is deleted in its entirety 
            // and is no longer a contact for any pupil previously identified with this contact.
            contacts = pupilRecordPage.ContactTable;
            Assert.AreEqual(false, contacts.Rows.Any(x => x.Name.Trim().Equals(titleContact2 + " " + forenameContact2 + " " + surnameContact2)), "Fail: Exist the contact");

            #endregion

            #region Post-Condition: Delete the contact 1 if existed
            Console.WriteLine("***Post-Condition: Delete the contact if existed");

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

            // Search the contact
            pupilContactTriplet = new PupilContactTriplet();
            pupilContactTriplet.SearchCriteria.Forename = forenameContact1;
            pupilContactTriplet.SearchCriteria.Surname = surnameContact1;
            pupilContactResults = pupilContactTriplet.SearchCriteria.Search();

            pupilContact = pupilContactResults.SingleOrDefault(x => x.Name.Trim().Equals(surnameContact1 + ", " + forenameContact1));
            pupilContactPage = pupilContact == null ? new PupilContactPage() : pupilContact.Click<PupilContactPage>();
            pupilContactPage.ClickDelete();
            pupilContactPage.ContinueDelete();

            #endregion

            #region Post-Condition: Delete the contact 2 if existed
            Console.WriteLine("***Post-Condition: Delete the contact if existed");

            // Search the contact
            pupilContactTriplet = new PupilContactTriplet();
            pupilContactTriplet.SearchCriteria.Forename = forenameContact2;
            pupilContactTriplet.SearchCriteria.Surname = surnameContact2;
            pupilContactResults = pupilContactTriplet.SearchCriteria.Search();

            pupilContact = pupilContactResults.SingleOrDefault(x => x.Name.Trim().Equals(surnameContact2 + ", " + forenameContact2));
            pupilContactPage = pupilContact == null ? new PupilContactPage() : pupilContact.Click<PupilContactPage>();
            pupilContactPage.ClickDelete();
            pupilContactPage.ContinueDelete();

            #endregion
        }

       
        #region DATA
        
        public List<object[]> TC_PU029_Data()
        {
            var randomName = Thread.CurrentThread.ManagedThreadId + "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
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
                    // Forename pupil
                    "Fiona",
                    // Surname pupil
                    "Baker",
                    // Priority
                    "1",
                    // Relationship
                    "Parent"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU030_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
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
                    "Prof J Aaron"
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

        public List<object[]> TC_PU032_Data()
        {
            var randomContact1 = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var randomContact2 = "Luong" + SeleniumHelper.GenerateRandomString(6) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Forename pupil
                    "Laura", 
                    // Surname pupil
                    "Adams",
                    // Title 1,
                    "Rev",
                    // Forename 1
                    randomContact1,
                    // Surname 1
                    randomContact1,
                    // Title 2,
                    "Mr",
                    // Forename 2
                    randomContact2,
                    // Surname 2
                    randomContact2,
                    // Forename pupil 2
                    "Carlton", 
                    // Surname pupil 2
                    "Allcroft",

                }
                
            };
            return res;
        }

        #endregion


    }
}
