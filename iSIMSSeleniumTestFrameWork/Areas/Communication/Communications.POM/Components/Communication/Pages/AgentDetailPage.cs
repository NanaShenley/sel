using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;

namespace POM.Components.Communication
{
    public class AgentDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("Agent_Datamaintenance_record_detail"); }
        }

        public static AgentDetailPage Create()
        {
            Wait.WaitForElementDisplayed(By.CssSelector("[data-automation-id='well_know_action_save']"));
            return new AgentDetailPage();
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "LegalForename")]
        private IWebElement _foreNameTextbox;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _surNameTextbox;

        [FindsBy(How = How.Name, Using = "AddressesAddress")]
        private IWebElement _addressTextArea;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_button']")]
        private IWebElement _addNewAddressButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Address']")]
        private IWebElement addressFieldTitle;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_linked_agency_button']")]
        private IWebElement _addLinkedAgenciesButton;

        //[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_linked_agencies_button']")]
        //private IWebElement _addLinkedAgenciesButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Basic Details']")]
        private IWebElement _selectBasicDetail;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Contact Details']")]
        private IWebElement _selectContactInfor;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Documents']")]
        private IWebElement _selectDocument;

        [FindsBy(How = How.Name, Using = "AddressesAddress")]
        private IWebElement _addressesTextarea;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = ".checkboxlist-column")]
        private IList<IWebElement> _serviceProvideList;

      [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit_button']")]
        private IWebElement _editAddressButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        public string SurName
        {
            set
            {
                _surNameTextbox.SetText(value);
            }
            get
            {
                return _surNameTextbox.GetValue();
            }
        }

        public string ForeName
        {
            set
            {
                _foreNameTextbox.SetText(value);
            }
            get
            {
                return _foreNameTextbox.GetValue();
            }
        }

        public string ServiceProvide
        {
            set
            {
                IWebElement service = SeleniumHelper.Get(SimsBy.AutomationId(value));
                service.Set(true);
            }
            get
            {
                string items = "";
                IList<IWebElement> checkedServices = SeleniumHelper.FindElements(SimsBy.CssSelector("[name='AgentProvidedServices.SelectedIds'][checked='checked']"));
                foreach (var element in checkedServices)
                {
                    items += String.Format("{0},", element.GetAttribute("data-automation-id"));
                }
                return items;
            }
        }

        public string Address
        {
            get
            {
                return _addressesTextarea.Text;
            }
        }
        #endregion

        #region Actions

        public bool IsMessageSuccessAppear()
        {
            return _messageSuccess.IsExist();
        }

        public void ScrollToBasicDetail()
        {
            if (_selectBasicDetail.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _selectBasicDetail.Click();
            }
            else
            {
                _selectBasicDetail.ClickByJS();
                Wait.WaitLoading();
                _selectBasicDetail.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='AgencyAgents']"));
        }

        public void ScrollToContactInformation()
        {
            if (_selectContactInfor.GetAttribute("class").Contains("collapsed"))
            {
                _selectContactInfor.Click();
            }
            else
            {
                _selectContactInfor.ClickByJS();
                Wait.WaitLoading();
                _selectContactInfor.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='AgentEmails']"));
        }

        public void ScrollToDocument()
        {
            if (_selectDocument.GetAttribute("class").Contains("collapsed"))
            {
                _selectDocument.Click();
            }
            else
            {
                _selectDocument.ClickByJS();
                Wait.WaitLoading();
                _selectDocument.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='AgentDocuments']"));
        }

        public EditAddressDialog EditAddress()
        {
            //if (addressFieldTitle.GetAttribute("class").Contains("collapsed"))
            //{
            //    addressFieldTitle.Click();
            //}
            //else
            //{
            //    addressFieldTitle.ClickByJS();
            //    Wait.WaitLoading();
            //    addressFieldTitle.Click();
            //}

            Wait.WaitForElementDisplayed(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address")));
            _editAddressButton.Click();
            Wait.WaitLoading();
            return new EditAddressDialog();
        }

        public SelectAgencyTripletDialog AddLinkAgencies()
        {
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_linked_agency_button"))).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new SelectAgencyTripletDialog();
        }

        public AddNewAddressDialog AddNewAddress()
        {
            Wait.WaitForElementDisplayed(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address")));
            //SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address"))).Click();
            Wait.WaitLoading();
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_new_button"))).Click();            
            //SeleniumHelper.ClickByJS(_addNewAddressButton);
            //_addNewAddressButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            SeleniumHelper.Sleep(2);
            return new AddNewAddressDialog();
        }

        public AddAddressDialog AddAddress()
        {
            SeleniumHelper.Sleep(2);
            if (SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address"))).GetAttribute("aria-expanded").Trim().Equals("false"))
                SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address"))).Click();
            SeleniumHelper.Sleep(2);
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            SeleniumHelper.FindElement(By.CssSelector("[data-automation-id='add_new_button']")).Click();
            return new AddAddressDialog();
        }

        public AddAddressDialog EditNewAddress()
        {
            SeleniumHelper.Sleep(2);
            if (SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address"))).GetAttribute("aria-expanded").Trim().Equals("false"))
                SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address"))).Click();
            SeleniumHelper.Sleep(2);
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            SeleniumHelper.FindElement(By.CssSelector("[data-automation-id='edit_button']")).Click();
            return new AddAddressDialog();
        }

        public void DeleteAddress()
        {
            SeleniumHelper.Sleep(2);
            if (SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address"))).GetAttribute("aria-expanded").Trim().Equals("false"))
                SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address"))).Click();
            SeleniumHelper.Sleep(2);
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            SeleniumHelper.FindElement(By.CssSelector("[title='Delete address detail']")).Click();
        }

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            SeleniumHelper.Sleep(3);
            Refresh();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Get all checked service provide name
        /// </summary>
        /// <returns>List name of checked service provide</returns>
        public IList<string> GetCheckedServiceProvide()
        {
            List<string> serviceProvides = new List<string>();
            foreach (var item in _serviceProvideList)
            {
                if (item.GetText().Trim().Equals("")) continue;
                if (item.FindElement(By.Name("AgentProvidedServices.SelectedIds")).IsChecked())
                {
                    serviceProvides.Add(item.FindElement(By.CssSelector("label")).GetText());
                }
            }
            return serviceProvides;
        }

        /// <summary>
        /// Wait Until Linked Agency display
        /// </summary>
        /// <param name="agencyName">Agency name</param>
        public void WaitUnilAgencyRowDisplay(string agencyName)
        {
            Wait.WaitUntil((d) => { return LinkedAgenciesTable.Rows.Any(t => t.Agency.Equals(agencyName)); });
        }

        public void Delete()
        {
            if(_deleteButton.IsExist())
            {
                Retry.Do(_deleteButton.ClickByJS);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                var deleteDialog = new DeleteConfirmationDialog();
                deleteDialog.ConfirmDelete();
            }
        }

        public void ClickDeleteButton()
        {
            _deleteButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        public bool IsWarningDeleteDisplay()
        {
            bool isDisplay = false;
            try
            {
                var warningDialog = new WarningConfirmationDialog();
                isDisplay = warningDialog != null;
            }
            catch (Exception)
            {
                isDisplay = false;
            }
            return isDisplay;
        }

        #endregion

        #region Grid

        public GridComponent<LinkedAgencies> LinkedAgenciesTable
        {
            get
            {
                GridComponent<LinkedAgencies> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<LinkedAgencies>(By.CssSelector("[data-maintenance-container='AgencyAgents']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<TelephoneNumber> TelephoneNumberTable
        {
            get
            {
                GridComponent<TelephoneNumber> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TelephoneNumber>(By.CssSelector("[data-maintenance-container='AgentTelephones']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<EmailAddress> EmailAddressTable
        {
            get
            {
                GridComponent<EmailAddress> returnValue = null;
                Retry.Do(() =>
                {
                    SeleniumHelper.Sleep(1);
                    returnValue = new GridComponent<EmailAddress>(By.CssSelector("[data-maintenance-container='AgentEmails']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<NoteAndDocument> NoteAndDocumentTable
        {
            get
            {
                GridComponent<NoteAndDocument> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<NoteAndDocument>(By.CssSelector("[data-maintenance-container='AgentDocuments']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class NoteAndDocument
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Summary']")]
            private IWebElement _summary;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
            private IWebElement _addNoteButton;

            [FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
            private IWebElement _noteTextarea;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id*='documents']")]
            private IWebElement _document;

            public string Summary
            {
                set
                {
                    _summary.SetText(value);
                    _summary.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _summary.GetValue();
                }
            }

            public string Note
            {
                set
                {
                    _addNoteButton.Click();
                    _noteTextarea.SetText(value);
                    _noteTextarea.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    _addNoteButton.Click();
                    return _noteTextarea.GetValue();
                }
            }

            public ViewDocumentDialog AddDocument()
            {
                _document.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                return new ViewDocumentDialog();
            }
        }

        public class LinkedAgencies : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='.AgencyAgencyName']")]
            private IWebElement _agencyNameTextbox;

            public string Agency
            {
                get
                {
                    return _agencyNameTextbox.GetValue();
                }
            }
        }

        public class TelephoneNumber
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='.TelephoneNumber']")]
            private IWebElement _numberTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.LocationType.dropdownImitator']")]
            private IWebElement _locationDropdown;

            [FindsBy(How = How.CssSelector, Using = "[name$='.IsMainTelephone']")]
            private IWebElement _mainNumberCheckbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Notes']")]
            private IWebElement _noteTextbox;

            

            public string Number
            {
                get { return _numberTextbox.GetValue(); }
                set
                {
                    _numberTextbox.SetAttribute("value", "");
                    _numberTextbox.SetText(value);
                    _numberTextbox.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
            }

            public string Location
            {
                get
                {
                    return _locationDropdown.GetValue();
                }
                set
                {
                    _locationDropdown.EnterForDropDown(value);
                }
            }

            public bool MainNumber
            {
                set
                {
                    _mainNumberCheckbox.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _mainNumberCheckbox.IsChecked(); }
            }

            public string Note
            {
                set
                {
                    _noteTextbox.SetAttribute("value", "");
                    _noteTextbox.SetText(value);
                    _noteTextbox.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _noteTextbox.GetValue(); }
            }


        }

        public class EmailAddress
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='.EmailAddress']")]
            private IWebElement _emailAddressTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.IsMainEmail']")]
            private IWebElement _mainEmailCheckbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Notes']")]
            private IWebElement _noteTextbox;

            public string Email
            {
                get { return _emailAddressTextbox.GetValue(); }
                set
                {
                    _emailAddressTextbox.SetAttribute("value", "");
                    _emailAddressTextbox.SetText(value);
                    _emailAddressTextbox.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
            }

            public bool MainEmail
            {
                set
                {
                    _mainEmailCheckbox.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _mainEmailCheckbox.IsChecked(); }
            }

            public string Note
            {
                set
                {
                    _noteTextbox.SetAttribute("value", "");
                    _noteTextbox.SetText(value);
                    _noteTextbox.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _noteTextbox.GetValue(); }
            }
        }

        #endregion
    }
}
