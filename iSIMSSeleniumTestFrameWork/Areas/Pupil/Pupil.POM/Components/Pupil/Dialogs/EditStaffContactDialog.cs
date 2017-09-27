using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class EditStaffContactDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_contact_relationship_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _Surname;

        public string Surename
        {
            get { return _Surname.GetValue(); }
            set{ _Surname.SetText(value);}
        }

        #endregion

        public GridComponent<StaffContactTelephoneRow> StaffContactTelephones
        {
            get
            {
                return new GridComponent<StaffContactTelephoneRow>(By.CssSelector("[data-maintenance-container='StaffContact.StaffContactTelephones']"), ComponentIdentifier);
            }
        }

        public class StaffContactTelephoneRow : GridRow
        { 
            [FindsBy(How = How.CssSelector, Using = "[name$='TelephoneNumber']")]
            private IWebElement _TelephoneNumber;

            public string TelephoneNumber
            {
                set { _TelephoneNumber.SetText(value); }
                get { return _TelephoneNumber.GetValue(); }
            }
        }

        public GridComponent<StaffContactEmailRow> StaffContactEmails
        {
            get
            {
                return new GridComponent<StaffContactEmailRow>(By.CssSelector("[data-maintenance-container='StaffContact.StaffContactEmails']"), ComponentIdentifier);
            }
        }

        public class StaffContactEmailRow : GridRow
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
