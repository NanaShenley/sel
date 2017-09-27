using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Collections.Generic;
using POM.Components.Pupil;

namespace POM.Components.Attendance
{
    public class AttendancePatternDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("task_menu_section_attendance_AttendancePattern-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextbox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='apply_pattern_button']")]
        private IWebElement _applyButton;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='close_button']"))]
        private IWebElement _closeButton;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='add_pupils_button']"))]
        private IWebElement _addPupil;

        [FindsBy(How = How.CssSelector, Using = ("[view_id='cxgridAttendancePatternLearner'] input"))]
        private IWebElement _selectCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[view_id='cxgridAttendancePatternLearner'] .webix_ss_body")]
        private IWebElement _selectPupilTable;

        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        private IWebElement _academicYearDropdown;

        public string AcademicYear
        {
            set { _academicYearDropdown.EnterForDropDown(value); }
            get { return _academicYearDropdown.GetValue(); }
        }

        public string StartDate
        {
            set { _startDateTextbox.SetDateTime(value); }
        }

        public string EndDate
        {
            set { _endDateTextbox.SetDateTime(value); }
        }

        public bool SelectPupil
        {
            set { _selectCheckBox.Set(value); }
            get { return _selectCheckBox.IsChecked(); }
        }

        public WebixComponent<WebixCell> SelectPupilsTable
        {
            get { return new WebixComponent<WebixCell>(_selectPupilTable); }
        }

        public bool IsPreserve
        {
            set
            {
                string javascript = "document.getElementsByName('PreserveOverwrite')[0].checked = '{0}';";
                SeleniumHelper.ExecuteJavascript(String.Format(javascript, value));
            }
        }

        
        public bool IsOverwrite
        {
            set
            {
                string javascript = "document.getElementsByName('PreserveOverwrite')[1].checked = '{0}';";
                SeleniumHelper.ExecuteJavascript(String.Format(javascript, value));
            }
        }

        public class AttendancePattern
        {
            [FindsBy(How = How.Name, Using = "MonAMSession.dropdownImitator")]
            private IWebElement _monAM;

            [FindsBy(How = How.Name, Using = "MonPMSession.dropdownImitator")]
            private IWebElement _monPM;

            [FindsBy(How = How.Name, Using = "TuesAMSession.dropdownImitator")]
            private IWebElement _tueAM;

            [FindsBy(How = How.Name, Using = "TuesPMSession.dropdownImitator")]
            private IWebElement _tuePM;

            [FindsBy(How = How.Name, Using = "WedAMSession.dropdownImitator")]
            private IWebElement _wedAM;

            [FindsBy(How = How.Name, Using = "WedPMSession.dropdownImitator")]
            private IWebElement _wedPM;

            [FindsBy(How = How.Name, Using = "ThursAMSession.dropdownImitator")]
            private IWebElement _thuAM;

            [FindsBy(How = How.Name, Using = "ThursPMSession.dropdownImitator")]
            private IWebElement _thuPM;

            [FindsBy(How = How.Name, Using = "FriAMSession.dropdownImitator")]
            private IWebElement _friAM;

            [FindsBy(How = How.Name, Using = "FriPMSession.dropdownImitator")]
            private IWebElement _friPM;

            public string MonAM
            {
                set { _monAM.EnterForDropDown(value); }
                get { return _monAM.GetValue(); }
            }

            public string MonPM
            {
                set { _monPM.EnterForDropDown(value); }
                get { return _monPM.GetValue(); }
            }

            public string TueAM
            {
                set { _tueAM.EnterForDropDown(value); }
                get { return _tueAM.GetValue(); }
            }

            public string TuePM
            {
                set { _tuePM.EnterForDropDown(value); }
                get { return _tuePM.GetValue(); }
            }

            public string WedAM
            {
                set { _wedAM.EnterForDropDown(value); }
                get { return _wedAM.GetValue(); }
            }

            public string WedPM
            {
                set { _wedPM.EnterForDropDown(value); }
                get { return _wedPM.GetValue(); }
            }

            public string ThuAM
            {
                set { _thuAM.EnterForDropDown(value); }
                get { return _thuAM.GetValue(); }
            }

            public string ThuPM
            {
                set { _thuPM.EnterForDropDown(value); }
                get { return _thuPM.GetValue(); }
            }

            public string FriAM
            {
                set { _friAM.EnterForDropDown(value); }
                get { return _friAM.GetValue(); }
            }

            public string FriPM
            {
                set { _friPM.EnterForDropDown(value); }
                get { return _friPM.GetValue(); }
            }
        }

        public Grid2Component<AttendancePattern> Table
        {
            get
            {
                Grid2Component<AttendancePattern> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new Grid2Component<AttendancePattern>(By.CssSelector("[data-automation-id='attendance_pattern_grid']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        #endregion

        #region Page actions

        public POM.Components.Common.ConfirmRequiredDialog ClickApply()
        {
            _applyButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new POM.Components.Common.ConfirmRequiredDialog();
        }

        public AddPupilsDialogTriplet AddPupil()
        {
            _addPupil.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddPupilsDialogTriplet();
        }

        public PupilRecordPage ClickClose()
        {
            _closeButton.Click();

            return new PupilRecordPage();
          
        }
        public void ClosePatternDialog()
        {
            _closeButton.Click();

        }

        #endregion
    }
}
