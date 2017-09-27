using Attendance.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace Attendance.Components.AttendancePages
{
    public class ExceptionalCircumstanceSearchPage : BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.CssSelector, Using = "[name='StartDate']")]
        public readonly IWebElement searchCriteriaStartDate;
        [FindsBy(How = How.CssSelector, Using = "[name='EndDate']")]
        public readonly IWebElement searchCriteriaEndDate;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        public readonly IWebElement searchButton;

         public ExceptionalCircumstanceSearchPage()
        {
            WaitUntilEnabled(AttendanceElements.ExceptionalCircumstanceElements.SearchButton);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

         public void EnterDate(string date, IWebElement Date)
        {
            Date.Clear();
            Date.SendKeys(date);
        }

        public ExceptionalCircumstanceSearchResultTile ClickSearchButton()
        {
            WaitUntilEnabled(AttendanceElements.ExceptionalCircumstanceElements.SearchButton);
            searchButton.Click();
            //Thread.Sleep(2000);
            WaitForAndGet(AttendanceElements.ExceptionalCircumstanceElements.SearchResults);
            return new ExceptionalCircumstanceSearchResultTile();
        }

    }
}
