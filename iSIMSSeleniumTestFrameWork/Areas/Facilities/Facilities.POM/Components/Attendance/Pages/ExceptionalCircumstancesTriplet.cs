using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Attendance
{
    public class ExceptionalCircumstancesTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public ExceptionalCircumstancesTriplet()
        {
            _searchCriteria = new ExceptionalCircumstancesSearchPage(this);
        }

        public class ExceptionalCircumstancesSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }


        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Selected']")]
        private IWebElement _selectedPupilMenuItem;

        #endregion


        #region Search

        private readonly ExceptionalCircumstancesSearchPage _searchCriteria;
        public ExceptionalCircumstancesSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Actions

        public void Create()
        {
            _createButton.ClickByJS();
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='Selected']"));
        }

        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public ExceptionalCircumstancesDetailPage SelectSelectedPupils()
        {
            _selectedPupilMenuItem.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            return new ExceptionalCircumstancesDetailPage();
        }

        #endregion
    }
}
