using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using SeSugar.Automation;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;

namespace POM.Components.Pupil
{
    public class PupilContactPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='detail']"); }
        }

        #region PROPERTIES

        #region Common

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _continueDeleteButton;

		#endregion

        #region Tabs

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Personal']")]
        private IWebElement _personalDetailLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Contact']")]
        private IWebElement _addressesLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Ethnic/Cultural']")]
        private IWebElement _ethnicCulturalLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Job']")]
        private IWebElement _jobDetailsLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Associated']")]
        private IWebElement _associatedPupilsLink;

		#endregion

        #region Personal Details section

        // Personal Details section
        [FindsBy(How = How.Name, Using = "Title.dropdownImitator")]
        private IWebElement _titleDropDown;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _forenameTextBox;

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _surnameTextBox;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _genderDropDown;

        [FindsBy(How = How.Name, Using = "Salutation")]
        private IWebElement _salutationTextBox;

        [FindsBy(How = How.Name, Using = "Addressee")]
        private IWebElement _addresseeTextBox;

        public string Title
        {
            set { _titleDropDown.EnterForDropDown(value); }
            get { return _titleDropDown.GetValue(); }
        }

        public string Forename
        {
            set { _forenameTextBox.SetText(value); }
            get { return _forenameTextBox.GetValue(); }
        }

        public string Surname
        {
            set { _surnameTextBox.SetText(value); }
            get { return _surnameTextBox.GetValue(); }
        }

        public string Gender
        {
            set { _genderDropDown.EnterForDropDown(value); }
            get { return _genderDropDown.GetValue(); }
        }

        public string Salutation
        {
            set { _salutationTextBox.SetText(value); }
            get { return _salutationTextBox.GetValue(); }
        }

        public string Addressee
        {
            set { _addresseeTextBox.SetText(value); }
            get { return _addresseeTextBox.GetValue(); }
        }

		#endregion

        #region Ethnic section

        [FindsBy(How = How.Name, Using = "HomeLanguage.dropdownImitator")]
        private IWebElement _languageDropDown;

        public string Language
        {
            set
            {
                Refresh();
                _languageDropDown.ChooseSelectorOption(value);
            }
            get { return _languageDropDown.GetValue(); }
        }

		#endregion

        #region Job Details section

        [FindsBy(How = How.Name, Using = "PlaceOfWork")]
        private IWebElement _placeOfWorkTextBox;

        [FindsBy(How = How.Name, Using = "JobTitle")]
        private IWebElement _jobTitleTextBox;

        [FindsBy(How = How.Name, Using = "Occupation.dropdownImitator")]
        private IWebElement _opcupationDropDown;

        public string PlaceOfWork
        {
            set { _placeOfWorkTextBox.SetText(value); }
            get { return _placeOfWorkTextBox.GetValue(); }
        }

        public string JobTitle
        {
            set { _jobDetailsLink.SetText(value); }
            get { return _jobDetailsLink.GetValue(); }
        }

        public string Occupation
        {
            set { _opcupationDropDown.ChooseSelectorOption(value); }
            get { return _opcupationDropDown.GetValue(); }
        }

		#endregion

        #region Associated Pupils section

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pupil_button']")]
        private IWebElement _addPupilLink;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='LearnerContactRelationships']")]
        private IWebElement _pupilTable;

        public GridComponent<PupilRow> PupilTable
        {
            get
            {
                GridComponent<PupilRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PupilRow>(By.CssSelector("[data-maintenance-container='LearnerContactRelationships']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class PupilRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name $= 'Priority']")]
            private IWebElement _priority;

            [FindsBy(How = How.CssSelector, Using = "[name $= 'LearnerContactRelationshipType.dropdownImitator']")]
            private IWebElement _relationship;

            [FindsBy(How = How.CssSelector, Using = "[name $= 'HasParentalResponsibility']")]
            private IWebElement _parentalResponsibility;

            [FindsBy(How = How.CssSelector, Using = "[name $= 'ReceivesCorrespondance']")]
            private IWebElement _receivesCorrespondance;

            [FindsBy(How = How.CssSelector, Using = "[name $= 'ReceivesSchoolReport']")]
            private IWebElement _schoolReport;

            [FindsBy(How = How.CssSelector, Using = "[name $= 'HasCourtOrder']")]
            private IWebElement _courtOrder;

            [FindsBy(How = How.CssSelector, Using = "[name $= 'LearnerLegalForename']")]
            private IWebElement _legalForename;

            [FindsBy(How = How.CssSelector, Using = "[name $= 'LearnerLegalSurname']")]
            private IWebElement _legalSurname;

            public string Priority
            {
                set
                {
                    Retry.Do(_priority.Click);
                    _priority.SetText(value);
                }
                get { return _priority.GetValue(); }
            }

            public string Relationship
            {
                set { _relationship.ChooseSelectorOption(value); }
                get { return _relationship.GetValue(); }
            }

            public bool ParentalResponsibility
            {
                set { _parentalResponsibility.Set(value); }
                get { return _parentalResponsibility.IsChecked(); }
            }

            public bool ReceivesCorrespondance
            {
                set { _receivesCorrespondance.Set(value); }
                get { return _receivesCorrespondance.IsChecked(); }
            }

            public bool SchoolReport
            {
                set { _schoolReport.Set(value); }
                get { return _schoolReport.IsChecked(); }
            }

            public bool CourtOrder
            {
                set { _courtOrder.Set(value); }
                get { return _courtOrder.IsChecked(); }
            }

            public string LegalForename
            {
                set { _legalForename.SetText(value); }
                get { return _legalForename.GetValue(); }
            }

            public string LegalSurname
            {
                set { _legalSurname.SetText(value); }
                get { return _legalSurname.GetValue(); }
            }
        }

		#endregion

        #region Addresses

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

            [FindsBy(How = How.CssSelector, Using = "[title='Edit full record']")]
            private IWebElement _edit;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'add_note_button']")]
            private IWebElement _note;

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

            public void Note(string note)
            {
                _note.Click();
                Wait.WaitForElementDisplayed(SeSugar.Automation.SimsBy.CssSelector("[name$='.Note']"));
                IWebElement _noteTextArea = SeleniumHelper.FindElement(SeSugar.Automation.SimsBy.CssSelector("[name$='.Note']"));
                _noteTextArea.SetText(note);
            }

            public void ClickEditAddress()
            {
                AutomationSugar.WaitFor("Action_Dropdown");
                AutomationSugar.ClickOn("Action_Dropdown");
                AutomationSugar.WaitForAjaxCompletion();

                AutomationSugar.WaitFor("Edit_Address_Action");
                AutomationSugar.ClickOn("Edit_Address_Action");
                AutomationSugar.WaitForAjaxCompletion();
            }

            public void ClickMoveAddress()
            {
                AutomationSugar.WaitFor("Action_Dropdown");
                AutomationSugar.ClickOn("Action_Dropdown");
                AutomationSugar.WaitForAjaxCompletion();

                AutomationSugar.WaitFor("Move_address_Action");
                AutomationSugar.ClickOn("Move_address_Action");
                AutomationSugar.WaitForAjaxCompletion();
            }
        }

        public GridComponent<AddressRow> AddressTable
        {
            get
            {
                GridComponent<AddressRow> returnValue = null;
                SeSugar.Automation.Retry.Do(() =>
                {
                    returnValue = new GridComponent<AddressRow>(By.CssSelector("[data-maintenance-container='ContactAddresses']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public void ClickAddAddress()
        {
            AutomationSugar.WaitFor("add_an_address_button");
            AutomationSugar.ClickOn("add_an_address_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        #endregion

        #region Phone/Email

        #region TelephoneNumber Grid

        public GridComponent<TelephoneNumberRow> TelephoneNumberTable
        {
            get
            {
                GridComponent<TelephoneNumberRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TelephoneNumberRow>(By.CssSelector("[data-maintenance-container='LearnerContactTelephones']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class TelephoneNumberRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='TelephoneNumber']")]
            private IWebElement _telephoneNumber;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _endCol;

            [FindsBy(How = How.CssSelector, Using = "[name$='LocationType.dropdownImitator']")]
            private IWebElement _location;

            [FindsBy(How = How.CssSelector, Using = "[name*='LearnerContactTelephones'][name$='UseForTextMessages']")]
            private IWebElement _aMS;

            [FindsBy(How = How.CssSelector, Using = "[name*='LearnerContactTelephones'][name$='IsMainTelephone']")]
            private IWebElement _mainNumber;

            public string TelephoneNumber
            {
                set
                {
                    _telephoneNumber.SetText(value, null);
                }
                get
                {
                    return _telephoneNumber.GetAttribute("value");
                }
            }

            public string Location
            {
                set
                {
                    _location.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _location.GetAttribute("value"); }
            }

            public bool AMS
            {
                set
                {
                    _aMS.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _aMS.IsChecked(); }
            }

            public bool MainNumber
            {
                set
                {
                    _mainNumber.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _mainNumber.IsChecked(); }
            }

        }

        #endregion

        #region Email Grid

        public GridComponent<EmailRow> EmailTable
        {
            get
            {
                GridComponent<EmailRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<EmailRow>(By.CssSelector("[data-maintenance-container='LearnerContactEmails']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class EmailRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='EmailAddress']")]
            private IWebElement _emailAddress;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _endCol;

            [FindsBy(How = How.CssSelector, Using = "[name$='LocationType.dropdownImitator']")]
            private IWebElement _location;

            [FindsBy(How = How.CssSelector, Using = "[name*='LearnerContactEmails'][name$='UseForTextMessages']")]
            private IWebElement _aMS;

            [FindsBy(How = How.CssSelector, Using = "[name*='LearnerContactEmails'][name$='IsMainEmail']")]
            private IWebElement _mainEmail;

            public string EmailAddress
            {
                set
                {
                    _emailAddress.SetText(value, null);
                }
                get
                {
                    return _emailAddress.GetAttribute("value");
                }
            }

            public string Location
            {
                set
                {
                    _location.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _location.GetAttribute("value");
                }
            }

            public bool AMS
            {
                set
                {
                    _aMS.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _aMS.IsChecked();
                }
            }

            public bool MainEmail
            {
                set
                {
                    _mainEmail.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _mainEmail.IsChecked();
                }
            }

        }

        #endregion


        #endregion

        #endregion

        #region ACTIONS

        public void SelectContactDetailTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Contact Details");
        }

        public void SelectPersonalDetailsTab()
        {
            _personalDetailLink.ClickByJS();
            Wait.WaitUntilDisplayed(SeSugar.Automation.SimsBy.Name("Forename"));
        }

        public void SelectAddressesTab()
        {
            _addressesLink.ClickByJS();
            Wait.WaitForElementDisplayed(SeSugar.Automation.SimsBy.CssSelector("[data-automation-id='section_menu_Addresses']"));
        }

        public void SelectEthnicCulturalTab()
        {
            _ethnicCulturalLink.ClickByJS();
            Wait.WaitForElementDisplayed(SeSugar.Automation.SimsBy.Name("HomeLanguage.dropdownImitator"));
        }

        public void SelectJobDetailsTab()
        {
            _jobDetailsLink.ClickByJS();
            Wait.WaitForElementDisplayed(SeSugar.Automation.SimsBy.Name("PlaceOfWork"));
        }

        public void SelectAssociatedPupilsTab()
        {
            _associatedPupilsLink.ClickByJS();
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerContactRelationships']"));
        }

        public AddAssociatedPupilsTripletDialog ClickAddPupilLink()
        {
            _addPupilLink.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AddAssociatedPupilsTripletDialog();
        }

        public bool IsSuccessMessageDisplayed()
        {
            return SeleniumHelper.DoesWebElementExist(_successMessage);
        }

        public void ClickSave()
        {
            SeleniumHelper.Sleep(5);
            Wait.WaitForControl(SimsBy.AutomationId("well_know_action_save"));
            IWebElement save = SeleniumHelper.Get(SimsBy.AutomationId("well_know_action_save"));
            save.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public void ClickDelete()
        {
            base.Refresh();
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
        }

        public void ContinueDelete()
        {
            WarningConfirmationDialog warningDialog = null;
            try
            {
                warningDialog = new WarningConfirmationDialog();
            }
            catch (Exception)
            {
                warningDialog = null;
            }

            if (warningDialog != null)
            {
                warningDialog.ConfirmDelete();
            }
        }

        public static PupilContactPage LoadPupilContactDetail(Guid pupilContactId)
        {
            var jsExecutor = (IJavaScriptExecutor)SeSugar.Environment.WebContext.WebDriver;
            string js = "sims_commander.OpenDetail(undefined, '/{0}/Pupils/SIMS8LearnerContactMaintenanceSimpleLearnerContact/ReadDetail/{1}')";

            Retry.Do(() => { jsExecutor.ExecuteScript(string.Format(js, TestSettings.TestDefaults.Default.Path, pupilContactId)); });

            AutomationSugar.WaitForAjaxCompletion();

            return new PupilContactPage();
        }

        #endregion
    }
}