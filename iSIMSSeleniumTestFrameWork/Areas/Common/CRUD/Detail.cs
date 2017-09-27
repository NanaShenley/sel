using System;
using System.Threading;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace SharedComponents.CRUD
{
    public static class Detail
    {
        public const string DetailName = "editableData";

        public const string SectionKeyFormat = "[data-section-link='{0}']";

        public const string StatusSuccess = "status_success";
        
        public static string DetailPanel
        {
            get { return string.Format("#{0}", DetailName); }
        }
        
        public static bool IsSectionOpen(string section)
        {
            var findElement = WebContext.WebDriver.FindElement(By.CssSelector(CssFormat(section)));

            return IsSectionOpen(findElement);
        }

        private static bool IsSectionOpen(IWebElement findElement)
        {
            return findElement.Displayed && findElement.GetAttribute("aria-expanded") == "true";
        }

        private static string CssFormat(string section)
        {
            return String.Format("{0} {1}", DetailPanel, String.Format(SectionKeyFormat, section));
        }


        public static void OpenSection(string section)
        {
            var findElement = WebContext.WebDriver.FindElement(By.CssSelector(CssFormat(section)));

            if (!IsSectionOpen(findElement))
            {
                findElement.Click();
                Thread.Sleep(400);
            }
        }
        
        public static void UpdateValue(string property, object value)
        {
            var css = string.Format("{0} [name='{1}']", DetailPanel, property);

            ControlsHelper.UpdateValue(css, value);
        }

        public static void Save()
        {
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("well_know_action_save"));
        }

        public static void WaitForDetail()
        {   Thread.Sleep(5000);
            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), By.Id(DetailName)); 
        }

        public static void WaitForStatus()
        {
            Thread.Sleep(2000);
            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), By.CssSelector(SeleniumHelper.AutomationId(StatusSuccess)));
        }

        public static bool HasConfirmedSave()
        {
            var css = SeleniumHelper.AutomationId(StatusSuccess);
            return WebContext.WebDriver.FindElement(By.CssSelector(css)).Displayed;
        }

        public static void ClickAccordion()
        {
            
        }
    }
}