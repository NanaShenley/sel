
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class SalaryUpdatePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("annual_increment_detail"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-grid-id='MyGrid1']")]
        private IWebElement _salaryUpdateTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        #endregion

        #region SalaryUpdate Grid

        public GridComponent<SalaryUpdate> SalaryUpdates
        {
            get
            {
                GridComponent<SalaryUpdate> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SalaryUpdate>(By.CssSelector("[data-maintenance-grid-id='MyGrid1']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class SalaryUpdate
        {
            [FindsBy(How = How.CssSelector, Using = "td:nth-child(4) span label input")]
            private IWebElement _selectCheckBox;

            [FindsBy(How = How.CssSelector, Using = "td:nth-child(5) div input")]
            private IWebElement _nameTextBox;

            public bool Select
            {
                set { _selectCheckBox.Set(value); }
                get { return _selectCheckBox.IsChecked(); }
            }

            public string Name
            {
                set { _nameTextBox.SetText(value); }
                get { return _nameTextBox.GetAttribute("value"); }
            }
        }

        #endregion

        #region Actions

        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public bool IsASuccessMessageIsDisplayed()
        {
            return SeleniumHelper.DoesWebElementExist(_successMessage);
        }

        #endregion
    }
}
