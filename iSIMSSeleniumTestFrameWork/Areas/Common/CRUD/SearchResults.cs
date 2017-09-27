using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace SharedComponents.CRUD
{
    public static class SearchResults
    {
        public const string SearchResultsName = "search_results";
        public const string SearchResultName = "search_result";
        public const string SearchResultFormat = "search_result_{0}";

        public static string SearchResultsSection
        {
            get { return SeleniumHelper.AutomationId(SearchResultsName); }
        }

        public static string SearchResultTile
        {
            get { return SeleniumHelper.AutomationId(SearchResultName); }
        }

        public static bool HasResults()
        {
            var css = string.Format("{0} {1}", SearchResultsSection, SearchResultTile);
            return WebContext.WebDriver.FindElements(By.CssSelector(css)).Any();
        }

        public static bool HasResults(int count)
        {
            var css = string.Format("{0} {1}", SearchResultsSection, SearchResultTile);
            return WebContext.WebDriver.FindElements(By.CssSelector(css)).Count == count;
        }

        public static int SearchResultCount
        {
            get
            {
                var css = string.Format("{0} {1}", SearchResultsSection, SearchResultTile);
                return WebContext.WebDriver.FindElements(By.CssSelector(css)).Count;
            }
        }

        public static void WaitForResults()
        {
            Thread.Sleep(3000);
            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 10), By.CssSelector(SearchResultTile));
        }

        public static void Click(string tile)
        {
            var s = string.Format(SearchResultFormat, tile.Replace('-', '_'));

            SeleniumHelper.FindAndClick(string.Format("{0} {1}", SearchResultsSection, SeleniumHelper.AutomationId(s)));
        }

        public static void SelectSearchResult(int rowNumber)
        {
            ReadOnlyCollection<IWebElement> searchResultsCollection = GetSearchResults();
            if (searchResultsCollection != null && searchResultsCollection.Any())
            {
                IWebElement searchResult = searchResultsCollection.ElementAtOrDefault(rowNumber);
                if (searchResult != null)
                {
                    searchResult.Click();
                }
            }
        }

        public static ReadOnlyCollection<IWebElement> GetSearchResults()
        {
            string css = string.Format("{0} {1}", SearchResultsSection, SearchResultTile);
            return WebContext.WebDriver.FindElements(By.CssSelector(css));
        }
    }
}

