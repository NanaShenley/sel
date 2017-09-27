using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using POM.Components.Attendance;
using POM.Components.Common;
using POM.Components.Conduct;
using POM.Components.Pupil;
using POM.Helper;

namespace Pupil.Conduct.Tests
{
    public class DisabledTests
    {

        /// <summary>
        /// TC_CON02
        /// Au : Hieu Pham
        /// Description: Exercise ability to add information to a 'Suspension' for a pupil that has a partially recorded suspension record.
        /// </summary>
        //TODO: TC_CON03 covers this scenario. Hence P3
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority4 }, DataProvider = "TC_CON02_DATA")]
        public void TC_CON02_Update_Suspension(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string YearGroup, string className, string pupilName, string status, string date, string reasonForChange, string meetingType,
            string startDate, string startTime, string preType, string preReason, string preStartDate, string preEndDate,
            string preStartTime, string preEndTime, string preLength, string preNote)
        {
            #region Pre-Conditions

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
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Navigate to suspension
            SeleniumHelper.NavigateMenu("Tasks", "Conduct", "Suspensions");

            // Delete row if exist
            var suspensionTriplet = new SuspensionTriplet();
            suspensionTriplet.SearchCriteria.PupilName = pupilName;
            var suspensionPage = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();
            var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var rows = suspensionGrid.Rows;
            suspensionPage = suspensionPage.ClickDeleteAllRow(rows);

            // Click add new record
            var addSuspensionDialog = suspensionPage.ClickAddNewRecord();

            // Enter values
            addSuspensionDialog.Type = preType;
            addSuspensionDialog.Reason = preReason;
            addSuspensionDialog.StartDate = preStartDate;
            addSuspensionDialog.EndDate = preEndDate;
            addSuspensionDialog.StartTime = preStartTime;
            addSuspensionDialog.EndTime = preEndTime;
            addSuspensionDialog.Lenght = preLength;
            addSuspensionDialog.Note = preNote;
            suspensionPage = addSuspensionDialog.SaveValue();
            suspensionPage.SaveValues();

            // Search suspension record again
            suspensionTriplet.SearchCriteria.PupilName = pupilName;
            suspensionPage = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();
            suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var row = suspensionGrid.Rows.FirstOrDefault(x => x.Type.Equals(preType) && x.Reason.Equals(preReason));

            #endregion

            #region Steps:

            // Click edit record
            var editDialog = row.ClickEditRecord();
            var statusGrid = editDialog.StatusGrid;

            // Enter values for status table
            var rowEmpty = statusGrid.Rows.FirstOrDefault(x => x.Date.Trim().Equals(String.Empty));
            rowEmpty.Status = status;
            rowEmpty.Date = date;
            rowEmpty.Reason = reasonForChange;

            // Click add meeting
            var meetingDialog = editDialog.AddMeeting();
            meetingDialog.Type = meetingType;
            meetingDialog.StartDate = startDate;
            meetingDialog.StartTime = startTime;
            editDialog = meetingDialog.SaveValue();
            suspensionPage = editDialog.SaveValue();
            suspensionPage.SaveValues();

            // Verify record is saved.
            suspensionTriplet.SearchCriteria.PupilName = pupilName;
            suspensionPage = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();
            suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            row = suspensionGrid.Rows.FirstOrDefault();
            editDialog = row.ClickEditRecord();
            statusGrid = editDialog.StatusGrid;
            var meetingGrid = editDialog.MeetingGrid;
            var rowStatusGrid = statusGrid.Rows.FirstOrDefault(x => x.Status.Equals(status) && x.Date.Equals(date) && x.Reason.Equals(reasonForChange));
            var rowMeetingGrid = meetingGrid.Rows.FirstOrDefault(x => x.Type.Equals(meetingType));
            Assert.AreNotEqual(null, rowStatusGrid, "Can not update status table");
            Assert.AreNotEqual(null, rowMeetingGrid, "Can not update meeting table");

            // Close dialog
            editDialog.Cancel();

            #endregion

            #region Post Condition

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(pupilName));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

       
        /// <summary>
        /// TC_CON07
        /// Au : Hieu Pham
        /// Description: Exercise ability to locate (search and select) existing 'Suspension' records by a given suspension 'Type'.
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_CON07_DATA")]
        public void TC_CON07_Adminstrator_Search_Select(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string YearGroup, string pupilName, string type, string length, string endDate,
            string firstStartDate, string secondStartDate, string thirdStartDate, string className, string preType, string preReason,
            string preStartDate, string preEndDate, string preStartTime, string preEndTime, string preLength, string preNote)
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
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Navigate to suspension record
            SeleniumHelper.NavigateBySearch("Suspensions", true);

            // Delete row if exist
            var suspensionTriplet = new SuspensionTriplet();
            suspensionTriplet.SearchCriteria.PupilName = pupilName;
            var suspensionPage = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();
            var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var rows = suspensionGrid.Rows;
            suspensionPage = suspensionPage.ClickDeleteAllRow(rows);

            // Click add new record
            var addSuspensionDialog = suspensionPage.ClickAddNewRecord();

            // Enter values
            addSuspensionDialog.Type = preType;
            addSuspensionDialog.Reason = preReason;
            addSuspensionDialog.StartDate = preStartDate;
            addSuspensionDialog.EndDate = preEndDate;
            addSuspensionDialog.StartTime = preStartTime;
            addSuspensionDialog.EndTime = preEndTime;
            addSuspensionDialog.Lenght = preLength;
            addSuspensionDialog.Note = preNote;
            suspensionPage = addSuspensionDialog.SaveValue();
            suspensionPage.SaveValues();
            Assert.AreEqual(true, suspensionPage.IsSuccessMessageDisplay(), "Success message does not display");

            #endregion

            #region Steps:

            // Search record by 'Suspension Type'
            suspensionTriplet = new SuspensionTriplet();
            suspensionTriplet.SearchCriteria.IsCurrent = false;
            suspensionTriplet.SearchCriteria.IsFuture = false;
            suspensionTriplet.SearchCriteria.IsLeaver = false;
            suspensionTriplet.SearchCriteria.ClickShowMore();
            suspensionTriplet.SearchCriteria.IsNewSuspension = true;
            var result = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName));

            //Verify result contains pupil.
            Assert.AreNotEqual(null, result, "Result does not contain pupil");
            suspensionPage = result.Click<SuspensionRecordPage>();
            suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var row = suspensionGrid.Rows.FirstOrDefault(x => x.Type.Equals(preType));
            Assert.AreNotEqual(null, row, "Record is not exist. Table is empty or values is not correct");

            // Edit values in suspension record
            var editDialog = row.ClickEditRecord();
            editDialog.Type = type;
            editDialog.Lenght = length;
            editDialog.EndDate = endDate;
            suspensionPage = editDialog.SaveValue();
            suspensionPage = suspensionPage.SaveValues();

            // Navigate to Edit mark
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Edit Marks");
            var editMarkTriplet = new EditMarksTriplet();

            // Verify Edit mark of first week
            editMarkTriplet.SearchCriteria.StartDate = firstStartDate;
            editMarkTriplet.SearchCriteria.Week = true;
            editMarkTriplet.SearchCriteria.SelectClass(className);
            var editMarkPage = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            var markTable = editMarkPage.Marks;
            Assert.AreEqual(5, markTable[pupilName].ListCells.Values.Count(t => t.Text.Equals("C")) / 2, "Number of marks of first week is incorrect");

            // Verify Edit mark of second week
            editMarkTriplet.SearchCriteria.StartDate = secondStartDate;
            editMarkTriplet.SearchCriteria.Week = true;
            editMarkTriplet.SearchCriteria.SelectClass(className);
            editMarkPage = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            markTable = editMarkPage.Marks;
            Assert.AreEqual(5, markTable[pupilName].ListCells.Values.Count(t => t.Text.Equals("C")) / 2, "Number of marks of second week is incorrect");

            // Verify Edit mark of third week
            editMarkTriplet.SearchCriteria.StartDate = thirdStartDate;
            editMarkTriplet.SearchCriteria.Week = true;
            editMarkTriplet.SearchCriteria.SelectClass(className);
            editMarkPage = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            markTable = editMarkPage.Marks;
            Assert.AreEqual(5, markTable[pupilName].ListCells.Values.Count(t => t.Text.Equals("C")) / 2, "Number of marks of third week is incorrect");

            // Return suspension record page
            SeleniumHelper.NavigateMenu("Tasks", "Conduct", "Suspensions");
            suspensionTriplet = new SuspensionTriplet();
            suspensionTriplet.SearchCriteria.ClickShowMore();
            suspensionTriplet.SearchCriteria.IsNewSuspension = true;
            result = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName));

            // Verify result is empty
            Assert.AreEqual(null, result, "Result is not empty.");

            suspensionTriplet.SearchCriteria.IsNewSuspension = false;
            suspensionTriplet.SearchCriteria.IsContinueSuspension = true;
            result = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName));

            // Verify result is not empty
            Assert.AreNotEqual(null, result, "Result is not empty.");

            #endregion

            #region Post Condition

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(pupilName));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }



        /// <summary>
        /// TC_CON09
        /// Au : Hieu Pham
        /// Description: Exercise ability to add a document to a meeting record that is recorded on a suspension (expulsion) record.
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_CON09_DATA")]
        public void TC_CON09_Adminstator_AddDocument(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string YearGroup, string className, string pupilName, string type, string reason, string startDate,
            string startTime, string meetingType, string meetingStartDate, string summary, string note)
        {
            #region Pre-Conditions

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
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Navigate to suspension
            SeleniumHelper.NavigateMenu("Tasks", "Conduct", "Suspensions");

            // Search and select pupil
            var suspensionTriplet = new SuspensionTriplet();
            suspensionTriplet.SearchCriteria.PupilName = pupilName;
            var suspensionPage = suspensionTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();

            // Click add new record
            var addSuspensionDialog = suspensionPage.ClickAddNewRecord();

            // Enter values
            addSuspensionDialog.Type = type;
            addSuspensionDialog.Reason = reason;
            addSuspensionDialog.StartDate = startDate;
            addSuspensionDialog.StartTime = startTime;

            // Add meeting 
            var meetingDialog = addSuspensionDialog.AddMeeting();
            meetingDialog.Type = meetingType;
            meetingDialog.StartDate = meetingStartDate;

            // Add Note and Document
            var noteDocumentGrid = meetingDialog.NoteDocumentGrid;
            var emptyNoteRow = noteDocumentGrid.GetLastRow();
            emptyNoteRow.Summary = summary;
            emptyNoteRow.Note = note;

            // Save meeting
            addSuspensionDialog = meetingDialog.SaveValue();

            // Save values
            suspensionPage = addSuspensionDialog.SaveValue();
            suspensionPage.SaveValues();

            #endregion

            #region Steps

            // Search suspension record again
            suspensionTriplet.SearchCriteria.PupilName = pupilName;
            suspensionPage = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();
            var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var row = suspensionGrid.Rows.FirstOrDefault(x => x.Type.Equals(type) && x.Reason.Equals(reason));

            // Edit the suspension record
            var editDialog = row.ClickEditRecord();

            // Edit Meeting
            var meetingGrid = editDialog.MeetingGrid;
            var rowMeeting = meetingGrid.Rows.FirstOrDefault(x => x.Type.Equals(meetingType));
            meetingDialog = rowMeeting.EditMeeting();
            noteDocumentGrid = meetingDialog.NoteDocumentGrid;

            // Click document icon control
            Assert.AreEqual(true, false, "Issue : Can not continue implementing test script because can not find 'Document' icon control.");

            #endregion

            #region Post Condition

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(pupilName));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion

        }


        /// <summary>
        /// TC_CON10
        /// Au : Hieu Pham
        /// Description: Exercise ability to invoke the 'Pupil Record' screen for the pupil with a suspension (expulsion) record directly via a contextual link within the 'Suspension Records' screen.
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_CON10_DATA")]
        public void TC_CON10_Adminstator_Invoke_Pupil_Record(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string YearGroup, string className, string pupilName, string preType, string preReason,
            string preStartDate, string preStartTime)
        {
            #region Pre-Conditions

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
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Navigate to suspension
            SeleniumHelper.NavigateMenu("Tasks", "Conduct", "Suspensions");

            // Delete row if exist
            var suspensionTriplet = new SuspensionTriplet();
            suspensionTriplet.SearchCriteria.PupilName = pupilName;
            var suspensionPage = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();
            var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var rows = suspensionGrid.Rows;
            suspensionPage = suspensionPage.ClickDeleteAllRow(rows);

            // Click add new record
            var addSuspensionDialog = suspensionPage.ClickAddNewRecord();

            // Enter values
            addSuspensionDialog.Type = preType;
            addSuspensionDialog.Reason = preReason;
            addSuspensionDialog.StartDate = preStartDate;
            addSuspensionDialog.StartTime = preStartTime;
            suspensionPage = addSuspensionDialog.SaveValue();
            suspensionPage.SaveValues();
            Assert.AreEqual(true, suspensionPage.IsSuccessMessageDisplay(), "Success message does not display");

            // Search suspension record again
            suspensionTriplet.SearchCriteria.PupilName = pupilName;
            suspensionPage = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();
            suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var row = suspensionGrid.Rows.FirstOrDefault(x => x.Type.Equals(preType) && x.Reason.Equals(preReason));
            Assert.AreNotEqual(null, row, "Suspension record is not existed");

            #endregion

            #region Steps

            // Navigate to pupil record by contextual link
            var pupilRecordPage = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Details");

            // Verify pupil detail displays
            Assert.AreEqual(pupilName, pupilRecordPage.PreferSurname + ", " + pupilRecordPage.PreferForeName, "Detail tab does not display");

            // Close suspension tab
            SeleniumHelper.CloseTab("Suspensions");

            // Verify tab Suspension is not displayed.
            Assert.AreEqual(false, SeleniumHelper.IsTabDisplay("Suspensions"), "Suspension page still displays");
            Assert.AreEqual(true, SeleniumHelper.IsTabDisplay("Pupil Record"), "Pupil record tab is not displayed");

            // Click contextual link to navigate to suspension
            suspensionPage = SeleniumHelper.NavigateViaAction<SuspensionRecordPage>("Suspensions and Expulsions");
            suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            row = suspensionGrid.Rows.FirstOrDefault(x => x.Type.Equals(preType));
            Assert.AreNotEqual(null, row, "Record is not exist. Table is empty or values is not correct");

            // Close the pupil record tab
            SeleniumHelper.CloseTab("Pupil Record");

            // Verify pupil record page is no longer displayed and only suspension record displays.
            Assert.AreEqual(true, SeleniumHelper.IsTabDisplay("Suspensions"), "Suspension page still is not displayed");
            Assert.AreEqual(false, SeleniumHelper.IsTabDisplay("Pupil Record"), "Pupil record tab still displays");

            // Navigate to pupil record by contextual link
            pupilRecordPage = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Details");

            // Verify pupil detail displays
            Assert.AreEqual(pupilName, pupilRecordPage.PreferSurname + ", " + pupilRecordPage.PreferForeName, "Detail tab does not display");

            // Close tab
            SeleniumHelper.CloseTab("Suspensions");
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Post Condition

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(pupilName));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion

        }

        /// <summary>
        /// TC_CON11
        /// Au : Hieu Pham
        /// Description: Exercise ability to delete a suspension (expulsion) record so allowing a pupil to continue education at the school.
        /// </summary>
        /// //TODO: Duplicate with TC_CON12
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority4 }, DataProvider = "TC_CON11_DATA")]
        public void TC_CON11_Adminstator_Delete_Suspension_Record(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string YearGroup, string className, string pupilName, string preType, string preReason, string preStartDate,
            string preStartTime)
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
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Navigate to suspension
            SeleniumHelper.NavigateMenu("Tasks", "Conduct", "Suspensions");

            // Delete row if exist
            var suspensionTriplet = new SuspensionTriplet();
            suspensionTriplet.SearchCriteria.PupilName = pupilName;
            var suspensionPage = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();
            var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var rows = suspensionGrid.Rows;
            suspensionPage = suspensionPage.ClickDeleteAllRow(rows);

            // Click add new record
            var addSuspensionDialog = suspensionPage.ClickAddNewRecord();

            // Enter values
            addSuspensionDialog.Type = preType;
            addSuspensionDialog.Reason = preReason;
            addSuspensionDialog.StartDate = preStartDate;
            addSuspensionDialog.StartTime = preStartTime;
            suspensionPage = addSuspensionDialog.SaveValue();
            suspensionPage.SaveValues();
            Assert.AreEqual(true, suspensionPage.IsSuccessMessageDisplay(), "Success message does not display");

            #endregion

            #region Steps:


            // Search and select pupil
            suspensionTriplet = new SuspensionTriplet();
            suspensionTriplet.SearchCriteria.IsCurrent = true;
            suspensionTriplet.SearchCriteria.ClickShowMore();
            suspensionTriplet.SearchCriteria.IsExpulsion = true;
            var searchResult = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName));
            // Verify search result has pupil.
            Assert.AreNotEqual(null, searchResult, "Pupil is not existed");

            // Select the pupil 
            suspensionPage = searchResult.Click<SuspensionRecordPage>();
            suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var row = suspensionGrid.Rows.FirstOrDefault(x => x.Type.Equals(preType));
            Assert.AreNotEqual(null, row, "Record is not exist. Table is empty or values is not correct");

            // Delete the suspension
            row.DeleteRow();
            suspensionPage = suspensionPage.SaveValues();

            // Verify deleted
            suspensionTriplet = new SuspensionTriplet();
            suspensionTriplet.SearchCriteria.PupilName = pupilName;
            suspensionTriplet.SearchCriteria.IsExpulsion = false;
            suspensionPage = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();
            suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            row = suspensionGrid.Rows.FirstOrDefault(x => x.Type.Equals(preType));
            Assert.AreEqual(null, row, "Delete suspension failed");

            #endregion

            #region Post Condition

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(pupilName));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion

        }


        /// <summary>
        /// TC_CON13
        /// Au : Hieu Pham
        /// Description: Exercise ability to record multiple suspension records against a single pupil as long as they do not overlap.
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_CON13_DATA")]
        public void TC_CON13_Senior_Team_Record_Multi_Suspension(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string YearGroup, string className, string pupilName, string firstType, string firstStartDate, string firstEndDate, string secondType,
            string secondReason, string secondStartDate, string secondStartTime, string secondNote, string oldType, string oldReason, string oldStartDate, string oldEndDate,
            string oldStartTime, string oldEndTime, string oldLength, string oldNote)
        {
            #region Pre-Conditions

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
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Logout
            SeleniumHelper.Logout();

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam);

            // Navigate to suspension
            SeleniumHelper.NavigateBySearch("Suspensions", true);

            // Search and select pupil
            SuspensionTriplet suspensionTriplet = new SuspensionTriplet();
            suspensionTriplet.SearchCriteria.IsCurrent = true;
            suspensionTriplet.SearchCriteria.PupilName = pupilName;

            // Select the pupil 
            var suspensionPage = suspensionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<SuspensionRecordPage>();

            // Delete all record
            var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var rows = suspensionGrid.Rows;
            suspensionPage = suspensionPage.ClickDeleteAllRow(rows);
            suspensionPage = suspensionPage.SaveValues();

            // Click add new suspension
            var addNewSuspensionDialog = suspensionPage.ClickAddNewRecord();

            // Fill values
            addNewSuspensionDialog.Type = oldType;
            addNewSuspensionDialog.Reason = oldReason;
            addNewSuspensionDialog.StartDate = oldStartDate;
            addNewSuspensionDialog.EndDate = oldEndDate;
            addNewSuspensionDialog.StartTime = oldStartTime;
            addNewSuspensionDialog.EndTime = oldEndTime;
            addNewSuspensionDialog.Lenght = oldLength;
            addNewSuspensionDialog.Note = oldNote;

            // Save values
            suspensionPage = addNewSuspensionDialog.SaveValue();
            suspensionPage = suspensionPage.SaveValues();

            // Click add new suspension
            addNewSuspensionDialog = suspensionPage.ClickAddNewRecord();

            // Fill values
            addNewSuspensionDialog.Type = oldType;
            addNewSuspensionDialog.Reason = oldReason;
            addNewSuspensionDialog.StartDate = oldStartDate;
            addNewSuspensionDialog.EndDate = oldEndDate;
            addNewSuspensionDialog.StartTime = oldStartTime;
            addNewSuspensionDialog.EndTime = oldEndTime;
            addNewSuspensionDialog.Lenght = oldLength;
            addNewSuspensionDialog.Note = oldNote;

            // Save values
            suspensionPage = addNewSuspensionDialog.SaveValue();
            suspensionPage = suspensionPage.SaveValues();

            // Verify date overlap message is displayed
            Assert.AreEqual(true, suspensionPage.IsOverlapMessageDisplayed(), "Date overlap message is not displayed.");

            #endregion

            #region Steps

            // Edit record at the bottom at grid
            suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            var indexLastRow = suspensionGrid.Rows.Count;
            var lastRow = suspensionGrid.Rows[indexLastRow - 1];
            var editDialog = lastRow.ClickEditRecord();
            editDialog.Type = firstType;
            editDialog.StartDate = firstStartDate;
            editDialog.EndDate = firstEndDate;
            // Save values
            suspensionPage = editDialog.SaveValue();
            suspensionPage = suspensionPage.SaveValues();

            // Add new suspension
            addNewSuspensionDialog = suspensionPage.ClickAddNewRecord();
            addNewSuspensionDialog.Type = secondType;
            addNewSuspensionDialog.Reason = secondReason;
            addNewSuspensionDialog.StartDate = secondStartDate;
            addNewSuspensionDialog.StartTime = secondStartTime;
            addNewSuspensionDialog.Note = secondNote;

            // Save values
            suspensionPage = addNewSuspensionDialog.SaveValue();
            suspensionPage = suspensionPage.SaveValues();

            // Verify table has 3 rows
            suspensionGrid = suspensionPage.SuspensionExpulsionGrid;
            Assert.AreEqual(3, suspensionGrid.Rows.Count, "Table has number of row incorrect");

            // Verify rows are sorted.
            Assert.AreEqual(0, suspensionGrid.Rows.FirstOrDefault(x => x.Type.Trim().Equals(secondType)).IndexOfRow(suspensionGrid), "Expulsion suspension 's index is not correct");
            Assert.AreEqual(1, suspensionGrid.Rows.FirstOrDefault(x => x.Type.Trim().Equals(firstType)).IndexOfRow(suspensionGrid), "Continue suspension 's index is not correct");
            Assert.AreEqual(2, suspensionGrid.Rows.FirstOrDefault(x => x.Type.Trim().Equals(oldType)).IndexOfRow(suspensionGrid), "New suspension 's index is not correct");

            // Close tab
            SeleniumHelper.CloseTab("Suspensions");

            // Logout
            SeleniumHelper.Logout();

            #endregion

            #region Post Condition

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(pupilName));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }


        #region DATA

        public List<object[]> TC_CON01_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);
            string endDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", pupilName, "New Suspension", "Substance Abuse", startDate
                    , endDate, "8:30 AM", "4:45 PM", "5", "Substance Abuse whilst on school premises.", "SEN" }

            };
            return data;
        }

        public List<object[]> TC_CON02_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string date = SeleniumHelper.GetToDay();
            string startDate = SeleniumHelper.GetToDay();
            string preStartDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);
            string preEndDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "Parent/Governors Notified", date, "Head Teacher's Decision"
                    , "Discipline Committee", startDate, "11:00", "New Suspension", "Substance Abuse", preStartDate
                    , preEndDate, "8:30 AM", "4:45 PM", "5", "Substance Abuse whilst on school premises." }

            };
            return data;
        }

        public List<object[]> TC_CON03_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday), 7);
            string date = SeleniumHelper.GetToDay();
            string startDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Tuesday, true);
            string startDateBefore = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);
            string startDateAfter = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday), 7);
            string preStartDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);
            string preEndDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", pupilName, endDate, "10", "Appeal Requested", date, "Parent Request"
                    , "Appeal Tribunal", startDate, "11:30" , "SEN", startDateBefore, startDateAfter, "New Suspension", "Substance Abuse", preStartDate
                    , preEndDate, "8:30 AM", "4:45 PM", "5", "Substance Abuse whilst on school premises."}

            };
            return data;
        }

        public List<object[]> TC_CON04_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday), 7);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "New Suspension", "Substance Abuse", "10", "Abused Substance Details", "Alprazolam",
                startDate, endDate, "8:30 AM", "4:45 PM", "Substance Abuse whilst on school premises."}

            };
            return data;
        }

        public List<object[]> TC_CON05_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);
            string endDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "New Suspension", "Substance Abuse",
                    startDate, endDate, "8:30 AM", "4:45 PM", "5", "Substance Abuse whilst on school premises."}

            };
            return data;
        }

        public List<object[]> TC_CON06_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday), 7);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "New Suspension",
                    "Substance Abuse", startDate, endDate , "8:30 AM", "4:45 PM", "10", "Substance Abuse whilst on school premises." }

            };
            return data;
        }

        public List<object[]> TC_CON07_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday), 14);
            string firstStartDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);
            string secondStartDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday), 7);
            string thirdStartDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday), 14);
            string preStartDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);
            string preEndDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday), 7);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", pupilName, "Continued Suspension", "15", endDate,
                    firstStartDate, secondStartDate, thirdStartDate, "SEN","New Suspension","Substance Abuse", preStartDate, preEndDate,
                    "8:30 AM", "4:45 PM", "10", "Substance Abuse whilst on school premises."
                }

            };
            return data;
        }

        public List<object[]> TC_CON08_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetToDay();
            string statusDate = SeleniumHelper.GetToDay();
            string meetingStartDate = SeleniumHelper.GetToDay();
            string preStartDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);
            string preEndDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday), 14);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "Continued Suspension", "Expulsion", "Physical Attack on Pupil", startDate, "Exclusion Upheld",
                statusDate, "Outcome of Appeal Tribunal", "Appeal Tribunal", meetingStartDate , "Expulsion Authorised", "Expulsion effective immediately",
                "Continued Suspension","Substance Abuse", preStartDate, preEndDate , "8:30 AM", "4:45 PM", "15", "Substance Abuse whilst on school premises."}

            };
            return data;
        }


        public List<object[]> TC_CON09_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetToDay();
            string meetingStartDate = SeleniumHelper.GetToDay();


            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "Expulsion", "Physical Attack on Pupil", startDate, "08:00 AM",
                "Appeal Tribunal", meetingStartDate , "Expulsion Authorised", "Expulsion effective immediately",
                }

            };
            return data;
        }

        public List<object[]> TC_CON10_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string preStartDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName,
                "Expulsion","Physical Attack on Pupil", preStartDate, "8:30 AM" }

            };
            return data;
        }

        public List<object[]> TC_CON11_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName,
                 "Expulsion","Physical Attack on Pupil", startDate , "8:30 AM"}

            };
            return data;
        }

        public List<object[]> TC_CON12_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetToDay();
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 7);


            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "New Suspension", "Disruptive Behaviour in Class",
                startDate, endDate, "8:15 AM", "5:15 PM", "5", "Disruptive behaviour whilst in the art class."}

            };
            return data;
        }

        public List<object[]> TC_CON13_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string firstStartDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 8);
            string firstEndDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 14);
            string secondStartDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 15);
            string oldStartDate = SeleniumHelper.GetToDay();
            string oldEndDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 7);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "Continued Suspension", firstStartDate, firstEndDate,
                    "Expulsion", "Physical Attack on Staff", secondStartDate, "8:00 AM", "Physical attack on art teacher and assistant.",
                    "New Suspension", "Disruptive Behaviour in Class", oldStartDate,
                     oldEndDate, "8:15 AM", "5:15 PM", "5", "Disruptive behaviour whilst in the art class."}

            };
            return data;
        }

        public List<object[]> TC_CON14_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string firstStartDate = SeleniumHelper.GetToDay();
            string firstEndDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 7);
            string secondStartDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 8);
            string secondEndDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 14);
            string thirdStartDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 15);

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName,
                    "New Suspension", "Disruptive Behaviour in Class", firstStartDate,
                    firstEndDate, "8:15 AM", "5:15 PM", "5", "Disruptive behaviour whilst in the art class.",
                    "Continued Suspension", "Stealing", secondStartDate, secondEndDate,
                    "8:15 AM", "5:15 PM", "5", "Disruptive behaviour whilst in the art class.",
                    "Expulsion", "Physical Attack on Staff", thirdStartDate,"8:00 AM"
                    }

            };
            return data;
        }
        #endregion
    }
}
