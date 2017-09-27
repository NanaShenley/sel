using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.SchoolGroups;
using POM.Helper;

namespace Facilities.POM.Components.Visitor_Book
{
    public class ManageVisitTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public ManageVisitTriplet()
        {
            this.Refresh();
            _searchCriteria = new ManageVisitSearchPage(this);
        }

        #region Search

        public class ManageVisitSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly ManageVisitSearchPage _searchCriteria;
        public ManageVisitSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _CreateButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        #endregion

        #region Public methods

        public ManageVisitDetail Create()
        {
            //SeleniumHelper.Get(SimsBy.AutomationId("create_button")).ClickByJS();
            _CreateButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ManageVisitDetail();
        }


        public void SelectSearchTile(ManageVisitSearchResultTile schemeTile)
        {
            if (schemeTile != null)
            {
                schemeTile.Click();
            }
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new ConfirmDeleteDialog();
                confirmDialog.ClickContinueDelete();
            }
        }

        #endregion
    }

    public class ManageVisitSearchPage : SearchCriteriaComponent<ManageVisitTriplet.ManageVisitSearchResultTile>
    {
        public ManageVisitSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "VisitorName")]
        private IWebElement _visitorName;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startdate;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _enddate;

        public string SearchByVisitorName
        {
            set { _visitorName.SetText(value); }
            get { return _visitorName.GetValue(); }
        }

        #endregion Search
    }
}

