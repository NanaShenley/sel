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
    public class AddNewPupilTests
    {
        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add new Pupil successfully
        /// </summary>
        /// 
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.Priority.Priority1, "TESTME" }, DataProvider = "TC_PU01_Data")]
        public void TC_PU001_Verify_Create_New_Pupil_Record(string ForeName, string MiddleName, string SurName, string Gender, string DOB, string DateOfAdmission, string YearGroup, string Class, string AttendanceMode, string Border)
        {

            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", SurName, ForeName);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", SurName, ForeName)));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", SurName, ForeName);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", SurName, ForeName)));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();


            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = ForeName;
            addNewPupilDialog.MiddleName = MiddleName;
            addNewPupilDialog.SurName = SurName;
            addNewPupilDialog.Gender = Gender;
            addNewPupilDialog.DateOfBirth = DOB;

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = DateOfAdmission;
            registrationDetailDialog.YearGroup = YearGroup;
            registrationDetailDialog.ClassName = Class;
            registrationDetailDialog.AttendanceMode = AttendanceMode;
            registrationDetailDialog.BorderStatus = Border;
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreEqual(ForeName, pupilRecord.LegalForeName, "LegalForName is not equal");
            Assert.AreEqual(SurName, pupilRecord.LegalSurname, "LegalSurName is not equal");
            Assert.AreEqual(DOB, pupilRecord.DOB, "Date of birth is not equal");
            Assert.AreEqual(Gender, pupilRecord.Gender, "Gender is not equal");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", SurName, ForeName);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", SurName, ForeName)));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", SurName, ForeName);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", SurName, ForeName)));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }


        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-027 : 
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU027_Data")]
        public void TC_PU027_Exercise_ability_to_re_admit_a_pupil_who_left_in_the_past(
            string forename, string surname, string gender, string dob,
            string dateOfAdmission, string yearGroup, string dateOfLeaving,
            string reasonForLeaving, string enrolmentStatus, string reAdmitDate)
        {
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);


            #region Pre-Condition: Create a new pupil for test

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = forename;
            addNewPupilDialog.SurName = surname;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = dob;

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecordPage = PupilRecordPage.Create();
            pupilRecordPage.SavePupil();

            #endregion

            #region Pre-Condition: Leaving the pupil

            var pupilLeavingDetailsPage = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");
            pupilLeavingDetailsPage.DOL = dateOfLeaving;
            pupilLeavingDetailsPage.ReasonForLeaving = reasonForLeaving;
            confirmRequiredDialog = pupilLeavingDetailsPage.ClickSave();
            confirmRequiredDialog.ClickOk();
            var leaverBackgroundProcessSubmitDialog = new LeaverBackgroundProcessSubmitDialog();
            leaverBackgroundProcessSubmitDialog.ClickOk();

            #endregion

            #region Steps

            // Search a pupil
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
            pupilRecordTriplet.SearchCriteria.IsCurrent = false;
            pupilRecordTriplet.SearchCriteria.IsFuture = false;
            pupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename)).Click<PupilRecordPage>();

            // Re-admit the former pupil
            addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = forename;
            addNewPupilDialog.SurName = surname;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = dob;
            var didYouMeanDialog = addNewPupilDialog.ContinueReAdmit();
            var formers = didYouMeanDialog.FormerPupils;
            var formerPupil = formers.Rows
                .FirstOrDefault(x => x.PupilName.Trim().Contains(String.Format("{0}, {1}", surname, forename))
                && x.DOB.Equals(dob));
            var registrationDetailsDialog = formerPupil.ClickReEnrolPupilLink();

            // Enter data for registration details dialog
            registrationDetailsDialog.DateOfAdmission = reAdmitDate; //Monday of this week
            registrationDetailsDialog.EnrolmentStatus = enrolmentStatus;
            registrationDetailsDialog.YearGroup = yearGroup;
            pupilRecordPage = registrationDetailsDialog.CreateRecord();

            // Verify that there's a new record in 'Enrolment History' grid 
            // for the re-admission with the correct 'Date of Admission' (Monday of this week)
            // and correct 'Enrolment Status'.
            pupilRecordPage.SelectRegistrationTab();
            var enrolmentStatusHistories = pupilRecordPage.EnrolmentStatusHistoryTable;
            Assert.AreEqual(true, enrolmentStatusHistories.Rows
                .Any(x => x.EnrolmentStatus.Trim().Equals("Single Registration")
                    && x.StartDate.Trim().Equals(reAdmitDate)), "'Date of Admission' or 'Enrolment Status' value is incorrect.");

            #endregion

            #region Post-Condition: Delete the pupil record

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
            deletePupilRecordTriplet.SearchCriteria.IsCurrent = true;
            deletePupilRecordTriplet.SearchCriteria.IsFuture = true;
            deletePupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));
            var deletePupilRecordPage = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecordPage.Delete();

            #endregion
        }


        #region DATA

        public List<object[]> TC_PU01_Data()
        {
            string pattern = "d/M/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();

            string randomCharacter = SeleniumHelper.GenerateRandomString(6);

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

        #endregion


    }
}
