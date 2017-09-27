using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Linq;

namespace SharedServices.Components.PageObjects
{
    public class VariantScreen : BaseSeleniumComponents
    {
        private const string AutomationIdAttr = "data-automation-id";

        public static By ComponentIdentifier
        {
            get { return By.CssSelector("[data-section-id=detailHost]"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[" + AutomationIdAttr + "^=variant_]")]
        private IWebElement screen;

        [FindsBy(How = How.CssSelector, Using = "[name=VariantField]")]
        private IWebElement field;

        [FindsBy(How = How.CssSelector, Using = "[name=ActionableField]")]
        private IWebElement _actionData;

        public VariantScreen()
        {
            var wd = WebContext.WebDriver;
            var wait = new WebDriverWait(wd, TimeSpan.FromSeconds(10));

            wait.Until(ExpectedConditions.ElementIsVisible(ComponentIdentifier));
            PageFactory.InitElements(wd, this);
        }

        public string AutomationId
        {
            get { return screen.GetAttribute(AutomationIdAttr); }
        }

        public bool FieldPresent
        {
            get
            {
                try
                {
                    return field.Displayed;
                }
                catch
                {
                    return false;
                }
            }
        }

        public string Label
        {
            get
            {
                var id = field.GetAttribute("id");
                var selector = string.Format("[for='{0}']", id);

                var label = WebContext.WebDriver.FindElement(By.CssSelector(selector));
                label = label.FindElement(By.CssSelector("[title]"));
                return label.GetAttribute("title");
            }
        }

        public string ActionData
        {
            get { return _actionData.GetValue(); }
        }

        public bool JobStepTestLinkedSecurableExists
        {
            get { return WebContext.WebDriver.FindElements(By.CssSelector("[name=TestLinkedSecurable]")).Any(); }
        }

        public bool JobStepTestLinkedSecurableForInvalidSecurityDomainExists
        {
            get { return WebContext.WebDriver.FindElements(By.CssSelector("[name=TestLinkedSecurableForInvalidSecurityDomain]")).Any(); }
        }

        public void ClickGenerate()
        {
            AutomationSugar.ClickOn("generate_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        public bool GridContains(string value)
        {
            const string statement = @"return $(""#variant_items:contains('{0}')"").length > 0";
            var jsExecutor = (IJavaScriptExecutor)SeSugar.Environment.WebContext.WebDriver;

            var exists = (bool)jsExecutor.ExecuteScript(string.Format(statement, value));
            return exists;
        }

    }
}
