using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.SchoolGroups;
using POM.Helper;

namespace Facilities.POM.Components.ManageNotice
{
    public class ManageNoticeTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public ManageNoticeTriplet()
        {
            this.Refresh();
            _searchCriteria = new ManageNoticeSearchPage(this);
        }

        #region Search

        public class ManageNoticeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly ManageNoticeSearchPage _searchCriteria;
        public ManageNoticeSearchPage SearchCriteria { get { return _searchCriteria; } }

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
        public ManageNoticePage Create()
        {
            _CreateButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ManageNoticePage();
        }

        public void SelectSearchTile(ManageNoticeSearchResultTile schemeTile)
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

    public class ManageNoticeSearchPage : SearchCriteriaComponent<ManageNoticeTriplet.ManageNoticeSearchResultTile>
    {
        public ManageNoticeSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "TitleOfSearch")]
        private IWebElement _NoticeName;

        public string SearchByNoticeName
        {
            set { _NoticeName.SetText(value); }
            get { return _NoticeName.GetValue(); }
        }

        #endregion Search
    }
}
