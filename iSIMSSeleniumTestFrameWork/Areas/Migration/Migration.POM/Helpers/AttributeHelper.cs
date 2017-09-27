
using System.Threading;
using OpenQA.Selenium.Remote;
using WebDriverRunner.webdriver;
namespace Migration.POM.Helpers
{
    using System;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    public class AttributeHelper
    {
        public static void WaitForClickByAttribute(string tagType, string attribute, string value, int timeout)
        {
            IWebElement output = null;
            var driver = WebContext.WebDriver as RemoteWebDriver;

            var start = DateTime.UtcNow;
            double timeTaken;
            bool clicked = false;
            do
            {
                try
                {
                    output =
                        (from a in driver.FindElementsByTagName(tagType)
                         where a.GetAttribute(attribute) == value
                         select a)
                            .FirstOrDefault();

                    if (output == null)
                    {
                        Thread.Sleep(10);
                    }
                    else
                    {
                        output.Click();
                        clicked = true;
                        timeTaken = (DateTime.UtcNow - start).TotalMilliseconds;
                        Console.WriteLine(
                            "Found and clicked element of type [{0}] with [{1}] attribute value of [{2}] after [{3}ms]",
                            tagType,
                            attribute,
                            value,
                            timeTaken);
                    }
                }
                catch (Exception)
                {
                    Thread.Sleep(10);
                }

            } while (clicked == false && DateTime.UtcNow < start.AddMilliseconds(timeout));


            if (output == null)
            {
                var error =
                    string.Format(
                        "Unable to find and click element of type [{0}] with [{1}] attribute value of [{2}] after waiting for [{3}ms]",
                        tagType, attribute, value, timeout);

                throw new Exception(error);
            }
        }
        public static void WaitForClickByAttribute(IWebElement parent, string tagType, string attribute, string value, int timeout)
        {
            IWebElement output = null;

            var start = DateTime.UtcNow;
            double timeTaken;
            bool clicked = false;
            do
            {
                try
                {
                    output =
                        (from a in parent.FindElements(By.TagName(tagType))
                                    where a.GetAttribute(attribute) == value
                         select a)
                            .FirstOrDefault();

                    if (output == null)
                    {
                        Thread.Sleep(10);
                    }
                    else
                    {
                        output.Click();
                        clicked = true;
                        timeTaken = (DateTime.UtcNow - start).TotalMilliseconds;
                        Console.WriteLine(
                            "Found and clicked element of type [{0}] with [{1}] attribute value of [{2}] after [{3}ms]",
                            tagType,
                            attribute,
                            value,
                            timeTaken);
                    }
                }
                catch (Exception)
                {
                    Thread.Sleep(10);
                }

            } while (clicked == false && DateTime.UtcNow < start.AddMilliseconds(timeout));


            if (output == null)
            {
                var error =
                    string.Format(
                        "Unable to find and click element of type [{0}] with [{1}] attribute value of [{2}] after waiting for [{3}ms]",
                        tagType, attribute, value, timeout);

                throw new Exception(error);
            }
        }

        public static IWebElement WaitForGetByAttribute(string tagType, string attribute, string value, int timeout)
        {
            IWebElement output = null;
            var driver = WebContext.WebDriver as RemoteWebDriver;

            var start = DateTime.UtcNow;
            double timeTaken;

            do
            {
                output = (from a in driver.FindElementsByTagName(tagType) where a.GetAttribute(attribute) == value select a)
                        .FirstOrDefault();

                if (output == null)
                {
                    Thread.Sleep(250);
                }
                else
                {
                    timeTaken = (DateTime.UtcNow - start).TotalMilliseconds;
                    Console.WriteLine("Time taken to find element [{0}ms]", timeTaken);
                }

            } while (output == null && DateTime.UtcNow < start.AddMilliseconds(timeout));


            if (output == null)
            {
                var error =
                    string.Format(
                        "Unable to find element of type [{0}] with [{1}] attribute value of [{2}] after waiting for [{3}ms]",
                        tagType, attribute, value, timeout);

                throw new Exception(error);
            }

            return output;
        }

        public static IWebElement WaitForGetByAttribute(IWebElement parent, string tagType, string attribute, string value, int timeout)
        {
            IWebElement output = null;

            var start = DateTime.UtcNow;
            double timeTaken;

            do
            {
                output = (from a in parent.FindElements(By.TagName(tagType)) where a.GetAttribute(attribute) == value select a)
                        .FirstOrDefault();

                if (output == null)
                {
                    Thread.Sleep(250);
                }
                else
                {
                    timeTaken = (DateTime.UtcNow - start).TotalMilliseconds;
                    Console.WriteLine("Time taken to find element [{0}ms]", timeTaken);
                }

            } while (output == null && DateTime.UtcNow < start.AddMilliseconds(timeout));


            if (output == null)
            {
                var error =
                    string.Format(
                        "Unable to find element of type [{0}] with [{1}] attribute value of [{2}] after waiting for [{3}ms]",
                        tagType, attribute, value, timeout);

                throw new Exception(error);
            }

            return output;
        }

        public static IWebElement GetByAttribute(string tagType, string attribute, string value)
        {
            var driver = WebContext.WebDriver as RemoteWebDriver;

            var output = (from a in driver.FindElementsByTagName(tagType) where a.GetAttribute(attribute) == value select a).FirstOrDefault();

            if (output == null)
            {
                var error = string.Format("Unable to find element of type [{0}] with [{1}] attribute value of [{2}]", tagType, attribute, value);

                Console.WriteLine(error);
                throw new Exception(error);
            }

            return output;
        }

        public static bool TryGetByAttribute(string tagType, string attribute, string value, out IWebElement output)
        {
            var driver = WebContext.WebDriver as RemoteWebDriver;

            output = (from a in driver.FindElementsByTagName(tagType) where a.GetAttribute(attribute) == value select a).FirstOrDefault();

            if (output == null)
            {
                var error = string.Format("Unable to find element of type [{0}] with [{1}] attribute value of [{2}]", tagType, attribute, value);
                Console.WriteLine(error);

                return false;
            }

            return true;
        }
    }
}