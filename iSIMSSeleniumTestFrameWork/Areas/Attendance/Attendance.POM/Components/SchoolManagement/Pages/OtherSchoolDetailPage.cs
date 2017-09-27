using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;
using POM.Components.Common;
using POM.Components.HomePages;

namespace POM.Components.SchoolManagement
{
    public class OtherSchoolDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector(".main"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _schoolNameTextbox;

        [FindsBy(How = How.Name, Using = "HeadTeacher")]
        private IWebElement _headTeacherTextbox;

        [FindsBy(How = How.Name, Using = "DENINumber")]
        private IWebElement _DENINumberTextbox;

        [FindsBy(How = How.Name, Using = "EducationLibraryBoard.dropdownImitator")]
        private IWebElement _educationLibraryDropdown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_School Address']")]
        private IWebElement _schoolAddressAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Contact Details']")]
        private IWebElement _contactDetailSection;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_button']")]
        private IWebElement _addNewAddressButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        [FindsBy(How = How.Name, Using = "TelephoneNumber")]
        private IWebElement _telephoneNumberTextbox;

        [FindsBy(How = How.Name, Using = "FaxNumber")]
        private IWebElement _faxNumberTextbox;

        [FindsBy(How = How.Name, Using = "EmailAddress")]
        private IWebElement _emailAddressTextbox;

        [FindsBy(How = How.Name, Using = "WebsiteAddress")]
        private IWebElement _websiteAddressTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.Name, Using = "AddressesAddress")]
        private IWebElement _addressTextArea;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit_button']")]
        private IWebElement _editAddressButton;

        [FindsBy(How = How.CssSelector, Using = ".alert-warning")]
        private IWebElement _warningMessage;

        public string Name
        {
            get { return _schoolNameTextbox.GetValue(); }
            set { _schoolNameTextbox.SetText(value); }
        }

        public string HeadTeacher
        {
            get { return _headTeacherTextbox.GetValue(); }
            set { _headTeacherTextbox.SetText(value); }
        }

        public string DENINumber
        {
            get { return _DENINumberTextbox.GetValue(); }
            set { _DENINumberTextbox.SetText(value); }
        }

        public string EducationLibraryBoard
        {
            get { return _educationLibraryDropdown.GetValue(); }
            set { _educationLibraryDropdown.EnterForDropDown(value); }
        }

        public string TelephoneNumber
        {
            get { return _telephoneNumberTextbox.GetValue(); }
            set { _telephoneNumberTextbox.SetText(value); }
        }

        public string FaxNumber
        {
            get { return _faxNumberTextbox.GetValue(); }
            set { _faxNumberTextbox.SetText(value); }
        }

        public string EmailAddress
        {
            get { return _emailAddressTextbox.GetValue(); }
            set { _emailAddressTextbox.SetText(value); }
        }

        public string WebsiteAddess
        {
            get { return _websiteAddressTextbox.GetValue(); }
            set { _websiteAddressTextbox.SetText(value); }
        }

        public string Address
        {
            get { return _addressTextArea.GetText().Replace("\r\n", " ").Trim(); }
        }

        #endregion

        #region Actions

        public void ScrollToSchoolAddress()
        {
            if (_schoolAddressAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _schoolAddressAccordion.ClickByJS();
            }
            else
            {
                _schoolAddressAccordion.ClickByJS();
                Wait.WaitLoading();
                _schoolAddressAccordion.ClickByJS();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-automation-id='add_new_button']"));
        }

        public void ScrollToContactDetail()
        {
            if (_contactDetailSection.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _contactDetailSection.ClickByJS();
            }
            else
            {
                _contactDetailSection.ClickByJS();
                Wait.WaitLoading();
                _contactDetailSection.ClickByJS();
            }
            Wait.WaitForElementDisplayed(By.Name("TelephoneNumber"));
        }

        public AddAddressDialogPage AddNewAddress()
        {
            _addNewAddressButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddAddressDialogPage();
        }

        public EditAddressDialog EditAddress()
        {
            _editAddressButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new EditAddressDialog();
        }

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool IsSuccessMessageDisplay()
        {
            try
            {
                return _messageSuccess.IsExist();
            }
            catch (Exception) { return false; }
        }

        public bool IsWarningMessageDisplay()
        {
            try
            {
                return _warningMessage.IsExist();
            }
            catch (Exception) { return false; }
        }

        public void Delete()
        {
            try
            {
                if (_deleteButton.IsExist())
                {
                    _deleteButton.Click();
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                    var confirmDeleteDialog = new DeleteConfirmationDialog();
                    confirmDeleteDialog.ConfirmDelete();
                }
            }
            catch (Exception) { }
        }
        #endregion
    }
}
