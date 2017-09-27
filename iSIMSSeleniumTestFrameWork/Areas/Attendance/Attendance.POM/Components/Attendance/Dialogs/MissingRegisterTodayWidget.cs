using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POM.Helper;
using WebDriverRunner.webdriver;

namespace POM.Components.Attendance
{
    public class MissingRegisterTodayWidget 
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='MissingRegistersWidget']")]
        public IWebElement _missingRegisterWidget;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='MissingRegistersWidget'] .hp-tile-title")]
        private IWebElement _missingRegisterCount;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='MissingRegistersWidget'] .hp-tile-desc")]
        private IWebElement _missingRegisterdescription;

        public MissingRegisterTodayWidget()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public string MissingRegisterWidgetCount
        {

            get { return _missingRegisterWidget.GetValue(); }
        }

        public string MissingRegisterDescription
        {

            get { return _missingRegisterdescription.GetValue(); }
        }


        #region Actions

        public bool IsDisplayedWidget()
        {

            bool IsDisplayedWidget = _missingRegisterWidget.IsElementDisplayed();
            return IsDisplayedWidget;
        }

        public void MissingRegisterWidget()
        {
            _missingRegisterWidget.Click();
        }

        #endregion
    }
}
