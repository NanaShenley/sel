using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
{
    public class PaySpineDialogTriplet : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_pay_scales_pay_spine_dialog"); }
        }

        public PaySpineDialogTriplet()
        {
            _searchCriteria = new PaySpineSearchDialog(this);
        }

        #region Search

        private readonly PaySpineSearchDialog _searchCriteria;
        public PaySpineSearchDialog SearchCriteria { get { return _searchCriteria; } }

        public class PaySpineDialogSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _code;
            public string Code
            {
                get { return _code.Text; }
            }
        }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_button']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "data-automation-id='pay_spines_create_button']")]
        private IWebElement _createButton;

        #endregion

        #region Public methods

        public void SelectSearchTile(PaySpineDialogSearchResultTile paySpineTile)
        {
            if (paySpineTile != null)
            {
                paySpineTile.Click();
            }
            
        }
        public void ClickSave()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }
         
        #endregion
    }
    public class PaySpineSearchDialog : SearchCriteriaComponent<PaySpineDialogTriplet.PaySpineDialogSearchResultTile>
    {
        public PaySpineSearchDialog(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _searchTextBox;

        public string SearchByCode
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion
    }
}
