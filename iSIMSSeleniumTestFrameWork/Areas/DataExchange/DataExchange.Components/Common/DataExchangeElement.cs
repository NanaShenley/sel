using System;
using OpenQA.Selenium;
using SharedComponents.Helpers;

namespace DataExchange.Components.Common
{
    /// <summary>
    /// This class is responsible to define constansts for Data Exchange
    /// </summary>
    public static class DataExchangeElement
    {
        public const string DENI = "DENI";
        public const string DENIVersion = "2015";

        public const string CensusTestTargetVersion = "Spring 2017";
        public const string SummerCensusTestTargetVersion = "Summer 2017";
        public const string CensusTestTargetType = "School Census Return";
        public const string MenuTopLevel = "Tasks";
        public const string MenuCategory = "Statutory Return";
        public const string MenuItem = "Manage Statutory Returns";
        public const string CensusGroup = "Census";

        public const string SignOffButton = "well_know_action_save";

        public static readonly By CreateButton = By.CssSelector("a[title^=Create]");
        public static readonly By DetailReportsButton = By.CssSelector("a[title^='Select a report to view']");
        public static readonly By DialogSelector = By.CssSelector("[data-dialog='editor-dialog']");
        public static readonly By VersionSelector = By.CssSelector("[placeholder='Version']");
        public static readonly By CreateReturnOkButton = By.CssSelector("[data-automation-id='ok_button']");
        public static readonly By PupilDeleteButton = By.CssSelector("[data-automation-id='pupil_delete_button']");
        public static readonly By PupilConfirmationButton = By.CssSelector("[data-automation-id='continue_with_delete_button']");
        public static readonly By ConfirmationButton = By.CssSelector("[data-automation-id='cancel_button']");
        public static readonly By VersionDropdown = By.CssSelector("[name=\"StatutoryReturnVersion.dropdownImitator\"]");
        public static readonly By ProcessingMessage = By.CssSelector(".media-heading");
        public static readonly By SearchCriteria = By.CssSelector("form[data-section-id='searchCriteria']");
        public static readonly By SearchReturnVersionSelector = By.CssSelector("[name=\"StatutoryReturnVersionSelector.dropdownImitator\"]");
        public static readonly By SearchButton = By.CssSelector("button[type='submit']");
        public static readonly By NotificationAlert = By.CssSelector("[role='alert']");
        public static readonly By SearchResults = By.CssSelector("div[data-section-id='searchResults'] a[data-automation-id='resultTile']");
        public static readonly By PupilRecordLeaverCheckBox = By.CssSelector("#tri_chkbox_StatusFormerCriterion");
        public const string CloseTabButton = "#shell-footer > div > div > div > ul > li.current > a.layout-col.tab-close > i";
        public const string AssessmentKeyStage = "section_menu_Assessment Key Stage 1 Result";
        public const string SearchCriteriaStatutoryVersion = "StatutoryReturnVersion.dropdownImitator";
        public static readonly By AdmissionNumber = By.CssSelector("#AssessmentKeyStage1ResultSection_AssessmentKeyStage1Results td[column='2'] span");

        public static readonly By UPN = By.CssSelector("#AssessmentKeyStage1ResultSection_AssessmentKeyStage1Results td[column='3'] span");

        public const string LeaverGrid = "section_menu_Leaver";

        public const string EarlyYearProvisionGrid ="section_menu_Early Years Provision";

        public const string EarlyYearProvisionIsNurseryFloodFillClick = "header_menu_Is Nursery";

        public static readonly By LeaverGridAdmissionNumber = By.CssSelector("#LeaversPupilSection_LeaversPupils td[column='2'] span");

        public static readonly By LeaverGridUpn = By.CssSelector("#LeaversPupilSection_LeaversPupils td[column='3'] span");

        public const string LeaverSenGrid = "section_menu_Leavers SEN";

        public static readonly By LeaverSenGridAdmissionStatusSenStatus = By.CssSelector("#LeaversSENSection_LeaversSENStatuses td[column='2'] span");
        public static readonly By LeaverSenGridUpnsenStatus = By.CssSelector("#LeaversSENSection_LeaversSENStatuses td[column='3'] span");
        public static readonly By LeaverSenNeedAdmissionStatus = By.CssSelector("#LeaversSENSection_LeaversSENNeeds td[column='2'] span");
        public static readonly By LeaverSenNeedUpn = By.CssSelector("#LeaversSENSection_LeaversSENNeeds td[column='3'] span");

        public static readonly By SearchResultRecords = By.CssSelector(string.Format("{0} {1}", SeleniumHelper.AutomationId("search_results"), SeleniumHelper.AutomationId("resultTile")));

        public const string OnRollPupilGrid = "section_menu_On Roll Pupils";
        public static readonly By OnRollPupilAdmissionNumber = By.CssSelector("#OnRollPupilSection_OnRollPupils td[column='2'] span");
        public static readonly By OnRollPupilUpn = By.CssSelector("#OnRollPupilSection_OnRollPupils td[column='3'] span");

        public const string OnRollPupilSEN = "section_menu_On Roll Pupil SEN";
        public static readonly By OnRollPupilSENAdmissionNumber = By.CssSelector("#OnRollPupilSENSection_OnRollPupilSENStatuses td[column='2'] span");
        public static readonly By OnRollPupilSENUPN = By.CssSelector("#OnRollPupilSENSection_OnRollPupilSENStatuses td[column='3'] span");

        public static readonly By EarlyYearProvisionIsNursery = By.CssSelector("#EarlyYearsProvisionSection_EarlyYearsProvisions td[column='6'] span");
        public static readonly By EarlyYearProvisionIsNurseryFloodFill = By.CssSelector("#EarlyYearsProvisionSection_EarlyYearsProvisions td[column='6'] [class='fa fa-caret-down fa-fw high-volume-grid-spreadsheet-menu']");
        public static readonly By EarlyYearProvisionIsNurseryFloodFillCheckbox = By.CssSelector("div[data-menu-column-id='_IsNursery'] input[type='checkbox']");
        public static readonly By EarlyYearProvisionIsNurseryFloodFillApplyToSelected = By.CssSelector("div[data-menu-column-id='_IsNursery'] span[class='btn btn-default']");


        public static readonly By LeaverSuName = By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='0'] div[class='webix_cell']");
        public static readonly By LeaverEducationSite = By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='7'] div[class='webix_cell']");
        public static readonly By LeaverFSMEligible = By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='11'] div[class='webix_cell']");
        public static readonly By LeaverData = By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='23'] div[class='webix_cell']");
        public static readonly By LeaverZeroRated = By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='24'] div[class='webix_cell']");
        public static readonly By LeaverSpecialUnit = By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='27'] div[class='webix_cell webix_cell_select']");


        public static readonly By OnRollEducationSite = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='7'] div[class='webix_cell']");
        public static readonly By OnRollIncomeSupport = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='9'] div[class='webix_cell']");
        public static readonly By OnRollIrishMedium = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='10'] div[class='webix_cell']");
        public static readonly By OnRollIsDisabled = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='13'] div[class='webix_cell']");
        public static readonly By OnRollFSMEligible = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='18'] div[class='webix_cell']");
        public static readonly By OnRollIsBorder = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='21'] div[class='webix_cell']");
        public static readonly By OnRollCompositeClass = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='24'] div[class='webix_cell']");
        public static readonly By OnRollFeePayer = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='31'] div[class='webix_cell']");
        public static readonly By OnRollUniformGrant = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='38'] div[class='webix_cell']");
        public static readonly By OnRollZeroRated = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='39'] div[class='webix_cell']");
        public static readonly By OnRollSpecialUnit = By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='42'] div[class='webix_cell']");
        public static readonly By IssuesAndQueriesReport = By.CssSelector("[data-automation-id='Error_Report_DENI2015']");

        public static readonly string DeletePupilSearchResultFormat = "div[data-ajax-url='/{0}/Pupils/SIMS8DeletePupilScreenLearner/ReadDetail/{1}']";

        public static By DeletePupilResult(string path,string id)
        {
            return By.CssSelector(String.Format(DeletePupilSearchResultFormat, path, id));
        }
        public static By DeletePupilMenuSelect(string path)
        {
            return By.CssSelector(String.Format(DeletePupilMenu, path));
        }

        public static readonly string DeletePupilMenu =".shell-task-menu-item[data-ajax-url=\"/{0}/Pupils/SIMS8DeletePupilScreenLearner/Details\"]";
    }
}
