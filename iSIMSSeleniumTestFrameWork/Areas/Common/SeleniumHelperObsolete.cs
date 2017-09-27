using System;
using System.Diagnostics.Eventing.Reader;
using System.Resources;
using System.Threading;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.HomePages;
using TestSettings;
using WebDriverRunner.webdriver;

namespace SharedComponents
{
    [Obsolete("Please use class Selenium Helper.")]
    public static class SeleniumHelperObsolete
    {

        public const string AutomationIdAttributeFormat = "[data-automation-id='{0}']";


        public static string AutomationId(string value)
        {
            return string.Format(AutomationIdAttributeFormat, value);
        }


        #region Obsolete helpers
        [Obsolete("Please use the FindAndClick methods which take OpenQA.Selenium.By as a parameter.")]
        public static void FindAndClick(string cssFormat, params object[] cssElements)
        {
            FindAndClick(string.Format(cssFormat, cssElements));
        }

        [Obsolete("Please use the FindAndClick methods which take OpenQA.Selenium.By as a parameter.")]
        public static void FindAndClick(string css)
        {
            BaseSeleniumComponents.WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(css));
        }

        [Obsolete("Please use the equivalent fluent version of this helper.")]
        public static void FindAndSetValueForSelector(string css, string value, bool hasDefault = false)
        {
            FindAndClick(css);
            const string selectorSearchBox = "[id*=s2id_autogen][id$=_search]";
            BaseSeleniumComponents.WaitForAndSetValue(BrowserDefaults.TimeOut, By.CssSelector(selectorSearchBox), value, hasDefault);
        }

        [Obsolete("Please use the equivalent fluent version of this helper.")]
        public static void FindAndSetCheckBox(string css, bool @checked)
        {
            var element = BaseSeleniumComponents.WaitForAndGet(BrowserDefaults.TimeOut, By.CssSelector(css));

            var checkedValue = element.GetAttribute("checked");

            if (@checked)
            {
                //Assuming NULL means unchecked.
                if (checkedValue == null)
                    element.Click();
            }
            else
            {
                //Assuming !NULL means checked.
                if (checkedValue != null)
                    element.Click();
            }
        }

        [Obsolete("Please use the equivalent fluent version of this helper.")]
        public static void FindAndSetValue(string css, string value, bool hasDefault = false)
        {
            BaseSeleniumComponents.WaitForAndSetValue(BrowserDefaults.TimeOut, By.CssSelector(css), value, hasDefault);
        }

        [Obsolete("Please use the equivalent fluent version of this helper.")]
        public static string GetElementValue(string css)
        {
            IWebElement element = WebContext.WebDriver.FindElement(By.CssSelector(css));
            return element.GetAttribute("value");
        }

	    public static bool FindElement(string css)
	    {
			var element = BaseSeleniumComponents.WaitForAndGet(BrowserDefaults.TimeOut, By.CssSelector(css));
		    return element != null;
	    }

		public static void FindAndClearValue(string css)
		{
			BaseSeleniumComponents.WaitForAndSetValue(BrowserDefaults.TimeOut, By.CssSelector(css), "", true);
		}

        public static string GetElementText(string css)
        {
            IWebElement element = WebContext.WebDriver.FindElement(By.CssSelector(css));
            return element.Text;
        }

        #endregion
    }
}
