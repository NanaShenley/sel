using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using SeSugar.Interfaces;
using WebDriverRunner.webdriver;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;


namespace POM.Base
{
    public class SearchListCriteriaComponent<TResultTile> : BaseComponent where TResultTile : SearchResultTileBase, new()
    {
        private readonly BaseComponent _parent;
        private static readonly ILogger _logger = SeSugar.Environment.Logger;

        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("search_criteria"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        private IWebElement _searchButton;

        public SearchListResultsComponent<TResultTile> Search()
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(10));
            Stopwatch sw = new Stopwatch();

            sw.Start();
            AutomationSugar.ClickOn(new ByChained(this._parent.ComponentIdentifier, SimsBy.AutomationId("search_criteria_submit")));
            _logger.LogLine("Waiting and clicking on search button took {0}ms.", sw.ElapsedMilliseconds);
            wait.Until(ExpectedConditions.ElementIsVisible(new ByChained(this._parent.ComponentIdentifier, SimsBy.AutomationId("resultTile"))));
            _logger.LogLine("Waiting search results took {0}ms.", sw.ElapsedMilliseconds);
            SearchListResultsComponent<TResultTile> results = new SearchListResultsComponent<TResultTile>(_parent);
            _logger.LogLine("Binding seach results tool {0}ms.", sw.ElapsedMilliseconds);
            sw.Stop();
            return results;
        }

        public SearchListCriteriaComponent(BaseComponent parent)
            : base(parent)
        {
            _parent = parent;
        }
    }

    public class SearchListResultsComponent<TResultTile> : BaseComponent where TResultTile : SearchResultTileBase, new()
    {
        private readonly List<TResultTile> _results = new List<TResultTile>();

        public IList<TResultTile> Where(Func<TResultTile, bool> predicate)
        {
            List<TResultTile> results = null;

            Retry.Do(() =>
            {
                results = _results.Where(predicate).ToList();
            }, catchAction: Initialise);

            return results;
        }

        public TResultTile Single(Func<TResultTile, bool> predicate)
        {
            var results = this.Where(predicate);
            return results.Single();
        }

        public TResultTile SingleOrDefault(Func<TResultTile, bool> predicate)
        {
            var results = this.Where(predicate);
            return results.SingleOrDefault();
        }

        public TResultTile FirstOrDefault(Func<TResultTile, bool> predicate)
        {
            return _results.FirstOrDefault(predicate);
        }

        public TResultTile this[int index]
        {
            get
            {
                var result = default(TResultTile);
                try
                {
                    result = _results[index];
                }
                catch
                {
                    Retry.Do(() =>
                    {
                        Initialise();
                        result = _results[index];
                    });
                }

                return result;
            }
        }
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("search_results_list"); }
        }

        public SearchListResultsComponent(BaseComponent parent)
            : base(parent)
        {
            Retry.Do(Initialise);
        }


        private void Initialise()
        {
            const string resultTileIdentifier = "search_result";

            var results = this.Component.FindElements(SimsBy.AutomationId(resultTileIdentifier));

            foreach (var result in results)
            {
                var resultComponent = new TResultTile { Tile = result };

                PageFactory.InitElements(resultComponent, new ElementLocator(result));
                _results.Add(resultComponent);
            }
        }
    }
}
