using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class AllowanceDetailsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-automation-id='allowance_record_detail']"); }
        }

        #region Page properties

        public GridComponent<AllowancesRow> Allowances
        {
            get
            {
                return new GridComponent<AllowancesRow>(By.CssSelector("[data-maintenance-container='Rows']"), ComponentIdentifier);
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
                get { return _code.GetValue(); }
            }

            public string Description
            {
                set { _Description.SetText(value); }
                get { return _Description.GetValue(); }
            }

            public string DisplayOrder
            {
                set { _DisplayOrder.SetText(value); }
                get { return _DisplayOrder.GetValue(); }
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
    }
}
