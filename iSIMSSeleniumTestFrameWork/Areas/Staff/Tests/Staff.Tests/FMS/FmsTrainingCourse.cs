using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner.internals;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Selene.Support.Attributes;

namespace Staff.Tests.FMS
{
    [TestClass]
    public class FmsTrainingCourse
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
        //With ref. to:
        //C:\WIP\Dev\iSIMSSeleniumTestFrameWork\Areas\Staff\Tests\Staff.StaffRecord.Tests\TrainingCourseTests.cs
        [TestMethod]
        [ChromeUiTest("FMS", "P1")]
        public void FMS_TrainingCource_CourseFees_MaxLength()
        {

            //Login as School Personnel Officer and navigate to Training Course  screen
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Training Courses");

            //Create new full-time training course
            var trainingCourseDetails = TrainingCourseTriplet.Create();
            var trainingCourseDetailsPage = new TrainingCourseDetailsPage
            {
                CourseFee = "1000000.999"
            };

            //Save training course
            trainingCourseDetails.ClickSave();

            Assert.IsTrue(trainingCourseDetailsPage.Validation.ToList().Contains("Course Fees cannot be more than 100000.00."));
            Assert.IsTrue(trainingCourseDetailsPage.Validation.ToList().Contains("Training Course Course Fees may have only 2 figure(s) after the decimal point."));

        }

        [TestMethod]
        [ChromeUiTest("FMS", "P1")]
        public void FMS_TrainingCource_NumberOfDays_MaxLength()
        {
            //Login as School Personnel Officer and navigate to Training Course  screen
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Training Courses");

            //Create new full-time training course
            var trainingCourseDetails = TrainingCourseTriplet.Create();
            var trainingCourseDetailsPage = new TrainingCourseDetailsPage
            {
                NumberDay = "999.99999"
            };

            //Save training course
            trainingCourseDetails.ClickSave();

            var validation = trainingCourseDetailsPage.Validation.ToList();
            Assert.IsTrue(validation.Contains("Number of days cannot be more than 999.9999."));
            Assert.IsTrue(validation.Contains("Training Course Duration may have only 4 figure(s) after the decimal point."));

        }

        [TestMethod]
        [ChromeUiTest("FMS", "P1")]
        public void FMS_TrainingCource_AdditionalCosts_MaxLength()
        {
            //Inject basic training course
            string description = "Injected Training Course";
            string CourseFees = "100.00";
            bool FullTime = true;
            string Duration = "10.0000";
            string CousrseLevelDescription = "Certificate";
            string Venue = "Learning place";
            string Comment = "You will learn";
            string AdditionalCosts = "10.00";
            string Provider = "Learning provider";
            string CourseEnrolmentStatus = "Complete";


            Guid attendee_id_1 = Guid.NewGuid();
            string attendee_1_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_1_fn = Utilities.GenerateRandomString(6);
            string attendee_1 = FormatName(attendee_1_fn, attendee_1_sn);

            string trainingCourseTitle = String.Format("Selenium_{0}_{1}", SeleniumHelper.GenerateRandomString(6), SeleniumHelper.GenerateRandomNumber(3));
            Guid trainingCourseId = Guid.NewGuid();
            Guid trainingCourseOccurrenceId = Guid.NewGuid();

            var testdata = new DataPackage[]
            {
               GetStaffRecord_current(attendee_id_1, attendee_1_fn, attendee_1_sn),
               GetTrainingCourse(trainingCourseId, trainingCourseOccurrenceId, description, CourseFees, FullTime, Duration, CousrseLevelDescription,
               Venue, Comment, Provider, trainingCourseTitle),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_1, AdditionalCosts, Comment, CourseEnrolmentStatus)

            };

            //Navigate to injected Training Course as PO
            using (new DataSetup(testdata))
            {
                //Login as School Personnel Officer and navigate to Training Course  screen

                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Training Courses");

                //Select existing training course
                var trainingCourseTriplet = new TrainingCourseTriplet();
                trainingCourseTriplet.SearchCriteria.Title = trainingCourseTitle;
                var trainingCourceSearchResults = trainingCourseTriplet.SearchCriteria.Search();
                var traingCourseSearchTile = trainingCourceSearchResults.Single(t => t.Title.Equals(trainingCourseTitle));
                var trainingCourseDetails = traingCourseSearchTile.Click<TrainingCourseDetailsPage>();
                var newTrainingCourseDetails = new TrainingCourseDetailsPage();

                var trainingEvents = trainingCourseDetails.TrainingCourseEvents.Rows.First().ClickEdit();
                var trainingEventsObject = trainingEvents.Attendees.Rows[0];
                trainingEventsObject.AdditionalCosts = "100000.999";

                trainingEvents.ClickOk();

                Assert.IsTrue(trainingEvents.Validation.ToList().Contains("Additional costs cannot be more than 100000.00"));
                Assert.IsTrue(trainingEvents.Validation.ToList().Contains("Staff Training Course Enrolment AdditionalCosts may have only 2 figure(s) after the decimal point."));
            }
        }

        //With ref. to:
        //C:\WIP\Dev\iSIMSSeleniumTestFrameWork\Areas\Staff\Tests\Staff.StaffRecord.Tests\StaffTrainingCourseEnrolmentTests.cs
        [TestMethod]
        [ChromeUiTest("FMS", "P1")]
        public void FMS_TrainingCourseEnrolment_AdditionalCosts_MaxLength()
        {
            //Inject basic training course
            string description = "Injected Training Course";
            string CourseFees = "100.00";
            bool FullTime = true;
            string Duration = "10.0000";
            string CousrseLevelDescription = "Certificate";
            string Venue = "Learning place";
            string Comment = "You will learn";
            string AdditionalCosts = "10.00";
            string Provider = "Learning provider";
            string CourseEnrolmentStatus = "Complete";


            Guid attendee_id_1 = Guid.NewGuid();
            string attendee_1_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_1_fn = Utilities.GenerateRandomString(6);
            string attendee_1 = FormatName(attendee_1_fn, attendee_1_sn);

            string trainingCourseTitle = String.Format("Selenium_{0}_{1}", SeleniumHelper.GenerateRandomString(6), SeleniumHelper.GenerateRandomNumber(3));
            Guid trainingCourseId = Guid.NewGuid();
            Guid trainingCourseOccurrenceId = Guid.NewGuid();

            var testdata = new DataPackage[]
            {
               GetStaffRecord_current(attendee_id_1, attendee_1_fn, attendee_1_sn),
               GetTrainingCourse(trainingCourseId, trainingCourseOccurrenceId, description, CourseFees, FullTime, Duration, CousrseLevelDescription,
               Venue, Comment, Provider, trainingCourseTitle),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_1, AdditionalCosts, Comment, CourseEnrolmentStatus)

            };

            //Navigate to injected Training Course as PO
            using (new DataSetup(testdata))
            {
                //Login as School Personnel Officer and navigate to Training Course  screen

                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                var staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = attendee_1_sn;
                staffRecordTriplet.SearchCriteria.IsCurrent = true;
                var staffTitles = staffRecordTriplet.SearchCriteria.Search();
                var staffRecordPage = staffTitles.SingleOrDefault(x => true).Click<StaffRecordPage>();
                staffRecordPage.SelectTrainingQualificationsTab();

                var gridRow = staffRecordPage.TrainingHistoryTable.Rows[0];
                gridRow.AdditionalCosts = "100000.999";
                staffRecordPage.ClickSave();

                Assert.IsTrue(staffRecordPage.Validation.ToList().Contains("Additional costs cannot be more than 100000.00"));
                Assert.IsTrue(staffRecordPage.Validation.ToList().Contains("Staff Training Course Enrolment AdditionalCosts may have only 2 figure(s) after the decimal point."));
            }
        }

        #region Data
        private DataPackage GetStaffRecord_current(Guid staffId, string forename, string surname)
        {
            return this.BuildDataPackage()
               .AddData("Staff", new
               {
                   Id = staffId,
                   LegalForename = forename,
                   LegalSurname = surname,
                   LegalMiddleNames = "Middle",
                   PreferredForename = forename,
                   PreferredSurname = surname,
                   DateOfBirth = new DateTime(2000, 1, 1),
                   Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                   PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                   School = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId
               })
               .AddData("StaffServiceRecord", new
               {
                   Id = Guid.NewGuid(),
                   DOA = DateTime.Today.AddDays(-1),
                   ContinuousServiceStartDate = DateTime.Today.AddDays(-1),
                   LocalAuthorityStartDate = DateTime.Today.AddDays(-1),
                   Staff = staffId,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
        }


        private DataPackage GetTrainingCourse(Guid trainingCourseId, Guid trainingCourseOccurrenceId, string desciption, string CourseFees,
           bool FullTime, string Duration, string CousrseLevelDescription, string Venue, string comment, string Provider, string title)
        {
            return this.BuildDataPackage()
                 .AddData("TrainingCourse", new
                 {
                     ID = trainingCourseId,
                     Title = title,
                     Description = desciption,
                     CourseFees = CourseFees,
                     FullTime = FullTime,
                     Duration = Duration,
                     ResourceProvider = CoreQueries.GetSchoolId(),
                     CourseLevel = CoreQueries.GetLookupItem("CourseLevel", description: CousrseLevelDescription),
                     TenantID = SeSugar.Environment.Settings.TenantId
                 })
                 .AddData("TrainingCourseOccurrence", new
                 {
                     ID = trainingCourseOccurrenceId,
                     StartDate = DateTime.Today.AddDays(-14),
                     EndDate = DateTime.Today.AddDays(-7),
                     RenewalDate = DateTime.Today.AddYears(1),
                     Venue = Venue,
                     Provider = Provider,
                     Comment = comment,
                     TrainingCourse = trainingCourseId,
                     TenantID = SeSugar.Environment.Settings.TenantId
                 });
        }


        private DataPackage GetTrainingCourseEnrolment(Guid trainingCourseOccurrenceId, Guid staffId, string additionalCosts, string comment, string CourseEnrolmentStatus)
        {
            return this.BuildDataPackage()
                .AddData("StaffTrainingCourseEnrolment", new
                {
                    ID = Guid.NewGuid(),
                    AdditionalCosts = additionalCosts,
                    Comment = comment,
                    Staff = staffId,
                    TrainingCourseOccurrence = trainingCourseOccurrenceId,
                    CourseEnrolmentStatus = CoreQueries.GetLookupItem("CourseEnrolmentStatus", code: CourseEnrolmentStatus),
                    TenantID = SeSugar.Environment.Settings.TenantId
                });
        }

        #endregion

        #region Support
        private string FormatName(string forename, string surname)
        {
            return string.Format("{1}, {0}", forename, surname);
        }
        #endregion
    }
}
