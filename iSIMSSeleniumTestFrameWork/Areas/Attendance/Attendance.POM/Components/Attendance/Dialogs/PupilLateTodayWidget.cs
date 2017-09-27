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
    public class PupilLateTodayWidget 
    {

        #region Properties
        [FindsBy(How = How.CssSelector, Using = ("button[data-automation-id='PupilLateTodayWidget']"))]
        private readonly IWebElement _pupilLateWidget;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='PupilLateTodayWidget'] .hp-tile-title"))]
        private readonly IWebElement _pupilLateWidgetCount;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='PupilLateTodayWidget'] .hp-tile-desc"))]
        private readonly IWebElement _pupilLateWidgetdescription;

        public PupilLateTodayWidget()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        #endregion


        #region Actions

        public bool IsDisplayedWidget()
        {

            bool IsDisplayedWidget = _pupilLateWidget.IsElementDisplayed();
            return IsDisplayedWidget;
        }

        public void PupilLateWidget()
        {

            _pupilLateWidget.Click();
        }

        #endregion
    }
}
