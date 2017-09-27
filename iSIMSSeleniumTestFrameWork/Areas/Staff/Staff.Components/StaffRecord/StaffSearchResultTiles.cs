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
    public class StaffSearchResultTiles : BaseSeleniumComponents
    {

        #region Const
        private const string _currentCriterionCheckbox = "[name=\"tri_chkbox_StatusCurrentCriterion\"]";
        private const string _futureCriterionCheckbox = "[name=\"tri_chkbox_StatusFutureCriterion\"]";
        private const string _FormerCriterionCheckbox = "[name=\"tri_chkbox_StatusFormerCriterion\"]";
        private const string _searchResults = "[data-automation-id=\"search_results\"]";
        #endregion

        #region Actions
        public StaffSearchResultTiles()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public IWebElement StaffSearchTile(string staffId)
        {
            staffId = staffId.Replace('-', '_');
            By loc = By.CssSelector(String.Format("div[data-automation-id=\"search_result_{0}\"]", staffId));
            WaitForElement(loc);
            return WebContext.WebDriver.FindElement(loc);
        }

        public IWebElement StaffSearchTile(string staffId, TimeSpan timeout)
        {
            try
            {
                staffId = staffId.Replace('-', '_');
                By loc = By.CssSelector(String.Format("div[data-automation-id=\"search_result_{0}\"]", staffId));
                return WaitForAndGet(timeout, loc);

            }
            catch (WebDriverException ex)
            {                
                return null;
            }
        }

        public StaffDetail SelectStaff(string staffId)
        {
            StaffSearchTile(staffId).Click();
            return new StaffDetail();
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

        [FindsBy(How = How.CssSelector, Using = _searchResults)]
        public IWebElement StaffSearchResults;

        public int GetStaffSearchResultsCount()
        {
            return StaffSearchResults.Text.Split('\r').Length - 1;
        }
        
        #endregion
    }
}
