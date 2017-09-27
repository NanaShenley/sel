using TestSettings;
using WebDriverRunner.internals;
using NUnit.Framework;
using Attendance.Components;
using OpenQA.Selenium;
using Attendance.Components.Common;
using Attendance.Components.AttendancePages;
using POM.Helper;
using Selene.Support.Attributes;
using POM.Components.HomePages;
using System.Linq;
using System;
using System.Collections.Generic;
using Attendance.POM.Entities;
using Attendance.POM.DataHelper;
using POM.Components.Attendance;
using System.Collections.ObjectModel;

namespace Attendance.EditMarks.Tests
{
    public class EditMarksDetailsTests
    {
        [WebDriverTest(TimeoutSeconds = 400, Groups = new[] { "P2" }, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void TC_HP001_Exercise_ability_search_function_Edit_Marks_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitLoading();
            var homePage = new HomePage();

            //Search with 'Edit Marks'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Edit Marks";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Edit Marks' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Edit Marks"));
            Assert.AreNotEqual(null, searchResult, "'Edit Marks' screen does not display on search result");
            Assert.AreEqual("Edit Marks", searchResult.TaskActionHighlight, "'Group' is not underline on 'Edit Marks'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Edit Marks'");
        }

        [WebDriverTest(TimeoutSeconds = 400, Groups = new[] { "P2" }, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void TC_HP002_Exercise_ability_search_function_Edit_MarksWithMicroServices_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Edit Marks Using Microservice");
            Wait.WaitLoading();
            var homePage = new HomePage();

            //Search with 'Edit Marks'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Edit Marks Using Microservice";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Edit Marks' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Edit Marks Using Microservice"));
            Assert.AreNotEqual(null, searchResult, "'Edit Marks Using Microservice' screen does not display on search result");
            Assert.AreEqual("Edit Marks Using Microservice", searchResult.TaskActionHighlight, "'Group' is not underline on 'Edit Marks Using Microservice'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Edit Marks Using Microservice'");
        }

        #region Verify Day View Of Register

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyDayViewOfRegister()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();

            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            group.EnterDate(SeleniumHelper.GetFirstDayOfWeek(System.DateTime.Now).ToShortDateString());
            var registerView = new AttendanceSearchPanel();
            registerView.ClickRadioButton("Day");

            AttendanceDetails editMarksPage = group.EditMarksSearchButton();

            IWebElement grid = SeleniumHelper.Get(By.CssSelector("[data-section-id=\"searchResults\"]"));
            var columns = grid.FindElements(By.CssSelector("[data-menu-column-id]"));

            // Only the Pupil Name column And Single Day Sessions should be present in the grid
            Assert.IsTrue(columns.Count == 4);
        }
        #endregion

        #region Verify Week View Of Register

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyWeekViewOfRegister()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var registerView = new AttendanceSearchPanel();
            registerView.ClickRadioButton("Week");
            var group = new AttendanceSearchPanel();

            group.Select("Year Group", "Year 1", "Year 2");
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();

            IWebElement grid = SeleniumHelper.Get(By.CssSelector("[data-section-id=\"searchResults\"]"));
            var columns = grid.FindElements(By.CssSelector("[data-menu-column-id]"));

            // Only the Pupil Name column And Single Day Sessions should be present in the grid
            Assert.IsTrue(columns.Count == 12);
        }
        #endregion

        #region Verify Horizontal Orientation of Marks on Register

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void HorizontalOrientationOfMarks_OnAttendanceRegister()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var registerView = new AttendanceSearchPanel();
            registerView.ClickRadioButton("Week");

            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();

            //Horizontal Cursor Orientation in Preserve Mode
            EditMarksGridHelper grid = new EditMarksGridHelper();
            grid.ClickOrientationbutton(grid.orientationButton);
            grid.ClickOrientationbutton(grid.horizontalMode);
            EditMarksGridHelper.ClickFirstCellofColumn("2");
            EditMarksGridHelper.GetEditor().SendKeys("L");
            EditMarksGridHelper.GetEditor().SendKeys("L");
            EditMarksGridHelper.GetEditor().SendKeys("B");
            EditMarksGridHelper.GetEditor().SendKeys("C");
            EditMarksGridHelper.GetEditor().SendKeys("D");
            EditMarksGridHelper.GetEditor().SendKeys("B");
        }
        #endregion

        #region Verify Vertical Orientation of Marks on Register

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerticalOrientationOfMarks_OnAttendanceRegister()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var registerView = new AttendanceSearchPanel();
            registerView.ClickRadioButton("Week");

            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();

            //Vertical Cursor Orientation in Preserve Mode
            EditMarksGridHelper grid = new EditMarksGridHelper();
            grid.ClickOrientationbutton(grid.orientationButton);
            grid.ClickOrientationbutton(grid.verticalMode);
            EditMarksGridHelper.ClickFirstCellofColumn("3");
            EditMarksGridHelper.GetEditor().SendKeys("L");
            EditMarksGridHelper.GetEditor().SendKeys("L");
            EditMarksGridHelper.GetEditor().SendKeys("B");
            EditMarksGridHelper.GetEditor().SendKeys("C");
            EditMarksGridHelper.GetEditor().SendKeys("C");
            EditMarksGridHelper.GetEditor().SendKeys("D");
        }
        #endregion

        #region Verify functionality of Preserve Mode on Register

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void PreseveModeOfMarks_OnAttendanceRegister()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var registerView = new AttendanceSearchPanel();
            registerView.ClickRadioButton("Week");

            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();

            var editMarksTripletPage = new EditMarksTriplet();
            var editPage = editMarksTripletPage.SearchCriteria.Search<EditMarksPage>();

            var editmarkTable = editPage.Marks;
            IEnumerable<SchoolAttendanceCode> getHolidays = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = getHolidays.Select(x => x.Code).ToList<string>();
            if (codes.Contains(editmarkTable[1][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Holidays");
                return;
            }

            //Vertical Cursor Orientation in Preserve Mode
            EditMarksGridHelper grid = new EditMarksGridHelper();
            grid.ClickOrientationbutton(grid.preserveButton);
            grid.ClickOrientationbutton(grid.preserveMode);
            EditMarksGridHelper.ClickFirstCellofColumn("3");
            EditMarksGridHelper.GetEditor().SendKeys("L");
            EditMarksGridHelper.GetEditor().SendKeys("L");
            EditMarksGridHelper.GetEditor().SendKeys("B");
            EditMarksGridHelper.GetEditor().SendKeys("C");
            EditMarksGridHelper.GetEditor().SendKeys("C");
            EditMarksGridHelper.GetEditor().SendKeys("D");
        }
        #endregion

        #region Verify functionality of Overwrite Mode on Register

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void OverwriteModeOfMarks_OnAttendanceRegister()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var registerView = new AttendanceSearchPanel();
            registerView.ClickRadioButton("Week");

            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();

            //Vertical Cursor Orientation in Preserve Mode
            EditMarksGridHelper grid = new EditMarksGridHelper();
            grid.ClickOrientationbutton(grid.preserveButton);
            grid.ClickOrientationbutton(grid.overwriteMode);
            EditMarksGridHelper.ClickFirstCellofColumn("3");
            EditMarksGridHelper.GetEditor().SendKeys("L");
            EditMarksGridHelper.GetEditor().SendKeys("L");
            EditMarksGridHelper.GetEditor().SendKeys("B");
            EditMarksGridHelper.GetEditor().SendKeys("C");
            EditMarksGridHelper.GetEditor().SendKeys("C");
            EditMarksGridHelper.GetEditor().SendKeys("D");
        }
        #endregion

        #region Attendance PLog

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldOpenPupilLog_OnClickofPupilLink()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            group.EnterDate(SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString());
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();
            EditMarksPupilDetail plog = editMarksPage.ClickPupilLink();
            Assert.IsTrue(plog.AttendanceLog.Displayed);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldHavePupilLogLinkAndAttendanceNoteOnPLogPopUp()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            group.EnterDate(SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString());
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();
            EditMarksPupilDetail plog = editMarksPage.ClickPupilLink();
            Assert.IsTrue(plog.viewPlogNote.Displayed && plog.attendanceNote.Displayed);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldRedirectToPupilLogPage_OnClickOfPupilLogLink()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            group.EnterDate(SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString());
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();
            EditMarksPupilDetail plog = editMarksPage.ClickPupilLink();
            plog.ClickViewPupilLogLink();
            Assert.IsTrue(plog.pupilLogPage.Displayed);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldHaveMandatoryNoteField()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            group.EnterDate(SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString());
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();
            EditMarksPupilDetail plog = editMarksPage.ClickPupilLink();
            plog.ClickOnAttendanceNoteButton();
            plog.AttendanceNoteTextSave();
            Assert.IsTrue(plog.attendanceNotevalidationWarning.Displayed);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyMaxLengthOfNoteAndTitleField()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            group.EnterDate(SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString());
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();
            EditMarksPupilDetail plog = editMarksPage.ClickPupilLink();
            plog.ClickOnAttendanceNoteButton();
            Assert.IsTrue(plog.attendanceNoteTextArea.GetAttribute("data-rule-maxlength") == "4000" && plog.attendanceTitleArea.GetAttribute("maxlength") == "200");
        }

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CreateAttendaceNoteWithoutPinNote()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            group.EnterDate(SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString());
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();
            EditMarksPupilDetail plog = editMarksPage.ClickPupilLink();
            plog.ClickOnAttendanceNoteButton();
            plog.EnterTextInAttendanceNoteTextArea("Test Note For Attendance Plog");
            plog.EnterTitle("AttendanceNote");
            AttendanceDetails editMarksPage1 = plog.AttendanceNoteTextSave();
            EditMarksPupilDetail plog1 = editMarksPage1.ClickPupilLink();
            plog1.ClickViewPupilLogLink();
            Assert.IsTrue(plog.AttendanceNoteOnPupilLogPage.Displayed);
        }

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CreateAttendaceNoteWithPinNote()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            group.EnterDate(SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString());
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();
            EditMarksPupilDetail plog = editMarksPage.ClickPupilLink();
            plog.ClickOnAttendanceNoteButton();
            plog.EnterTextInAttendanceNoteTextArea("Pin Note Test For Attendance Plog");
            plog.EnterTitle("Pin This Note");
            plog.PinNote();
            AttendanceDetails editMarksPage1 = plog.AttendanceNoteTextSave();
            EditMarksPupilDetail plog1 = editMarksPage1.ClickPupilLink();
            plog1.ClickViewPupilLogLink();
            Assert.IsTrue(plog.AttendanceNoteOnPupilLogPage.Displayed && plog.pinNote.Displayed);
        }

        #endregion

        #region Dinner Money

        #region Enter meal codes in meal cell in Edit Marks screen
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EnterMealTypeInMealCell_InEditMarks()
        {
            string[] featureList = { "Dinner Money Settings" };
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, featureList);

            Wait.WaitForDocumentReady();
            SeleniumHelper.NavigateQuickLink("Edit Marks");
            AttendanceNavigations.ClickDayOrWeekRadioButton("Day");
            AttendanceNavigations.SelectClass("6A");
            AttendanceNavigations.ClickEditMarksSearchButton();

            Wait.WaitForDocumentReady();
            SeleniumHelper.Sleep(10);

            //Vertical Cursor Orientation in Preserve Mode
            EditMarksGridHelper grid = new EditMarksGridHelper();
            grid.ClickOrientationbutton(grid.preserveButton);
            grid.ClickOrientationbutton(grid.overwriteMode);
            EditMarksGridHelper.ClickFirstCellofColumn("3");
            EditMarksGridHelper.GetEditor().SendKeys("A");
            EditMarksGridHelper.GetEditor().SendKeys("H");
            EditMarksGridHelper.GetEditor().SendKeys("P");
            EditMarksGridHelper.GetEditor().SendKeys("S");

            List<IWebElement> cells = EditMarksGridHelper.FindAllcells();

            //check if edited cell has meal types (A, H, P or S) in it, if yes, return test as pass
            var mealCodes = cells.Skip(1).FirstOrDefault().Text;

            Assert.IsTrue(mealCodes.Contains("A") || mealCodes.Contains("H") || mealCodes.Contains("P") || mealCodes.Contains("S"));
        }

        #endregion

        #region Verify Horizontal Orientation of Meal Marks on Edit Marks

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void HorizontalOrientationOfMealMarks_OnEditMarks()
        {
            string[] featureList = { "Dinner Money Settings" };
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, featureList);

            Wait.WaitForDocumentReady();
            SeleniumHelper.NavigateQuickLink("Edit Marks");
            AttendanceNavigations.ClickDayOrWeekRadioButton("Week");
            SeleniumHelper.Sleep(5);
            AttendanceNavigations.SelectClass("6A");
            AttendanceNavigations.ClickEditMarksSearchButton();

            Wait.WaitForDocumentReady();
            SeleniumHelper.Sleep(10);

            //Horizontal Cursor Orientation in Preserve Mode
            EditMarksGridHelper grid = new EditMarksGridHelper();
            grid.ClickOrientationbutton(grid.orientationButton);
            grid.ClickOrientationbutton(grid.horizontalMode);
            EditMarksGridHelper.ClickFirstCellofColumn("3");
            EditMarksGridHelper.GetEditor().SendKeys("A");
            EditMarksGridHelper.GetEditor().SendKeys("H");
            EditMarksGridHelper.GetEditor().SendKeys("P");
            EditMarksGridHelper.GetEditor().SendKeys("S");
            EditMarksGridHelper.GetEditor().SendKeys("H");
        }
        #endregion

        #region Verify Vertical Orientation of Meal Marks on Edit Marks

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerticalOrientationOfMealMarks_OnEditMarks()
        {
            string[] featureList = { "Dinner Money Settings" };
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, featureList);

            Wait.WaitForDocumentReady();
            SeleniumHelper.NavigateQuickLink("Edit Marks");
            AttendanceNavigations.ClickDayOrWeekRadioButton("Week");
            SeleniumHelper.Sleep(5);
            AttendanceNavigations.SelectClass("6A");
            AttendanceNavigations.ClickEditMarksSearchButton();

            Wait.WaitForDocumentReady();
            SeleniumHelper.Sleep(10);

            //Vertical Cursor Orientation in Preserve Mode
            EditMarksGridHelper grid = new EditMarksGridHelper();
            grid.ClickOrientationbutton(grid.orientationButton);
            grid.ClickOrientationbutton(grid.verticalMode);
            EditMarksGridHelper.ClickFirstCellofColumn("3");
            EditMarksGridHelper.GetEditor().SendKeys("A");
            EditMarksGridHelper.GetEditor().SendKeys("H");
            EditMarksGridHelper.GetEditor().SendKeys("P");
            EditMarksGridHelper.GetEditor().SendKeys("S");
            EditMarksGridHelper.GetEditor().SendKeys("H");
        }
        #endregion

        #region Verify the Meal register link in quick link for class teacher
        // <summary>
        /// Auth : Gazbare, Swapnil
        /// Desc : Verify the Meal register link in quick link for class teacher
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.AllEnglish)]

        public void Verify_MealRegister_link()
        {
            string[] featureList = { "Dinner Money Settings" };
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher, featureList);

            Wait.WaitForDocumentReady();
            ReadOnlyCollection<IWebElement> mealRegisterLink = SeleniumHelper.FindElements(By.LinkText("Meal Register"));

            Assert.IsTrue(mealRegisterLink != null && mealRegisterLink.Count > 0);
        }


        #endregion

        #endregion
    }
}




