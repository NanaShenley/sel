using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using Staff.POM.Base;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.POM.Components.Staff.Dialogs
{
    /// <summary>
    /// Service Agreement Dialog View Model.
    /// </summary>
    /// <seealso cref="Staff.POM.Base.BaseDialogComponent" />
    public class ServiceAgreementDialog : BaseDialogComponent
    {
        /// <summary>
        /// Gets the dialog identifier.
        /// </summary>
        /// <value>
        /// The dialog identifier.
        /// </value>
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_agreement_dialog"); }
        }

        #region Page fields
        /// <summary>
        /// The _start date text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextBox;
        /// <summary>
        /// The _end date text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateTextBox;
        /// <summary>
        /// The _offered date text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "OfferedDate")]
        private IWebElement _offeredDateTextBox;
        /// <summary>
        /// The _accepted date text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "AcceptedDate")]
        private IWebElement _acceptedDateTextBox;
        /// <summary>
        /// The __resason drop down list
        /// </summary>
        [FindsBy(How = How.Name, Using = "ServiceAgreementReason.dropdownImitator")]
        private IWebElement __resasonDropDownList;
        /// <summary>
        /// The _service type drop down list
        /// </summary>
        [FindsBy(How = How.Name, Using = "ServiceAgreementType.dropdownImitator")]
        private IWebElement _serviceTypeDropDownList;
        /// <summary>
        /// The _sourced by drop down list
        /// </summary>
        [FindsBy(How = How.Name, Using = "ServiceAgreementSource.dropdownImitator")]
        private IWebElement _sourcedByDropDownList;
        /// <summary>
        /// The _source name text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "SourceName")]
        private IWebElement _sourceNameTextBox;
        /// <summary>
        /// The _fte text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "FTEHoursPerWeek")]
        private IWebElement _fteTextBox;
        /// <summary>
        /// The _agreement hours text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "AgreementHoursPerWeek")]
        private IWebElement _agreementHoursTextBox;
        /// <summary>
        /// The _weeks per year text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "WeeksPerYear")]
        private IWebElement _weeksPerYearTextBox;
        /// <summary>
        /// The _daily rate CheckBox
        /// </summary>
        [FindsBy(How = How.Name, Using = "DailyRate")]
        private IWebElement _dailyRateCheckBox;
        /// <summary>
        /// The _notes text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _notesTextBox;
        #endregion

        #region Page properties

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public string StartDate
        {
            get { return _startDateTextBox.GetValue(); }
            set { _startDateTextBox.SetText(value); }
        }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public string EndDate
        {
            get { return _endDateTextBox.GetValue(); }
            set { _endDateTextBox.SetText(value); }
        }

        /// <summary>
        /// Gets or sets the offered date.
        /// </summary>
        /// <value>
        /// The offered date.
        /// </value>
        public string OfferedDate
        {
            get { return _offeredDateTextBox.GetValue(); }
            set { _offeredDateTextBox.SetText(value); }
        }

        /// <summary>
        /// Gets or sets the accepted date.
        /// </summary>
        /// <value>
        /// The accepted date.
        /// </value>
        public string AcceptedDate
        {
            get { return _acceptedDateTextBox.GetValue(); }
            set { _acceptedDateTextBox.SetText(value); }
        }

        /// <summary>
        /// Gets or sets the resason.
        /// </summary>
        /// <value>
        /// The resason.
        /// </value>
        public string Resason
        {
            get { return __resasonDropDownList.GetValue(); }
            set { __resasonDropDownList.EnterForDropDown(value); }
        }

        /// <summary>
        /// Gets or sets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public string ServiceType
        {
            get { return _serviceTypeDropDownList.GetValue(); }
            set { _serviceTypeDropDownList.EnterForDropDown(value); }
        }

        /// <summary>
        /// Gets or sets the sourced by.
        /// </summary>
        /// <value>
        /// The sourced by.
        /// </value>
        public string SourcedBy
        {
            get { return _sourcedByDropDownList.GetValue(); }
            set { _sourcedByDropDownList.EnterForDropDown(value); }
        }

        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        public string SourceName
        {
            get { return _sourceNameTextBox.GetValue(); }
            set { _sourceNameTextBox.SetText(value); }
        }

        /// <summary>
        /// Gets or sets the add role button.
        /// </summary>
        /// <value>
        /// The add role button.
        /// </value>
        public void AddRoleButtonClick()
        {
            AutomationSugar.WaitFor("add_role_button");
            AutomationSugar.ClickOn("add_role_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        /// <summary>
        /// Gets the roles table.
        /// </summary>
        /// <value>
        /// The roles table.
        /// </value>
        public GridComponent<RolesRow> RolesTable
        {
            get
            {
                return new GridComponent<RolesRow>(By.CssSelector("[data-maintenance-container='ServiceAgreementRoles']"), ComponentIdentifier);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="Staff.POM.Base.GridRow" />
        public class RolesRow : GridRow
        {
            /// <summary>
            /// The _role
            /// </summary>
            [FindsBy(How = How.CssSelector, Using = "[name$='StaffRole.dropdownImitator']")]
            private IWebElement _role;

            /// <summary>
            /// The _start date
            /// </summary>
            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            /// <summary>
            /// The _end date
            /// </summary>
            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            /// <summary>
            /// Gets or sets the role.
            /// </summary>
            /// <value>
            /// The role.
            /// </value>
            public string Role
            {
                get { return _role.GetAttribute("value"); }
                set { _role.EnterForDropDown(value); }
            }

            /// <summary>
            /// Gets or sets the start date.
            /// </summary>
            /// <value>
            /// The start date.
            /// </value>
            public string StartDate
            {
                get { return _startDate.GetValue(); }
                set { _startDate.SetText(value); }
            }
            /// <summary>
            /// Gets or sets the end date.
            /// </summary>
            /// <value>
            /// The end date.
            /// </value>
            public string EndDate
            {
                get { return _endDate.GetValue(); }
                set { _endDate.SetText(value); }
            }
        }

        /// <summary>
        /// Gets or sets the fte.
        /// </summary>
        /// <value>
        /// The fte.
        /// </value>
        public string FTE
        {
            get { return _fteTextBox.GetValue(); }
            set { _fteTextBox.SetText(value); }
        }

        /// <summary>
        /// Gets or sets the agreement hours.
        /// </summary>
        /// <value>
        /// The agreement hours.
        /// </value>
        public string AgreementHours
        {
            get { return _agreementHoursTextBox.GetValue(); }
            set { _agreementHoursTextBox.SetText(value); }
        }

        /// <summary>
        /// Gets or sets the weeks per year.
        /// </summary>
        /// <value>
        /// The weeks per year.
        /// </value>
        public string WeeksPerYear
        {
            get { return _weeksPerYearTextBox.GetValue(); }
            set { _weeksPerYearTextBox.SetText(value); }
        }

        /// <summary>
        /// Gets or sets the is daily rate.
        /// </summary>
        /// <value>
        /// The is daily rate.
        /// </value>
        public bool IsDailyRate
        {
            get { return _dailyRateCheckBox.IsCheckboxChecked(); }
            set { _dailyRateCheckBox.SetCheckBox(value); }
        }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes
        {
            get { return _notesTextBox.GetValue(); }
            set { _notesTextBox.SetText(value); }
        }

        #endregion
    }
}
