using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class SuperannuationSchemesDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_term_superannuation_scheme_dialog"); }
        }

        #region Page properties
        [FindsBy(How = How.Name, Using = "SuperannuationScheme.Code")]
        private IWebElement _codeTextBox;

        [FindsBy(How = How.Name, Using = "SuperannuationScheme.Description")]
        private IWebElement _descriptionTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_scheme_values_button']")]
        private IWebElement _addSchemeValueButton;

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
                return new GridComponent<SchemeValueGridRow>(By.CssSelector("[data-maintenance-container='SuperannuationScheme.SuperannuationSchemeDetails']"), ComponentIdentifier);
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
                get { return _applicationDate.GetValue(); }
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
    }
}
