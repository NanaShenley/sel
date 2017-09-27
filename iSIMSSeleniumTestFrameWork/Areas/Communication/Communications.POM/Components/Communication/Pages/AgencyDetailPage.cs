using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using POM.Components.Common;

namespace POM.Components.Communication
{
    public class AgencyDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("Agencies_Datamaintenance_record_detail"); }
        }

        public static AgencyDetailPage Create()
        {
            Wait.WaitForElementDisplayed(By.CssSelector("[data-automation-id = 'well_know_action_save']"));
            return new AgencyDetailPage();
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.Name, Using = "AgencyName")]
        private IWebElement _agencyNameTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Address']")]
        private IWebElement _addNewAddressButton;

        [FindsBy(How = How.CssSelector, Using = ".checkboxlist-column")]
        private IList<IWebElement> _serviceProvideList;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_linked_agent_button']")]
        private IWebElement _addLinkedAgent;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Basic Details']")]
        private IWebElement _basicDetailAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Contact Details']")] //"[data-automation-id='section_menu_Contact Information']")]
        private IWebElement _contactAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Documents']")]
        private IWebElement _documentAccordion;

        [FindsBy(How = How.Name, Using = "AddressesAddress")]
        private IWebElement _addressesTextarea;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        [FindsBy(How = How.Name, Using = "Website")]
        private IWebElement _websiteAddressTextBox;

        public string WebsiteAddress
        {
            set { _websiteAddressTextBox.SetText(value); }
            get { return _websiteAddressTextBox.GetValue(); }
        }
        public string AgencyName
        {
            set { _agencyNameTextBox.SetText(value); }
            get { return _agencyNameTextBox.GetValue(); }
        }

        public string ServiceProvide
        {
            set
            {
                IList<string> services = value.Trim().Split(',').ToList();
                foreach (var item in _serviceProvideList)
                {
                    if (services.Count == 0)
                    {
                        break;
                    }

                    string itemText = item.FindElement(By.CssSelector("label")).GetText().Trim();
                    if (services.Contains(itemText))
                    {
                        item.FindElement(By.Name("AgencyProvidedServices.SelectedIds")).Set(true);
                        services.Remove(itemText);
                    }
                }
            }
            get
            {
                string items = "";
                IList<IWebElement> checkedServices = SeleniumHelper.FindElements(SimsBy.CssSelector("[name='AgencyProvidedServices.SelectedIds'][checked='checked']"));
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

        public SelectAgentTripletDialog AddLinkedAgent()
        {
            _addLinkedAgent.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            SeleniumHelper.Sleep(2);
            return new SelectAgentTripletDialog();
        }

        public void Delete()
        {
            if(_deleteButton.IsExist())
            {
                _deleteButton.Click();
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

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            SeleniumHelper.Sleep(3);
            Refresh();
        }

        public AddNewAddressDialog AddNewAddress()
        {
            SeleniumHelper.Sleep(2);
            if (SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address"))).GetAttribute("aria-expanded").Trim().Equals("false"))
                SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Address"))).Click();
            SeleniumHelper.Sleep(2);
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            SeleniumHelper.FindElement(By.CssSelector("[data-automation-id='add_new_button']")).Click();
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

        public AddAddressDialog EditAddress()
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
                if (item.FindElement(By.Name("AgencyProvidedServices.SelectedIds")).IsChecked())
                {
                    serviceProvides.Add(item.FindElement(By.CssSelector("label")).GetText().Trim());
                }
            }
            return serviceProvides;
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
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='AgencyAgents']"));
        }

        public void ScrollToContact()
        {
            var temp = _contactAccordion.GetAttribute("aria-expanded");
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
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='AgencyEmails']"));
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
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='AgencyDocuments']"));
        }

        #endregion

        #region Grid

        public GridComponent<LinkedAgent> LinkedAgents
        {
            get
            {
                GridComponent<LinkedAgent> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<LinkedAgent>(By.CssSelector("[data-maintenance-container='AgencyAgents']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class LinkedAgent : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='AgentLegalForename']")]
            private IWebElement _foreName;

            [FindsBy(How = How.CssSelector, Using = "[name$='AgentLegalSurname']")]
            private IWebElement _surName;

            public string ForeName
            {
                get { return _foreName.GetValue(); }
            }

            public string SurName
            {
                get { return _surName.GetValue(); }
            }
        }

        public GridComponent<TelephoneNumber> TelephoneNumberTable
        {
            get
            {
                GridComponent<TelephoneNumber> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TelephoneNumber>(By.CssSelector("[data-maintenance-container='AgencyTelephones']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class TelephoneNumber : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='TelephoneNumber']")]
            private IWebElement _numberTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='LocationType.dropdownImitator']")]
            private IWebElement _locationDropdown;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsMainTelephone']")]
            private IWebElement _mainNumberCheckbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
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
                get { return _locationDropdown.GetValue(); }
                set { _locationDropdown.EnterForDropDown(value); }
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

        public GridComponent<EmailAddress> EmailAddressTable
        {
            get
            {
                GridComponent<EmailAddress> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<EmailAddress>(By.CssSelector("[data-maintenance-container='AgencyEmails']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class EmailAddress : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='EmailAddress']")]
            private IWebElement _emailAddressTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsMainEmail']")]
            private IWebElement _mainEmailCheckbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
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

        public GridComponent<Document> Documents
        {
            get
            {
                GridComponent<Document> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Document>(By.CssSelector("[data-maintenance-container='AgencyDocuments']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class Document : GridRow
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
                get { return _summary.GetValue(); }
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
                    string note = _noteTextarea.GetText();
                    _noteTextarea.SendKeys(Keys.Tab);
                    return note;
                }
            }

            public ViewDocumentDialog AddDocument()
            {
                _document.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                return new ViewDocumentDialog();
            }
        }
        #endregion
    }
}
