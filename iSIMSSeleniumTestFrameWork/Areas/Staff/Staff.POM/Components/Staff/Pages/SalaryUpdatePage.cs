using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        #endregion

        #region SalaryUpdate Grid

        public GridComponent<SalaryUpdate> SalaryUpdates
        {
            get
            {
                return new GridComponent<SalaryUpdate>(By.CssSelector("[data-maintenance-grid-id='MyGrid1']"), ComponentIdentifier);
            }
        }

        public class SalaryUpdate : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$=ColumnSelector][type=checkbox]")]
            private IWebElement _selectCheckBox;
            
            [FindsBy(How = How.CssSelector, Using = "td:nth-child(4) div input")]
            private IWebElement _nameTextBox;

            public bool Select
            {
                set { _selectCheckBox.Set(value); }
                get { return _selectCheckBox.IsChecked(); }
            }

            public string Name
            {
                set { _nameTextBox.SetText(value); }
                get { return _nameTextBox.GetValue(); }
            }
        }

        #endregion

        #region Actions

        public bool IsASuccessMessageIsDisplayed()
        {
            return SeleniumHelper.DoesWebElementExist(_successMessage);
        }

        #endregion
    }
}
