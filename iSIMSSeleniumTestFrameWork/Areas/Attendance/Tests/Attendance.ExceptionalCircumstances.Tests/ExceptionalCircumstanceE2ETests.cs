using Attendance.Components.AttendancePages;
using NUnit.Framework;
using SharedComponents.CRUD;
using System;
using TestSettings;
using WebDriverRunner.internals;
using POM.Helper;
using Attendance.POM.DataHelper;
using SeSugar.Data;
using System.Collections.Generic;
using Selene.Support.Attributes;
using POM.Components.Attendance;
using Attendance.POM.Entities;
using System.Linq;
using System.Globalization;
using SeSugar.Automation;


namespace Attendance.ExceptionalCircumstances.Tests
{
    public class ExceptionalCircumstanceE2ETests
    {
        /// <summary>
        /// Author: Goyal, Gaurav
        /// Description: Validate 'Exceptional Circumstance' functionality for NI :-
        ///     1. 'Y' Mark should be created on Register on creatring EC for selected Pupil.
        ///     2. On Deleting EC, '-' mark should be there for initially blank register.
        ///     3. If there is any existing mark on the register, that should be there after deletng EC.
        /// </summary>
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome}, DataProvider = "TC_ATT_002_Data")]
        
        public void ShouldApplyExceptionForSelectedPupil_OnRegister(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string description, string mark1, string mark2, string dateOfBirth, string DateOfAdmission)
        {
            #region Data Preperation

            //Inject Pupil from database
            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.CreatePupil(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, yeargroup);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            //Navigate to Edit Marks and apply marks
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");
            Wait.WaitLoading();
            var editMarksTripletPage = new EditMarksTriplet();
            editMarksTripletPage.SearchCriteria.StartDate = startDate;
            editMarksTripletPage.SearchCriteria.Week = true;
            editMarksTripletPage.SearchCriteria.SelectYearGroup(yeargroup);
            var editMarksPage = editMarksTripletPage.SearchCriteria.Search<EditMarksPage>();

            var editmarkTable = editMarksPage.Marks;

            IEnumerable<SchoolAttendanceCode> attendanceNotRequiredCodes = Queries.GetHolidaysAndInsetDays();
            List<string> codes = attendanceNotRequiredCodes.Select(x => x.Code).ToList<string>();

            if (codes.Contains(editmarkTable[pupilName][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Holidays And Inset Days");
                return;
            }

            editMarksPage.ModePreserve = false;
            editMarksPage.ModeHorizontal = true;
            var oldMarkAM = editmarkTable[pupilName][2].Text = mark1;
            var oldMarkPM = editmarkTable[pupilName][3].Text = mark2;
            var BlankMarkAM = editmarkTable[pupilName][4].Text = String.Empty;
            var BlankMarkPM = editmarkTable[pupilName][5].Text = String.Empty;

            editMarksPage.ClickSave();
            #endregion

            #region Steps:

            // Navigate to Exceptional Circumstances page
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Exceptional Circumstances");
            var exceptionalCircumstancesTriplet = new ExceptionalCircumstancesTriplet();

            // Click on Create button
            exceptionalCircumstancesTriplet.Create();

            // Select on Selected Pupils menu item
            var exceptionalCircumstancesDetailsPage = exceptionalCircumstancesTriplet.SelectSelectedPupils();

            // Enter data
            exceptionalCircumstancesDetailsPage.Description = description;
            exceptionalCircumstancesDetailsPage.StartDate = startDate;
            exceptionalCircumstancesDetailsPage.SessionStart = "AM";
            exceptionalCircumstancesDetailsPage.EndDate = endDate;
            exceptionalCircumstancesDetailsPage.SessionEnd = "PM";

            // Click on Add Pupil link
            var addPupilDialogTriplet = exceptionalCircumstancesDetailsPage.AddPupils();

            // Select some options
            var addPupilDialogSearchPage = addPupilDialogTriplet.SearchCriteria;

            addPupilDialogSearchPage.SearchByPupilName = pupilName;
           // var yearGroups = addPupilDialogSearchPage.YearGroups;
           // yearGroups[yeargroup].Select = true;

            // Search and select a pupil
            var addPupilsDetailsPage = addPupilDialogSearchPage.Search<AddPupilsDetailsPage>();
            addPupilsDetailsPage.Pupils.FirstOrDefault(x => x.Text.Trim().Equals(pupilName)).Click(); ;
            addPupilsDetailsPage.AddSelectedPupils();

            // Click on OK button
            addPupilDialogTriplet.ClickOk();

            // Save
            exceptionalCircumstancesTriplet.Save();

            exceptionalCircumstancesTriplet.SearchCriteria.StartDate = startDate;
            exceptionalCircumstancesTriplet.SearchCriteria.EndDate = endDate;
            var exCirResults=exceptionalCircumstancesTriplet.SearchCriteria.Search();
            //Ensure an 'Exceptional Circumstance'  can be found in a search by specifying a date range.
            Assert.AreNotEqual(null, exCirResults.FirstOrDefault(x => x.Name.Trim().Equals(description)));

            // Navigate to Edit Marks page
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");
            var editMarksTriplet = new EditMarksTriplet();
            //editMarksTriplet.SearchCriteria.StartDate = startDate;
            editMarksPage = editMarksTriplet.SearchCriteria.Search<EditMarksPage>();

            var editmarkTableNew = editMarksPage.Marks;

            if (codes.Contains(editmarkTableNew[pupilName][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Holidays And Inset Days");
                return;
            }
            // Ensure the applied  'Exceptional Circumstance' updates all the attendance registers for the selected date and half day range.
            var marks = editMarksPage.Marks;
            Assert.AreEqual("Y", marks[pupilName][2].Text, "Exceptional Circumstance should be updated for the selected pupil");
            Assert.AreEqual("Y", marks[pupilName][3].Text, "Exceptional Circumstance should be updated for the selected pupil");
            Assert.AreEqual("Y", marks[pupilName][4].Text, "Exceptional Circumstance should be updated for the selected pupil");
            Assert.AreEqual("Y", marks[pupilName][5].Text, "Exceptional Circumstance should be updated for the selected pupil");
            #endregion

            #region Post-Condition: Delete Exceptional Circumstances if existed

            // Navigate to Exceptional Circumstances page
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Exceptional Circumstances");
            exceptionalCircumstancesTriplet = new ExceptionalCircumstancesTriplet();
            var exCirResults1= exceptionalCircumstancesTriplet.SearchCriteria.Search();
            exceptionalCircumstancesDetailsPage = exCirResults1.FirstOrDefault(x => x.Name.Trim().Equals(description)).Click<ExceptionalCircumstancesDetailPage>();

            // Delete
            exceptionalCircumstancesDetailsPage.Delete();
            exceptionalCircumstancesTriplet.SearchCriteria.Search();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");
            var editMarksTripletNew = new EditMarksTriplet();
            //editMarksTripletNew.SearchCriteria.StartDate = startDate;
            var editMarksPageNew = editMarksTripletNew.SearchCriteria.Search<EditMarksPage>();

            // Ensure the applied  'Exceptional Circumstance' updates all the attendance registers for the selected date and half day range.
            var marksNew = editMarksPageNew.Marks;
            Assert.AreEqual(mark1, marksNew[pupilName][2].Text, "Any existing mark should be updated after deleting EC for completed register");
            Assert.AreEqual(mark2, marksNew[pupilName][3].Text, "Any existing mark should be updated after deleting EC for completed register");
            Assert.AreEqual(true, BlankMarkAM.Equals(String.Empty), "Missing Mark should be updated after deleting EC for blank register");
            Assert.AreEqual(true, BlankMarkPM.Equals(String.Empty), "Missing Mark should be updated after deleting EC for blank register");
            #endregion
        }


        #region Data
        public List<object[]> TC_ATT_002_Data()
        {
            string pattern = "M/d/yyyy";
            string pupilSurName = "Exceptional_" + SeleniumHelper.GenerateRandomString(8);
            string pupilForeName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();
            string endDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).AddDays(1).ToShortDateString();
            string dateOfBirth = DateTime.ParseExact("10/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string description = SeleniumHelper.GenerateRandomString(15);

            string mark1 = "H";
            string mark2 = "H";

            var data = new List<Object[]>
            {
                new object[] { startDate, endDate, "Year 1", pupilForeName, pupilSurName, pupilName, description, mark1, mark2, dateOfBirth, DateOfAdmission }
            };
            return data;
        }
        #endregion
    }
}