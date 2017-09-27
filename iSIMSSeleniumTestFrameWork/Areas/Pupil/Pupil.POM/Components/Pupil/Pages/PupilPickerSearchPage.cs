using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class PupilPickerSearchPage : SearchCriteriaComponent<PupilSearchTriplet.PupilSearchResultTile>
    {
        public PupilPickerSearchPage(BaseComponent parent) : base(parent) { }

        #region Search roperties

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _pupilNameTextBox;

        public string PupilName
        {
            set { _pupilNameTextBox.SetText(value); }
            get { return _pupilNameTextBox.GetValue(); }
        }

        #endregion
    }
}
