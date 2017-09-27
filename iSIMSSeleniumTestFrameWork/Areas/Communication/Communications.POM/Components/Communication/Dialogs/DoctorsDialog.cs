using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using SeSugar.Automation;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;

namespace POM.Components.Communication
{
    public class DoctorsDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_an_address_button']")]
        private IWebElement _addaAdditionalAddressesLink;

        [FindsBy(How = How.Name, Using = "Title.dropdownImitator")]
        private IWebElement _titleDropdown;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _foreNameTextBox;

        [FindsBy(How = How.Name, Using = "MiddleName")]
        private IWebElement _middleNameTextBox;

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _sureNameTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        #endregion

        #region Actions

        public string Title
        {
            set { _titleDropdown.EnterForDropDown(value); }
            get { return _titleDropdown.GetValue(); }
        }
        public string ForeName
        {
            set { _foreNameTextBox.SetText(value); }
            get { return _foreNameTextBox.GetValue(); }
        }

        public string MiddleName
        {
            set { _middleNameTextBox.SetText(value); }
            get { return _middleNameTextBox.GetValue(); }
        }

        public string SureName
        {
            set { _sureNameTextBox.SetText(value); }
            get { return _sureNameTextBox.GetValue(); }
        }

        public MedicalPracticePage OK()
        {
            if (_okButton.IsExist())
            {
                _okButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            }
            return new MedicalPracticePage();

        }

        public MedicalPracticePage Cancel()
        {
            if (_cancelButton.IsExist())
            {
                _cancelButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new MedicalPracticePage();
        }

        public void SelectAddressesTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Addresses");
        }

        public AddDoctorAddressDialog ClickAddanAdditionalAddressLink()
        {
            _addaAdditionalAddressesLink.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AddDoctorAddressDialog();
        }

        public void ClickAddAddress()
        {
            AutomationSugar.WaitFor("add_an_address_button");
            AutomationSugar.ClickOn("add_an_address_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        public GridComponent<DoctorAddressRow> DoctorAddressTable
        {

            get
            {
                GridComponent<DoctorAddressRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<DoctorAddressRow>(By.CssSelector("[data-maintenance-container='Addresses']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class DoctorAddressRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='DoctorAddressesAddress']")]
            private IWebElement _address;

            [FindsBy(How = How.CssSelector, Using = "[name$='AddressStatus']")]
            private IWebElement _addressStatus;

            [FindsBy(How = How.CssSelector, Using = "[name$='AddressType.dropdownImitator']")]
            private IWebElement _addressType;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            [FindsBy(How = How.CssSelector, Using = "[title='Actions Dropdown']")]
            private IWebElement _actions;

            [FindsBy(How = How.CssSelector, Using = "[title='Edit Address']")]
            private IWebElement _edit;

            [FindsBy(How = How.CssSelector, Using = "[title='Move address']")]
            private IWebElement _move;

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

        #endregion
    }
}