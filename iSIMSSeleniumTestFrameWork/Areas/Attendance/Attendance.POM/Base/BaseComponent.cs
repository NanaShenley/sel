using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using SeSugar.Automation;
using System;
using System.Collections.Generic;
using WebDriverRunner.webdriver;

namespace POM.Base
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
            POM.Helper.Retry.Do(() =>
            {
                _component = _context != null
                    ? _context.FindElement(ComponentIdentifier).DeStaler(ComponentIdentifier).WaitUntilState(ElementState.Displayed)
                    : WebContext.WebDriver.FindElement(ComponentIdentifier).DeStaler(ComponentIdentifier).WaitUntilState(ElementState.Displayed);

                PageFactory.InitElements(this, new ElementLocator(_component));
            });
        }

        public IEnumerable<string> Validation
        {
            get { return AutomationSugar.GetValidationMessages(ComponentIdentifier); }
        }

        public void Refresh()
        {
            InitialiseComponenent();
        }

        public void ClickAdd()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId("add_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId("add_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        protected TPageObject ClickAdd<TPageObject>() where TPageObject : BaseComponent, new()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId("add_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId("add_button")));
            AutomationSugar.WaitForAjaxCompletion();

            return new TPageObject();
        }

        public void ClickSave()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId("well_know_action_save")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId("well_know_action_save")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void ClickDelete()
        {
            AutomationSugar.WaitFor("delete_button");
            AutomationSugar.ClickOn("delete_button");
            AutomationSugar.WaitForAjaxCompletion();

            AutomationSugar.WaitFor(new ByChained(SeSugar.Automation.SimsBy.AutomationId("confirm_delete_dialog"), SeSugar.Automation.SimsBy.AutomationId("continue_with_delete_button")));
            AutomationSugar.ClickOn(new ByChained(SeSugar.Automation.SimsBy.AutomationId("confirm_delete_dialog"), SeSugar.Automation.SimsBy.AutomationId("continue_with_delete_button")));
            AutomationSugar.WaitForAjaxCompletion();
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
                POM.Helper.Retry.Do(accordion.Click);
        }
    }
}
