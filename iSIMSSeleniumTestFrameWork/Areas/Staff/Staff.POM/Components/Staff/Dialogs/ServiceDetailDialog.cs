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
    public class ServiceDetailDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("add_new_staff_wizard"); }
        }        
        
        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_record_button']")]
        private IWebElement _createRecordButton;
        
        [FindsBy(How = How.Name, Using = "DateOfArrival")]
        private IWebElement _dateOfArrivalTextBox;

        [FindsBy(How = How.Name, Using = "ContinuousServiceStartDate")]
        private IWebElement _continuousServiceStartDateTextBox;

        [FindsBy(How = How.Name, Using = "LocalAuthorityStartDate")]
        private IWebElement _localAuthorityStartDateTextBox;

        [FindsBy(How = How.Name, Using = "PreviousEmployer")]
        private IWebElement _previousEmployerTextBox;

        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _noteTextBox;

        // Support for issue Cannot open staff record
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='yes,_open_staff_record_button']")]
        private IWebElement _OpenExistRecordLink;

        public string DateOfArrival
        {
            set { _dateOfArrivalTextBox.SetText(value); }
            get { return _dateOfArrivalTextBox.GetValue(); }
        }

        public string ContinuousServiceStartDate
        {
            set { _continuousServiceStartDateTextBox.SetText(value); }
            get { return _continuousServiceStartDateTextBox.GetValue(); }
        }

        public string LocalAuthorityStartDate
        {
            set { _localAuthorityStartDateTextBox.SetText(value); }
            get { return _localAuthorityStartDateTextBox.GetValue(); }
        }

        public string PreviousEmployer
        {
            set { _previousEmployerTextBox.SetText(value); }
            get { return _previousEmployerTextBox.GetText(); }
        }

        public string Note
        {
            set { _noteTextBox.SetText(value); }
            get { return _noteTextBox.GetText(); }
        }

        #endregion

        #region Public actions

        public StaffRecordPage SaveRecord()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("save_record_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("save_record_button")));
            AutomationSugar.WaitForAjaxCompletion();
            return new StaffRecordPage();
        }
        
        public StaffRecordPage CreateRecord()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_record_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_record_button")));
            AutomationSugar.WaitForAjaxCompletion();
            return new StaffRecordPage();
        }

        /// <summary>
        /// This function used to handle issue.
        /// </summary>
        public void OpenExistRecord()
        {
            Retry.Do(_OpenExistRecordLink.ClickByJS);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        #endregion
    }
}
