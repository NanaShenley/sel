using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.HomePages;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using Assessment.Components;
using Assessment.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using SharedComponents;



namespace Assessment.Components.Common
{
    public struct MarksheetConstants
    {
        public static By CreateMarksheetMenu = By.LinkText("Set Up Marksheets and Parental Reports");
        public static By CreateMarksheetMenuById = By.CssSelector("[data-automation-id='d422a89e-26d0-4c8f-a4c0-a34fdebbe91f']");

        public static By AssessmentsLink1 = By.LinkText("Assessments Columns");
        public static By MarksheetTitle = By.CssSelector("span[data-automation-id='marksheets_header_display_name']");

        public readonly static string UrlUndertest = TestSettings.Configuration.GetSutUrl();
        public readonly static string Testuser = TestDefaults.Default.TestUser;
        public readonly static string Password = TestDefaults.Default.Password;
        public readonly static string SchoolName = TestDefaults.Default.SchoolName;
        public static readonly int TenantId = TestDefaults.Default.TenantId;
        public static readonly int Timeout = 30;

        //Types of Marksheets
        public static By NewFromExisting = By.CssSelector("a[data-automation-id='create_from_existing_marksheet'] span[class='menu-item-text-title']");
        public static By MarksheetWithLevels = By.CssSelector("a[data-automation-id='marksheet_with_level'] span[class='menu-item-text-title']");
        public static By AssignTemplateGroupAndFilter = By.CssSelector("a[data-automation-id='template_marksheet_filter'] span[class='menu-item-text-title']");
        public static By MarksheetWithLevelsNew = By.CssSelector("a[data-automation-id='marksheet_with_level_new']");
        public static By ProgrammeOfStudyTracking = By.CssSelector("a[data-automation-id='program_of_study_tracking' span[class='menu-item-text-title']]");
        public static By TrackingGrid = By.CssSelector("a[data-automation-id='program_of_study_tracking'] span[class='menu-item-text-title']");
        public static By NewFromExistingPalette = By.XPath("//div[@id='palette-editor']/div[2]/div/div/h4");

        public static By ModifyMarksheet = By.CssSelector("a[data-automation-id='marksheet_with_level_modify']");        
        public static By CopyTemplate = By.CssSelector("a[data-automation-id='marksheet_with_level_copy'] span[class='menu-item-text-title']");

        public static By PropertiesTab = By.CssSelector("[data-automation-id='properties_tab']");
        public static By GenericOkButton = By.CssSelector("[data-automation-id='ok_button']");
        public static By LevelColumn = By.CssSelector("div[column='7']");
        public static By GradesetColumn = By.CssSelector("div[column='6']");
        public static By ColumnCell = By.CssSelector("div[class*=\"webix_cell\"]");
        public static By DailogSubtitle = By.CssSelector("[data-dialog-subtitle]");
        public static By ModalDailog = By.CssSelector("div[class$='modal-dialog layout-page']");
        public static By SearchCriteriaButton = By.CssSelector("button[data-automation-id='search_criteria_submit']");
        public static By SearchResultNameList = By.CssSelector("div[data-automation-id='search_results'] a[data-automation-id='resultTile']");
        public static By ColourColumn = By.CssSelector("div[column='8']");
        public static By ColumnEditedCell = By.CssSelector("div[class='webix_dt_editor open']");
        public static By ColumnEditedCellUp = By.CssSelector("div[class='webix_dt_editor open dropup']");
        public static By Applygradeset = By.CssSelector("[data-apply-bulkupdate]");
        //Types of Marksheets Labels
        public const string NewFromExistingLabel = "Clone Marksheet";
        public const string MarksheetWithLevelsLabel = "New Template";
        public const string ProgrammeOfStudyTrackingLabel = "Programme of Study Tracking";
        public const string TrackingGridLabel = "Tracking Grid";
        public const string NewFromExistingPaletteTitle = "New from existing - Marksheets ";


        public static By CreateMarksheetButton = By.LinkText("Create Marksheet");
        public static By MarksheetBuilderTitle = By.CssSelector("div[data-slide-panel-id='home'] div.slider-header-title");
        public static By AspectPalletteSearch = By.CssSelector("button[data-ajax-url*='CreateMarksheetAspectPicker/Search']");
        public static By AssessmentPerPalletteSearch = By.CssSelector("button[data-ajax-url*='SIMS8AssessmentPeriod/Search']");
        public static By MarksheetActive = By.CssSelector("input[id='tri_chkbox_Active']");
        public static By MarksheetFilteredBy = By.CssSelector("input[name*=\"FilteredBy\"]");

        public const string Savebtn = "[title='Save Record']";
        public const string MarksheetNameLabel = "Test Marksheet Name";
        public const string MarksheetDescriptionLabel = "Marksheet Description for this marksheet template - Test";

        //Column Defination
        public static By CreateColDefButton = By.CssSelector("button[title='Create column definition']");
        public static By ColDefNewRow = By.CssSelector("button[title='Delete this row?']");

        //Additional Column CSS selector
        public static By AdditionalColumnName = By.CssSelector("[name*=\"Name\"]");
        public static By AdditionalColumnGridRow = By.CssSelector("[data-maintenance-container=\"AdditionalColumnSpecifications\"]");
        public static By AdditionalColumnWidth = By.CssSelector("[name*=\"dropdownImitator\"]");
        public static By AdditionalColumnSelect = By.CssSelector("[name*=\"tri_chkbox_AdditionalColumnSpecifications\"]");
        public static By AdditionalColumnSelectvalue = By.XPath("//div[@class='select2-result-label']");

        //Additional Column values
        public const string AdditionalcolumnAge = "Age (YY/MM)";
        public const string AdditionalcolumnDate = "Date of Birth";
        public const string AdditionalcolumnGender = "Gender";
        public const string AdditionalcolumnAdmission = "Admission Number";
        public const string AdditionalcolumnEthnicity = "Ethnicity";
        public const string AdditionalcolumnDateAdmission = "Date of Admission";
        public const string AdditionalcolumnLanguage = "Language";
        public const string AdditionalcolumnYear = "Current Year Group";
        public const string AdditionalcolumnMeals = "Free School Meals";
        public const string AdditionalcolumnLAC = "Looked After (LAC)";
        public const string AdditionalcolumnStatus = "SEN Status";
        public const string AdditionalcolumnNeed = "SEN Need";
        public const string AdditionalcolumnClass = "Class";
        public const string AdditionalcolumnNC = "Current NC Year";
        public const string AdditionalcolumnNewcomer = "Newcomer";
        public const string AdditionalcolumnAsylumSeeker = "Asylum Status";
        public const string AdditionalcolumnPercentage = "Percentage Attendance";

        public static string[] AdditionalColumnValues = new string[]
        {
            "Admission Number",
            "Age (YY/MM)",
            "Asylum Status",
            "Class",
            "Current NC Year",
            "Current Year Group",
            "Date of Admission",
            "Date of Birth",
            "Ethnicity",
            "Free School Meals",
            "Gender",
            "Language",
            "Looked After (LAC)",
            "Newcomer",
            "Percentage Attendance",
            "SEN Need",
            "SEN Status"
        };

        //Assign Groups
        public static string[] YearGroupsValues = new string[]
        {
            "Year N",
            "Reception",
            "Year 1",
            "Year 2",
            "Year 3",
            "Year 4",
            "Year 5",
            "Year 6",
            "Year 7"
        };

        public static string[] ClassesValues = new string[]
        {
            "Robin",
            "Wren",
            "Jay",
            "Blackbird",
            "Moorhen",
            "Swallow",
            "Brambling",
            "Magpie",
            "Puffin",
            "Chaffinch",
            "Jackdaw",
            "Linnet",
            "Cuckoo"
        };
        public static string[] NCYearValues = new string[]
        {
            "Curriculum Year N",
            "Curriculum Year R",
            "Curriculum Year 1",
            "Curriculum Year 2",
            "Curriculum Year 3",
            "Curriculum Year 4"
        };
        public static string[] EthnicityValues = new string[]
        {
            "Black-African",
            "Black-Caribbean",
            "Bangladeshi",
            "Black-Other",
            "Chinese/Hong Kong"
        };
        public static string[] LanguageValues = new string[]
        {
            "Afrikaans",
            "Akan/Twi-Fante",
            "Albanian/Shqip",
            "Arabic",
            "Belarusian"
        };
        public static string[] SchoolIntakeValues = new string[]
        {
            "2015/2016 - Spring Year 1",
        };

        public static string[] SenNeedTypeValues = new string[]
        {
            "ADD/ADHD",
            "Anaphylaxis",
            "Aspergers",
            "Asthma",
            "Autism",
        };
        public static string[] UserDefinedGroupValues = new string[]
        {
            "Test User Defined Group"
        };
        public static string[] TeachingGroupValues = new string[]
        {
            "Test Teaching Group"
        };
        public static string[] SENStatusValues = new string[]
        {
            "0-Provision no longer needed",
            "1-Identify need",
            "2-In-School provision",
            "3-Involve o/s agencies",
            "4-ELB Assessment"
        };
        public static By GroupList = By.CssSelector("div[data-slide-panel-id='groups'] > div.layout-page.form-panel > div.form-body > div.layout-page.pane-body > div > div.slider-body > div.wrap.col-fill > div.checkbox-tree.wrap > div.container-fluid > div.row > div.panel-group.accordion-inline > div.panel-default.panel > div.panel-collapse.collapse.in > div.panel-body > div.checkboxlist > div.checkboxlist-column");
        public static By YearGroupsCheckBox = By.CssSelector("input[name='YearGroups.SelectedIds']");
        public static By ClassesCheckBox = By.CssSelector("input[name='Classes.SelectedIds']");
        //  public static readonly By SelectGroups = By.LinkText("+ Groups");

        //Validations on Create Marksheet Page
        public const string ValNameWarning = "Marksheet Template Name is required";
        public const string ValAssessmentNameWarning = "Assessment Name is required";
        public const string ValAssessmentPeriodWarning = "Assessment Period Name is required";
        public static By ValName = By.XPath("//div[@class='validation-summary-errors']/ul/li[3]");
        public static By ValAssessment = By.CssSelector("div.validation-summary-errors > ul > li");
        public static By ValAssessmentPeriod = By.XPath("//div[@class='validation-summary-errors']/ul/li[2]");

        //Marksheet Properties
        public static By MarksheetName = By.CssSelector("input[data-automation-id='marksheet-name']"); //By.CssSelector("[name='Name']");
        public static By MarksheetDescription = By.CssSelector("textarea[data-automation-id='marksheet-description']");//By.XPath("//textarea[@name='Description']");
        public static By AspectName = By.CssSelector("a[title='Assessment Name']");
        public static By AssessmentPeriodTitleName = By.CssSelector("a[title='Name']");
        public static By MarksheetProperties = By.CssSelector("button[class*='marksheet-toggle collapsed']");

        //Slider Navigation control
        public const string MarksheetBuilderAspectsLabel = "Add Aspects";
        public const string MarksheetBuilderTitleLabel = "Marksheet Builder";
        public const string MarksheetBuilderAPLabel = "Add Assessment Periods";
        public const string AspectSearchResultLabel = "No Matches";
        public static readonly By resultTile = By.CssSelector("a[data-automation-id='resultTile']");
        public static readonly By SearchAssessmentResult = By.CssSelector("div[data-section-id='marksheets-aspect-searchResults'] a[data-automation-id='resultTile']");

        public static readonly By GroupsCloseButton = By.CssSelector("div[data-slide-panel-id='groups'] a[class='btn btn-link'] [data-automation-id='groupclosebutton']");
        public static readonly By GroupsBackButton = By.CssSelector("div[data-slide-panel-id='groups'] [data-marksheet-groups-back]");
        public static readonly By GroupFilterCloseButton = By.CssSelector("div[data-slide-panel-id='group-filter'] a[class='btn btn-link'] [data-automation-id='groupfilterclosebutton']");
        public static readonly By GroupFilterBackButton = By.CssSelector("div[data-slide-panel-id='group-filter'] [data-marksheet-groupfilter-back]");

        //Close buttons on slider control
        public static readonly By AspectCloseButton = By.CssSelector("i[data-automation-id='aspectclosebutton']");
        public static readonly By AspectAssessmentPeriodCloseButton = By.CssSelector("i[data-automation-id='aspectassessmentperiodclosebutton']");
        public static readonly By SubjectCloseButton = By.CssSelector("i[data-automation-id='subjectclosebutton']");
        public static readonly By ModeCloseButton = By.CssSelector("i[data-automation-id='modeclosebutton']");
        public static readonly By SubjectAssessmentPeriodCloseButton = By.CssSelector("i[data-automation-id='subjectassessmentperiodclosebutton']");
        public static readonly By SubjectBackButton = By.CssSelector("i[data-automation-id='subject-back']");
        public static readonly By ModeBackButton = By.CssSelector("i[data-automation-id='mode-back']");
        public static readonly By SubjectAssessmnetPeriodBackButton = By.CssSelector("i[data-automation-id='subject-assessment-period-back']");
        public static readonly By AspectBackButton = By.CssSelector("i[data-automation-id='aspect-back']");
        public static readonly By AspectAPBackButton = By.CssSelector("i[data-automation-id='aspect-assessment-period-back']");
        public static readonly By SelectAssessments = By.LinkText("Assessments Columns");
        public static readonly By SelectAspectsLink = By.LinkText("Assessments");
        public static readonly By SelectSubjectsLink = By.LinkText("Subjects");

        //Next and done buttons on slider control
        //public static readonly By AspectNextButton = By.CssSelector("[class='btn btn-primary btn-block btn-lg']");
        public static readonly By AspectNextButton = By.CssSelector("button[data-automation-id='next-aspects-periods']");
        public static readonly By AspectDoneButton = By.CssSelector("button[data-automation-id='done-aspect-periods']");
        public static readonly By SubjectDoneButton = By.CssSelector("button[data-automation-id='done-subject-periods']");
        public static readonly By SubjectNextButton = By.CssSelector("button[data-automation-id='next-subject-periods']");
        public static readonly By SubjectPeriodNextButton = By.CssSelector("button[data-automation-id='next-type-periods']");

        public static readonly By AspectModeList = By.CssSelector("div[data-automation-id='marksheet-mode-type'] div.list-group-item.search-result-tile");
        public static readonly By AspectMethodList = By.CssSelector("div[data-automation-id='marksheet-method-type'] div.list-group-item.search-result-tile");
        public static readonly By AspectPurposeList = By.CssSelector("div[data-automation-id='marksheet-purpose-type'] div.list-group-item.search-result-tile");

        //Search buttons on slider control
        public static readonly By SelectSubjectButton = By.CssSelector("div[data-slide-panel='subjects']");
        public static readonly By AssessmentPeriodSearchButton = By.CssSelector("form[data-section-id='marksheets-aspect-assessmentperiod-searchCriteria'] > div.search-criteria-form-action > button.btn.btn-primary.btn-block");
        public static readonly By SubjectSearchButton = By.CssSelector("form[data-section-id='marksheets-subject-searchCriteria'] button[data-automation-id = 'search_criteria_submit']");
        public static readonly By SubjectAssessmentPeriodSearchButton = By.CssSelector("form[data-section-id='marksheets-subject-assessmentperiod-searchCriteria'] button[data-automation-id = 'search_criteria_submit']");

        //  public static By AssessmentPeriodList = By.CssSelector("[class='search-result-tile-detail']");
        //public static By AssessmentPeriodList = By.CssSelector("a[title='Name']");
        public static By AssessmentPeriodList = By.CssSelector("div[data-section-id='marksheets-aspect-assessmentperiod-searchResults'] a[data-automation-id='resultTile']");
        public static By SubjectList = By.CssSelector("div[data-section-id='marksheets-subject-searchResults'] div.list-group-item.search-result-tile");


        //Marksheet Search
        public static readonly By SearchMarksheetPanelButton = By.CssSelector("button[data-toggle='show-left-panel']");
        public static readonly By SearchMarksheetName = By.CssSelector("input[name='MarksheetName']");
        public static readonly By SearchButton = By.CssSelector("button.btn.btn-primary.btn-block");
        public static readonly By SelectedYearGroup = By.CssSelector("[class=\"checkboxlist-checkbox\"]");
        public static readonly By OwnerValue = By.CssSelector("[name$='Owners.dropdownImitator']");
        public static readonly By ShowMore = By.CssSelector("[class=\"btn btn-link btn-block\"]");
        public static readonly By selectMarksheet = By.CssSelector("[class=\".search-result-tile-detail.loaded\"]");
        public static readonly By SearchPanel = By.CssSelector("label[title='Marksheet Name']");
        public static readonly By SearchResultMatches = By.CssSelector("span.result-counter");
        public static readonly By SearchResultText = By.CssSelector("[data-automation-id='resultTile']");
        public static string MarksheetNameText = "Recording Year 6";
        public static readonly By ActiveCheckBox = By.Id("tri_chkbox_Active");
        public static readonly By ShowMoreLink = By.CssSelector("button[data-automation-id='search_criteria_advanced']");

        //Additioanl Column Section
        public static readonly By AdditionalColumnBackButton = By.CssSelector("a[additional-column-back]");
        public static readonly By AdditionalColumnCloseButton = By.CssSelector("[data-automation-id='additionalcolumnclosebutton']");
        public static readonly By SelectAdditioanlColumnLink = By.LinkText("Pupil Details");
        public static readonly By AdditionalColumnDoneButton = By.CssSelector("button[data-automation-id='done-additionalColumn']");
        public static By AdditoanlColumnResult = By.CssSelector("div[marksheets-additionalColumnSpecifications-searchResults'] > div.panel.panel-default > div.panel-heading > span.result-counter");
        public static readonly By AdditoanlColumnElementSelection = By.CssSelector("div[createmarksheet-additionalcolumn-section] a[createmarksheet-additionalcolumn]");



        public static readonly By SelectGroupsLinks = By.LinkText("+ Groups");
        public static By AspectSearchResult = By.CssSelector("div[data-section-id='marksheets-aspect-searchResults'] > div.panel.panel-default > div.panel-heading > span.result-counter");
        public static By AspectAssessmentPeriodSearchResult = By.CssSelector("div[data-automation-id='marksheet-aspect-period-searchResults'] a[data-automation-id='resultTile']");
        public static By APSearchResult = By.CssSelector("div[data-automation-id='marksheet-aspect-period-searchResults'] span[data-automation-id='search_results_counter']");

        public static readonly By AspectAssessmentPeriodNameInput = By.CssSelector("form[data-section-id='marksheets-aspect-assessmentperiod-searchCriteria'] input[data-automation-id='Input-Aspect-Assessment-periods']");
        public static readonly By AssessmentPeriodNameInput = By.CssSelector("form[data-section-id='marksheets-subject-assessmentperiod-searchCriteria'] input[data-automation-id='Input-Aspect-Assessment-periods']");
        public static readonly By AspectNameInput = By.CssSelector("input[name='AspectName']");
        public static readonly By SubjectNameInput = By.CssSelector("input[name='SubjectName']");



        // public static By APSearchResult = By.CssSelector("div[data-section-id='marksheets-aspect-assessmentperiod-searchResults'] > div.panel.panel-default > div.panel-heading > span.result-counter");
        public static By SubjectSearchResult = By.CssSelector("div[data-section-id='marksheets-subject-searchResults'] span[data-automation-id='search_results_counter']");
        public static readonly By AspectElementSelection = By.CssSelector("div[data-automation-id='marksheets-aspect-searchResults'] a[assessment-createmarksheet-aspect]");
        public static readonly By AspectAssessmentPeriodElementSelection = By.CssSelector("div[data-automation-id='marksheet-aspect-period-searchResults'] a[assessment-createmarksheet-period]");
        public static readonly By SubjectElementSelection = By.CssSelector("div[createmarksheet-assessmentsubject-section] a[createmarksheet-assessmentsubject]");
        public static readonly By ModeElementSelection = By.CssSelector("div[data-automation-id='marksheet-mode-type'] a[assessment-createmarksheet-assessmentmode]");
        public static readonly By MethodElementSelection = By.CssSelector("div[data-automation-id='marksheet-method-type'] a[assessment-createmarksheet-assessmentmethod]");
        public static readonly By PurposeElementSelection = By.CssSelector("div[data-automation-id='marksheet-purpose-type'] a[assessment-createmarksheet-assessmentpurpose]");
        public static readonly By SubjectAssessmentPeriodElementSelection = By.CssSelector("div[data-automation-id='marksheets-subject-assessmentperiod-searchResults'] a[assessment-createmarksheet-period]");


        public static readonly By SelectAdditionalColumn = By.LinkText("+ Additional Columns");
        public static By AdditionalColumnsResults = By.CssSelector("div[data-slide-panel-id='additional'] > div.layout-page.form-panel > div.form-body > div.layout-page.pane-body > div > div.slider-body > div.panel.panel-default > div.list-group.search-result-tiles > div.list-group-item.search-result-tile > div.search-result-tile-detail > a.search-result.h1-result");
        public static By AdditionalColumnList = By.CssSelector("a[createmarksheet-additionalcolumn='']");
        public static By TreeOpenGrid = By.CssSelector("[class='webix_tree_close']");

        //Aspect Grid Properties
        public static readonly By AspectGrid = By.CssSelector("[data-maintenance-container='Aspects']");
        public static readonly By AssessmentName = By.CssSelector("[name$='Name']");
        public static readonly By AssessmentPurpose = By.CssSelector("[name$='AssessmentPurpose']");
        public static readonly By AssessmentMode = By.CssSelector("[name$='AssessmentMode']");
        public static readonly By AssessmentMethod = By.CssSelector("[name$='AssessmentMethod']");
        public static readonly By SelectAssessmentButton = By.CssSelector("button[title='Select the assessment from assessment palette']");
        public static readonly By AddSelectedAssessmentsButton = By.CssSelector("button[title='Add Selected']");
        public static readonly By OkButton = By.CssSelector("div.modal-footer.pane-footer.layout-row > button.btn.btn-default");
        //Assessment Period Grid Properties
        public static readonly By AssessmentPeriodGrid = By.CssSelector("[data-maintenance-container='AssessmentPeriod']");
        public static readonly By AssessmentPeriodName = By.CssSelector("[name$='Name']");
        public static readonly By NcYear = By.CssSelector("[name$='NCYear']");
        public static readonly By FormalAssessmentPeriod = By.CssSelector("[name$='FormalAssessmentPeriod']");
        public static readonly By SelectAssessmentPeriodButton = By.CssSelector("button[data-ajax-url*='AddAssessmentPeriodDialog']");

        //Column Defintion Grid Properties
        public static readonly By ColumnDefinitionGrid = By.CssSelector("[data-maintenance-container='ColumnDefinition']");
        public static readonly By AssessmentValue = By.CssSelector("[name$='Aspect.dropdownImitator']");
        public static readonly By AssessmentPeriod = By.CssSelector("[name$='AssessmentPeriods.dropdownImitator']");
        public static readonly By Header = By.CssSelector("[name$='Header']");
        public static readonly By Hidden = By.CssSelector("[name$='IsHidden']");
        public static readonly By Readonly = By.CssSelector("[name$='IsReadOnly']");
        public static readonly By DisplayOrder = By.CssSelector("[name$='DisplayOrder']");
        public static readonly By ColumnWidth = By.CssSelector("[name$='Width.dropdownImitator']");
        public static readonly By AddColumnDefinitionButton = By.CssSelector("button[title='Create column definition']");

        //Gradeset Details
        public static readonly By GradesetDetilsTitle = By.CssSelector("[data-automation-id='view_gradeset_popup_header_title']");


        //Pastoral Groups
        public static By PastoralGroups = By.LinkText("Pastoral Groups");
        public static By YearGroup = By.LinkText("Year Group");
        public static By Class = By.LinkText("Class");

        public static By SelectYearGroups = By.CssSelector("input[name=\"YearGroups.SelectedIds\"]");
        public static By SelectClasses = By.CssSelector("input[name='Classes.SelectedIds']");
        public static By SelectYearGroupslist = By.CssSelector("css=input[id^='SelectedIds']");
        public static By SelectClasseslist = By.CssSelector("input[id='SelectedIds']");

        public static By ExpandYearGroupSelector = By.LinkText("Year Group");

        //Save Marksheet Assertion
        public static By MarksheetSaveMessageCSS = By.CssSelector("div[data-automation-id='status_success']");
        public static By SaveError = By.CssSelector("div[data-automation-id='status_error']");
        public static By ValidationError = By.CssSelector("div.validation-summary-errors");
        public const string MarksheetSaveMessage = "Assessment Marksheet Creation Data record saved";
        public const string SaveErrorMessage = "Validation Warning";


        //Marksheet Preview
        public static By MarksheetPreviewButton = By.CssSelector("button[title='Preview Marksheet']");
        public static int NoOfAdditionalCol = 3;
        //Aspect Listdata-automation-id


        //Preview
        public static By AspectResult = By.CssSelector("div[data-automation-id='marksheets-aspect-searchResults']  div.list-group-item.search-result-tile");
        public static By PeriodResult = By.CssSelector("div[data-automation-id='marksheet-aspect-period-searchResults']  div.list-group-item.search-result-tile");
        public static By SubjectModeResult = By.CssSelector("div[data-automation-id='marksheet-mode-type']  div.list-group-item.search-result-tile");
        public static By SubjectMethodResult = By.CssSelector("div[data-automation-id='marksheet-method-type']  div.list-group-item.search-result-tile");
        public static By SubjectPurposeResult = By.CssSelector("div[data-automation-id='marksheet-purpose-type']  div.list-group-item.search-result-tile");
        public static By SubjectPeriodResult = By.CssSelector("div[data-automation-id='marksheets-subject-assessmentperiod-searchResults']  div.list-group-item.search-result-tile");
        public static By SubjectResult = By.CssSelector("div[data-automation-id='marksheets-subject-searchResults']  div.list-group-item.search-result-tile");
        public static List<string> AspectList = AddAspectList();


        //GroupFilter
        public static By GroupFilterSearchResults = By.CssSelector("div[data-automation-id='marksheets-groupfilter-searchResults']");
        public static By GroupNextButton = By.CssSelector("button[data-automation-id='next-Group']");
        public static By SelectNCYear = By.CssSelector("input[data-automation-id='control_checkboxlist_rootnode_checkbox_NCYear']");
        public static By SelectEthnicity = By.CssSelector("input[data-automation-id='control_checkboxlist_rootnode_checkbox_Ethnicity']");
        public static By SelectLanguage = By.CssSelector("input[data-automation-id='control_checkboxlist_rootnode_checkbox_Language']");
        public static By SelectSchoolIntake = By.CssSelector("input[data-automation-id='control_checkboxlist_rootnode_checkbox_School_Intake']");
        public static By SelectClassesFilter = By.CssSelector("input[name='ClassesFilter.SelectedIds']");
        public static By SelectYearGroupsFilter = By.CssSelector("input[name='YearGroupsFilter.SelectedIds']");
        public static By SelectSenNeedType = By.CssSelector("input[name='SenNeedType.SelectedIds']");
        public static By SelectUserDefined = By.CssSelector("input[name='UserDefined.SelectedIds']");
        public static By SelectTeachingGroup = By.CssSelector("input[name='TeachingGroup.SelectedIds']");
        public static By SelectSenStatus = By.CssSelector("input[name='SenStatus.SelectedIds']");

        public static By NCYear = By.LinkText("Curriculum Year");
        public static By Ethnicity = By.LinkText("Ethnicity");
        public static By Language = By.LinkText("Language");
        public static By SchoolIntake = By.LinkText("New Intake Group");
        public static By SenNeedType = By.LinkText("SEN Need Type");
        public static By UserDefined = By.LinkText("User Defined Group");
        public static By TeachingGroup = By.LinkText("Teaching Group");
        public static By SenStatus = By.LinkText("SEN Status");


        public static readonly By GroupDoneButton = By.CssSelector("button[data-automation-id='done-group']");
        private static List<string> AddAspectList()
        {
            AspectList = new List<string>();
            AspectList.Add("div[data-selectable-id='b5195d94-7ff1-b6fd-0990-3309e8411b6e']");
            AspectList.Add("div[data-selectable-id='e441f18c-cc8a-735e-115b-a321676e1687']");
            return AspectList;
        }
        //Column Definition List
        public static IList<ColumnDefPro> colProperties = GetColumnProperties();
        private static IList<ColumnDefPro> GetColumnProperties()
        {
            //set logic....
            colProperties = new List<ColumnDefPro>();
            //First Column Definition
            var SetColDef = new ColumnDefPro()
            {
                gridrowIdx = 0,
                aspectname = "EXT PRIMARY:CAT:Mean ATN",
                assessmentPeriod = "Year 4 End of Key Stage",
                colHeader = "Header 1",
                isHidden = true,
                isreadonly = true,
                displayOrder = "1",
                columnWidth = "Medium"
            };
            colProperties.Add(SetColDef);
            //Second Column Definition
            SetColDef = new ColumnDefPro()
            {
                gridrowIdx = 1,
                aspectname = "NA PRIMARY:Language and Literacy DIFFERENCE",
                assessmentPeriod = "Year 1 Annual",
                colHeader = "Header 2",
                isHidden = false,
                isreadonly = true,
                displayOrder = "2",
                columnWidth = "Small"
            };
            colProperties.Add(SetColDef);
            //Returning the List of Column Definition.
            return colProperties;
        }

    }

    public struct General
    {
        public static readonly By Screen = By.CssSelector("#screen");
        public static readonly By DataMaintenance = By.CssSelector("#editableData");
        public static readonly By DialogLevel1 = By.CssSelector("#dialog-palette-editor");
        public static readonly By DialogLevel2 = By.CssSelector("#dialog-dialog-palette-editor");
        public static readonly By ValidationDialogLevel2 = By.CssSelector("#dialog-dialog-palette-editor > div.modal-dialog.layout-page > div > div.modal-body.layout-row.pane-body > div > div > div > div.form-body > div > div > div > div > div > div.form-header > div.validation-summary-errors > ul");

        public static readonly By EditButton = By.CssSelector("[title=\"Edit full record\"]");
        public static readonly By GridDeleteButton = By.CssSelector("[title=\"Delete this row?\"]");
        public static readonly By GridDeleteYesButton = By.CssSelector("[class=\"btn btn-warning btn-block\"]");
        public static readonly By GridDeleteNoButton = By.CssSelector("[class=\"btn btn-warning btn-outline btn-block\"]");
        public static readonly By SaveAndContinueButton = By.CssSelector("[data-section-id=\"confirm-SaveContinue\"]");
    }


}
