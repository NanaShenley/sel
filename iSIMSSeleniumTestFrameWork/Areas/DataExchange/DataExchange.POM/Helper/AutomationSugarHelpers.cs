using OpenQA.Selenium;
using SeSugar.Automation;

namespace DataExchange.POM.Helper
{
    public static class AutomationSugarHelpers
    {
        public static void WaitForAndClickOn(By element)
        {
            AutomationSugar.WaitFor(element);
            AutomationSugar.ClickOn(element);
        }

        public static void WaitForAndClickOn(string element)
        {
            AutomationSugar.WaitFor(element);
            AutomationSugar.ClickOn(element);
        }
    }
}
