using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace Staff.Components.StaffRecord
{
    public class QueuedItemsScreen : BaseSeleniumComponents
    {
        #region By Strings

        #region Fields

        private const string _statusBy = "[name=\"Status\"]";
        private const string _completedAtBy = "[name=\"CompleteTime\"]";
        private const string _submittedByBy = "[name=\"InitiatingUser\"]";
        private const string _submittedAtBy = "[name=\"RequestTime\"]";
        private const string _messagesBy = "[name=\"FormattedCompletionCallStatus\"]";

        #endregion

        #region Buttons

        private const string _searchBy = "button[type='submit']";

        #endregion

        #endregion

        public QueuedItemsScreen()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        #region Web Elements

        #region Fields

        [FindsBy(How = How.CssSelector, Using = _statusBy)]
        public IWebElement Status;

        [FindsBy(How = How.CssSelector, Using = _completedAtBy)]
        public IWebElement CompletedAt;

        [FindsBy(How = How.CssSelector, Using = _submittedByBy)]
        public IWebElement SubmittedBy;

        [FindsBy(How = How.CssSelector, Using = _submittedAtBy)]
        public IWebElement SubmittedAt;

        [FindsBy(How = How.CssSelector, Using = _messagesBy)]
        public IWebElement Messages;

        #endregion

        #region Buttons

        [FindsBy(How = How.CssSelector, Using = _searchBy)]
        public IWebElement SearchButton;

        #endregion

        #endregion

        #region Actions

        public string ReadStatus()
        {
            return Status.GetAttribute("value");
        }

        public string ReadCompletedAt()
        {
            return CompletedAt.GetAttribute("value");
        }

        public string ReadSubmittedBy()
        {
            return SubmittedBy.GetAttribute("value");
        }

        public string ReadSubmittedAt()
        {
            return SubmittedAt.GetAttribute("value");
        }

        public string ReadMessages()
        {
            return Messages.GetAttribute("value");
        }

        public void Search()
        {
            SearchButton.Click();
        }

        #endregion
    }
}
