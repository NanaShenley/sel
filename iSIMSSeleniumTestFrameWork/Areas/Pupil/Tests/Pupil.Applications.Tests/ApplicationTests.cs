using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using POM.Components.Admission;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using SeSugar;
using SeSugar.Data;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;

// ReSharper disable once CheckNamespace
namespace Pupil.Admissions.Tests
{
    public class ApplicationTests
    {
        /// <summary/>
        /// Author: Huy.Vo
        /// Des: Exercise ability to add 1st new 'Application' to an admission group, checking defaults and validation.
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.Application.Page, PupilTestGroups.Application.AddNew, PupilTestGroups.Priority.Priority1, "A_ANA" },
            DataProvider = "TC_AD07_Data")]
        public void Add_New_Applicant(
            string sureName, string middleName, string foreName, string gender, string dateOfBirth,
            string enrolmentStatus, string Class, string attendanceMode, string boarderStatus)
        {

            #region PRE-CONDITIONS:

            var schoolIntakeId = Guid.NewGuid();
            const string admissionTerm = "Spring";
            const int numberOfPlannedAdmission = 31;
            const int capacity = 10;
            var admissionYear = String.Format("{0}/{1}", (DateTime.Now.Year), (DateTime.Now.Year + 1));
            var dateOfAdmission = new DateTime(DateTime.Today.Year, 8, 8);
            var admissionGroupName = Utilities.GenerateRandomString(10, "TC_AG01_Data");
            var yearGroup = Queries.GetFirstYearGroup();

            var dataPackage = this.BuildDataPackage();
            dataPackage.AddSchoolIntake(
                schoolIntakeId,
                admissionYear,
                admissionTerm,
                yearGroup,
                numberOfPlannedAdmission,
                admissionGroupName,
                dateOfAdmission,
                capacity);

            // Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                // Login as Admission 
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

                // Navigate to Application page
                AutomationSugar.NavigateMenu("Tasks", "Admissions", "Applications");

                Wait.WaitForAjaxReady();

                var applicantTriplet = new ApplicationTriplet();

                // Search to change to Admitted if existing Applicant
                applicantTriplet.SearchCriteria.SetStatus("Applied", true);
                applicantTriplet.SearchCriteria.SetStatus("Offered", true);
                applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
                applicantTriplet.SearchCriteria.SearchByName = sureName;
                var searchResults = applicantTriplet.SearchCriteria.Search();
                var applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName + ", " + foreName));
                applicantTriplet.ChangeToAdmit(applicantTile);

                #endregion

                #region STEPS:

                // Create new an Applicant
                applicantTriplet = new ApplicationTriplet();
                var addNewApplicationDialog = applicantTriplet.AddNewApplicationDialog();
                addNewApplicationDialog.ForeName = foreName;
                addNewApplicationDialog.MiddleName = middleName;
                addNewApplicationDialog.SureName = sureName;
                addNewApplicationDialog.Gender = gender;
                addNewApplicationDialog.DateofBirth = dateOfBirth;
                addNewApplicationDialog.Continue();

                // Navigate to Registration Details dialog
                var registrationDetailsDialog = new RegistrationDetailsDialog();
                registrationDetailsDialog.AdmissionGroup = admissionGroupName;
                registrationDetailsDialog.EnrolmentStatus = enrolmentStatus;
                registrationDetailsDialog.Class = Class;
                registrationDetailsDialog.AttendanceMode = attendanceMode;
                registrationDetailsDialog.BoarderStatus = boarderStatus;
                registrationDetailsDialog.AddRecord();

                // Search Applicant that has just created
                applicantTriplet.SearchCriteria.SearchByName = sureName;
                searchResults = applicantTriplet.SearchCriteria.Search();
                applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName + ", " + foreName));

                try
                {
                    // Verify that Applicant was created
                    Assert.IsNotNull(applicantTile, "Application record not created");

                    var applicationPage = applicantTile.Click<ApplicationPage>();

                    // Verify that Applicant was created successfully
                    Assert.AreEqual(foreName, applicationPage.LegalForeName, "Legal ForName is not equal");
                    Assert.AreEqual(sureName, applicationPage.LegalSureName, "Legal SurName is not equal");
                    Assert.AreEqual(gender, applicationPage.Gender, "Gender is not equal");
                    Assert.AreEqual(dateOfBirth, applicationPage.DateOfBirth, "Date of Birth is not equal");
                    Assert.AreEqual(dateOfBirth.GetMonthsAndYears(), applicationPage.Age, "Ages is not equal");
                    Assert.AreEqual("Applied", applicationPage.ApplicationStatus, "Application Status is not equal");
                    Assert.AreEqual(admissionGroupName, applicationPage.AdmissionGroup, "Admission Group is not equal");
                    Assert.AreEqual("", applicationPage.AdmissionNumber, "Admission Number is not equal");
                    Assert.AreEqual(true, applicationPage.VerifyDisable(), "Age and Age On Entry are not disabled");
                    
                }
                finally
                {
                    // Tear down linked records before clean up
                    var learnerId = Queries.GetLearnerId(sureName, foreName, dateOfBirth);
                    var applicationId = Queries.GetApplicationId(learnerId);
                    PurgeLinkedData.DeleteApplicationStatusLog(applicationId);
                    PurgeLinkedData.DeleteApplication(applicationId);
                    PurgeLinkedData.DeleteLearnerEnrolmentStatusForPupil(learnerId);
                    PurgeLinkedData.DeleteLearnerEnrolmentForPupil(learnerId);
                    PurgeLinkedData.DeleteNcYearMembershipForPupil(learnerId);
                    PurgeLinkedData.DeleteYearGroupMembershipForPupil(learnerId);
                    PurgeLinkedData.DeleteAttendanceModelForLearner(learnerId);
                    PurgeLinkedData.DeleteBorderStatusForLearner(learnerId);
                    PurgeLinkedData.DeletePrimaryClassMembershipForLearner(learnerId);
                    PurgeLinkedData.DeletePupil(foreName, sureName);
                }
            }
            #endregion
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Des: Confirm the correct 'Applicants' are output in a report based on their current 'Application Status'.
        /// </summary>
        ///
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.Application.Page, PupilTestGroups.Application.ViewReport, PupilTestGroups.Priority.Priority1, "A_AA" },
            DataProvider = "TC_AD08_Data")]
        public void Admit_Applicant(
            string sureName, string middleName, string foreName, string gender, string dateOfBirth, string enrolmentStatus, string Class, string attendanceMode, string boarderStatus
            )
        {
           var schoolIntakeId = Guid.NewGuid();
            var admissionGroupId = Guid.NewGuid();
            const string admissionTerm = "Spring";
            const int numberOfPlannedAdmission = 31;
            const int capacity = 10;
            var admissionYear = string.Format("{0}/{1}", (DateTime.Now.Year), (DateTime.Now.Year + 1));
            var dateOfAdmission = new DateTime(DateTime.Today.Year, 8, 8);
            var admissionGroupName = Utilities.GenerateRandomString(10, "TC_AG02_Data");
            var yearGroup = Queries.GetFirstYearGroup();

            var dataPackage = this.BuildDataPackage();

            dataPackage.AddSchoolIntake(
                schoolIntakeId,
                admissionYear,
                admissionTerm,
                yearGroup,
                numberOfPlannedAdmission,
                admissionGroupName,
                dateOfAdmission,
                capacity,
                admissionGroupId);

            var pupilId = Guid.NewGuid();
            var learnerEnrolmentId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();
            var dob = new DateTime(2010,01,01);
            var pupilSurname = Utilities.GenerateRandomString(10, "TestPupil1_Surname" + Thread.CurrentThread.ManagedThreadId);
            var pupilForename = Utilities.GenerateRandomString(10, "TestPupil1_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, dob,
                new DateTime(2015, 09, 01), enrolStatus: "F", uniqueLearnerEnrolmentId: learnerEnrolmentId);
            dataPackage.AddBasicApplicant(applicantId, pupilId, learnerEnrolmentId, admissionGroupId, new DateTime(2015, 09, 01));

            // Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                // Login as Admission 
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

                // Navigate to Application page
                AutomationSugar.NavigateMenu("Tasks", "Admissions", "Applications");

                Wait.WaitForAjaxReady();

                var applicantTriplet = new ApplicationTriplet();

                // Find the applicant
                applicantTriplet.SearchCriteria.SetStatus("Applied", true);
                applicantTriplet.SearchCriteria.SetStatus("Offered", true);
                applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
                applicantTriplet.SearchCriteria.SearchByName = pupilSurname;
                var searchResults = applicantTriplet.SearchCriteria.Search();
                var applicantTile =
                    searchResults.SingleOrDefault(t => t.Code.Equals(pupilSurname + ", " + pupilForename));

                try
                {
                    //make sure applicant is found
                    Assert.IsNotNull(applicantTile, "Application record not created");

                    //change status to admitted
                    applicantTriplet.ChangeToAdmit(applicantTile);

                    applicantTriplet.SearchCriteria.DisplayIfHidden();

                    // Verify that Applicant was admitted
                    applicantTriplet.SearchCriteria.SetStatus("Applied", false);
                    applicantTriplet.SearchCriteria.SetStatus("Offered", false);
                    applicantTriplet.SearchCriteria.SetStatus("Accepted", false);
                    applicantTriplet.SearchCriteria.SetStatus("Reserved", false);
                    applicantTriplet.SearchCriteria.SetStatus("Admitted", true);

                    applicantTriplet.SearchCriteria.SearchByName = pupilSurname;
                    searchResults = applicantTriplet.SearchCriteria.Search();
                    applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(pupilSurname + ", " + pupilForename));

                    Assert.IsNotNull(applicantTile, "Application record not admitted");

                    var applicationPage = applicantTile.Click<ApplicationPage>();

                    Assert.AreEqual("Admitted", applicationPage.ApplicationStatus, "Application Status is not equal");

                }
                finally
                {
                    // Tear down linked records before clean up
                    var learnerId = Queries.GetLearnerId(pupilSurname, pupilForename, dob.ToLongDateString());
                    var applicationId = Queries.GetApplicationId(learnerId);
                    PurgeLinkedData.DeleteApplicationStatusLog(applicationId);
                    PurgeLinkedData.DeleteApplication(applicationId);
                    PurgeLinkedData.DeleteLearnerEnrolmentStatusForPupil(learnerId);
                    PurgeLinkedData.DeleteLearnerEnrolmentForPupil(learnerId);
                    PurgeLinkedData.DeleteNcYearMembershipForPupil(learnerId);
                    PurgeLinkedData.DeleteYearGroupMembershipForPupil(learnerId);
                    PurgeLinkedData.DeleteAttendanceModelForLearner(learnerId);
                    PurgeLinkedData.DeleteBorderStatusForLearner(learnerId);
                    PurgeLinkedData.DeletePrimaryClassMembershipForLearner(learnerId);
                    PurgeLinkedData.DeletePupil(foreName, sureName);
                }
            }
        }

        #region DATA

        public List<object[]> TC_AD07_Data()
        {
            var pattern = "M/d/yyyy";
            var sureName = Utilities.GenerateRandomString(10, "SureNameTC07");
            var middleName = Utilities.GenerateRandomString(10, "MiddleNameTC07");
            var foreName = Utilities.GenerateRandomString(10, "ForeNameTC07");
            var dateOfBirth = DateTime.ParseExact("07/07/1982", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var gender = "Male";
            var enrolmentStatus = Queries.GetEnrolmentStatus();
            var primaryClass = Queries.GetFirstPrimaryClass();
            var attendanceMode = Queries.GetFirstAttendanceMode();
            var boarderStatus = Queries.GetFirstBorderStatus();

            var res = new List<object[]>
            {
                new object[]
                {
                    sureName,
                    middleName,
                    foreName,
                    gender,
                    dateOfBirth,
                    enrolmentStatus.Description,
                    primaryClass.FullName,
                    attendanceMode.Description,
                    boarderStatus.Description
                }
            };
            return res;
        }

        public List<object[]> TC_AD08_Data()
        {
            var pattern = "M/d/yyyy";
            var sureName = Utilities.GenerateRandomString(10, "SureNameTC08");
            var middleName = Utilities.GenerateRandomString(10, "MiddleNameTC08");
            var foreName = Utilities.GenerateRandomString(10, "ForeNameTC08");
            var dateOfBirth = DateTime.ParseExact("06/06/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var gender = "Female";
            var enrolmentStatus = Queries.GetEnrolmentStatus();
            var primaryClass = Queries.GetFirstPrimaryClass();
            var attendanceMode = Queries.GetFirstAttendanceMode();
            var boarderStatus = Queries.GetFirstBorderStatus();

            var res = new List<object[]>
            {
                new object[]
                {
                    sureName,
                    middleName,
                    foreName,
                    gender,
                    dateOfBirth,
                    enrolmentStatus.Description,
                    primaryClass.FullName,
                    attendanceMode.Description,
                    boarderStatus.Description
                }
            };
            return res;
        }
        #endregion
    }
}
