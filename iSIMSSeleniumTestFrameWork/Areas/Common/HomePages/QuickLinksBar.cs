using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;

namespace SharedComponents.HomePages
{
    public class QuickLinksBar : BaseSeleniumComponents
    {
        private const string PupilRecordsCss = "a[data-ajax-url$='Pupils/SIMS8LearnerMaintenanceSimpleLearner/Details']";

        [FindsBy(How = How.CssSelector, Using = "a[data-ajax-url$='Attendance/EditMarks/Details']")]
        private IWebElement _editMarks;

        [FindsBy(How = How.CssSelector, Using = PupilRecordsCss)]
        private IWebElement _pupilRecords;

        [FindsBy(How = How.CssSelector, Using = "a[data-ajax-url$='Learner/PupilLog/Details']")]
        private IWebElement _pupilLog;

        [FindsBy(How = How.CssSelector, Using = "a[data-ajax-url$='Pupils/SIMS8LearnerContactMaintenanceSimpleLearnerContact/Details']")]
        private IWebElement _pupilContact;

        [FindsBy(How = How.CssSelector, Using = "a[data-ajax-url$='Staff/SIMS8StaffMaintenanceTripleStaff/Details']")]
        private IWebElement _staffRecords;

        public QuickLinksBar()
        {
            var menu = WebContext.WebDriver.FindElement(By.Id("shell"));
            PageFactory.InitElements(menu, this);
        }

        public void EditMarks()
        {
            Thread.Sleep(500);
            _editMarks.Click();
        }

        public void PupilContact()
        {
            Thread.Sleep(500);
            _pupilContact.Click();
        }

        public void StaffRecords()
        {
            Thread.Sleep(3000);
            _staffRecords.Click();
        }

        public void PupilLog()
        {
            Thread.Sleep(500);
            _pupilLog.Click();
        }

        public void PupilRecords()
        {
            Thread.Sleep(500);
            WaitUntilEnabled(By.CssSelector(PupilRecordsCss));
            _pupilRecords.Click();
        }

        public QuickLinksBar WaitFor()
        {
            var loc = By.CssSelector("a[data-ajax-url$='/Attendance/EditMarks/Details']");
            WaitForElement(BrowserDefaults.TimeOut, loc);
            return this;
        }
    }
}