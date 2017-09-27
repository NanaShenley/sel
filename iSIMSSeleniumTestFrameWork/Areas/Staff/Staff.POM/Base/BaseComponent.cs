using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Threading;

using WebDriverRunner.webdriver;
using Staff.POM.Helper;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using SeSugar.Interfaces;

namespace Staff.POM.Base
{
    public abstract class BaseComponent
    {
        private readonly IWebElement _context;
        private readonly By _contextIdentifier;
        private IWebElement _component;
        private static readonly ILogger _logger = SeSugar.Environment.Logger;

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
            _logger.LogLine("Initialising component: {0}", ComponentIdentifier);

            var wd = WebContext.WebDriver;
            WebDriverWait wait = new WebDriverWait(wd, TimeSpan.FromSeconds(10));
            if (_context != null)
            {
                wait.Until(ExpectedConditions.ElementIsVisible(new ByChained(_contextIdentifier, ComponentIdentifier)));
                _component = wd.FindElement(new ByChained(_contextIdentifier, ComponentIdentifier));


                PageFactory.InitElements(this, new ElementLocator(new ByChained(_contextIdentifier, ComponentIdentifier)));
            }
            else
            {
                wait.Until(ExpectedConditions.ElementIsVisible(ComponentIdentifier));
                _component = wd.FindElement(ComponentIdentifier);

                PageFactory.InitElements(this, new ElementLocator(ComponentIdentifier));
            }


            _logger.LogLine("Success");
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
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        protected TPageObject ClickAdd<TPageObject>() where TPageObject : BaseComponent, new()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_button")));
            AutomationSugar.WaitForAjaxCompletion();

            return new TPageObject();
        }

        public void ClickSave()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("well_know_action_save")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("well_know_action_save")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void ClickDelete()
        {
            AutomationSugar.WaitFor("delete_button");
            AutomationSugar.ClickOn("delete_button");
            AutomationSugar.WaitForAjaxCompletion();

            AutomationSugar.WaitFor(new ByChained(SimsBy.AutomationId("confirm_delete_dialog"), SimsBy.AutomationId("continue_with_delete_button")));
            AutomationSugar.ClickOn(new ByChained(SimsBy.AutomationId("confirm_delete_dialog"), SimsBy.AutomationId("continue_with_delete_button")));
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
                Retry.Do(accordion.Click);
        }
    }
}
