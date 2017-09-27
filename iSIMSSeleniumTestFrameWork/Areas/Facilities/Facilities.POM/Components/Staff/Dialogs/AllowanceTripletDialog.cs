using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
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
    }
}
