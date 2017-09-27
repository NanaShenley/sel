using DataExchange.POM.Helper;
using OpenQA.Selenium;
using System;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Base
{
    public class TabComponent
    {
        public string Title;

        public bool CheckTabExists(string tabId)
        {
            bool isExists = false;

            IWebElement element = WebContext.WebDriver.FindElementSafe(By.Id(tabId));
            if (element.IsElementExists())
            {
                isExists = true;
            }

            return isExists;
        }

        public void ClickTab(string tabId)
        {
            IWebElement element = WebContext.WebDriver.FindElementSafe(By.Id(tabId));
            if (element.IsElementDisplayed())
            {
                //element.Click();
                SeleniumHelper.WaitForElementClickableThenClick(element);
            }
        }

        public bool CheckTabElementExists(string elementID)
        {
            bool isExists = false;

            IWebElement element = WebContext.WebDriver.FindElementSafe(By.CssSelector(elementID));
            if (element.IsElementExists())
            {
                isExists = true;
            }

            return isExists;
        }

        public IWebElement GetTabElement(string elementID)
        {
            IWebElement element = WebContext.WebDriver.FindElementSafe(By.CssSelector(elementID));
            return element;
        }
    }
}
