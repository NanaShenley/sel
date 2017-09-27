using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POM.Components.Attendance
{
    public class AttendanceSettingsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _beforeDOBDatePicker;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _afterDOBDatePicker;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        public string BeforeDateOfBirth
        {
            set { _beforeDOBDatePicker.SetDateTime(value); }
            get { return _beforeDOBDatePicker.GetDateTime(); }
        }

        public string AfterDateOfBirth
        {
            set { _afterDOBDatePicker.SetDateTime(value); }
            get { return _afterDOBDatePicker.GetDateTime(); }
        }

        #endregion

        #region Action

        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool IsSuccessMessageDisplay()
        {
            return _successMessage.IsElementDisplayed();
        } 

        #endregion
    }
}
