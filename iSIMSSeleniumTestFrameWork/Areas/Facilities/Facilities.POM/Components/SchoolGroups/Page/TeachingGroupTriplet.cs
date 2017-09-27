using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;


namespace POM.Components.SchoolGroups
{
    public class TeachingGroupTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public TeachingGroupTriplet()
        {
            this.Refresh();
            _searchCriteria = new TeachingGroupSearchPage(this);
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        private IWebElement  _errorMessage;

        #endregion

        #region Search

        public class TeachingGroupResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly TeachingGroupSearchPage _searchCriteria;
        public TeachingGroupSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Action

        public TeachingGroupPage ClickCreate()
        {
            if (_createButton.IsExist())
            {
                _createButton.ClickByJS();

                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new TeachingGroupPage();
        }

        public void ClickSave()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                var deleteConfirmationPage = new WarningConfirmationDialog();
                deleteConfirmationPage.ConfirmDelete();
            }
        }

        public bool DoesMessageSuccessDisplay()
        {
            return _messageSuccess.IsExist();
        }

        public bool IsErrorMessageDisplay()
        {

            return _errorMessage.IsExist();
        }
        #endregion

        
    }
}
