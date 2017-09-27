using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class AddAssociatedPupilsTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        public AddAssociatedPupilsTripletDialog()
        {
            _searchCriteria = new AddAssociatedPupilsSearchDialog(this);
        }

        #region Search

        public class AssociatedPupilsSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly AddAssociatedPupilsSearchDialog _searchCriteria;
        public AddAssociatedPupilsSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

    }
}
