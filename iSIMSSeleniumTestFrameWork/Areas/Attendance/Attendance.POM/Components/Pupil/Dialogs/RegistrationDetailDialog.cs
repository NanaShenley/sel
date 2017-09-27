using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class RegistrationDetailDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-editableData"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_record_button']")]
        private IWebElement _createRecordButton;

        [FindsBy(How = How.Name, Using = "DateOfAdmission")]
        private IWebElement _dateOfAdmissionTextBox;


        [FindsBy(How = How.Name, Using = "EnrolmentStatus.dropdownImitator")]
        private IWebElement _enrolmentStatusDropdown;


        [FindsBy(How = How.Name, Using = "YearGroup.dropdownImitator")]
        private IWebElement _yearGroupDropdown;

        [FindsBy(How = How.Name, Using = "PrimaryClass.dropdownImitator")]
        private IWebElement _classNameDropdown;

        [FindsBy(How = How.Name, Using = "AttendanceMode.dropdownImitator")]
        private IWebElement _attendanceModeDropdown;


        [FindsBy(How = How.Name, Using = "BoarderStatus.dropdownImitator")]
        private IWebElement _borderField;

        public string BorderStatus
        {
            set { _borderField.EnterForDropDown(value); }
            get { return _borderField.GetValue(); }
        }

        public string DateOfAdmission
        {
            set { _dateOfAdmissionTextBox.SetDateTime(value); }
            get { return _dateOfAdmissionTextBox.GetDateTime(); }
        }


        public string EnrolmentStatus
        {
            set { _enrolmentStatusDropdown.EnterForDropDown(value); }
            get { return _enrolmentStatusDropdown.GetText(); }
        }

        public string YearGroup
        {
            set { _yearGroupDropdown.EnterForDropDown(value); }
            get { return _yearGroupDropdown.GetText(); }
        }

        public string ClassName
        {
            set { _classNameDropdown.EnterForDropDown(value); }
            get { return _classNameDropdown.GetText(); }
        }

        public string AttendanceMode
        {
            set { _attendanceModeDropdown.EnterForDropDown(value); }
            get { return _attendanceModeDropdown.GetText(); }
        }

        #endregion

        #region Public actions

        public PupilRecordPage CreateRecord()
        {
            _createRecordButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new PupilRecordPage();
        }

        #endregion
    }
}
