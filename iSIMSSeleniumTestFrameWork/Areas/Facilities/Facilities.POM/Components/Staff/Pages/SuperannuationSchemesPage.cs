using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using NUnit.Framework;
using Staff.POM.Helper;
using Staff.POM.Base;

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

        public GridComponent<SchemeValue> SchemeValueTable
        {
            get
            {
                GridComponent<SchemeValue> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SchemeValue>(By.CssSelector("[data-maintenance-container='SuperannuationSchemeDetails']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class SchemeValue : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='ApplicationDate']")]
            private IWebElement _applicationDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='Value']")]
            private IWebElement _value;

            public string ApplicationDate
            {
                set { _applicationDate.SetDateTime(value); }
                get
                {
                    return _applicationDate.GetDateTime();
                }
            }

            public string Value
            {
                set { _value.SetText(value); }
                get { return _value.GetValue(); }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Luong.Mai
        /// Description: Check a success message is displayed
        /// </summary>
        public bool IsSuccessMessageIsDisplayed()
        {
            return SeleniumHelper.DoesWebElementExist(_successMessage);
        }

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
        public SuperannuationSchemesPage ClickSave()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SuperannuationSchemesPage();
        }

        #endregion
    }
}
