using Attendance.POM.DataHelper;
using NUnit.Framework;
using POM.Components.Attendance;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using Selene.Support.Attributes;
using SeSugar.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using TestSettings;
using Attendance.POM.Entities;
using System.Linq;
using SeSugar.Automation;
using Attendance.Components.AttendancePages;


namespace Attendance.ApplyMarkOverDateRange.Tests
{
    public class ApplyMarkE2ETests
    {

        #region Preserve Functionality Of Apply Mark Over Date Range for already existing & Blank codes on the Register
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_001_Data")]
        public void VerifyPreserveFunctionalityOfApplyMarkOverDateRange(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string Mark, string mark1, string mark2, string dateOfBirth, string DateOfAdmission)
        {
            #region Data Preperation

            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);
            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.CreatePupil(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, yeargroup);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");
            Wait.WaitLoading();
            var editMarksTripletPage = new EditMarksTriplet();
            editMarksTripletPage.SearchCriteria.StartDate = startDate;
            editMarksTripletPage.SearchCriteria.Week = true;
            editMarksTripletPage.SearchCriteria.SelectYearGroup(yeargroup);
            var editMarksPage = editMarksTripletPage.SearchCriteria.Search<EditMarksPage>();

            editMarksPage.ModePreserve = false;
            editMarksPage.ModeHorizontal = true;

            var editmarkTable = editMarksPage.Marks;
            IEnumerable<SchoolAttendanceCode> attendanceNotRequiredCodes = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = attendanceNotRequiredCodes.Select(x => x.Code).ToList<string>();

            if (codes.Contains(editmarkTable[pupilName][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Attendance Not Required Codes");
                return;
            }

            var oldMarkAM = editmarkTable[pupilName][2].Text = mark1;
            var oldMarkPM = editmarkTable[pupilName][3].Text = mark2;
            var BlankMarkAM = editmarkTable[pupilName][4].Text = String.Empty;
            var BlankMarkPM = editmarkTable[pupilName][5].Text = String.Empty;

            editMarksPage.ClickSave();
            SeleniumHelper.CloseTab("Edit Marks");

            #endregion

            #region STEPS
            AttendanceNavigations.NavigateToApplyMarkOverDateRange();
            var applyMarkOverDateRange = new ApplyMarksOverDateRangeDialog();
            applyMarkOverDateRange.IsPreserve = true;
            applyMarkOverDateRange.StartDate = startDate;
            applyMarkOverDateRange.EndDate = endDate;
            applyMarkOverDateRange.SelectMark = Mark;

            // Click on Add Pupil link
            var addPupilsDialogTriplet = applyMarkOverDateRange.AddPupil();

            // Select some options
            addPupilsDialogTriplet.SearchCriteria.SearchByPupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var addPupilDialogSearchPage = addPupilsDialogTriplet.SearchCriteria;

            // Search and select a pupil
            var addPupilsDetailsPage = addPupilDialogSearchPage.Search<AddPupilsDetailsPage>();
            addPupilsDetailsPage.AddAllPupils();

            // Click on OK button
            addPupilsDialogTriplet.ClickOk();

            var confirmRequiredDialog1 = applyMarkOverDateRange.ClickApply();
            confirmRequiredDialog1.ClickApplyThisMark();
            applyMarkOverDateRange.ClosePatternDialog();

            //Navigate to Edit marks & Click on Search
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editmarkTriplet = new EditMarksTriplet();
            editmarkTriplet.SearchCriteria.StartDate = startDate;
            editmarkTriplet.SearchCriteria.Week = true;
            editmarkTriplet.SearchCriteria.SelectYearGroup(yeargroup);
            editMarksPage = editmarkTriplet.SearchCriteria.Search<EditMarksPage>();
            var editMarksTable1 = editMarksPage.Marks;

            //Verfiy Preserve functionality
            Assert.AreEqual(oldMarkAM, editMarksTable1[pupilName][2].Text, "For Existing records, Marks can't be overwritten in Preserve Mode");
            Assert.AreEqual(oldMarkPM, editMarksTable1[pupilName][3].Text, "For Existing records, Marks can't be overwritten in Preserve Mode");
            Assert.AreNotEqual(BlankMarkAM, editMarksTable1[pupilName][4].Text, "For Blank records, Mark can be overwritten in Preserve Mode");
            Assert.AreNotEqual(BlankMarkPM, editMarksTable1[pupilName][5].Text, "For Blank record, Mark can be overwritten in Preserve Mode");
            #endregion
        }
        #endregion

        #region Overwrite Functionality Of Apply Mark Over Date Range for already existing & Blank Codes on the Register
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_002_Data")]
        public void VerifyOverwriteFunctionalityOfApplyMarkOverDateRange(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string Mark, string mark1, string mark2, string dateOfBirth, string DateOfAdmission)
        {
            #region Data Preperation

            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);
            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.CreatePupil(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, yeargroup);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");
            Wait.WaitLoading();

            var editMarksTripletPage = new EditMarksTriplet();
            editMarksTripletPage.SearchCriteria.StartDate = startDate;
            editMarksTripletPage.SearchCriteria.Week = true;
            editMarksTripletPage.SearchCriteria.SelectYearGroup(yeargroup);
            
            var editMarksPage = editMarksTripletPage.SearchCriteria.Search<EditMarksPage>();

            editMarksPage.ModePreserve = false;
            editMarksPage.ModeHorizontal = true;

            var editmarkTable = editMarksPage.Marks;
            IEnumerable<SchoolAttendanceCode> attendanceNotRequiredCodes = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = attendanceNotRequiredCodes.Select(x => x.Code).ToList<string>();

            if (codes.Contains(editmarkTable[pupilName][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Attendance Not Required Codes");
                return;
            }

            var oldMarkAM = editmarkTable[pupilName][2].Text = mark1;
            var oldMarkPM = editmarkTable[pupilName][3].Text = mark2;
            var BlankMarkAM = editmarkTable[pupilName][4].Text = String.Empty;
            var BlankMarkPM = editmarkTable[pupilName][5].Text = String.Empty;

            editMarksPage.ClickSave();
            SeleniumHelper.CloseTab("Edit Marks");

            #endregion

            #region STEPS
            AttendanceNavigations.NavigateToApplyMarkOverDateRange();
            var applyMarkOverDateRange = new ApplyMarksOverDateRangeDialog();
            applyMarkOverDateRange.IsOverwrite = true;
            applyMarkOverDateRange.StartDate = startDate;
            applyMarkOverDateRange.EndDate = endDate;
            applyMarkOverDateRange.SelectMark = Mark;

            // Click on Add Pupil link
            var addPupilsDialogTriplet = applyMarkOverDateRange.AddPupil();

            // Select some options
            addPupilsDialogTriplet.SearchCriteria.SearchByPupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var addPupilDialogSearchPage = addPupilsDialogTriplet.SearchCriteria;

            // Search and select a pupil
            var addPupilsDetailsPage = addPupilDialogSearchPage.Search<AddPupilsDetailsPage>();
            addPupilsDetailsPage.AddAllPupils();

            // Click on OK button
            addPupilsDialogTriplet.ClickOk();

            var confirmRequiredDialog1 = applyMarkOverDateRange.ClickApply();
            confirmRequiredDialog1.ClickApplyThisMark();
            applyMarkOverDateRange.ClosePatternDialog();

            //Navigate to Edit marks & Click on Search
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editmarkTriplet = new EditMarksTriplet();
            editmarkTriplet.SearchCriteria.StartDate = startDate;
            editmarkTriplet.SearchCriteria.Week = true;
            editmarkTriplet.SearchCriteria.SelectYearGroup(yeargroup);
            editMarksPage = editmarkTriplet.SearchCriteria.Search<EditMarksPage>();
            var editMarksTable1 = editMarksPage.Marks;

            Assert.AreNotEqual(oldMarkAM, editMarksTable1[pupilName][2].Text, "On Existing records, Marks can be overwritten in Overwrite Mode");
            Assert.AreNotEqual(oldMarkPM, editMarksTable1[pupilName][3].Text, "On Existing records, Marks can be overwritten in Overwrite Mode");
            Assert.AreNotEqual(BlankMarkAM, editMarksTable1[pupilName][4].Text, "On Blank records, Mark can be overwritten in Overwrite Mode");
            Assert.AreNotEqual(BlankMarkPM, editMarksTable1[pupilName][5].Text, "On Blank record, Mark can be overwritten in Overwrite Mode");
            #endregion
        }
        #endregion

        #region Data
        public List<object[]> TC_ATT_001_Data()
        {
            string pattern = "M/d/yyyy";
            string pupilSurName = "TC_ATT_001_Data" + SeleniumHelper.GenerateRandomString(8);
            string pupilForeName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();
            string endDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).AddDays(3).ToShortDateString();
            string dateOfBirth = DateTime.ParseExact("10/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //Guid yeargroupid = new Guid();
            string mark1 = "H";
            string mark2 = "H";
            string Mark = "A";

            var data = new List<Object[]>
            {
                new object[] { startDate, endDate, "Year 1", pupilForeName, pupilSurName, pupilName, Mark, mark1, mark2, dateOfBirth, DateOfAdmission }
            };
            return data;
        }

        public List<object[]> TC_ATT_002_Data()
        {
            string pattern = "M/d/yyyy";
            string pupilSurName = "TC_ATT_002_Data" + SeleniumHelper.GenerateRandomString(8);
            string pupilForeName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();
            string endDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).AddDays(3).ToShortDateString();
            string dateOfBirth = DateTime.ParseExact("10/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //Guid yeargroupid = new Guid();
            string mark1 = "H";
            string mark2 = "H";
            string Mark = "A";

            var data = new List<Object[]>
            {
                new object[] { startDate, endDate, "Year 1", pupilForeName, pupilSurName, pupilName, Mark, mark1, mark2, dateOfBirth, DateOfAdmission }
            };
            return data;
        }
        #endregion

    }
}
