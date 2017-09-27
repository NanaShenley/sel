using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class StaffContractAllowanceDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_employment_contract_allowance_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Allowance.dropdownImitator")]
        private IWebElement _allowanceDropdown;

        [FindsBy(How = How.Name, Using = "AllowanceType.dropdownImitator")]
        private IWebElement _typeDropdown;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateTextBox;

        [FindsBy(How = How.Name, Using = "Reason")]
        private IWebElement _reasonTextBox;

        [FindsBy(How = How.Name, Using = "PayFactor")]
        private IWebElement _payFactor;

        [FindsBy(How = How.Name, Using = "Amount")]
        private IWebElement _amount;

        public string Allowance
        {
            get { return _allowanceDropdown.GetValue(); }
            set
            {
                _allowanceDropdown.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public string Type
        {
            get { return _typeDropdown.GetValue(); }
            set { _typeDropdown.EnterForDropDown(value);}
        }

        public string Reason
        {
            get { return _reasonTextBox.GetValue(); }
            set { _reasonTextBox.SetText(value); }
        }

        public string StartDate
        {
            get { return _startDateTextBox.GetValue(); }
            set { _startDateTextBox.SetText(value); }
        }

        public string EndDate
        {
            get { return _endDateTextBox.GetValue(); }
            set { _endDateTextBox.SetText(value); }
        }

        public string PayFactor
        {
            get { return _payFactor.GetValue(); }
            set { _payFactor.SetText(value); }            
        }

        public string Amount
        {
            get { return _amount.GetValue(); }
            set { _amount.SetText(value); }
        }

        #endregion
    }
}
