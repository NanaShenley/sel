using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
{
    public class AddStaffContractAllowanceDialog : BaseDialogComponent
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

        [FindsBy(How = How.Name, Using = "Reason")]
        private IWebElement _reasonTextBox;

        public string Allowance
        {
            set
            {
                _allowanceDropdown.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public string Type
        {
            set { _typeDropdown.EnterForDropDown(value); }
        }

        public string Reason
        {
            set { _reasonTextBox.SetText(value); }
        }

        public string StartDate
        {
            set
            {
                _startDateTextBox.SetDateTimeByJS(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        #endregion

        #region Page action

        #endregion
    }
}
