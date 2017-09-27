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
    public class AssessmentPeriodLookupDetailsScreen : BaseSeleniumComponents
    {
        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        public AssessmentPeriodLookupDetailsScreen()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='Create_button']")]
        private IWebElement AssessmentPeriodNameCreateButton;

        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public AssessmentPeriodLookupDetailsScreen AssessmentPeriodNameCreateClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AssessmentPeriodNameCreateButton));
            AssessmentPeriodNameCreateButton.Click();
            
            return new AssessmentPeriodLookupDetailsScreen();
        }
 
    }
}
