using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.internals;
using NUnit.Framework;
using System.Globalization;
using TestSettings;
using POM.Components.Attendance;
using POM.Components.Pupil;
using POM.Components.Common;

namespace Attendance.DealWithSpecificMarks.Tests
{
    public class DealWithSpecificMarksTests
    {
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_ATT07_Data")]
        public void TC_ATT07_Validate_Deal_with_Chosen_Code_functionality(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup, string className,
            string academicYear, string searchMark)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

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
            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            //var pupilRecord = PupilRecordPage.Create();
            //pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Navigate to Deal with Chosen
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Deal with Specific Marks");
            var dealwithCodes = new DealWithSpecifcMarksTriplet();

            //Select Search Criteria
            dealwithCodes.SearchCriteria.SelectAcademicYear = academicYear;
            dealwithCodes.SearchCriteria.SelectMark = searchMark;
            dealwithCodes.SearchCriteria.YearGroup = "Year 2";
            var dealChosenCode = dealwithCodes.SearchCriteria.Search<DealWithSpecificMarkPage>();

            //Input by keyboard
            dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][1].Marks = "/";
            dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][2].Marks = "\\";
            dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][3].Marks = "/";

            //Input by drop-down
            dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][4].MarkDropDown = "A";
            dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][5].MarkDropDown = "C";

            //Save
            dealChosenCode.Save();

            //Verify input
            marksRow = dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)];
            Assert.AreEqual("/", marksRow[1].Marks, "Cannot input by keyboard");
            Assert.AreEqual("\\", marksRow[2].Marks, "Cannot input by keyboard");
            Assert.AreEqual("/", marksRow[3].Marks, "Cannot input by keyboard");
            Assert.AreEqual("A", marksRow[4].Marks, "Cannot input by dropdown");
            Assert.AreEqual("C", marksRow[5].Marks, "Cannot input by dropdown");
            Assert.AreEqual("A", marksRow[6].Marks, "Cannot input by code list");
            Assert.AreEqual("B", marksRow[7].Marks, "Cannot input by code list");
            Assert.AreEqual("C", marksRow[8].Marks, "Cannot input by code list");

            //Verify can edit in mode overwrite
            dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][1].MarkDropDown = "A";
            Assert.AreEqual("A", dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][1].Text, "Mark cannot be edit in overwrite mode by dropdown");
            dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][2].Select();
            dealChosenCode.Code = "B";
            Assert.AreEqual("B", dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][2].Text, "Mark cannot be edit in overwrite mode by select from code list");

            //Verify cannot edit with invalid value
            dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][3].Marks = "T";
            Assert.AreEqual("/", dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][3].Text, "Mark can be edit with invalid value by keyboard");

            //Add comment
            var addComment = dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][1].AddComment();
            addComment.Comment = "Test Comment";
            addComment.ClickOk();

            //Save
            dealChosenCode.Save();

            //Verify comment
            dealChosenCode = dealChosenCodes.SearchCriteria.Search<DealWithChosenPage>();
            addComment = dealChosenCode.Marks[String.Format("{0}, {1}", surName, foreName)][1].AddComment();
            Assert.AreEqual("Test Comment", addComment.Comment, "Can not add comment in Deal with Chosen code page");
            addComment.ClickCancel();

            //Verify mark of Edit Mark page
            SeleniumHelper.NavigateQuickLink("Edit Marks");
            var editMarkTriplet = new EditMarksTriplet();
            editMarkTriplet.SearchCriteria.Week = true;
            editMarkTriplet.SearchCriteria.SelectClass(className);
            editMarkTriplet.SearchCriteria.SelectYearGroup(yearGroup);
            var editMarks = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            var marks = editMarks.Marks[String.Format("{0}, {1}", surName, foreName)];
            Assert.AreEqual("A", marks[1].Text, "Change in Deal Chosen Screen is not record");
            Assert.AreEqual("B", marks[2].Text, "Change in Deal Chosen Screen is not record");
            Assert.AreEqual("/", marks[3].Text, "Change in Deal Chosen Screen is not record");
            Assert.AreEqual("A", marks[4].Text, "Change in Deal Chosen Screen is not record");
            Assert.AreEqual("C", marks[5].Text, "Change in Deal Chosen Screen is not record");
            Assert.AreEqual("A", marks[6].Text, "Change in Deal Chosen Screen is not record");
            Assert.AreEqual("B", marks[7].Text, "Change in Deal Chosen Screen is not record");
            Assert.AreEqual("C", marks[8].Text, "Change in Deal Chosen Screen is not record");

            #endregion
        }

    }
}
