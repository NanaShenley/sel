using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Admission
{
    public class RegistrationDetailsDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("add_new_application_wizard"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "AdmissionGroup.dropdownImitator")]
        private IWebElement _admissionGroupDropDown;

        [FindsBy(How = How.Name, Using = "EnrolmentStatus.dropdownImitator")]
        private IWebElement _enrolmentStatusDropDown;

        [FindsBy(How = How.Name, Using = "PrimaryClass.dropdownImitator")]
        private IWebElement _classDropDown;

        [FindsBy(How = How.Name, Using = "AttendanceMode.dropdownImitator")]
        private IWebElement _attendanceModeDropDown;

        [FindsBy(How = How.Name, Using = "BoarderStatus.dropdownImitator")]
        private IWebElement _boarderStatusDropDown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_record_button']")]
        private IWebElement _creatRecordButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='back_button']")]
        private IWebElement _backButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        #endregion

        #region Actions

        public string AdmissionGroup
        {
            set
            {
                _admissionGroupDropDown.EnterForDropDown(value);
            }
            get { return _admissionGroupDropDown.GetValue(); }
        }
        public string EnrolmentStatus
        {
            set { _enrolmentStatusDropDown.EnterForDropDown(value); }
            get { return _enrolmentStatusDropDown.GetValue(); }
        }

        public string Class
        {
            set { _classDropDown.EnterForDropDown(value); }
            get { return _classDropDown.GetValue(); }
        }
        public string AttendanceMode
        {
            set { _attendanceModeDropDown.EnterForDropDown(value); }
            get { return _attendanceModeDropDown.GetValue(); }
        }
        public string BoarderStatus
        {
            set { _boarderStatusDropDown.EnterForDropDown(value); }
            get { return _boarderStatusDropDown.GetValue(); }
        }


        public ApplicationPage CreateRecord()
        {
            if (_creatRecordButton.IsExist())
            {
                _creatRecordButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new ApplicationPage();
            Refresh();
        }

        public ApplicationPage Cancel()
        {
            if (_cancelButton.IsExist())
            {
                _cancelButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new ApplicationPage();
            Refresh();
        }
        #endregion
    }
}
