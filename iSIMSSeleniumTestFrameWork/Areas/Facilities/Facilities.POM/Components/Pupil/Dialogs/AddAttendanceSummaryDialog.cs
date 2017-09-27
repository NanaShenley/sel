using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class AddAttendanceSummaryDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Year")]
        private IWebElement _year;

        public string Year
        {
            set { _year.SetText(value); }
            get { return _year.GetValue(); }
        }

        [FindsBy(How = How.Name, Using = "PossibleSessions")]
        private IWebElement _possibleSessions;

        public string PossibleSessions
        {
            set { _possibleSessions.SetText(value); }
            get { return _possibleSessions.GetValue(); }
        }

        [FindsBy(How = How.Name, Using = "AuthorisedSessions")]
        private IWebElement _authorisedSessions;

        public string AuthorisedSessions
        {
            set { _authorisedSessions.SetText(value); }
            get { return _authorisedSessions.GetValue(); }
        }

        [FindsBy(How = How.Name, Using = "UnauthorisedSessions")]
        private IWebElement _unauthorisedSessions;

        public string UnauthorisedSessions
        {
            set { _unauthorisedSessions.SetText(value); }
            get { return _unauthorisedSessions.GetValue(); }
        }

        [FindsBy(How = How.Name, Using = "AttendedSessions")]
        private IWebElement _attendedSessions;

        public string AttendedSessions
        {
            set { _attendedSessions.SetText(value); }
            get { return _attendedSessions.GetValue(); }
        }

        #endregion
    }
}
