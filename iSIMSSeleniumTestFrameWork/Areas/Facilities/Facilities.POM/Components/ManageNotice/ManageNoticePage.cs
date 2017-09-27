using POM.Base;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Common;

namespace Facilities.POM.Components.ManageNotice
{
    public class ManageNoticePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("editableData"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        private IWebElement _validationError;

        [FindsBy(How = How.Name, Using = "Title")]
        private IWebElement _noticeTitle;

        [FindsBy(How = How.Name, Using = "Notice")]
        private IWebElement _noticeDescription;

        [FindsBy(How = How.Name, Using = "PostingDate")]
        private IWebElement _noticePostingDate;

        [FindsBy(How = How.Name, Using = "PostingExpiryDate")]
        private IWebElement _noticePostingExpiryDate;



        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");

        #endregion

        #region Action

        public string NoticeTitle
        {
            set { _noticeTitle.SetText(value); }
            get { return _noticeTitle.GetValue(); }
        }

        public string NoticeDescription
        {
            set { _noticeDescription.SetText(value); }
            get { return _noticeDescription.GetValue(); }
        }

        public string NoticePostingDate
        {
            get { return _noticePostingDate.GetValue(); }
            set { _noticePostingDate.SetDateTime(value); }
        }

        public string NoticePostingExpiryDate
        {
            get { return _noticePostingExpiryDate.GetValue(); }
            set { _noticePostingExpiryDate.SetDateTime(value); }
        }


        public bool IsSuccessMessageDisplayed()
        {
            return _successMessage.IsExist();
        }

        public void Save()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("well_know_action_save")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public DeleteConfirmationDialog Delete()
        {
            _deleteButton.ClickByJS();
            return new DeleteConfirmationDialog();
        }

        #endregion
    }
}
