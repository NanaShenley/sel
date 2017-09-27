using Attendance.Components.AttendancePages;
using Attendance.Components.Common;
using NUnit.Framework;
using POM.Components.HomePages;
using POM.Helper;
using Selene.Support.Attributes;
using System;
using System.Collections.Generic;
using TestSettings;
using WebDriverRunner.internals;
using System.Linq;
using POM.Components.Attendance;
using SeSugar.Automation;
using WebDriverRunner.webdriver;

namespace Attendance.ApplyMarkOverDateRange.Tests
{
    public class ApplyMarkOverDateRangeDialogTests
    {
        #region Story #4636 : Navigate to Apply Marks Over Date Range Page
        [WebDriverTest(TimeoutSeconds = 400, Groups = new[] { "P2" }, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome})]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void TC_HP001_Exercise_ability_search_function_ApplyMark_Over_DateRange_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitLoading();
            var homePage = new HomePage();
            WebContext.Screenshot();

            //Search with 'Apply Mark Over Date Range'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Apply Mark";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Apply Mark Over Date Range' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Apply Mark Over Date Range"));
            Assert.AreNotEqual(null, searchResult, "'Apply Mark Over Date Range' screen does not display on search result");
            Assert.AreEqual("Apply Mark", searchResult.TaskActionHighlight, "'Group' is not underline on 'Apply Mark Over Date Range'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Apply Mark Over Date Range'");
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome})]

        public void Should_Diplay_ApplyMark_Dialog_OnClick_OfLink()
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            Assert.IsTrue(page.headerTitle.Displayed);
        }
        #endregion

        #region Story #4636 : RadioButton Default Behaviour
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome})]

        public void CheckDefaultBehaviourOfRadiobutton()
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            var preserve = page.ClickRadioButton("Keep existing marks");
            Assert.IsTrue(preserve.GetAttribute("checked")=="true");
        }
        #endregion

        #region Story : DatePicker Default Behaviour
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_01_Data")]

        public void Verify_DefaultValue_Of_DateControl(string academicYear, string dropdowntext)
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            string defaultdropdownValue = page.SelectDateRangeButtonDefaultValue.Text.Trim();
            string defaultacademicyear = page.academicYearDropdown.GetValue();
            DateTime dStartDate = Convert.ToDateTime(page.StartDate.GetValue());
            DateTime dEndDate = Convert.ToDateTime(page.EndDate.GetValue());
            Assert.IsTrue((defaultdropdownValue == dropdowntext) && defaultacademicyear == academicYear && dStartDate == page.weekstartdate() && dEndDate == page.weekEndDate());
        }
        #endregion

        #region Story : Mark Dropdown Codes Verification
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Verify_AttendanceCodes_In_Dropdown()
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            var patternCodes = page.PatternCodesForMark();
            Assert.IsTrue(patternCodes);
        }
        #endregion

        #region Story: Verfiy the behaviour of Pupil Grid
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]

        public void ShouldAddSelectedPupilsInGrid()
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            AttendanceSearchPanel searchcriteria = page.ClickAddPupilButton();
            var checkbox = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            checkbox.Click(SeSugar.Automation.SimsBy.AutomationId("section_menu_Year Group"));
            checkbox.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
            PupilPickerAvailablePupilSection AvailablePupils = searchcriteria.PupilPickerSearchButton();
            AvailablePupils.GetAvailablePupils();
            PupilPickerSelectedPupilSection selectedPupil = AvailablePupils.AddSelectedPupil();
            ApplyMarkOverDateRangePage app1 = selectedPupil.ClickApplyMarkOverDateRange_PupilPickerOkButton();
            Assert.IsTrue(app1.trashIcon.Displayed);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]

        public void ShouldRemoveSelectedPupilsFromGrid()
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            AttendanceSearchPanel searchcriteria = page.ClickAddPupilButton();
            var checkbox = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            checkbox.Click(SeSugar.Automation.SimsBy.AutomationId("section_menu_Year Group"));
            checkbox.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
            PupilPickerAvailablePupilSection AvailablePupils = searchcriteria.PupilPickerSearchButton();
            AvailablePupils.GetAvailablePupils();
            PupilPickerSelectedPupilSection selectedPupil = AvailablePupils.AddSelectedPupil();
            ApplyMarkOverDateRangePage app1 = selectedPupil.ClickApplyMarkOverDateRange_PupilPickerOkButton();
            app1.RemovePupilFromGrid();
            //Assert.IsTrue(app1.PupilGrid.Displayed);
        }
        #endregion

        #region Verify Mandatory fields on page
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldhaveValidationWarningMessage_if_NoPupil_Or_MarkSelected()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AttendanceNavigations.NavigateToApplyMarkOverDateRange();
            var applyMarkOverDateRange = new ApplyMarksOverDateRangeDialog();
            applyMarkOverDateRange.IsPreserve = true;
            applyMarkOverDateRange.StartDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();
            applyMarkOverDateRange.EndDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString(); 
            applyMarkOverDateRange.ClickApplyThisMark();
            Assert.IsTrue(applyMarkOverDateRange.IsValidationWarningDisplayed());
        }
        #endregion

        #region SchoolAdministrator Permission Test
        [WebDriverTest(Enabled = true, Groups = new[] { "P1" }, Browsers = new[] { BrowserDefaults.Chrome })]
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
        [WebDriverTest(Enabled = true, Groups = new[] { "P1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldNotDisplayAttendancePatternMenuForClassTeacher()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            AttendanceNavigations navigationPage = new AttendanceNavigations();
            navigationPage.NavigateToAttendanceMenu();
            string loc = "[data-automation-id='task_menu_section_attendance_ApplyMarkOverDateRange-']";
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
        #endregion

    }
}