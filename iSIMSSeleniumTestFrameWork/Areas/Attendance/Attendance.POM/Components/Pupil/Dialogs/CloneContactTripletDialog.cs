using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class CloneContactTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        public CloneContactTripletDialog()
        {
            _searchCriteria = new CloneContactSearchDialog(this);
        }

        #region Search

        public class CloneContactSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly CloneContactSearchDialog _searchCriteria;
        public CloneContactSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        #endregion
    }
}
