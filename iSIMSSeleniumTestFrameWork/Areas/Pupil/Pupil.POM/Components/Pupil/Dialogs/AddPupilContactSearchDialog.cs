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

        [FindsBy(How = How.Name, Using = "NameSearchText")]
        private IWebElement _nameSearchText;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _genderDropdown;

        public string NameSearchText
        {
            set { _nameSearchText.SetText(value); }
            get { return _nameSearchText.GetAttribute("value"); }
        }

        public string Gender
        {
            set { _genderDropdown.EnterForDropDown(value); }
            get { return _genderDropdown.GetAttribute("value"); }
        }

        #endregion

    }
}
