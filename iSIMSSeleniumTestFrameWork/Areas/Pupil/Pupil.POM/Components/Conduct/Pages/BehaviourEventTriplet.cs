using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Conduct.Pages
{
    public class BehaviourEventTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        private readonly BehaviourEventSearchPage _searchCriteria;
        public BehaviourEventSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        public BehaviourEventTriplet()
        {
            _searchCriteria = new BehaviourEventSearchPage(this);
        }

        public class BehaviourEventSearchResultTile : SearchResultTileBase
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
