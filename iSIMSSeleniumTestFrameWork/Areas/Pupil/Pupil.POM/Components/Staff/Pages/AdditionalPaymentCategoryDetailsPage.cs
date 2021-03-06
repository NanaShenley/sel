﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class AdditionalPaymentCategoryDetailsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_detail_category"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _statusSuccess;



        public GridComponent<AdditionalPaymentCategoriesRow> AdditionalPaymentCategories
        {
            get
            {
                GridComponent<AdditionalPaymentCategoriesRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AdditionalPaymentCategoriesRow>(By.CssSelector("[data-maintenance-grid-id='LookupsWithCategoryGrid1']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }



        public class AdditionalPaymentCategoriesRow: GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _Description;

            [FindsBy(How = How.CssSelector, Using = "[name$='DisplayOrder']")]
            private IWebElement _DisplayOrder;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsVisible']")]
            private IWebElement _IsVisible;

            [FindsBy(How = How.CssSelector, Using = "[name$='dropdownImitator']")]
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

        #region Public actions

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Click 'save' button to finish creating a additional category.
        /// </summary>
        public void ClickSave()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion
    }
}
