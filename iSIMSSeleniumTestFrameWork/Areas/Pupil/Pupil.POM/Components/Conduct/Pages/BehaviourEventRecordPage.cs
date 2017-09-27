using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Pupil.Dialogs;
using POM.Helper;
using SeSugar.Automation;
using SimsBy = POM.Helper.SimsBy;

namespace POM.Components.Conduct.Pages
{
    public class BehaviourEventRecordPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("pupil_behaviour_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='behaviour_event_record_header_title']")]
        private IWebElement _title;

        [FindsBy(How = How.Name, Using = "BehaviourEventCategories.dropdownImitator")]
        private IWebElement _behaviourEventCategoryDropDown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='pupil_button']")]
        private IWebElement _addPupilButton;

        [FindsBy(How = How.Id, Using = "FollowUpActionTab-label")]
        private IWebElement _followUpTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='apply_button']")]
        private IWebElement _addFollowUpButton;

        [FindsBy(How = How.Name, Using = "FollowUpAction.dropdownImitator")]
        private IWebElement _followUpActionDropDown;

        [FindsBy(How = How.Name, Using = "BehaviourEventDate")]
        private IWebElement _behaviourEventDatePicker;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='pupil_Involved_Selection_Strip']")]
        private IWebElement _pupilsInvolvedSelectionStrip;

        public string Title
        {
            get { return _title.GetText(); }
            set { _title.SetText(value); }
        }

        public string BehaviourEventCategory
        {
            get { return _behaviourEventCategoryDropDown.GetValue(); }
            set { _behaviourEventCategoryDropDown.EnterForDropDown(value); }
        }

        public string FollowUpAction
        {
            set
            {
                _followUpActionDropDown.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get
            {
                return _followUpActionDropDown.GetValue();
            }
        }

        public string BehaviourEventDate
        {
            set
            {
                _behaviourEventDatePicker.SetDateTime(value);
            }
            get
            {
                return _behaviourEventDatePicker.GetDateTime();
            }
        }

        public static BehaviourEventRecordPage Create()
        {
            Wait.WaitUntilDisplayed(SimsBy.AutomationId("add_button"));
            return new BehaviourEventRecordPage();
        }

        #endregion

        #region Page action

        public void ClickAdd()
        {
            _addButton.ClickByJS();
            Wait.WaitUntilDisplayed(SimsBy.AutomationId("well_know_action_save"));
            Refresh();
        }

        public void ClickSave()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public PupilPickerDialog ClickAddPupil()
        {
            _addPupilButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            return new PupilPickerDialog();
        }

        public void ClickFollowUpTab()
        {
            _followUpTab.ClickByJS();
            Wait.WaitUntilDisplayed(SimsBy.AutomationId("pupil_Involved_Selection_Strip"));
            Refresh();
        }

        public void ClickApplyFollowUp()
        {
            _addFollowUpButton.ClickByJS();
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void SelectPupilInvolvedForFollowUp(string value)
        {
            IList<IWebElement> pupilInvolvedLabels = _pupilsInvolvedSelectionStrip.FindElements(By.CssSelector(".checkbox-container label .checkbox-label-text"));
            for (int i = 0; i < pupilInvolvedLabels.Count; i++)
            {
                if (pupilInvolvedLabels[i].GetText().Equals(value))
                {
                    IWebElement input = _pupilsInvolvedSelectionStrip.FindElements(By.CssSelector(".checkbox-container label input[type='checkbox']"))[i];
                    input.Set(true);
                    break;
                }
            }
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Verify success message is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessMessageDisplayed()
        {
            AutomationSugar.WaitForAjaxCompletion();
            return SeleniumHelper.IsElementExists(SimsBy.AutomationId("status_success"));
        }

        #endregion
    }
}
