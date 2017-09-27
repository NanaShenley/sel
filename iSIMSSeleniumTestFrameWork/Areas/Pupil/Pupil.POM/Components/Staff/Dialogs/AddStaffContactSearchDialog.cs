using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class AddStaffContactSearchDialog : SearchCriteriaComponent<AddStaffContactTripletDialog.StaffContactSearchResultTile>
    {
        public AddStaffContactSearchDialog(BaseDialogComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _surnameTextBox;

        public string SurName
        {
            set { _surnameTextBox.SetText(value); }
            get { return _surnameTextBox.GetAttribute("value"); }
        }

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _forenameTextBox;

        public string ForeName
        {
            set { _forenameTextBox.SetText(value); }
            get { return _forenameTextBox.GetAttribute("value"); }
        }

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _genderDropdown;

        public string Gender
        {
            set { _genderDropdown.EnterForDropDown(value); }
            get { return _genderDropdown.GetAttribute("value"); }
        }

        #endregion

    }
}
