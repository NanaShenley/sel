using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using Selene.Support.Attributes;

namespace WebDriverRunnerTests.mocktests
{

    class ManualTest
    {
        [WebDriverTest]
        public void WebDriverSingleTest()
        {
            WebContext.WebDriver.Url = "http://10.128.0.156:4444/grid/console";
            Assert.AreEqual("Grid Console",WebContext.WebDriver.Title);
            WebContext.WebDriver.PageSource.Contains("Grid");
            IReadOnlyCollection<IWebElement> tabs = WebContext.WebDriver.FindElements(By.CssSelector("li[type='config']"));

            foreach (var element in tabs)
            {
                element.Click();
            }
            WebContext.Screenshot();
        }



    }
}
