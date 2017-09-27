using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class CopyContactTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        public CopyContactTripletDialog()
        {
            _searchCriteria = new CopyContactSearchDialog(this);
        }

        #region Search

        public class CopyContactSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly CopyContactSearchDialog _searchCriteria;
        public CopyContactSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        #endregion
    }
}
