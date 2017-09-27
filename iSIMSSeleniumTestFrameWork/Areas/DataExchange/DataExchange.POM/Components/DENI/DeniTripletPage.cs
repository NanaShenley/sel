using System;
using DataExchange.POM.Base;
using OpenQA.Selenium;
using SeSugar.Automation;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;


namespace DataExchange.POM.Components.DENI
{
    /// <summary>
    ///  Deni PAGE Page object
    /// </summary>
    public class DeniTripletPage : BaseComponent
    {
            public override By ComponentIdentifier
            {
                get
                {
                   return SimsBy.AutomationId("DENI_DETAIL_PAGE");
                }
            }
            private readonly DeniSearchPanel _deniSearchPanel;

            public DeniSearchPanel SearchCriteria { get { return _deniSearchPanel; } }

        public bool ClickSearchResultItemIfAny()
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;

            string script = "var searchResults = $(\"[data-automation-id = 'search_results_list']\").find(\"[data-automation-id='resultTile']\"); ";
            script += " if(searchResults.length > 0){searchResults[0].click(); return true;} else { return false;} ";
            bool isAnyResultItemExist = (Boolean)jsExecutor.ExecuteScript(script);

            return isAnyResultItemExist;
        }
     

        public DeniTripletPage()
            {
                _deniSearchPanel = new DeniSearchPanel(this);

            }

            /// <summary>
            /// Create Census
            /// </summary>
            /// <returns></returns>
            public CreateDeniDialog CreateDeni()
            {
                AutomationSugar.ClickOnAndWaitFor("create_button", "create_Deni_dialog");
                return new CreateDeniDialog();
            }

            /// <summary>
            /// Deni Result Tile
            /// </summary>
            public class DeniSearchResultTile : SearchResultTileBase
            {
                [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
                private IWebElement _name;

                public string Name
                {
                    get { return _name.Text; }
                }
            }




        }
    }

