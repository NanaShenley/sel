using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Pupil.Components.Common;
using SharedComponents.BaseFolder;
using System;
using System.Threading;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Pupil.Components.PupilRecord
{
    public class PupilRecordSearch : BaseSeleniumComponents
    {
        /// <summary>
        /// Represents search button
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        [CacheLookup]
        private IWebElement _searchButton = null;
        
        [FindsBy(How = How.CssSelector, Using = "[name='LegalSurname']")]
        [CacheLookup]
        private IWebElement _legalName = null;

        /// <summary>
        /// Constructor implemented to initialise the elements in this page object, ie: Pupil Consents Search
        /// </summary>
        public PupilRecordSearch()
        {
            WaitForElement(PupilRecordElements.PupilRecord.Search.SearchButton);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        /// <summary>
        /// Implements automation for search click 
        /// </summary>
        /// <returns>Pupil Record Detail Page Object</returns>
        public PupilRecordDetail SearchAndSelectPupil()
        {
            _legalName.SendKeys("Chris Aaron");

            _searchButton.Click();

            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("resultTile"));

            return new PupilRecordDetail();
        }
    }
}