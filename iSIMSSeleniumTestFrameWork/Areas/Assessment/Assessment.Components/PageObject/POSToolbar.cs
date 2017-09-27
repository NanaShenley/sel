using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace Assessment.Components.PageObject
{
    public class POSToolbar: BaseSeleniumComponents
    {
        public POSToolbar()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='save_button']")]
        private IWebElement SaveButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='toggleOption']")]
        public IWebElement ToggleButton;

        [FindsBy(How = How.CssSelector, Using = "input[data-toggle-filter='Summativecolumns']")]
        private IWebElement SummativecolumnsToggleOption;


        [FindsBy(How = How.CssSelector, Using = "input[data-toggle-filter='HideStrengthNextStepColumns']")]
        private IWebElement HideStrengthNextStepsToggleOption;

        [FindsBy(How = How.CssSelector, Using = "input[data-toggle-filter='SubjectSummativecolumns']")]
        private IWebElement SubjectSummativecolumnsToggleOption;

        [FindsBy(How = How.CssSelector, Using = "input[data-toggle-filter='Nextyearsstatements']")]
        private IWebElement NextYearStatementsToggleOption;

        [FindsBy(How = How.CssSelector, Using = "input[data-toggle-filter='Previousyearsstatements']")]
        private IWebElement PreviousYearStatementsToggleOption;

        [FindsBy(How = How.CssSelector, Using = "input[data-toggle-filter='Summarycolumns']")]
        private IWebElement SummarycolumnsToggleOption;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        By POSExpectationAchievedColumn = By.CssSelector("div[id='cOverallPos']");
        private static By saveSuccessMessage_New = By.CssSelector("div[data-automation-id='status_success']");

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success']")]
        private IWebElement SaveSuccessMessage;

        public static readonly By VerticalOrientation = By.CssSelector("[data-automation-id='Vertical']");
        public static readonly By HorizontalOrientation = By.CssSelector("[data-automation-id='Horizontal']");


        /// <summary>
        /// clicks the Save button on the data maintenance screen
        /// </summary>
        public void ClickSaveButton()
        {
            SaveButton.Click();
        }

        /// <summary>
        /// save the POS marksheet.         
        /// </summary>
        /// <returns></returns>
        public bool SaveMarksheetAssertionSuccess()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));
            return waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "POS marksheet saved"));
           
        }

        public void waitforSavemessagetoAppear()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until<Boolean>(d =>
            {
                IWebElement saveMessage = WebContext.WebDriver.FindElement(By.CssSelector("[class='inline-alert-title']"));
                if (saveMessage.Displayed)
                {
                    return true;
                }
                return false;
            }
            );
        }

        /// <summary>
        /// Clicks on the Toggle Option to open the Toggle Menu List
        /// </summary>
        public POSToolbar OpenToggleMenuList()
        {
            Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(ToggleButton));
            if (ToggleButton.GetAttribute("aria-expanded") == null || ToggleButton.GetAttribute("aria-expanded") == "false")
            {
                ToggleButton.Click();
                while (true)
                {
                    if (ToggleButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
            }
            return new POSToolbar();
        }

        /// <summary>
        /// Clicks on the Summative Column Toggle Option on the Toggle Menu List
        /// </summary>
        public POSDataMaintainanceScreen ClickSummativecolumnsToggleOption(bool show)
        {
            if ((SummativecolumnsToggleOption.GetAttribute("checked") == null && show) || ((SummativecolumnsToggleOption.GetAttribute("checked") == "true" && !show)))
                waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("label[for='" + SummativecolumnsToggleOption.GetAttribute("id") + "']"))).Click();
            WaitUntillAjaxRequestCompleted();
            return new POSDataMaintainanceScreen();
        }

        /// <summary>
        /// Clicks on the Hide Strength and Next Steps Toggle Option on the Toggle Menu List
        /// </summary>
        public POSDataMaintainanceScreen ClickHideStrengthNextStepsToggleOption(bool show)
        {
            if ((HideStrengthNextStepsToggleOption.GetAttribute("checked") == null && show) || ((HideStrengthNextStepsToggleOption.GetAttribute("checked") == "true" && !show)))
                waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("label[for='" + HideStrengthNextStepsToggleOption.GetAttribute("id") + "']"))).Click();
            Thread.Sleep(2000);
            return new POSDataMaintainanceScreen();
        }

        /// <summary>
        /// Clicks on the Summative Column Toggle Option on the Toggle Menu List
        /// </summary>
        public POSDataMaintainanceScreen ClickSubjectSummativecolumnsToggleOption(bool show)
        {
            if ((SubjectSummativecolumnsToggleOption.GetAttribute("checked") == null && show) || ((SubjectSummativecolumnsToggleOption.GetAttribute("checked") == "true" && !show)))
                waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("label[for='" + SubjectSummativecolumnsToggleOption.GetAttribute("id") + "']"))).Click();
            WaitUntillAjaxRequestCompleted();
            return new POSDataMaintainanceScreen();
        }

        /// <summary>
        /// Clicks on the Summary Column Toggle Option on the Toggle Menu List
        /// </summary>
        public POSDataMaintainanceScreen ClickSummarycolumnsToggleOption(bool show)
        {
            if ((SummarycolumnsToggleOption.GetAttribute("checked") == null && show) || ((SummarycolumnsToggleOption.GetAttribute("checked") == "true" && !show)))
               waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("label[for='" + SummarycolumnsToggleOption.GetAttribute("id") + "']"))).Click();
            WaitUntillAjaxRequestCompleted();
            return new POSDataMaintainanceScreen();
        }

        /// <summary>
        /// Clicks on the Summative Column Toggle Option on the Toggle Menu List
        /// </summary>
        public POSDataMaintainanceScreen ClickNextYearStatementsToggleOption(bool show)
        {
            if ((NextYearStatementsToggleOption.GetAttribute("checked") == null && show) || ((NextYearStatementsToggleOption.GetAttribute("checked") == "true" && !show)))
                waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("label[for='" + NextYearStatementsToggleOption.GetAttribute("id") + "']"))).Click();
            WaitUntillAjaxRequestCompleted();
            return new POSDataMaintainanceScreen();
        }
        /// <summary>
        /// Clicks on the Summative Column Toggle Option on the Toggle Menu List
        /// </summary>
        public POSDataMaintainanceScreen ClickPreviousYearStatementsToggleOption(bool show)
        {
            if ((PreviousYearStatementsToggleOption.GetAttribute("checked") == null && show) || ((PreviousYearStatementsToggleOption.GetAttribute("checked") == "true" && !show)))
                waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("label[for='" + PreviousYearStatementsToggleOption.GetAttribute("id") + "']"))).Click();
            WaitUntillAjaxRequestCompleted();
            return new POSDataMaintainanceScreen();
        }

        #region Cell Navigation Orientation
        public POSToolbar ClickPOSMarksheetCellNavigationDropdown()
        {
            var cellNavigator = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='cell_navigator']"));
            cellNavigator.Click();
            return this;
        }

        public bool CheckStateofCellNavigation()
        {
            string state = "";
            //state = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='Horizontal']")).FindChild(By.CssSelector("[name='IsVertical']")).GetAttribute("checked");
            var vertical = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='Vertical']"));
            state = vertical.FindChild(By.CssSelector("[name='Orientation']")).GetAttribute("checked");
            return Convert.ToBoolean(state);
        }

        public POSToolbar ClickHorizontalOrientation()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(HorizontalOrientation));
            WebContext.WebDriver.FindElement(HorizontalOrientation).Click();
            return this;

        }

        public POSToolbar ClickVerticalOrientation()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(VerticalOrientation));
            WebContext.WebDriver.FindElement(VerticalOrientation).Click();
            return this;
        }

        #endregion


    }
}
