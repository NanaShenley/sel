﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Helper
{
    public class ElementLocator : IElementLocator
    {
        private readonly By _scopingElementSelector;
        private readonly IWebElement _scopingElement;

        public ElementLocator(IWebElement scopingElement)
        {
            _scopingElement = scopingElement;
        }

        public ElementLocator(By scopingElementSelector)
        {
            _scopingElementSelector = scopingElementSelector;
        }


        public IWebElement LocateElement(IEnumerable<By> bys)
        {
            if (_scopingElementSelector != null)
            {
                IWebElement element = null;

                WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(10));

                wait.Until(ExpectedConditions.ElementIsVisible(new ByChained(_scopingElementSelector, bys.First())));
                element = WebContext.WebDriver.FindElement(new ByChained(_scopingElementSelector, bys.First()));

                return element;
            }
            else
            {
                IWebElement element = null;

                SeSugar.Automation.Retry.Do(() =>
                {
                    element = _scopingElement
                        .FindElement(bys.First())
                        .WaitUntilState(ElementState.Displayed);

                });
                return element;
            }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> LocateElements(IEnumerable<By> bys)
        {
            if (_scopingElementSelector != null)
            {
                WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(10));

                wait.Until(ExpectedConditions.ElementIsVisible(_scopingElementSelector));
                var element = WebContext.WebDriver.FindElement(_scopingElementSelector);

                return element.FindElements(bys.First());
            }
            else
            {
                return _scopingElement.WaitUntilState(ElementState.Displayed).FindElements(bys.First());
            }
        }

        public ISearchContext SearchContext
        {
            get { return WebContext.WebDriver; }
        }
    }
}
