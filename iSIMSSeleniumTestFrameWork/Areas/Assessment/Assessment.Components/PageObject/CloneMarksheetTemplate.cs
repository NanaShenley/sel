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
    public class CloneMarksheetTemplate
    {
        public CloneMarksheetTemplate()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='clone_template_button']")]
        private IWebElement CloneButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success'] strong.inline-alert-title")]
        private IWebElement CloneSuccessMessage;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Clicks on Clone Marksheet Button
        /// </summary>
        public CloneMarksheetTemplate ClickCloneButton()
        {
            CloneButton.Click();
            while (true)
            {
                if (CloneButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new CloneMarksheetTemplate();
        }

        /// <summary>
        /// Returns a New Page Object for Clone
        /// </summary>
        public CloneMarksheetTemplate CloneMarksheetTemplatePageObject()
        {
            return new CloneMarksheetTemplate();
        }

        /// <summary>
        /// Returns the Success Message displayed on screen on clone success
        /// </summary>
        public string GetCloneSuccessMessage()
        {
            Thread.Sleep(2000);
            return CloneSuccessMessage.Text;
        }
    }
}
