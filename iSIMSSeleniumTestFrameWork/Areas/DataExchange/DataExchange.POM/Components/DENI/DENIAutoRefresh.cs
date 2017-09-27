using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.CRUD;
using TestSettings;
using WebDriverRunner.webdriver;
using DataExchange.POM.Components.Common;
using DataExchange.POM.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataExchange.POM.Components.DENI
{
    public class StatutoryReturnPageObject : AutoRefreshSeleniumComponents
    {
        public StatutoryReturnPageObject()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1000));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        /// <summary>
        /// creates a deni return for the year 2015
        /// </summary>
        public void CreateDeni()
        {
            WaitUntilDisplayed(SeleniumHelper.SelectByDataAutomationID("create_button"));
            ClickButton(SeleniumHelper.SelectByDataAutomationID("create_button"));
            IWebElement dialogSelector = SeleniumHelper.Get(DataExchangeElements.CommonElements.DialogSelector);
            WaitUntilDisplayed(DataExchangeElements.Deni.VersionSelector);
            dialogSelector.ChooseSelectorOption(DataExchangeElements.Deni.VersionSelector, "2015");
            ClickButton(DataExchangeElements.Deni.CreateReturnOkButton);
        }

        private void ClickButton(By element)
        {
            //WaitUntilEnabled(element);
            SeleniumHelper.WaitForElementClickableThenClick(element);
        }

        /// <summary>
        /// checks if the screen has reloaded and returns the message
        /// </summary>
        /// <param name="isfinalized"></param>
        /// <returns></returns>
        public string CheckAfterRefresh(bool isfinalized = false)
        {
            By loc = SeleniumHelper.SelectByDataAutomationID("section_menu_Basic Parameters");
            //By loc = isfinalized ? DeniConstants.DetailedScreenFinaliseLoaded : Constants.DetailedScreenLoaded;
            WaitUntilDisplayed(loc);
            WebContext.Screenshot();
            return isfinalized ? SeleniumHelper.Get(DeniConstants.DetailedScreenFinaliseLoaded).Text : SeleniumHelper.Get(Constants.DetailedScreenLoaded).Text;
        }

        /// <summary>
        /// search the results for the deni version
        /// </summary>
        /// <param name="deniversion"></param>
        public void SearchForResults(string deniversion)
        {
            IWebElement searchButton = SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID("search_criteria_submit"));
            Assert.IsNotNull(searchButton);
            searchButton.Click();
        }

        /// <summary>
        /// selects the deni return details from the search result lists
        /// </summary>
        /// <param name="status"></param>
        public IWebElement GetResultElement(string status)
        {
            SearchResults.WaitForResults();
            ReadOnlyCollection<IWebElement> searchResultCollection = SearchResults.GetSearchResults();
            foreach (IWebElement element in searchResultCollection)
            {
                ReadOnlyCollection<IWebElement> resultTiles = element.FindElements(By.CssSelector(string.Format("{0} {1}",
                    SeleniumHelper.AutomationId("resultTile"), "span")));
                foreach (IWebElement resultTile in resultTiles)
                {
                        if (resultTile.Text == status)
                        {
                            return element;
                        }
                }
            }
            return null;
        }

        /// <summary>
        /// validates the return
        /// </summary>
        public void ValidateReturn()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, SeleniumHelper.SelectByDataAutomationID("ActivateCustomBehaviourButton-Validate"));
            WebContext.Screenshot();
        }

        /// <summary>
        /// finalizes the return
        /// </summary>
        public void FinaliseReturn()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, DeniConstants.FinaliseButton);
            WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CommonElements.ConfirmationButton);
            WebContext.Screenshot();
        }

        /// <summary>
        /// sign off the return
        /// </summary>
        public void SignOffReturn()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, DeniConstants.SignOffButton);
            WebContext.Screenshot();
        }
    }
}
