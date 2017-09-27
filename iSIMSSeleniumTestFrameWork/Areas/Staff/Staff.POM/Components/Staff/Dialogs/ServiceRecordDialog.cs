using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff
{
    public class ServiceRecordDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_service_history_dialog"); }
        }
        
        #region Page properties
       
        [FindsBy(How = How.Name, Using = "DOA")]
        private IWebElement _doaTextBox;

        [FindsBy(How = How.Name, Using = "DOL")]
        private IWebElement _dolTextBox;

        [FindsBy(How = How.Name, Using = "ContinuousServiceStartDate")]
        private IWebElement _continuousServiceStartDateTextBox;

        [FindsBy(How = How.Name, Using = "LocalAuthorityStartDate")]
        private IWebElement _localAuthorityStartDateTextBox;

        [FindsBy(How = How.Name, Using = "StaffReasonForLeavingString")]
        private IWebElement _reasonForLeaving;

        [FindsBy(How = How.Name, Using = "PreviousEmployer")]
        private IWebElement _previousEmployerTextBox;

        [FindsBy(How = How.Name, Using = "NextEmployer")]
        private IWebElement _nextEmployerTextBox;

        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _noteTextBox;

        [FindsBy(How = How.CssSelector, Using = "div.validation-summary-errors>ul>li")]
        private IList<IWebElement> _validationErrors;

        public string DOA
        {
            set { _doaTextBox.SetText(value); }
            get { return _doaTextBox.GetDateTime(); }
        }

        public string DOL
        {
            get { return _dolTextBox.GetDateTime(); }
        }

        public string ContinuousServiceStartDate
        {
            set { _continuousServiceStartDateTextBox.SetDateTime(value); }
            get { return _continuousServiceStartDateTextBox.GetDateTime(); }
        }

        public string ReasonForLeaving
        {
            get { return _reasonForLeaving.GetText(); }
        }

        public string LocalAuthorityStartDate
        {
            set { _localAuthorityStartDateTextBox.SetDateTime(value); }
            get { return _localAuthorityStartDateTextBox.GetDateTime(); }
        }

        public string PreviousEmployer
        {
            set { _previousEmployerTextBox.SetText(value); }
            get { return _previousEmployerTextBox.GetText(); }
        }

        public string NextEmployer
        {
            get { return _nextEmployerTextBox.GetText(); }
        }

        public string Note
        {
            set { _noteTextBox.SetText(value); }
            get { return _noteTextBox.GetText(); }
        }

        public IEnumerable<string> ValidationErrors
        {
            get { return _validationErrors.Select(x => x.Text); }
        }

        #endregion
    }
}
