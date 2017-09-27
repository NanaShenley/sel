using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class HistoryAttendanceModeDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            //-maintenance-grid-id
            get { return SimsBy.CssSelector("[data-maintenance-container='LearnerAttendanceModes']"); }
           // get { return SimsBy.CssSelector("[data-section-id='LearnerAttendanceModes-grid-editor-dialog'][aria-hidden='false']"); }
        }

        #region Attendance Mode Membership Grid
        public GridComponent<AttendanceModeMembership> AttendanceModes
        {
            get
            {
                GridComponent<AttendanceModeMembership> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AttendanceModeMembership>(By.CssSelector("[data-maintenance-container='LearnerAttendanceModes']"), DialogIdentifier);

                });
                return returnValue;
            }
        }

        public class AttendanceModeMembership : GridRow
        {

            //[FindsBy(How = How.CssSelector, Using = "[name$='AttendanceMode.dropdownImitator']")]
            [FindsBy(How = How.CssSelector, Using = "[class='form-control select-imitator']")]
            private IWebElement _attendanceMode;

            public string AttendanceMode
            {
                get
                {
                    return _attendanceMode.GetValue();
                }
                set
                {
                   _attendanceMode.EnterForDropDown(value); 
                }
            }
        }
        #endregion

        public void SelectAttendanceMode()
        {

            IWebElement okButton = SeleniumHelper.FindElement(SimsBy.CssSelector("[class='history-editor is-active']"));

            var dropdown = okButton.FindElement(SimsBy.CssSelector("[class='form-control select-imitator']"));

            dropdown.Click();
            IWebElement lstCodes = SeleniumHelper.FindElement(SimsBy.Id("s2id_autogen1_search"));
            lstCodes.SetText("AM only");
            lstCodes.SendKeys(Keys.Enter);

        }

        #region Page actions
        public PupilRecordPage CloseAttendanceModeOK()
        {
           
            IWebElement okButton = SeleniumHelper.FindElement(SimsBy.CssSelector("[data-automation-id='LearnerAttendanceModes_grid_editor_dialog_button_close']"));

            okButton.ClickByJS();
            return new PupilRecordPage();
        }
        #endregion

    }
}
