using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Components.Common
{
    public struct EditMarksElements
    {
        public const string EditMarksQuickLink = "";

        public struct SearchPanel
        {
            public static By SearchCriteria = By.CssSelector("[data-automation-id='search_criteria']");
            public static readonly By SearchButton = By.CssSelector("[data-automation-id='search_criteria_submit']");
            public static readonly By RadioButton = By.Name("IsDaily");
            public static readonly By WholeSchoolChecbox = By.Name("tri_chkbox_WholeSchool");
        }

        public struct Toolbar
        {
            public static readonly By EditMarksQuickLink = By.CssSelector("[data-automation-id='quicklinks_top_level_attendance_submenu_editmarks']");
            public static readonly By SummaryLink = By.CssSelector("[data-automation-id='summary_button']");
            public static readonly By toolbarSection = By.CssSelector(".form-header .pane-toolbar .navbar");
            public static readonly By Save = By.CssSelector("a[id='RegisterSave']");
            public static readonly By AllCodessDropdown = By.CssSelector("[data-automation-id='Button_Dropdown']");
            public static readonly By AllCodesMenu = By.CssSelector(".dropdown.menu.open .dropdown-menu.toolbar-menu");
            public static readonly By PreserveModeButton = By.CssSelector("[data-automation-id='Extended_Dropdown']");
            public static readonly By Preserve = By.CssSelector("[data-automation-id='Keep']");
            public static readonly By Overwrite = By.CssSelector("[data-automation-id='Replace']");
            public static readonly By AdditionalColumnButton = By.CssSelector("[data-automation-id='additional_columns_button']");
            public static readonly By FilterVisibility = By.CssSelector("a[title='Toggle the filter visibility']");
            public static readonly By OrientationButton = By.CssSelector("a[title='Automatically advance down the register']");
            public static readonly By Vertical = By.CssSelector("[data-automation-id='Vertical']");
            public static readonly By Horizontal = By.CssSelector("[data-automation-id='Horizontal']");
            public static readonly By AdditionalColumnPage = By.CssSelector(".modal-content.layout-page");
            public static readonly By ConfirmedSave = By.CssSelector("[data-automation-id='status_success']");

        }

        public struct AdditionalColumn
        {
            public static By PersonalDetails = By.CssSelector("div[webix_tm_id='Personal Details'] .webix_tree_checkbox");
            public static By DateOfBirth = By.CssSelector("div[webix_tm_id='Learner.DateOfBirth']  .webix_tree_checkbox");
            public static By Gender = By.CssSelector("div[webix_tm_id='Learner.Gender.Description'] .webix_tree_checkbox");
            public static By LegalName = By.CssSelector("div[webix_tm_id='Learner.LegalForename'] .webix_tree_checkbox");

            public static By RegistrationDetails = By.CssSelector("div[webix_tm_id='Registration Details'] .webix_tree_checkbox");
            public static By AdmissionNumber = By.CssSelector("div[webix_tm_id='Learner.AdmissionNumber'] .webix_tree_checkbox");
            public static By Class = By.CssSelector("div[webix_tm_id='Learner.PrimaryClass.FullName'] .webix_tree_checkbox");
            public static By YearGroup = By.CssSelector("div[webix_tm_id='Learner.YearGroup.FullName'] .webix_tree_checkbox");
            public static By QuickNote = By.CssSelector("div[webix_tm_id='Learner.QuickNote'] .webix_tree_checkbox");
            public static By MedicalCondition = By.CssSelector("div[webix_tm_id='LearnerMedicalCondition.MedicalCondition'] .webix_tree_checkbox");

            public static By Attendance = By.CssSelector("div[webix_tm_id='Attendance'] .webix_tree_checkbox");
            public static By PercentageAttendance = By.CssSelector("div[webix_tm_id='AttendanceRecord'] .webix_tree_checkbox");

            public static By ClearSelection = By.CssSelector("[data-clear-container-id='editablecolumntreenode']");

            public static By OkButton = By.CssSelector(".modal-footer.pane-footer.layout-row [data-automation-id='ok_button']");
        }

        public struct GridColumns
        {
            public static By DateOfBirth = By.Id("cDateOfBirth");
            public static By Gender = By.Id("cGender");
            public static By AdmissionNumber = By.Id("cAdmissionNumber");
            public static By Class = By.Id("cClass");
            public static By YearGroup = By.Id("cYearGroup");
            public static By PupilLink = By.CssSelector(".webix_ss_left .webix_cell.webix_cell_select");
            
        }

        public struct AttendancePLog
        {
            public static By AttedancePLogNote = By.CssSelector("[data-automation-id='log-note-header-attendance']");
            public static By viewPlogNote = By.CssSelector("[data-automation-id='view_pupil_log_button']");
        }

    }
}
