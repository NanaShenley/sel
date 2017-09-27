using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using TestSettings;
using SeSugar;
using SeSugar.Data;
using Selene.Support.Attributes;
using SeSugar.Automation;

namespace Pupil.PupilRecords.Tests
{
    public class AddNewPupilTests
    {
        string _pattern = "d/M/yyyy";
        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add new Pupil successfully
        /// </summary>
        /// 
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.Priority.Priority1, "TESTME" })]
        public void Create_New_Pupil_Record()
        {
            //Arrange
            string dateOfBirth = DateTime.ParseExact("01/01/1995", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
            var dateOfAdmission = DateTime.ParseExact("06/06/2010", _pattern, CultureInfo.InvariantCulture);
            string foreName = "addNewPuil" + Utilities.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string middleName = "aPupilMiddleName" + Utilities.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surName = "aPupilSurName" + Utilities.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string gender = "Male";
            var attendanceMode = Queries.GetFirstAttendanceMode();
            var yearGroup = Queries.GetFirstYearGroup();
            var classGroup = Queries.GetFirstPrimaryClass(dateOfAdmission: dateOfAdmission);
            var borderStatus = Queries.GetFirstBorderStatus();

            //Act
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Record
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilSearchTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.MiddleName = middleName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = dateOfBirth;

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission.ToString(_pattern);
            registrationDetailDialog.YearGroup = yearGroup.FullName;
            registrationDetailDialog.ClassName = classGroup.FullName;
            registrationDetailDialog.AttendanceMode = attendanceMode.Description;
            registrationDetailDialog.BorderStatus = borderStatus.Description;
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Assert
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message (after save) was not displayed");
            Assert.AreEqual(foreName, pupilRecord.LegalForeName, "LegalForName is not equal");
            Assert.AreEqual(surName, pupilRecord.LegalSurname, "LegalSurName is not equal");
            Assert.AreEqual(dateOfBirth, pupilRecord.DOB, "Date of birth is not equal");
            Assert.AreEqual(gender, pupilRecord.Gender, "Gender is not equal");
        }

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-027 : 
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.Priority.Priority2 })]
        public void Re_admit_a_pupil_who_left_in_the_past()
        {
            //Arrange
            string forename = "SelReadmitPupil" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surname = "SelReadmitPupil" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            DateTime dob = new DateTime(2011, 02, 02);
            var reasonForLeaving = Queries.GetFirstReasonForLeavingLookup();
            var enrolmentStatus = Queries.GetEnrolmentStatus("C");
            var yearGroup = Queries.GetFirstYearGroup();
            var learnerId = Guid.NewGuid();
            var readmitDate = DateTime.ParseExact("02/02/2016", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);

            var pupilRecord = this.BuildDataPackage();
            pupilRecord.
                AddLeaver(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05), dateOfLeaving: new DateTime(2014, 11, 05), reasonForLeaving: reasonForLeaving.Code);

            //Act
            using (new DataSetup(false, true, pupilRecord))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

                    // Search a pupil
                    var pupilRecordTriplet = new PupilSearchTriplet();
                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    pupilRecordTriplet.SearchCriteria.IsCurrent = false;
                    pupilRecordTriplet.SearchCriteria.IsFuture = false;
                    pupilRecordTriplet.SearchCriteria.IsLeaver = true;
                    var pupilResults = pupilRecordTriplet.SearchCriteria.Search();

                    var pupilSearchTile = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename));

                    var pupilRecordPage = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();

                    // Re-admit the former pupil
                    var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
                    addNewPupilDialog.Forename = forename;
                    addNewPupilDialog.SurName = surname;
                    addNewPupilDialog.Gender = "Male";
                    addNewPupilDialog.DateOfBirth =
                        DateTime.ParseExact("02/02/2011", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);

                    var didYouMeanDialog = addNewPupilDialog.ContinueReAdmit();
                    var formers = didYouMeanDialog.FormerPupils;
                    var formerPupil = formers.Rows
                        .FirstOrDefault(x => x.PupilName.Trim().Contains(String.Format("{0}, {1}", surname, forename)));
                    var registrationDetailsDialog = formerPupil.ClickReEnrolPupilLink();

                    // Enter data for registration details dialog
                    registrationDetailsDialog.DateOfAdmission = readmitDate; //Monday of this week
                    registrationDetailsDialog.EnrolmentStatus = enrolmentStatus.Description;
                    registrationDetailsDialog.YearGroup = yearGroup.FullName;

                    pupilRecordPage = registrationDetailsDialog.CreateRecord();
                    AutomationSugar.WaitForAjaxCompletion();

                    // Verify that there's a new record in 'Enrolment History' grid 
                    // for the re-admission with the correct 'Date of Admission' (Monday of this week)
                    // and correct 'Enrolment Status'.
                    pupilRecordPage.SelectRegistrationTab();
                    var enrolmentStatusHistories = pupilRecordPage.EnrolmentStatusHistoryTable;

                    //Assert
                    var results = new Dictionary<string, string>();
                    foreach (var x in enrolmentStatusHistories.Rows)
                    {
                        results.Add(x.StartDate, x.EnrolmentStatus);
                    }

                    var dateMatch = enrolmentStatusHistories.Rows.Any(x => x.EnrolmentStatus.Trim().Equals(enrolmentStatus.Description) && x.StartDate.Trim().Equals(readmitDate));

                    Assert.AreEqual(true, dateMatch,
                        "Either 'Date of Admission' is not the expected value of 02/02/2016 or or 'Enrolment Status' is not the expected 'Single Registration'");

                }
                finally
                {
                    //Teardown linked data 
                    PurgeLinkedData.DeleteYearGroupMembershipForPupil(learnerId);
                    PurgeLinkedData.DeleteLearnerEnrolmentStatusForPupil(learnerId);
                    PurgeLinkedData.DeleteLearnerEnrolmentForPupil(learnerId);
                    PurgeLinkedData.DeleteNcYearMembershipForPupil(learnerId);
                    PurgeLinkedData.DeleteAttendanceModelForLearner(learnerId);
                    PurgeLinkedData.DeleteBorderStatusForLearner(learnerId);
                }
            }
        }
    }
}
