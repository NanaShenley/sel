using NUnit.Framework;
using SeSugar.Data;
using System;
using System.Collections.Generic;
using TestSettings;
using WebDriverRunner.internals;
using Attendance.Components.AttendancePages;
using Attendance.Components.Common;
using Selene.Support.Attributes;
using POM.Helper;
using POM.Components.HomePages;
using System.Linq;
using Attendance.POM.DataHelper;
using System.Globalization;

namespace Attendance.AttendancePattern.Tests
{
    public class AttendancePatterenDialogTests
    {
        #region Story 4654, 4655: Open Attendance Pattern Dialog From Submenu

        [WebDriverTest(TimeoutSeconds = 400, Groups = new[] { "P2" }, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome})]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void TC_HP001_Exercise_ability_search_function_Attendance_Pattern_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Attendance Pattern'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Attendance Pattern";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Attendance Pattern' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Attendance Pattern"));
            Assert.AreNotEqual(null, searchResult, "'Attendance Pattern' screen does not display on search result");
            Assert.AreEqual("Attendance Pattern", searchResult.TaskActionHighlight, "'Group' is not underline on 'Attendance Pattern'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Attendance Pattern'");
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldDisplayAttendancePatternDialog_OnClickOfMenuAttendancePattern()
        {
            AttendancePatternPage page = AttendanceNavigations.NavigateToAttendancePatternFromTaskMenu();
            Assert.IsTrue(page.AttendancePatternDialog.Displayed && page.ApplyPattern.Displayed && page.closeButton.Displayed);
        }
        #endregion

        #region Story 4659: DatePicker Default Behaviour
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_01_Data")]
        public void CheckDefaultValueOfDatePicker(string academicYear, string dropdowntext)
        {
            AttendancePatternPage page = AttendanceNavigations.NavigateToAttendancePatternFromTaskMenu();
            string defaultdropdownValue = page.SelectDateRangeButtonDefaultValue.Text.Trim();
            string defaultacademicyear = page.academicYearDropdown.GetValue();
            DateTime dStartDate = Convert.ToDateTime(page.StartDate.GetValue());
            DateTime dEndDate = Convert.ToDateTime(page.EndDate.GetValue());
            Assert.IsTrue((defaultdropdownValue == dropdowntext) && defaultacademicyear == academicYear && dStartDate == page.weekstartdate() && dEndDate == page.weekEndDate());

        }
        #endregion  

        #region Story 4051 : Pattern Dropdown Codes Verification
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyAttendancePattenCodesForAMSession()
        {
            AttendancePatternPage page = AttendanceNavigations.NavigateToAttendancePatternFromTaskMenu();
            var patternCodes = page.PatternCodesForAMSession();
            Assert.IsTrue(patternCodes);
        }
        #endregion

        #region Story 6301: Verify Controls in Selected Pupil Section

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifySelectedPupilSection()
        {
            AttendancePatternPage page = AttendanceNavigations.NavigateToAttendancePatternFromTaskMenu();
            Assert.IsTrue(page.SelectedPupilSection.Displayed && page.AddPupilLink.Displayed);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_002_Data")]
        public void ShouldAddSelectedPupilsInGrid(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string dateOfBirth, string DateOfAdmission)
        {
            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.CreatePupil(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, yeargroup);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            AttendancePatternPage page = AttendanceNavigations.NavigateToAttendancePatternFromTaskMenu();
            AttendanceSearchPanel searchcriteria = page.ClickAddPupilLink();
            
            var checkbox = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            checkbox.Click(SimsBy.AutomationId("section_menu_Year Group"));
            checkbox.FindCheckBoxAndClick("Year Group", new List<string> {yeargroup});
            PupilPickerAvailablePupilSection AvailablePupils = searchcriteria.PupilPickerSearchButton();
            AvailablePupils.GetAvailablePupils();
            PupilPickerSelectedPupilSection selectedPupil = AvailablePupils.AddSelectedPupil();
            AttendancePatternPage app1 =  selectedPupil.ClickAttendancePattern_PupilPickerOkButton();
            Assert.IsTrue(app1.trashIcon.Displayed);
        }


        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_002_Data")]
        public void ShouldRemoveSelectedPupilsFromGrid(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string dateOfBirth, string DateOfAdmission)
        {
            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.CreatePupil(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, yeargroup);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);
            AttendancePatternPage page = AttendanceNavigations.NavigateToAttendancePatternFromTaskMenu();
            AttendanceSearchPanel searchcriteria = page.ClickAddPupilLink();
            var checkbox = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            checkbox.Click(SimsBy.AutomationId("section_menu_Year Group"));
            checkbox.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1"});
            PupilPickerAvailablePupilSection AvailablePupils = searchcriteria.PupilPickerSearchButton();
            AvailablePupils.GetAvailablePupils();
            PupilPickerSelectedPupilSection selectedPupil = AvailablePupils.AddSelectedPupil();
            AttendancePatternPage app1 = selectedPupil.ClickAttendancePattern_PupilPickerOkButton();
            app1.RemovePupilFromGrid();
            Assert.IsTrue(app1.attendancePatternLearnerHeader.Displayed);
        }

        #endregion

        #region Story 7711 : Attendance Pattern Link On Pupil Record Page
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyAddAndRemoveLinks_On_AttendancePattenDialog_OnPupilRecordPage()
        {
            AttendancePatternPage page = AttendanceNavigations.NavigateToAttendancePatternPage_OnPupilRecordPage();
            Assert.IsTrue(page.LinksAbsent());
        }
        #endregion

        #region Story 3959 : Pattern Dropdown and Pupils Validations
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void PatternDropdwonandSelectedPupilsdValidations()
        {
            AttendancePatternPage app = AttendanceNavigations.NavigateToAttendancePatternFromTaskMenu();
            app.ClickApplyPatternButton();
            Assert.IsTrue(app.ValidationWarning.Displayed, "Validation Warning");

        }
        #endregion

        #region Story 3959 : Mandatory Date Validations
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldHaveMandatoryDateField()
        {
            AttendancePatternPage app = AttendanceNavigations.NavigateToAttendancePatternFromTaskMenu();
            app.StartDate.Clear();
            app.EndDate.Clear();
            app.ClickApplyPatternButton();
            Assert.IsTrue(app.ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story 3959 : Start Dates and End Dates Validations
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldHaveStartDateLessthanEndDate()
        {
            AttendancePatternPage app = AttendanceNavigations.NavigateToAttendancePatternFromTaskMenu();
            app.StartDate.Clear();
            app.StartDate.SendKeys(Convert.ToString(DateTime.Today.ToShortDateString()));
            app.EndDate.Clear();
            app.EndDate.SendKeys(Convert.ToString(DateTime.Today.AddDays(-1).ToShortDateString()));
            app.ClickApplyPatternButton();
            Assert.IsTrue(app.ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region SchoolAdministrator Permission Test
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldDisplayAttendancePatternMenuForSchoolAdministrator()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AttendanceNavigations navigationPage = new AttendanceNavigations();
            navigationPage.NavigateToAttendanceMenu();
            Assert.IsTrue(navigationPage.attendancePatternSubMenu.Displayed);
        }
        #endregion

        #region ClassTeacher Permission Test
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldNotDisplayAttendancePatternMenuForClassTeacher()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            AttendanceNavigations navigationPage = new AttendanceNavigations();
            navigationPage.NavigateToAttendanceMenu();
            string loc = "[data-automation-id='task_menu_section_attendance_AttendancePattern-']";
            Assert.True(navigationPage.SubmenuNotVisibleForClassTeacher(loc));
        }
        #endregion

        #region Data
        public List<object[]> TC_ATT_01_Data()
        {
            string academicYear = String.Format("{0}", SeleniumHelper.GetAcademicYear(DateTime.Now));
            string dropdowntext = "This Week";

            var data = new List<Object[]>
            {
                new object[] {academicYear,dropdowntext}
            };
            return data;
        }

        public List<object[]> TC_ATT_002_Data()
        {
            string pattern = "M/d/yyyy";
            string pupilSurName = "AH_" + SeleniumHelper.GenerateRandomString(8);
            string pupilForeName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            string dateOfBirth = DateTime.ParseExact("10/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();
            string endDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).AddDays(3).ToShortDateString();

            var data = new List<Object[]>
            {                
                new object[] { startDate, endDate, "Year 1", pupilForeName, pupilSurName, pupilName, dateOfBirth, DateOfAdmission}                
            };
            return data;
        }


        #endregion

    }
}