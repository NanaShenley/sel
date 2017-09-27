using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using System;
using WebDriverRunner.webdriver;
using SharedComponents.Helpers;
using System.IO;
using Staff.Components.StaffRecord.Enumerations;

namespace Staff.Components.StaffRecord
{
    public class StaffSearchBase : BaseSeleniumComponents
    {

        #region Const

        private const string _createButtonBy = "a[title=\"Create the Record\"]";
        private const string _searchButton = "[data-automation-id=\"search_criteria_submit\"]";
        private const string _currentCriterionCheckbox = "[name=\"tri_chkbox_StatusCurrentCriterion\"]";
        private const string _futureCriterionCheckbox = "[name=\"tri_chkbox_StatusFutureCriterion\"]";
        private const string _FormerCriterionCheckbox = "[name=\"tri_chkbox_StatusFormerCriterion\"]";
        private const string _searchResults = "[data-automation-id=\"search_results\"]";
        private const string _searchCriteriaAdvanced = "[data-automation-id=\"search_criteria_advanced\"]";
        #endregion

        public static readonly By SearchCriteriaAdvanced = By.CssSelector("[data-automation-id=\"search_criteria_advanced\"]");

        #region Actions
        public StaffSearchBase()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void Create()
        {
            By loc = By.CssSelector(_createButtonBy);
            WaitForElement(loc);
            CreateButton.Click();
        }

        public void SearchCriteriaShowMore()
        {
            try
            {

            By loc = By.CssSelector(_searchCriteriaAdvanced);
            WaitForAndClick(new TimeSpan(0, 0, 10), loc);
                
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public void ClickSearchButton()
        {
            By loc = By.CssSelector(_searchButton);
            WaitForAndClick(new TimeSpan(0, 0, 10), loc);
        }

        public IWebElement StaffSearchTile(string staffId, TimeSpan timeout)
        {
            try
            {
                By loc = By.CssSelector(String.Format("div[data-automation-id=\"search_result_{0}\"]", staffId.Replace('-', '_')));
                return WaitForAndGet(timeout, loc);

            }
            catch (WebDriverException ex)
            {
                return null;
            }
        }

        public IWebElement SetCurrentCriterionCheckbox(CheckboxState state)
        {
            return SeleniumHelper.SetCheckBox(CurrentCriterionCheckBox, (int)state == 1);
        }

        public IWebElement SetFutureCriterionCheckbox(CheckboxState state)
        {
            return SeleniumHelper.SetCheckBox(FutureCriterionCheckBox, (int)state == 1);
        }

        public IWebElement SetFormerCriterionCheckbox(CheckboxState state)
        {
            return SeleniumHelper.SetCheckBox(FormerCriterionCheckBox, (int)state == 1);
        }
        #endregion

        #region Controls
        [FindsBy(How = How.CssSelector, Using = _currentCriterionCheckbox)]
        public IWebElement CurrentCriterionCheckBox;

        [FindsBy(How = How.CssSelector, Using = _futureCriterionCheckbox)]
        public IWebElement FutureCriterionCheckBox;

        [FindsBy(How = How.CssSelector, Using = _FormerCriterionCheckbox)]
        public IWebElement FormerCriterionCheckBox;

        [FindsBy(How = How.CssSelector, Using = _createButtonBy)]
        public IWebElement CreateButton;

        public IWebElement StaffPalette { get; set; }

        [FindsBy(How = How.CssSelector, Using = _searchResults)]
        public IWebElement StaffSearchResults;

        [FindsBy(How = How.CssSelector, Using = _searchButton)]
        public IWebElement SearchButton;

        #endregion

    }
}
