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
                
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_attendees_button']")]
        private IWebElement _deleteAttendeesButton;

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

            [FindsBy(How = How.CssSelector, Using = "[name$='.ColumnSelector']")]
            private IWebElement _selected;

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
                set { _AdditionalCosts.SetText(value); }

            }

            public string Comment
            {
                get { return _Comment.GetAttribute("Value"); }
                set { _Comment.SetText(value); }
            }

            public bool Selected
            {
                set { _selected.Set(value); }
                get { return _selected.IsChecked(); }
            }
        }

        #endregion

        #region Public methods
                
        public FindStaffTripletDialog AddAttendees()
        {
            AutomationSugar.ClickOnAndWaitFor("add_attendees_button", "staff_simple_search_palette_triplet");
            return new FindStaffTripletDialog();
        }

        public void ClickDeleteAttendees(bool isChecked)
        {
            if (isChecked == true)
            {
                AutomationSugar.ClickOnAndWaitFor(SimsBy.AutomationId("delete_attendees_button"), By.CssSelector("[data-section-id='generic-confirm-dialog']"));
                AutomationSugar.ClickOnAndWaitForUntilStale(new ByChained(By.CssSelector("[data-section-id='generic-confirm-dialog']"), SimsBy.AutomationId("yes_button")), By.CssSelector("[data-section-id='generic-confirm-dialog']"));
            }
            else
            {
                AutomationSugar.ClickOnAndWaitFor(SimsBy.AutomationId("delete_attendees_button"), By.CssSelector("[data-section-id='generic-confirm-dialog']"));
                AutomationSugar.ClickOnAndWaitForUntilStale(new ByChained(By.CssSelector("[data-section-id='generic-confirm-dialog']"), SimsBy.AutomationId("ok_button")), By.CssSelector("[data-section-id='generic-confirm-dialog']"));
            }
          
        }

        #endregion
    }
}
