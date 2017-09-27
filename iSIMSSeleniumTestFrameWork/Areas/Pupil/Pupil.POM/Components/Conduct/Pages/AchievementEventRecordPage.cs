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
    public class AchievementEventRecordPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("pupil_achievement_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='achievement_event_record_header_title']")]
        private IWebElement _title;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='pupil_button']")]
        private IWebElement _addPupilButton;

        [FindsBy(How = How.Name, Using = "AchievementEventDate")]
        private IWebElement _achievementEventDatePicker;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='pupil_Involved_Selection_Strip']")]
        private IWebElement _pupilsInvolvedSelectionStrip;

        [FindsBy(How = How.Name, Using = "AchievementEventCategories.dropdownImitator")]
        private IWebElement _achievementEventCategoryDropDown;

        public string AchievementEventDate
        {
            set
            {
                _achievementEventDatePicker.SetDateTime(value);
            }
            get
            {
                return _achievementEventDatePicker.GetDateTime();
            }
        }

        public string Title
        {
            get { return _title.GetText(); }
            set { _title.SetText(value); }
        }

        public string AchievementEventCategory
        {
            get { return _achievementEventCategoryDropDown.GetValue(); }
            set { _achievementEventCategoryDropDown.EnterForDropDown(value); }
        }

        public static AchievementEventRecordPage Create()
        {
            Wait.WaitUntilDisplayed(SimsBy.AutomationId("add_button"));
            return new AchievementEventRecordPage();
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
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public PupilPickerDialog ClickAddPupil()
        {
            _addPupilButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            return new PupilPickerDialog();
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
