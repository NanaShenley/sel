using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.AllDocumentsComponent
{
    public class SearchRecords:BaseSeleniumComponents
    {
        private const string SearchSelectorToFind = "button[type='submit']";

        [FindsBy(How = How.CssSelector, Using = SearchSelectorToFind)]
        public IWebElement SearchPupils;

        public SearchRecords()
        {
            WaitForElement(By.CssSelector(SearchSelectorToFind));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public SearchResultTile SearchResultTile()
        {            
            SearchPupils.Click();
            Thread.Sleep(5000);
            return new SearchResultTile();
        }
    }
}
