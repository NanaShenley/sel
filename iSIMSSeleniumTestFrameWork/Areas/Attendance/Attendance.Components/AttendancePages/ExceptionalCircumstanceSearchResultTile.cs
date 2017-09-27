using Attendance.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using POM.Helper;
using System.Linq;
using System.Threading;
using WebDriverRunner.webdriver;

namespace Attendance.Components.AttendancePages
{
    public class ExceptionalCircumstanceSearchResultTile : BaseSeleniumComponents
    {
#pragma warning disable 0649
        public const string SearchResultsName = "search_results";
        public const string SearchResultCounter = "search_results_counter";
        public const string SearchResultFormat = "search_result_{0}";

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
        public readonly IWebElement ResultTile;
        

        public ExceptionalCircumstanceSearchResultTile()
        {
            WaitForAndGet(AttendanceElements.ExceptionalCircumstanceElements.SearchResults);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public static string SearchResultsSection
        {
            get { return SeleniumHelper.AutomationId(SearchResultsName); }
        }

        public static string SearchResultTile
        {
            get { return SeleniumHelper.AutomationId(SearchResultsName); }
        }

        public bool HasResults()
        {
            var css = string.Format("{0} {1}", SearchResultsSection, SearchResultTile);
            return WebContext.WebDriver.FindElements(By.CssSelector(css)).Any();
        }

        public bool HasResults(int count)
        {
            var css = string.Format("{0} {1}", SearchResultsSection, SearchResultCounter);
            return WebContext.WebDriver.FindElements(By.CssSelector(css)).Count == count;
        }

        public string SearchResultCount()
        {
            return WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='search_results_counter']")).Text;
        }

        public CreateExceptionalCircumstancesPage ClickSearchResults()
        {
            WaitUntilEnabled(AttendanceElements.ExceptionalCircumstanceElements.SearchResultTile);
            ResultTile.Click();
            Thread.Sleep(2000);
            return new CreateExceptionalCircumstancesPage();
        }
    }
}
