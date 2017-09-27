using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class AddNewPupilDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "LegalForename")]
        private IWebElement _forenameTextBox;

        [FindsBy(How = How.Name, Using = "LegalMiddleNames")]
        private IWebElement _middleNameTextBox;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _surNameTextBox;

        [FindsBy(How = How.Name, Using = "DateOfBirth")]
        private IWebElement _dateOfBirthTextBox;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _genderCombobox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_button']")]
        private IWebElement _continueButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        public string Forename
        {
            set { _forenameTextBox.SetText(value); }
            get { return _forenameTextBox.GetValue(); }
        }

        public string MiddleName
        {
            set { _middleNameTextBox.SetText(value); }
            get { return _middleNameTextBox.GetValue(); }
        }

        public string Gender
        {
            set { _genderCombobox.EnterForDropDown(value); }
            get { return _genderCombobox.GetText(); }
        }

        public string SurName
        {
            set { _surNameTextBox.SetText(value); }
            get { return _surNameTextBox.GetValue(); }
        }

        public string DateOfBirth
        {

            set { _dateOfBirthTextBox.SetDateTime(value); }
            get { return _dateOfBirthTextBox.GetDateTime(); }
        }

        #endregion

        #region Public actions

        public RegistrationDetailDialog Continue()
        {
            Retry.Do(_continueButton.ClickByJS);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new RegistrationDetailDialog();
        }

        public DidYouMeanDialog ContinueReAdmit()
        {
            Retry.Do(_continueButton.ClickByJS);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new DidYouMeanDialog();
        }

        #endregion
    }
}
