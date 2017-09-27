using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace Staff.Components.StaffRecord
{
    public class StaffServiceRecordScreen : BaseSeleniumComponents
    {
        #region By Strings

        #region Fields

        private const string _dateOfArrivalBy = "[name=\"DOA\"]";
        private const string _dateOfLeavingBy = "[name=\"DOL\"]";
        private const string _continuousServiceStartDateBy = "[name=\"ContinuousServiceStartDate\"]";
        private const string _localAuthorityStartDateBy = "[name=\"LocalAuthorityStartDate\"]";
        private const string _staffReasonForLeavingBy = "[name=\"StaffReasonForLeaving.dropdownImitator\"]";
        private const string _destinationBy = "[name=\"Destination\"]";
        private const string _previousEmployerBy = "[name=\"PreviousEmployer\"]";
        private const string _nextEmployerBy = "[name=\"NextEmployer\"]";
        private const string _notesBy = "[name=\"Notes\"]";

        #endregion

        #region Buttons

        private const string _okButtonBy = "//span[text()='OK']";
        private const string _cancelButtonBy = "//span[text()='Cancel']";

        #endregion
       
        #endregion

        public StaffServiceRecordScreen()
        {
            WaitForElement(By.CssSelector(_dateOfArrivalBy));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        #region Web Elements

        #region Fields

        [FindsBy(How = How.CssSelector, Using = _dateOfArrivalBy)]
        public IWebElement DOA;

        [FindsBy(How = How.CssSelector, Using = _continuousServiceStartDateBy)]
        public IWebElement ContinuousServiceStartDate;

        [FindsBy(How = How.CssSelector, Using = _localAuthorityStartDateBy)]
        public IWebElement LocalAuthorityStartDate;

        [FindsBy(How = How.CssSelector, Using = _dateOfLeavingBy)]
        public IWebElement DOL;

        [FindsBy(How = How.CssSelector, Using = _staffReasonForLeavingBy)]
        public IWebElement StaffReasonForLeaving;

        [FindsBy(How = How.CssSelector, Using = _destinationBy)]
        public IWebElement Destination;

        [FindsBy(How = How.CssSelector, Using = _previousEmployerBy)]
        public IWebElement PreviousEmployer;

        [FindsBy(How = How.CssSelector, Using = _nextEmployerBy)]
        public IWebElement NextEmployer;

        [FindsBy(How = How.CssSelector, Using = _notesBy)]
        public IWebElement Notes;

        #endregion

        #region Buttons

        [FindsBy(How = How.XPath, Using = _okButtonBy)]
        public IWebElement OKButton;

        [FindsBy(How = How.XPath, Using = _cancelButtonBy)]
        public IWebElement CancelButton;

        #endregion

        #endregion

        #region Actions

        public void EnterDOA(string value)
        {
            DOA.Clear();
            DOA.SendKeys(value);
        }

        public void EnterContinuousServiceStartDate(string value)
        {
            ContinuousServiceStartDate.Clear();
            ContinuousServiceStartDate.SendKeys(value);
        }

        public void EnterLocalAuthorityStartDate(string value)
        {
            LocalAuthorityStartDate.Clear();
            LocalAuthorityStartDate.SendKeys(value);
        }

        public void EnterPreviousEmployer(string value)
        {
            PreviousEmployer.Clear();
            PreviousEmployer.SendKeys(value);
        }

        public void EnterNextEmployer(string value)
        {
            NextEmployer.Clear();
            NextEmployer.SendKeys(value);
        }

        public void EnterNotes(string value)
        {
            Notes.Clear();
            Notes.SendKeys(value);
        }

        public StaffDetail ClickOK()
        {
            OKButton = WaitForAndGet(By.XPath(_okButtonBy));
            OKButton.Click();
            return new StaffDetail();
        }

        public StaffDetail ClickCancel()
        {
            CancelButton = WaitForAndGet(By.XPath(_cancelButtonBy));
            CancelButton.Click();
            return new StaffDetail();
        }

        #endregion
    }
}
