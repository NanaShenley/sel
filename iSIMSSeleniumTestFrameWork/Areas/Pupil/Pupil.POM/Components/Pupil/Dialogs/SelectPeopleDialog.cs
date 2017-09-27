using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Pupil.Dialogs;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class SelectPeopleDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("sen_review_people_involved_participant_detail"); }
        }

        public SelectPeopleDialog()
        {
            _searchCriteria = new StaffSearch(this);
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        #endregion

        #region Public methods

        public AddPeopleInvolvedDialog ClickOk()
        {
            _okButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            return new AddPeopleInvolvedDialog();
        }

        public PEPContributorsDialog ClickPepContributorsOk()
        {
            _okButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            return new PEPContributorsDialog();
        }

        #endregion

        #region Search

        private readonly StaffSearch _searchCriteria;
        public StaffSearch SearchCriteria { get { return _searchCriteria; } }

        public class StaffSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        #endregion
    }

    public class StaffSearch : SearchListCriteriaComponent<SelectPeopleDialog.StaffSearchResultTile>
    {
        public StaffSearch(BaseComponent parent) : base(parent) { }

        #region Search section properties

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _surnameTextBox;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _forenameTextBox;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _genderDropdown;

        [FindsBy(How = How.Name, Using = "Role.dropdownImitator")]
        private IWebElement _roleDropdown;

        public string SurName
        {
            set { _surnameTextBox.SetText(value); }
            get { return _surnameTextBox.GetValue(); }
        }

        public string ForeName
        {
            set { _forenameTextBox.SetText(value); }
            get { return _forenameTextBox.GetValue(); }
        }

        public string Gender
        {
            set { _genderDropdown.EnterForDropDown(value); }
            get { return _genderDropdown.GetValue(); }
        }

        public string Role
        {
            set { _roleDropdown.EnterForDropDown(value); }
            get { return _roleDropdown.GetValue(); }
        }
        #endregion
    }
}
