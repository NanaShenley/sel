using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;

namespace Staff.Components.StaffRegression
{
    public class StaffRecordTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_record_triplet"); }
        }

        public class StaffRecordSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly StaffSearch _searchCriteria;
        public StaffSearch SearchCriteria { get { return _searchCriteria; } }

        public StaffRecord Create()
        {
            SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("staff_records_create_button"), By.CssSelector(".has-datamaintenance"));
            return new StaffRecord();
        }

        public AddNewStaffDialog CreateStaff()
        {
            Action addNewStaff = () => SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("add_new_staff_button"), SimsBy.AutomationId("add_new_staff_wizard"));
            Retry.Do(addNewStaff);
            return new AddNewStaffDialog();
        }

        public StaffRecordTriplet()
        {
            _searchCriteria = new StaffSearch(this);
        }
    }

    public class ConfirmDeleteDialog : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("confirm_delete_dialog"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _deleteButton;

        public void Delete()
        {
            Retry.Do(_deleteButton.Click);
        }
    }

    public class AddNewStaffDialog : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("add_new_staff_wizard"); }
        }

        #region Personal Details

        [FindsBy(How = How.Name, Using = "LegalForename")]
        private IWebElement _legalForeName;

        [FindsBy(How = How.Name, Using = "LegalMiddleNames")]
        private IWebElement _legalMiddleNames;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _legalSurname;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _gender;

        [FindsBy(How = How.Name, Using = "DateOfBirth")]
        private IWebElement _dateOfBirth;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_button']")]
        private IWebElement _continueButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        public string LegalForename
        {
            set { _legalForeName.SetText(value); }
            get { return _legalForeName.GetValue(); }
        }
        public string LegalMiddleNames
        {
            set { _legalMiddleNames.SetText(value); }
            get { return _legalMiddleNames.GetValue(); }
        }
        public string LegalSurname
        {
            set { _legalSurname.SetText(value); }
            get { return _legalSurname.GetValue(); }
        }
        public string Gender
        {
            set { Retry.Do(() => _gender.ChooseSelectorOption(value), catchAction: Refresh); }
            get { return _gender.GetValue(); }
        }
        public string DateOfBirth
        {
            set { _dateOfBirth.SetText(value); }
            get { return _dateOfBirth.GetValue(); }
        }

        public void Continue()
        {
            Retry.Do(_continueButton.Click);
        }

        public void Cancel()
        {
            Retry.Do(_cancelButton.Click);
        }

        #endregion

        #region Service Details

        [FindsBy(How = How.Name, Using = "DateOfArrival")]
        private IWebElement _dateOfArrival;

        [FindsBy(How = How.Name, Using = "ContinuousServiceStartDate")]
        private IWebElement _continuousServiceStartDate;

        [FindsBy(How = How.Name, Using = "LocalAuthorityStartDate")]
        private IWebElement _localAuthorityStartDate;

        [FindsBy(How = How.Name, Using = "PreviousEmployer")]
        private IWebElement _previousEmployer;

        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _notes;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_record_button']")]
        private IWebElement _createRecordButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_record_button']")]
        private IWebElement _saveRecordButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='back_button']")]
        private IWebElement _backButton;

        public string DateOfArrival
        {
            set { _dateOfArrival.SetText(value); }
            get { return _dateOfArrival.GetValue(); }
        }

        public string ContinuousServiceStartDate
        {
            set { _continuousServiceStartDate.SetText(value); }
            get { return _continuousServiceStartDate.GetValue(); }
        }

        public string LocalAuthorityStartDate
        {
            set { _localAuthorityStartDate.SetText(value); }
            get { return _localAuthorityStartDate.GetValue(); }
        }

        public string PreviousEmployer
        {
            set { _previousEmployer.SetText(value); }
            get { return _previousEmployer.GetValue(); }
        }

        public string Notes
        {
            set { _notes.SetText(value); }
            get { return _notes.GetValue(); }
        }

        public void CreateRecord()
        {
            Retry.Do(_createRecordButton.Click);
        }

        public StaffRecord SaveRecord()
        {
            Action saveStaff = () => SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("save_record_button"), By.CssSelector(".has-datamaintenance"));
            Retry.Do(saveStaff);
            return new StaffRecord();
        }

        public void Back()
        {
            Retry.Do(_backButton.Click);
        }

        public class StaffMatch
        {
            [FindsBy(How = How.CssSelector, Using = "[name$=PreferredListName]")]
            private IWebElement _staffName;

            [FindsBy(How = How.CssSelector, Using = "[name$=Gender]")]
            private IWebElement _gender;

            [FindsBy(How = How.CssSelector, Using = "[name$=DateOfBirth]")]
            private IWebElement _dateOfBirth;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id$=StaffAddress]")]
            private IWebElement _address;

            public string StaffName
            {
                set { _staffName.SetText(value); }
                get { return _staffName.GetValue(); }
            }
            public string Gender
            {
                set { _gender.SetText(value); }
                get { return _gender.GetValue(); }
            }
            public string DateOfBirth
            {
                set { _dateOfBirth.SetText(value); }
                get { return _dateOfBirth.GetValue(); }
            }
            public string OpenAddress()
            {
                Retry.Do(_address.Click);

                StaffAddressPopover addressPopover = new StaffAddressPopover();

                return addressPopover.Text;
            }
        }

        public class StaffAddressPopover : BaseComponent
        {
            public override By ComponentIdentifier
            {
                get { return SimsBy.AutomationId("StaffAddressPopover"); }
            }

            public string Text
            {
                get
                {
                    return Component.Text;
                }
            }
        }

        public GridComponent<StaffMatch> CurrentStaffMatchesGrid
        {
            get
            {
                GridComponent<StaffMatch> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffMatch>(By.CssSelector("[data-maintenance-grid-id='staff_matches_current']"));
                });
                return returnValue;
            }
        }

        public GridComponent<StaffMatch> FutureStaffMatchesGrid
        {
            get
            {
                GridComponent<StaffMatch> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffMatch>(By.CssSelector("[data-maintenance-grid-id='staff_matches_future']"));
                });
                return returnValue;
            }
        }

        public GridComponent<StaffMatch> LeaverStaffMatchesGrid
        {
            get
            {
                GridComponent<StaffMatch> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffMatch>(By.CssSelector("[data-maintenance-grid-id='staff_matches_leaver']"));
                });
                return returnValue;
            }
        }

        #endregion    
    }

    public class StaffRecord : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_record_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Title.dropdownImitator")]
        private IWebElement _title;

        [FindsBy(How = How.Name, Using = "LegalForename")]
        private IWebElement _legalForeName;

        [FindsBy(How = How.Name, Using = "LegalMiddleNames")]
        private IWebElement _legalMiddleNames;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _legalSurname;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _gender;

        [FindsBy(How = How.Name, Using = "DateOfBirth")]
        private IWebElement _dateOfBirth;

        [FindsBy(How = How.Name, Using = "QuickNote")]
        private IWebElement _QuickNote;

        [FindsBy(How = How.Name, Using = "DateOfArrival")]
        public IWebElement DOA;

        [FindsBy(How = How.Name, Using = "DateOfLeaving")]
        public IWebElement DOL;

        public string Title
        {
            set { _title.ChooseSelectorOption(value); }
            get { return _title.GetValue(); }
        }
        public string LegalForename
        {
            set { _legalForeName.SetText(value); }
            get { return _legalForeName.GetValue(); }
        }

        public string QuickNote
        {
            set { _QuickNote.SetText(value); }
            get { return _QuickNote.GetValue(); }
        }
        public string LegalMiddleNames
        {
            set { _legalMiddleNames.SetText(value); }
            get { return _legalMiddleNames.GetValue(); }
        }
        public string LegalSurname
        {
            set { _legalSurname.SetText(value); }
            get { return _legalSurname.GetValue(); }
        }
        public string Gender
        {
            set { Retry.Do(() => _gender.ChooseSelectorOption(value), catchAction: Refresh); }
            get { return _gender.GetValue(); }
        }
        public string DateOfBirth
        {
            set { _dateOfBirth.SetText(value); }
            get { return _dateOfBirth.GetValue(); }
        }

        public ServiceRecordGrid ServiceRecordsGrid
        {
            get { return new ServiceRecordGrid(this.Component.DeStaler(ComponentIdentifier).FindElement(By.CssSelector("[data-maintenance-container='StaffServiceRecords']"))); }
        }

        public class ServiceRecordGrid
        {
            private IWebElement _element;
            private List<ServiceRecordGridRow> _rows = new List<ServiceRecordGridRow>();

            public List<ServiceRecordGridRow> Rows
            {
                get { return _rows; }
            }

            public ServiceRecordGrid(IWebElement element)
            {
                _element = element;
                _rows = _element.FindElements(By.CssSelector("tbody tr[data-role=\"gridRow\"]")).Select(e => new ServiceRecordGridRow(e)).ToList();
            }
        }

        public class ServiceRecordGridRow
        {
            private IWebElement _doa;

            private IWebElement _dol;

            private IWebElement _localAuthorityStartDateBy;

            private IWebElement _continuousServiceStartDateBy;

            private IWebElement _element;

            private string _gridRowID;

            public ServiceRecordGridRow(IWebElement element)
            {
                _element = element;
                _doa = _element.FindElement(By.CssSelector("input[type='text'][name$='DOA']"));
                _dol = _element.FindElement(By.CssSelector("input[type='text'][name$='DOL']"));
                _localAuthorityStartDateBy = _element.FindElement(By.CssSelector("input[type='text'][name$='LocalAuthorityStartDate']"));
                _continuousServiceStartDateBy = _element.FindElement(By.CssSelector("input[type='text'][name$='ContinuousServiceStartDate']"));
                _gridRowID = _element.GetAttribute("data-row-id");
            }

            public string DOA
            {
                set { _doa.SetText(value); }
                get { return _doa.GetValue(); }
            }
         
            public string DOL
            {
                get { return _dol.GetValue(); }
            }

            public string LocalAuthorityStartDate
            {
                set { _localAuthorityStartDateBy.SetText(value); }
                get { return _localAuthorityStartDateBy.GetValue(); }
            }

            public string ContinuousServiceStartDate
            {
                set { _continuousServiceStartDateBy.SetText(value); }
                get { return _continuousServiceStartDateBy.GetValue(); }
            }

            public string GridRowID
            {
                get { return _gridRowID; }
            }
        }

        #endregion

        #region Accordions
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='staff_record_service_details_accordion']")]
        private IWebElement _serviceHistoryAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='staff_record_absences_accordion']")]
        private IWebElement _staffAbsencesAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='staff_record_training_qualifications_accordion']")]
        private IWebElement _trainingAndQualificationsAccordion;

        public void GoToServiceDetailsAccordion()
        {
            this.ExpandAccordion(x => x._serviceHistoryAccordion);
        }

        public void GoToStaffAbsencesAccordion()
        {
            this.ExpandAccordion(x => x._staffAbsencesAccordion);
        }

        public void GoToTrainingAndQualificationsAccordion()
        {
            this.ExpandAccordion(x => x._trainingAndQualificationsAccordion);
        }
        #endregion

        #region Grids
        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='StaffRoleAssignments']")]
        private IWebElement _staffRoleGrid;

        

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='StaffServiceRecords']")]
        private IWebElement _serviceRecordGrid;
        #endregion

        #region Grid add actions
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_service_record_button']")]
        private IWebElement _addServiceRecord;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_staff_absence_button']")]
        private IWebElement _addStaffAbsence;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_training_event_button']")]
        private IWebElement _addTrainingCourse;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_contract_details_button']")]
        private IWebElement _addEmploymentContract;


        public ServiceHistoryDialog AddServiceRecord()
        {
            Retry.Do(_addServiceRecord.Click);
            return new ServiceHistoryDialog();
        }

        public StaffAbsenceDialog AddStaffAbsence()
        {
            Retry.Do(_addStaffAbsence.Click);
            return new StaffAbsenceDialog();
        }

        public TrainingCourseDialog AddTrainingCourse()
        {
            Retry.Do(_addTrainingCourse.Click);
            return new TrainingCourseDialog();
        }

        public EmploymentContractDialog AddEmploymentContracts()
        {
            Retry.Do(_addEmploymentContract.Click);
            return new EmploymentContractDialog();
        }

        public void AddStaffRole(string staffRole, string startDate, string endDate)
        {
            //UHOH! Auto blank rows are not playing nice.
            var blankRow = _staffRoleGrid.FindElements(By.CssSelector("tbody tr")).First();

            blankRow.ChooseSelectorOption(By.CssSelector("[name$='StaffRole.dropdownImitator']"), staffRole);

            //Retry.Do(() =>
            //{
            //    blankRow = _staffRoleGrid.FindElements(By.CssSelector("tbody tr")).First();
            //    var startDateElement = blankRow.FindElement(By.CssSelector("[name$=StartDate]"));

            //    startDateElement.SetText(startDate, false);
            //});

            //Retry.Do(() =>
            //{
            //    blankRow = _staffRoleGrid.FindElements(By.CssSelector("tbody tr")).First();
            //    var endDateElement = blankRow.FindElement(By.CssSelector("[name$=EndDate]"));

            //    endDateElement.SetText(endDate, false);
            //});
        }
        #endregion
        
        public StaffRecord Save()
        {
            Action save = () => SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("well_know_action_save"), By.CssSelector("[data-automation-id='staff_record_detail'] div.alert"));
            Retry.Do(save);
            return new StaffRecord();
        } 
        
        public StaffRecord Delete()
        {
            //TODO - Platform change - change Delete Button Automation ID in D:\Dev\Platform\Libraries\MVCCoreLibraries\iSIMSMVCClient.Core\Helpers\Actions\CRUDActionsHtmlHelperExtensions.cs LN178
            Action delete = () => SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("E0605157-0334-4C57-8325-656738B19949"), SimsBy.AutomationId("continue_with_delete_button"));
            Retry.Do(delete);
            var confirmationDialog = new ConfirmDeleteDialog();
            confirmationDialog.Delete();
            return new StaffRecord();
        }

        public AddNewStaffDialog ReadmitStaff()
        {
            Action readmitStaff = () => SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("re-admit_staff_button"), SimsBy.AutomationId("add_new_staff_wizard"));
            Retry.Do(readmitStaff);
            return new AddNewStaffDialog();
        }
    }
}
