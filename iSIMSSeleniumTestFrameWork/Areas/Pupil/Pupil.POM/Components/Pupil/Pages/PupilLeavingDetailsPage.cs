using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;
using SeSugar.Automation;
using SimsBy = POM.Helper.SimsBy;


namespace POM.Components.Pupil
{
    public class PupilLeavingDetailsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "DOL")]
        private IWebElement _dolTextBox;

        [FindsBy(How = How.Name, Using = "ReasonForLeaving.dropdownImitator")]
        private IWebElement _reasonForLeavingDropDown;

        [FindsBy(How = How.Name, Using = "Destination")]
        private IWebElement _destinationTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        public string DOL
        {
            set { _dolTextBox.SetDateTime(value); }
            get { return _dolTextBox.GetDateTime(); }
        }

        public string ReasonForLeaving
        {
            set { _reasonForLeavingDropDown.EnterForDropDown(value); }
            get { return _reasonForLeavingDropDown.GetValue(); }
        }

        public string Destination
        {
            set { _destinationTextBox.SetText(value); }
        }

        #endregion

        #region Page Actions

        public ConfirmRequiredDialog ClickSave(bool waitForConfirmDialog = true)
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            if (waitForConfirmDialog)
            {
                Wait.WaitForElement(SimsBy.CssSelector("[data-section-id='generic-confirm-dialog']"));
                return new ConfirmRequiredDialog();
            }
            return null;
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Check Pupil Leaving detail display for pupil name.
        /// </summary>
        /// <param name="pupilName"></param>
        /// <returns></returns>
        public bool IsPupilLeavingDetailForPupilName(string pupilName)
        {
            try
            {
                IWebElement titleElement = SeleniumHelper.FindElement(SimsBy.AutomationId("pupil_leaving_details_header_display_name"));
                return titleElement.GetText().Equals(pupilName);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        #endregion

        public bool IsDateOfAdmissionWarningMessageDisplayed()
        {
            try
            {
				AutomationSugar.WaitForAjaxCompletion();
				return SeleniumHelper.IsElementExists(SimsBy.AutomationId("status_error"));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
