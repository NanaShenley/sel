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

namespace Attendance.EarlyYearSessionPattern.Tests
{
    public class EarlyYearSessionE2ETests
    {
        #region Preserve Functionality Of Early Year Session Pattern for already existing & Blank codes on Register
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_001_Data")]
        public void VerifyPreserveFunctionalityOfEarlyYearSessionPattern(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string MonAM, string MonPM, string TueAM, string TuePM, string mark1, string mark2, string dateOfBirth, string DateOfAdmission)
        {
            #region Data Preperation

            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.AddBasicLearner(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, yeargroup);

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
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Early Years Session Pattern");
            var earlyYears = new EarlyYearsSessionPatternDialog();
            earlyYears.IsPreserve = true;
            earlyYears.StartDate = startDate;
            earlyYears.EndDate = endDate;

            // Click on Add Pupil link
            var addPupilsDialogTriplet = earlyYears.AddPupil();

            // Select some options
            addPupilsDialogTriplet.SearchCriteria.SearchByPupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var addPupilDialogSearchPage = addPupilsDialogTriplet.SearchCriteria;

            // Search and select a pupil
            var addPupilsDetailsPage = addPupilDialogSearchPage.Search<AddPupilsDetailsPage>();
            addPupilsDetailsPage.AddAllPupils();

            // Click on OK button
            addPupilsDialogTriplet.ClickOk();
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
                new object[] { startDate, endDate, "Year 1", pupilForeName, pupilSurName, pupilName, MonAM, MonPM, TueAM, TuePM, mark1, mark2, dateOfBirth, DateOfAdmission}                
            };
            return data;
        }
        #endregion
    }
}
