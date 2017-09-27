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

        [FindsBy(How = How.Name, Using = "LegalForename")]
        private IWebElement _legalLegalForenameTextBox;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _legalSurnameTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        private IWebElement _searchButton;

        public string LegalForename
        {
            set { _legalLegalForenameTextBox.SetText(value); }
            get { return _legalLegalForenameTextBox.GetValue(); }
        }

        public string LegalSurname
        {
            set { _legalSurnameTextBox.SetText(value); }
            get { return _legalSurnameTextBox.GetValue(); }
        }

        #endregion

    }
}
