using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.PageObjects
{
    public class LookupScreen : BaseSeleniumComponents
    {
        public static By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_tester"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[name=\"LazyLookup.dropdownImitator\"]")]
        private IWebElement _lazyLookupElement;

        public LookupScreen()
        {
            var wd = WebContext.WebDriver;
            var wait = new WebDriverWait(wd, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(ComponentIdentifier));
            PageFactory.InitElements(wd, this);
        }

        public IWebElement LazyLookup
        {
            get
            {
                _lazyLookupElement.WaitUntilState(ElementState.Displayed);
                return _lazyLookupElement;
            }
        }

        public bool LookupContains(string value)
        {
            var helper = new CommonHelpers();
            return helper.VerifyDropdownContainsValue("Lookup.Binding", value);
        }

        public bool LazyLookupContains(string value)
        {
            var helper = new CommonHelpers();
            return helper.VerifyDropdownContainsValue("LazyLookup.Binding", value);
        }
    }
}
