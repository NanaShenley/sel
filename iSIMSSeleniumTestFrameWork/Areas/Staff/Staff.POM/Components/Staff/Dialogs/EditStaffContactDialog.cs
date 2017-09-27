using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
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
        public GridComponent<StaffContactAddressRow> StaffContactAddressTable
        {

            get
            {
                GridComponent<StaffContactAddressRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffContactAddressRow>(By.CssSelector("[data-maintenance-container='StaffContact.ContactAddresses']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class StaffContactAddressRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='LearnerAddressesAddress']")]
            private IWebElement _address;

            [FindsBy(How = How.CssSelector, Using = "[name$='AddressStatus']")]
            private IWebElement _addressStatus;

            [FindsBy(How = How.CssSelector, Using = "[name$='AddressType.dropdownImitator']")]
            private IWebElement _addressType;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            public string Address
            {
                get { return _address.GetText(); }
            }

            public string AddressStatus
            {
                get { return _addressStatus.GetValue(); }
            }

            public string AddressType
            {
                set { _addressType.EnterForDropDown(value); }
                get { return _addressType.GetAttribute("value"); }
            }

            public string StartDate
            {
                set { _startDate.SetText(value); }
                get { return _startDate.GetAttribute("value"); }
            }

            public string EndDate
            {
                set { _endDate.SetText(value); }
                get { return _endDate.GetAttribute("value"); }
            }

            public void ClickEditStaffContactAddress()
            {
                AutomationSugar.WaitFor("Action_Dropdown");
                AutomationSugar.ClickOn("Action_Dropdown");
                AutomationSugar.WaitForAjaxCompletion();

                AutomationSugar.WaitFor("Edit_full_record_Action");
                AutomationSugar.ClickOn("Edit_full_record_Action");
                AutomationSugar.WaitForAjaxCompletion();
            }

            public void ClickMoveStaffContactAddress()
            {
                AutomationSugar.WaitFor("Action_Dropdown");
                AutomationSugar.ClickOn("Action_Dropdown");
                AutomationSugar.WaitForAjaxCompletion();

                AutomationSugar.WaitFor("Move_address_Action");
                AutomationSugar.ClickOn("Move_address_Action");
                AutomationSugar.WaitForAjaxCompletion();
            }
        }
    }
}
