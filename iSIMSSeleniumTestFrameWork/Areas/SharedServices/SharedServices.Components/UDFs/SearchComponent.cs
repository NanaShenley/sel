using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using System;
using System.Linq;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.UDFs
{
    public class SearchComponent
    {
        private const string SearchButton = "[data-automation-id='search_criteria_submit']";
        private const string SearchCriteriaNameTextbox = "input[name='Name']";
        private const string SearchCriteriaScreenSelector = "input[name=\"Screens.dropdownImitator\"]";
        private const string SearchCriteriaIsActiveCheckbox = "input[name='IsActive']";
        private const string SearchResultsSection = "[data-automation-id='search_results']";

        [FindsBy(How = How.CssSelector, Using = SearchButton)]
        private IWebElement _searchButtonElement;

        [FindsBy(How = How.CssSelector, Using = SearchCriteriaNameTextbox)]
        private IWebElement _nameElement;

        [FindsBy(How = How.CssSelector, Using = SearchCriteriaScreenSelector)]
        private IWebElement _screenElement;

        [FindsBy(How = How.CssSelector, Using = SearchCriteriaIsActiveCheckbox)]
        private IWebElement _isActiveElement;

        [FindsBy(How = How.CssSelector, Using = SearchResultsSection)]
        private IWebElement _searchResults;

        public void Search(string name = null, string screen = null, bool? includeInactive = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _nameElement.SetText(name);
            }

            if (!string.IsNullOrEmpty(screen))
            {
                _screenElement.ChooseSelectorOption(screen);
            }

            if (includeInactive.HasValue)
            {
                _isActiveElement.SetCheckBox(includeInactive.Value);
            }

            _searchButtonElement.Click();
            AutomationSugar.WaitForAjaxCompletion();
        }
        public bool HasSearchResultFor(string udfDefinition)
        {
            return SearchResults.GetSearchResults().Where(element => element.Text.Contains(udfDefinition)).Any();
        }

        public void Select()
        {
            _searchResults.Click();
            AutomationSugar.WaitForAjaxCompletion();
        }
    }
}
