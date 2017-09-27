using System.Collections.Generic;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class ServiceTermAllowanceDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_term_allowance_detail"); }
        }

        #region Page properties

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

        [FindsBy(How = How.Name, Using = "AllowanceAwardAttached")]
        private IList<IWebElement> _allowanceType;

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

        public bool PersonalAllowance
        {
            set { _allowanceType[0].Set(value); }
            get { return _allowanceType[0].IsChecked(); }
        }

        public bool FixedAllowance
        {
            set { _allowanceType[1].Set(value); }
            get { return _allowanceType[1].IsChecked(); }
        }

        public GridComponent<AllowanceDialog.AwardsRow> Awards
        {
            get
            {
                return new GridComponent<AllowanceDialog.AwardsRow>(By.CssSelector("[data-maintenance-container='Allowance.AllowanceAwards']"), ComponentIdentifier);
            }
        }

        public class AwardsRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Date']")]
            private IWebElement _AwardDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='Amount']")]
            private IWebElement _Amount;

            public string AwardDate
            {
                set { _AwardDate.SetText(value); }
                get { return _AwardDate.GetValue(); }
            }

            public string Amount
            {
                set { _Amount.SetText(value); }
                get { return _Amount.GetValue(); }
            }
        }

        #endregion
    }
}
