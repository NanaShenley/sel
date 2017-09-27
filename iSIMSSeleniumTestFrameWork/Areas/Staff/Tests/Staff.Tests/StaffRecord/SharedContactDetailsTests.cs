using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using SeSugar.Automation;
using SeSugar.Data;
using Selene.Support.Attributes;
using Staff.POM.Components.Staff.Dialogs;

namespace Staff.Tests.StaffRecord
{
    [TestClass]
    public class SharedContactDetailsTests
    {
        #region MS Unit Testing support

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Init()
        {
            TestRunner.VSSeleniumTest.Init(this, TestContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.VSSeleniumTest.Cleanup(TestContext);
        }

        #endregion

        #region Private Parameters

        private readonly int tenantID = SeSugar.Environment.Settings.TenantId;
        private readonly DateTime startDate = DateTime.Today.AddDays(-1);
        private readonly string telephoneAutomationID = "update_button";
        private readonly string emailAutomationID = "update_button";

        #endregion

        #region Tests

        [TestMethod]
        [ChromeUiTest("SharedContactDetails", "P1", "StaffTelephone")]
        public void Shared_Contact_Update_Matches_Staff_Telephone()
        {
            //Arrange
            #region IDs

            Guid locationTypeID;

            Guid staffID, staffTelephoneID;
            Guid staffContactID, staffContactTelephoneID, staffContactRelationshipID;

            Guid pupilID, pupilEnrolmentID, pupilEnrolmentStatusID, pupilTelephoneID;
            Guid pupilContactID, pupilContactTelephoneID, pupilContactRelationshipID;

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

            string oldTelephoneNumber = GenerateNumber();
            string newTelephoneNumber = oldTelephoneNumber + "new";

            #endregion

            #region Data

            DataPackage package = new DataPackage();
            package.AddData("LocationType", DataPackageHelper.GenerateLocationType(out locationTypeID, code: code, description: description));

            package.AddData("Staff", DataPackageHelper.GenerateStaff(out staffID, surname: staffSurname, forename: staffForename));
            package.AddData("StaffTelephone", DataPackageHelper.GenerateStaffTelephone(out staffTelephoneID, telephoneNumber: oldTelephoneNumber, staffID: staffID, locationTypeID: locationTypeID));

            package.AddData("StaffContact", DataPackageHelper.GenerateStaffContact(out staffContactID, surname: staffContactSurname));
            package.AddData("StaffContactTelephone", DataPackageHelper.GenerateStaffContactTelephone(out staffContactTelephoneID, telephoneNumber: oldTelephoneNumber, staffContactID: staffContactID, locationTypeID: locationTypeID));
            package.AddData("StaffContactRelationship", DataPackageHelper.GenerateStaffContactRelationship(out staffContactRelationshipID, staffID: staffID, staffContactID: staffContactID));

            package.AddData("Learner", DataPackageHelper.GeneratePupil(out pupilID, surname: pupilSurname, forename: pupilForename, dateOfBirth: startDate));
            package.AddData("LearnerEnrolment", DataPackageHelper.GeneratePupilEnrolment(out pupilEnrolmentID, learnerID: pupilID, doa: startDate));
            package.AddData("LearnerEnrolmentStatus", DataPackageHelper.GeneratePupilEnrolmentStatus(out pupilEnrolmentStatusID, learnerEnrolmentID: pupilEnrolmentID, doa: startDate));
            package.AddData("LearnerTelephone", DataPackageHelper.GeneratePupilTelephone(out pupilTelephoneID, telephoneNumber: oldTelephoneNumber, pupilID: pupilID, locationTypeID: locationTypeID));

            package.AddData("LearnerContact", DataPackageHelper.GeneratePupilContact(out pupilContactID, surname: pupilContactSurname));
            package.AddData("LearnerContactTelephone", DataPackageHelper.GeneratePupilContactTelephone(out pupilContactTelephoneID, telephoneNumber: oldTelephoneNumber, pupilContactID: pupilContactID, locationTypeID: locationTypeID));
            package.AddData("LearnerContactRelationship", DataPackageHelper.GeneratePupilContactRelationship(out pupilContactRelationshipID, pupilID: pupilID, pupilContactID: pupilContactID));

            #endregion

            //Act
            using (new DataSetup(package))
            {
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffID);

                //Update Staff Telephone
                staff.SelectPhoneEmailTab();
                var gridRow = staff.TelephoneNumberTable.Rows[0];
                gridRow.TelephoneNumber = newTelephoneNumber;

                //Matches
                var matchesDialog = new SharedContactDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.Matches.Rows[2].Selected = true;
                matchesDialog.ClickSave(telephoneAutomationID);

                //Staff
                gridRow = staff.TelephoneNumberTable.Rows[0];
                Assert.AreEqual(newTelephoneNumber, gridRow.TelephoneNumber);

                //Staff Contact
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditStaffContactDialog();
                var sctGridRow = contactDialog.StaffContactTelephones.Rows[0];
                Assert.AreEqual(newTelephoneNumber, sctGridRow.TelephoneNumber);

                //Pupil
                PupilRecordPage pupil = LoadPupil(pupilID);
                pupil.SelectPhoneEmailTab();
                var pGridRow = pupil.TelephoneNumberTable.Rows[0];
                Assert.AreEqual(newTelephoneNumber, pGridRow.TelephoneNumber);

                //Pupil Contact
                pupil.SelectFamilyHomeTab();
                pupil.ContactTable.Rows[0].ClickEdit();
                var pupilContactDialog = new EditContactDialog();
                var pcGridRow = pupilContactDialog.PupilContactTelephones.Rows[0];
                Assert.AreEqual(newTelephoneNumber, pcGridRow.TelephoneNumber);
            }
        }

        [TestMethod]
        [ChromeUiTest("SharedContactDetails", "P1", "StaffContactTelephone")]
        public void Shared_Contact_Update_Matches_StaffContact_Telephone()
        {
            //Arrange
            #region IDs

            Guid locationTypeID;

            Guid staffID, staffTelephoneID;
            Guid staffContactID, staffContactTelephoneID, staffContactRelationshipID;

            Guid pupilID, pupilEnrolmentID, pupilEnrolmentStatusID, pupilTelephoneID;
            Guid pupilContactID, pupilContactTelephoneID, pupilContactRelationshipID;

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

            string oldTelephoneNumber = GenerateNumber();
            string newTelephoneNumber = oldTelephoneNumber + "new";

            #endregion

            #region Data

            DataPackage package = new DataPackage();
            package.AddData("LocationType", DataPackageHelper.GenerateLocationType(out locationTypeID, code: code, description: description));

            package.AddData("Staff", DataPackageHelper.GenerateStaff(out staffID, surname: staffSurname, forename: staffForename));
            package.AddData("StaffTelephone", DataPackageHelper.GenerateStaffTelephone(out staffTelephoneID, telephoneNumber: oldTelephoneNumber, staffID: staffID, locationTypeID: locationTypeID));

            package.AddData("StaffContact", DataPackageHelper.GenerateStaffContact(out staffContactID, surname: staffContactSurname));
            package.AddData("StaffContactTelephone", DataPackageHelper.GenerateStaffContactTelephone(out staffContactTelephoneID, telephoneNumber: oldTelephoneNumber, staffContactID: staffContactID, locationTypeID: locationTypeID));
            package.AddData("StaffContactRelationship", DataPackageHelper.GenerateStaffContactRelationship(out staffContactRelationshipID, staffID: staffID, staffContactID: staffContactID));

            package.AddData("Learner", DataPackageHelper.GeneratePupil(out pupilID, surname: pupilSurname, forename: pupilForename, dateOfBirth: startDate));
            package.AddData("LearnerEnrolment", DataPackageHelper.GeneratePupilEnrolment(out pupilEnrolmentID, learnerID: pupilID, doa: startDate));
            package.AddData("LearnerEnrolmentStatus", DataPackageHelper.GeneratePupilEnrolmentStatus(out pupilEnrolmentStatusID, learnerEnrolmentID: pupilEnrolmentID, doa: startDate));
            package.AddData("LearnerTelephone", DataPackageHelper.GeneratePupilTelephone(out pupilTelephoneID, telephoneNumber: oldTelephoneNumber, pupilID: pupilID, locationTypeID: locationTypeID));

            package.AddData("LearnerContact", DataPackageHelper.GeneratePupilContact(out pupilContactID, surname: pupilContactSurname));
            package.AddData("LearnerContactTelephone", DataPackageHelper.GeneratePupilContactTelephone(out pupilContactTelephoneID, telephoneNumber: oldTelephoneNumber, pupilContactID: pupilContactID, locationTypeID: locationTypeID));
            package.AddData("LearnerContactRelationship", DataPackageHelper.GeneratePupilContactRelationship(out pupilContactRelationshipID, pupilID: pupilID, pupilContactID: pupilContactID));

            #endregion

            //Act
            using (new DataSetup(package))
            {
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffID);

                //Update Staff Contact Telephone
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditStaffContactDialog();
                var sctGridRow = contactDialog.StaffContactTelephones.Rows[0];
                sctGridRow.TelephoneNumber = newTelephoneNumber;

                //Matches
                var matchesDialog = new SharedContactDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.Matches.Rows[2].Selected = true;
                matchesDialog.ClickSave(telephoneAutomationID);

                //Staff
                staff.SelectPhoneEmailTab();
                var gridRow = staff.TelephoneNumberTable.Rows[0];
                Assert.AreEqual(newTelephoneNumber, gridRow.TelephoneNumber);

                //Staff Contact
                staff.ContactTable.Rows[0].ClickEdit();
                contactDialog = new EditStaffContactDialog();
                sctGridRow = contactDialog.StaffContactTelephones.Rows[0];
                Assert.AreEqual(newTelephoneNumber, sctGridRow.TelephoneNumber);

                //Pupil
                PupilRecordPage pupil = LoadPupil(pupilID);
                pupil.SelectPhoneEmailTab();
                var pGridRow = pupil.TelephoneNumberTable.Rows[0];
                Assert.AreEqual(newTelephoneNumber, pGridRow.TelephoneNumber);

                //Pupil Contact
                pupil.SelectFamilyHomeTab();
                pupil.ContactTable.Rows[0].ClickEdit();
                var pupilContactDialog = new EditContactDialog();
                var pcGridRow = pupilContactDialog.PupilContactTelephones.Rows[0];
                Assert.AreEqual(newTelephoneNumber, pcGridRow.TelephoneNumber);
            }
        }

        [TestMethod]
        [ChromeUiTest("SharedContactDetails", "P1", "StaffEmail")]
        public void Shared_Contact_Update_Matches_Staff_Email()
        {
            //Arrange

            #region IDs

            Guid locationTypeID;

            Guid staffID, staffTelephoneID;
            Guid staffContactID, staffContactTelephoneID, staffContactRelationshipID;

            Guid pupilID, pupilEnrolmentID, pupilEnrolmentStatusID, pupilTelephoneID;
            Guid pupilContactID, pupilContactTelephoneID, pupilContactRelationshipID;

            #endregion

            #region Values

            string staffForename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string staffSurname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string staffContactSurname = CoreQueries.GetColumnUniqueString("StaffContact", "Surname", 10, tenantID);

            string pupilForename = CoreQueries.GetColumnUniqueString("Learner", "LegalForename", 10, tenantID);
            string pupilSurname = CoreQueries.GetColumnUniqueString("Learner", "LegalSurname", 10, tenantID);
            string pupilContactSurname = CoreQueries.GetColumnUniqueString("LearnerContact", "Surname", 10, tenantID);

            string code = CoreQueries.GetColumnUniqueString("EmailLocationType", "Code", 10, tenantID);
            string description = CoreQueries.GetColumnUniqueString("EmailLocationType", "Description", 10, tenantID);

            var email = SeleniumHelper.GenerateRandomString(10);
            string oldEmail = email + "@capita.co.uk";
            string newEmail = email + "new@capita.co.uk";

            #endregion

            #region Data

            DataPackage package = new DataPackage();
            package.AddData("EmailLocationType", DataPackageHelper.GenerateLocationType(out locationTypeID, code: code, description: description));

            package.AddData("Staff", DataPackageHelper.GenerateStaff(out staffID, surname: staffSurname, forename: staffForename));
            package.AddData("StaffEmail", DataPackageHelper.GenerateStaffEmail(out staffTelephoneID, emailAddress: oldEmail, staffID: staffID, emailLocationTypeID: locationTypeID));

            package.AddData("StaffContact", DataPackageHelper.GenerateStaffContact(out staffContactID, surname: staffContactSurname));
            package.AddData("StaffContactEmail", DataPackageHelper.GenerateStaffContactEmail(out staffContactTelephoneID, emailAddress: oldEmail, staffContactID: staffContactID, emailLocationTypeID: locationTypeID));
            package.AddData("StaffContactRelationship", DataPackageHelper.GenerateStaffContactRelationship(out staffContactRelationshipID, staffID: staffID, staffContactID: staffContactID));

            package.AddData("Learner", DataPackageHelper.GeneratePupil(out pupilID, surname: pupilSurname, forename: pupilForename, dateOfBirth: startDate));
            package.AddData("LearnerEnrolment", DataPackageHelper.GeneratePupilEnrolment(out pupilEnrolmentID, learnerID: pupilID, doa: startDate));
            package.AddData("LearnerEnrolmentStatus", DataPackageHelper.GeneratePupilEnrolmentStatus(out pupilEnrolmentStatusID, learnerEnrolmentID: pupilEnrolmentID, doa: startDate));
            package.AddData("LearnerEmail", DataPackageHelper.GeneratePupilEmail(out pupilTelephoneID, emailAddress: oldEmail, pupilID: pupilID, emailLocationTypeID: locationTypeID));

            package.AddData("LearnerContact", DataPackageHelper.GeneratePupilContact(out pupilContactID, surname: pupilContactSurname));
            package.AddData("LearnerContactEmail", DataPackageHelper.GeneratePupilContactEmail(out pupilContactTelephoneID, emailAddress: oldEmail, pupilContactID: pupilContactID, emailLocationTypeID: locationTypeID));
            package.AddData("LearnerContactRelationship", DataPackageHelper.GeneratePupilContactRelationship(out pupilContactRelationshipID, pupilID: pupilID, pupilContactID: pupilContactID));

            #endregion

            //Act
            using (new DataSetup(package))
            {
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffID);

                //Update Staff Email
                staff.SelectPhoneEmailTab();
                var gridRow = staff.EmailTable.Rows[0];
                gridRow.EmailAddress = newEmail;

                //Matches
                var matchesDialog = new SharedContactDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.Matches.Rows[2].Selected = true;
                matchesDialog.ClickSave(emailAutomationID);

                //Staff
                gridRow = staff.EmailTable.Rows[0];
                Assert.AreEqual(newEmail, gridRow.EmailAddress);

                //Staff Contact
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditStaffContactDialog();
                var sctGridRow = contactDialog.StaffContactEmails.Rows[0];
                Assert.AreEqual(newEmail, sctGridRow.EmailAddress);

                //Pupil
                PupilRecordPage pupil = LoadPupil(pupilID);
                pupil.SelectPhoneEmailTab();
                var pGridRow = pupil.EmailTable.Rows[0];
                Assert.AreEqual(newEmail, pGridRow.EmailAddress);

                //Pupil Contact
                pupil.SelectFamilyHomeTab();
                pupil.ContactTable.Rows[0].ClickEdit();
                var pupilContactDialog = new EditContactDialog();
                var pcGridRow = pupilContactDialog.PupilContactEmails.Rows[0];
                Assert.AreEqual(newEmail, pcGridRow.EmailAddress);
            }
        }

        [TestMethod]
        [ChromeUiTest("SharedContactDetails", "P1", "StaffContactEmail")]
        public void Shared_Contact_Update_Matches_StaffContact_Email()
        {
            //Arrange

            #region IDs

            Guid locationTypeID;

            Guid staffID, staffTelephoneID;
            Guid staffContactID, staffContactTelephoneID, staffContactRelationshipID;

            Guid pupilID, pupilEnrolmentID, pupilEnrolmentStatusID, pupilTelephoneID;
            Guid pupilContactID, pupilContactTelephoneID, pupilContactRelationshipID;

            #endregion

            #region Values

            string staffForename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string staffSurname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string staffContactSurname = CoreQueries.GetColumnUniqueString("StaffContact", "Surname", 10, tenantID);

            string pupilForename = CoreQueries.GetColumnUniqueString("Learner", "LegalForename", 10, tenantID);
            string pupilSurname = CoreQueries.GetColumnUniqueString("Learner", "LegalSurname", 10, tenantID);
            string pupilContactSurname = CoreQueries.GetColumnUniqueString("LearnerContact", "Surname", 10, tenantID);

            string code = CoreQueries.GetColumnUniqueString("EmailLocationType", "Code", 10, tenantID);
            string description = CoreQueries.GetColumnUniqueString("EmailLocationType", "Description", 10, tenantID);

            var email = SeleniumHelper.GenerateRandomString(10);
            string oldEmail = email + "@capita.co.uk";
            string newEmail = email + "new@capita.co.uk";

            #endregion

            #region Data

            DataPackage package = new DataPackage();
            package.AddData("EmailLocationType", DataPackageHelper.GenerateLocationType(out locationTypeID, code: code, description: description));

            package.AddData("Staff", DataPackageHelper.GenerateStaff(out staffID, surname: staffSurname, forename: staffForename));
            package.AddData("StaffEmail", DataPackageHelper.GenerateStaffEmail(out staffTelephoneID, emailAddress: oldEmail, staffID: staffID, emailLocationTypeID: locationTypeID));

            package.AddData("StaffContact", DataPackageHelper.GenerateStaffContact(out staffContactID, surname: staffContactSurname));
            package.AddData("StaffContactEmail", DataPackageHelper.GenerateStaffContactEmail(out staffContactTelephoneID, emailAddress: oldEmail, staffContactID: staffContactID, emailLocationTypeID: locationTypeID));
            package.AddData("StaffContactRelationship", DataPackageHelper.GenerateStaffContactRelationship(out staffContactRelationshipID, staffID: staffID, staffContactID: staffContactID));

            package.AddData("Learner", DataPackageHelper.GeneratePupil(out pupilID, surname: pupilSurname, forename: pupilForename, dateOfBirth: startDate));
            package.AddData("LearnerEnrolment", DataPackageHelper.GeneratePupilEnrolment(out pupilEnrolmentID, learnerID: pupilID, doa: startDate));
            package.AddData("LearnerEnrolmentStatus", DataPackageHelper.GeneratePupilEnrolmentStatus(out pupilEnrolmentStatusID, learnerEnrolmentID: pupilEnrolmentID, doa: startDate));
            package.AddData("LearnerEmail", DataPackageHelper.GeneratePupilEmail(out pupilTelephoneID, emailAddress: oldEmail, pupilID: pupilID, emailLocationTypeID: locationTypeID));

            package.AddData("LearnerContact", DataPackageHelper.GeneratePupilContact(out pupilContactID, surname: pupilContactSurname));
            package.AddData("LearnerContactEmail", DataPackageHelper.GeneratePupilContactEmail(out pupilContactTelephoneID, emailAddress: oldEmail, pupilContactID: pupilContactID, emailLocationTypeID: locationTypeID));
            package.AddData("LearnerContactRelationship", DataPackageHelper.GeneratePupilContactRelationship(out pupilContactRelationshipID, pupilID: pupilID, pupilContactID: pupilContactID));

            #endregion

            //Act
            using (new DataSetup(package))
            {
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffID);

                //Update Staff Contact Email
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditStaffContactDialog();
                var sctGridRow = contactDialog.StaffContactEmails.Rows[0];
                sctGridRow.EmailAddress = newEmail;

                //Matches
                var matchesDialog = new SharedContactDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.Matches.Rows[2].Selected = true;
                matchesDialog.ClickSave(emailAutomationID);

                //Staff
                staff.SelectPhoneEmailTab();
                var gridRow = staff.EmailTable.Rows[0];
                Assert.AreEqual(newEmail, gridRow.EmailAddress);

                //StaffContact
                staff.ContactTable.Rows[0].ClickEdit();
                contactDialog = new EditStaffContactDialog();
                sctGridRow = contactDialog.StaffContactEmails.Rows[0];
                Assert.AreEqual(newEmail, sctGridRow.EmailAddress);

                //Pupil
                PupilRecordPage pupil = LoadPupil(pupilID);
                pupil.SelectPhoneEmailTab();
                var pGridRow = pupil.EmailTable.Rows[0];
                Assert.AreEqual(newEmail, pGridRow.EmailAddress);

                //Pupil Contact
                pupil.SelectFamilyHomeTab();
                pupil.ContactTable.Rows[0].ClickEdit();
                var pupilContactDialog = new EditContactDialog();
                var pcGridRow = pupilContactDialog.PupilContactEmails.Rows[0];
                Assert.AreEqual(newEmail, pcGridRow.EmailAddress);
            }
        }

        #endregion

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, "SharedContactDetails");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
        }

        private static StaffRecordPage LoadStaff(Guid staffId)
        {
            return StaffRecordPage.LoadStaffDetail(staffId);
        }

        private static PupilRecordPage LoadPupil(Guid pupilId)
        {
            return PupilRecordPage.LoadPupilDetail(pupilId);
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

        #endregion
    }
}
