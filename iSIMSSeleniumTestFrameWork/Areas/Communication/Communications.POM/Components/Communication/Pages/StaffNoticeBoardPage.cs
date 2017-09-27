using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using POM.Base;
using POM.Helper;

namespace Attendance.POM.Components.Communications
{
    public class StaffNoticeBoardPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("editableData"); }
        }
        #region Properties
        [FindsBy(How = How.Name, Using = "Title")]
        private IWebElement _title;

        [FindsBy(How = How.Name, Using = "Notice")]
        private IWebElement _notice;

        [FindsBy(How = How.Name, Using = "PostingDate")]
        private IWebElement _postingDate;

        [FindsBy(How = How.Name, Using = "PostingExpiryDate")]
        private IWebElement _postingExpiryDate;


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='documents_button']")]
        private IWebElement _documentButton;

        [FindsBy(How = How.CssSelector, Using = "[id^='popover']")]
        private IWebElement _popOver;

        public string Title
        {
            set { _title.SetText(value); }
            get { return _title.GetValue(); }
        }

        public string Notice
        {
            set { _notice.SetText(value); }
            get { return _notice.GetText(); }
        }

        public string StartDate
        {
            set { _postingDate.SetDateTime(value); }
            get { return _postingDate.GetDateTime(); }
        }

        public string EndDate
        {
            set { _postingExpiryDate.SetDateTime(value); }
            get { return _postingExpiryDate.GetDateTime(); }
        }

        #endregion

        #region Action
        public void ClickDocument()
        {
            _documentButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector("[data-automation-id='documents_button'][disabled='disabled']"));

        }
        public bool DoesPopErrorExist()
        {
            return SeleniumHelper.IsElementExists(_popOver);
        }
        #endregion

    }
}
