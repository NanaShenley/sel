using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using TestSettings;
using WebDriverRunner.webdriver;

namespace PageObjectModel.Helper
{
    /// <summary>
    /// Denotes the states of a DOM Element.
    /// </summary>
    [Flags]
    public enum ElementState
    {
        Displayed = 1,
        Enabled = 2,
        Selected = 4
    }

    /// <summary>
    /// Static helper methods which safely retrieve elements from the DOM.
    /// </summary>
    public static class ElementRetriever
    {
        public static IWebElement WaitUntilCondition(this IWebElement element, Func<IWebElement, bool> condition)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(x => condition.Invoke(element));
            return element;
        }

        //public static Boolean IsExisted(this IWebElement element)
        //{
        //    try
        //    {
        //        if (element.Displayed)
        //        {
        //            return true;
        //        }
        //        return true;
        //    }
        //    catch (NoAlertPresentException nex)
        //    {
        //        Console.WriteLine(nex.Message);
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return true;
        //    }
        //}


        /// <summary>
        /// Wait until a given element state condition is met.
        /// </summary>
        /// <param name="element">The element to test.</param>
        /// <param name="elementState">The state flags to test for.</param>
        /// <returns></returns>
        public static IWebElement WaitUntilState(this IWebElement element, ElementState elementState)
        {
            Guid id = Guid.NewGuid();
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(x => UntilAction(element, elementState, x, id));
            return element;
        }

        //wrap these two gets in a retry
        /// <summary>
        /// Gets an element from the DOM once it has loaded.
        /// </summary>
        /// <param name="selector">A selector describing which element to get once it's loaded. Scoped to the document root.</param>
        /// <returns></returns>
        public static IWebElement GetOnceLoaded(By selector)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            IWebElement foundElement = null;
            wait.Until(x =>
            {
                foundElement = x.FindElementSafe(selector);
                return foundElement;
            });
            return foundElement;
        }

        /// <summary>
        /// Gets an element from the DOM once it has loaded.
        /// </summary>
        /// <param name="parent">The containing element.</param>
        /// <param name="selector">A selector describing which element to get once it's loaded. Scoped by the element this method is called upon.</param>
        /// <returns></returns>
        public static IWebElement GetOnceLoaded(IWebElement parent, By selector)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            IWebElement foundElement = null;
            wait.Until(x =>
            {
                foundElement = parent.FindElementSafe(selector);
                return foundElement;
            });
            return foundElement;
        }

        public static IWebElement FindElementSafe(this IWebElement element, By selector)
        {
            IWebElement childElement = null;
            Retry.Do(() =>
            {
                childElement = element.FindElement(selector);
            });

            return childElement;
        }

        public static IWebElement FindElementSafe(this IWebDriver driver, By selector)
        {
            IWebElement childElement = null;
            Retry.Do(() =>
            {
                childElement = driver.FindElement(selector);
            });

            return childElement;
        }

        public static ReadOnlyCollection<IWebElement> FindElementsSafe(this IWebElement element, By selector)
        {
            ReadOnlyCollection<IWebElement> childElements = null;
            Retry.Do(() =>
            {
                childElements = element.FindElements(selector);
            });

            return childElements;
        }

        public static void WaitUntilElementIsInView(this IWebElement element)
        {
            var shell = GetOnceLoaded(By.CssSelector("#shell"));
            var shellHeight = shell.Size.Height;
            Guid id = Guid.NewGuid();
            while (element.Location.Y > shellHeight)
            {
                Debug.WriteLine(string.Format("****WAITING FOR POSITION {0} : {1}, {2}****", element.Location.Y, shellHeight, id));
                Thread.Sleep(50);
            }
        }


        public static IWebElement ClickUntilAppearElement(this IWebElement element, By selector, int count = 16)
        {
            bool result = false;
            do
            {
                //element.ClickByJS();
                Retry.Do(element.Click);
                count--;
                try
                {
                    var targetElement = SeleniumHelper.FindElementWithOutTimeout(selector);
                    if (targetElement.Displayed)
                        return targetElement;
                }
                catch (Exception ex)
                {
                    result = false;
                }
            } while (count > 0 && result == false);
            return null;
        }

        private static bool UntilAction(IWebElement element, ElementState elementState, IWebDriver webDriver, Guid id)
        {
            bool ret = true;

            Retry.Do(() =>
            {
                if (elementState.HasFlag(ElementState.Displayed))
                {
                    // element.WaitUntilElementIsInView();

                    ret = ret && element.Displayed; // && element.ElementIsInView();

                }

                if (elementState.HasFlag(ElementState.Enabled))
                {
                    ret = ret && element.Enabled;
                }

                if (elementState.HasFlag(ElementState.Selected))
                {
                    ret = ret && element.Selected;
                }
            });

            //Debug.WriteLine("****UNTIL {0} {1}****", id, ret);
            return ret;
        }

    }
}
