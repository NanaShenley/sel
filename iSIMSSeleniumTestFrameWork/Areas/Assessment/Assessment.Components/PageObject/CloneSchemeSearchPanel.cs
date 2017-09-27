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
    public class CloneSchemeSearchPanel : BaseSeleniumComponents
    {
        public CloneSchemeSearchPanel()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement SearchButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement CloneSchemePalletteSearch;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='select_scheme_button']")]
        private IWebElement SelectSchemeButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='ok_button']")]
        private IWebElement OkButton;
        
        private static By ClonedSchemeNameTextField = By.CssSelector("div[data-automation-id='Schemes-selector'] span[class='panel-title']");

        private static By schemeNameTextField = By.CssSelector("form[data-automation-id='search_criteria'] input[name='Name']");

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));


        // Search Result List
        private static By SearchResultNameList = By.CssSelector("div[data-automation-id='search_result'] a[class='search-result h1-result']");
        private static By existingSchemeDetails = By.CssSelector("div[data-section-id=dialog-detail]");
        private static By ClonedSchemeDetails = By.CssSelector("div[data-section-id=dialog-dialog-detail]");

        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public CloneSchemeSearchPanel Search()
        {
            WaitUntilDisplayed(schemeNameTextField);
            waiter.Until(ExpectedConditions.ElementToBeClickable(CloneSchemePalletteSearch));
            // Sleep has been added as Search Button is not accessable at this time.
            Thread.Sleep(3000);
            CloneSchemePalletteSearch.Click();
            while (true)
            {
                if (CloneSchemePalletteSearch.GetAttribute("disabled") != "true")
                    break;
            }

            return new CloneSchemeSearchPanel();
        }

        /// <summary>
        /// Selects a particular Gardeset based on the Gradeset Name
        /// </summary>
        public CloneSchemeSearchPanel SelectSchemeByName(string schemeName)
        {
            ReadOnlyCollection<IWebElement> SearchResults = WebContext.WebDriver.FindElements(SearchResultNameList);
            foreach (IWebElement eachelement in SearchResults)
            {
                if (eachelement.Text == schemeName)
                {
                    eachelement.Click();
                    Thread.Sleep(2000);
                    break;
                }
            }
            return new CloneSchemeSearchPanel();
        }


        /// <summary>
        /// It sets the given scheme name in the Name text field
        /// </summary>
        public void SetSchemeName(string SchemeName)
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(schemeNameTextField));
            WebContext.WebDriver.FindElement(schemeNameTextField).Clear();
            WebContext.WebDriver.FindElement(schemeNameTextField).SendKeys(SchemeName);
        }

        /// <summary>
        /// Click on Ok Button
        /// </summary>
        public CloneSchemeSearchPanel ClickSelectSchemeButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SelectSchemeButton));
            waiter.Until(ExpectedConditions.ElementIsVisible(existingSchemeDetails));
            Thread.Sleep(2000);
            SelectSchemeButton.Click();

            return new CloneSchemeSearchPanel();
        }

        public CloneSchemeSearchPanel ClickOkButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(OkButton));
            waiter.Until(ExpectedConditions.ElementIsVisible(ClonedSchemeDetails));
            Thread.Sleep(2000);
            OkButton.Click();

            return new CloneSchemeSearchPanel();
        }

        /// <summary>
        /// Click on OK Button to add new Subject
        /// </summary>
        public string GetClonedSchemeName()
        {           
            waiter.Until(ExpectedConditions.ElementIsVisible(ClonedSchemeNameTextField));
            return WebContext.WebDriver.FindElement(ClonedSchemeNameTextField).Text;
        }


    }
}
