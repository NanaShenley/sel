using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Base;
using PageObjectModel.Helper;

using System;

namespace PageObjectModel.Components.Admission
{
    public class AddNewApplicationDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("add_new_application_wizard"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "LegalForename")]
        private IWebElement _foreNameTextBox;

        [FindsBy(How = How.Name, Using = "LegalMiddleNames")]
        private IWebElement _middleNameTextBox;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _sureNameTextBox;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _genderTextBox;

        [FindsBy(How = How.Name, Using = "DateOfBirth")]
        private IWebElement _dateOfBirthTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_button']")]
        private IWebElement _continueButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        #endregion

        #region Actions

        public string ForeName
        {
            set { _foreNameTextBox.SetText(value); }
            get { return _foreNameTextBox.GetValue(); }
        }

        public string MiddleName
        {
            set { _middleNameTextBox.SetText(value); }
            get { return _middleNameTextBox.GetValue(); }
        }
        public string SureName
        {
            set { _sureNameTextBox.SetText(value); }
            get { return _sureNameTextBox.GetValue(); }
        }
        public string Gender
        {
            set { _genderTextBox.EnterForDropDown(value); }
            get { return _sureNameTextBox.GetValue(); }
        }
        public string DateofBirth
        {
            set { _dateOfBirthTextBox.SetDateTime(value); }
            get { return _dateOfBirthTextBox.GetDateTime(); }
        }

        public void Continue()
        {
            if (_continueButton.IsExist())
            {
                _continueButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }

            Refresh();
        }

        public void ContiueCreateApplicant()
        {

            try
            {
                IWebElement _buttonContinue = SeleniumHelper.FindElement(SimsBy.AutomationId("no,_this_application_is_for_a_new_pupil_button"));
                _buttonContinue.Click();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            catch (Exception)
            {

            }

        }

        public ApplicationPage Cancel()
        {
            if (_cancelButton.IsExist())
            {
                _cancelButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new ApplicationPage();
            Refresh();
        }
        #endregion
    }
}
