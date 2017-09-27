using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Threading;
using Staff.POM.Helper;
using SeSugar.Automation;
using SimsBy = SeSugar.Automation.SimsBy;

namespace Staff.POM.Base
{
    public class SearchCriteriaComponent<TResultTile> : BaseComponent where TResultTile : SearchResultTileBase, new()
    {
        private readonly BaseComponent _parent;
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("search_criteria"); }
        }

        public SearchResultsComponent<TResultTile> Search()
        {
            SearchResultsComponent<TResultTile> results = null;
            AutomationSugar.SearchAndWaitForResults(new ByChained(_parent.ComponentIdentifier, this.ComponentIdentifier));
            SeSugar.Automation.Retry.Do(() =>
            {
                results = new SearchResultsComponent<TResultTile>(_parent);
            });

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
            SeSugar.Automation.Retry.Do(Tile.Click);
            AutomationSugar.WaitForAjaxCompletion();
            return new TDetail(); 
        }

        public void Click()
        {
            SeSugar.Automation.Retry.Do(Tile.Click);
            AutomationSugar.WaitForAjaxCompletion();
        }

    }

    public class SearchResultsComponent<TResultTile> : BaseComponent where TResultTile : SearchResultTileBase, new()
    {
        private readonly List<TResultTile> _results = new List<TResultTile>();

        public IList<TResultTile> Where(Func<TResultTile, bool> predicate)
        {
            List<TResultTile> results = null;

            SeSugar.Automation.Retry.Do(() =>
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

        public bool Any()
        {
            return _results.Any();
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
            get { return SimsBy.AutomationId("search_results"); }
        }

        public SearchResultsComponent(BaseComponent parent)
            : base(parent)
        {
            SeSugar.Automation.Retry.Do(Initialise);
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
