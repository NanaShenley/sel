using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Pupil;
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

        [FindsBy(How = How.CssSelector, Using = ".media-heading .display-dl dd")]
        private IWebElement _pupilName;

        [FindsBy(How = How.CssSelector, Using = ".display-wrap:nth-child(3) time")]
        private IWebElement _pupilDOB;

        public string PupilName
        {
            get { return _pupilName.GetText(); }
        }

        public string PupilDOB
        {
            get
            {
                //Find an element which have date time format
                IWebElement dateTimeElement;
                try
                {
                    dateTimeElement = SeleniumHelper.FindElement(By.CssSelector("[data-date-validator-format]"));
                }
                catch (Exception)
                {
                    dateTimeElement = null;
                }

                string dateOfBirth;
                if (dateTimeElement == null)
                {
                    dateOfBirth = _pupilDOB.GetText();
                }
                else
                {
                    //Format value with format
                    string dateFormat = dateTimeElement.GetAttribute("data-date-validator-format");
                    string value = _pupilDOB.GetAttribute("datetime");
                    value = value.Split(' ')[0];
                    dateOfBirth = SeleniumHelper.Format(value, dateFormat);
                }
                return dateOfBirth;
            }
        }

        public GridComponent<Absences> AbsenceTable
        {
            get
            {
                GridComponent<Absences> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Absences>(By.CssSelector("[data-maintenance-container='RecentAbsenceInformation']"), DialogIdentifier);
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
                IWebElement _contactDetailTable = SeleniumHelper.FindElement(SimsBy.CssSelector("[data-maintenance-container='LearnerContactInformations']"));
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
