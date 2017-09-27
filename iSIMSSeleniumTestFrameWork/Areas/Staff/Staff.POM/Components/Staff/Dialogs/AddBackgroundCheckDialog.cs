using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;

using System.Threading;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class AddBackgroundCheckDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_check_dialog"); }
        }

        #region Page propertise

        [FindsBy(How = How.Name, Using = "StaffCheckType.dropdownImitator")]
        private IWebElement _checkDropdown;

        [FindsBy(How = How.Name, Using = "StaffCheckClearanceLevel.dropdownImitator")]
        private IWebElement _clearanceLevelDropdown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.Name, Using = "ClearanceDate")]
        private IWebElement _clearanceDateTextBox;

        [FindsBy(How = How.Name, Using = "RequestedDate")]
        private IWebElement _requestedDateTextBox;

        [FindsBy(How = How.Name, Using = "ExpiryDate")]
        private IWebElement _expiryDateTextBox;

        [FindsBy(How = How.Name, Using = "ReferenceNumber")]
        private IWebElement _referenceNumber;

        [FindsBy(How = How.Name, Using = "DocumentNumber")]
        private IWebElement _documentNumber;

        [FindsBy(How = How.Name, Using = "AuthenticatedBy")]
        private IWebElement _authenticatedBy;

        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _notes;

        public string ExpiryDate
        {
            get { return _expiryDateTextBox.GetValue(); }
            set { _expiryDateTextBox.SetText(value); }
        }

        public string Check
        {
            get { return _checkDropdown.GetValue(); }
            set { _checkDropdown.ChooseSelectorOption(value); }
        }

        public string ClearanceLevel
        {
            get { return _clearanceLevelDropdown.GetAttribute("value"); }
            set { _clearanceLevelDropdown.EnterForDropDown(value); }
        }

        public string ClearanceDate
        {
            get { return _clearanceDateTextBox.GetValue(); }
            set { _clearanceDateTextBox.SetText(value); }
        }

        public string RequestedDate
        {
            get { return _requestedDateTextBox.GetValue(); }
            set { _requestedDateTextBox.SetText(value); }
        }

        public string ReferenceNumber
        {
            get { return _referenceNumber.GetValue(); }
            set { _referenceNumber.SetText(value); }
        }

        public string DocumentNumber
        {
            get { return _documentNumber.GetValue(); }
            set { _documentNumber.SetText(value); }
        }

        public string AuthenticatedBy
        {
            get { return _authenticatedBy.GetValue(); }
            set { _authenticatedBy.SetText(value); }
        }

        public string Notes
        {
            get { return _notes.GetValue(); }
            set { _notes.SetText(value); }
        }

        #endregion

        #region Page actions

        public StaffRecordPage AddStaffCheck()
        {
            AutomationSugar.WaitFor("ok_button");
            AutomationSugar.ClickOn("ok_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new StaffRecordPage();
        }

        #endregion


    }
}
