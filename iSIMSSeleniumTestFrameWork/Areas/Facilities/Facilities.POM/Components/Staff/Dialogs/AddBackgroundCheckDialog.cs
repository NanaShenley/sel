using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Threading;

namespace POM.Components.Staff
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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _OkButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.Name, Using = "ClearanceDate")]
        private IWebElement _clearanceDateTextBox;

        [FindsBy(How = How.Name, Using = "ExpiryDate")]
        private IWebElement _expiryDateTextBox;

        public string ExpiryDate
        {
            get { return _expiryDateTextBox.GetDateTime(); }
            set { _expiryDateTextBox.SetDateTime(value); }
        }

        public string Check
        {
            get { return _checkDropdown.GetText(); }
            set { _checkDropdown.ChooseSelectorOption(value); }
        }

        public string ClearanceLevel
        {
            get { return _clearanceLevelDropdown.GetAttribute("value"); }
            set { _clearanceLevelDropdown.EnterForDropDown(value); }
        }

        public string ClearanceDate
        {
            set
            {
                _clearanceDateTextBox.SetDateTime(value);
            }
        }

        #endregion

        #region Page actions

        public StaffRecordPage AddStaffCheck()
        {
            _OkButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Wait.WaitLoading();
            return new StaffRecordPage();
        }

        #endregion


    }
}
