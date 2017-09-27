using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class TrainingCourseEventDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_training_course_event_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateTextBox;

        [FindsBy(How = How.Name, Using = "RenewalDate")]
        private IWebElement _renewalDateTextBox;

        [FindsBy(How = How.Name, Using = "Venue")]
        private IWebElement _venueTextBox;

        [FindsBy(How = How.Name, Using = "Provider")]
        private IWebElement _providerTextBox;

        [FindsBy(How = How.Name, Using = "Comment")]
        private IWebElement _commentTextBox;


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_attendee_button']")]
        private IWebElement _addAttendeeButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        public string StartDate
        {
            get { return _startDateTextBox.GetDateTime(); }
            set { _startDateTextBox.SetDateTime(value); }
        }

        public string EndDate
        {
            get { return _endDateTextBox.GetDateTime(); }
            set { _endDateTextBox.SetDateTime(value); }
        }

        public string RenewalDate
        {
            get { return _renewalDateTextBox.GetDateTime(); }
            set { _renewalDateTextBox.SetDateTime(value); }
        }

        public string Venue
        {
            get { return _venueTextBox.GetValue(); }
            set { _venueTextBox.SetText(value); }
        }

        public string Provider
        {
            get { return _providerTextBox.GetValue(); }
            set { _providerTextBox.SetText(value); }
        }

        public string Comment
        {
            get { return _commentTextBox.GetValue(); }
            set { _commentTextBox.SetText(value); }
        }




        public GridComponent<AttendeesRow> Attendees
        {
            get
            {
                GridComponent<AttendeesRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AttendeesRow>(By.CssSelector("[data-maintenance-container='StaffTrainingCourseEnrolments']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }


        public class AttendeesRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='StaffName']")]
            private IWebElement _AttendeeName;

            [FindsBy(How = How.CssSelector, Using = "[name$='CourseEnrolmentStatus.dropdownImitator']")]
            private IWebElement _CourseStatus;

            [FindsBy(How = How.CssSelector, Using = "[name$='AdditionalCosts']")]
            private IWebElement _AdditionalCosts;

            [FindsBy(How = How.CssSelector, Using = "[name$='Comment']")]
            private IWebElement _Comment;

            public string AttendeeName
            {
                get { return _AttendeeName.GetAttribute("Value"); }
            }

            public string CourseStatus
            {
                get { return _CourseStatus.GetAttribute("Value"); }
            }

            public string AdditionalCosts
            {
                get { return _AdditionalCosts.GetAttribute("Value"); }
            }

            public string Comment
            {
                get { return _Comment.GetAttribute("Value"); }
            }
        }



        #endregion

        #region Public methods

        public TrainingCourseDetailsPage ClickOk()
        {
            _okButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            //Wait for loading data into table
            SeleniumHelper.Sleep(10);
            return new TrainingCourseDetailsPage();
        }

        public TrainingCourseDetailsPage ClickCancel()
        {
            _cancelButton.ClickByJS();
            return new TrainingCourseDetailsPage();
        }
        public FindStaffTripletDialog AddAttendee()
        {
            _addAttendeeButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new FindStaffTripletDialog();
        }

      

        #endregion
    }
}
