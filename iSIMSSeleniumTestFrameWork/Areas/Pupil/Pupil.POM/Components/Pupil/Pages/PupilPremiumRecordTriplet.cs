using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using POM.Components.Pupil.Pages;


namespace POM.Components.Pupil
{
    public class PupilPremiumRecordTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public PupilPremiumRecordTriplet()
        {
            _searchCriteria = new PupilPremiumRecordSearchPage(this);
        }

        #region Properties

        #endregion

        #region Search

        public class PupilPremiumRecordSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Name']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly PupilPremiumRecordSearchPage _searchCriteria;
        public PupilPremiumRecordSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Action

        #endregion
    }
}
