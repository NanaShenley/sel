using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;

using System.Linq;

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
        private IWebElement _descriptionTextBox;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateTextBox;

        [FindsBy(How = How.Name, Using = "StartSession.dropdownImitator")]
        private IWebElement _sessionStartDropDown;

        [FindsBy(How = How.Name, Using = "EndSession.dropdownImitator")]
        private IWebElement _sessionEndDropDown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pupils_button']")]
        private IWebElement _addPupilsLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        public string Description
        {
            set { _descriptionTextBox.SetText(value); }
            get { return _descriptionTextBox.GetValue(); }
        }

        public string StartDate
        {
            set { _startDateTextBox.SetDateTime(value); }
            get { return _startDateTextBox.GetDateTime(); }
        }

        public string EndDate
        {
            set { _endDateTextBox.SetDateTime(value); }
            get { return _endDateTextBox.GetDateTime(); }
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

       
        #endregion
    }
}
