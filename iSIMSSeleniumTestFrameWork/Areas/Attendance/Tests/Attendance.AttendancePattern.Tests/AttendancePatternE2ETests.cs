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

namespace Attendance.AttendancePattern.Tests
{

    public class AttendancePatternE2ETests
    {
        #region Preserve Functionality Of Attendance Pattern for already existing & Blank codes on Register
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_001_Data")]
        public void VerifyPreserveFunctionalityOfAttendancePattern(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string MonAM, string MonPM, string TueAM, string TuePM, string mark1, string mark2, string dateOfBirth, string DateOfAdmission)
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

            var oldMarkAM= editmarkTable[pupilName][2].Text = mark1;
            var oldMarkPM = editmarkTable[pupilName][3].Text = mark2;
            var BlankMarkAM=  editmarkTable[pupilName][4].Text = String.Empty;
            var BlankMarkPM= editmarkTable[pupilName][5].Text = String.Empty;

            editMarksPage.ClickSave();
            SeleniumHelper.CloseTab("Edit Marks");

            #endregion

            #region STEPS
            AttendanceNavigations.NavigateToAttendancePattern();          
            var attendancePattern = new AttendancePatternDialog();
            attendancePattern.IsPreserve = true;
            attendancePattern.StartDate = startDate;
            attendancePattern.EndDate = endDate;

            // Click on Add Pupil link
            var addPupilsDialogTriplet = attendancePattern.AddPupil();

            // Select some options
            addPupilsDialogTriplet.SearchCriteria.SearchByPupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var addPupilDialogSearchPage = addPupilsDialogTriplet.SearchCriteria;

            // Search and select a pupil
            var addPupilsDetailsPage = addPupilDialogSearchPage.Search<AddPupilsDetailsPage>();
            addPupilsDetailsPage.AddAllPupils();

            // Click on OK button
            addPupilsDialogTriplet.ClickOk();

            // Add data into Pattern table
            var patternRow = attendancePattern.Table[0];
            patternRow.MonAM = MonAM;
            patternRow.MonPM = MonPM;
            patternRow.TueAM = TueAM;
            patternRow.TuePM = TuePM;

            var confirmRequiredDialog1 = attendancePattern.ClickApply();
            confirmRequiredDialog1.ClickApplyThisPattern();
            attendancePattern.ClosePatternDialog();

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

        #region Overwrite Functionality Of Attendance Pattern for already existing & Blank Codes on the Register
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome}, DataProvider = "TC_ATT_002_Data")]
        public void VerifyOverwriteFunctionalityOfAttendancePattern(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string MonAM, string MonPM, string TueAM, string TuePM, string mark1, string mark2, string dateOfBirth, string DateOfAdmission)
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
            //editMarksTripletPage.SearchCriteria.SelectClass(className);
            var editMarksPage = editMarksTripletPage.SearchCriteria.Search<EditMarksPage>();

            editMarksPage.ModePreserve = false;
            editMarksPage.ModeHorizontal = true;

            var editmarkTable = editMarksPage.Marks;
            IEnumerable<SchoolAttendanceCode> attendanceNotRequiredCodes = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = attendanceNotRequiredCodes.Select(x => x.Code).ToList<string>();

            if (codes.Contains(editmarkTable[pupilName][2].Text))
            {
                //Assert.IsTrue(true, "Marks can't be overwritten on Attendance Not Required Codes");
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
            AttendanceNavigations.NavigateToAttendancePattern(); 
            var attendancePattern = new AttendancePatternDialog();
            attendancePattern.IsOverwrite = true;
            attendancePattern.StartDate = startDate;
            attendancePattern.EndDate = endDate;

            // Click on Add Pupil link
            var addPupilsDialogTriplet = attendancePattern.AddPupil();

            // Select some options
            addPupilsDialogTriplet.SearchCriteria.SearchByPupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var addPupilDialogSearchPage = addPupilsDialogTriplet.SearchCriteria;

            // Search and select a pupil
            var addPupilsDetailsPage = addPupilDialogSearchPage.Search<AddPupilsDetailsPage>();
            addPupilsDetailsPage.AddAllPupils();

            // Click on OK button
            addPupilsDialogTriplet.ClickOk();

            // Add data into Pattern table
            var patternRow = attendancePattern.Table[0];
            patternRow.MonAM = MonAM;
            patternRow.MonPM = MonPM;
            patternRow.TueAM = TueAM;
            patternRow.TuePM = TuePM;

            var confirmRequiredDialog1 = attendancePattern.ClickApply();
            confirmRequiredDialog1.ClickApplyThisPattern();
            attendancePattern.ClosePatternDialog();

            // Click on Search on edit mark
            // Navigate to Edit marks
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editmarkTriplet = new EditMarksTriplet();
            editmarkTriplet.SearchCriteria.StartDate = startDate;
            editmarkTriplet.SearchCriteria.Week = true;
            editmarkTriplet.SearchCriteria.SelectYearGroup(yeargroup);
            editMarksPage = editmarkTriplet.SearchCriteria.Search<EditMarksPage>();
            var editMarksTable1 = editMarksPage.Marks;

            //Verfiy Preserve functionality
            Assert.AreNotEqual(oldMarkAM, editMarksTable1[pupilName][2].Text, "For Existing records, Marks can be overwritten in Overwrite Mode");
            Assert.AreNotEqual(oldMarkPM, editMarksTable1[pupilName][3].Text, "For Existing records, Marks can be overwritten in Overwrite Mode");
            Assert.AreNotEqual(BlankMarkAM, editMarksTable1[pupilName][4].Text, "For Blank records, Mark can be overwritten in Overwrite Mode");
            Assert.AreNotEqual(BlankMarkPM, editMarksTable1[pupilName][5].Text, "For Blank records, Mark can be overwritten in Overwrite Mode");
            #endregion
        }
        #endregion

        #region Data
        public List<object[]> TC_ATT_001_Data()
        {
            string pattern = "M/d/yyyy";
            string pupilSurName = "AH_" + SeleniumHelper.GenerateRandomString(8);
            string pupilForeName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            string dateOfBirth = DateTime.ParseExact("10/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();
            string endDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).AddDays(3).ToShortDateString();

            string mark1 = "H";
            string mark2 = "H";

            string MonAM = "A";
            string MonPM = "A";
            string TueAM = "B";
            string TuePM = "B";

            var data = new List<Object[]>
            {                
                new object[] { startDate, endDate, "Year 2", pupilForeName, pupilSurName, pupilName, MonAM, MonPM, TueAM, TuePM, mark1, mark2, dateOfBirth, DateOfAdmission}                
            };
            return data;
        }


        public List<object[]> TC_ATT_002_Data()
        {
            string pattern = "M/d/yyyy";
            string pupilSurName = SeleniumHelper.GenerateRandomString(8);
            string pupilForeName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);        
            string dateOfBirth = DateTime.ParseExact("10/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();
            string endDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).AddDays(3).ToShortDateString();

            string mark1 = "L";
            string mark2 = "L";

            string MonAM = "C";
            string MonPM = "C";
            string TueAM = "D";
            string TuePM = "D";

            var data = new List<Object[]>
            {                
                new object[] { startDate, endDate, "Year 1", pupilForeName, pupilSurName, pupilName, MonAM, MonPM, TueAM, TuePM, mark1, mark2, dateOfBirth, DateOfAdmission}                
            };
            return data;
        }

        #endregion
    }
}