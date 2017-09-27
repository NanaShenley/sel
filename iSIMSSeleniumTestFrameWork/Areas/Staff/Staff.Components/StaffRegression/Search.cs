﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using System.Threading;
using WebDriverRunner.webdriver;

namespace Staff.Components.StaffRegression
{
    public class SearchCriteriaComponent<TResultTile> : BaseComponent where TResultTile : SearchResultTileBase, new()
    {
        private readonly BaseComponent _parent;
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("search_criteria"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        private IWebElement _searchButton;

        public SearchResultsComponent<TResultTile> Search()
        {
            Retry.Do(_searchButton.Click);
            SearchResultsComponent<TResultTile> results = null;
            Thread.Sleep(1000);
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
            Tile.Click();

            var detail = default(TDetail);

            Retry.Do(() =>
            {
                detail = new TDetail();
            });

            Thread.Sleep(2000);
            detail.Refresh();
            return detail;
        }

        public void Click()
        {
            Tile.Click();

            Thread.Sleep(2000);
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

        public TResultTile Single(Func<TResultTile, bool> predicate)
        {
            var results = this.Where(predicate);
            return results.Single();
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
