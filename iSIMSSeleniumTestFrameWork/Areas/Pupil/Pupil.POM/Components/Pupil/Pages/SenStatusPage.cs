using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using SeSugar.Automation;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;

namespace POM.Components.Pupil
{
    public class SenStatusPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_detail_category"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        public class SENStatus : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _description;

            [FindsBy(How = How.CssSelector, Using = "[name$='DisplayOrder']")]
            private IWebElement _displayOrder;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsVisible']")]
            private IWebElement _isVisible;

            [FindsBy(How = How.CssSelector, Using = "[name$='Parent.dropdownImitator']")]
            private IWebElement _category;

            public string Code
            {
                set { _code.SetText(value); }
                get { return _code.GetAttribute("value"); }
            }

            public string Description
            {
                set { _description.SetText(value); }
                get { return _description.GetAttribute("value"); }
            }

            public string DisplayOrder
            {
                set { _displayOrder.SetText(value); }
                get { return _displayOrder.GetAttribute("value"); }
            }

            public bool IsVisible
            {
                set { _isVisible.Set(value); }
                get { return _isVisible.IsChecked(); }
            }

            public string Category
            {
                set { _category.EnterForDropDown(value); }
                get { return _category.GetAttribute("value"); }
            }
        }

        public GridComponent<SENStatus> SenStatusTable
        {
            get
            {
                GridComponent<SENStatus> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SENStatus>(By.CssSelector("[data-maintenance-grid-id='LookupsWithCategoryGrid1']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        #endregion

        #region Public methods

        public void ClickSave()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
        }

        public void DeleteTableRow(GridRow row)
        {
            if (row != null)
            {
                row.DeleteRow();
                ClickSave();
            }
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Verify success message is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessMessageDisplayed()
        {
            AutomationSugar.WaitForAjaxCompletion();
            return SeleniumHelper.IsElementExists(SimsBy.AutomationId("status_success"));
        }

        #endregion
    }
}
