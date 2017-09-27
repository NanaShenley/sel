using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POM.Helper;

namespace POM.Base
{
    public class CheckBoxGroupElement
    {
        private IWebElement _elementPanel;
        private IWebElement _rootElement;
        private By _childCheckBoxElement;

        public CheckBoxGroupElement(IWebElement elementPanel, IWebElement rootElement)
        {
            _elementPanel = elementPanel;
            _rootElement = rootElement;
        }

        public CheckBoxGroupElement(IWebElement elementPanel, IWebElement rootElement, By childCheckBoxElement)
        {
            _elementPanel = elementPanel;
            _rootElement = rootElement;
            _childCheckBoxElement = childCheckBoxElement;
        }

        public CheckBoxElement this[string title]
        {
            get
            {
                CheckBoxElement element = null;
                if (title.ToLower().Equals("all"))
                {
                    element = new CheckBoxElement(_rootElement);
                }
                else
                {
                    element = CheckBoxItems.FirstOrDefault(x => x.Title.Equals(title));
                }

                return element;
            }
        }

        public IList<CheckBoxElement> CheckBoxItems
        {
            get
            {
                IList<CheckBoxElement> lstCheckBoxItem = new List<CheckBoxElement>();

                _elementPanel.ScrollToByAction();
                IReadOnlyCollection<IWebElement> checkBoxes;
                if (_childCheckBoxElement != null)
                {
                    checkBoxes = _elementPanel.FindElements(_childCheckBoxElement);
                }
                else
                {
                    checkBoxes = _elementPanel.FindElements(By.CssSelector(".checkboxlist-column"));
                }

                // Mapping to object
                foreach (var chkItem in checkBoxes)
                {
                    lstCheckBoxItem.Add(new CheckBoxElement(chkItem));
                }

                return lstCheckBoxItem;
            }
        }

        public bool Select
        {
            set { _rootElement.Set(value); }
        }
    }
}
