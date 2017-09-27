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
    public class SaveMarksheetTemplate
    {
        public SaveMarksheetTemplate()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='save_button']")]
        private IWebElement SaveButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success'] strong.inline-alert-title")]
        private IWebElement SaveSuccessMessage;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Clicks Save Button
        /// </summary>
        public SaveMarksheetTemplate ClickSaveButton()
        {
            SaveButton.Click();
            while (true)
            {
                if (SaveButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new SaveMarksheetTemplate();
        }

        /// <summary>
        /// Returns a New Page Object for Save
        /// </summary>
        public SaveMarksheetTemplate SaveMarksheetTemplatePageObject()
        {
            return new SaveMarksheetTemplate();
        }

        /// <summary>
        /// Returns the Success Message displayed on screen on save success
        /// </summary>
        public string GetSaveSuccessMessage()
        {
            Thread.Sleep(2000);
            return SaveSuccessMessage.Text;
        }
    }
}
