using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.AdditionalColumn
{
    public class AllocatePupilAdditionalColumnsDialog : BaseSeleniumComponents
    {
        [FindsBy(How = How.CssSelector, Using = Constants.SearchButtonToFind)]
        private IWebElement _searchButton;

        // ReSharper disable UnassignedField.Compiler
        [FindsBy(How = How.CssSelector, Using = SharedServicesElements.AdditionalColumns.YearGroupCssSelectorToFind)] 
        private IWebElement _yearGroupCheckBox;

      
        // ReSharper restore UnassignedField.Compiler

        public IWebElement _additionalColumnButton
        {
            get { return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID("additional_columns_button")); }
        }

        public void WaitForAdditonalColumn()
        {
            WaitUntilDisplayed(new System.TimeSpan(0, 0, 0, 10), SeleniumHelper.SelectByDataAutomationID("additional_columns_button"));
        }

        public AllocatePupilAdditionalColumnsDialog()
        {
            WaitUntilDisplayed(By.CssSelector(SharedServicesElements.AdditionalColumns.YearGroupCssSelectorToFind));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void OpenAdditionalColumnDialog()
        {
            var searchCriteria = SeleniumHelper.Get(SharedServicesElements.CommonElements.SearchCriteria);
            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });

            AutomationSugar.ClickOn(SharedServicesElements.CommonElements.SearchButton);
            AutomationSugarHelpers.WaitForAndClickOn(SharedServicesElements.CommonElements.AdditionalColumnButton);

            WaitUntilDisplayed(By.CssSelector(SharedServicesElements.AdditionalColumns.OkButton));
        }
    }
}