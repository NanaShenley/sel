using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.PLASC
{
    public class PlascSearchPanel : SearchCriteriaComponent<PlascSearchResultTile>
    {
        public PlascSearchPanel(BaseComponent parent) : base(parent) { }

        [FindsBy(How = How.CssSelector, Using = "form[data-section-id='searchCriteria'] input[name='StatutoryReturnType.dropdownImitator']")]
        private IWebElement _returnTypeDropdown;

        [FindsBy(How = How.CssSelector, Using = "form[data-section-id='searchCriteria'] input[name='StatutoryReturnVersion.dropdownImitator']")]
        private IWebElement _returnTypeVersionDropdown;

        public string ReturnTypeDropdown
        {
            get { return _returnTypeDropdown.GetAttribute("value"); }
            set { _returnTypeDropdown.EnterForDropDown(value); }
        }


        public string ReturnTypeVersionDropdown
        {
            get { return _returnTypeVersionDropdown.GetAttribute("value"); }
            set { _returnTypeVersionDropdown.EnterForDropDown(value); }
        }

        public void ClearSearchResults()
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;

            //clear search results
            string script = "$(\"[data-automation-id = 'search_results_list']\").find(\"[data-automation-id='search_result']\").remove()";
            jsExecutor.ExecuteScript(script);

            //clear search counter
            script = "$(\"[data-automation-id = 'search_results_counter']\").empty();";
            jsExecutor.ExecuteScript(script);
        }

        public bool ClickSearchResultItemIfAny()
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;

            string script = "var searchResults = $(\"[data-automation-id = 'search_results_list']\").find(\"[data-automation-id='resultTile']\"); ";
            script += " if(searchResults.length > 0){searchResults[0].click(); return true;} else { return false;} ";
            bool isAnyResultItemExist = (Boolean)jsExecutor.ExecuteScript(script);

            return isAnyResultItemExist;
        }

    }

    public class PlascSearchResultTile : SearchResultTileBase
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
        private IWebElement _name;

        public string Name
        {
            get { return _name.Text; }
        }
    }
}
