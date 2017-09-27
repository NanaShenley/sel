using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Base;
using PageObjectModel.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjectModel.Components.HomePages
{
    public class LeftMenu : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector(".hp-sidebar"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createNoticeBoardButton;

        #endregion

        #region Actions

        public bool DoesNoticeBoardExist(string name)
        {

            var listElements = SeleniumHelper.FindElements(SimsBy.CssSelector("[data-automation-id='notice-board-detail']"));
            foreach (var item in listElements)
            {
                if (item.GetText().Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAttachmentUnavailableExist()
        {
            return SeleniumHelper.IsElementExists(By.CssSelector(".popover.right"));
        }

        public CreateNoticeDialog CreateNotice()
        {
            _createNoticeBoardButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new CreateNoticeDialog();
        }

        #endregion

        #region Notice

        public ListItemComponent<NoticeItem> NoticeBoard
        {
            get { return new ListItemComponent<NoticeItem>(By.CssSelector(".schedule-notice-event")); }
        }

        public class NoticeItem
        {
            [FindsBy(How = How.CssSelector, Using = "[data-toggle='grid-popover']")]
            private IWebElement _title;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='attachment_button']")]
            private IWebElement _addAttachment;

            public string Title
            {
                get { return _title.GetText().Trim(); }
            }

            public void AddAttachment()
            {
                _addAttachment.Click();
                Wait.WaitForAjaxReady(By.CssSelector("[data-automation-id='attachment_button'][disabled='disabled']"));
            }

            public NoticePopover ViewDetail()
            {
                _title.Click();
                return new NoticePopover();
            }
        }

        #endregion

    }

    public class CreateNoticeDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("home_create_notice_record_details"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "Title")]
        private IWebElement _titleTextbox;

        [FindsBy(How = How.Name, Using = "Notice")]
        private IWebElement _noticeTextbox;

        [FindsBy(How = How.Name, Using = "PostingDate")]
        private IWebElement _startDateTextbox;

        [FindsBy(How = How.Name, Using = "PostingExpiryDate")]
        private IWebElement _expiryDateTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_button']")]
        private IWebElement _saveButton;

        public string Title
        {
            set { _titleTextbox.SetText(value); }
            get { return _titleTextbox.GetValue(); }
        }

        public string Notice
        {
            set { _noticeTextbox.SetText(value); }
            get { return _noticeTextbox.GetValue(); }
        }

        public string StartDate
        {
            set { _startDateTextbox.SetDateTime(value); }
            get { return _startDateTextbox.GetDateTime(); }
        }

        public string ExpiryDate
        {
            set { _expiryDateTextbox.SetDateTime(value); }
            get { return _expiryDateTextbox.GetDateTime(); }
        }

        #endregion

        #region Actions

        public void SaveNotice()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        #endregion
    }

    public class NoticePopover : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("popover-custom-id"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = ".popover-title")]
        private IWebElement _title;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='staff-notice-board-name']")]
        private IWebElement _notice;

        [FindsBy(How = How.CssSelector, Using = ".btn-close")]
        private IWebElement _closeButton;

        public string Title
        {
            get { return _title.GetText(); }
        }

        public string Notice
        {
            get { return _notice.GetText(); }
        }

        public void ClosePopup()
        {
            _closeButton.Click();
        }

        #endregion
    }
}
