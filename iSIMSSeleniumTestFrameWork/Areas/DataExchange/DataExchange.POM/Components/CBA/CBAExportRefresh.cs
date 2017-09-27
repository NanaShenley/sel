using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using TestSettings;
using WebDriverRunner.webdriver;
using DataExchange.POM.Components.Common;
using DataExchange.POM.Helper;

namespace DataExchange.POM.Components.CBA
{
    public class CbaExportRefresh : AutoRefreshSeleniumComponents
    {
        public static readonly int Timeout = 30;
        private IWebElement searchButton
        {
            get
            {
                WaitUntilEnabled(By.CssSelector(DataExchangeElements.CbaExport.SearchButtonToFind));
                return SeleniumHelper.Get(By.CssSelector(DataExchangeElements.CbaExport.SearchButtonToFind));
            }
        }

        [FindsBy(How = How.CssSelector, Using = DataExchangeElements.CbaExport.CreateButtonToFind)]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = DataExchangeElements.CbaExport.GenerateButtonToFind)]
        private IWebElement _generateButton;

        public CbaExportRefresh()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        /// <summary>
        /// method to export the CBA file
        /// </summary>
        public void GenerateCbaExport()
        {
            //WaitElementToBeClickable(By.CssSelector(DataExchangeElements.CbaExport.SearchButtonToFind));
            searchButton.Click();
            searchButton.Click();

            //wait until the result is loaded and has text
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(Timeout));
            waiter.Until(ExpectedConditions.ElementToBeClickable(DataExchangeElements.CbaExport.Result));
            WaitUntilEnabled(DataExchangeElements.CbaExport.Result);
            SeleniumHelper.WaitForElementClickableThenClick(DataExchangeElements.CbaExport.Result);
            
            waiter.Until(ExpectedConditions.ElementIsVisible(DataExchangeElements.CbaExport.ResultCba));

            if (SeleniumHelper.Get(DataExchangeElements.CbaExport.ResultCba).Text != "No Matches")
            {
                //if (SeleniumHelper.Get(DataExchangeElements.CbaExport.CbaStatus).Text == "Pending with Errors")
                {
                    WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CbaExport.SelectCba);
                    //wait till the generate button is clickable and then click it
                    WaitUntilDisplayed(DataExchangeElements.CommonElements.PupilGridCheckBox);
                    WaitElementToBeClickable(DataExchangeElements.CbaExport.GenerateButton);
                    AutomationSugarHelpers.WaitForAndClickOn(DataExchangeElements.CbaExport.GenerateButton);
                    //WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(DataExchangeElements.CbaExport.GenerateButtonToFind));
                }
            }
            else
            {
                _createButton.Click();
                WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CbaExport.AddPupilLink);
                WaitUntilDisplayed(DataExchangeElements.CbaExport.PrimaryClassToFind);
                WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CbaExport.PrimaryClassToFind);
                WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CbaExport.UploadCbaSearchButton);
                
                WaitUntilEnabled(DataExchangeElements.CbaExport.SearchRecordsToFindtext);
                SeleniumHelper.WaitForElementClickableThenClick(DataExchangeElements.CbaExport.SearchRecordsToFindtext);

                WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CbaExport.AddSelectedButton);
                WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CbaExport.OkButton);

                //wait till the generate button is clickable and then click it
                WaitUntilDisplayed(DataExchangeElements.CommonElements.PupilGridCheckBox);
                WaitElementToBeClickable(DataExchangeElements.CbaExport.GenerateButton);
                WaitElementToBeClickable(DataExchangeElements.CbaExport.GenerateButton);
                AutomationSugarHelpers.WaitForAndClickOn(DataExchangeElements.CbaExport.GenerateButton);
            }
        }
    }
}
