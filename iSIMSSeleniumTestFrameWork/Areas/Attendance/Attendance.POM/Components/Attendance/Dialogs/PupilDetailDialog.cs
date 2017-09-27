using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;


namespace POM.Components.Attendance
{
    public class PupilDetailDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("popover-custom-id"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='popover-card-id']")]
        private IWebElement _pupilName;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='popover-card-id']")]
        private IWebElement _pupilDOB;

        public string PupilName
        {
            get { return _pupilName.GetText(); }
        }

        public string PupilDOB
        {
            get
            {
                return _pupilDOB.GetText();
            }
        }

        public GridComponent<Absences> AbsenceTable
        {
            get
            {
                GridComponent<Absences> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Absences>(By.CssSelector("[data-automation-id='section_menu_Recent Absences']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class Absences
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='.AbsenceDate']")]
            private IWebElement _date;

            [FindsBy(How = How.CssSelector, Using = "[name$='.AbsenceReason']")]
            private IWebElement _reason;
        }

        #endregion

        #region Action

        public bool IsContactDetailDisplay()
        {
            try
            {
                IWebElement _contactDetailTable = SeleniumHelper.FindElement(SimsBy.CssSelector("[data-automation-id='section_menu_Pupil Contact']"));
                _contactDetailTable.Click();
                return _contactDetailTable.IsExist();
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

    }
}
