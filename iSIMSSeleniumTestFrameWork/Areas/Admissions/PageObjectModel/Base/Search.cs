using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace PageObjectModel.Base
{
    public class SearchCriteriaComponent<TResultTile> : BaseComponent where TResultTile : SearchResultTileBase, new()
    {
        private readonly BaseComponent _parent;
        public override By ComponentIdentifier
        {
	        get
	        {
		        SeleniumHelper.FindElement(By.CssSelector("[data-automation-id='search_criteria']"));
		        return SimsBy.AutomationId("search_criteria");
	        }
        }

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement _searchButton;

        public SearchResultsComponent<TResultTile> Search()
        {
            Wait.WaitUntilEnabled(By.CssSelector("button[data-automation-id='search_criteria_submit']"));
            Thread.Sleep(2000);
            Retry.Do(_searchButton.Click);
            SearchResultsComponent<TResultTile> results = null;
            Thread.Sleep(500);
            //Wait.WaitUntilEnabled(By.CssSelector("[data-automation-id='search_criteria_submit']"));
            Wait.WaitForAjaxReady(By.CssSelector("button[data-automation-id='search_criteria_submit'][disabled='disabled']"));
            // Retry.Do(() =>
            //{
            results = new SearchResultsComponent<TResultTile>(_parent);
            // });

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
            Retry.Do(Tile.Click);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Wait.WaitForDocumentReady();
            var detail = default(TDetail);

            Retry.Do(() =>
            {
                detail = new TDetail();
            });

            //Thread.Sleep(2000);
            detail.Refresh();
            return detail;
        }

        public void Click()
        {
            Tile.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Wait.WaitForDocumentReady();
        }

    }

    public class SearchResultsComponent<TResultTile> : BaseComponent where TResultTile : SearchResultTileBase, new()
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

        public IList<TResultTile> ToList()
        {
            return _results;
        }

        public int Count()
        {
            return _results.Count;
        }

        public TResultTile Single()
        {
            return _results.Single();
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
            get { return SimsBy.AutomationId("search_results"); }
        }

        public SearchResultsComponent(BaseComponent parent)
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
