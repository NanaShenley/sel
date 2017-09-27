using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using DataExchange.POM.Helper;
using System.Collections.Generic;
using System.Linq;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Base
{
    public class ListItemComponent<TrItem> where TrItem : new()
    {
        protected IList<IWebElement> _components;
        protected readonly By _componentIdentifier;
        protected readonly By _parentComponentIdentifier;
        protected readonly List<TrItem> _items = new List<TrItem>();

        public TrItem this[int index]
        {
            get
            {
                var row = default(TrItem);
                try
                {
                    row = _items[index];
                }
                catch
                {
                    Retry.Do(() =>
                    {
                        Initialise();
                        row = _items[index];
                    });
                }

                return row;
            }
        }

        public List<TrItem> Items { get { return _items; } }

        public ListItemComponent(By componentIdentifier)
        {
            _componentIdentifier = componentIdentifier;
            Initialise();
        }

        public ListItemComponent(By componentIdentifier, By parentComponentIdentifier)
        {
            _componentIdentifier = componentIdentifier;
            _parentComponentIdentifier = parentComponentIdentifier;
            Initialise();
        }

        public void Refresh()
        {
            Initialise();
        }

        protected virtual void Initialise()
        {
            if (_parentComponentIdentifier != null)
            {
                _components = WebContext.WebDriver.FindElement(_parentComponentIdentifier).FindElements(_componentIdentifier);
            }
            else
            {
                _components = WebContext.WebDriver.FindElements(_componentIdentifier);
            }

            _items.Clear();
            foreach (var component in _components)
            {
                var item = new TrItem();
                PageFactory.InitElements(item, new ElementLocator(component));
                _items.Add(item);
            }
        }
    }
}
