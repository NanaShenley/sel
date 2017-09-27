using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Helper;
using System;
using WebDriverRunner.webdriver;

namespace PageObjectModel.Base
{
    public abstract class BaseComponent
    {
        private readonly IWebElement _context;
        private IWebElement _component;

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
            InitialiseComponenent();
        }

        private void InitialiseComponenent()
        {
            Retry.Do(() =>
            {
                _component = _context != null
                    ? _context.FindElement(ComponentIdentifier).DeStaler(ComponentIdentifier).WaitUntilState(ElementState.Displayed)
                    : WebContext.WebDriver.FindElement(ComponentIdentifier).DeStaler(ComponentIdentifier).WaitUntilState(ElementState.Displayed);

                PageFactory.InitElements(this, new ElementLocator(_component));
            });
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
