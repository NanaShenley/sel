using Facilities.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace Facilities.Components.Facilities_Pages
{
    public class HomePageWidgetPage: BaseSeleniumComponents
    {
    #pragma warning disable 0649
        [FindsBy(How = How.CssSelector, Using = ("button[data-automation-id='StaffAbsenceWidget']"))]
        private readonly IWebElement _Widget;

        public HomePageWidgetPage()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public bool IsDisplayedWidget()
        {
            
            bool IsDisplayedWidget = _Widget.Displayed;
            return IsDisplayedWidget;
        }

    }
}
