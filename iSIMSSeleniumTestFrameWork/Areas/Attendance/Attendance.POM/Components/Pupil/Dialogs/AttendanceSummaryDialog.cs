using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class AttendanceSummaryDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_attendance_summary_button']")]
        private IWebElement _addAttendanceSummary;

        public class AttendanceSummary
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Year']")]
            private IWebElement _year;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _blankCell;

            public string Year
            {
                set
                {
                    _year.SetText(value);
                    Retry.Do(_blankCell.Click);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _year.GetValue();
                }
            }
        }

        public GridComponent<AttendanceSummary> PreviousSchoolAttendanceSummary
        {
            get
            {
                GridComponent<AttendanceSummary> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AttendanceSummary>(By.CssSelector("[data-maintenance-container='LearnerPreviousSchoolAttendanceSummary']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }
        #endregion

        #region Public actions
        public AddAttendanceSummaryDialog OpenAddAttendanceSummaryDialog()
        {
            _addAttendanceSummary.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddAttendanceSummaryDialog();
        }

        #endregion
    }
}
