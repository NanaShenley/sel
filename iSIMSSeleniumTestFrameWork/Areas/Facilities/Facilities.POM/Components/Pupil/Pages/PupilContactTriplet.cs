using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class PupilContactTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public PupilContactTriplet()
        {
            _searchCriteria = new PupilContactSearchPage(this);
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        #endregion

        #region Search

        public class PupilContractSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Name']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly PupilContactSearchPage _searchCriteria;
        public PupilContactSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Action

        public PupilContactPage ClickCreate()
        {
            _createButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new PupilContactPage();
        }

        public void ClickSave()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion
    }
}
