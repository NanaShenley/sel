using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class SchoolHistoryDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='select_button']")]
        private IWebElement _selectPreviousSchoolButton;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateField;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateField;

        [FindsBy(How = How.Name, Using = "EnrolmentStatus.dropdownImitator")]
        private IWebElement _enrolmentStatusField;

        [FindsBy(How = How.Name, Using = "ReasonForLeaving.dropdownImitator")]
        private IWebElement _reasonForLeavingField;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_attendance_summary_button']")]
        private IWebElement _addattendanceSummaryButton;

        public string StartDate
        {
            set
            {
                _startDateField.SetDateTime(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                _startDateField.SendKeys(Keys.Tab);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get
            {
                return _startDateField.GetDateTime();
            }
        }

        public string EndDate
        {
            set
            {
                _endDateField.SetDateTime(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                _startDateField.SendKeys(Keys.Tab);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get
            {
                return _endDateField.GetDateTime();
            }
        }

        public string EnrolmentStatus
        {
            set { _enrolmentStatusField.ChooseSelectorOption(value); }
            get { return _enrolmentStatusField.GetValue(); }
        }

        public string ReasonForLeaving
        {
            set { _reasonForLeavingField.ChooseSelectorOption(value); }
            get { return _reasonForLeavingField.GetValue(); }
        }

        public SelectSchoolTripletDialog ClickSelectPreviousSchool()
        {
            _selectPreviousSchoolButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new SelectSchoolTripletDialog();
        }

        public GridComponent<AttendanceSummaryRow> AttendanceSummaryTable
        {
            get
            {
                GridComponent<AttendanceSummaryRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AttendanceSummaryRow>(By.CssSelector("[data-maintenance-container='LearnerPreviousSchoolAttendanceSummary']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public void AddAttendanceSummaryRow()
        {
            _addattendanceSummaryButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }
    }

    public class AttendanceSummaryRow : GridRow
    {
        [FindsBy(How = How.CssSelector, Using = "[Name$='Year']")]
        private IWebElement _year;

        public string Year
        {
            set { _year.SetText(value); }
            get { return _year.GetValue(); }
        }

        [FindsBy(How = How.CssSelector, Using = "[Name$='PossibleSessions']")]
        private IWebElement _possibleSessions;

        public string PossibleSessions
        {
            set { _possibleSessions.SetText(value); }
            get { return _possibleSessions.GetValue(); }
        }

        [FindsBy(How = How.CssSelector, Using = "[Name$='AuthorisedSessions']")]
        private IWebElement _authorisedSessions;

        public string AuthorisedSessions
        {
            set { _authorisedSessions.SetText(value); }
            get { return _authorisedSessions.GetValue(); }
        }

        [FindsBy(How = How.CssSelector, Using = "[Name$='UnauthorisedSessions']")]
        private IWebElement _unauthorisedSessions;

        public string UnauthorisedSessions
        {
            set { _unauthorisedSessions.SetText(value); }
            get { return _unauthorisedSessions.GetValue(); }
        }

        [FindsBy(How = How.CssSelector, Using = "[Name$='AttendedSessions']")]
        private IWebElement _attendedSessions;

        public string AttendedSessions
        {
            set { _attendedSessions.SetText(value); }
            get { return _attendedSessions.GetValue(); }
        }
    }
}
