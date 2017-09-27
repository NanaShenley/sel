using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Interactions;
using POM.Helper;

namespace Attendance.Components.AttendancePages
{
    class Tree : BaseSeleniumComponents
    {
        private readonly IWebElement _header;
        private readonly IWebElement _headerCheckbox;
        private readonly IReadOnlyCollection<IWebElement> _leaves;

        public Tree(IWebElement container)
        {
            _header = container.FindElement(By.CssSelector("a[data-parent]"));
            _headerCheckbox = container.FindElement(By.CssSelector("input"));
            var id = _header.GetAttribute("data-parent");
            _leaves = container.FindElements(By.CssSelector("label[class='checkboxlist-checkbox']"));
        }

        public bool IsExpanded()
        {
            if (_leaves.Count == 0)
            {
                throw new Exception("tree without leaves");
            }

            return _leaves.First().Displayed;
        }

        public override string ToString()
        {
            return "tree (" + _header.Text + "), with " + _leaves.Count + " children" + IsExpanded();
        }

        public string GetParentText()
        {
            return _header.Text;
        }

        public void SelectParent(){
            Actions action = new Actions(WebContext.WebDriver);
            action.MoveToElement(_headerCheckbox).Click().Perform();
           //_headerCheckbox.Click();
        }

        public void Select(string leaf)
        {
            foreach (var element in _leaves)
            {
                if (element.Text.Equals(leaf))
                {
                    Retry.Do(element.Click);
                    return;
                }
            }
            throw new Exception("couldn t find leaf " + leaf);
        }

        public void Expand()
        {
            _header.Click();
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            waiter.Until<Boolean>(d =>
            {
                return IsExpanded();
            });
        }
    }
}