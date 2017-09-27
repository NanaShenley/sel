using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Conduct.Pages
{
    public class AchievementEventTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        private readonly AchievementEventSearchPage _searchCriteria;
        public AchievementEventSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        public AchievementEventTriplet()
        {
            _searchCriteria = new AchievementEventSearchPage(this);
        }

        public class AchievementEventSearchResultTile : SearchResultTileBase
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
