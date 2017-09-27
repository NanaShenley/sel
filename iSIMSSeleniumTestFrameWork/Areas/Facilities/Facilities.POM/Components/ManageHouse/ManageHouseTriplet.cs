using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.SchoolGroups;
using POM.Helper;

namespace Facilities.POM.Components.ManageHouse
{
    public class ManageHouseTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public ManageHouseTriplet()
        {
            this.Refresh();
            _searchCriteria = new ManageHouseSearchPage(this);
        }

        #region Search

        public class ManageHouseSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly ManageHouseSearchPage _searchCriteria;
        public ManageHouseSearchPage SearchCriteria { get { return _searchCriteria; } }

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
        public ManageHouseDetailPage Create()
        {
            //SeleniumHelper.Get(SimsBy.AutomationId("create_button")).ClickByJS();
            _CreateButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ManageHouseDetailPage();
        }        

        public void SelectSearchTile(ManageHouseSearchResultTile schemeTile)
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

    public class ManageHouseSearchPage : SearchCriteriaComponent<ManageHouseTriplet.ManageHouseSearchResultTile>
    {
        public ManageHouseSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "FullName")]
        private IWebElement _houseName;

        public string SearchByHouseName
        {
            set { _houseName.SetText(value); }
            get { return _houseName.GetValue(); }
        }

        #endregion Search
    }
}
