using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.SchoolGroups;
using POM.Helper;

namespace Facilities.POM.Components.ManageTier
{
    public class ManageTierTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public ManageTierTriplet()
        {
            this.Refresh();
            _searchCriteria = new ManageTierSearchPage(this);
        }

        #region Search

        public class ManageTierSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly ManageTierSearchPage _searchCriteria;
        public ManageTierSearchPage SearchCriteria { get { return _searchCriteria; } }

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
        public ManageTierPage Create()
        {
            _CreateButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ManageTierPage();
        }

        public void SelectSearchTile(ManageTierSearchResultTile schemeTile)
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

        public void Save()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("well_know_action_save")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion
    }

    public class ManageTierSearchPage : SearchCriteriaComponent<ManageTierTriplet.ManageTierSearchResultTile>
    {
        public ManageTierSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        private IWebElement _searchByAcademicYear;

        public string SearchByAcademicYear
        {
            get { return _searchByAcademicYear.GetValue(); }
            set { _searchByAcademicYear.EnterForDropDown(value); }
        }

        #endregion Search
    }
}
