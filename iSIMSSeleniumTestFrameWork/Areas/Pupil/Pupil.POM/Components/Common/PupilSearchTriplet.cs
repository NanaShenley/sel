using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Pupil;
using POM.Helper;

namespace POM.Components.Common
{
    public class PupilSearchTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public PupilSearchTriplet()
        {
            _searchCriteria = new PupilSearchPage(this);
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_pupil_button']")]
        private IWebElement _addNewPupilButton;

        #endregion

        #region Search

        public class PupilSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Name']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly PupilSearchPage _searchCriteria;
        public PupilSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Action

        public AddNewPupilDialog AddNewPupil()
        {
            _addNewPupilButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddNewPupilDialog();
        }

        #endregion
    }
}
