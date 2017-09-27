using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;
using System.Collections.Generic;
using System.Linq;
using WebDriverRunner.webdriver;

namespace POM.Components.Attendance
{
    public class ExceptionalCircumstancesDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='detail']"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Description")]
        public IWebElement descriptionTextBox;

        [FindsBy(How = How.Name, Using = "StartDate")]
        public IWebElement startDateTextBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        public IWebElement endDateTextBox;

        [FindsBy(How = How.Name, Using = "StartSession.dropdownImitator")]
        private IWebElement _sessionStartDropDown;

        [FindsBy(How = How.Name, Using = "EndSession.dropdownImitator")]
        private IWebElement _sessionEndDropDown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pupils_button']")]
        private IWebElement _addPupilsLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        private IWebElement validationWarning;

        public string Description
        {
            set { descriptionTextBox.SetText(value); }
            get { return descriptionTextBox.GetValue(); }
        }

        public string StartDate
        {
            set { startDateTextBox.SetDateTime(value); }
            get { return startDateTextBox.GetDateTime(); }
        }

        public string EndDate
        {
            set { endDateTextBox.SetDateTime(value); }
            get { return endDateTextBox.GetDateTime(); }
        }

        public string SessionStart
        {
            set { _sessionStartDropDown.EnterForDropDown(value); }
            get { return _sessionStartDropDown.GetValue(); }
        }

        public string SessionEnd
        {
            set { _sessionEndDropDown.EnterForDropDown(value); }
            get { return _sessionEndDropDown.GetValue(); }
        }

        public AddPupilsDialogTriplet AddPupils()
        {
            _addPupilsLink.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            return new AddPupilsDialogTriplet();
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

                var warningConfirmDialog = new WarningConfirmationDialog();
                warningConfirmDialog.ConfirmDelete();
            }
        }

        public bool DeleteDialogDisappeared()
        {
            //WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.ExceptionalCircumstanceElements.ContinueWithDelete);
            //Thread.Sleep(2000);
            IReadOnlyCollection<IWebElement> list = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='confirm_delete_dialog']"));
            bool val = (list.Count == 0);
            return val;
        }

        public bool IsDisplayedValidationWarning()
        {
            bool value = validationWarning.Displayed;
            return value;
        }


        #endregion
    }
}
