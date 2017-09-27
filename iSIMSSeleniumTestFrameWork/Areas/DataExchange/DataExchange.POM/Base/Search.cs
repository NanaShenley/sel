using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DataExchange.POM.Helper;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using WebDriverRunner.webdriver;
using SimsBy = SeSugar.Automation.SimsBy;

namespace DataExchange.POM.Base
{
    public class SearchCriteriaComponent<TResultTile> : BaseComponent where TResultTile : SearchResultTileBase, new()
    {
        private readonly BaseComponent _parent;
        public override By ComponentIdentifier
        {
            get { return SeSugar.Automation.SimsBy.AutomationId("search_criteria"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        private IWebElement _searchButton;

        public SearchResultsComponent<TResultTile> Search()
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(100));

            wait.Until(ExpectedConditions.ElementToBeClickable(SimsBy.AutomationId("search_criteria_submit")));

            WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='search_criteria_submit']")).SendKeys(Keys.Enter);

            //Let's wait till ajax request is completed
            Wait.WaitTillAllAjaxCallsComplete();

            Thread.Sleep(500);
            SearchResultsComponent<TResultTile> results = new SearchResultsComponent<TResultTile>(_parent);
            
            return results;
        }

        public SearchCriteriaComponent(BaseComponent parent)
            : base(parent)
        {
            _parent = parent;
        }
    }

    public abstract class SearchResultTileBase
    {
        public IWebElement Tile { get; set; }

        public TDetail Click<TDetail>() where TDetail : BaseComponent, new()
        {
            Thread.Sleep(200);
            SeSugar.Automation.Retry.Do(Tile.Click);
            Thread.Sleep(500);
            return new TDetail();
        }

        public void Click()
        {
            Tile.Click();
            Thread.Sleep(500);
            AutomationSugar.WaitForAjaxCompletion();
            //Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Wait.WaitForDocumentReady();
        }

    }

    public class SearchResultsComponent<TResultTile> : BaseComponent where TResultTile : SearchResultTileBase, new()
    {
        private readonly List<TResultTile> _results = new List<TResultTile>();

        public IList<TResultTile> Where(Func<TResultTile, bool> predicate)
        {
            List<TResultTile> results  = _results.Where(predicate).ToList();

            return results;
        }

        public IList<TResultTile> ToList()
        {
            return _results;
        }

        public int Count()
        {
            return _results.Count;
        }

        public TResultTile Single(Func<TResultTile, bool> predicate)
        {
            var results = this.Where(predicate);
            return results.Single();
        }

        public IList<TResultTile> All(Func<TResultTile, bool> predicate)
        {
            var results = this.Where(predicate);
            return results;
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

        public TResultTile FirstOrDefault()
        {
            return _results.FirstOrDefault();
        }

        public TResultTile this[int index]
        {
            get
            {
                var result = default(TResultTile);
                try
                {
                    result = _results[index];
                    result.Tile.ScrollToByAction();
                }
                catch
                {
                    SeSugar.Automation.Retry.Do(() =>
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
            get { return SeSugar.Automation.SimsBy.AutomationId("search_results"); }
        }

        public SearchResultsComponent(BaseComponent parent)
            : base(parent)
        {
            Initialise();
        }


        private void Initialise()
        {
            const string resultTileIdentifier = "search_result";

            var results = this.Component.FindElements(SeSugar.Automation.SimsBy.AutomationId(resultTileIdentifier));

            foreach (var result in results)
            {
                var resultComponent = new TResultTile { Tile = result };

                PageFactory.InitElements(resultComponent, new ElementLocator(result));
                _results.Add(resultComponent);
            }
        }
    }
}
