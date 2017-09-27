using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
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

        [FindsBy(How = How.Id, Using = "tri_chkbox_FullTime")]
        private IWebElement _isFullTimeCheckbox;

        [FindsBy(How = How.Name, Using = "CourseFeesString")]
        private IWebElement _courseFeeTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_training_course_events_button']")]
        private IWebElement _addTrainingCourseEventButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

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
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                return new TrainingCourseEventDialog();
            }
            #endregion
        }

        #endregion

        #region Public methods

        public TrainingCourseEventDialog AddTrainingCourseEvents()
        {
            _addTrainingCourseEventButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
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
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new DeleteConfirmationPage();
                confirmDialog.ConfirmDelete();
                Refresh();
            }
        }

        #endregion
    }
}

