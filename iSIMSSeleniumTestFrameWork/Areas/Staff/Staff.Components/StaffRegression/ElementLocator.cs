using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Staff.Components.StaffRegression
{
    public class ElementLocator : IElementLocator
    {
        private readonly By _scopingElementSelector;
        private readonly IWebElement _scopingElement;

        public ElementLocator(IWebElement scopingElement)
        {
            _scopingElement = scopingElement;
        }

        public IWebElement LocateElement(IEnumerable<By> bys)
        {
            IWebElement element = null;

            Retry.Do(() =>
            {
                element = _scopingElement
                    .FindElement(bys.First())
                    .WaitUntilState(ElementState.Displayed);

            });
            return element;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> LocateElements(IEnumerable<By> bys)
        {
            return SearchContext.FindElement(_scopingElementSelector).WaitUntilState(ElementState.Displayed).FindElements(bys.First());
        }

        public ISearchContext SearchContext
        {
            get { return WebContext.WebDriver; }
        }
    }
}
