using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Attendance.Components.Common
{
    public class AttendanceElements
    {
        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");
        public static readonly By Save = By.CssSelector("[data-automation-id='well_know_action_save']");
        public static readonly By TaskMenuSearch = By.Id("task-menu-search");
        public static readonly By TaskMenuSearchOption = By.CssSelector(".search-result.h1-result");
        public static readonly By AttendanceMenu = By.CssSelector("[data-automation-id='section_menu_Attendance']");

        public struct ExceptionalCircumstanceElements
        {
            //Excetional Circumstances Page Elements
            public static readonly By ExceptionalCircumstanceSubMenu = By.CssSelector("[data-automation-id='task_menu_section_attendance_ExceptionalCircumstance']");
            public static readonly By CreateExceptionalCircumstances = By.CssSelector("[data-automation-id='Button_Dropdown']");
            public static readonly By WholeSchoolOption = By.CssSelector("[data-automation-id='Whole']");
            public static readonly By SelectedPupilOption = By.CssSelector("[data-automation-id='Selected']");
            public static readonly By HeaderTitle = By.CssSelector(".pane-header.section-header");
            public static readonly By Description = By.Name("Description");
            public static readonly By MainPageStartDate = By.CssSelector("#editableData [name='StartDate']");
            public static readonly By MainPageEndDate = By.CssSelector("#editableData [name='EndDate']");
            public static readonly By SearchCriteria_StartDate = By.CssSelector("[name='StartDate']");
            public static readonly By SearchCriteria_EndDate = By.CssSelector("[name='EndDate']");

            public static readonly By SessionStart = By.Name("StartSession.dropdownImitator");
            public static readonly By SessionEnd = By.Name("EndSession.dropdownImitator");

            public static readonly By ConfirmationYes = By.CssSelector("[data-automation-id='yes_button']");
            public static readonly By ConfirmationNo = By.CssSelector("[data-automation-id='no_button']");
            public static readonly By SearchButton = By.CssSelector("[data-automation-id='search_criteria_submit']");
            public static readonly By SearchResultsCounter = By.CssSelector("[data-automation-id='search_results_counter']");
            public static readonly By SearchResultTile = By.CssSelector("[data-automation-id='resultTile']");
            public static readonly By SearchResults = By.CssSelector("[data-automation-id='search_results']");
            public static readonly By SpecficSearchResult = By.CssSelector("[data-automation-id='search_result']");
            public static readonly By DeleteButton = By.CssSelector("[data-automation-id='delete_button']");
            public static readonly By ContinueWithDelete = By.CssSelector("[data-automation-id='continue_with_delete_button']");
            public static readonly By CancelButton = By.CssSelector("[data-automation-id='cancel_button']");
        }

        public struct AddPupilPopUpElements
        {
            public static readonly By AddPupilLink = By.CssSelector("[data-automation-id='add_pupils_button']");
            public static readonly By PupilPickerPopPage = By.CssSelector("[data-automation-id='pupil_picker_dialog']");
            public static readonly By PupilPickerSearchPanel = By.CssSelector("[data-automation-id='pupil_picker_dialog'] [data-automation-id='search_criteria']");
            public static readonly By PupilPickerSearchButton = By.CssSelector("[data-automation-id='pupil_picker_dialog'] [data-automation-id='search_criteria_submit']");
            public static readonly By AddSelectedPupilButton = By.CssSelector("[data-automation-id='add_selected_button']");
            public static readonly By AddAllPupilButton = By.CssSelector("[data-automation-id='add_all_button']");
            public static readonly By RemoveSelectedPupilButton = By.CssSelector("[data-automation-id='remove_selected_button']");
            public static readonly By RemoveAllPupilButton = By.CssSelector("[data-automation-id='remove_all_button']");
            public static readonly By PupilSelectorOkButton = By.CssSelector("[data-automation-id='ok_button']");
            public static readonly By PupilSelectorCancelButton = By.CssSelector("[data-automation-id='cancel_button']");
            public static readonly By SearchRecordsToFindtext = By.CssSelector("[data-automation-id='pupil_picker_dialog'] [data-automation-id='resultTile']");
            public static readonly By TrashIcon = By.CssSelector("[data-automation-id='remove_button']");
        }

        public struct AttendancePatternElements
        {
            public static readonly By AttendancePatternSubMenu = By.CssSelector("[data-automation-id='task_menu_section_attendance_AttendancePattern-']");
            public static readonly By AttendancePatternDialog = By.CssSelector("#palette-editor .modal-dialog");
            public static readonly By AttendancePatternDetailPage = By.CssSelector("[data-section-id='task_menu_section_attendance_AttendancePattern-detail']");
            public static readonly By AttendancePageHeader = By.CssSelector("#palette-editor .modal-header");
            public static readonly By Radiobuttons = By.Name("PreserveOverwrite");

            public static readonly By dropdownvalue = By.Name("DateRange.dropdownImitator");
            public static readonly By startdate = By.Name("StartDate");
            public static readonly By EndDate = By.Name("EndDate");
            public static readonly By PatternSection = By.CssSelector("[data-automation-id='section_menu_Pattern']");
            public static readonly By SelectedPupilSection = By.CssSelector("[data-automation-id='section_menu_Selected Pupils']");

            public static readonly By ApplyPatternButton = By.CssSelector("[data-automation-id='apply_pattern_button']");
            public static readonly By CancelButton = By.CssSelector("[data-automation-id='cancel_button']");
            public static readonly By Confirmationpopup = By.CssSelector(".modal-dialog-message");
        }

        public struct EarlyYearProvisionsElements
        {
            public static readonly By createProvisions = By.CssSelector("[data-automation-id='add_button']");
            public static readonly By provisionName = By.Name("ProvisionName");
            public static readonly By MainPageStartTime = By.CssSelector("#editableData [name='StartTime']");
            public static readonly By MainPageEndTime = By.CssSelector("#editableData [name='EndTime']");
        }

       public struct EnterMarksOverDateRangeElements
       {
           public static readonly By applyMarkButton = By.CssSelector("[data-automation-id='apply_mark_button']");

           public static readonly By AddPupilLink = By.CssSelector("[data-automation-id='add_pupils_button']");        
           public static readonly By PupilPickerPopPage = By.CssSelector("[data-automation-id='pupil_picker_dialog']");
           public static readonly By PupilPickerSearchPanel = By.CssSelector("[data-automation-id='pupil_picker_dialog'] [data-automation-id='search_criteria']");
           public static readonly By PupilPickerSearchButton = By.CssSelector("[data-automation-id='pupil_picker_dialog'] [data-automation-id='search_criteria_submit']");
           public static readonly By AddSelectedPupilButton = By.CssSelector("[data-automation-id='add_selected_button']");
           public static readonly By AddAllPupilButton = By.CssSelector("[data-automation-id='add_all_button']");
           public static readonly By RemoveSelectedPupilButton = By.CssSelector("[data-automation-id='remove_selected_button']");
           public static readonly By RemoveAllPupilButton = By.CssSelector("[data-automation-id='remove_all_button']");
           public static readonly By PupilSelectorOkButton = By.CssSelector("[data-automation-id='ok_button']");
           public static readonly By PupilSelectorCancelButton = By.CssSelector("[data-automation-id='cancel_button']");
           public static readonly By PupilGridCheckBox = By.CssSelector(".webix_table_checkbox");
           
       }
    }
}
