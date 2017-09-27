using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Components.Conduct;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using TestSettings;
using SimsBy = POM.Helper.SimsBy;

namespace Pupil.Conduct.Tests
{
    public class SuspensionTests
    {
        //private SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;
        private readonly SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.TestUser;

        /// <summary>
        /// Description: Create a New Suspension
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2 })]
        public void Suspensions_AddSuspension()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            var forename = "AddSuspension";
            var surname = Utilities.GenerateRandomString(10, "AddSuspension");
            string pupilName = String.Format("{0}, {1}", surname, forename);

            //Add learner
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03));

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    // Login
                    SeleniumHelper.Login(LoginAs);

                    //Navigate to suspension
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Suspensions");

                    //Create a new suspension
                    Thread.Sleep(2000);
                    var suspensionTriplet = new SuspensionTriplet();
                    suspensionTriplet.SearchCriteria.PupilName = pupilName;
                    Thread.Sleep(5000);
                    var suspensionPage =
                        suspensionTriplet.SearchCriteria.Search()
                            .SingleOrDefault(x => x.Name.Equals(pupilName))
                            .Click<SuspensionRecordPage>();

                    var addSuspensionDialog = suspensionPage.ClickAddNewRecord();

                    var exclusionType = Queries.GetFirstExclusionType();
                    var exclusionReason = Queries.GetFirstExclusionReason();

                    // Enter values
                    addSuspensionDialog.Type = exclusionType.Description;
                    addSuspensionDialog.Reason = exclusionReason.Description;
                    addSuspensionDialog.StartDate = new DateTime(DateTime.Today.Year, 2, 2).ToString("M/d/yyyy");
                    addSuspensionDialog.EndDate = new DateTime(DateTime.Today.Year, 3, 3).ToString("M/d/yyyy");
                    addSuspensionDialog.StartTime = "09:00";
                    addSuspensionDialog.EndTime = "16:00";
                    addSuspensionDialog.Lenght = "25";
                    addSuspensionDialog.Note = Utilities.GenerateRandomString(10, "AddSuspension");
                    suspensionPage = addSuspensionDialog.SaveValue();
                    suspensionPage = suspensionPage.SaveValues();
                    Assert.IsTrue(suspensionPage.IsSuccessMessageDisplay(), "Success message did not display");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }



        /// <summary>
        ///  Edit an existing Suspension (add a status and a meeting)
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2 })]
        public void Suspensions_EditSuspension()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            var forename = "EditSuspension";
            var surname = Utilities.GenerateRandomString(10, "EditSuspension");
            string pupilName = String.Format("{0}, {1}", surname, forename);

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddBasicSuspension(learnerId, new DateTime(2016, 02, 01), new DateTime(2016, 02, 05), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    // Login
                    SeleniumHelper.Login(LoginAs);

                    // Navigate to suspension
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Suspensions");
                    Thread.Sleep(2000);
                    var suspensionTriplet = new SuspensionTriplet();
                    suspensionTriplet.SearchCriteria.PupilName = pupilName;
                    Thread.Sleep(5000);
                    var suspensionPage =
                        suspensionTriplet.SearchCriteria.Search()
                            .SingleOrDefault(x => x.Name.Equals(pupilName))
                            .Click<SuspensionRecordPage>();
                    var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
                    var row = suspensionGrid.Rows[0];

                    // Edit Suspension record 
                    var editSuspensionDialog = row.ClickEditRecord();

                    var exclusionStatus = "Parent/Governors Notified";
                    var exclusionStatusChangeReason = "Head Teacher's Decision";
                    var exclusionMeetingType = "Discipline Committee";

                    // Add a Status
                    editSuspensionDialog.AddSuspensionStatus();
                    var statusGrid = editSuspensionDialog.StatusGrid;
                    var statusGridEmptyRow = statusGrid.Rows.FirstOrDefault(x => x.Date.Trim().Equals(String.Empty));
                    statusGridEmptyRow.Status = exclusionStatus;
                    statusGridEmptyRow.Reason = exclusionStatusChangeReason;
                    statusGridEmptyRow.Date = SeleniumHelper.GetToDay();

                    // Add a meeting
                    var meetingDialog = editSuspensionDialog.AddMeeting();
                    meetingDialog.Type = exclusionMeetingType;
                    meetingDialog.StartDate = SeleniumHelper.GetToDay();
                    meetingDialog.StartTime = "11:30 AM";
                    editSuspensionDialog = meetingDialog.SaveValue();
                    suspensionPage = editSuspensionDialog.SaveValue();
                    suspensionPage.SaveValues();

                    // Verify message displays.
                    Assert.IsTrue(suspensionPage.IsSuccessMessageDisplay(), "Success not displayed");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }


        /// <summary>
        /// update an existing suspension (modify existing values)
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2, })]
        public void Suspension_UpdateSuspension()
        {

            //Arrange
            var learnerId = Guid.NewGuid();
            var suspensionId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            var forename = "UpdateSuspension";
            var surname = Utilities.GenerateRandomString(10, "UpdateSuspension");
            string pupilName = String.Format("{0}, {1}", surname, forename);

            var exclusionType = Queries.GetFirstExclusionType();
            var exclusionReason = Queries.GetFirstExclusionReason();

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddSuspensionWithMeeting(learnerId, suspensionId, new DateTime(2016, 02, 01), new DateTime(2016, 02, 05), 5, exclusionType.ID, exclusionReason.ID);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    // Login
                    SeleniumHelper.Login(LoginAs);
                    Thread.Sleep(2000);

                    // Navigate to suspension record
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Suspensions");
                    Thread.Sleep(2000);

                    var suspensionTriplet = new SuspensionTriplet();
                    suspensionTriplet.SearchCriteria.PupilName = pupilName;
                    var suspensionPage =
                        suspensionTriplet.SearchCriteria.Search()
                            .SingleOrDefault(x => x.Name.Equals(pupilName))
                            .Click<SuspensionRecordPage>();
                    var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
                    var row = suspensionGrid.Rows[0];
                    Assert.AreNotEqual(null, row, "Record does not exist. Table is empty or values are not correct");

                    string pattern = "M/d/yyyy";
                    var exclusionStartDate = DateTime.ParseExact("02/02/2016", pattern, CultureInfo.InvariantCulture).ToString(pattern);
                    var exclusionMeetingStartDate =
                        DateTime.ParseExact("02/04/2016", pattern, CultureInfo.InvariantCulture).ToString(pattern);
                    var exclusionMeetingType = "Appeal Tribunal";

                    // Edit the basic details of the suspension record
                    var editSuspensionDialog = row.ClickEditRecord();
                    editSuspensionDialog.Type = Queries.GetFirstExclusionType(exclusionType.Description).Description;
                    editSuspensionDialog.Reason = Queries.GetFirstExclusionReason(exclusionReason.Description).Description;
                    editSuspensionDialog.StartDate = exclusionStartDate;

                    // Edit Meeting
                    var meetingGrid = editSuspensionDialog.MeetingGrid;
                    var rowMeeting = meetingGrid.Rows[0];
                    var meetingDialog = rowMeeting.EditMeeting();
                    meetingDialog.Type = exclusionMeetingType;
                    meetingDialog.StartDate = exclusionMeetingStartDate;
                    editSuspensionDialog = meetingDialog.SaveValue();
                    suspensionPage = editSuspensionDialog.SaveValue();
                    suspensionPage = suspensionPage.SaveValues();

                    // Verify message displays.
                    Assert.IsTrue(suspensionPage.IsSuccessMessageDisplay(), "Success msg not displayed");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }

        /// <summary>
        /// Check 'Senior Leadership Team' permissions.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2 })]
        public void Suspensions_SeniorLeadershipPermissions()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            var forename = "SeniorLeadershipPermissions";
            var surname = Utilities.GenerateRandomString(10, "SeniorLeadershipPermissions");
            string pupilName = String.Format("{0}, {1}", surname, forename);

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddBasicSuspension(learnerId, new DateTime(2016, 02, 01), new DateTime(2016, 02, 05), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    // Login
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam);

                    // Navigate to suspensions
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Suspensions");

                    var suspensionTriplet = new SuspensionTriplet();
                    suspensionTriplet = new SuspensionTriplet();
                    suspensionTriplet.SearchCriteria.PupilName = pupilName;
                    var suspensionPage =
                        suspensionTriplet.SearchCriteria.Search()
                            .SingleOrDefault(x => x.Name.Equals(pupilName))
                            .Click<SuspensionRecordPage>();
                    var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
                    var row = suspensionGrid.Rows[0];

                    // Edit suspension
                    var editSuspensionDialog = row.ClickEditRecord();

                    // Enter values note document grid
                    editSuspensionDialog.AddSuspensionDocumentNote();
                    var noteDocumentGrid = editSuspensionDialog.NoteDocumentGrid;
                    var emptyRow = noteDocumentGrid.Rows[0];
                    emptyRow.Summary = "SeniorLeadershipPermissionsSummary";
                    suspensionPage = editSuspensionDialog.SaveValue();
                    suspensionPage.SaveValues();

                    // Verify message displays.
                    Assert.IsTrue(suspensionPage.IsSuccessMessageDisplay(), "Message success does not display");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }


        /// <summary>
        /// Check 'SEN Coordinator' Pemissions (view)
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2 })]
        public void Suspensions_SENCoordPermissions()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            var forename = "SENCoordPermissions";
            var surname = Utilities.GenerateRandomString(10, "SENCoordPermissions");
            string pupilName = String.Format("{0}, {1}", surname, forename);

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddBasicSuspension(learnerId, new DateTime(2016, 02, 01), new DateTime(2016, 02, 05), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    // Login
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SENCoordinator);
                    Thread.Sleep(2000);

                    // Navigate to Suspensions by search as SEN Coordinnator logged in to Classroom staff view by default which won't have menu route for Suspensions.
                    SeleniumHelper.NavigateBySearch("Suspensions");
                    Thread.Sleep(2000);

                    // Search suspension record again and verify record exists and diplays correct values
                    var suspensionTriplet = new SuspensionTriplet();
                    suspensionTriplet = new SuspensionTriplet();
                    suspensionTriplet.SearchCriteria.PupilName = pupilName;
                    var suspensionPage =
                        suspensionTriplet.SearchCriteria.Search()
                            .SingleOrDefault(x => x.Name.Equals(pupilName))
                            .Click<SuspensionRecordPage>();
                    var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
                    var row = suspensionGrid.Rows[0];
                    Assert.AreNotEqual(null, row, "Record does not exist. Table is empty or values are not correct");

                    // Verify delete button is disabled
                    Assert.AreEqual(false, row.isDeleteEnable(), "Button delete is enabled");

                    // Verify, although the edit button is enabled on opening the dialog all options are disabled.
                    Assert.AreEqual(true, row.isEditEnable(), "Edit button is disabled");
                    var editDialog = row.ClickEditRecord();
                    Assert.IsTrue(editDialog.IsAllElementDisable(), "All element are not disabled");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }


        /// <summary>
        /// Check an overlapping suspension cannot be created
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2 })]
        public void Suspensions_Overlap()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var suspensionId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            var forename = "Overlap";
            var surname = Utilities.GenerateRandomString(10, "Overlap");
            string pupilName = string.Format("{0}, {1}", surname, forename);

            var exclusionType = Queries.GetFirstExclusionType();
            var exclusionReason = Queries.GetFirstExclusionReason();

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                       .AddSuspensionWithMeeting(learnerId, suspensionId, new DateTime(2016, 02, 01), new DateTime(2016, 02, 05), 5, exclusionType.ID, exclusionReason.ID);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    // Login
                    SeleniumHelper.Login(LoginAs);

                    // Navigate to suspension
                    Thread.Sleep(2000);
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Suspensions");

                    // Search and select pupil
                    Thread.Sleep(5000);
                    var suspensionTriplet = new SuspensionTriplet();
                    suspensionTriplet.SearchCriteria.IsCurrent = true;
                    suspensionTriplet.SearchCriteria.PupilName = pupilName;

                    // Select the pupil 
                    var suspensionPage =
                        suspensionTriplet.SearchCriteria.Search()
                            .SingleOrDefault(x => x.Name.Equals(pupilName))
                            .Click<SuspensionRecordPage>();

                    // Click add new suspension
                    var addNewSuspensionDialog = suspensionPage.ClickAddNewRecord();

                    string pattern = "M/d/yyyy";

                    var exclusionStartDate = DateTime.ParseExact("02/02/2016", pattern, CultureInfo.InvariantCulture).ToString(pattern);
                    var exclusionEndDate = DateTime.ParseExact("02/04/2016", pattern, CultureInfo.InvariantCulture).ToString(pattern);
                    var exclusionLength = "3";

                    // Fill values
                    addNewSuspensionDialog.Type = Queries.GetFirstExclusionType(exclusionType.Description).Description;
                    addNewSuspensionDialog.Reason = Queries.GetFirstExclusionReason(exclusionReason.Description).Description;
                    addNewSuspensionDialog.StartDate = exclusionStartDate;
                    addNewSuspensionDialog.EndDate = exclusionEndDate;
                    addNewSuspensionDialog.StartTime = "09:00";
                    addNewSuspensionDialog.EndTime = "16:00";
                    addNewSuspensionDialog.Lenght = exclusionLength;

                    // Save values
                    suspensionPage = addNewSuspensionDialog.SaveValue();
                    suspensionPage = suspensionPage.SaveValues();

                    // Verify date overlap message is displayed
                    Assert.IsTrue(suspensionPage.IsOverlapMessageDisplayed(), "Expected Date Overlap message not displayed.");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }

        /// <summary>
        /// Delete Suspension 
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2 })]
        public void Suspensions_DeleteSuspension()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            var forename = "DeleteSuspension";
            var surname = Utilities.GenerateRandomString(10, "DeleteSuspension");
            string pupilName = String.Format("{0}, {1}", surname, forename);

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddBasicSuspension(learnerId, new DateTime(2016, 02, 01), new DateTime(2016, 02, 05), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    // Login
                    SeleniumHelper.Login(LoginAs);

                    // Navigate to suspensions
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Suspensions");

                    // Search record
                    Thread.Sleep(5000);
                    var suspensionTriplet = new SuspensionTriplet();
                    suspensionTriplet = new SuspensionTriplet();
                    suspensionTriplet.SearchCriteria.PupilName = pupilName;
                    var suspensionPage =
                        suspensionTriplet.SearchCriteria.Search()
                            .SingleOrDefault(x => x.Name.Equals(pupilName))
                            .Click<SuspensionRecordPage>();
                    var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
                    var row = suspensionGrid.Rows[0];

                    // Click delete button and verify delete options appear ( do not delete row)
                    row.ClickDeleteRow();
                    Assert.AreEqual(true, suspensionPage.IsYesNoDeleteButtonDisplayed(), "Delete options not displayed");
                    suspensionPage = suspensionPage.ConfirmDelete(row);

                    // Click delete button and verify delete options appear (delete row)
                    row.ClickDeleteRow();
                    Assert.AreEqual(true, suspensionPage.IsYesNoDeleteButtonDisplayed(), "Delete options not displayed");
                    suspensionPage = suspensionPage.ConfirmDelete(row, true);

                    //Verify row is removed
                    suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
                    Assert.AreEqual(null, suspensionGrid.Rows.FirstOrDefault(), "Row still exists");

                    // Exit page
                    var warningDialog = suspensionTriplet.ExistSuspensionPage();
                    warningDialog.DontSave();
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }


        /// <summary>
        /// Description: delete a pupil who has suspension record
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2, })]
        public void Suspensions_DeletePupil()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            var forename = "DeletePupil";
            var surname = Utilities.GenerateRandomString(10, "DeletePupil");
            string pupilName = String.Format("{0}, {1}", surname, forename);

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddBasicSuspension(learnerId, new DateTime(2016, 02, 01), new DateTime(2016, 02, 05), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                // Login
                SeleniumHelper.Login(LoginAs);

                // Navigate to delete pupil
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
                DeletePupilRecordTriplet deletePupilTriplet = new DeletePupilRecordTriplet();
                deletePupilTriplet.SearchCriteria.PupilName = pupilName;
                var deletePupilRecordPage = deletePupilTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<DeletePupilRecordPage>();
                deletePupilRecordPage.Delete();

                // Verify message displays.
                IWebElement _successMessage = SeleniumHelper.FindElement(SimsBy.AutomationId("status_success"));
                Assert.IsTrue(_successMessage.IsExist(), "Message success does not display");
            }
        }
    }
}
