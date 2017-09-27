using System;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;
using SharedComponents.Helpers;

namespace Assessment.Components.PageObject
{
    public class AssessmentPeriodLookupSearchPanel
    {
        public AssessmentPeriodLookupSearchPanel()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "form[data-automation-id='search_criteria'] input[name='Name']")]
        private IWebElement AssessmentPeriodNameTextField;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement SearchButton;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='AssessmentPeriods']")]
        private IWebElement AssessmentPeriodLookupGrid;

        [FindsBy(How = How.CssSelector, Using = "button[class='btn-show-panel']")]
        private IWebElement filterButton;

        [FindsBy(How = How.CssSelector, Using = "input[name='AcademicYear.dropdownImitator']")]
        private IWebElement AcademicGroupSelector;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public AssessmentPeriodLookupDataMaintainanceScreen Search()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButton));
            SearchButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (SearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new AssessmentPeriodLookupDataMaintainanceScreen();
        }

        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public AssessmentPeriodLookupDataMaintainanceScreen FilterClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(filterButton));
            filterButton.Click();            
            return new AssessmentPeriodLookupDataMaintainanceScreen();
        }

        /// <summary>
        /// Select a Strand on Gradeset Pannel
        /// </summary>
        public AssessmentPeriodLookupSearchPanel SelectAcademicYear(string AcademicYear)
        {
            SeleniumHelper.ChooseSelectorOption(AcademicGroupSelector, AcademicYear);
            return new AssessmentPeriodLookupSearchPanel();
        }


        /// <summary>
        /// New Page Object for Assessment Period Lookup Search Panel
        /// </summary>
        public AssessmentPeriodLookupSearchPanel NewAssessmentPeriodLookupSearchPanelPageObject()
        {
            return new AssessmentPeriodLookupSearchPanel();
        }

        /// <summary>
        /// It sets the given gradeset name in the Name text field
        /// </summary>
        public void SetAssessmentPeriodName(string AssessmentPeriodName)
        {
            AssessmentPeriodNameTextField.Clear();
            AssessmentPeriodNameTextField.SendKeys(AssessmentPeriodName);
        }

        public string GetSearchResultCount()
        {
            return AssessmentPeriodLookupGrid.GetAttribute("data-grid-row-count");
        }
    }
}
