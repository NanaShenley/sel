using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class AddStaffRoleDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get
            {
                return SimsBy.AutomationId("staff_role_dialog");
            }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _codeTextBox;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _descriptionTextBox;

        [FindsBy(How = How.Name, Using = "Parent.dropdownImitator")]
        private IWebElement _categoryDropdown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        public string Code
        {
            set
            {
                Wait.WaitLoading();
                _codeTextBox.SetText(value);
            }
            get { return _codeTextBox.GetValue(); }
        }
        public string Description
        {
            set
            {
                Wait.WaitLoading();
                _descriptionTextBox.SetText(value);
            }
            get { return _descriptionTextBox.GetValue(); }
        }

        public string Category
        {
            set { _categoryDropdown.EnterForDropDown(value); }
            get { return _categoryDropdown.GetText(); }
        }

        #endregion

        #region Page action

        public StaffRolePage SaveValues()
        {
            _okButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Wait.WaitLoading();
            return new StaffRolePage();
        }

        #endregion

    }
}
