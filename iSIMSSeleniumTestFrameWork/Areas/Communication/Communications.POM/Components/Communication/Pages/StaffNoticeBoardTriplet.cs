using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Communications;
using POM.Helper;
using POM.Base;
using POM.Components.Common;


namespace Attendance.POM.Components.Communications
{
    public class StaffNoticeBoardTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public StaffNoticeBoardTriplet()
        {
            this.Refresh();
            _searchCriteria = new StaffNoticeBoardSearchPage(this);
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

        #endregion

        #region Search

        public class StaffNoticeBoardSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly StaffNoticeBoardSearchPage _searchCriteria;
        public StaffNoticeBoardSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Action

        public StaffNoticeBoardPage ClickCreate()
        {
            if (_createButton.IsExist())
            {
                _createButton.ClickByJS();

                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new StaffNoticeBoardPage();
        }

        public void ClickSave()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
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
        #endregion

        public bool DoesMessageSuccessDisplay()
        {
            return _messageSuccess.IsExist();
        }
    }
}
