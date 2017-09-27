using System.Collections.ObjectModel;
using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using SeSugar.Automation;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.CRUD;
using WebDriverRunner.webdriver;
using System;

namespace DataExchange.POM.Components.Census
{
    /// <summary>
    ///  Census PAGE Page object
    /// </summary>
    public class CensusPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get
            {
                //return Helper.SimsBy.AutomationId("create_Deni_dialog");
                return By.Id("screen");
            }
        }
        private readonly CensusSearchPanel _censusSearchPanel;
        
        public CensusSearchPanel SearchCriteria { get { return _censusSearchPanel; } }
           

        public CensusPage()
        {
            _censusSearchPanel = new CensusSearchPanel(this); 

        }

        /// <summary>
        /// Create Census
        /// </summary>
        /// <returns></returns>
        public CreateCensusDialog CreateCensus()
        {                      
            AutomationSugar.ClickOnAndWaitFor("create_button", "create_Deni_dialog");           
            return new CreateCensusDialog();
        }

        /// <summary>
        /// Census Result Tile
        /// </summary>
        public class CensusSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;
        
            public string Name
            {
                get { return _name.Text; }
            }
        }

        /// <summary>
        /// Get results only for status as "Validated with Issues"
        /// </summary>
        public IWebElement GetSearchResults(string status)
        {
            var censusTripletPage = new CensusPage();
            censusTripletPage.SearchCriteria.ReturnTypeDropdown = "School Census Return";
            censusTripletPage.SearchCriteria.ReturnTypeVersionDropdown = "Spring 2017";
            SearchCriteria.Search();
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
        /// Create Return
        /// </summary>
        public void CreateReturn()
        {
            var createCensusDialog = CreateCensus();
            createCensusDialog.ReturnTypeDropdown = "School Census Return";
            createCensusDialog.ReturnTypeVersionDropdown = "Spring 2016";                        
            createCensusDialog.OKButton();
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
}
