using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using System;
using SharedServices.Components.Common;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.PageObjects
{
    public class LookupPageObject
    {
        #region Automation IDs
        private const string SuccessMessageBanner = "status_success";
        private const string ErrorMessageBanner = "status_error";
        private const string SearchButton = "search_criteria_submit";
        private const string SaveButton = "well_know_action_save";
        #endregion

        public LookupPageObject()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1000));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        #region Page Elements
        private IWebElement successMessageBanner
        {
            get
            {
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(SuccessMessageBanner));
            }
        }

        private IWebElement errorMessageBanner
        {
            get
            {
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(ErrorMessageBanner));
            }
        }

        #endregion

        #region public Methods

        /// <summary>
        /// Finds and clicks on search button
        /// </summary>
        public void ClickSearch()
        {
            AutomationSugarHelpers.WaitForAndClickOn(SearchButton);
        }

        /// <summary>
        /// Clicks Create Button
        /// </summary>
        public void ClickCreateButton(string createButtonId)
        {
            AutomationSugarHelpers.WaitForAndClickOn(createButtonId);
        }

        /// <summary>
        /// Sets the value of the supplied element
        /// </summary>
        /// <param name="element">The element to set the value of</param>
        /// <param name="cssSelector">The css selector of the element to find</param>/// 
        /// <param name="value">The value to set it as</param>
        public void SetElementValue(IWebElement element, string cssSelector, string value)
        {
            var fieldElement = element.FindElement(By.CssSelector(cssSelector));
            Assert.IsNotNull(fieldElement);
            SetTextToElement(fieldElement, value);            
        }

        /// <summary>
        /// Clicks on Save Button
        /// </summary>
        public void ClickSaveButton()
        {
            AutomationSugarHelpers.WaitForAndClickOn(SaveButton);
        }

        public IWebElement GetMessage(string messageToGet)
        {
            return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(messageToGet));
        }

        /// <summary>
        /// Gets the success message banner
        /// </summary>
        /// <returns></returns>
        public IWebElement GetSuccessMessage()
        {
            return successMessageBanner;
        }

        /// <summary>
        /// Gets the error message banner
        /// </summary>
        /// <returns></returns>
        public IWebElement GetErrorMessage()
        {
            return errorMessageBanner;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Sets text to element
        /// </summary>
        /// <param name="fieldElement"></param>
        /// <param name="value"></param>
        private static void SetTextToElement(IWebElement fieldElement, string value)
        {
            fieldElement.SetText(value);
        }

        #endregion

        public IWebElement GetGridElement(string gridID)
        {
            return SeleniumHelper.Get(By.CssSelector(string.Format("table[data-maintenance-grid-id='{0}']", gridID)));
        }
    }
}
