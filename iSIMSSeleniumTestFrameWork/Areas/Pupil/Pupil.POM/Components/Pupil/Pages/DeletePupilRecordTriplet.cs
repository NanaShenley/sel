using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class DeletePupilRecordTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public DeletePupilRecordTriplet()
        {
            _searchCriteria = new DeletePupilRecordSearchPage(this);
        }
              
        #region Search

        public class PupilRecordSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Name']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly DeletePupilRecordSearchPage _searchCriteria;
        public DeletePupilRecordSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion
    }
}
