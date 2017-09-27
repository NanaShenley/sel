using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class ManageLeaversPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='searchResults']"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.Name, Using = "DateOfLeaving")]
        private IWebElement _dateOfLeavingTextBox;

        [FindsBy(How = How.Name, Using = "ReasonForLeaving.dropdownImitator")]
        private IWebElement _reasonLeavingDropdown;

        [FindsBy(How = How.Name, Using = "Destination")]
        private IWebElement _destinationTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='apply_to_selected_pupils_button']")]
        private IWebElement _applyButton;

        [FindsBy(How = How.Name, Using = "Rows")]
        private IWebElement _manageLeaverTable;

        public string DateOfLeaving
        {
            get { return _dateOfLeavingTextBox.GetValue(); }
            set { _dateOfLeavingTextBox.SetDateTime(value); }
        }

        public string ReasonForLeaving
        {
            get { return _reasonLeavingDropdown.GetValue(); }
            set { _reasonLeavingDropdown.EnterForDropDown(value); }
        }

        public string DestinationDetails
        {
            get { return _destinationTextBox.GetValue(); }
            set { _destinationTextBox.SetText(value); }
        }

        public WebixComponent<WebixCell> ManageLeaverTable
        {
            get { return new WebixComponent<WebixCell>(_manageLeaverTable); }
        }

        #endregion

        #region Public methods

        public void ClickApplyToSelectedPupil()
        {
            _applyButton.Click();
        }

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        #endregion
    }
}
