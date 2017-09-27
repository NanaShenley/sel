using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestSettings;
using WebDriverRunner.webdriver;
using DataExchange.POM.Helper;

namespace DataExchange.POM.Components.HomePages
{
    public class QuickLinksBar
    {      
        [FindsBy(How = How.CssSelector, Using = "a[data-ajax-url$='Pupils/SIMS8LearnerMaintenanceSimpleLearner/Details']")]
        private IWebElement _pupilRecords;


        public QuickLinksBar()
        {
            var menu = WebContext.WebDriver.FindElement(By.CssSelector(".navbar-left ul[role='menubar']"));
            PageFactory.InitElements(menu, this);
        }

        public void PupilRecords()
        {
            Thread.Sleep(500);
            _pupilRecords.Click();
        }

        public QuickLinksBar WaitFor()
        {
            var loc = By.CssSelector("a[data-ajax-url$='/Attendance/EditMarks/Details']");
            Wait.WaitForElement(BrowserDefaults.TimeOut, loc);
            return this;
        }
    }
}