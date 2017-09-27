using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.Components.StaffRegression
{
    public class TrainingCourseEventDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_training_course_event_dialog"); }
        }

        #region Page properties
        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDate;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDate;

        [FindsBy(How = How.Name, Using = "RenewalDate")]
        private IWebElement _renewalDate;

        [FindsBy(How = How.Name, Using = "Venue")]
        private IWebElement _venue;

        [FindsBy(How = How.Name, Using = "Provider")]
        private IWebElement _provider;

        [FindsBy(How = How.Name, Using = "Comment")]
        private IWebElement _comment;

        public string StartDate
        {
            set { _startDate.SetText(value); }
            get { return _startDate.GetValue(); }
        }

        public string EndDate
        {
            set { _endDate.SetText(value); }
            get { return _endDate.GetValue(); }
        }

        public string RenewalDate
        {
            set { _renewalDate.SetText(value); }
            get { return _renewalDate.GetValue(); }
        }

        public string Venue
        {
            set { _venue.SetText(value); }
            get { return _venue.GetValue(); }
        }

        public string Provider
        {
            set { _provider.SetText(value); }
            get { return _provider.GetValue(); }
        }

        public string Comment
        {
            set { _comment.SetText(value); }
            get { return _comment.GetValue(); }
        }
        #endregion
    }
    public class TrainingCourses : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_training_courses_triplet"); }
        }

        public class TrainingCoursesDetail : BaseComponent
        {
            public override By ComponentIdentifier
            {
                get { return SimsBy.AutomationId("staff_training_courses_detail"); }
            }

            #region Page properties
            [FindsBy(How = How.Name, Using = "ResourceProviderString")]
            private IWebElement _resourceProvider;

            [FindsBy(How = How.Name, Using = "Title")]
            private IWebElement _title;

            [FindsBy(How = How.Name, Using = "Description")]
            private IWebElement _description;

            [FindsBy(How = How.Name, Using = "CourseLevel.dropdownImitator")]
            private IWebElement _level;

            [FindsBy(How = How.Name, Using = "Duration")]
            private IWebElement _noOfDays;

            [FindsBy(How = How.Name, Using = "tri_chkbox_FullTime")]
            private IWebElement _isFullTime;

            [FindsBy(How = How.Name, Using = "CourseFeesString")]
            private IWebElement _courseFees;

            public string ResourceProvider
            {
                set { _resourceProvider.SetText(value); }
                get { return _resourceProvider.GetValue(); }
            }

            public string Title
            {
                set { _title.SetText(value); }
                get { return _title.GetValue(); }
            }

            public string Description
            {
                set { _description.SetText(value); }
                get { return _description.GetValue(); }
            }

            public string Level
            {
                set { _level.ChooseSelectorOption(value); }
                get { return _level.GetValue(); }
            }

            public string NumberOfDays
            {
                set { _noOfDays.SetText(value); }
                get { return _noOfDays.GetValue(); }
            }

            public bool IsFullTime
            {
                set { _isFullTime.SetCheckBox(value); }
                get { return _isFullTime.IsCheckboxChecked(); }
            }

            public string CourseFees
            {
                set { _courseFees.SetText(value); }
                get { return _courseFees.GetValue(); }
            }
            #endregion

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_training_course_events_button']")]
            private IWebElement _addTrainingCourseEventsButton;

            public TrainingCourseEventDialog AddTrainingCourseEvent()
            {
                Retry.Do(_addTrainingCourseEventsButton.Click);
                return new TrainingCourseEventDialog();
            }

            public TrainingCoursesDetail Save()
            {
                Action save = () => SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("well_know_action_save"), By.CssSelector("[data-automation-id='staff_training_courses_detail'] div.alert"));
                Retry.Do(save);
                return new TrainingCoursesDetail();
            }
        }

        public TrainingCoursesDetail Create()
        {
            SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("staff_training_courses_create_button"), By.CssSelector("[data-automation-id='staff_training_courses_detail'].has-datamaintenance"));
            return new TrainingCoursesDetail();
        }
    }
}
