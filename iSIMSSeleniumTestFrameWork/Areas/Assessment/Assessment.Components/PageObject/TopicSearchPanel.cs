using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace Assessment.Components.PageObject
{
  public  class TopicSearchPanel
    {
        public TopicSearchPanel()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "form[data-section-id='searchCriteria'] input[name='Name']")]
        private IWebElement TopicName;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement SearchButton;

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='search_results_counter'] > strong")]
        private IWebElement SearchResultCount;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='search_results_list']")]
        public IWebElement SearchResultList;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        public TopicSearchPanel Search()
        {                   
                //  waiter.Until(ExpectedConditions.TextToBePresentInElement(GradesetHeader, "Filter By"));
                waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButton));
                SearchButton.Click();
                while (true)
                {
                    if (SearchButton.GetAttribute("disabled") != "true")
                        break;
                }
            Thread.Sleep(4000);
            // This method allows user to wait until the results are getting displayed after click of serach button
            return new TopicSearchPanel();
        }

        /// <summary>
        /// It sets the given Aspect Description in the Description text field
        /// </summary>
        public void setTopicName(string SearchAspectName)
        {
            TopicName.Clear();
            TopicName.SendKeys(SearchAspectName);
        }


        public string GetSearchResultCount()
        {
            //Explicit Sleep is used over here because we are not sure till when should we wait to get those search result, as the serach criteria is different for diffrent cases. So, have to use Thread.Sleep over here
            //System.Threading.Thread.Sleep(5000);
            try
            {
                return SearchResultCount.Text;
            }
            catch
            {
                return "No Topics Found";
            }
        }

        public void SelectResult()
        {
            ReadOnlyCollection<IWebElement> searchResults = SearchResultList.FindElements((By.ClassName("search-result-tile")));
            foreach (IWebElement e in searchResults)
            {
                e.Click();
            }
        }
    }
}
