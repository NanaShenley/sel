using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using POM.Helper;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestSettings;
using WebDriverRunner.webdriver;

namespace POM.Helper
{
    public static class Wait
    {
        public static void WaitUntilDisplayed(TimeSpan timeout, By loc)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, timeout);
            WaitLogic(wait, loc);
        }

        public static void WaitUntilDisplayed(By loc)
        {
            WaitUntilDisplayed(BrowserDefaults.TimeOut, loc);
        }

        public static void WaitForElement(TimeSpan timeout, By loc)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, timeout);
            wait.Until(driver => driver.FindElement(loc));
        }

        public static void WaitForElement(By loc)
        {
            WaitForElement(BrowserDefaults.ElementTimeOut, loc);
        }

        public static void WaitForAndClick(TimeSpan timeout, By loc)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, timeout);
            WaitLogic(wait, loc);

            var element = WebContext.WebDriver.FindElement(loc);
            element.ClickByJS();
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


        /// <summary>
        /// Au: Logigear
        /// Des: Wait for a control until it's visiabled with time out is inputed
        /// </summary>
        /// <param name="by"></param>
        /// <param name="timeout"></param>
        public static void WaitForControl(By by)
        {
            try
            {
                WebDriverWait _wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.ElementTimeOut);
                _wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
            catch (Exception e)
            {
                Console.WriteLine("WaitForControl: " + e.Message);
            }
        }

        public static bool WaitForAjaxReady()
        {
            Console.WriteLine("Waiting for Ajax request to complete");
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
                throw new WebDriverTimeoutException(string.Format("3. Waiting for connections to close timed out after {0}ms", timeOut.Milliseconds));

            return true;

            //SJOOTLE: Temp - Replacing logigear ajax wait. Leaving old code here incase revert needed, and to emphasise that this is a *trial*.
            /*try
            {
                IWebDriver driver = WebContext.WebDriver;
                SeleniumHelper.Sleep(2);
                WebDriverWait wait = new WebDriverWait(driver, BrowserDefaults.AjaxElementTimeOut);
                return wait.Until<bool>((d) =>
                {
                    try
                    {
                        if (driver.FindElements(ajaxProgressElement).Any() == false)
                        {
                            SeleniumHelper.Sleep(2);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        return true;
                    }
                });
            }
            catch (Exception)
            {
                return true;
            }*/
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Wait for an ajax request is completed
        /// Wait for progress bar is invisibled
        /// </summary>
        public static bool WaitForAjaxReady(By ajaxProgressElement)
        {
            return WaitForAjaxReady();
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Wait for an element ready
        /// </summary>
        public static bool WaitForElementReady(By element)
        {
            try
            {
                IWebDriver driver = WebContext.WebDriver;
                WebDriverWait wait = new WebDriverWait(driver, BrowserDefaults.AjaxElementTimeOut);
                return wait.Until<bool>((d) =>
                {
                    try
                    {
                        return driver.FindElements(element).Any() == true;
                    }
                    catch (Exception)
                    {
                        return true;
                    }
                });
            }
            catch (Exception)
            {
                return true;
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Wait 
        /// </summary>
        public static bool WaitLoading()
        {
            try
            {
                IWebDriver driver = WebContext.WebDriver;
                WebDriverWait wait = new WebDriverWait(driver, BrowserDefaults.WaitLoading);
                return wait.Until<bool>((d) =>
                {
                    return false;
                });
            }
            catch (Exception)
            {
                return true;
            }
        }

        /// <summary>
        /// Wait for UI Auto Element appears
        /// </summary>
        /// <param name="element"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public static bool WaitForUIAutoElementReady(AutomationElement element)
        {
            try
            {
                IWebDriver driver = WebContext.WebDriver;
                WebDriverWait wait = new WebDriverWait(driver, timeout: TimeSpan.FromSeconds(5));
                return wait.Until<bool>((d) =>
                {
                    try
                    {
                        return element != null;
                    }
                    catch (Exception)
                    {
                        return true;
                    }
                });
            }
            catch (Exception)
            {
                return true;
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Wait for a document is ready. Use same WaitForPageLoad()
        /// </summary>
        public static void WaitForDocumentReady()
        {
            IWebDriver driver = WebContext.WebDriver;
            var timeout = new TimeSpan(0, 5, 0);
            var wait = new WebDriverWait(driver, timeout);

            var javascript = driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("driver", "Driver must support javascript execution");

            wait.Until((d) =>
            {
                try
                {
                    string readyState = javascript.ExecuteScript(
                        "if (document.readyState) return document.readyState;").ToString();
                    return readyState.ToLower() == "complete";
                }
                catch (InvalidOperationException e)
                {
                    //Window is no longer available
                    return e.Message.ToLower().Contains("unable to get browser");
                }
                catch (WebDriverException e)
                {
                    //Browser is no longer available
                    return e.Message.ToLower().Contains("unable to connect");
                }
                catch (Exception)
                {
                    return false;
                }
            });

        }

        /// <summary>
        /// Au: Logigear
        /// Des: Wait for element enabled
        /// </summary>
        public static void WaitForElementEnabled(By element)
        {
            SeleniumHelper.Sleep(2);
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.ElementTimeOut);
            wait.Until<bool>((d) =>
            {
                try
                {
                    IWebElement webElement = d.FindElement(element);
                    return webElement.Enabled;
                }
                catch (Exception)
                {
                    // If the find fails, the element exists, and
                    // by definition, cannot then be visible.
                    return true;
                }
            });
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Wait for element displayed
        /// </summary>
        public static void WaitForElementDisplayed(By element)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.ElementTimeOut);
            wait.Until<bool>((d) =>
            {
                try
                {
                    IWebElement webElement = d.FindElement(element);
                    return webElement.Displayed;
                }
                catch (Exception)
                {
                    return true;
                }
            });
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Wait for element bring to view
        /// </summary>
        public static void WaitForElementBringToView(By parent, By element, int timeout = 30, int postTimeOut = 2)
        {
            Thread.Sleep(postTimeOut * 1000);
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(timeout));
            wait.Until<bool>((d) =>
            {
                try
                {
                    IWebElement webElement = d.FindElement(element);
                    IWebElement parentElement = d.FindElement(parent);
                    bool isInner = webElement.Location.Y >= parentElement.Location.Y
                                   && (webElement.Location.Y + webElement.Size.Height) <= (parentElement.Location.Y + parentElement.Size.Height);
                    return isInner;
                }
                catch (Exception)
                {
                    return true;
                }
            });
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Wait Until condition
        /// </summary>
        public static void WaitUntil(Func<IWebDriver, bool> condition)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.ElementTimeOut);
            wait.Until<bool>(condition);
        }
    }
}
