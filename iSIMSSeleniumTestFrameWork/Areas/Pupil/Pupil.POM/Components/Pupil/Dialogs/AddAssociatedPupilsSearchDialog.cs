using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class AddAssociatedPupilsSearchDialog : SearchCriteriaComponent<AddAssociatedPupilsTripletDialog.AssociatedPupilsSearchResultTile>
    {
        public AddAssociatedPupilsSearchDialog(BaseDialogComponent parent) : base(parent) { }

        #region Page properties     
        [FindsBy(How = How.Name ,Using = "NameSearchText")]
        private IWebElement _pupilName;
          
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        private IWebElement _searchButton;

        public string PupilName
        {
            set { _pupilName.SetText(value); }
            get { return _pupilName.GetValue(); }
        }
        #endregion

    }
}
