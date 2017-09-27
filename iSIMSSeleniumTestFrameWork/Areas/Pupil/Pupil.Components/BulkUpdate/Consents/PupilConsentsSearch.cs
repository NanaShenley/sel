using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Pupil.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Pupil.Components.BulkUpdate.Consents
{
    /// <summary>
    /// PageObject class for bulk update pupil consents screen's search section
    /// </summary>
    public class PupilConsentsSearch : BaseSeleniumComponents
    {
        private IWebDriver _driver;

        /// <summary>
        /// Represents search button
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        [CacheLookup]
        private IWebElement _searchButton;

        /// <summary>
        /// Represents search criteria
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria']")]
        [CacheLookup]
        private IWebElement _searchCriteria;

        /// <summary>
        /// Constructor implemented to initialise the elements in this page object, ie: Pupil Consents Search
        /// </summary>
        public PupilConsentsSearch()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            Thread.Sleep(2000);

            WaitForElement(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton);
        }

        /// <summary>
        /// Select year group(s)
        /// </summary>
        /// <param name="yearGroupName"></param>
        public PupilConsentsSearch WithYearGroupsAs(string yearGroupName)
        {
            Thread.Sleep(2000);
            SeleniumHelper.FindAndClickSingleGroupInTreeList("section_menu_Year Group", yearGroupName);

            return this;
        }

        /// <summary>
        /// Select class group(s)
        /// </summary>
        /// <param name="classGroups"></param>
        public PupilConsentsSearch WithClassGroupsAs(string classGroupName)
        {
            SeleniumHelper.FindAndClickSingleGroupInTreeList("section_menu_Class", classGroupName);

            return this;
        }

        /// <summary>
        /// Implements automation for search click
        /// </summary>
        /// <returns>Pupil Consents Detail Page Object</returns>
        public PupilConsentsDetail SearchAndReturnDetail()
        {
            _searchButton.Click();

            return new PupilConsentsDetail();
        }
    }
}