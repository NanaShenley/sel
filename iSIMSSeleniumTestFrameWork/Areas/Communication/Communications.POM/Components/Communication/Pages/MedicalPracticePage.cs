using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Common;
using POM.Base;
using POM.Helper;



namespace POM.Components.Communication
{
    public class MedicalPracticePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='detail']"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[title='Delete address detail']")]
        private IWebElement _deleteAddessButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _deleteAddessAndContinueButton;

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _practiceNameTextBox;

        [FindsBy(How = How.Name, Using = "AddressesAddress")]
        private IWebElement _medicalPracticeAddress;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='MedicalPracticeTelephones']")]
        private IWebElement _telephoneNumbersTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_doctor_button']")]
        private IWebElement _addDoctorButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_button']")]
        private IWebElement _addNewButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit_button']")]
        private IWebElement _editNewButton;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='Doctors']")]
        private IWebElement _doctorsTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _statusMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Basic Details']")]
        private IWebElement _basicDetailAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Contact Details']")]
        private IWebElement _contactAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Associated Doctors']")]
        private IWebElement _associatedAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Documents']")]
        private IWebElement _documentAccordion;

        public string PracticeName
        {
            set
            {
                _practiceNameTextBox.SetText(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get { return _practiceNameTextBox.GetValue(); }
        }
        public string MedicalPracticeAddresss
        {
            set { _medicalPracticeAddress.SetText(value); }
            get { return _medicalPracticeAddress.GetValue(); }
        }

        #endregion

        #region Grid

        public GridComponent<TelephoneNumbersTable> TelephoneNumbers
        {
            get
            {
                GridComponent<TelephoneNumbersTable> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TelephoneNumbersTable>(By.CssSelector("[data-maintenance-container='MedicalPracticeTelephones']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class TelephoneNumbersTable : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$=TelephoneNumber]")]
            private IWebElement _telephoneNumbersTextBox;

            [FindsBy(How = How.CssSelector, Using = "[id$='UseForTextMessages']")]
            private IWebElement _aMSCheckBox;

            [FindsBy(How = How.CssSelector, Using = "[id$='__LocationType_dropdownImitator']")]
            private IWebElement _locationDropDown;

            [FindsBy(How = How.CssSelector, Using = "[id$='IsMainTelephone']")]
            private IWebElement _mainNumberCheckBox;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
            private IWebElement _noteButton;
            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _blankCell;

            public string TelephoneNumbers
            {
                set
                {
                    _telephoneNumbersTextBox.SetAttribute("value", "");
                    _telephoneNumbersTextBox.SetText(value);
                    _telephoneNumbersTextBox.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _telephoneNumbersTextBox.GetAttribute("value");
                }
            }
            public bool AMS
            {
                set { _aMSCheckBox.Set(value); }
                get { return _aMSCheckBox.IsChecked(); }
            }

            public string Location
            {
                set { _locationDropDown.EnterForDropDown(value); }
                get { return _locationDropDown.GetValue(); }
            }

            public bool MainNumber
            {
                set { _mainNumberCheckBox.Set(value); }
                get { return _mainNumberCheckBox.IsChecked(); }
            }
        }


        public GridComponent<DoctorsTable> Doctors
        {
            get
            {
                GridComponent<DoctorsTable> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<DoctorsTable>(By.CssSelector("[data-maintenance-container='Doctors']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class DoctorsTable : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$=DoctorsForename]")]
            private IWebElement _nameTextBox;


            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _editLink;
            public string Name
            {
                set { _nameTextBox.SetText(value); }
                get { return _nameTextBox.GetValue(); }
            }

            public void ClickEdit()
            {

                _editLink.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }

        }


        #endregion

        #region Actions

        public DoctorsDialog AddDoctor()
        {
            _addDoctorButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            return new DoctorsDialog();
        }

        public DeleteConfirmationDialog Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            return new DeleteConfirmationDialog();
        }

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                   }


        public bool IsSuccessMessageIsDisplayed()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_success']"));
            return SeleniumHelper.DoesWebElementExist(SimsBy.CssSelector("[data-automation-id='status_success']"));
        } 

        public void ScrollToBasicDetail()
        {
            if (_basicDetailAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _basicDetailAccordion.Click();
            }
            else
            {
                _basicDetailAccordion.ClickByJS();
                Wait.WaitLoading();
                _basicDetailAccordion.Click();
            }
            Wait.WaitForElementDisplayed(By.Name("Name"));
        }

        public void ScrollToContact()
        {
            if (_contactAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _contactAccordion.Click();
            }
            else
            {
                _contactAccordion.ClickByJS();
                Wait.WaitLoading();
                _contactAccordion.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='MedicalPracticeEmails']"));
        }

        public void ClickAddAddrss()
        {
            Wait.WaitForControl(SimsBy.AutomationId("add_new_button"));
            _addNewButton.ClickAndWaitFor(SimsBy.AutomationId("find_address_detail"));;
        }

        public void ClickEditAddrss()
        {
            Wait.WaitForControl(SimsBy.AutomationId("edit_button"));
            _editNewButton.ClickAndWaitFor(SimsBy.AutomationId("find_address_detail")); ;
        }

        public void ClickDeleteAddrss()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[title='Delete address detail']"));
            _deleteAddessButton.ClickAndWaitFor(SimsBy.AutomationId("continue_with_delete_button"));
            SeSugar.Automation.AutomationSugar.ClickOn("continue_with_delete_button");
            Wait.WaitLoading();
        }  

        public void ScrollToAssociated()
        {
            if (_associatedAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _associatedAccordion.Click();
            }
            else
            {
                _associatedAccordion.ClickByJS();
                Wait.WaitLoading();
                _associatedAccordion.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='Doctors']"));
        }

        public void ScrollToDocument()
        {
            if (_documentAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _documentAccordion.Click();
            }
            else
            {
                _documentAccordion.ClickByJS();
                Wait.WaitLoading();
                _documentAccordion.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='MedicalPracticeNotes']"));
        }

        #endregion

    }
}
