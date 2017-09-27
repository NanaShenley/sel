using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Admission
{
	public class ApplicationPage : BaseComponent
	{
		public override By ComponentIdentifier
		{
			get { return SimsBy.AutomationId("pupil_record_detail"); }
		}

		public static ApplicationPage Create()
		{
			Wait.WaitUntilDisplayed(By.CssSelector("[data-automation-id='well_know_action_save']"));
			return new ApplicationPage();
		}

		#region Page properties

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
		private IWebElement _saveButton;

		[FindsBy(How = How.Name, Using = "LegalForename")]
		private IWebElement _legalForeNameTextBox;

		[FindsBy(How = How.Name, Using = "LegalMiddleNames")]
		private IWebElement _middleNameTextBox;

		[FindsBy(How = How.Name, Using = "LegalSurname")]
		private IWebElement _legalSureNameTextBox;

		[FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
		private IWebElement _genderDropDown;

		[FindsBy(How = How.Name, Using = "DateOfBirth")]
		private IWebElement _dateOfBirthTextBox;

		[FindsBy(How = How.Name, Using = "Age")]
		private IWebElement _ageTextBox;

		[FindsBy(How = How.Id, Using = "BirthCertificateSeen")]
		private IWebElement _birthCertificateSeenCheckbox;

		[FindsBy(How = How.Name, Using = "LearnerApplication.ApplicationStatusSelector.dropdownImitator")]
		private IWebElement _applicationStatusDropDown;

		[FindsBy(How = How.Id, Using = "LearnerApplication.IsLateApplication")]
		private IWebElement _lateApplicationCheckBox;

		[FindsBy(How = How.Name, Using = "LearnerApplication.AdmissionGroupSelector.dropdownImitator")]
		private IWebElement _admissionGroupDropDown;

		[FindsBy(How = How.Name, Using = "LearnerApplication.DateOfAdmission")]
		private IWebElement _dateOfAdmissionTextBox;

		[FindsBy(How = How.Name, Using = "LearnerApplication.AgeOnEntry")]
		private IWebElement _ageOnEntryTextBox;

		[FindsBy(How = How.Name, Using = "YearGroupName")]
		private IWebElement _yearGroupTextBox;

		[FindsBy(How = How.Name, Using = "LearnerApplication.ProposedEnrolmentStatusSelector.dropdownImitator")]
		private IWebElement _ProposedEnrolmentStatusDropDown;

		[FindsBy(How = How.Name, Using = "AdmissionNumber")]
		private IWebElement _admissionNumberTextBox;

		[FindsBy(How = How.Name, Using = "UPN")]
		private IWebElement _uniquePupilNumberTextBox;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='generate_upn_jobstep_button']")]
		private IWebElement _generateUPNButton;

		[FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='Addresses']")]
		private IWebElement _addressTable;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_address_button']")]
		private IWebElement _addAnAdditionalAddressButton;

		[FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='LearnerContactRelationships']")]
		private IWebElement _contactTable;

		[FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='FamilyLinks']")]
		private IWebElement _familyLinkTable;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_contact_button']")]
		private IWebElement _addContactButton;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
		private IWebElement _statusMessage;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Addresses']")]
		private IWebElement _addressTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Registration']")]
		private IWebElement _registrationTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Family/Contacts']")]
		private IWebElement _familyHomeTab;

		// Define Address Table
		public GridComponent<Addresses> AddressesGrid
		{
			get
			{
				GridComponent<Addresses> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<Addresses>(By.CssSelector("[data-maintenance-container='Addresses']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class Addresses
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='LearnerAddressesAddress']")]
			private IWebElement _addressTextBox;

			[FindsBy(How = How.CssSelector, Using = "[name$='AddressStatus']")]
			private IWebElement _addressStatusTextBox;

			[FindsBy(How = How.CssSelector, Using = "[name$='AddressType.dropdownImitator']")]
			private IWebElement _addressTypeDropDown;

			[FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
			private IWebElement _startDateTextBox;

			[FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
			private IWebElement _endDateTextBox;

			public string Address
			{
				set { _addressTextBox.SetText(value); }
				get { return _addressTextBox.GetValue(); }
			}

			public string AddressStatus
			{
				set { _addressStatusTextBox.SetText(value); }
				get { return _addressStatusTextBox.GetValue(); }
			}

			public string AddressType
			{
				set { _addressTypeDropDown.EnterForDropDown(value); }
				get { return _addressTypeDropDown.GetValue(); }
			}

			public string StartDate
			{
				set { _startDateTextBox.SetDateTime(value); }
				get { return _startDateTextBox.GetDateTime(); }
			}

			public string EndDate
			{
				set { _endDateTextBox.SetDateTime(value); }
				get { return _endDateTextBox.GetDateTime(); }
			}

		}

		// Define Contact Table

		public GridComponent<Contacts> ContactsGrid
		{
			get
			{
				GridComponent<Contacts> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<Contacts>(By.CssSelector("[data-maintenance-container='LearnerContactRelationships']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class Contacts
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='Priority']")]
			private IWebElement _priorityTextBox;

			[FindsBy(How = How.CssSelector, Using = "[name$='LearnerContactRelationshipsLearnerContact']")]
			private IWebElement _nameTextBox;

			[FindsBy(How = How.CssSelector, Using = "[name$='LearnerContactRelationshipType.dropdownImitator']")]
			private IWebElement _relationShipDropDown;

			[FindsBy(How = How.CssSelector, Using = "[name$='MainNumber']")]
			private IWebElement _mainTelephoneTextBox;

			[FindsBy(How = How.CssSelector, Using = "[id$='HasCourtOrder']")]
			private IWebElement _courtOrderCheckBox;

			[FindsBy(How = How.CssSelector, Using = "[name$='HasParentalResponsibility']")]
			private IWebElement _parentalResponsibilityCheckBox;

			public string Priority
			{
				set { _priorityTextBox.SetText(value); }
				get { return _priorityTextBox.GetValue(); }
			}

			public string Name
			{
				set { _nameTextBox.SetText(value); }
				get { return _nameTextBox.GetValue(); }
			}

			public string RelationShip
			{
				set { _relationShipDropDown.EnterForDropDown(value); }
				get { return _relationShipDropDown.GetValue(); }
			}
			public string MainTelephone
			{
				set { _mainTelephoneTextBox.SetText(value); }
				get { return _mainTelephoneTextBox.GetValue(); }
			}
			public bool CourtOrder
			{
				set { _courtOrderCheckBox.Set(value); }
				get { return _courtOrderCheckBox.IsChecked(); }
			}
			public bool ParentalResponsibility
			{
				set { _parentalResponsibilityCheckBox.Set(value); }
				get { return _parentalResponsibilityCheckBox.IsChecked(); }
			}

		}

		// Define Family Link Table
		public GridComponent<FamilyLinks> FamilyLinksGrid
		{
			get
			{
				GridComponent<FamilyLinks> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<FamilyLinks>(By.CssSelector("[data-maintenance-container='FamilyLinks']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class FamilyLinks
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='PreferredListName']")]
			private IWebElement _NameTextBox;


			public string Name
			{
				get { return _NameTextBox.GetValue(); }
			}

		}

		public string LegalForeName
		{
			set { _legalForeNameTextBox.SetText(value); }
			get { return _legalForeNameTextBox.GetValue(); }
		}

		public string MiddleName
		{
			set { _middleNameTextBox.SetText(value); }
			get { return _middleNameTextBox.GetValue(); }
		}

		public string LegalSureName
		{
			set { _legalSureNameTextBox.SetText(value); }
			get { return _legalSureNameTextBox.GetValue(); }
		}

		public string Gender
		{
			set { _genderDropDown.EnterForDropDown(value); }
			get { return _genderDropDown.GetValue(); }
		}

		public string DateOfBirth
		{
			set { _dateOfBirthTextBox.SetDateTime(value); }
			get { return _dateOfBirthTextBox.GetDateTime(); }
		}

		public string Age
		{
			get { return _ageTextBox.GetValue(); }
		}

		public bool VerifyDisable()
		{
			return _ageTextBox.GetAttribute("readonly").Equals("true") && _ageOnEntryTextBox.GetAttribute("readonly").Equals("true");
		}

		public bool BirthCertificateSeen
		{
			set { _birthCertificateSeenCheckbox.Set(value); }
			get { return _birthCertificateSeenCheckbox.IsChecked(); }
		}
		public string ApplicationStatus
		{
			set { _applicationStatusDropDown.EnterForDropDown(value); }
			get { return _applicationStatusDropDown.GetValue(); }
		}

		public bool LateApplication
		{
			set { _lateApplicationCheckBox.Set(value); }
			get { return _lateApplicationCheckBox.IsChecked(); }
		}
		public string AdmissionGroup
		{
			set { _admissionGroupDropDown.EnterForDropDown(value); }
			get { return _admissionGroupDropDown.GetValue(); }
		}

		public string AdmissionNumber
		{
			get { return _admissionNumberTextBox.GetValue(); }
		}

		public string DateOfAdmission
		{
			set { _dateOfAdmissionTextBox.SetDateTime(value); }
			get { return _dateOfAdmissionTextBox.GetDateTime(); }
		}

		public string AgeOnEntry
		{
			get { return _ageOnEntryTextBox.GetValue(); }
		}

		public string YearGroup
		{
			get { return _yearGroupTextBox.GetValue(); }
		}

		public string ProposedEnrolmentStatus
		{
			set { _ProposedEnrolmentStatusDropDown.EnterForDropDown(value); }
			get { return _ProposedEnrolmentStatusDropDown.GetValue(); }
		}
		public string UniquePupilNumber
		{
			get { return _uniquePupilNumberTextBox.GetValue(); }
		}

		#endregion

		#region Actions

		//public void ClickSave()
		//{
		//    _saveButton.Click();
		//    Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
		//    Refresh();
		//}

		public void SaveApplicant()
		{
			Wait.WaitUntilDisplayed(By.CssSelector("[data-automation-id='well_know_action_save']"));
			_saveButton.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			Wait.WaitForControl(By.CssSelector("[data-automation-id='status_success']"));
			Refresh();
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


		public void GenerateUPN()
		{

			_generateUPNButton.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

		}

		public void SelectRegistrationSection()
		{
			_registrationTab.ClickByJS();
		}
		public void SelectAddressSection()
		{
			_addressTab.ClickByJS();
		}

		public void SelectFamilyHomeTab()
		{
			_familyHomeTab.ClickByJS();
		}
		public AddressDialog AddAddress()
		{
			SelectAddressSection();
			_addAnAdditionalAddressButton.ClickByJS();
			Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
			return new AddressDialog();
		}

		public ContactTripletDialog AddFamilyHome()
		{
			SelectFamilyHomeTab();
			_addContactButton.ClickByJS();
			Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
			return new ContactTripletDialog();
		}

		public bool IsSuccessMessageIsDisplayed()
		{
			//Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_success']"));
			return SeleniumHelper.DoesWebElementExist(SimsBy.CssSelector("[data-automation-id='status_success']"));
		}

		#endregion

	}
}
