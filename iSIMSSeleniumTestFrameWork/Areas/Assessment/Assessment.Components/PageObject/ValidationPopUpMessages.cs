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
    public class ValidationPopUpMessages
    {
        public ValidationPopUpMessages()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='ColumnDetailsPopover'] button[type='button']")]
        private readonly IWebElement PopUpWindowCloseButton = null;

        [FindsBy(How = How.CssSelector, Using = "div.validation-summary-errors li[message-custom]")]
        private readonly IWebElement InlineValidationMessageText = null;

        private static By PopUpValidationMessageText = By.CssSelector("div.popover-content");

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Returns a New Page Object for Validation PopUp Messages
        /// </summary>
        public ValidationPopUpMessages ValidationPopUpMessagePageObject()
        {
            return new ValidationPopUpMessages();
        }

        /// <summary>
        /// This method returns a Validation Message that is shown on the UI
        /// </summary>
        public string GetPopUpValidationMessageText()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(PopUpValidationMessageText));
            return WebContext.WebDriver.FindElement(PopUpValidationMessageText).Text;
        }

        /// <summary>
        /// This method returns a Validation Message that is shown on the UI
        /// </summary>
        public string GetInlineValidationMessageText()
        {
            return InlineValidationMessageText.Text;
        }

         ///    <summary>
         ///    Closes the Pop Up Message
         ///    </summary>
        public void ClosePopUpMessageWindow()
        {
            PopUpWindowCloseButton.Click();
        }
    }
}
