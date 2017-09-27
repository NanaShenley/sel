using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class AddPupilContactSearchDialog : SearchCriteriaComponent<AddPupilContactTripletDialog.PupilCotactSearchResultTile>
    {
        public AddPupilContactSearchDialog(BaseDialogComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _surnameTextBox;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _forenameTextBox;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _genderDropdown;

        public string SurName
        {
            set { _surnameTextBox.SetText(value); }
            get { return _surnameTextBox.GetAttribute("value"); }
        }

        public string ForeName
        {
            set { _forenameTextBox.SetText(value); }
            get { return _forenameTextBox.GetAttribute("value"); }
        }

        public string Gender
        {
            set { _genderDropdown.EnterForDropDown(value); }
            get { return _genderDropdown.GetAttribute("value"); }
        }

        #endregion

    }
}
