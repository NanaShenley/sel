using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class SuperannuationSchemesPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("superannuation_schemes_detail"); }
        }

        #region Page properties
        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _codeTextBox;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _descriptionTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_scheme_values_button']")]
        private IWebElement _addSchemeValueButton;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='SuperannuationSchemeDetails']")]
        private IWebElement _schemeValueTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        public string Code
        {
            set { _codeTextBox.SetText(value); }
            get { return _codeTextBox.GetValue(); }
        }

        public string Description
        {
            set { _descriptionTextBox.SetText(value); }
            get { return _descriptionTextBox.GetValue(); }
        }

        #endregion

        #region SchemeValue Grid

        public GridComponent<SchemeValueGridRow> SchemeValues
        {
            get
            {
                return new GridComponent<SchemeValueGridRow>(By.CssSelector("[data-maintenance-container='SuperannuationSchemeDetails']"), ComponentIdentifier);
            }
        }

        public class SchemeValueGridRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='ApplicationDate']")]
            private IWebElement _applicationDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='Value']")]
            private IWebElement _value;

            public string ApplicationDate
            {
                set { _applicationDate.SetText(value); }
                get{ return _applicationDate.GetValue();}
            }

            public string Value
            {
                set { _value.SetText(value); }
                get { return _value.GetValue(); }
            }
        }

        #endregion

        #region Public methods

        public void ClickAddSchemeValues()
        {
            AutomationSugar.WaitFor("add_scheme_values_button");
            AutomationSugar.ClickOn("add_scheme_values_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        #endregion

        #region LOGIGEAR - NOT USED

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Action is used to click 'Add Scheme Value' button
        /// </summary>
        public void AddSchemeValues()
        {
            _addSchemeValueButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".table-toolbar .btn-group button[disabled='disabled']"));
            Refresh();
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Click 'save' button to finish creating a scheme.
        /// </summary>
        public SuperannuationSchemesPage ClickSaveButton()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SuperannuationSchemesPage();
        }

        /// <summary>
        /// Author: Luong.Mai
        /// Description: Check a success message is displayed
        /// </summary>
        public bool IsSuccessMessageIsDisplayed()
        {
            return SeleniumHelper.DoesWebElementExist(_successMessage);
        }

        #endregion
    }
}
