using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class EditContactDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("pupil_contact_record_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "LearnerContact.Surname")]
        private IWebElement _surNameTextBox;
      
        public string SurName
        {
            get { return _surNameTextBox.GetText(); }
            set { _surNameTextBox.SetText(value); }
        }

        #endregion

        public GridComponent<PupilContactTelephoneRow> PupilContactTelephones
        {
            get
            {
                return new GridComponent<PupilContactTelephoneRow>(By.CssSelector("[data-maintenance-container='LearnerContact.LearnerContactTelephones']"), ComponentIdentifier);
            }
        }

        public class PupilContactTelephoneRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='TelephoneNumber']")]
            private IWebElement _TelephoneNumber;

            public string TelephoneNumber
            {
                set { _TelephoneNumber.SetText(value); }
                get { return _TelephoneNumber.GetValue(); }
            }
        }

        public GridComponent<PupilContactEmailRow> PupilContactEmails
        {
            get
            {
                return new GridComponent<PupilContactEmailRow>(By.CssSelector("[data-maintenance-container='LearnerContact.LearnerContactEmails']"), ComponentIdentifier);
            }
        }

        public class PupilContactEmailRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='EmailAddress']")]
            private IWebElement _EmailAddress;

            public string EmailAddress
            {
                set { _EmailAddress.SetText(value); }
                get { return _EmailAddress.GetValue(); }
            }
        }
    }
}
