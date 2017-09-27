using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class AddSenReviewDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("sen_review_record_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "ReviewType.dropdownImitator")]
        private IWebElement _reviewTypeDropdown;

        [FindsBy(How = How.Name, Using = "ReviewStatus.dropdownImitator")]
        private IWebElement _reviewStatusDropdown;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextBox;

        [FindsBy(How = How.Name, Using = "StartTime")]
        private IWebElement _startTimeTextBox;

        [FindsBy(How = How.Name, Using = "Venue.Name")]
        private IWebElement _venueTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _addPeopleInvolvedButton;

        public string ReviewType
        {
            set { _reviewTypeDropdown.EnterForDropDown(value); }
            get { return _reviewTypeDropdown.GetValue(); }
        }

        public string ReviewStatus
        {
            set { _reviewStatusDropdown.EnterForDropDown(value); }
            get { return _reviewStatusDropdown.GetValue(); }
        }

        public string StartDate
        {
            set { _startDateTextBox.SetDateTime(value); }
            get { return _startDateTextBox.GetDateTime(); }
        }

        public string StartTime
        {
            set { _startTimeTextBox.SetDateTime(value, true, true); }
            get { return _startDateTextBox.GetDateTime(true, true); }
        }

        public string Venue
        {
            set { _venueTextBox.SetText(value); }
            get { return _venueTextBox.GetText(); }
        }
        #endregion

        #region Public actions

        public AddPeopleInvolvedDialog ClickAddPeopleInvolved()
        {
            _addPeopleInvolvedButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddPeopleInvolvedDialog();
        }
        #endregion
    }
}
