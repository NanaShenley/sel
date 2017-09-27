using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class AllowanceTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_allowances_dialog"); }
        }

        public AllowanceTripletDialog()
        {
            _searchCriteria = new AllowanceSearchDialog(this);
        }

        #region Search

        public class AllowanceSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Code']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        private readonly AllowanceSearchDialog _searchCriteria;
        public AllowanceSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='service_terms_allowances_dialog_create_button']")]
        private IWebElement _createButton;

        #endregion

        #region Page Actions

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        public void SelectSearchTile(AllowanceSearchResultTile paySpineTile)
        {
            if (paySpineTile != null)
            {
                paySpineTile.Click();
            }
        }

        public new void ClickCreate()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("service_terms_allowances_dialog_create_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("service_terms_allowances_dialog_create_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        #endregion
    }

    public class AllowanceSearchDialog : SearchCriteriaComponent<AllowanceTripletDialog.AllowanceSearchResultTile>
    {
        public AllowanceSearchDialog(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _searchByCodeTextBox;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _searchByDescriptionTextBox;

        public string SearchByCode
        {
            set { _searchByCodeTextBox.SetText(value); }
            get { return _searchByCodeTextBox.GetValue(); }
        }

        public string SearchByDescription
        {
            set { _searchByDescriptionTextBox.SetText(value); }
            get { return _searchByDescriptionTextBox.GetValue(); }
        }

        #endregion
    }

    public class AllowanceDetailsDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_allowances_dialog_detail"); }
        }

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
            get { return _Category.GetValue(); }
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

        public void ClickAddAward()
        {
            AutomationSugar.WaitFor("add_award_button");
            AutomationSugar.ClickOn("add_award_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        public GridComponent<AllowanceDialog.AwardsRow> Awards
        {
            get
            {
                return new GridComponent<AllowanceDialog.AwardsRow>(By.CssSelector("[data-maintenance-container='AllowanceAwards']"), ComponentIdentifier);
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
    }
}
