using NUnit.Framework;
using POM.Components.Attendance;
using POM.Components.Common;
using POM.Components.Conduct;
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
    public class PupilRecordTests
    {
      
        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add PersonalDetail information sucessfully
        /// </summary>
        /// 
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority1 }, DataProvider = "TC_PU02_Data")]
        public void TC_PU002_Verify_Add_PersonalDetail(string[] pupilInfo, bool BirthCertificateSeen, string QuickNote)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Add value for Quick Note
            pupilRecord.QuickNote = QuickNote;
            // Add value for BirthCertificateSeen
            pupilRecord.BirthCertificateSeen = BirthCertificateSeen;

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreEqual(BirthCertificateSeen, pupilRecord.BirthCertificateSeen, "BirthCertificateSeen is not equal");
            Assert.AreEqual(QuickNote, pupilRecord.QuickNote, "QuickNote is not equal");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add Registration information sucessfully
        /// </summary>
        /// 
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority1 }, DataProvider = "TC_PU03_Data")]
        public void TC_PU003_Verify_Add_Registration(string[] pupilInfo)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            pupilRecord.SelectRegistrationTab();


            pupilRecord.ClickGenerateUPN();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreNotEqual("", pupilRecord.UniquePupilNumber, "Unique pupil number is created");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }
        
        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add Verify Address information sucessfully
        /// </summary>
        /// 
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU04_Data")]
        public void TC_PU004_Verify_Add_Address_Section(string[] pupilInfo, string[] newAddress)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            pupilRecord.SelectAddressesTab();
            var addAddressDialog = pupilRecord.ClickAddAddress();

            addAddressDialog.BuildingNo = newAddress[0];
            addAddressDialog.BuildingName = newAddress[1];
            addAddressDialog.Flat = newAddress[2];
            addAddressDialog.Street = newAddress[3];
            addAddressDialog.District = newAddress[4];
            addAddressDialog.City = newAddress[5];
            addAddressDialog.County = newAddress[6];
            addAddressDialog.PostCode = newAddress[7];
            addAddressDialog.CountryPostCode = newAddress[8];
            addAddressDialog.ClickOk();


            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            //Addresses: Verify address of staff                             
            var rowAddress = pupilRecord.AddressTable.Rows
                                    .SingleOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        newAddress[2], newAddress[0], newAddress[1], newAddress[3],
                                                        newAddress[4], newAddress[5], newAddress[6], newAddress[7], newAddress[8])));

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            pupilRecord.Delete();
            #endregion
        }

        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add Registration information sucessfully
        /// </summary>
        /// 
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU05_Data")]
        public void TC_PU005_Verify_Add_Registration_Section(string[] pupilInfo, string[] telephoneNumber, string[] emailAddress)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var DeletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            DeletePupilRecord.Delete();


            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            pupilRecord.SelectPhoneEmailTab();

            // Add Pupil Telephone number
            pupilRecord = new PupilRecordPage();
            pupilRecord.ClickAddTelephoneNumber();
            SeleniumHelper.Sleep(1);
            pupilRecord.TelephoneNumberTable[0].TelephoneNumber = telephoneNumber[0];

            // Add Email Address
            pupilRecord.ClickAddEmailAddress();
            SeleniumHelper.Sleep(1);
            pupilRecord.EmailTable[0].EmailAddress = emailAddress[0];

            // Save Current pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Change location to Other
            SeleniumHelper.Sleep(1);
            pupilRecord.TelephoneNumberTable[0].Location = telephoneNumber[1];
            SeleniumHelper.Sleep(1);
            pupilRecord.EmailTable[0].Location = emailAddress[1];

            //Provide text in the 'Notes' field 
            SeleniumHelper.Sleep(1);
            pupilRecord.TelephoneNumberTable[0].Note = telephoneNumber[2];
            SeleniumHelper.Sleep(1);
            pupilRecord.EmailTable[0].Note = emailAddress[2];

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreEqual(telephoneNumber[0], pupilRecord.TelephoneNumberTable[0].TelephoneNumber, "The telephone number displays correctly");
            Assert.AreEqual(emailAddress[0], pupilRecord.EmailTable[0].EmailAddress, "The Email Address displays correctly");

            Assert.AreEqual(telephoneNumber[1], pupilRecord.TelephoneNumberTable[0].Location, "The Location of telephone number displays correctly");
            Assert.AreEqual(emailAddress[1], pupilRecord.EmailTable[0].Location, "The Location of Email Address displays correctly");

            Assert.AreEqual(telephoneNumber[2], pupilRecord.TelephoneNumberTable[0].Note, "The Note of telephone number displays correctly");
            Assert.AreEqual(emailAddress[2], pupilRecord.EmailTable[0].Note, "The Note of Email Address displays correctly");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            DeletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            DeletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add Contact into pupil record sucessfully
        /// </summary>
        /// 
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU06_Data")]
        public void TC_PU006_Verify_Add_Contact_Section(string[] pupilInfo, string[] newAddress)
        {
            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");
            
            // Add Address
            pupilRecord.SelectAddressesTab();
            var addAddressDialog = pupilRecord.ClickAddAddress();

            addAddressDialog.BuildingNo = newAddress[0];
            addAddressDialog.BuildingName = newAddress[1];
            addAddressDialog.Flat = newAddress[2];
            addAddressDialog.Street = newAddress[3];
            addAddressDialog.District = newAddress[4];
            addAddressDialog.City = newAddress[5];
            addAddressDialog.County = newAddress[6];
            addAddressDialog.PostCode = newAddress[7];
            addAddressDialog.CountryPostCode = newAddress[8];
            addAddressDialog.ClickOk();
            
            pupilRecord.SelectFamilyHomeTab();

            var addPupilContactTripletDialog = pupilRecord.ClickAddContact();
            var contactSearchResults = addPupilContactTripletDialog.SearchCriteria.Search();

            // 'Add' a contact, by selecting the 2nd contact
            var pupilContactSearchTile = contactSearchResults[1];
            var pupilContactDialog = pupilContactSearchTile == null ? null : pupilContactSearchTile.Click<AddPupilContactDialog>();
            addPupilContactTripletDialog.ClickOk();

            pupilRecord.ContactTable.WaitForNewRowAppear();
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            Assert.AreNotEqual(0, pupilRecord.ContactTable.Rows.Count, "The row contact does not display after adding it");

            //update value into table Contact                        
            pupilRecord.ContactTable[0].Priority = "1";
            pupilRecord.ContactTable[0].RelationShip = "Parent";
            pupilRecord.ContactTable[0].ParentalReponsibility = true;

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Generate Salutation
            pupilRecord = new PupilRecordPage();
            pupilRecord.ClickParentalSalutation();
            pupilRecord.ClickParentalAddressee();
            pupilRecord.MailingPoint = true;

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreNotEqual(null, pupilRecord.ContactTable[0], "The contact row is null");

            //Verify The contact is indicated as a Mailing Point.
            Assert.AreEqual(true, pupilRecord.MailingPoint);

            // Verify The contact has a parental salutation.
            Assert.AreNotEqual("", pupilRecord.ParentalSalutation);
            // Verify The contact has a parental addressee.
            Assert.AreNotEqual("", pupilRecord.ParentalAddress);

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify edit consent information sucessfully
        /// </summary>
        ///

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU15_Data")]
        public void TC_PU015_Verify_Edit_Consents_Section(string[] pupilInfo, string DateOfAdmission, string ConsentSignatory, string Note)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Aditional section
            pupilRecord.SelectConsentsTab();

            //Copyright Permission = Refused.
            var row = pupilRecord.Consents[0];
            row.Active = true;
            row.ConsentStatus = "Refused";
            row.Date = DateOfAdmission;
            row.ConsentSignatory = ConsentSignatory;
            row.Note = Note;

            //Internet Access = Received.
            row = pupilRecord.Consents[1];
            row.Active = true;
            row.ConsentStatus = "Received";
            row.Date = DateOfAdmission;
            row.ConsentSignatory = ConsentSignatory;
            row.Note = Note;

            //Photograph Student = Received.
            row = pupilRecord.Consents[2];
            row.Active = true;
            row.ConsentStatus = "Received";
            row.Date = DateOfAdmission;
            row.ConsentSignatory = ConsentSignatory;
            row.Note = Note;

            // Sex Education = Refused.
            row = pupilRecord.Consents[3];
            row.Active = true;
            row.ConsentStatus = "Refused";
            row.Date = DateOfAdmission;
            row.ConsentSignatory = ConsentSignatory;
            row.Note = Note;

            //Data Exchange = Received.
            row = pupilRecord.Consents[4];
            row.Active = true;
            row.ConsentStatus = "Received";
            row.Date = DateOfAdmission;
            row.ConsentSignatory = ConsentSignatory;
            row.Note = Note;

            //School Visit = Received.
            row = pupilRecord.Consents[5];
            row.Active = true;
            row.ConsentStatus = "Received";
            row.Date = DateOfAdmission;
            row.ConsentSignatory = ConsentSignatory;
            row.Note = Note;

            //- Share ULN related date = Refused. ---- NOT FOUND
            // Public Health Authority = Received.
            row = pupilRecord.Consents[6];
            row.Active = true;
            row.ConsentStatus = "Received";
            row.Date = DateOfAdmission;
            row.ConsentSignatory = ConsentSignatory;
            row.Note = Note;

            //- Medical Attention = Received. ---- 
            row = pupilRecord.Consents[7];
            row.Active = true;
            row.ConsentStatus = "Received";
            row.Date = DateOfAdmission;
            row.ConsentSignatory = ConsentSignatory;
            row.Note = Note;

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");


            row = pupilRecord.Consents[7];
            Assert.AreEqual(true, row.Active, "The value of Active is false");
            Assert.AreEqual("Received", row.ConsentStatus, "The value of Consent Status is not correct");
            Assert.AreEqual(DateOfAdmission, row.Date, "The Date value is not correct");
            Assert.AreEqual(ConsentSignatory, row.ConsentSignatory, "The value of consent Signature is not correct");
            Assert.AreEqual(Note, row.Note, "The value of note is not correct");

            row = pupilRecord.Consents[6];
            Assert.AreEqual(true, row.Active, "The value of Active is false");
            Assert.AreEqual("Received", row.ConsentStatus, "The value of Consent Status is not correct");
            Assert.AreEqual(DateOfAdmission, row.Date, "The Date value is not correct");
            Assert.AreEqual(ConsentSignatory, row.ConsentSignatory, "The value of consent Signature is not correct");
            Assert.AreEqual(Note, row.Note, "The value of note is not correct");

            row = pupilRecord.Consents[5];
            Assert.AreEqual(true, row.Active, "The value of Active is false");
            Assert.AreEqual("Received", row.ConsentStatus, "The value of Consent Status is not correct");
            Assert.AreEqual(DateOfAdmission, row.Date, "The Date value is not correct");
            Assert.AreEqual(ConsentSignatory, row.ConsentSignatory, "The value of consent Signature is not correct");
            Assert.AreEqual(Note, row.Note, "The value of note is not correct");

            row = pupilRecord.Consents[4];
            Assert.AreEqual(true, row.Active, "The value of Active is false");
            Assert.AreEqual("Received", row.ConsentStatus, "The value of Consent Status is not correct");
            Assert.AreEqual(DateOfAdmission, row.Date, "The Date value is not correct");
            Assert.AreEqual(ConsentSignatory, row.ConsentSignatory, "The value of consent Signature is not correct");
            Assert.AreEqual(Note, row.Note, "The value of note is not correct");

            row = pupilRecord.Consents[3];
            Assert.AreEqual(true, row.Active, "The value of Active is false");
            Assert.AreEqual("Refused", row.ConsentStatus, "The value of Consent Status is not correct");
            Assert.AreEqual(DateOfAdmission, row.Date, "The Date value is not correct");
            Assert.AreEqual(ConsentSignatory, row.ConsentSignatory, "The value of consent Signature is not correct");
            Assert.AreEqual(Note, row.Note, "The value of note is not correct");

            row = pupilRecord.Consents[2];
            Assert.AreEqual(true, row.Active, "The value of Active is false");
            Assert.AreEqual("Received", row.ConsentStatus, "The value of Consent Status is not correct");
            Assert.AreEqual(DateOfAdmission, row.Date, "The Date value is not correct");
            Assert.AreEqual(ConsentSignatory, row.ConsentSignatory, "The value of consent Signature is not correct");
            Assert.AreEqual(Note, pupilRecord.Consents[5].Note, "The value of note is not correct");

            row = pupilRecord.Consents[1];
            Assert.AreEqual(true, row.Active, "The value of Active is false");
            Assert.AreEqual("Received", row.ConsentStatus, "The value of Consent Status is not correct");
            Assert.AreEqual(DateOfAdmission, row.Date, "The Date value is not correct");
            Assert.AreEqual(ConsentSignatory, row.ConsentSignatory, "The value of consent Signature is not correct");
            Assert.AreEqual(Note, row.Note, "The value of note is not correct");

            row = pupilRecord.Consents[0];
            Assert.AreEqual(true, row.Active, "The value of Active is false");
            Assert.AreEqual("Refused", row.ConsentStatus, "The value of Consent Status is not correct");
            Assert.AreEqual(DateOfAdmission, row.Date, "The Date value is not correct");
            Assert.AreEqual(ConsentSignatory, row.ConsentSignatory, "The value of consent Signature is not correct");
            Assert.AreEqual(Note, row.Note, "The value of note is not correct");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add statutorySEN information sucessfully
        ///  SERIAL ONLY
        /// </summary>
        ///
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU16_Data")]
        public void TC_PU016_Verify_Add_StatutorySEN_Section(string[] pupilInfo, string Today, string EndDate, string SenStagesStage, string SenNeedType, string SenNeedsRank, string SenNeedsDescription, string SenProvisionsType, string Comment)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Aditional section
            pupilRecord.SelectStatutorySenTab();
            pupilRecord.ClickAddSENStage();
            pupilRecord.SenStages[0].Stage = SenStagesStage;
            pupilRecord.SenStages[0].StartDay = Today;

            pupilRecord.ClickAddSENNeed();
            pupilRecord.SenNeeds[0].NeedType = SenNeedType;
            pupilRecord.SenNeeds[0].Rank = SenNeedsRank;
            pupilRecord.SenNeeds[0].Description = SenNeedsDescription;
            pupilRecord.SenNeeds[0].StartDay = Today;
            pupilRecord.SenNeeds[0].EndDay = EndDate;
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Add document
            pupilRecord.SenNeeds[0].AddDocument();
            ViewDocumentDialog viewDocument = new ViewDocumentDialog();
            AddAttachmentDialog addAttchmentDialog = viewDocument.ClickAddAttachment();
            addAttchmentDialog.BrowserToDocument();
            viewDocument = addAttchmentDialog.UploadDocument();
            viewDocument.ClickOk();

            pupilRecord.ClickAddSENProvision();
            pupilRecord.SenProvisions[0].ProvisionType = SenProvisionsType;
            pupilRecord.SenProvisions[0].StartDay = Today;
            pupilRecord.SenProvisions[0].EndDay = EndDate;
            pupilRecord.SenProvisions[0].Comment = Comment;

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreEqual(SenStagesStage, pupilRecord.SenStages[0].Stage, "The Stage value is not correct");
            Assert.AreEqual(Today, pupilRecord.SenStages[0].StartDay, "The Start Date value is not correct");

            Assert.AreEqual(SenNeedType, pupilRecord.SenNeeds[0].NeedType, "The Need Type value is not correct");
            Assert.AreEqual(SenNeedsRank, pupilRecord.SenNeeds[0].Rank, "The Rank value is not correct");
            Assert.AreEqual(SenNeedsDescription, pupilRecord.SenNeeds[0].Description, "The Description value is not correct");
            Assert.AreEqual(Today, pupilRecord.SenNeeds[0].StartDay, "The Start Date value is not correct");
            Assert.AreEqual(EndDate, pupilRecord.SenNeeds[0].EndDay, "The End Date value is not correct");

            Assert.AreEqual(SenProvisionsType, pupilRecord.SenProvisions[0].ProvisionType, "The ProvisionType value is not correct");
            Assert.AreEqual(Today, pupilRecord.SenProvisions[0].StartDay, "The StartDate value is not correct");
            Assert.AreEqual(EndDate, pupilRecord.SenProvisions[0].EndDay, "The EndDay value is not correct");
            Assert.AreEqual(Comment, pupilRecord.SenProvisions[0].Comment, "The Comment value is not correct");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        /// TC PU19
        /// Au : An Nguyen
        /// Description: Exercise ability to search for existing pupil.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU019_Data")]
        public void TC_PU019_Exercise_ability_to_search_for_existing_pupil(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup, string className)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Search with forename and surname
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.IsCurrent = true;
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            Assert.AreNotEqual(null, pupilTile, "Search by surname and forename is incorrect");
            SeleniumHelper.CloseTab("Pupil Record");

            //Search by class name
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.IsCurrent = true;
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.Class = className;
            pupilSearchResults = pupilRecords.SearchCriteria.Search();
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            Assert.AreNotEqual(null, pupilTile, "Search by class name is incorrect");
            SeleniumHelper.CloseTab("Pupil Record");

            //Search by year group
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.IsCurrent = true;
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.YearGroup = yearGroup;
            pupilSearchResults = pupilRecords.SearchCriteria.Search();
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            Assert.AreNotEqual(null, pupilTile, "Search by year group is incorrect");

            #endregion

            #region Post-Condition

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC PU23
        /// Au : An Nguyen
        /// Description: Exercise ability to search for a Current, Future and Former pupil.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU023_Data")]
        public void TC_PU023_Exercise_ability_to_search_for_Current_Future_Former_pupil(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup, string className,
                    string dateLeaving, string reason, string readmitDay)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Search Current Pupil
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.IsCurrent = true;
            pupilRecords.SearchCriteria.IsFuture = false;
            pupilRecords.SearchCriteria.IsLeaver = false;
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();

            //Verify Search Current Pupil
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            Assert.AreNotEqual(null, pupilTile, "Search current pupil unsuccessfull");

            //Leaver pupil
            pupilRecord = pupilTile.Click<PupilRecordPage>();
            var pupilLeavingDetail = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");
            pupilLeavingDetail.DOL = dateLeaving;
            pupilLeavingDetail.ReasonForLeaving = reason;
            var confirmDialog = pupilLeavingDetail.ClickSave();
            confirmDialog.ClickOk();
            var leaverBackgroundProcessSubmitDialog = new LeaverBackgroundProcessSubmitDialog();
            leaverBackgroundProcessSubmitDialog.ClickOk();
            SeleniumHelper.CloseTab("Pupil Leaving Details");
            SeleniumHelper.CloseTab("Pupil Record");

            //Search Former Pupil
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.IsCurrent = false;
            pupilRecords.SearchCriteria.IsFuture = false;
            pupilRecords.SearchCriteria.IsLeaver = true;
            pupilSearchResults = pupilRecords.SearchCriteria.Search();

            //Verify Search Former Pupil
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            Assert.AreNotEqual(null, pupilTile, "Search former pupil unsuccessfull");

            //Re-admit pupil in future
            pupilRecord = pupilTile.Click<PupilRecordPage>();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var didYouMeanDialog = addNewPupilDialog.ContinueReAdmit();
            var formers = didYouMeanDialog.FormerPupils;
            var formerPupil = formers.Rows
                .FirstOrDefault(x => x.PupilName.Trim().Contains(String.Format("{0}, {1}", surName, foreName))
                                && x.DOB.Equals(DOB));
            var registrationDetailsDialog = formerPupil.ClickReEnrolPupilLink();

            //Enter data for registration details dialog
            registrationDetailsDialog.DateOfAdmission = readmitDay;
            registrationDetailsDialog.EnrolmentStatus = "Single Registration";
            registrationDetailsDialog.YearGroup = yearGroup;
            registrationDetailsDialog.ClassName = className;
            registrationDetailsDialog.CreateRecord();

            //Save pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            //Search Future Pupil
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.IsCurrent = false;
            pupilRecords.SearchCriteria.IsFuture = true;
            pupilRecords.SearchCriteria.IsLeaver = false;
            pupilSearchResults = pupilRecords.SearchCriteria.Search();

            //Verify Search Future Pupil
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            Assert.AreNotEqual(null, pupilTile, "Search future pupil unsuccessfull");

            #endregion

            #region Post-Condition

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC PU24
        /// Au : An Nguyen
        /// Description: Exercise ability to record a part-time attendance pattern for a pupil that is not 'Full Time' at the school.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1sharon", PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU024_Data")]
        public void TC_PU024_Exercise_ability_to_record_a_part_time_attendance_pattern(string foreName, string surName, string gender, string DOB,
                    string dateOfAdmission, string yearGroup, string className, string attendanceMode, string patternStart, string patternEnd)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            #endregion

            #region Test steps

            //Add AM-only pupil
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.AttendanceMode = attendanceMode;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            //Search attendance record
            SeleniumHelper.NavigateQuickLink("Edit Marks");
            var editMarkTriplet = new EditMarksTriplet();
            editMarkTriplet.SearchCriteria.Week = true;
            editMarkTriplet.SearchCriteria.SelectClass(className);
            editMarkTriplet.SearchCriteria.SelectYearGroup(yearGroup);
            var editMarks = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            editMarks.ModePreserve = false;
            var editMarkTable = editMarks.Marks;

            //Enter values
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][1].Text = "/";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][2].Text = @"\";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][3].Text = "/";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][4].Text = @"\";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][5].Text = "/";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][6].Text = @"\";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][7].Text = "/";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][8].Text = @"\";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][9].Text = "/";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][10].Text = @"\";
            editMarks.Save();

            //Search attendance record again
            editMarkTriplet = new EditMarksTriplet();
            editMarks = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            editMarks.ModePreserve = false;
            editMarkTable = editMarks.Marks;

            //Enter values
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][1].Text = "";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][2].Text = "";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][3].Text = "";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][4].Text = "";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][5].Text = "";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][6].Text = "";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][7].Text = "";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][8].Text = "";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][9].Text = "";
            editMarkTable[String.Format("{0}, {1}", surName, foreName)][10].Text = "";

            //Save values
            editMarks.Save();
            var warningDialog = new POM.Components.Attendance.WarningConfirmDialog();
            warningDialog.ClickContinueDelete();
            SeleniumHelper.CloseTab("Edit Marks");

            //Navigate to pupil record and search pupil
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            var pupilSearchResults = pupilRecordTriplet.SearchCriteria.Search();
            pupilRecord = pupilSearchResults.FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName))).Click<PupilRecordPage>();

            //Select "Attendance Pattern" and input data
            var attendancePattern = SeleniumHelper.NavigateViaAction<AttendancePatternDialog>("Attendance Pattern");
            attendancePattern.IsPreserve = true;
            attendancePattern.StartDate = patternStart;
            attendancePattern.EndDate = patternEnd;
            var patternRow = attendancePattern.Table[0];
            patternRow.MonAM = "2";
            patternRow.TueAM = "2";
            patternRow.WedAM = "2";
            patternRow.ThuAM = "2";
            patternRow.FriAM = "2";

            //Apply pattern
            var confirmDialog = attendancePattern.ClickApply();
            confirmDialog.ClickOk();
            attendancePattern.ClickClose();
            pupilRecord.SavePupil();

            //Navigate to Edit Marks
            SeleniumHelper.NavigateQuickLink("Edit Marks");
            editMarkTriplet = new EditMarksTriplet();
            editMarkTriplet.SearchCriteria.Week = true;
            editMarkTriplet.SearchCriteria.SelectClass(className);
            editMarkTriplet.SearchCriteria.SelectYearGroup(yearGroup);
            editMarks = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();

            //Verify data
            var marksRow = editMarks.Marks[String.Format("{0}, {1}", surName, foreName)];
            Assert.AreEqual("2", marksRow[1].Text, "Attendance Pattern do not apply to Monday AM");
            Assert.AreEqual("2", marksRow[3].Text, "Attendance Pattern do not apply to Tuesday AM");
            Assert.AreEqual("2", marksRow[5].Text, "Attendance Pattern do not apply to Wednesday AM");
            Assert.AreEqual("2", marksRow[7].Text, "Attendance Pattern do not apply to Thursday AM");
            Assert.AreEqual("2", marksRow[9].Text, "Attendance Pattern do not apply to Friday AM");

            #endregion

            #region Post-Condition

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Hieu Pham
        /// Descriptions: Exercise ability to invoke a 'Related Link' namely - Previous Name History.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU044A_DATA")]
        public void TC_PU044A_Adminstrator_History_Name(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string YearGroup, string className, string pupilName, string foreName, string middleName, string surName, string reason, string date)
        {
            #region Pre-condition
            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            // Enter values
            addNewPupilDialog.Forename = pupilForeName;
            addNewPupilDialog.SurName = pupilSurName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = dateOfBirth;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = DateOfAdmission;
            registrationDetailDialog.YearGroup = YearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();

            // Confirm create new pupil
            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            // Save values
            var pupilRecord = new PupilRecordPage();
            pupilRecord.SavePupil();

            #endregion

            #region Steps
            // Search pupil record
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();

            // Navigate to 'Previous Name History'
            var nameChangeHistoryPage = SeleniumHelper.NavigateViaAction<NameChangeHistoryPage>("Previous Name History");
            var nameHistoryTable = nameChangeHistoryPage.NameHistoryTable;

            // Verify 'Name change history' page displays for this pupil
            Assert.AreEqual(true, nameChangeHistoryPage.IsNameChangeHistoryForPupilName(pupilName), "Name change history page displays for another pupil");

            //Delete record
            var existedRow = nameHistoryTable.Rows.FirstOrDefault(x => x.LegalSurName.Equals(surName) && x.LegalMiddleName.Equals(middleName)
                && x.LegalForeName.Equals(foreName) && x.Reason.Equals(reason) && x.DateOfChange.Equals(date));
            nameChangeHistoryPage.DeleteRow(existedRow);

            // Enter values
            var emptyRow = nameHistoryTable.Rows.FirstOrDefault(x => x.LegalForeName.Trim().Equals(""));
            emptyRow.LegalForeName = foreName;
            emptyRow.LegalMiddleName = middleName;
            emptyRow.LegalSurName = surName;
            emptyRow.Reason = reason;
            emptyRow.DateOfChange = date;

            // Save values
            nameChangeHistoryPage.Save();

            // Exit screen
            SeleniumHelper.CloseTab("Name Change History");

            // Search pupil again
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();

            // Navigate to name change history page
            nameChangeHistoryPage = SeleniumHelper.NavigateViaAction<NameChangeHistoryPage>("Previous Name History");
            nameHistoryTable = nameChangeHistoryPage.NameHistoryTable;

            // Verify record is existed
            Assert.AreNotEqual(null, nameHistoryTable.Rows.FirstOrDefault(x => x.LegalForeName.Equals(foreName) && x.LegalMiddleName.Equals(middleName)
                && x.LegalSurName.Equals(surName) && x.Reason.Equals(reason) && x.DateOfChange.Equals(date)), "Name change history is not correct");

            // Exit screen
            SeleniumHelper.CloseTab("Name Change History");

            #endregion

            #region Post-condition

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName)));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Hieu Pham
        /// Descriptions: Exercise ability to invoke a 'Related Link' namely -'Previous Address History'.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU044B_DATA")]
        public void TC_PU044B_Adminstrator_History_Address(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string YearGroup, string className, string pupilName, string startDate, string endDate, string note)
        {
            #region Pre-condition
            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            // Enter values
            addNewPupilDialog.Forename = pupilForeName;
            addNewPupilDialog.SurName = pupilSurName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = dateOfBirth;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = DateOfAdmission;
            registrationDetailDialog.YearGroup = YearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();

            // Confirm create new pupil
            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            // Save values
            var pupilRecord = new PupilRecordPage();
            pupilRecord.SavePupil();

            #endregion

            #region Steps
            // Search pupil record
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();

            // Navigate to 'Previous Address History'
            SeleniumHelper.NavigateViaAction<PreviousAddressPage>("Previous Address History");

            // Verify grid is empty
            var previousAddressPage = new PreviousAddressPage();
            var addressGrid = previousAddressPage.PreviousAddress;
            var numberOfRows = addressGrid.Rows.Count;
            Assert.AreEqual(0, numberOfRows, "Address Grid is not empty");

            // Close tab 
            SeleniumHelper.CloseTab("Previous Address History");

            // Search pupil record
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();

            // Add address
            pupilRecordPage.SelectAddressesTab();
            var addAddressDialog = pupilRecordPage.ClickAddAddress();
            addAddressDialog.ClickOk(10);
            pupilRecordPage.Refresh();

            // Add start date, end date and note to address
            var pupilAddressGrid = pupilRecordPage.AddressTable;
            var row = pupilAddressGrid.Rows.FirstOrDefault();
            row.StartDate = startDate;
            row.EndDate = endDate;
            row.Note(note);

            // Save values
            pupilRecordPage.Refresh();
            pupilRecordPage.SavePupil();

            // Confirm message success displays
            Assert.AreEqual(true, pupilRecordPage.IsSuccessMessageDisplayed(), "Success message does not displays");

            // Navigate to Previous Address History
            SeleniumHelper.NavigateViaAction<PreviousAddressPage>("Previous Address History");
            previousAddressPage = new PreviousAddressPage();
            addressGrid = previousAddressPage.PreviousAddress;

            // Verify previous address is displays
            var rowPreviousAddress = addressGrid.Rows.FirstOrDefault(x => x.StartDate.Equals(startDate) && x.EndDate.Equals(endDate) && x.Note.Equals(note));
            Assert.AreNotEqual(null, rowPreviousAddress, "Previous Address History is not correct");

            // Close tab
            SeleniumHelper.CloseTab("Previous Address History");

            #endregion

            #region Post-condition

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName)));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }


        /// <summary>
        /// TC PU62
        /// Au : An Nguyen
        /// Description: Exercise ability to view a Pupil's Record.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU062_Data")]
        public void TC_PU062_Exercise_ability_to_view_a_Pupil_Record(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup, string quickNote)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Invoke the 'Pupil Record' screen via the Quick Link
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();

            //Perform an advance search (Show more selected) and perform a search for pupils by 'Enrolment Status' of "Single Registration" 
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.YearGroup = yearGroup;
            pupilRecords.SearchCriteria.EnrolmentStatus = "Single Registration";
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();

            //Select the 'Pupil'
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Confirm on selection of a pupil from the results list the central data maintenance panel displays the recorded data for this pupil
            Assert.AreEqual(pupilTile.Name, String.Format("{0}, {1}", pupilRecord.PreferSurname, pupilRecord.PreferForeName, "Name of pupil is incorrect"));

            //Edit QuickNote
            pupilRecord.QuickNote = quickNote;
            pupilRecord.SavePupil();
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.YearGroup = yearGroup;
            pupilRecords.SearchCriteria.EnrolmentStatus = "Single Registration";
            pupilSearchResults = pupilRecords.SearchCriteria.Search();
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Verify QuickNote can be write
            Assert.AreEqual(quickNote, pupilRecord.QuickNote, "Data of pupil can not write");

            #endregion

            #region Post-Condition

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC PU67
        /// Au : An Nguyen
        /// Description: Exercise ability to expand a pupil's 'Pervious Name' history by recording 'Name Change History' records.
        /// Role: School Administrator
        /// </summary>
        //TODO: Duplication. Hence P4
        //[WebDriverTest(TimeoutSeconds = 1500, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority4 }, DataProvider = "TC_PU067_Data")]
        public void TC_PU067_Exercise_ability_to_expand_pupil_Pervious_Name_history_Name_Change_History(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup,
                    string[] firstPrevious, string[] secondPrevious, string[] thirdPrevious)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.IsCurrent = true;
            pupilRecords.SearchCriteria.IsFuture = false;
            pupilRecords.SearchCriteria.IsLeaver = false;
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();

            //Select the 'Pupil'
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Navigate to "Previous Name History"
            var nameHistory = SeleniumHelper.NavigateViaAction<NameChangeHistoryPage>("Previous Name History");

            //Add First Previous Name
            var nameHistoryRow = nameHistory.NameHistoryTable.Rows.Last();
            nameHistoryRow.LegalForeName = firstPrevious[0];
            nameHistoryRow.LegalMiddleName = firstPrevious[1];
            nameHistoryRow.LegalSurName = firstPrevious[2];
            nameHistoryRow.Reason = firstPrevious[3];
            nameHistoryRow.DateOfChange = firstPrevious[4];
            nameHistory.Save();

            //Add Second Previous Name
            nameHistoryRow = nameHistory.NameHistoryTable.Rows.Last();
            nameHistoryRow.LegalForeName = secondPrevious[0];
            nameHistoryRow.LegalMiddleName = secondPrevious[1];
            nameHistoryRow.LegalSurName = secondPrevious[2];
            nameHistoryRow.Reason = secondPrevious[3];
            nameHistoryRow.DateOfChange = secondPrevious[4];
            nameHistory.Save();

            //Add Third Previous Name
            nameHistoryRow = nameHistory.NameHistoryTable.Rows.Last();
            nameHistoryRow.LegalForeName = thirdPrevious[0];
            nameHistoryRow.LegalMiddleName = thirdPrevious[1];
            nameHistoryRow.LegalSurName = thirdPrevious[2];
            nameHistoryRow.Reason = thirdPrevious[3];
            nameHistoryRow.DateOfChange = thirdPrevious[4];
            nameHistory.Save();

            //Close Tab to return Home Page
            SeleniumHelper.CloseTab("Name Change History");

            //Navigate to "Name History Change" from Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.IsCurrent = true;
            pupilRecords.SearchCriteria.IsFuture = false;
            pupilRecords.SearchCriteria.IsLeaver = false;
            pupilSearchResults = pupilRecords.SearchCriteria.Search();
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();
            nameHistory = SeleniumHelper.NavigateViaAction<NameChangeHistoryPage>("Previous Name History");

            //Verify data First Previous Name
            var nameHistoryRows = nameHistory.NameHistoryTable.Rows;
            nameHistoryRow = nameHistoryRows.FirstOrDefault(t => t.LegalForeName.Equals(firstPrevious[0])
                && t.LegalMiddleName.Equals(firstPrevious[1])
                && t.LegalSurName.Equals(firstPrevious[2]));
            Assert.AreNotEqual(null, nameHistoryRow, "Add First Previous Name unsuccessfull");
            Assert.AreEqual(firstPrevious[3], nameHistoryRow.Reason, "Reason of First previous name is incorrect");
            Assert.AreEqual(firstPrevious[4], nameHistoryRow.DateOfChange, "Date of change of First previous name is incorrect");

            //Verify data Second Previous Name
            nameHistoryRow = nameHistoryRows.FirstOrDefault(t => t.LegalForeName.Equals(secondPrevious[0])
                && t.LegalMiddleName.Equals(secondPrevious[1])
                && t.LegalSurName.Equals(secondPrevious[2]));
            Assert.AreNotEqual(null, nameHistoryRow, "Add Second Previous Name unsuccessfull");
            Assert.AreEqual(secondPrevious[3], nameHistoryRow.Reason, "Reason of Second previous name is incorrect");
            Assert.AreEqual(secondPrevious[4], nameHistoryRow.DateOfChange, "Date of change of Second previous name is incorrect");

            //Verify data Third Previous Name
            nameHistoryRow = nameHistoryRows.FirstOrDefault(t => t.LegalForeName.Equals(thirdPrevious[0])
                && t.LegalMiddleName.Equals(thirdPrevious[1])
                && t.LegalSurName.Equals(thirdPrevious[2]));
            Assert.AreNotEqual(null, nameHistoryRow, "Add Third Previous Name unsuccessfull");
            Assert.AreEqual(thirdPrevious[3], nameHistoryRow.Reason, "Reason of Third previous name is incorrect");
            Assert.AreEqual(thirdPrevious[4], nameHistoryRow.DateOfChange, "Date of change of Third previous name is incorrect");

            #endregion

            #region Post-Condition

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC PU68
        /// Au : An Nguyen
        /// Description: Exercise ability to expand a pupil's enrolment history record whilst they are currently enrolled at a school. 
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU068_Data")]
        public void TC_PU068_Exercise_ability_to_expand_a_pupil_enrolment_history_record(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup,
            string endGuest, string startSub, string endSub, string startMain, string endMain, string startSingle)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps


            //Navigate to pupil records page
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();

            //Search pupil
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.YearGroup = yearGroup;
            pupilRecords.SearchCriteria.EnrolmentStatus = "Single Registration";
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();

            //Select the 'Pupil'
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Navigate to the 'Registration' section
            pupilRecord.SelectRegistrationTab();

            //Change 'Single Registration' to 'Guest Pupil'
            var enrolmentRows = pupilRecord.EnrolmentStatusHistoryTable.Rows;
            var enrolmentRow = enrolmentRows.FirstOrDefault(t => t.EnrolmentStatus.Equals("Single Registration"));
            enrolmentRow.EnrolmentStatus = "Guest Pupil";
            enrolmentRow.EndDate = endGuest;

            //Add new 'Subsidiary - Dual registration'
            enrolmentRow = pupilRecord.EnrolmentStatusHistoryTable.Rows.Last();
            enrolmentRow.EnrolmentStatus = "Subsidiary – Dual Registration";
            enrolmentRow.StartDate = startSub;
            enrolmentRow.EndDate = endSub;

            //Add new 'Main – Dual Registration'
            enrolmentRow = pupilRecord.EnrolmentStatusHistoryTable.Rows.Last();
            enrolmentRow.EnrolmentStatus = "Main – Dual Registration";
            enrolmentRow.StartDate = startMain;
            enrolmentRow.EndDate = endMain;

            //Add new 'Single Registration'
            enrolmentRow = pupilRecord.EnrolmentStatusHistoryTable.Rows.Last();
            enrolmentRow.EnrolmentStatus = "Single Registration";
            enrolmentRow.StartDate = startSingle;

            //Save pupil
            pupilRecord.SavePupil();
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Pupil can not be saved");
            pupilRecord.SelectRegistrationTab();
            enrolmentRows = pupilRecord.EnrolmentStatusHistoryTable.Rows;

            //Verify 'Subsidiary - Dual registration'
            enrolmentRow = enrolmentRows.FirstOrDefault(t => t.EnrolmentStatus.Equals("Subsidiary – Dual Registration"));
            Assert.AreEqual(startSub, enrolmentRow.StartDate, "Start Date of Subsidiary – Dual Registration is incorrect");
            Assert.AreEqual(endSub, enrolmentRow.EndDate, "End Date of Subsidiary – Dual Registration is incorrect");

            //Verify 'Main - Dual registrationn'
            enrolmentRow = enrolmentRows.FirstOrDefault(t => t.EnrolmentStatus.Equals("Main – Dual Registration"));
            Assert.AreEqual(startMain, enrolmentRow.StartDate, "Start Date of Main – Dual Registration is incorrect");
            Assert.AreEqual(endMain, enrolmentRow.EndDate, "End Date of Main – Dual Registration is incorrect");

            //Verify 'Single Registration'
            enrolmentRow = enrolmentRows.FirstOrDefault(t => t.EnrolmentStatus.Equals("Single Registration"));
            Assert.AreEqual(startSingle, enrolmentRow.StartDate, "Start Date of Single Registration is incorrect");

            #endregion

            #region Post-Condition

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

 
        #region DATA

        public List<object[]> TC_PU01_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU01";

            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU01";

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    //ForeName
                    "aPupilForeName"+randomCharacter+random.ToString(),
                    // Middle Name
                    randomCharacter+"aPupilMiddleName"+randomCharacter+random.ToString(),
                    // SurName
                    randomCharacter+"aPupilSurName"+randomCharacter+random.ToString(),
                    // Gender
                    "Male",
                    // DOB
                    PupilDateOfBirth,
                    PupilDateOfAdmission,
                    // Year Group
                    "Year 2",
                    "2A",
                    "Full-Time",
                    "Not a Boarder"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU02_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU02";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU02";

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},                   
                    true,
                    // Quick Note
                    "This is Quick Note"                                        
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU03_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU03";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU03";

            var res = new List<Object[]>
            {                
                new object[] 
                {
                     new string[]{"aForeName"+randomCharacter+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},                                        
                }
                
            };
            return res;
        }
        public List<object[]> TC_PU04_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU04";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU04";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    // New Address
                    new string[]{"123", "House Name", "Flat", "Street", "District", "City", "United Kingdom", "EC1A 1BB", "United Kingdom"}
                }
                
            };
            return res;
        }


        public List<object[]> TC_PU05_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU05";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU05";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    // New Address
                    new string[]{"0123456789", "Other", "Note for Phone Number"},
                    new string[]{"abcd@c2kni.org.uk", "Other", "Note for Email Address"},
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU06_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU06";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU06";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                                        // New Address
                    new string[]{"123", "House Name", "Flat", "Street", "District", "City", "United Kingdom", "EC1A 1BB", "United Kingdom"}

                }
                
            };
            return res;
        }

        public List<object[]> TC_PU07_Data()
        {
            string pattern = "M/d/yyyy";
            string EligibleFreeMealStartDatePast = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string EligibleFreeMealEndDatePast = DateTime.ParseExact("06/01/2008", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string EligibleFreeMealStartDateToday = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string EligibleFreeMealEndDateToday = DateTime.ParseExact("06/01/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            // Meal Pattern
            string MealPatternStartDate = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string MealPatternEndDate = DateTime.ParseExact("06/01/2008", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string MealPatternStartToday = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string MealPatternEndToday = DateTime.ParseExact("06/01/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU07";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU07";

            var res = new List<Object[]>
            {                
                new object[] 
                {
                   new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    // Eligible Free Meal
                    new string[]{ EligibleFreeMealStartDatePast, EligibleFreeMealEndDatePast, EligibleFreeMealStartDateToday,EligibleFreeMealEndDateToday,"This is note"},
                    // Meal Pattern
                    new string[]{MealPatternStartDate,MealPatternEndDate,MealPatternStartToday,MealPatternEndToday}
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU08_Data()
        {
            string pattern = "M/d/yyyy";
            string dateEvent = DateTime.ParseExact("06/06/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU08";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU08";
            var res = new List<Object[]>
            {            
                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //NSHNumber
                    "123456",
                    // Medical Practice
                    new string [] {"Bushmills Medical Centre","Dr T Brown"},
                    // Summary Note
                    "Summary",
                    // Medical Description
                    "None",
                    // Medical Event
                    new string [] {"Accident","Accident",dateEvent}
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU09_Data()
        {
            string pattern = "M/d/yyyy";
            string dateStart = DateTime.ParseExact("06/06/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateEnd = DateTime.ParseExact("06/06/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU09_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU09_Data";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //Ethnicity
                    "Vietnamese",
                    //HomeLanguage
                    "English",
                    //religion
                    "Church of God",
                    //AccommodationType
                    "Other",
                    //AsylumStatus
                    "Refugee",
                    //NewcomerPeriods
                    new string [] {dateStart,dateEnd},
                    dateStart
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU10_Data()
        {
            string pattern = "M/d/yyyy";
            string dateStart = DateTime.ParseExact("06/06/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateEnd = DateTime.ParseExact("06/06/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU010";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "TC_PU010";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //ServiceChildren
                    "Unknown",
                    //ServiceChildrenSource
                    "Other",
                    //ModeTravel
                    "Taxi",
                    //TravelRoute
                    "",
                    //AcademicYear                    
                    "Academic Year 2016/2017",
                    //LearnerUniformGrantEligibilities
                    new string [] {dateStart,dateEnd}
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU11_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstDay = new DateTime(now.Year, now.Month, 1);
            var lastDay = new DateTime(now.Year, 12, 31);
            string dateStart = firstDay.ToString(pattern);
            string dateEnd = lastDay.ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU11_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU11_Data";

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //CareAuthority
                    "Northern Health and Social Care Trust",
                    //LivingArrangement
                   "Foster Care",
                    dateStart,
                    dateEnd
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU12_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstStartDay = new DateTime(now.Year, now.Month, now.Day);
            var firstEndDay = firstStartDay.AddMonths(3);
            string dateStart = firstStartDay.ToString(pattern);
            string dateEnd = firstEndDay.ToString(pattern);
            string notes = "Personal Education Plan Note";

            var secondStartDate = firstEndDay.AddDays(1);
            var secondEndDate = new DateTime(firstEndDay.Year, 12, 25);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU12_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU12_Data";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},           
                    dateStart,
                    dateEnd,
                    notes,
                    secondStartDate.ToString(pattern),
                    secondEndDate.ToString(pattern),
                    //CareAuthority
                    "Northern Health and Social Care Trust",
                    //LivingArrangement
                   "Foster Care",
                }
                
            };
            return res;
        }


        public List<object[]> TC_PU12a_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstStartDay = new DateTime(now.Year, now.Month, now.Day);
            var firstEndDay = firstStartDay.AddMonths(3);
            string dateStart = firstStartDay.ToString(pattern);
            string dateEnd = firstEndDay.ToString(pattern);
            string notes = "Personal Education Plan Note";

            string pupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU012a";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU012a";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",pupilDateOfBirth,pupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    dateStart,
                    dateEnd,
                    notes,
                    //CareAuthority
                    "Northern Health and Social Care Trust",
                    //LivingArrangement
                   "Foster Care"
                }

            };
            return res;
        }

        public List<object[]> TC_PU12b_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstStartDay = new DateTime(now.Year, now.Month, now.Day);
            var firstEndDay = firstStartDay.AddMonths(3);
            string dateStart = firstStartDay.ToString(pattern);
            string dateEnd = firstEndDay.ToString(pattern);
            string notes = "Young Carer Notes";

            var secondStartDate = firstEndDay.AddDays(1);
            var secondEndDate = new DateTime(firstEndDay.Year, 12, 25);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU12b_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU12b_Data";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year  2","2A","Full-Time","Not a Boarder"},
                    dateStart,
                    dateEnd,
                    notes
                }

            };
            return res;
        }


        public List<object[]> TC_PU12c_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstStartDay = new DateTime(now.Year, now.Month, now.Day);
            var firstEndDay = firstStartDay.AddMonths(3);
            string dateStart = firstStartDay.ToString(pattern);
            string dateEnd = firstEndDay.ToString(pattern);
            string notes = "Disability Notes";

            var secondStartDate = firstEndDay.AddDays(1);
            var secondEndDate = new DateTime(firstEndDay.Year, 12, 25);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU12c_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU12c_Data";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year  2","2A","Full-Time","Not a Boarder"},
                    dateStart,
                    dateEnd,
                    notes
                }

            };
            return res;
        }

        public List<object[]> TC_PU13_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU13_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU13_Data";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},                       
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU14_Data()
        {
            string pattern = "M/d/yyyy";
            var year = DateTime.Now.Year;
            var newYear = year++;
            string yearEdit = String.Format("{0}/{1}", year, newYear);
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU014";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU014";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    yearEdit         
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU15_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU015";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU015";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},         
                    PupilDateOfAdmission,
                    //ConsentSignatory
                    "Parent Signatory Received",
                    //Note
                    "Parental consent status duly recorded"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU16_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var toDay = new DateTime(now.Year, now.Month, now.Day);

            var endDate = new DateTime(toDay.Year, 12, 25);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU016";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU016";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},       
                   toDay.ToString(pattern),
                   endDate.ToString(pattern),
                   //SenStagesStage
                   "Identify need",
                   // SenNeedType
                   "Mild Learning Difficulties",
                   //SenNeedsRank
                   "1",
                   //Sen Description
                   "GP",
                   //SenProvisionsType
                   "Time in Specialist Unit",
                   // Comment
                   "Requested Time in Special Unit"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU17_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU017";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU017";

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},         
                    // Note
                    "Remove"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU019_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU020_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                    new string[]{"1234", "House", "Flat", "Street", "District", "City", "County", "EC1A 1BB", "United Kingdom"},
                    new string[]{"567", "House Name", "Flat2", "Street2", "District2", "City2", "Conty2", "EC1A 1BB", "United Kingdom"},
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU021_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string pastStart1 = DateTime.ParseExact("01/01/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pastEnd1 = DateTime.ParseExact("05/05/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pastStart2 = DateTime.ParseExact("06/05/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pastEnd2 = DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                    new string[]{"123", "House", "Flat", "Street", "District", "City", "County", "EC1A 1BB", "United Kingdom"}, pastStart1, pastEnd1,
                    new string[]{"234", "House Name", "Flat2", "Street2", "District2", "City2", "County2", "EC1A 1BB", "United Kingdom"}, pastStart2, pastEnd2,
                    new string[]{"567", "House Name", "Flat3", "Street3", "District3", "City3", "County2", "EC1A 1BB", "United Kingdom"},
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU022_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                    "Mr", "LG", "Contact",
                    new string[]{"1234", "House", "Flat", "Street", "District", "City", "County", "EC1A 1BB", "United Kingdom"},
                    new string[]{"567", "House Name", "Flat2", "Street2", "District2", "City2", "Conty2", "EC1A 1BB", "United Kingdom"},
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU023_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfLeaving = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(14))).ToString(pattern);
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Add(TimeSpan.FromDays(14))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    foreName, surName, "Male", dateOfBirth, PupilDateOfAdmission, "Year 2", "SEN", dateOfLeaving, "Not Known", dateOfAdmission
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU024_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string patternStart = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToString(pattern);
            string patternEnd = SeleniumHelper.GetLastDayOfWeek(DateTime.Now).ToString(pattern);
            string foreName = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Aa", SeleniumHelper.GenerateRandomString(7));
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    foreName, surName, "Male", dateOfBirth, dateOfAdmission, "Year 2", "SEN", "AM only", patternStart, patternEnd,
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU025_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName1 = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName1 = String.Format("{0}_{1}", "Aa", SeleniumHelper.GenerateRandomString(7));
            string foreName2 = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName2 = String.Format("{0}_{1}", "Ab", SeleniumHelper.GenerateRandomString(7));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{foreName1, surName1, "Male", dateOfBirth, dateOfAdmission}, 
                    new string[]{foreName2, surName2, "Female", dateOfBirth, dateOfAdmission},
                    "Single Registration", "Year 2", "SEN", dateOfLeaving, "Voluntary transfer", "Queens Secondary School"
                }
                
            };
            return res;
        }


        public List<object[]> TC_PU026_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Add(TimeSpan.FromDays(7))).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU026";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU026";

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"AFutureTest"+randomCharacter+random, "Aaa", "Male", dateOfBirth, dateOfAdmission}, 
                    new string[]{"AFutureTest"+randomCharacter+random, "Aab", "Female", dateOfBirth, dateOfAdmission},
                    "Single Registration", "Year 5", "5A", dateOfLeaving, "Not Known", "Kings Secondary School"
                }
                
            };
            return res;
        }


        public List<object[]> TC_PU027_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Forename
                    randomName,
                    // Surname
                    randomName,
                    // Gender
                    "Male",
                    // DOB
                    "2/2/2011",
                    //DateOfAdmission
                    "10/5/2014",
                    // YearGroup
                    "Year 1",
                    // DOL
                    "11/5/2014",
                    // ReasonForLeaving
                    "Not Known",
                    // EnrolmentStatus
                    "Single Registration",
                    //re-admit date
                    "10/5/2015",
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU028_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Forename
                    randomName,
                    // Surname
                    randomName,
                    // Gender
                    "Male",
                    // DOB
                    "2/2/2011",
                    //DateOfAdmission
                    "10/5/2014",
                    // YearGroup
                    "Year 1",
                    // DOL
                    "11/5/2014",
                    // ReasonForLeaving
                    "Not Known",
                    // EnrolmentStatus
                    "Single Registration",
                    //re-admit date
                    "10/5/2015",
                }
                
            };
            return res;
        }

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

        public List<object[]> TC_PU034_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name,
                    "Bains, Kirk",
                    // Forename pupil
                    "Bains", 
                    // Surname pupil
                    "Kirk"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU036a_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // forename,
                    randomName,
                    // surname
                    randomName
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU036b_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(6) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // forename,
                    randomName,
                    // surname
                    randomName
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU036c_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // forename,
                    randomName,
                    // surname
                    randomName
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU037_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name,
                    "Bains, Kirk",
                    // fore name
                    "Bains"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU038_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name,
                    "Bains, Kirk",
                    // fore name
                    "Bains"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU039_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name,
                    "Bains, Kirk",
                    // fore name
                    "Bains"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU040_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name,
                    "Bains, Kirk"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU041_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name,
                    "Bains, Kirk"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU043_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(8));
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, dateOfLeaving, "Not Known", "Outside Local District"}

                };
            return data;
        }

        public List<object[]> TC_PU044A_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(8));
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string date = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -7);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "Charles", "Jon", 
                        "Man-Warning", "Deed Poll", date }

                };
            return data;
        }

        public List<object[]> TC_PU044B_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(8));
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -14);
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -7);



            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, startDate, endDate, "This is now a former address"}

                };
            return data;
        }

        public List<object[]> TC_PU044C_DATA()
        {

            string pattern = "M/d/yyyy";
            string surName = String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(8));
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string registerDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -7);
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday, true), -7);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, pupilName, registerDate, endDate
                    , "SEN", "Year 2", "2", "4"}

                };
            return data;
        }

        public List<object[]> TC_PU044D_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(8));
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetToDay();
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 1);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "Baker, Jade", 
                        "New Suspension", "Substance Abuse", startDate, endDate, "8:30 AM", "4:45 PM", "1"}

                };
            return data;
        }


        public List<object[]> TC_PU062_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "LGG Test",
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU063_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", 
                    "In-school provision", startDate},
            };
            return res;
        }

        public List<object[]> TC_PU064_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", 
                    "Severe Learning Difficulties", startDate, "SLD Confirmed by family GP"},
            };
            return res;
        }

        public List<object[]> TC_PU065_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string endDate = DateTime.Now.Add(TimeSpan.FromDays(365)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                    "Time in Specialist Unit", startDate, endDate, "Time in special unit approved by family GP"},
            };
            return res;
        }

        public List<object[]> TC_PU066a_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string previousMonday = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string startTime = DateTime.ParseExact("10:30", "h:mm", CultureInfo.InvariantCulture).ToString("h:mm tt");
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            int staff = SeleniumHelper.GenerateRandomNumber(15);
            var res = new List<Object[]>
            {                
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                        "In-school provision",
                       "Initial", "Meeting Completed", previousMonday, startTime, "Math Room 11",
                       staff, "Test", "Test", "SEN Co-ordinator", previousMonday},
            };
            return res;
        }

        public List<object[]> TC_PU066b_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string previousMonday = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                        "In-school provision",
                       previousMonday, previousMonday, 
                       "ELB Agreed", "Proposed Statement made",
                       previousMonday, previousMonday},
            };
            return res;
        }

        public List<object[]> TC_PU067_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string past3Month = DateTime.Now.Subtract(TimeSpan.FromDays(90)).ToString(pattern);
            string past2Month = DateTime.Now.Subtract(TimeSpan.FromDays(60)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[] {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                    new string[]{"Charlie", "Sage", "River", "Deed Poll", past3Month},
                    new string[]{"Taylor", "Logan", "Kerry", "Adoption", past2Month},
                    new string[]{"Jessie", "Casey", "Harley", "Marriage", past2Month}
                },
            };
            return res;
        }

        public List<object[]> TC_PU068_Data()
        {

            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            DateTime twoYearPast = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(730)));
            string endGuest = twoYearPast.Subtract(TimeSpan.FromDays(1)).ToString(pattern);
            string startSub = twoYearPast.ToString(pattern);
            string endSub = twoYearPast.Add(TimeSpan.FromDays(1)).ToString(pattern);
            string startMain = twoYearPast.Add(TimeSpan.FromDays(2)).ToString(pattern);
            string endMain = twoYearPast.Add(TimeSpan.FromDays(20)).ToString(pattern);
            string startSingle = twoYearPast.Add(TimeSpan.FromDays(21)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                     endGuest, startSub, endSub, startMain, endMain, startSingle,
                }
            };
            return res;
        }

        public List<object[]> TC_PU069_Data()
        {
            var code = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(4), SeleniumHelper.GenerateRandomNumber(999));
            var description = String.Format("{0}_{1}_{2}", "Description", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var displayOrder = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {                
                new object[] {code, description, displayOrder, "Physical"},
            };
            return res;
        }

        public List<object[]> TC_PU070_Data()
        {
            var code = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(4), SeleniumHelper.GenerateRandomNumber(999));
            var description = String.Format("{0}_{1}_{2}", "Description", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var displayOrder = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {                
                new object[] {code, description, displayOrder},
            };
            return res;
        }

        public List<object[]> TC_PU071_Data()
        {
            var code = String.Format("{0}_{1}", "11", SeleniumHelper.GenerateRandomNumber(999));
            var description = String.Format("{0}_{1}_{2}", "Description", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var displayOrder = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {                
                new object[] {code, description, displayOrder, "In-school provision"},
            };
            return res;
        }

        public List<object[]> TC_PU072_Data()
        {
            string pupilName = String.Format("PU072_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    //SenStage name
                    "In-school provision",
                    //SenNeeds
                    new string[]{"Mild Learning Difficulties", "Dangerous Fluid Loss", DateTime.Today.ToString("M/d/yyyy"), (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                    //Provisions
                    new string[]{"IT Provision", (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                },
            };
            return res;
        }

        public List<object[]> TC_PU073_Data()
        {
            string pupilName = String.Format("PU073_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    //SenStage name
                    "In-school provision",
                    //SenNeeds
                    new string[]{"Mild Learning Difficulties", "Dangerous Fluid Loss", DateTime.Today.ToString("M/d/yyyy"), (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                    //Provisions
                    new string[]{"IT Provision", (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                },
            };
            return res;
        }

        public List<object[]> TC_PU078_Data()
        {
            string pupilName = String.Format("PU078_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday), "Not Known", "Elective Home Education"
                },
            };
            return res;
        }

        public List<object[]> TC_PU079_Data()
        {
            string pupilName = String.Format("PU079_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));

            var res = new List<Object[]>
            {           
                new object[] {
                    new string[]{pupilName, pupilName, 
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"}, 
                    new string[]{DateTime.ParseExact("12/12/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Not Known", "Elective Home Education"},
                    new string[]{DateTime.ParseExact("2/12/2014", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Subsidiary – Dual Registration", "Year 1"},
                    new string[]{DateTime.ParseExact("4/30/2014", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Not Known", "Elective Home Education"},
                    new string[]{DateTime.ParseExact("5/8/2014", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Main – Dual Registration", "Year 1"},
                    new string[]{DateTime.ParseExact("2/01/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Single Registration", 
                                 DateTime.ParseExact("2/02/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy")}
                },
            };
            return res;
        }

        public List<object[]> TC_PU081_Data()
        {
            string pupilName = String.Format("PU081_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{pupilName, pupilName, 
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    new string[]{"Family", "Family Documents", "document.txt"}, 
                    new string[]{"Welfare", "Welfare Documents", "document_1.txt"},
                    new string[]{"Consent", "Consent Documents", "document_2.txt"}
                },
            };
            return res;
        }

        public List<object[]> TC_PU087_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "3A"
                },
            };
            return res;
        }

        public List<object[]> TC_PU088_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "Year 1"
                },
            };
            return res;
        }

        public List<object[]> TC_PU089_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "1A", "Year 1"
                },
            };
            return res;
        }

        public List<object[]> TC_PU090_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "2A", "Year 2"
                },
            };
            return res;
        }
        

        #endregion


    }
}
