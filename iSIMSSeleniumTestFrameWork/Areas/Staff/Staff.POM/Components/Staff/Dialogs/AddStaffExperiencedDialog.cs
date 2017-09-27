using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff
{
    public class AddStaffExperiencedDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_experience_dialog"); }
        }        

        #region Page propertise

        [FindsBy(How = How.Name, Using = "Employer")]
        private  IWebElement _employer;

        [FindsBy(How = How.Name, Using = "StaffRole")]
        private IWebElement _staffRole;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.Name, Using = "DateOfArrival")]
        private IWebElement _dateOfArrival;

        [FindsBy(How = How.Name, Using = "DateOfLeaving")]
        private IWebElement _dateOfLeaving;

        public string EmployerField
        {
            get { return _employer.GetText(); }
            set { _employer.SetText(value); }
        }

        public string StaffRole
        {
            get { return _staffRole.GetText(); }
            set { _staffRole.SetText(value); }
        }

        public string DateOfArrivalField
        {
            get { return _dateOfArrival.GetDateTime(); }
            set { _dateOfArrival.SetDateTime(value); }
        }
        
        public string DateOfLeavingField
        {
            get { return _dateOfLeaving.GetDateTime(); }
            set { _dateOfLeaving.SetDateTime(value); }
        }

        #endregion

        #region Page action

        public StaffRecordPage AddStaffExperience()
        {
            _okButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new StaffRecordPage();
        }

        #endregion

    }
}
