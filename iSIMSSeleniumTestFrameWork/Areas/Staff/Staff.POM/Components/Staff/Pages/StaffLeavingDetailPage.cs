using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff 
{
    public  class StaffLeavingDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_leaving_maintenance"); }
        }

        #region Page propertise

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private  IWebElement _saveButton;

        [FindsBy(How = How.Name, Using = "DOL")]
        private  IWebElement _dateOfLeavingTextbox;

        [FindsBy(How = How.Name, Using = "StaffReasonForLeaving.dropdownImitator")]
        private IWebElement _reasonForLeavingDropdown;

        [FindsBy(How = How.Name, Using = "EmploymentContractDestination.dropdownImitator")]
        private IWebElement _employmentContractDestinationDropdown;

        [FindsBy(How = How.Name, Using = "NextEmployer")]
        private  IWebElement _nextEmployeeTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private  IWebElement _successMessage;

        public  string DateOfLeaving
        {
            set
            {
                _dateOfLeavingTextbox.SetDateTime(value);
            }
        }

        public  string NextEmployee
        {
            set { _nextEmployeeTextbox.SetText(value); }
        }

        public string ReasonForLeaving
        {
            get { return _reasonForLeavingDropdown.GetValue(); }
            set { _reasonForLeavingDropdown.EnterForDropDown(value); }
        }

        public string EmploymentContractDestination
        {
            get { return _employmentContractDestinationDropdown.GetValue(); }
            set { _employmentContractDestinationDropdown.EnterForDropDown(value); }
        }

        #endregion

        #region Page actions

        public ConfirmationDialog SaveValue()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ConfirmationDialog();
        }

        #endregion
    }
}
