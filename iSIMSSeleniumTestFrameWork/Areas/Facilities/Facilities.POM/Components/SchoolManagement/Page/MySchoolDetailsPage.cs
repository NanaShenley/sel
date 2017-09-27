using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POM.Components.SchoolManagement
{
    public class MySchoolDetailsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _schoolNameTextBox;

        [FindsBy(How = How.Name, Using = "HeadTeacher")]
        private IWebElement _headTeacherTextBox;

        /// <summary>
        /// Variant Specific:NI Region
        /// </summary>
        [FindsBy(How = How.Name, Using = "DENINumber")]
        public IWebElement _DENINumberTextBox;

        /// <summary>
        ///  Variant Specfic Eng Region
        /// </summary>
        [FindsBy(How = How.Name, Using = "SchoolPhase.dropdownImitator")]
        public IWebElement _schoolPhaseDropdown;

        /// <summary>
        /// Variant Specfic Eng Region
        /// </summary>
        [FindsBy(How = How.Name, Using = "SchoolType.dropdownImitator")]
        public IWebElement _schoolTypeDropdown;

        [FindsBy(How = How.Name, Using = "ChildcarePlaces")]
        public IWebElement _childcarePlacesTextBox;

        [FindsBy(How = How.Name, Using = "ChildcareNurseryPlaces")]
        public IWebElement _childcareNurseryPlacesTextBox;

        /// <summary>
        /// Variant Specific:NI Region
        /// </summary>
        [FindsBy(How = How.Name, Using = "EducationLibraryBoard")]
        public IWebElement _EducationLibraryBoardTextBox;

        [FindsBy(How = How.Name, Using = "IntakeType.dropdownImitator")]
        private IWebElement _intakeTypeDropdown;

        [FindsBy(How = How.Name, Using = "EstablishmentNumber")]
        public IWebElement _establishmentNumberTextBox;

         [FindsBy(How = How.Name, Using = "GoverningBodyNumber")]
        public IWebElement _governingBodyNumberTextBox;



        /// <summary>
        /// Variant Specific:NI And Welsh Region
        /// </summary>
        [FindsBy(How = How.Name, Using = "TeachingMedium.dropdownImitator")]
        private IWebElement _teachingMediumDropdow;
        
        [FindsBy(How = How.Name, Using = "PupilGender.dropdownImitator")]
        private IWebElement _pupilGenderDropdown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Basic Details']")]
        private IWebElement _basicDetailsAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_School Address']")]
        private IWebElement _schoolAddressAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Contact Details']")]
        private IWebElement _contactDetailAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_School Sites and Buildings']")]
        private IWebElement _siteAndBuildingAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Curriculum Years']")]
        private IWebElement _curriculumYearAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Associated Schools']")]
        private IWebElement _associatedAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Documents']")]
        private IWebElement _documentsAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_button']")]
        public IWebElement _addNewAddressButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit_button']")]
        public IWebElement _editAddressButton;

        [FindsBy(How = How.Name, Using = "TelephoneNumber")]
        private IWebElement _telephoneNumberTextBox;

        [FindsBy(How = How.Name, Using = "FaxNumber")]
        private IWebElement _faxNumberTextBox;

        [FindsBy(How = How.Name, Using = "EmailAddress")]
        private IWebElement _emailAddressTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_an_associated_school_button']")]
        private IWebElement _addAssociatedSchoolButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_curriculum_year_button']")]
        private IWebElement _addCurriculumYearLink;

        public string Name
        {
            get { return _schoolNameTextBox.GetValue(); }
            set { _schoolNameTextBox.SetText(value); }
        }

        public string HeadTeacher
        {
            get { return _headTeacherTextBox.GetValue(); }
            set { _headTeacherTextBox.SetText(value); }
        }

        public string SchoolPhase
        {
            get { return _schoolPhaseDropdown.GetValue().Trim(); }
            set { _schoolPhaseDropdown.EnterForDropDownByClick(value); }
        }

        public string SchoolType
        {
            get { return _schoolTypeDropdown.GetValue().Trim(); }
            set { _schoolTypeDropdown.EnterForDropDownByClick(value); }
        }

        public string ChildcarePlaces 
        {
            get { return _childcarePlacesTextBox.GetValue(); }
            set { _childcarePlacesTextBox.SetText(value); }
        }


        public string ChildcareNurseryPlaces
        {
            get { return _childcareNurseryPlacesTextBox.GetValue(); }
            set { _childcareNurseryPlacesTextBox.SetText(value); }
        }


        public string IntakeType
        {
            get 
            { return _intakeTypeDropdown.GetValue().Trim(); }
            set { _intakeTypeDropdown.EnterForDropDownByClick(value); }
        }

        public string TeachingMedium
        {
            get { return _teachingMediumDropdow.GetValue().Trim(); }
            set { _teachingMediumDropdow.EnterForDropDownByClick(value); }
        }

        public string PupilGender
        {
            get { return _pupilGenderDropdown.GetValue().Trim(); }
            set { _pupilGenderDropdown.EnterForDropDownByClick(value); }
        }

        public string TelephoneNumber
        {
            get { return _telephoneNumberTextBox.GetValue(); }
            set { _telephoneNumberTextBox.SetText(value); }
        }

        public string FaxNumber
        {
            get { return _faxNumberTextBox.GetValue(); }
            set { _faxNumberTextBox.SetText(value); }
        }

        public string EmailAddress
        {
            get { return _emailAddressTextBox.GetValue(); }
            set { _emailAddressTextBox.SetText(value); }
        }

        public string DENINumber
        {
            get { return _DENINumberTextBox.GetValue(); }
            set { _DENINumberTextBox.SetText(value); }
        }


        public string EducationLibraryBoard
        {
            get { return _EducationLibraryBoardTextBox.GetValue(); }
            set { _EducationLibraryBoardTextBox.SetText(value); }
        }

        public string EstablishmentNumber
        {
            get { return _establishmentNumberTextBox.GetValue(); }
            set { _establishmentNumberTextBox.SetText(value); }
        }

        public string GoverningBodyNumber
        {
            get { return _governingBodyNumberTextBox.GetValue(); }
            set { _governingBodyNumberTextBox.SetText(value); }
        }



        #endregion

        #region Action

        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool IsSuccessMessageDisplay()
        {
            return _successMessage.IsElementDisplayed();
        }

        public void ExpandBasicDetails()
        {
            if (_basicDetailsAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                SeleniumHelper.Get(By.CssSelector("[aria-expanded='true']")).ClickByJS();
                _basicDetailsAccordion.ClickByJS();
                Wait.WaitForElementDisplayed(By.Name("HeadTeacher"));
            }
        }

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
            Wait.WaitForElementDisplayed(By.Name("AddressesAddress"));
        }

        public void ScrollToContactDetails()
        {
            if (_contactDetailAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _contactDetailAccordion.ClickByJS();
            }
            else
            {
                _contactDetailAccordion.ClickByJS();
                Wait.WaitLoading();
                _contactDetailAccordion.ClickByJS();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='SchoolContacts']"));
        }

        public void ScrollToSchoolSitesAndBuildings()
        {
            if (_siteAndBuildingAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _siteAndBuildingAccordion.ClickByJS();
            }
            else
            {
                _siteAndBuildingAccordion.ClickByJS();
                Wait.WaitLoading();
                _siteAndBuildingAccordion.ClickByJS();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='SchoolSites']"));
        }

        public void ScrollToCurriculumYear()
        {
            if (_curriculumYearAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                SeleniumHelper.Get(By.CssSelector("[aria-expanded='true']")).ClickByJS();
                _curriculumYearAccordion.ClickByJS();
                Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='SchoolNCYearSetMemberships']"));
            }
        }

        public void ScrollToAssociatedSchool()
        {
            if (_associatedAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _associatedAccordion.ClickByJS();
            }
            else
            {
                _associatedAccordion.ClickByJS();
                Wait.WaitLoading();
                _associatedAccordion.ClickByJS();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='SchoolAssociations']"));
        }

        public void ScrollToDocuments()
        {
            if (_documentsAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _documentsAccordion.ClickByJS();
            }
            else
            {
                _documentsAccordion.ClickByJS();
                Wait.WaitLoading();
                _documentsAccordion.ClickByJS();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='SchoolNotes']"));
        }

        public void DeleteGridRowIfExist(GridRow row)
        {
            if (row != null)
            {
                row.DeleteRow();
            }
        }

        public EditAddressDialog EditAddress()
        {
            _editAddressButton.ClickByJS();
            Wait.WaitForDocumentReady();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new EditAddressDialog();
            
        }

        public AddAddressDialogPage AddAddress()
        {
            _addNewAddressButton.ClickByJS();
            Wait.WaitForDocumentReady();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddAddressDialogPage();
           
        }

        public PickAssociatedSchoolTriplet AddAssociatedSchool()
        {
            _addAssociatedSchoolButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new PickAssociatedSchoolTriplet();
        }

        public void ClickAddCurriculumYearLink()
        {
            _addCurriculumYearLink.ClickByJS();
            _addCurriculumYearLink.WaitUntilState(ElementState.Enabled);
        }

        public void BasicCommonDetails()
        {


        }
        #endregion

        #region Grid

        public GridComponent<CurriculumYear> CurriculumYears
        {
            get
            {
                GridComponent<CurriculumYear> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<CurriculumYear>(By.CssSelector("[data-maintenance-container='SchoolNCYearSetMemberships']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class CurriculumYear : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='SchoolNCYear.dropdownImitator']")]
            private IWebElement _ncyear;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            public string NCYear
            {
                set { _ncyear.EnterForDropDown(value); }
                get { return _ncyear.GetValue(); }
            }

            public string StartDate
            {
                set { _startDate.SetDateTime(value); }
                get { return _startDate.GetDateTime(); }
            }

            public string EndDate
            {
                set { _endDate.SetDateTime(value); }
                get { return _endDate.GetDateTime(); }
            }
        }

        public GridComponent<AssociatedSchool> AssociatedSchools
        {
            get
            {
                GridComponent<AssociatedSchool> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AssociatedSchool>(By.CssSelector("[data-maintenance-container='SchoolAssociations']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class AssociatedSchool : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='SchoolAssociationsAssociatedSchool']")]
            private IWebElement _associatedSchool;

            [FindsBy(How = How.CssSelector, Using = "[name$='AssociationType.dropdownImitator']")]
            private IWebElement _associationType;

            [FindsBy(How = How.CssSelector, Using = "[name$='AssociatedSchoolDENI']")]
            private IWebElement _DENINumber;

            public string AssociatedSchoolValue
            {
                get { return _associatedSchool.GetValue(); }
            }

            public string AssociationType
            {
                set { _associationType.EnterForDropDown(value); }
                get { return _associationType.GetValue(); }
            }

            public string DENINumber
            {
                get { return _DENINumber.GetValue(); }
            }
        }

        #endregion
    }
}
