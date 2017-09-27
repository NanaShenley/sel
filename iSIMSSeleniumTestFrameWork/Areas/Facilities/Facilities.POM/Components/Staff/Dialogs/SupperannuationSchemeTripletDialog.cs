using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
{
    public class SupperannuationSchemeTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("superannuation_scheme_palette_triplet"); }
        }

        public SupperannuationSchemeTripletDialog()
        {
            _searchCriteria = new SuperannuationSchemeSearchDialog(this);
        }

        #region Search

        public class SupperannuationSchemeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Code']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        private readonly SuperannuationSchemeSearchDialog _searchCriteria;
        public SuperannuationSchemeSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        #endregion

        #region Actions

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>

        public void SelectSearchTile(SupperannuationSchemeSearchResultTile supperannuationSchemeTile)
        {
            if (supperannuationSchemeTile != null)
            {
                supperannuationSchemeTile.Click();
            }
        }

        public void ClickOK()
        {
            _okButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion
    }

    public class SuperannuationSchemeSearchDialog : SearchCriteriaComponent<SupperannuationSchemeTripletDialog.SupperannuationSchemeSearchResultTile>
    {
        public SuperannuationSchemeSearchDialog(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _code;

        public string SearchByCodeOrDescriptions
        {
            set { _code.SetText(value); }
            get { return _code.GetValue(); }
        }

        #endregion
    }
}
