using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using NUnit.Framework;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class TrainingCourseDetailsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_training_courses_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Title")]
        private IWebElement _titleTextBox;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _descriptionTextBox;

        [FindsBy(How = How.Name, Using = "CourseLevel.dropdownImitator")]
        private IWebElement _levelCombobox;

        [FindsBy(How = How.Name, Using = "Duration")]
        private IWebElement _numberDayTextBox;

        [FindsBy(How = How.Name, Using = "FullTime")]
        private IWebElement _isFullTimeCheckbox;

        [FindsBy(How = How.Name, Using = "CourseFees")]
        private IWebElement _courseFeeTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        public string Title
        {
            set { _titleTextBox.SetText(value); }
            get { return _titleTextBox.GetValue(); }
        }

        public string Description
        {
            set { _descriptionTextBox.SetText(value); }
            get { return _descriptionTextBox.GetValue(); }
        }

        public string Level
        {
            set { _levelCombobox.EnterForDropDown(value); }
            get { return _levelCombobox.GetValue(); }
        }

        public string NumberDay
        {
            set { _numberDayTextBox.SetText(value); }
            get { return _numberDayTextBox.GetValue(); }
        }

        public bool IsFullTime
        {
            set { _isFullTimeCheckbox.Set(value); }
            get { return _isFullTimeCheckbox.IsChecked(); }
        }

        public string CourseFee
        {
            set { _courseFeeTextBox.SetText(value); }
            get { return _courseFeeTextBox.GetValue(); }
        }

        #endregion

        #region Grid

        public GridComponent<TrainingCourseEvent> TrainingCourseEvents
        {
            get
            {
                GridComponent<TrainingCourseEvent> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TrainingCourseEvent>(By.CssSelector("[data-maintenance-container='TrainingCourseOccurrences']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class TrainingCourseEvent : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='TrainingCourseOccurrencesStartDate']")]
            private IWebElement _courseDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='VenueDisplay']")]
            private IWebElement _venue;

            [FindsBy(How = How.CssSelector, Using = "[name$='ProviderDisplay']")]
            private IWebElement _courseProvider;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _editButton;


            #region Properties

            public string CourseDate
            {
                set { _courseDate.SetText(value); }
                get { return _courseDate.GetAttribute("value"); }
            }

            public string Venue
            {
                set { _venue.SetText(value); }
                get { return _venue.GetAttribute("value"); }
            }

            public string CourseProvider
            {
                set { _courseProvider.SetText(value); }
                get { return _courseProvider.GetAttribute("Value"); }
            }

            #endregion

            #region Methods

            public TrainingCourseEventDialog ClickEdit()
            {
                _editButton.ClickByJS();
                AutomationSugar.WaitForAjaxCompletion();
                return new TrainingCourseEventDialog();
            }
            #endregion
        }

        #endregion

        #region Public methods

        public TrainingCourseEventDialog AddTrainingCourseEvents()
        {
            AutomationSugar.ClickOnAndWaitFor("add_training_course_event_button", "staff_training_course_event_dialog");

            return new TrainingCourseEventDialog();
        }

        /// <summary>
        /// Author: Luong.Mai
        /// Description: Check a success message is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessMessageIsDisplayed()
        {
            return SeleniumHelper.DoesWebElementExist(_successMessage);
        }

        public void ClickSave()
        {
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("well_know_action_save")));
            AutomationSugar.WaitForAjaxCompletion();         
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("delete_button")));
                AutomationSugar.WaitForAjaxCompletion();
                var confirmDialog = new DeleteConfirmationPage();
                confirmDialog.ConfirmDelete();
                Refresh();
            }
        }

        #endregion
    }
}

