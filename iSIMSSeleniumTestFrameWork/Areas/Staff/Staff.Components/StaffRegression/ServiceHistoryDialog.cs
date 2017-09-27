using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;

namespace Staff.Components.StaffRegression
{
    public class ServiceHistoryDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_service_history_dialog"); }
        }

        #region Page properties
        [FindsBy(How = How.Name, Using = "DOA")]
        private IWebElement _dateOfArrival;

        [FindsBy(How = How.Name, Using = "ContinuousServiceStartDate")]
        private IWebElement _continuousServiceStartDate;

        [FindsBy(How = How.Name, Using = "LocalAuthorityStartDate")]
        private IWebElement _localAuthorityStartDate;

        [FindsBy(How = How.Name, Using = "PreviousEmployer")]
        private IWebElement _previousEmployer;

        [FindsBy(How = How.Name, Using = "NextEmployer")]
        private IWebElement _nextEmployer;

        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _notes;

        public string DateOfArrival
        {
            set
            {
                _dateOfArrival.SetText(value);
            }
            get
            {
                return _dateOfArrival.GetValue();
            }
        }
        public string ContinuousServiceStartDate
        {
            set
            {
                _continuousServiceStartDate.SetText(value);
            }
            get
            {
                return _continuousServiceStartDate.GetValue();
            }
        }
        public string LocalAuthorityStartDate
        {
            set
            {
                _localAuthorityStartDate.SetText(value);
            }
            get
            {
                return _localAuthorityStartDate.GetValue();
            }
        }
        public string PreviousEmployer
        {
            set
            {
                _previousEmployer.SetText(value);
            }
            get
            {
                return _previousEmployer.GetValue();
            }
        }
        public string NextEmployer
        {
            set
            {
                _nextEmployer.SetText(value);
            }
            get
            {
                return _nextEmployer.GetValue();
            }
        }
        public string Notes
        {
            set
            {
                _notes.SetText(value);
            }
            get
            {
                return _notes.GetValue();
            }
        }
        #endregion
    }
}
