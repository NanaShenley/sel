using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using DataExchange.POM.Helper;
using System;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Base
{
    public abstract class BaseComponent
    {
        private readonly IWebElement _context;
        private IWebElement _component;
        private readonly By _contextIdentifier;

        protected IWebElement Component
        {
            get { return _component; }
        }

        public abstract By ComponentIdentifier { get; }

        protected BaseComponent()
        {
            InitialiseComponenent();
        }

        protected BaseComponent(BaseComponent context)
        {
            _context = context.Component;
            _contextIdentifier = context.ComponentIdentifier;
            InitialiseComponenent();
        }

        private void InitialiseComponenent()
        {
            var wd = WebContext.WebDriver;
            WebDriverWait wait = new WebDriverWait(wd, TimeSpan.FromSeconds(10));
      
            wait.Until(ExpectedConditions.ElementIsVisible(ComponentIdentifier));
            _component = wd.FindElement(ComponentIdentifier);

            PageFactory.InitElements(this, new ElementLocator(ComponentIdentifier));
        }

        public void Refresh()
        {
            InitialiseComponenent();
        }
    }

    public static class BaseComponentExtensions
    {
        public static BaseComponent currentComponent = null;

        public static IWebElement DeStaler(this IWebElement element, By selector)
        {
            try
            {
                var displayed = element.Displayed;
            }
            catch (StaleElementReferenceException)
            {
                element = WebContext.WebDriver.FindElement(selector);
            }
            return element;
        }

        public static void ExpandAccordion<TComponent, TElement>(this TComponent component, Func<TComponent, TElement> accordionElement)
            where TComponent : BaseComponent
            where TElement : IWebElement
        {
            IWebElement accordion = accordionElement(component);

            if (accordion.GetAttribute("aria-expanded").Equals("false", StringComparison.OrdinalIgnoreCase))
                Retry.Do(accordion.Click);
        }
    }
}
