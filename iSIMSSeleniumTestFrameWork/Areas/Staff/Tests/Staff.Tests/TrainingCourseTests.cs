using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;

namespace Staff.Tests
{
    [TestClass]
    public class TrainingCourseTests
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
        [TestMethod]
        [ChromeUiTest("StaffTrainingCourses", "P1")]
        public void Create_basic_Training_Course_as_PO()
        {
            string title = String.Format("Selenium_{0}_{1}", SeleniumHelper.GenerateRandomString(6), SeleniumHelper.GenerateRandomNumber(3));
            string description = "Full Time Training Course Test";
            string level = "Certificate";
            string numberDay = "10";
            bool isFullTime = true;
            string courseFee = "100";

            string startDate = DateTime.Today.AddDays(5).ToShortDateString();
            string endDate = DateTime.Today.AddDays(10).ToShortDateString();
            string renewalDate = DateTime.Today.AddYears(1).ToShortDateString();
            string venue = "Venue 01 Test";
            string provider = "Provider 01 Test";
            string comment = "This is the first event created for Training course 01";

            //Login as School Personnel Officer and navigate to Training Course  screen
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Training Courses");

            //Create new full-time training course
            var trainingCourseDetails = TrainingCourseTriplet.Create();
            trainingCourseDetails.Title = title;
            trainingCourseDetails.Description = description;
            trainingCourseDetails.Level = level;
            trainingCourseDetails.NumberDay = numberDay;
            trainingCourseDetails.IsFullTime = isFullTime;
            trainingCourseDetails.CourseFee = courseFee;

            //Add training course event
            var trainingEvents = trainingCourseDetails.AddTrainingCourseEvents();
            trainingEvents.StartDate = startDate;
            trainingEvents.EndDate = endDate;
            trainingEvents.RenewalDate = renewalDate;
            trainingEvents.Venue = venue;
            trainingEvents.Provider = provider;
            trainingEvents.Comment = comment;

            trainingEvents.ClickOk();

            //Save training course
            trainingCourseDetails.ClickSave();

            Assert.IsTrue(AutomationSugar.SuccessMessagePresent(trainingCourseDetails.ComponentIdentifier), "Add New Training Course failed");

        }

        [TestMethod]
        [ChromeUiTest("StaffTrainingCourses", "P1")]
        public void Read_existing_basic_Training_Course_as_PO()
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


            Guid attendee_id_2 = Guid.NewGuid();
            string attendee_2_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_2_fn = Utilities.GenerateRandomString(6);
            string attendee_2 = FormatName(attendee_2_fn, attendee_2_sn);

            Guid attendee_id_3 = Guid.NewGuid();
            string attendee_3_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_3_fn = Utilities.GenerateRandomString(6);
            string attendee_3 = FormatName(attendee_3_fn, attendee_3_sn);

            Guid attendee_id_4 = Guid.NewGuid();
            string attendee_4_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_4_fn = Utilities.GenerateRandomString(6);
            string attendee_4 = FormatName(attendee_4_fn, attendee_4_sn);

            string trainingCourseTitle = String.Format("Selenium_{0}_{1}", SeleniumHelper.GenerateRandomString(6), SeleniumHelper.GenerateRandomNumber(3));
            Guid trainingCourseId = Guid.NewGuid();
            Guid trainingCourseOccurrenceId = Guid.NewGuid();

            var testdata = new DataPackage[]
            {
               GetStaffRecord_current(attendee_id_1, attendee_1_fn, attendee_1_sn),
               GetStaffRecord_current(attendee_id_2, attendee_2_fn, attendee_2_sn),
               GetStaffRecord_current(attendee_id_3, attendee_3_fn, attendee_3_sn),
               GetStaffRecord_current(attendee_id_4, attendee_4_fn, attendee_4_sn),
               GetTrainingCourse(trainingCourseId, trainingCourseOccurrenceId, description, CourseFees, FullTime, Duration, CousrseLevelDescription,
               Venue, Comment, Provider, trainingCourseTitle),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_1, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_2, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_3, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_4, AdditionalCosts, Comment, CourseEnrolmentStatus)
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

                var trainingEvents = trainingCourseDetails.TrainingCourseEvents.Rows.First().ClickEdit();
                var _additionalCosts = trainingEvents.Attendees.Rows[0].AdditionalCosts;
                var _provider = trainingEvents.Provider;
                var __venue = trainingEvents.Venue;

                Assert.AreEqual(trainingCourseTitle, trainingCourseDetails.Title, "injected Title not present");
                Assert.AreEqual(description, trainingCourseDetails.Description, "injected Description not present");
                Assert.AreEqual(CourseFees, trainingCourseDetails.CourseFee, "injected CourseFee not present");
                Assert.AreEqual(FullTime, trainingCourseDetails.IsFullTime, "injected IsFullTime not present");
                Assert.AreEqual(Duration, trainingCourseDetails.NumberDay, "injected NumberDay not present");
                Assert.AreEqual(Provider, _provider, "injected Provide not present");
                Assert.AreEqual(Venue, __venue, "injected Venue not present");
                Assert.AreEqual(AdditionalCosts, _additionalCosts, "injected NumberDay not present");

            }
        }

        [TestMethod]
        [ChromeUiTest("StaffTrainingCourses", "P1")]
        public void Update_existing_basic_Training_Course_as_PO()
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

            Guid attendee_id_2 = Guid.NewGuid();
            string attendee_2_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_2_fn = Utilities.GenerateRandomString(6);
            string attendee_2 = FormatName(attendee_2_fn, attendee_2_sn);

            Guid attendee_id_3 = Guid.NewGuid();
            string attendee_3_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_3_fn = Utilities.GenerateRandomString(6);
            string attendee_3 = FormatName(attendee_3_fn, attendee_3_sn);

            Guid attendee_id_4 = Guid.NewGuid();
            string attendee_4_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_4_fn = Utilities.GenerateRandomString(6);
            string attendee_4 = FormatName(attendee_4_fn, attendee_4_sn);

            string trainingCourseTitle = String.Format("Selenium_{0}_{1}", SeleniumHelper.GenerateRandomString(6), SeleniumHelper.GenerateRandomNumber(3));
            Guid trainingCourseId = Guid.NewGuid();
            Guid trainingCourseOccurrenceId = Guid.NewGuid();

            var testdata = new DataPackage[]
            {
               GetStaffRecord_current(attendee_id_1, attendee_1_fn, attendee_1_sn),
               GetStaffRecord_current(attendee_id_2, attendee_2_fn, attendee_2_sn),
               GetStaffRecord_current(attendee_id_3, attendee_3_fn, attendee_3_sn),
               GetStaffRecord_current(attendee_id_4, attendee_4_fn, attendee_4_sn),
               GetTrainingCourse(trainingCourseId, trainingCourseOccurrenceId, description, CourseFees, FullTime, Duration, CousrseLevelDescription,
                Venue, Comment, Provider, trainingCourseTitle),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_1, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_2, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_3, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_4, AdditionalCosts, Comment, CourseEnrolmentStatus)
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

                //Make some changes, Save
                trainingCourseTitle = newTrainingCourseDetails.Title = Utilities.GenerateRandomString(8, "Selenium");
                CourseFees = newTrainingCourseDetails.CourseFee = "200.00";
                description = newTrainingCourseDetails.Description = Utilities.GenerateRandomString(15, "Selenium");

                var trainingEvents = trainingCourseDetails.TrainingCourseEvents.Rows.First().ClickEdit();
                var trainingEventsObject = trainingEvents.Attendees.Rows[0];
                AdditionalCosts = trainingEventsObject.AdditionalCosts = "200.00";
                Comment = trainingEventsObject.Comment = Utilities.GenerateRandomString(8, "Selenium");

                var newTrainingEvents = new TrainingCourseEventDialog();
                Provider = newTrainingEvents.Provider = Utilities.GenerateRandomString(8, "Selenium");
                Venue = newTrainingEvents.Venue = Utilities.GenerateRandomString(8, "Selenium");
                trainingEvents.ClickOk();
                //Save training course
                //SSJ - VERY VERY SORRY FOR THIS - temp fix to allow jquery custom changescope insertion to take place - Selenium is too fast - working on a fix.
                System.Threading.Thread.Sleep(1000);
                trainingCourseDetails.ClickSave();
                trainingCourseDetails.Refresh();

                // load new results
                var editedTrainingCourseDetails = traingCourseSearchTile.Click<TrainingCourseDetailsPage>();

                var editedtrainingEvents = trainingCourseDetails.TrainingCourseEvents.Rows.First().ClickEdit();

                //Assert save success, values retained

                Assert.AreEqual(trainingCourseTitle, editedTrainingCourseDetails.Title, "injected Title not present");
                Assert.AreEqual(description, editedTrainingCourseDetails.Description, "injected Description not present");
                Assert.AreEqual(CourseFees, editedTrainingCourseDetails.CourseFee, "injected CourseFee not present");
                Assert.AreEqual(FullTime, editedTrainingCourseDetails.IsFullTime, "injected IsFullTime not present");
                Assert.AreEqual(Duration, editedTrainingCourseDetails.NumberDay, "injected NumberDay not present");
                Assert.AreEqual(Provider, editedTrainingCourseDetails.TrainingCourseEvents.Rows[0].CourseProvider, "injected Provide not present");
                Assert.AreEqual(Venue, editedTrainingCourseDetails.TrainingCourseEvents.Rows[0].Venue, "injected Venue not present");
                Assert.AreEqual(AdditionalCosts, editedtrainingEvents.Attendees.Rows[0].AdditionalCosts, "injected NumberDay not present");
                Assert.AreEqual(Comment, editedtrainingEvents.Attendees.Rows[0].Comment, "injected Comment not present");

            }
        }

        [TestMethod]
        [ChromeUiTest("StaffTrainingCourses", "P1")]
        public void Add_attendees_one_by_one_to_existing_Training_Course_as_PO()
        {
            Action<string, string, string, string, TrainingCourseEventDialog, string> addAttendeesAction = AddAttendeesOneByOne;
            AddAttendeesCommon(addAttendeesAction);
        }

        [TestMethod]
        [ChromeUiTest("StaffTrainingCourses", "P1")]
        public void Add_attendees_in_bulk_to_existing_Training_Course_as_PO()
        {
            Action<string, string, string, string, TrainingCourseEventDialog, string> addAttendeesAction = AddAttendeesBulk;
            AddAttendeesCommon(addAttendeesAction);
        }

        private void AddAttendeesCommon(Action<string, string, string, string, TrainingCourseEventDialog, string> addAttendeesAction)
        {

            string key = Utilities.GenerateRandomString(2);

            string description = "Injected Training Course";
            string CourseFees = "100.00";
            bool FullTime = true;
            string Duration = "10.0000";
            string CousrseLevelDescription = "Certificate";
            string Venue = "Learning place";
            string Comment = "You will learn";
            string Provider = "Learning provider";

            Guid attendee_id_1 = Guid.NewGuid();
            string attendee_1_sn = Utilities.GenerateRandomString(8, "Selenium_" + key);
            string attendee_1_fn = Utilities.GenerateRandomString(6);
            string attendee_1 = FormatName(attendee_1_fn, attendee_1_sn);

            Guid attendee_id_2 = Guid.NewGuid();
            string attendee_2_sn = Utilities.GenerateRandomString(8, "Selenium_" + key);
            string attendee_2_fn = Utilities.GenerateRandomString(6);
            string attendee_2 = FormatName(attendee_2_fn, attendee_2_sn);

            Guid attendee_id_3 = Guid.NewGuid();
            string attendee_3_sn = Utilities.GenerateRandomString(8, "Selenium_" + key);
            string attendee_3_fn = Utilities.GenerateRandomString(6);
            string attendee_3 = FormatName(attendee_3_fn, attendee_3_sn);

            Guid attendee_id_4 = Guid.NewGuid();
            string attendee_4_sn = Utilities.GenerateRandomString(8, "Selenium_" + key);
            string attendee_4_fn = Utilities.GenerateRandomString(6);
            string attendee_4 = FormatName(attendee_4_fn, attendee_4_sn);

            string trainingCourseTitle = String.Format("Selenium_{0}_{1}", SeleniumHelper.GenerateRandomString(6), SeleniumHelper.GenerateRandomNumber(3));
            Guid trainingCourseId = Guid.NewGuid();
            Guid trainingCourseOccurrenceId = Guid.NewGuid();

            var testData = new DataPackage[]
            {
                GetStaffRecord_current(attendee_id_1, attendee_1_fn, attendee_1_sn),
                GetStaffRecord_current(attendee_id_2, attendee_2_fn, attendee_2_sn),
                GetStaffRecord_current(attendee_id_3, attendee_3_fn, attendee_3_sn),
                GetStaffRecord_current(attendee_id_4, attendee_4_fn, attendee_4_sn),
                 GetTrainingCourse(trainingCourseId, trainingCourseOccurrenceId, description, CourseFees, FullTime, Duration, CousrseLevelDescription,
               Venue, Comment, Provider, trainingCourseTitle)
            };

            using (new DataSetup(testData))
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

                var trainingEvents = trainingCourseDetails.TrainingCourseEvents.Rows.First().ClickEdit();

                //Add attendees
                addAttendeesAction(attendee_1, attendee_2, attendee_3, attendee_4, trainingEvents, key);

                trainingEvents.ClickOk();

                //Save training course
                //SSJ - VERY VERY SORRY FOR THIS - temp fix to allow jquery custom changescope insertion to take place - Selenium is too fast - working on a fix.
                System.Threading.Thread.Sleep(1000);
                trainingCourseDetails.ClickSave();

                Assert.IsTrue(AutomationSugar.SuccessMessagePresent(trainingCourseDetails.ComponentIdentifier), "Update Training Course failed");

                //Check the attendees were added to training course successfully
                var events = trainingCourseDetails.TrainingCourseEvents.Rows;
                trainingEvents = events.First().ClickEdit();

                trainingEvents.Attendees.Refresh();
                var trainingEventsAttendees = trainingEvents.Attendees.Rows;
                Assert.AreEqual(4, trainingEventsAttendees.Count, "List of attendees are not enough 4 members");
                Assert.AreEqual(true, trainingEventsAttendees.Any(x => x.AttendeeName.Equals(attendee_1)), String.Format("The first member is not {0}", attendee_1));
                Assert.AreEqual(true, trainingEventsAttendees.Any(x => x.AttendeeName.Equals(attendee_2)), String.Format("The second member is not {0}", attendee_2));
                Assert.AreEqual(true, trainingEventsAttendees.Any(x => x.AttendeeName.Equals(attendee_3)), String.Format("The third member is not {0}", attendee_3));
                Assert.AreEqual(true, trainingEventsAttendees.Any(x => x.AttendeeName.Equals(attendee_4)), String.Format("The fourth member is not {0}", attendee_4));
            }
        }

        private static void AddAttendeesOneByOne(string attendee_1, string attendee_2, string attendee_3, string attendee_4, TrainingCourseEventDialog trainingEvents, string key)
        {
            var staffSearch = trainingEvents.AddAttendees();
            staffSearch.SearchCriteria.StaffName = attendee_1;
            var searchResults = staffSearch.SearchCriteria.Search();
            searchResults.Single(t => t.Name.Equals(attendee_1)).Click();
            staffSearch.ClickOk(waitForStale: true);

            staffSearch = trainingEvents.AddAttendees();
            staffSearch.SearchCriteria.StaffName = attendee_2;
            searchResults = staffSearch.SearchCriteria.Search();
            searchResults.Single(t => t.Name.Equals(attendee_2)).Click();
            staffSearch.ClickOk(waitForStale: true);

            staffSearch = trainingEvents.AddAttendees();
            staffSearch.SearchCriteria.StaffName = attendee_3;
            searchResults = staffSearch.SearchCriteria.Search();
            searchResults.Single(t => t.Name.Equals(attendee_3)).Click();
            staffSearch.ClickOk(waitForStale: true);

            staffSearch = trainingEvents.AddAttendees();
            staffSearch.SearchCriteria.StaffName = attendee_4;
            searchResults = staffSearch.SearchCriteria.Search();
            searchResults.Single(t => t.Name.Equals(attendee_4)).Click();
            staffSearch.ClickOk(waitForStale: true);
        }

        private static void AddAttendeesBulk(string attendee_1, string attendee_2, string attendee_3, string attendee_4, TrainingCourseEventDialog trainingEvents, string key)
        {
            var staffSearch = trainingEvents.AddAttendees();
            staffSearch.SearchCriteria.StaffName = "Selenium_" + key;
            var searchResults = staffSearch.SearchCriteria.Search();

            searchResults.Single(t => t.Name.Equals(attendee_1)).Click();
            searchResults.Single(t => t.Name.Equals(attendee_2)).Click();
            searchResults.Single(t => t.Name.Equals(attendee_3)).Click();
            searchResults.Single(t => t.Name.Equals(attendee_4)).Click();

            staffSearch.ClickOk(waitForStale: true);
        }

        [TestMethod]
        [ChromeUiTest("StaffTrainingCourses", "P1")]
        public void Delete_existing_Training_Course_as_PO()
        {

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

            Guid attendee_id_2 = Guid.NewGuid();
            string attendee_2_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_2_fn = Utilities.GenerateRandomString(6);
            string attendee_2 = FormatName(attendee_2_fn, attendee_2_sn);

            Guid attendee_id_3 = Guid.NewGuid();
            string attendee_3_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_3_fn = Utilities.GenerateRandomString(6);
            string attendee_3 = FormatName(attendee_3_fn, attendee_3_sn);

            Guid attendee_id_4 = Guid.NewGuid();
            string attendee_4_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_4_fn = Utilities.GenerateRandomString(6);
            string attendee_4 = FormatName(attendee_4_fn, attendee_4_sn);

            string trainingCourseTitle = String.Format("Selenium_{0}_{1}", SeleniumHelper.GenerateRandomString(6), SeleniumHelper.GenerateRandomNumber(3));
            Guid trainingCourseId = Guid.NewGuid();
            Guid trainingCourseOccurrenceId = Guid.NewGuid();

            var testData = new DataPackage[]
            {
                GetStaffRecord_current(attendee_id_1, attendee_1_fn, attendee_1_sn),
                GetStaffRecord_current(attendee_id_2, attendee_2_fn, attendee_2_sn),
                GetStaffRecord_current(attendee_id_3, attendee_3_fn, attendee_3_sn),
                GetStaffRecord_current(attendee_id_4, attendee_4_fn, attendee_4_sn),
                GetTrainingCourse(trainingCourseId, trainingCourseOccurrenceId, description, CourseFees, FullTime, Duration, CousrseLevelDescription,
               Venue, Comment, Provider, trainingCourseTitle),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_1, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_2, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_3, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_4, AdditionalCosts, Comment, CourseEnrolmentStatus)
            };

            using (new DataSetup(testData))
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

                //Delete the data has just added
                trainingCourseDetails.ClickDelete();

                Assert.IsTrue(AutomationSugar.SuccessMessagePresent(trainingCourseDetails.ComponentIdentifier), "Delete Training Course failed");
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "StaffTrainingCourses", "P2" })]
        public void Remove_Staff_Attendee_from_Training_Course_Occurrence_as_PO()
        {
            /* AS:                  PO
             * PREREQ:              Inject Staff Record, Inject Training Course, Inject Training Course Occurrence, Inject Staff Training Course  (i.e. a staff with a training course)
             * STEPS:               1) Navigate to Training Courses, Search for and load Training Course, Remove Injected Staff Training Course Enrolment (Grid row from attendees grid), Save, 
             *                      2) Load Staff record for the attendee, verify Training Course is not present in training cources grid.
             * ASSERTS:             Save success, attendee removed in Staff Record, 
             * SEVERITY ON FAILURE: P2
             */

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

            Guid attendee_id_2 = Guid.NewGuid();
            string attendee_2_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_2_fn = Utilities.GenerateRandomString(6);
            string attendee_2 = FormatName(attendee_2_fn, attendee_2_sn);

            Guid attendee_id_3 = Guid.NewGuid();
            string attendee_3_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_3_fn = Utilities.GenerateRandomString(6);
            string attendee_3 = FormatName(attendee_3_fn, attendee_3_sn);

            Guid attendee_id_4 = Guid.NewGuid();
            string attendee_4_sn = Utilities.GenerateRandomString(8, "Selenium");
            string attendee_4_fn = Utilities.GenerateRandomString(6);
            string attendee_4 = FormatName(attendee_4_fn, attendee_4_sn);

            string trainingCourseTitle = String.Format("Selenium_{0}_{1}", SeleniumHelper.GenerateRandomString(6), SeleniumHelper.GenerateRandomNumber(3));
            Guid trainingCourseId = Guid.NewGuid();
            Guid trainingCourseOccurrenceId = Guid.NewGuid();


            var testdata = new DataPackage[]
            {
               GetStaffRecord_current(attendee_id_1, attendee_1_fn, attendee_1_sn),
               GetStaffRecord_current(attendee_id_2, attendee_2_fn, attendee_2_sn),
               GetStaffRecord_current(attendee_id_3, attendee_3_fn, attendee_3_sn),
               GetStaffRecord_current(attendee_id_4, attendee_4_fn, attendee_4_sn),
               GetTrainingCourse(trainingCourseId, trainingCourseOccurrenceId, description, CourseFees, FullTime, Duration, CousrseLevelDescription,
                Venue, Comment, Provider, trainingCourseTitle),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_1, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_2, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_3, AdditionalCosts, Comment, CourseEnrolmentStatus),
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_4, AdditionalCosts, Comment, CourseEnrolmentStatus)
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
                var trainingEvents = trainingCourseDetails.TrainingCourseEvents.Rows.First().ClickEdit();
                var FirsttrainingCourseOccurrenceId = trainingEvents.Attendees.Rows[0].RowId;

                // checkbox selected warning
                bool checkboxvalue = trainingEvents.Attendees.Rows[0].Selected = true;

                trainingEvents.ClickDeleteAttendees(checkboxvalue);
                trainingEvents.ClickOk();
                //SSJ - VERY VERY SORRY FOR THIS - temp fix to allow jquery custom changescope insertion to take place - Selenium is too fast - working on a fix.
                System.Threading.Thread.Sleep(1000);
                trainingCourseDetails.ClickSave();

                // load new results
                var editedTrainingCourseDetails = traingCourseSearchTile.Click<TrainingCourseDetailsPage>();

                var editedtrainingEvents = trainingCourseDetails.TrainingCourseEvents.Rows.First().ClickEdit();

                Assert.IsFalse(editedtrainingEvents.Attendees.Rows.Exists(x => new Guid(x.RowId).Equals(Guid.Parse(FirsttrainingCourseOccurrenceId))), "delete attendee not successfull");
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "StaffTrainingCourses", "P2" })]
        public void Remove_Training_Course_Enrolment_from_Staff_Record_as_PO()
        {
            /* AS:                  PO
             * PREREQ.:             Inject Staff Record, Inject Training Course, Inject Training Course Occurrence, Inject Staff Training Course Enrolment (i.e. a staff with a training course)
             * STEPS:               1) Navigate to Staff Record, Remove Injected Training Course from grid, Save
             *                      2) Navigate to Training Course, Search for and load the injected training course, verify that the injected staff is no longer an attendee
             * ASSERTS:             At the start that the Staff IS an Atendeed, Save success of Staff Record, values removed from grid, At the end that the Staff is no longer an Attendee on training courses page.
             * SEVERITY ON FAILURE: P2
             */

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
               GetTrainingCourseEnrolment(trainingCourseOccurrenceId, attendee_id_1, AdditionalCosts, Comment, CourseEnrolmentStatus),

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
                var gridRowRowId = gridRow.RowId;
                gridRow.DeleteRow();
                staffRecordPage.ClickSave();

                AutomationSugar.NavigateMenu("Tasks", "Staff", "Training Courses");
                //Select existing training course
                var trainingCourseTriplet = new TrainingCourseTriplet();
                trainingCourseTriplet.SearchCriteria.Title = trainingCourseTitle;
                var trainingCourceSearchResults = trainingCourseTriplet.SearchCriteria.Search();
                var traingCourseSearchTile = trainingCourceSearchResults.Single(t => t.Title.Equals(trainingCourseTitle));
                var trainingCourseDetails = traingCourseSearchTile.Click<TrainingCourseDetailsPage>();
                var trainingEvents = trainingCourseDetails.TrainingCourseEvents.Rows.First().ClickEdit();

                Assert.IsFalse(trainingEvents.Attendees.Rows.Exists(x => new Guid(x.RowId).Equals(Guid.Parse(gridRowRowId))), "delete attendee from staff record not successfull");

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
