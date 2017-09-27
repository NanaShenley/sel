using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class AddPupilContactTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("pupil_contact_record_triplet"); }
        }

        public AddPupilContactTripletDialog()
        {
            _searchCriteria = new AddPupilContactSearchDialog(this);
        }

        #region Search

        public class PupilCotactSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly AddPupilContactSearchDialog _searchCriteria;
        public AddPupilContactSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        #endregion

        #region Page Action

        public AddPupilContactDialog CreateContact()
        {
            _createButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddPupilContactDialog();
        }

        #endregion
    }
}
