using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestSettings;
using WebDriverRunner.webdriver;
using System.Diagnostics;
using System.Threading;
using POM.Helper;

namespace SharedComponents.BaseFolder
{
    public class BaseSeleniumComponents
    {
        public static void WaitUntilDisplayed(TimeSpan timeout, By loc)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, timeout);
            WaitLogic(wait, loc);
        }

        public static void WaitUntilDisplayed(By loc)
        {
            WaitUntilDisplayed(BrowserDefaults.TimeOut,loc);
        }

        protected void WaitForElement(TimeSpan timeout, By loc)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, timeout);
            // Note: driver.FindElement throws NoSuchElementException and fails immediately, incorrect behaviour for this method:
            //http://stackoverflow.com/questions/37127667/defaultwaitt-until-behavior-for-polling
            //wait.Until(driver => driver.FindElement(loc));

            wait.Until(ExpectedConditions.ElementExists(loc));
        }
        
        protected void WaitForElement(By loc)
        {
            WaitForElement(BrowserDefaults.TimeOut, loc);
        }

        public static void WaitForAndClick(TimeSpan timeout, By loc)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, timeout);
            WaitLogic(wait, loc);

            var element = WebContext.WebDriver.FindElement(loc);
            element.Click();
        }

        public static void WaitForAndSetValue(TimeSpan timeout, By loc, string value, bool hasDefault)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, timeout);
            WaitLogic(wait, loc);

            var element = WebContext.WebDriver.FindElement(loc);
            element.Click();

            if (hasDefault)
            {
                element.Clear();
            }

            element.SendKeys(value);
            element.SendKeys(Keys.Tab);
        }

        public static IWebElement WaitForAndGet(By loc)
        {
            return WaitForAndGet(BrowserDefaults.TimeOut, loc);
        }

        public static IWebElement WaitForAndGet(TimeSpan timeout, By loc)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, timeout);
            WaitLogic(wait, loc);

            return WebContext.WebDriver.FindElement(loc);
        }

        public static void WaitLogic(WebDriverWait wait, By loc)
        {
            wait.Until(driver =>
            {
                try
                {
                    var findElement = driver.FindElement(loc);  
                    return findElement.Displayed || findElement.GetAttribute("type") == "hidden";
                }
                catch
                {
                    return false;
                }
            });
        }

        public static void WaitUntilEnabled(By loc)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);            
            wait.Until(driver =>
            {
                try
                {
                    var findElement = driver.FindElement(loc);
                    return findElement.Enabled || findElement.GetAttribute("type") == "hidden";
                }
                catch
                {
                    return false;
                }
            });
        }

        public static bool WaitUntillAjaxRequestCompleted()
        {
            bool ajaxIsComplete = false;
            Stopwatch waiting = new Stopwatch();
            var timeOut = BrowserDefaults.TimeOut;
            waiting.Start();
            while (waiting.Elapsed <= timeOut)
            {
                string openConnections = (WebContext.WebDriver as IJavaScriptExecutor).ExecuteScript("return jQuery.active").ToString();

                ajaxIsComplete = openConnections == "0";
                if (ajaxIsComplete)
                    break;

                Thread.Sleep(10);
            }
            waiting.Stop();

            if (ajaxIsComplete)
                Console.WriteLine("Ajax request completed in approximately {0}ms.", waiting.ElapsedMilliseconds);
            else
                throw new WebDriverTimeoutException(string.Format("6. Waiting for connections to close timed out after {0}ms", timeOut.Milliseconds));
            return true;
        }
    }
}