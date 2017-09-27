using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class PupilRecordTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public PupilRecordTriplet()
        {
            _searchCriteria = new PupilRecordSearchPage(this);
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_pupil_button']")]
        private IWebElement _addNewPupilButton;

        #endregion

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

        private readonly PupilRecordSearchPage _searchCriteria;
        public PupilRecordSearchPage SearchCriteria { get { return _searchCriteria; } }

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
