using OpenQA.Selenium;
using SharedComponents.Helpers;
using TestSettings;

namespace DataExchange.POM.Components.Common
{
    public struct DataExchangeElements
    {
        public struct CommonElements
        {
            public static readonly By SearchCriteria = By.CssSelector("form[data-section-id='searchCriteria']");
            public static readonly By SearchButton = By.CssSelector("button[type='submit']");
            public static readonly By DialogSelector = By.CssSelector("[data-dialog='editor-dialog']");
            public static readonly By ConfirmationButton = By.CssSelector("[data-section-id='generic-confirm-dialog']>div>div> div:nth-child(3)>button:nth-child(1)");
            public static readonly By AdditionalColumnButton = By.CssSelector("a[data-show-additional-columns-modal]");
            //public static readonly By AdditionalColumnButton = SeleniumHelper.SelectByDataAutomationID("additional_columns_button");
            public static readonly By PickerDialogOkButton = By.CssSelector("[id='dialog-palette-editor']> div:nth-child(2) > div > div:nth-child(3)>button:nth-child(1)");
            public static readonly By DataMaintenance = By.CssSelector("#editableData");
            public static readonly By ProcessingMessage = By.CssSelector(".media-heading");
            public static readonly By CtfImportMainScreen = By.CssSelector("[class='section-header-title'] > span:nth-child(2)");
            public static readonly By LopImportMainScreen = By.CssSelector(".section-header-title>span");
            public static readonly By AdditionalColumnName = By.CssSelector("[name*=\"Name\"]");
            public static readonly By AdditionalColumnGridRow = By.CssSelector("[data-maintenance-container=\"AdditionalColumnSpecifications\"]");
            public static readonly By AdditionalColumnWidth = By.CssSelector("[name*=\"dropdownImitator\"]");
            public static readonly By WholeSchool = By.CssSelector("[name='tri_chkbox_WholeSchool']");
            public static readonly By NotificationAlert = By.CssSelector("[role='alert']");
            public static readonly By SaveButton = By.CssSelector("[title=\"Save Record\"]");
            public static readonly By FileName = By.CssSelector("input[name='FileName']");
            public const string WebixCheckBoxCss = ".webix_tree_item";
            public const string CreateButton = "a[title^=Create]";
            public static readonly By EditableColumnTreeNode = By.Id("editablecolumntreenode");
            public static readonly By PupilGridCheckBox = By.CssSelector("input.webix_table_checkbox");
        }

        public struct QuickLinks
        {
           // public static readonly By PupilRecord = By.CssSelector("#quick-links > div > ul > li:nth-child(2) > a");
            public static readonly By PupilRecord = SeleniumHelper.SelectByDataAutomationID("quicklinks_top_level_pupil_submenu_pupilrecords");
            public static readonly By PupilContact = By.CssSelector("#quick-links > div > ul > li:nth-child(4) > a");
        }

        public struct Menu
        {
            public const string PupilHeader = "[data-automation-id='task_menu_section_pupils']";
            public const string PupilRecord = "[data-automation-id='task_menu_section_pupils_pupil_record']";
            public const string StaffHeader = "#task-menu > div:nth-child(1) > div > div:nth-child(14) > div.panel-heading > h4 > a > span";
            public static readonly By MenuButton = SimsBy.AutomationId("task_menu");
            public static readonly By StatutoryReturnHeader = By.CssSelector("#task-menu > div:nth-child(1) > div > div:nth-child(8) > div.panel-heading > h4 > a > span");
            public static readonly By ManageStatutoryReturnButton = By.CssSelector(string.Format(".shell-task-menu-item[data-ajax-url=\"/{0}/StatutoryReturn/StatutoryReturnLog/Details\"]", TestDefaults.Default.Path));
            public static readonly By AttendanceHeader = SeleniumHelper.SelectByDataAutomationID("section_menu_Attendance");
            public static readonly By SchoolGroupsHeader = By.CssSelector("a[data-automation-id='section_menu_School Groups']");
            public static readonly By AllocatePupilToGroupButton = By.CssSelector(string.Format(".shell-task-menu-item[data-ajax-url=\"/{0}/BulkPupils/AllocatePupilsToGroups/Details\"]", TestDefaults.Default.Path));
            public static readonly By FuturePupilsButton = By.CssSelector(string.Format(".shell-task-menu-item[data-ajax-url=\"/{0}/BulkPupils/AllocateNewIntake/Details\"]", TestDefaults.Default.Path));
            public static readonly By EditMarklink = SeleniumHelper.SelectByDataAutomationID("task_menu_section_attendance_EditMarks");
            public static readonly By TakeRegisterlink = SeleniumHelper.SelectByDataAutomationID("095182fc-ace5-4a09-a090-31f259de2887");
            public static readonly By DataOutHeader = By.CssSelector("#task-menu > div:nth-child(1) > div > div:nth-child(7) > div.panel-heading > h4 > a > span");
            public static readonly By CtfExportButton = By.CssSelector(string.Format(".shell-task-menu-item[data-ajax-url=\"/{0}/DataOut/SIMS8CTFExportTripletCTFExportLog/Details\"]", TestDefaults.Default.Path));
            public static readonly By CbaExportButton = By.CssSelector(string.Format(".shell-task-menu-item[data-ajax-url=\"/{0}/DataOut/SIMS8CBAExportTripletCBAExport/Details\"]", TestDefaults.Default.Path));
            public static readonly By LopExportButton = By.CssSelector(string.Format(".shell-task-menu-item[data-ajax-url=\"/{0}/DataOut/SIMS8LevelsOfProgressionExportMaintenanceTripleLoPExport/Details\"]", TestDefaults.Default.Path));
            public static readonly By DataInHeader = By.CssSelector("a[data-automation-id='section_menu_Data In']");
            public static readonly By CtfImportButton = By.CssSelector(string.Format(".shell-task-menu-item[data-ajax-url=\"/{0}/DataIn/SIMS8CtfImportMaintenanceTripletCTFImportLog/Details\"]", TestDefaults.Default.Path));
            public static readonly By LopImportButton = By.CssSelector(string.Format(".shell-task-menu-item[data-ajax-url=\"/{0}/DataIn/SIMS8LevelsOfProgressionImportMaintenanceTripleLoPImport/Details\"]", TestDefaults.Default.Path));
        }

        public struct AdditionalColumns
        {
            public static readonly By AllocatePupilGlobalSearchItem = By.CssSelector(string.Format("a[data-ajax-url='/{0}/BulkPupils/AllocatePupilsToGroups/Details']", TestDefaults.Default.Path));
            public static readonly By FuturePupilsGlobalSearchItem = By.CssSelector(string.Format("a[data-ajax-url='/{0}/BulkPupils/AllocateNewIntake/Details']", TestDefaults.Default.Path));
           // public static readonly By AdditionalColumnButton = SeleniumHelper.SelectByDataAutomationID("additional_columns_button");
            
            public const string YearGroupCssSelectorToFind = "input[data-checkbox-tree-parent='true']";
            public const string AdditionalColumnsButtonToFind = "a[data-show-additional-columns-modal]";
            public const string OkButton = "button[title='OK']";

            public static readonly By CheckboxClick = By.XPath("//div[@id='editablecolumntreenode']/div/div/div/div[1]/div[2]/div[3]/div/input");
            public static readonly By AdmissionNumberCheckBox = By.XPath("//div[@id='editablecolumntreenode']/div/div/div/div[2]/div[2]/div/div/input");
            public static readonly By ModalDialogBox = By.CssSelector("div[class='modal fade dialog-small']");
            public static readonly By CheckBox = By.CssSelector("input[type='checkbox']");
            public static readonly By EditableColumnTreeNode = By.CssSelector("div[id='editablecolumntreenode']");
            public const string AdditionalColumnButtonToFind = "a[data-show-additional-columns-modal='']";
            public const string SearchButtonToFind = "button[type='submit']";
            
        }

        public struct CtfExport
        {
            public const string CreateButtonToFind = "a[title='Create the Record']";
            public const string SearchCtfDestinationButton = "button[data-ajax-url*='DataOut/CTFExportDestinationSearchAndResults/AddSearchResults']";
            public const string SearchExportDestinationButton = "button[data-ajax-url*='DataOut/CTFExportDestinationSearchAndResults/Search']";
            public static readonly By SearchRecordsToFindtext = By.CssSelector("a.search-result.h1-result");
            public static readonly By OkButton = By.CssSelector("button[data-ajax-url*='DataOut/CTFExportDestinationSearchAndResults/ValidateSelection']");
            public static readonly By CtfExportTypeSelector = By.CssSelector("[name=\"CtfPurpose.dropdownImitator\"]");
            public static readonly By CtfDestinationSelector = By.CssSelector("input[name='Destination.dropdownImitator']");
            
            public static readonly By AddPupilLink = By.CssSelector("button[title='Add New Linked Pupils']");
            public static readonly By SearchPupilButton = By.XPath("//*[@id='dialog-palette-editor']/div[2]/div/div[2]/div/div[1]/div/form/div[2]/button");
            public static readonly By AddButton = By.CssSelector("div.btn-group.btn-group-lg.btn-block > button");
            public static readonly By PupilSelectorOkButton = By.CssSelector("#dialog-palette-editor>div:nth-child(2)>div>div:nth-child(3)>button");
            public static readonly By GenerateButton = By.LinkText("Generate");

            public const string DETAIL_PANEL_FORM_ID = "editableData";
            public const string EXPORT_DESTINATION_DIALOG_TITLE_AUTOMATION_ID = "pick_export_destination_popup_header_title";
            public const string PUPIL_RECORD_QUICK_LINK_AUTOMATION_ID = "quicklinks_top_level_pupil_submenu_pupilrecords";

            public const string PICK_EXPORT_DESTINATION_SEARCH_RESULTS_CONTAINER_SECTION_ID = "dialog-searchResults";
            public const string PICK_EXPORT_DESTINATION_SEARCH_RESULT_ITEM_AUTOMATION_ID = "resultTile";
            public const string PICK_EXPORT_DESTINATION_OK_BUTTON_AUTOMATION_ID = "ok_button";
        }

        public struct CbaExport
        {
            public static readonly By CreateButton = By.CssSelector("a[data-ajax-url*='DataOut/SIMS8CBAExportTripletCBAExport/CreateDetail']");
            public static readonly By AddPupilLink = By.CssSelector("button[data-ajax-url*='DataOut/SIMS8CBAExportPupilPicker/AddPupilPickerDialog']");
            public static readonly By GenerateButton = By.CssSelector("a[data-automation-id='well_know_action_save']");
            public static readonly By UploadCbaSearchButton = By.CssSelector("button[data-ajax-url*='SIMS8CBAExportPupilPicker/Search']");
            public static readonly By OkButton = By.CssSelector("button[data-ajax-url*='DataOut/SIMS8CBAExportPupilPicker/ValidateSelection']");
            public static readonly By SearchRecordsToFindtext = By.CssSelector("a.search-result.h1-result");
            public static readonly By PrimaryClassToFind = By.CssSelector("input[class='checkbox']");
            public static readonly By AddSelectedButton = By.CssSelector("div.btn-group.btn-group-lg.btn-block > button");
            public static readonly By Result = By.CssSelector("[data-section-id='searchResults']");
            public static readonly By ResultCba = By.CssSelector("[data-section-id='searchResults']>div>div>span");
            public static readonly By SelectCba = By.CssSelector("[data-section-id='searchResults']>div>div:nth-child(2)>div>div>a");
            public static readonly By CbaStatus = By.CssSelector("[data-section-id='searchResults']>div>div:nth-child(2)>div>div>a[title='CBA Export Status']");
            public const string CreateButtonToFind = "a[title='Create the Record']";
            public const string SearchButtonToFind = "button[type='submit']";
            public const string GenerateButtonToFind = "a[title='Save Record']";
        }

        public struct LopExport
        {
            public static readonly By SearchRecordsToFindtext = By.CssSelector("div.search-result-tile-detail>a[title='Academic Year']");
            public static readonly By SearchButtonLoaded = By.CssSelector("button.btn.btn-primary.btn-block");
            public static readonly By SearchArea = By.CssSelector(".search-criteria-form");
            public static readonly By SearchButton = By.CssSelector(".search-criteria-form-action>button:nth-child(1)");
            public static readonly By SelectLop = By.CssSelector("[data-section-id='searchResults']>div>div:nth-child(2)>div>div>a");
            public static readonly By GenerateLop = By.CssSelector("a[title='Generate Levels of Progression Export']");
            public static readonly By CreateLop = By.CssSelector("a[title='Create new Levels of Progression Export']");
            public static readonly By LopStatus = By.CssSelector("[data-section-id='searchResults']>div>div:nth-child(2)>div>div>a[title='LOP Export File Status']");
            public static readonly By AcademicYearSelector = By.CssSelector("label[title='Academic Year']+div>input");
            public static readonly By TotalPupilsTextBox = By.CssSelector("input[name='TotalPupils']");
        }

        public struct Import
        {
            public static readonly By ImportButton = By.CssSelector("a[title^=Import]");
            public static readonly By UploadToSharePoint = By.CssSelector("[data-ajax-workspace-targetid='palette-editor-container']");
            public static readonly By OkButton = By.CssSelector("button[data-ajax-url*='SIMS8CTFSharepointPicker/ValidateSelection']");
            public static readonly By UploadLOPOkButton = By.CssSelector("button[data-ajax-url*='SIMS8LOPSharepointPicker/ValidateSelection']");

            public static readonly By UploadSearchButton = By.CssSelector("button[data-ajax-url*='SIMS8CTFSharepointPicker/Search']");
            public static readonly By UploadLOPSearchButton = By.CssSelector("button[data-ajax-url*='SIMS8LOPSharepointPicker/Search']");

            public static readonly By FirstRecordSelect = By.CssSelector("#dialog-editableData .search-result-tile-detail>a");
            public static readonly By SearchBtnDisabled = By.CssSelector("[data-section-id='dialog-searchCriteria'] button.disabled");
            public static readonly By SearchBtnEnabled = By.CssSelector("[data-section-id='dialog-searchCriteria'] button");

            public static readonly By UploadFromSharePoint = By.CssSelector("button[data-ajax-url*='SIMS8LOPSharepointPicker/AddSearchResults");
        }

        public struct EditMark
        {
            public static readonly By EditMarkGridColumns = By.CssSelector("div[view_id='registerGrid']>div:nth-child(1)>div:nth-child(2)>table>tbody>tr:nth-child(3)");
            public static readonly By EditMarkGrid = By.CssSelector("div[view_id='registerGrid']");
            public const string WholeSchool = "#tri_chkbox_WholeSchool";
            public const string SearchButton = "search_criteria_submit";
            // public static readonly SearchButton = SeleniumHelper.SelectByDataAutomationID()

            public const string Attendances = "[webix_tm_id='Attendance']>input";
            public const string MovePrevious = "[title='Move Previous']";
           // public const string MovePrevious = "button[class='btn btn-default']";
           // public static readonly MovePrevious = SeleniumHelper.SelectByDataAutomationID("move_previous_button");
            public static readonly By OkButton = By.CssSelector("div[data-additional-columns-dialog] div[class='modal-footer'] button[title='OK']");

            public static readonly By EditableColumnTreeNode = By.CssSelector("div[id='editablecolumntreenode']");
            public static readonly By CheckBox = By.CssSelector("input[type='checkbox']");
        }

        public struct Deni
        {
            public static readonly By VersionSelector = By.CssSelector("[placeholder='Version']");
            public static readonly By CreateReturnOkButton =
                By.CssSelector("[data-section-id='palette-editor-container'] >div > div > div >div>[title='OK']");
        }

        public struct StaffRecord
        {
            public const string MedicalNotesGrid = "table[data-maintenance-container='StaffMedicalNotes']";
            public const string DocumentNotesGrid = "table[data-maintenance-container='StaffNotes']";
            public static string StafftHeader ="#task-menu > div:nth-child(1) > div > div:nth-child(13) > div.panel-heading > h4 > a > span";
            public static string StaffRecordScreen = ".shell-task-menu-item[data-ajax-url*=\"Staff/SIMS8StaffMaintenanceTripleStaff/Details\"]";
            public static readonly By AttachmentDialog = By.CssSelector("[data-section-id='dialog-palette-editor']");
            public const string AttachmentGrid = "div[id='DocumentStoreFiles']";
            public static By MedicalViewDocGrid = By.CssSelector("div[view_id='cxgridDocumentStoreFiles']>div:nth-child(2)>div:nth-child(2)>div>div>div>a>span");
        }

        public struct PupilRecord
        {
            public const string MedicalNotesGrid = "table[data-maintenance-container='LearnerMedicalNotes']";
            public const string DocumentNotesGrid = "table[data-maintenance-container='LearnerNotes']";
        }

      
    }
}