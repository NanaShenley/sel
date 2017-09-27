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
    public class UnexplainedAbsencesTodayWidget
    {
        #region Properties
        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='AttendanceRecordWidget']"))]
        private readonly IWebElement _unexplainedAbsencesWidget;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='AttendanceRecordWidget'] .hp-tile-title"))]
        private readonly IWebElement _unexplainedAbsencesCount;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='AttendanceRecordWidget'] .hp-tile-desc"))]
        private readonly IWebElement _unexplainedAbsencesdescription;

        public UnexplainedAbsencesTodayWidget()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        #endregion


        #region Actions

        public bool IsDisplayedWidget()
        {

            bool IsDisplayedWidget = _unexplainedAbsencesWidget.IsElementDisplayed();
            return IsDisplayedWidget;
        }

        public void UnexplainedAbsencesWidget()
        {

            _unexplainedAbsencesWidget.Click();
        }

        #endregion
    }
}
