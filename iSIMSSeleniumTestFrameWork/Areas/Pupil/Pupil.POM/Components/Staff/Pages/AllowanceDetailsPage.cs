using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class AllowanceDetailsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-automation-id='allowance_record_detail']"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

    

        public GridComponent<AllowancesRow> Allowances
        {
            get
            {
                GridComponent<AllowancesRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AllowancesRow>(By.CssSelector("[data-maintenance-container='Rows']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }


        public class AllowancesRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _Description;

            [FindsBy(How = How.CssSelector, Using = "[name$='DisplayOrder']")]
            private IWebElement _DisplayOrder;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsVisible']")]
            private IWebElement _IsVisible;

            [FindsBy(How = How.CssSelector, Using = "[name$='AdditionalPaymentCategory.dropdownImitator']")]
            private IWebElement _Category;

            public string Code
            {
                set { _code.SetText(value); }
                get { return _code.GetAttribute("Value"); }
            }

            public string Description
            {
                set { _Description.SetText(value); }
                get { return _Description.GetAttribute("Value"); }
            }

            public string DisplayOrder
            {
                set { _DisplayOrder.SetText(value); }
                get { return _DisplayOrder.GetAttribute("Value"); }
            }

            public bool IsVisible
            {
                set { _IsVisible.Set(value); }
                get { return _IsVisible.IsChecked(); }
            }

            public string Category
            {
                set { _Category.EnterForDropDown(value); }
                get { return _Category.GetAttribute("Value"); }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Click Save button to save the data changed
        /// </summary>
        public void ClickSave()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion
    }
}
