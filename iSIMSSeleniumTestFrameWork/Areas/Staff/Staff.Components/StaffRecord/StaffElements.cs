using OpenQA.Selenium;
using SharedComponents.Helpers;
using System;
using TestSettings;

namespace Staff.Components.StaffRecord
{
    public class StaffElements
    {
        public struct QuickLinks
        {
            public static readonly By StaffQuickLink = SimsBy.AutomationId("quicklinks_top_level_staff_submenu_staffrecords");
        }

        public struct Tabs
        {
            public const string CloseTabButton = "#shell-footer > div > div > div > ul > li.current > a.layout-col.tab-close > i";
        }

        public static class General
        {
            public static readonly By Screen = By.CssSelector("#screen");
            public static readonly By SaveMessage = By.CssSelector("[class=\"alert-success inline-alert alert animated\"]");
            public static readonly By DataMaintenance = By.CssSelector("#editableData");
            public static readonly By DialogDataMaintenanceLevel1 = By.CssSelector("#dialog-editableData");
            public static readonly By DialogDataMaintenanceLevel2 = By.CssSelector("#dialog-dialog-editableData");
            public static readonly By DialogLevel1 = By.CssSelector("#dialog-palette-editor");
            public static readonly By DialogLevel2 = By.CssSelector("#dialog-dialog-palette-editor");
            public static readonly By DialogLevel3 = By.CssSelector("#dialog-dialog-dialog-palette-editor");            
            public static readonly By ValidationScreen = By.CssSelector("#screen > div > div.layout-col.main.pane > div > div > div > div.form-body div.form-header > div.validation-summary-errors > ul");
            public static readonly By ValidationDialogLevel1 = By.CssSelector("#dialog-palette-editor > div.modal-dialog.layout-page > div > div.modal-body.layout-row.pane-body div.form-header > div.validation-summary-errors > ul");
            public static readonly By ValidationDialogLevel1ByAutomationId = By.CssSelector("[data-automation-id='status_error'] + div.validation-summary-errors > ul");
            public static readonly By ValidationDialogLevel2 = By.CssSelector("#dialog-dialog-palette-editor > div.modal-dialog > div > div.modal-body div.validation-summary-errors > ul");

            public static readonly By DialogLevel1OKButton =
                By.CssSelector(
                    "[data-automation-id='ok_button']");

            public static readonly By DialogLevel2OKButton =
                By.CssSelector(
                    "[data-automation-id='ok_button']");

            public static readonly By DialogLevel1CancelButton =
                By.CssSelector(
                    "[data-automation-id='cancel_button']");

            public static readonly By EditButton = By.CssSelector("[title=\"Edit full record\"]");
            public static readonly By GridDeleteButton = By.CssSelector("[title=\"Delete this row?\"]");
            public static readonly By GridDeleteYesButton = By.CssSelector("[class=\"btn btn-warning btn-block\"]");
            public static readonly By GridDeleteNoButton = By.CssSelector("[class=\"btn btn-warning btn-outline btn-block\"]");
            public static readonly By SearchButton = By.CssSelector("button[type='submit']");
            public static readonly By ConfirmationPopup = By.CssSelector("#palette-editor-container");
            public static readonly By ConfirmDeleteButton = By.CssSelector("[data-automation-id='continue_with_delete_button']");
        }

        #region Staff Record

        public struct StaffRecord
        {
            public static readonly By SearchCritera = SimsBy.AutomationId("search_criteria");
            public static readonly By SearchButton = SimsBy.AutomationId("search_criteria_submit");

            public static readonly By CurrentCriteria = By.CssSelector("[id=\"tri_chkbox_StatusCurrentCriterion\"]");

            public static readonly By LeaverCriteria = By.CssSelector("[id=\"tri_chkbox_StatusFormerCriterion\"]");

            public static readonly By DeleteStaffSearchButton = By.CssSelector("#screen > div > div.layout-col.search.pane > div > div.form-body > div > div > div:nth-child(1) > form > div.search-criteria-form-action > button");

            #region Toolbar

            public static readonly By CreateButton = SimsBy.AutomationId("add_new_staff_button");
            public static readonly By SaveButton = By.CssSelector("#screen > div > div.layout-col.main.pane > div > div > div > div.form-header > div > div > ul > li:nth-child(3) > a");
            public static readonly By SaveButtonByAutomationId = SimsBy.AutomationId("well_know_action_save");
            public static readonly By CancelButton = By.CssSelector("#screen > div > div.layout-col.main.pane > div > div > div > div.form-header > div > div > ul > li:nth-child(6) > a");
            public static readonly By EditButton = By.CssSelector(" #editableData > div > div:nth-child(6) [title='Edit full record']");

            #endregion

            public static readonly By DeleteButton = By.CssSelector("[data-automation-id='delete_button']");
            public static readonly By ConfirmDeleteButton = By.CssSelector("#palette-editor-container > div > div.modal-dialog > div > div.modal-footer > button.btn.btn-default.btn-outline");
            public static readonly string StaffSearchResultFormat ="div[data-ajax-url*='/Staff/SIMS8StaffMaintenanceTripleStaff/ReadDetail/{0}']";
            public static readonly string DeleteStaffSearchResultFormat = "div[data-ajax-url*='/Staff/SIMS8DeleteStaffScreenStaff/ReadDetail/{0}']";           
            public static readonly By AlertBanner = By.CssSelector("div.alert");

            #region Contextual Actions

            public static readonly By StaffLeavingDetails = By.CssSelector("#screen > div > div.layout-col.main.pane > div > div > div > div.form-body > div > div > div > div.layout-col.actions.pane > div > div.form-body > div > div > div:nth-child(1) > div.list-group > a");

            #endregion

            #region Basic Details

            public static readonly By PersonalDetailsAccordion = By.CssSelector("div:nth-child(1) > div.panel-heading > h4 > a > span");
            public static readonly By Title = By.CssSelector("[name=\"Title.dropdownImitator\"]");
            public static readonly By LegalForename = By.CssSelector("[name=\"LegalForename\"]");
            public static readonly By LegalMiddleNames = By.CssSelector("[name=\"LegalMiddleNames\"]");
            public static readonly By LegalSurname = By.CssSelector("[name=\"LegalSurname\"]");
            public static readonly By PreferredForename = By.CssSelector("[name=\"PreferredForename\"]");
            public static readonly By PreferredSurname = By.CssSelector("[name=\"PreferredSurname\"]");
            public static readonly By Gender = By.CssSelector("[name=\"Gender.dropdownImitator\"]");
            public static readonly By DateOfBirth = By.CssSelector("[name=\"DateOfBirth\"]"); 
            public static readonly By MaritalStatus = By.CssSelector("[name=\"MaritalStatus.dropdownImitator\"]");
            public static readonly By QuickNote = By.CssSelector("[name=\"QuickNote\"]");
            
            #endregion

            #region Service Details

            public static readonly By ServiceDetailsAccordion = By.CssSelector("div[data-automation-id='section_menu_Service Details']");
            public static readonly By DOA = By.CssSelector("[name=\"DateOfArrival\"]");           
            public static readonly By DOL = By.CssSelector("[name=\"DateOfLeaving\"]");
            public static readonly By StaffCode = By.CssSelector("[name=\"StaffCode\"]");
            public static readonly By NINumber = By.CssSelector("[name=\"NINumber\"]");
            public static readonly By TeacherReferenceNumber = By.CssSelector("[name=\"TeacherReferenceNumber\"]");
            public static readonly By PayrollNumber = By.CssSelector("[name=\"Employee.PayrollNumber\"]");
            public static readonly By TeachingStaff = By.CssSelector("[name=\"IsTeachingStaff\"]");

            #region Staff Checks - Grid

            public static readonly By AddBackgroundChecksButton = By.CssSelector("[title=\"Add New Linked Check Details\"]");

            public struct StaffChecks
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"StaffChecks\"]");

                public struct GridColumns
                {
                    public static readonly By GridCheck = By.CssSelector("[name$=\"StaffCheckType.dropdownImitator\"]");
                    public static readonly By GridClearanceLevel = By.CssSelector("[name$=\"StaffCheckClearanceLevel.dropdownImitator\"]");
                    public static readonly By GridClearanceDate = By.CssSelector("[name$=\"ClearanceDate\"]");
                    public static readonly By GridExpiryDate = By.CssSelector("[name$=\"ExpiryDate\"]");
                }

                public static readonly By Check = By.CssSelector("[name=\"StaffCheckType.dropdownImitator\"]");
                public static readonly By RequestedDate = By.CssSelector("[name=\"RequestedDate\"]");
                public static readonly By ClearanceDate = By.CssSelector("[name=\"ClearanceDate\"]");
                public static readonly By ClearanceLevel = By.CssSelector("[name=\"StaffCheckClearanceLevel.dropdownImitator\"]");
                public static readonly By ExpiryDate = By.CssSelector("[name=\"ExpiryDate\"]");
                public static readonly By ReferenceNumber = By.CssSelector("[name=\"ReferenceNumber\"]");
                public static readonly By DocumentNumber = By.CssSelector("[name=\"DocumentNumber\"]");
                public static readonly By AuthenticatedBy = By.CssSelector("[name=\"AuthenticatedBy\"]");
                public static readonly By Notes = By.CssSelector("#dialog-editableData > div > div:nth-child(9) > div > div > div [name='Notes']");
            }

            #endregion

            #region Staff Roles - Grid

            public struct StaffRoleAssignment
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"StaffRoleAssignments\"]");

                public struct GridColumns
                {
                    public static readonly By GridRoles = By.CssSelector("[name$=\"StaffRole.dropdownImitator\"]");
                    public static readonly By GridStartDate = By.CssSelector("[name$=\"StartDate\"]");
                    public static readonly By GridEndDate = By.CssSelector("[name$=\"EndDate\"]");
                }
            }

            #endregion

            #region Bank Details - Grid

            public static readonly By AddBankDetailsButton = By.CssSelector("[title=\"Add New Linked Bank Details\"]");
            public static readonly By AddContractsButton = By.CssSelector("[title=\"Add New Linked Contract Details\"]");

            #endregion

            #region Contracts - Grid
            #endregion

            #region Staff Service Records - Grid

            public static By DeleteStaffSearchResult(string id)
            {
                return By.CssSelector(String.Format(DeleteStaffSearchResultFormat, id));
            }

            public static By SearchResult(string id)
            {
                return By.CssSelector(String.Format(StaffSearchResultFormat, id));
            }

            public static readonly By AddServiceRecordButton = By.CssSelector("[title=\"Add Service Record\"]");

            public struct StaffServiceRecords
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"StaffServiceRecords\"]");

                public struct GridColumns
                {
                    public static readonly By GridDOA = By.CssSelector("[name$=\"DOA\"]");
                    public static readonly By GridDOL = By.CssSelector("[name$=\"DOL\"]");
                    public static readonly By GridContinuousServiceStartDate = By.CssSelector("[name$=\"ContinuousServiceStartDate\"]");
                    public static readonly By GridLocalAuthorityStartDate = By.CssSelector("[name$=\"LocalAuthorityStartDate\"]");
                    public static readonly By GridStaffReasonForLeaving = By.CssSelector("[name$=\"StaffReasonForLeaving.dropdownImitator\"]");
                    public static readonly By GridNotes = By.CssSelector("[name$=\"Notes\"]");
                }

                public static readonly By ServiceDOA = By.CssSelector("[name=\"DOA\"]");
                public static readonly By ServiceDOL = By.CssSelector("[name=\"DOL\"]");
                public static readonly By ContinuousServiceStartDate = By.CssSelector("[name=\"ContinuousServiceStartDate\"]");
                public static readonly By LocalAuthorityStartDate = By.CssSelector("[name=\"LocalAuthorityStartDate\"]");
                public static readonly By StaffReasonForLeaving = By.CssSelector("[name=\"StaffReasonForLeaving.dropdownImitator\"]");
                public static readonly By Destination = By.CssSelector("[name=\"Destination\"]");
                public static readonly By PreviousEmployer = By.CssSelector("[name=\"PreviousEmployer\"]");
                public static readonly By NextEmployer = By.CssSelector("[name=\"NextEmployer\"]");
                public static readonly By Notes = By.CssSelector("#dialog-editableData > div > div:nth-child(6) > div > div > div [name='Notes']");       
            }

            #endregion

            #endregion

            #region Addresses

            public static readonly By AddressesAccordion = By.CssSelector("div:nth-child(3) > div.panel-heading > h4 > a > span");

            #region Addresses - Grid
            #endregion

            #endregion

            #region Staff Phone/Email

            public static readonly By StaffPhoneEmailAccordion = By.CssSelector("div:nth-child(4) > div.panel-heading > h4 > a > span");

            #region Staff Telephone Numbers - Grid
            #endregion

            #region Staff Email Addresses - Grid
            #endregion

            #region Vehicle Details - Grid
            #endregion

            #endregion

            #region Next of Kin

            public static readonly By NextOfKinAccordion = By.CssSelector("div:nth-child(5) > div.panel-heading > h4 > a > span");

            #region Staff Contacts - Grid
            #endregion

            #endregion

            #region Absences

            public static readonly By AbsencesAccordion = By.CssSelector("div:nth-child(6) > div.panel-heading > h4 > a > span");
            public static readonly By Absences = By.CssSelector("#editableData > div > div:nth-child(6) > div.panel-heading > h4 > a > span");
            public static readonly By AddStaffAbsenceButton = By.CssSelector("[title=\"Add New Linked Staff Absence\"]");

            #region Absence History - Grid

            public struct StaffAbsences
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"StaffAbsences\"]");

                public struct GridColumns
                {
                    public static readonly By EndDate = By.CssSelector("[name$=\"EndDate\"]");
                }

                public struct StaffAbsencePopup
                {
                    public static readonly By FirstDay = By.CssSelector("#dialog-palette-editor [name='StartDate']");
                    public static readonly By LastDay = By.CssSelector("#dialog-palette-editor [name='EndDate']");
                    public static readonly By ExpectedReturn = By.CssSelector("[name=\"ExpectedReturnDate\"]");
                    public static readonly By WorkingDaysLost = By.CssSelector("[name=\"WorkingDaysLost\"]");
                    public static readonly By WorkingHoursLost = By.CssSelector("[name=\"WorkingHoursLost\"]");

                    public static readonly By ActualReturn = By.CssSelector("[name=\"ActualReturnDate\"]");
                    public static readonly By Notes = By.CssSelector("#dialog-palette-editor [name='Notes']");

                    public static readonly By AbsenceTypeLookup = By.CssSelector("[name=\"AbsenceType.dropdownImitator\"]");
                    public static readonly By IllnessCategoryLookup = By.CssSelector("[name=\"IllnessCategory.dropdownImitator\"]");
                    public static readonly By AbsencePayRateLookup = By.CssSelector("[name=\"AbsencePayRate.dropdownImitator\"]");
                    public static readonly By PayrollAbsenceCategoryLookup = By.CssSelector("[name=\"PayrollAbsenceCategory.dropdownImitator\"]");
                    public static readonly By AddAbsenceCertificateButton = By.CssSelector("[title=\"Add Absence Certificate\"]");
                    public static readonly By AnnualLeaveCheckBox = By.CssSelector("[name=\"tri_chkbox_AnnualLeave\"]");

                    public static readonly By AbsenceEditButton =
                        By.CssSelector("#dialog-editableData > div > div:nth-child(8) > div > div > div > div.table-scroll.grid-height-sm > div > table > tbody > tr > td:nth-child(2) > button");

                    public struct StaffAbsenceCertificatePopup
                    {
                        public static readonly By SignatoryTypeLookup = By.CssSelector("[name=\"SignatoryType.dropdownImitator\"]");
                        public static readonly By CertificateAdviceLookup = By.CssSelector("[name=\"CertificateAdvice.dropdownImitator\"]");
                        public static readonly By ReturnToWorkConditions = By.CssSelector("[name=\"AbsenceCertificateReturnToWorkConditions.SelectedIds\"]");
                        public static readonly By SelectedReturnToWorkCondition = By.CssSelector("[name=\"AbsenceCertificateReturnToWorkConditions.SelectedIds\"][checked]");
                        public static readonly By DateReceived = By.CssSelector("[name=\"DateReceived\"]");
                        public static readonly By DateSigned = By.CssSelector("[name=\"DateSigned\"]");
                        public static readonly By SignedBy = By.CssSelector("[name=\"SignedBy\"]");

                        public static readonly By CertificateStartDate = By.CssSelector("#dialog-dialog-editableData input[name='StartDate']");
                        public static readonly By CertificateEndDate = By.CssSelector("#dialog-dialog-editableData input[name='EndDate']");
                        public static readonly By CertificateDuration = By.CssSelector("[name=\"Duration\"]");

                        //Check Boxes
                        public static readonly By APhasedReturnToWork = By.CssSelector("#ui-id-10 > div:nth-child(5) > div > div > div > div > div:nth-child(1) > input:nth-child(1)");
                        public static readonly By AmendedDuties = By.CssSelector("#ui-id-10 > div:nth-child(5) > div > div > div > div > div:nth-child(1) > input:nth-child(4)");
                        public static readonly By AlteredHours = By.CssSelector("#ui-id-10 > div:nth-child(5) > div > div > div > div > div:nth-child(1) > input:nth-child(6)");
                        public static readonly By WorkplaceAdaptions = By.CssSelector("#ui-id-10 > div:nth-child(5) > div > div > div > div > div:nth-child(1) > input:nth-child(8)");

                        public static readonly By StartDate = By.CssSelector("[name=\"StartDate\"]");
                        public static readonly By EndDate = By.CssSelector("[name=\"EndDate\"]");
                    }
                }
            }

            #endregion

            #endregion

            #region Medical

            public static readonly By MedicalAccordion = By.CssSelector("div:nth-child(7) > div.panel-heading > h4 > a > span");

            #region Dietary Needs - Multiple Entity Selector
            #endregion

            #region Impairments - Grid
            #endregion

            #region Medical Notes - Grid
            #endregion

            #endregion

            #region Training/Qualifications

            public static readonly By TrainingQualificationsAccordion = SimsBy.AutomationId("section_menu_Training/Qualifications");

            #region Training History - Grid

            public static readonly By AddTrainingEventButton = SimsBy.AutomationId("add_training_event_button");

            public struct StaffTrainingCourseEnrolmentsGrid
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"StaffTrainingCourseEnrolments\"]");

                public struct GridColumns
                {
                    public static readonly By Title = By.CssSelector("[name$=\"TrainingCourseTitle\"]");
                    public static readonly By CourseDate = By.CssSelector("[name$=\"TrainingCourseOccurrencesStartDate\"]");
                    public static readonly By CourseStatus = By.CssSelector("[name$=\"CourseEnrolmentStatus.dropdownImitator\"]");
                    public static readonly By AdditionalCosts = By.CssSelector("[name$=\"AdditionalCosts\"]");
                    public static readonly By Comment = By.CssSelector("[title=\"Add Note\"]");
                }

                public struct TrainingCourseEventPopup
                {
                }

                public struct TrainingCourseOccurrencePalette
                {
                    public static By SearchResult(string id)
                    {
                        const string searchResultFormat = "div[data-ajax-url*='/Staff/SIMS8StaffTrainingCourseEnrolment/ReadDetail/{0}']";
                        return By.CssSelector(string.Format(searchResultFormat, id));
                    }
                }
            }

            #endregion

            #region Qualifications - Grid
            #endregion

            #endregion

            #region Ethnic/Cultural

            public static readonly By EthnicCulturalAccordion = By.CssSelector("div:nth-child(9) > div.panel-heading > h4 > a > span");

            #region Languages - Grid
            #endregion

            #endregion

            #region Experience

            public static readonly By ExperiencAccordion = By.CssSelector("div:nth-child(10) > div.panel-heading > h4 > a > span");

            #region Eperience - Grid
            #endregion

            #endregion

            #region Documents

            public static readonly By DocumentsAccordion = By.CssSelector("div:nth-child(11) > div.panel-heading > h4 > a > span");

            #region Staff Notes/Documents- Grid
            #endregion

            #endregion
        }

        public struct SuperannuationScheme
        {
            public static readonly By SaveButton = By.CssSelector("#screen > div > div.layout-col.main.pane > div > div > div > div.form-header > div > div > ul > li:nth-child(4) > a");
            public static readonly By ValidationScreen = By.CssSelector("#screen > div > div.layout-col.main.pane > div > div > div > div.form-body > div > div > div > div > div > div.form-header > div.validation-summary-errors");
        }

        #endregion

        #region Staff Leaving Details

        public struct StaffLeavingDetails
        {
            public static readonly By SaveButton = By.CssSelector("#screen > div > div:nth-child(1) > div > div > div.form-header > div > div > ul > li:nth-child(3) > a");
            public static readonly By DOL = By.CssSelector("[name=\"DOL\"]");
            public static readonly By ReasonForLeaving = By.CssSelector("[name=\"StaffReasonForLeaving.dropdownImitator\"]");
            public static readonly By DestinationDetails = By.CssSelector("[name=\"Destination\"]");
        }

        #endregion

        #region Sevice Terms

        public struct ServiceTerm
        {
            public static readonly By CreateButton = By.CssSelector("a[data-ajax-url$=\"/Staff/SIMS8ServiceTermsMaintenanceTripleServiceTerm/CreateDetail\"]");

            public static readonly By SaveButton = By.CssSelector("a[title=\"Save Record\"]");

            public static readonly By DeleteButton =
                By.CssSelector(
                    "#screen > div > div.layout-col.main.pane > div > div > div > div.form-header > div > div > ul > div > div > ul > li:nth-child(8) > a");

            public static readonly By ResourceProvider = By.CssSelector("[name=\"ResourceProvider\"]");
            public static readonly By Code = By.CssSelector("[name=\"Code\"]");
            public static readonly By Description = By.CssSelector("[name=\"Description\"]");

            public static readonly By IncrementMonthSelector =
                By.CssSelector("[name=\"IncrementMonthSelector.dropdownImitator\"]");

            public static readonly By SpinalProgression = By.CssSelector("[name=\"tri_chkbox_SpinalProgression\"]");
            public static readonly By HoursWorkedPerWeek = By.CssSelector("[name=\"HoursWorkedPerWeek\"]");
            public static readonly By WeeksWorkedPerYear = By.CssSelector("[name=\"WeeksWorkedPerYear\"]");

            public static readonly By MonthlyReconciliation =
                By.CssSelector("[name=\"tri_chkbox_MonthlyReconciliation\"]");

            public static readonly By Salaried = By.CssSelector("[name=\"tri_chkbox_Salaried\"]");

            public static readonly By April = By.CssSelector("[name=\"PayPatternApr\"]");
            public static readonly By May = By.CssSelector("[name=\"PayPatternMay\"]");
            public static readonly By June = By.CssSelector("[name=\"PayPatternJun\"]");
            public static readonly By July = By.CssSelector("[name=\"PayPatternJul\"]");
            public static readonly By August = By.CssSelector("[name=\"PayPatternAug\"]");
            public static readonly By September = By.CssSelector("[name=\"PayPatternSep\"]");
            public static readonly By October = By.CssSelector("[name=\"PayPatternOct\"]");
            public static readonly By November = By.CssSelector("[name=\"PayPatternNov\"]");
            public static readonly By December = By.CssSelector("[name=\"PayPatternDec\"]");
            public static readonly By January = By.CssSelector("[name=\"PayPatternJan\"]");
            public static readonly By February = By.CssSelector("[name=\"PayPatternFeb\"]");
            public static readonly By March = By.CssSelector("[name=\"PayPatternMar\"]");
            public static readonly By TotalWeeks = By.CssSelector("[name=\"TotalPayPattern\"]");

            public static readonly By TermTimeOnly = By.CssSelector("[name=\"tri_chkbox_TermTimeOnly\"]");
            public static readonly By IsVisible = By.CssSelector("[name=\"tri_chkbox_IsVisible\"]");

            public static readonly By AddPayScaleButton = By.CssSelector("[title=\"Add Pay Scale\"]");
            public static readonly By AddAllowanceButton = By.CssSelector("[title=\"Add New Linked Allowances\"]");
            public static readonly By AddPostTypeButton = By.CssSelector("[title=\"Add New Linked Post Types\"]");

            public static readonly By AddSuperannuationSchemeButton =
                By.CssSelector("button[title=\"Add New Linked Superannuation Schemes\"]");

            public static readonly By AddFinancialSubGroupButton = By.CssSelector("[title=\"Add new row\"]");

            public static readonly By ServiceTermAccordion =
                By.CssSelector("div:nth-child(1) > div.panel-heading > h4 > a > span");

            public static readonly By PayAwardsAccordion =
                By.CssSelector("div:nth-child(2) > div.panel-heading > h4 > a > span");

            public static readonly By AllowancesAccordion =
                By.CssSelector("div:nth-child(3) > div.panel-heading > h4 > a > span");

            public static readonly By PostTypesAccordion =
                By.CssSelector("div:nth-child(4) > div.panel-heading > h4 > a > span");

            public static readonly By SuperannuationSchemesAccordion =
                By.CssSelector("div:nth-child(5) > div.panel-heading > h4 > a > span");

            public static readonly By FinancialSubGroupsAccordion =
                By.CssSelector("div:nth-child(6) > div.panel-heading > h4 > a > span");

            public struct ServiceTermPayAwardGrid
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"PayScales\"]");

                public struct GridColumns
                {
                    public static readonly By PAIsVisible = By.CssSelector("[name$=\"IsVisible\"]");
                    public static readonly By PACode = By.CssSelector("[name$=\"Code\"]");
                    public static readonly By PADescription = By.CssSelector("[name$=\"Description\"]");

                    public static readonly By PAStatutoryPayScale = By.CssSelector("[name$=\"StatutoryPayScale.dropdownImitator\"]");

                    public static readonly By PAMinimumPoint = By.CssSelector("[name$=\"MinimumPoint\"]");
                    public static readonly By PAMaximumPoint = By.CssSelector("[name$=\"MaximumPoint\"]");
                    public static readonly By PAPaySpineCode = By.CssSelector("[name$=\"PaySpineCode\"]");
                }

                public struct PayScalePopup
                {
                    public static readonly By PSIsVisible = By.CssSelector("[name=\"IsVisible\"]");
                    public static readonly By PSCode = By.CssSelector("[name=\"Code\"]");
                    public static readonly By PSDescription = By.CssSelector("[name=\"Description\"]");

                    public static readonly By PSStatutoryPayScale =
                        By.CssSelector("[name=\"StatutoryPayScale.dropdownImitator\"]");

                    public static readonly By PSMinimumPoint = By.CssSelector("[name=\"MinimumPoint\"]");
                    public static readonly By PSMaximumPoint = By.CssSelector("[name=\"MaximumPoint\"]");
                    public static readonly By PSPaySpineCode = By.CssSelector("[name=\"PaySpineCode\"]");

                    public static readonly By AddPaySpineButton =
                        By.CssSelector(
                            "button[data-ajax-url*='/Staff/SIMS8PaySpinesSearchPalettePaySpine/AddpaySpineDialog'");

                    public struct PaySpinePalette
                    {
                        public static readonly By PSPCreateButton =
                            By.CssSelector(
                                "a[data-ajax-url*='/Staff/SIMS8PaySpinesSearchPalettePaySpine/CreateDetail'");

                        public static readonly By CriteriaCode = By.CssSelector("[name=\"Code\"]");

                        public static readonly By PSPCode = By.CssSelector("[name=\"Code\"]");
                        public static readonly By PSPDescription = By.CssSelector("[name=\"Description\"]");
                        public static readonly By PSPMinimumPoint = By.CssSelector("[name=\"MinimumPoint\"]");
                        public static readonly By PSPMaximumPoint = By.CssSelector("[name=\"MaximumPoint\"]");
                        public static readonly By PSPInterval = By.CssSelector("[name=\"Interval\"]");
                        public static readonly By PSPAwardDate = By.CssSelector("[name=\"AwardDate\"]");

                        public static readonly By AddScaleAwardButton = By.CssSelector("button[name=\"GeneratePayAwards\"]");

                        public struct ScaleAwardGrid
                        {
                            public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"PayAwards\"]");
                            public static readonly By SAAwardDate = By.CssSelector("[name$=\"Date\"]");
                            public static readonly By SAScalePoint = By.CssSelector("[name$=\"ScalePoint\"]");
                            public static readonly By SAScaleAmount = By.CssSelector("[name$=\"ScaleAmount\"]");
                        }

                        public static By SearchResult(string id)
                        {
                            const string searchResultFormat =
                                "div[data-ajax-url*='/Staff/SIMS8PaySpinesSearchPalettePaySpine/ReadDetail/{0}']";
                            return By.CssSelector(string.Format(searchResultFormat, id));
                        }
                    }
                }
            }

            public struct ServiceTermAllowanceGrid
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"ServiceTermAllowances\"]");

                public struct GridColumns
                {
                    public static readonly By AGCode = By.CssSelector("[name$=\"Code\"]");
                    public static readonly By AGDescription = By.CssSelector("[name$=\"Description\"]");
                }

                public struct AllowancePalette
                {
                    public static readonly By APCreateButton =
                        By.CssSelector(
                            "a[data-ajax-url*='/Staff/SIMS8AllowancePaletteEditorAllowance/CreateDetail']");

                    public static readonly By CriteriaCode = By.CssSelector("[name=\"Code\"]");
                    public static readonly By CriteriaDescription = By.CssSelector("[name=\"Description\"]");

                    public static readonly By APCode = By.CssSelector("[name=\"Code\"]");
                    public static readonly By APDescription = By.CssSelector("[name=\"Description\"]");
                    public static readonly By APDisplayOrder = By.CssSelector("[name=\"DisplayOrder\"]");
                    public static readonly By APIsVisible = By.CssSelector("[name=\"IsVisible\"]");

                    public static readonly By AdditionalPaymentCategory =
                        By.CssSelector("[name=\"AdditionalPaymentCategory.dropdownImitator\"]");

                    public static readonly By AllowanceAwardAttachedFixed = By.CssSelector("[value=\"True\"]");
                    public static readonly By AllowanceAwardAttachedPersonal = By.CssSelector("[value=\"False\"]");
               
                    public struct AllowanceAwardGrid
                    {
                        public static readonly By Grid = By.CssSelector("table[data-maintenance-container=\"AllowanceAwards\"]");

                        public struct GridColumns
                        {                        
                            public static readonly By AwardDate = By.CssSelector("[name$=\"AwardDate\"]");
                            public static readonly By Amount = By.CssSelector("[name$=\"Amount\"]");
                        }
                    }

                    public static By SearchResult(string id)
                    {
                        const string searchResultFormat =
                            "div[data-ajax-url*='/Staff/SIMS8AllowancePaletteEditorAllowance/ReadDetail/{0}']";
                        return By.CssSelector(string.Format(searchResultFormat, id));
                    }
                }

                public struct AllowancePopup
                {
                    public static readonly By AResourceProvider =
                        By.CssSelector("[name=\"Allowance.ResourceProviderName\"]");

                    public static readonly By ACode = By.CssSelector("[name=\"Allowance.Code\"]");
                    public static readonly By ADescription = By.CssSelector("[name=\"Allowance.Description\"]");
                    public static readonly By ADisplayOrder = By.CssSelector("[name=\"Allowance.DisplayOrder\"]");
                    public static readonly By AIsVisible = By.CssSelector("[name=\"Allowance.IsVisible\"]");

                    public static readonly By AdditionalPaymentCategory =
                        By.CssSelector("[name=\"Allowance.AdditionalPaymentCategory.dropdownImitator\"]");

                    public static readonly By AllowanceAwardAttachedFixed = By.CssSelector("[value=\"True\"]");
                    public static readonly By AllowanceAwardAttachedPersonal = By.CssSelector("[value=\"False\"]");

                    public static readonly By AllowanceAwardGrid = By.CssSelector("[name=\"Allowance.AllowanceAwards\"]");                
                }
            }

            public struct ServiceTermPostTypeGrid
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"ServiceTermPostTypes\"]");
                public static readonly By AddServiceTermPostTypeButton = By.CssSelector("[title=\"Add New Linked Post Types\"]");

                public struct GridColumns
                {
                    public static readonly By PTGCode = By.CssSelector("[name$=\"Code\"]");
                    public static readonly By PTGDescription = By.CssSelector("[name$=\"Description\"]");
                }

                public struct PostTypePalette
                {
                    public static readonly By PTPCreateButton =
                        By.CssSelector(
                            "div[data-ajax-url*='/Unknown/SIMS8PostTypePaletteEditorPostType/CreateDetail']");

                    public static readonly By SearchButton = By.CssSelector("button[data-ajax-url*=\"/Unknown/SIMS8PostTypePaletteEditorPostType/Search\"]");
                    public static readonly By CriteriaCode = By.CssSelector("[name=\"Code\"]");
                    public static readonly By CriteriaDescription = By.CssSelector("[name=\"Description\"]");

                    public static readonly By PTPCode = By.CssSelector("[name=\"Code\"]");
                    public static readonly By PTPDescription = By.CssSelector("[name=\"Description\"]");
                    public static readonly By PTPDisplayOrder = By.CssSelector("[name=\"DisplayOrder\"]");
                    public static readonly By PTPIsVisible = By.CssSelector("[name=\"IsVisible\"]");
                    public static readonly By PTPParent = By.CssSelector("[name=\"ParentDescription.dropdownImitator\"]");

                    public static By SearchResult(string id)
                    {
                        const string searchResultFormat =
                            "div[data-ajax-url*='/Unknown/SIMS8PostTypePaletteEditorPostType/ReadDetail/{0}']";
                        return By.CssSelector(string.Format(searchResultFormat, id));
                    }

                    public static readonly By FirstSearchResult = By.CssSelector(String.Format("div[data-ajax-url^='/{0}/Unknown/SIMS8PostTypePaletteEditorPostType/ReadDetail/']:nth-child(1)", TestDefaults.Default.Path));
                }

                public struct PostTypePopup
                {
                    public static readonly By PTResourceProvider =
                        By.CssSelector("[name=\"PostType.ResourceProviderName\"]");

                    public static readonly By PTCode = By.CssSelector("[name=\"PostType.Code\"]");
                    public static readonly By PTDescription = By.CssSelector("[name=\"PostType.Description\"]");
                    public static readonly By PTDisplayOrder = By.CssSelector("[name=\"PostType.DisplayOrder\"]");
                    public static readonly By PTIsVisible = By.CssSelector("[name=\"PostType.IsVisible\"]");

                    public static readonly By PTParent =
                        By.CssSelector("[name=\"PostType.ParentDescription.dropdownImitator\"]");
                }
            }

            public struct ServiceTermSuperannuationSchemeGrid
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"ServiceTermSuperannuationSchemes\"]");

                public struct GridColumns
                {
                    public static readonly By SSGCode = By.CssSelector("[name$=\"Code\"]");
                    public static readonly By SSGDescription = By.CssSelector("[name$=\"Description\"]");
                }

                public struct SuperannuationSchemePalette
                {
                    public static readonly By SSPCreateButton =
                        By.CssSelector(
                            "a[data-ajax-url*='/Staff/SIMS8SuperannuationPaletteEditorSuperannuationScheme/CreateDetail'");

                    public static readonly By CriteriaDescription = By.CssSelector("[name=\"Description\"]");

                    public static readonly By SSPResourceProvider = By.CssSelector("[name=\"ResourceProviderName\"]");
                    public static readonly By SSPCode = By.CssSelector("[name=\"Code\"]");
                    public static readonly By SSPDescription = By.CssSelector("[name=\"Description\"]");

                    public static readonly By AddSuperannuationSchemeDetailButton =
                        By.CssSelector("[title=\"Add new row\"]");

                    public struct SuperannuationSchemeDetail
                    {
                        public static readonly By Grid =
                            By.CssSelector("[data-maintenance-container=\"SuperannuationSchemeDetails\"]");

                        public struct GridColumns
                        {
                        public static readonly By ApplicationDate = By.CssSelector("[name$=\"ApplicationDate\"]");
                        public static readonly By Value = By.CssSelector("[name$=\"Value\"]");
                    }
                    }

                    public static By SearchResult(string id)
                    {
                        const string searchResultFormat =
                            "div[data-ajax-url*='/Staff/SIMS8SuperannuationPaletteEditorSuperannuationScheme/ReadDetail/{0}']";
                        return By.CssSelector(string.Format(searchResultFormat, id));
                    }
                }

                public struct SuperannuationSchemePopup
                {
                    public static readonly By SSResourceProvider =
                        By.CssSelector("[name=\"SuperannuationScheme.ResourceProviderName\"]");

                    public static readonly By SSCode = By.CssSelector("[name=\"SuperannuationScheme.Code\"]");

                    public static readonly By SSDescription =
                        By.CssSelector("[name=\"SuperannuationScheme.Description\"]");

                    public struct SuperannuationSchemeDetail
                    {
                        public static readonly By Grid =
                            By.CssSelector(
                                "[data-maintenance-container=\"SuperannuationScheme.SuperannuationSchemeDetails\"]");

                        public static readonly By ApplicationDate = By.CssSelector("[name$=\"ApplicationDate\"]");
                        public static readonly By Value = By.CssSelector("[name$=\"Value\"]");
                    }
                }
            }

            public struct FinancialSubGroupGrid
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"ServiceTermFinancialSubGroup\"]");

                public struct GridColumns
                {
                public static readonly By FSGCode = By.CssSelector("[name$=\"Code\"]");
                public static readonly By FSGDescription = By.CssSelector("[name$=\"Description\"]");
            }
        }
        }

        #endregion

        #region Training Course

        public struct TrainingCourse
        {
            public static readonly By CreateButton = SimsBy.AutomationId("staff_training_courses_create_button");
            public static readonly By SaveButton = SimsBy.AutomationId("well_know_action_save");
            public static readonly By DeleteButton = SimsBy.AutomationId("delete_button");
            public static readonly By ResourceProvider = By.CssSelector("[name=\"ResourceProviderString\"]");
            public static readonly By Title = By.CssSelector("[name=\"Title\"]");
            public static readonly By Description = By.CssSelector("[name=\"Description\"]");
            public static readonly By Level = By.CssSelector("[name=\"CourseLevel.dropdownImitator\"]");
            public static readonly By NumberOfDays = By.CssSelector("[name=\"Duration\"]");
            public static readonly By IsFullTime = By.CssSelector("[name=\"FullTime\"]");
            public static readonly By CourseFees = By.CssSelector("[name=\"CourseFeesString\"]");

            public static readonly By AddTrainingCourseOccurrenceButton = By.CssSelector("[title=\"Add New Linked Training Course Events\"]");

            public static By SearchResult(string id)
            {
                const string searchResultFormat = "div[data-ajax-url*='/Staff/SIMS8TrainingCourseSearchTrainingCourse/ReadDetail/{0}']";
                return By.CssSelector(string.Format(searchResultFormat, id));
            }

            public struct TrainingCourseOccurrenceGrid
            {
                public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"TrainingCourseOccurrences\"]");

                public struct GridColumns
                {
                    public static readonly By CourseDate = By.CssSelector("[name$=\"TrainingCourseOccurrencesStartDate\"]");
                    public static readonly By Venue = By.CssSelector("[name$=\"VenueDisplay\"]");
                    public static readonly By CourseProvider = By.CssSelector("[name$=\"ProviderDisplay\"]");
                }
            }

            public struct TrainingCourseOccurrencePopup
            {
                public static readonly By AddAttendeesButton = By.CssSelector("[title=\"Add Attendee To Training Course Occurence\"]");

                public static readonly By StartDate = By.CssSelector("[name=\"StartDate\"]");
                public static readonly By EndDate = By.CssSelector("[name=\"EndDate\"]");
                public static readonly By Renewal = By.CssSelector("[name=\"RenewalDate\"]");
                public static readonly By Venue = By.CssSelector("[name=\"Venue\"]");
                public static readonly By CourseProvider = By.CssSelector("[name=\"Provider\"]");
                public static readonly By Comment = By.CssSelector("[name=\"Comment\"]");

                public struct StaffTrainingCourseEnrolmentGrid
                {
                    public static readonly By Grid = By.CssSelector("[data-maintenance-container=\"StaffTrainingCourseEnrolments\"]");

                    public struct GridColumns
                    {
                        public static readonly By AttendeeName = By.CssSelector("[name$=\"StaffName\"]");
                        public static readonly By CourseStatus = By.CssSelector("[name$=\"CourseEnrolmentStatus.dropdownImitator\"]");
                        public static readonly By AdditionalCosts = By.CssSelector("[name$=\"AdditionalCosts\"]");
                        public static readonly By AttComment = By.CssSelector("[name$=\"Comment\"]");
                    }
                }

                public struct StaffPalette
                {
                    public static readonly By AttendeeName = By.CssSelector("[name=\"LegalSurname\"]");

                    public static readonly By ShowMore = By.CssSelector("[data-flag-id=\"#ShowAdvanced\"]");

                    public static readonly By StaffCode = By.CssSelector("[name=\"StaffCode\"]");
                    public static readonly By Gender = By.CssSelector("[name=\"Gender.dropdownImitator\"]");
                    public static readonly By CurrentCriteria = By.CssSelector("[name=\"tri_chkbox_StatusCurrentCriterion\"]");
                    public static readonly By FutureCriteria = By.CssSelector("[name=\"tri_chkbox_StatusFutureCriterion\"]");
                    public static readonly By StaffRoleCategory = By.CssSelector("[name=\"StaffRoleCategory.dropdownImitator\"]");
                    public static readonly By StaffRole = By.CssSelector("[name=\"StaffRole.dropdownImitator\"]");

                    public static readonly By SearchButton = By.CssSelector("button[type='submit']");

                    public static readonly By FirstSearchResult = By.CssSelector("#dialog-dialog-editableData > div > div.list-group.search-result-tiles > div:nth-child(1)");
                }
            }
        }

        #endregion
    }
}
