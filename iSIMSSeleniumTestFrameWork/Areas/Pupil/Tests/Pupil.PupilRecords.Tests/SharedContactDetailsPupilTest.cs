using NUnit.Framework;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SeSugar.Data;
using System;
using TestSettings;

namespace Pupil.PupilRecords.Tests
{
    public class SharedContactDetailsPupilTest
    {
        #region Private Parameters

        private readonly int tenantID = SeSugar.Environment.Settings.TenantId;
        private readonly DateTime startDate = DateTime.Today.AddDays(-1);
        private readonly string telephoneAutomationID = "update_button";
        private readonly string emailAutomationID = "update_button";

        #endregion

        #region Tests

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "SharedContactDetails"
            })]
        public void Shared_Contact_Update_Matches_Pupil_Telephone()
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
                LoginAndNavigate();
                PupilRecordPage pupil = LoadPupil(pupilID);

                //Update Pupil Telephone
                pupil.SelectPhoneEmailTab();
                var gridRow = pupil.TelephoneNumberTable.Rows[0];
                gridRow.TelephoneNumber = newTelephoneNumber;

                //Matches
                var matchesDialog = new SharedContactDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.Matches.Rows[2].Selected = true;
                matchesDialog.ClickSave(telephoneAutomationID);

                //Pupil
                gridRow = pupil.TelephoneNumberTable.Rows[0];
                Assert.AreEqual(newTelephoneNumber, gridRow.TelephoneNumber);

                //Pupil Contact
                pupil.SelectFamilyHomeTab();
                pupil.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditContactDialog();
                var pctGridRow = contactDialog.LearnerContactTelephones.Rows[0];
                Assert.AreEqual(newTelephoneNumber, pctGridRow.TelephoneNumber);

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
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "SharedContactDetails"
            })]
        public void Shared_Contact_Update_Matches_PupilContact_Telephone()
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
                LoginAndNavigate();
                PupilRecordPage pupil = LoadPupil(pupilID);

                //Update Pupil Contact Telephone
                pupil.SelectFamilyHomeTab();
                pupil.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditContactDialog();
                var pctGridRow = contactDialog.LearnerContactTelephones.Rows[0];
                pctGridRow.TelephoneNumber = newTelephoneNumber;

                //Matches
                var matchesDialog = new SharedContactDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.Matches.Rows[2].Selected = true;
                matchesDialog.ClickSave(telephoneAutomationID);

                //Pupil
                pupil.SelectPhoneEmailTab();
                var gridRow = pupil.TelephoneNumberTable.Rows[0];
                Assert.AreEqual(newTelephoneNumber, gridRow.TelephoneNumber);

                //Pupil Contact
                pupil.ContactTable.Rows[0].ClickEdit();
                contactDialog = new EditContactDialog();
                pctGridRow = contactDialog.LearnerContactTelephones.Rows[0];
                Assert.AreEqual(newTelephoneNumber, pctGridRow.TelephoneNumber);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "SharedContactDetails"
            })]
        public void Shared_Contact_Update_Matches_Pupil_Email()
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
                LoginAndNavigate();
                PupilRecordPage pupil = LoadPupil(pupilID);

                //Update Pupil Email
                pupil.SelectPhoneEmailTab();
                var gridRow = pupil.EmailTable.Rows[0];
                gridRow.EmailAddress = newEmail;

                //Matches
                var matchesDialog = new SharedContactDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.Matches.Rows[2].Selected = true;
                matchesDialog.ClickSave(emailAutomationID);

                //Pupil
                gridRow = pupil.EmailTable.Rows[0];
                Assert.AreEqual(newEmail, gridRow.EmailAddress);

                //Pupil Contact
                pupil.SelectFamilyHomeTab();
                pupil.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditContactDialog();
                var pctGridRow = contactDialog.PupilContactEmails.Rows[0];
                Assert.AreEqual(newEmail, pctGridRow.EmailAddress);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "SharedContactDetails"
            })]
        public void Shared_Contact_Update_Matches_PupilContact_Email()
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
                LoginAndNavigate();
                PupilRecordPage pupil = LoadPupil(pupilID);

                //Update Pupil Contact Email
                pupil.SelectFamilyHomeTab();
                pupil.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditContactDialog();
                var pctGridRow = contactDialog.PupilContactEmails.Rows[0];
                pctGridRow.EmailAddress = newEmail;

                //Matches
                var matchesDialog = new SharedContactDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.Matches.Rows[2].Selected = true;
                matchesDialog.ClickSave(emailAutomationID);

                //Pupil
                pupil.SelectPhoneEmailTab();
                var gridRow = pupil.EmailTable.Rows[0];
                Assert.AreEqual(newEmail, gridRow.EmailAddress);

                //Pupil Contact
                pupil.SelectFamilyHomeTab();
                pupil.ContactTable.Rows[0].ClickEdit();
                contactDialog = new EditContactDialog();
                pctGridRow = contactDialog.PupilContactEmails.Rows[0];
                Assert.AreEqual(newEmail, pctGridRow.EmailAddress);

            }
        }

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
                ID = learnerEnrolmentID =  Guid.NewGuid(),
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

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "SharedContactDetails");
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
        }

        private static PupilRecordPage LoadPupil(Guid pupilId)
        {
            return PupilRecordPage.LoadPupilDetail(pupilId);
        }

        private static StaffRecordPage LoadStaff(Guid staffId)
        {
            return StaffRecordPage.LoadStaffDetail(staffId);
        }

        #endregion
    }
}