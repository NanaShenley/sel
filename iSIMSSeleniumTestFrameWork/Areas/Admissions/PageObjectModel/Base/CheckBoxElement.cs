using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageObjectModel.Helper;

namespace PageObjectModel.Base
{
    public class CheckBoxElement
    {
        private IWebElement _checkBoxElement;
        private string _title;

        public CheckBoxElement(IWebElement element)
        {
            _checkBoxElement = element;
        }

        public string Title
        {
            get { return _checkBoxElement.Text.Trim(); }
        }

        public bool Select
        {
            set
            {
                try
                {
                    _checkBoxElement.FindElement(By.CssSelector("input")).Set(value);
                }
                catch (Exception)
                {
                    _checkBoxElement.Set(value);
                }
            }
            get { return _checkBoxElement.IsChecked(); }
        }
    }
}
