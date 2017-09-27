using OpenQA.Selenium;
using System;

namespace PageObjectModel.Helper
{
    public class SimsBy
    {
        public static By AutomationId(string value)
        {
            return By.CssSelector(String.Format("[data-automation-id='{0}']", value));
        }

        public static By Id(string value)
        {
            return By.Id(value);
        }

        public static By Name(string value)
        {
            return By.Name(value);
        }

        public static By Xpath(string value)
        {
            return By.XPath(value);
        }

        public static By CssSelector(string value)
        {
            return By.CssSelector(value);
        }
    }

}
