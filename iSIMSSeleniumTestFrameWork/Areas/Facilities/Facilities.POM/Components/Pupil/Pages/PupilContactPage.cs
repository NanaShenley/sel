using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;

using System;

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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Addresses']")]
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
            set { _languageDropDown.ChooseSelectorOption(value); }
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

        public string PlaceOfWork {
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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_an_additional_address_button']")]
        private IWebElement _addaAdditionalAddressesLink;

        #endregion

        #endregion

        #region ACTIONS
        public void SelectPersonalDetailsTab()
        {
            _personalDetailLink.ClickByJS();
            Wait.WaitUntilDisplayed(SimsBy.Name("Forename"));
        }

        public void SelectAddressesTab()
        {
            _addressesLink.ClickByJS();
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='ContactAddresses']"));
        }

        public void SelectEthnicCulturalTab()
        {
            _ethnicCulturalLink.ClickByJS();
            Wait.WaitForElementDisplayed(SimsBy.Name("HomeLanguage.dropdownImitator"));
        }
        
        public void SelectJobDetailsTab()
        {
            _jobDetailsLink.ClickByJS();
            Wait.WaitForElementDisplayed(SimsBy.Name("PlaceOfWork"));
        }
        
        public void SelectAssociatedPupilsTab() 
        {
            _associatedPupilsLink.ClickByJS();
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerContactRelationships']"));
        }

        public AddContactAddressDialog ClickAddanAdditionalAddressLink()
        {
            _addaAdditionalAddressesLink.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AddContactAddressDialog();
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

        public void ClickDelete()
        {
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

        #endregion

    }
}
