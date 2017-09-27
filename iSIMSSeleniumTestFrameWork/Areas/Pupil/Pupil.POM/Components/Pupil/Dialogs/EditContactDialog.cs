using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;

namespace POM.Components.Pupil
{
    public class EditContactDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("pupil_contact_record_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "LearnerContact.Title.dropdownImitator")]
        private IWebElement _titleDropdown;

        [FindsBy(How = How.Name, Using = "LearnerContact.Forename")]
        private IWebElement _foreNameTextBox;

        [FindsBy(How = How.Name, Using = "LearnerContact.MiddleName")]
        private IWebElement _middleNameTextBox;

        [FindsBy(How = How.Name, Using = "LearnerContact.Surname")]
        private IWebElement _surNameTextBox;

        [FindsBy(How = How.Name, Using = "LearnerContact.GenerateSalutation")]
        private IWebElement _salutationSectionButton;

        [FindsBy(How = How.Name, Using = "LearnerContact.GenerateAddressee")]
        private IWebElement _addresseeSectionButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Addresses']")]
        private IWebElement _addressLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_an_address_button']")]
        private IWebElement _addAddressButton;

        public string Title
        {
            get { return _titleDropdown.GetAttribute("value"); }
            set { _titleDropdown.EnterForDropDown(value); }
        }

        public string ForeName
        {
            get { return _foreNameTextBox.GetText(); }
            set { _foreNameTextBox.SetText(value); }
        }

        public string MiddleName
        {
            get { return _middleNameTextBox.GetText(); }
            set { _middleNameTextBox.SetText(value); }
        }

        public string SurName
        {
            get { return _surNameTextBox.GetText(); }
            set { _surNameTextBox.SetText(value); }
        }

        public class AddressRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='LearnerAddressesAddress']")]
            private IWebElement _address;

            [FindsBy(How = How.CssSelector, Using = "[name$='AddressStatus']")]
            private IWebElement _status;

            [FindsBy(How = How.CssSelector, Using = "[name$='AddressType.dropdownImitator']")]
            private IWebElement _type;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDay;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDay;

            [FindsBy(How = How.CssSelector, Using = "[title='Actions Dropdown']")]
            private IWebElement _actions;

            [FindsBy(How = How.CssSelector, Using = "[title='Edit Address']")]
            private IWebElement _edit;

            [FindsBy(How = How.CssSelector, Using = "[title='Move address']")]
            private IWebElement _move;


            public string Address
            {
                get { return _address.Text; }
            }

            public string AddressStatus
            {
                get { return _status.GetAttribute("value"); }
            }

            public string AddressType
            {
                set { _type.EnterForDropDown(value); }
                get { return _type.GetAttribute("value"); }
            }

            public string StartDate
            {
                set { _startDay.SetText(value); }
                get { return _startDay.GetAttribute("value"); }
            }

            public string EndDate
            {
                set { _endDay.SetText(value); }
                get { return _endDay.GetAttribute("value"); }
            }

            public void Edit()
            {
                _actions.ClickByJS();
                _edit.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }

            public void Move()
            {
                _actions.ClickByJS();
                _move.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
        }

        public GridComponent<AddressRow> AddressTable
        {
            get
            {
                GridComponent<AddressRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AddressRow>(By.CssSelector("[data-maintenance-container='LearnerContact.ContactAddresses']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }


        #endregion

        #region Action
        public void ClickSalutationSection()
        {
            _salutationSectionButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
        }

        public void ClickAddresseeSection()
        {
            _addresseeSectionButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
        }

        public void ScrollToAddressPanel()
        {
            this.ExpandAccordion(t => t._addressLink);
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='LearnerContact.ContactAddresses']"));
        }

        public void ClickAddAddress()
        {
            _addAddressButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        #endregion

        public GridComponent<LearnerContactTelephoneRow> LearnerContactTelephones
        {
            get
            {
                return new GridComponent<LearnerContactTelephoneRow>(By.CssSelector("[data-maintenance-container='LearnerContact.LearnerContactTelephones']"), ComponentIdentifier);
            }
        }

        public class LearnerContactTelephoneRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='TelephoneNumber']")]
            private IWebElement _TelephoneNumber;

            public string TelephoneNumber
            {
                set { _TelephoneNumber.SetText(value, null); }
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
                set { _EmailAddress.SetText(value, null); }
                get { return _EmailAddress.GetValue(); }
            }
        }
    }
}
