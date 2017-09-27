using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using SeSugar.Automation;
using WebDriverRunner.webdriver;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;
#pragma warning disable 649

namespace POM.Components.Pupil
{
	public class PupilRecordPage : BaseComponent
	{
		public override By ComponentIdentifier
		{
			get { return SimsBy.AutomationId("pupil_record_detail"); }
		}

		public static PupilRecordPage LoadPupilDetail(Guid pupilId)
		{
			var jsExecutor = (IJavaScriptExecutor)SeSugar.Environment.WebContext.WebDriver;
			string js = "sims_commander.OpenDetail(undefined, '/{0}/Pupils/SIMS8LearnerMaintenanceSimpleLearner/ReadDetail/{1}')";

			Retry.Do(() => { jsExecutor.ExecuteScript(string.Format(js, TestSettings.TestDefaults.Default.Path, pupilId)); });

			AutomationSugar.WaitForAjaxCompletion();

			return new PupilRecordPage();
		}

		public static PupilRecordPage Create()
		{
			Wait.WaitUntilDisplayed(By.CssSelector("[data-automation-id='well_know_action_save']"));
			return new PupilRecordPage();
		}

		#region Properties

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
		private IWebElement _savePupilButton;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
		private IWebElement _successMessage;

		[FindsBy(How = How.Name, Using = "LegalSurname")]
		private IWebElement _legalSurnameTextbox;

		[FindsBy(How = How.Name, Using = "LegalMiddleNames")]
		private IWebElement _middleNameTextBox;

		[FindsBy(How = How.Name, Using = "LegalForename")]
		private IWebElement _legalForenameTextBox;

		[FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
		private IWebElement _genderDropDown;

		[FindsBy(How = How.Name, Using = "DateOfBirth")]
		private IWebElement _DOBTextBox;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='pupil_delete_button']")]
		private IWebElement _deleteButton;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
		private IWebElement _continueDeleteButton;

		[FindsBy(How = How.Name, Using = "QuickNote")]
		private IWebElement _quickNoteTextBox;

		[FindsBy(How = How.Name, Using = "PreferredSurname")]
		private IWebElement _preferSurnameTextbox;

		[FindsBy(How = How.Name, Using = "PreferredForename")]
		private IWebElement _preferForenameTextBox;

		[FindsBy(How = How.Name, Using = "BirthCertificateSeen")]
		private IWebElement _birthCertificateSeenCheckBox;

		[FindsBy(How = How.Name, Using = "GenerateUPN")]
		private IWebElement _GenerateUPNButton;

		[FindsBy(How = How.Name, Using = "UPN")]
		private IWebElement _UPNTextbox;

		[FindsBy(How = How.CssSelector, Using = "[title = 'Actions']")]
		private IWebElement _actionLink;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'service_navigation_contextual_link_Pupil_Leaving_Details']")]
		private IWebElement _pupilLeavingDetailsLink;

		[FindsBy(How = How.Name, Using = "GenerateParentalSalutation")]
		private IWebElement _parentalSalutationButton;

		[FindsBy(How = How.Name, Using = "GenerateParentalAddressee")]
		private IWebElement _parentalAddresseeButton;

		[FindsBy(How = How.Name, Using = "ParentalSalutation")]
		private IWebElement _parentalSalutationField;

		[FindsBy(How = How.Name, Using = "ParentalAddressee")]
		private IWebElement _parentalAddresseeField;

		[FindsBy(How = How.Name, Using = "IsMailingPoint")]
		private IWebElement _mailingPointCheckbox;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='clone_contact_button']")]
		private IWebElement _cloneContactLink;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='copy_contact_button']")]
		private IWebElement _copyContactLink;

		[FindsBy(How = How.Name, Using = "GenerateParentalSalutation")]
		private IWebElement _generateSalutationButton;

		[FindsBy(How = How.Name, Using = "GenerateParentalAddressee")]
		private IWebElement _generateAddresseeButton;

		[FindsBy(How = How.Name, Using = "LearnerReceivesPupilPremium")]
		private IWebElement _pupilPremiumIndicator;

		[FindsBy(How = How.Name, Using = "LearnerEligibleForPupilPremium")]
		private IWebElement _pupilPremiumEligibility;

		[FindsBy(How = How.Name, Using = "DoNotDisclose")]
		private IWebElement _pupilDoNotDisclose;

		public string PupilPremiumIndicator
		{
			get { return _pupilPremiumIndicator.GetValue(); }
			set { _pupilPremiumIndicator.SetText(value); }
		}

		public string PupilPremiumEligibility
		{
			get { return _pupilPremiumEligibility.GetValue(); }
			set { _pupilPremiumEligibility.SetText(value); }
		}

		public string UniquePupilNumber
		{
			get { return _UPNTextbox.GetValue(); }
			set { _UPNTextbox.SetText(value); }
		}

		public bool MailingPoint
		{
			get { return _mailingPointCheckbox.IsChecked(); }
			set { _mailingPointCheckbox.Set(value); }
		}

		public string ParentalSalutation
		{
			get { return _parentalSalutationField.GetValue(); }
		}

		public string ParentalAddress
		{
			get { return _parentalAddresseeField.GetValue(); }
		}

		public bool BirthCertificateSeen
		{
			get { return _birthCertificateSeenCheckBox.IsChecked(); }
			set
			{
				_birthCertificateSeenCheckBox.Click();
				_birthCertificateSeenCheckBox.Set(value);
			}
		}

		public string LegalSurname
		{
			get { return _legalSurnameTextbox.GetValue(); }
			set { _legalSurnameTextbox.SetText(value); }
		}

		public string LegalForeName
		{
			get { return _legalForenameTextBox.GetAttribute("value"); }
			set { _legalForenameTextBox.SetText(value); }
		}

		public string MiddleName
		{
			get { return _middleNameTextBox.GetAttribute("value"); }
			set { _middleNameTextBox.SetText(value); }
		}

		public string Gender
		{
			get { return _genderDropDown.GetAttribute("value"); }
		}

		public string DOB
		{
			get { return _DOBTextBox.GetDateTime(); }
			set { _DOBTextBox.SetDateTime(value); }
		}

		public string QuickNote
		{
			get { return _quickNoteTextBox.GetText(); }
			set
			{
				_quickNoteTextBox.SetText(value);
				_quickNoteTextBox.SendKeys(Keys.Tab);
				Wait.WaitForDocumentReady();
			}
		}

		public string PreferSurname
		{
			get { return _preferSurnameTextbox.GetValue(); }
			set { _preferSurnameTextbox.SetText(value); }
		}

		public string PreferForeName
		{
			get { return _preferForenameTextBox.GetAttribute("value"); }
			set { _preferForenameTextBox.SetText(value); }
		}

		public bool DoNotDisclose
		{
			get { return _pupilDoNotDisclose.IsChecked(); }
			set
			{
				_pupilDoNotDisclose.Click();
				_pupilDoNotDisclose.Set(value);
			}
		}

		#endregion Properties

		#region Table

		public GridComponent<LearnerPreviousSchoolsRow> LearnerPreviousSchools
		{
			get
			{
				GridComponent<LearnerPreviousSchoolsRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<LearnerPreviousSchoolsRow>(By.CssSelector("[data-maintenance-container='LearnerPreviousSchools']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class LearnerPreviousSchoolsRow : GridRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
			private IWebElement _startDate;

			[FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
			private IWebElement _endDate;

			[FindsBy(How = How.CssSelector, Using = "[name$='EnrolmentStatus.dropdownImitator']")]
			private IWebElement _enrolmentStatusField;

			[FindsBy(How = How.CssSelector, Using = "[name$='ReasonForLeaving.dropdownImitator']")]
			private IWebElement _reasonForLeavingField;

			[FindsBy(How = How.CssSelector, Using = "[name$='.EducationEstablishmentName']")]
			private IWebElement _schoolName;

			[FindsBy(How = How.CssSelector, Using = "[name$='.LearnerPreviousSchoolAttendanceSummaryCount']")]
			private IWebElement _attendanceSummaryCount;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit_school_history_button']")]
			private IWebElement _editSchoolHistoryButton;

			public string StartDate
			{
				set
				{
					_startDate.SetDateTime(value);
					_startDate.SendKeys(Keys.Enter);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _startDate.GetDateTime();
				}
			}

			public string EndDate
			{
				set
				{
					_endDate.SetDateTime(value);
					_endDate.SendKeys(Keys.Enter);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _endDate.GetDateTime();
				}
			}

			public string EnrolmentStatus
			{
				set { _enrolmentStatusField.ChooseSelectorOption(value); }
				get { return _enrolmentStatusField.GetValue(); }
			}

			public string ReasonForLeaving
			{
				set { _reasonForLeavingField.ChooseSelectorOption(value); }
				get { return _reasonForLeavingField.GetValue(); }
			}

			public string SchoolName
			{
				get { return _schoolName.GetValue(); }
			}

			public string AttendanceSummaryCount
			{
				get { return _attendanceSummaryCount.GetValue(); }
			}

			public SchoolHistoryDialog OpenEditSchoolHistoryDialog()
			{
				_editSchoolHistoryButton.ClickByJS();
				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
				return new SchoolHistoryDialog();
			}
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
				Wait.WaitForElementDisplayed(SimsBy.CssSelector("[name$='.Note']"));
				IWebElement _noteTextArea = SeleniumHelper.FindElement(SimsBy.CssSelector("[name$='.Note']"));
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
				Retry.Do(() =>
				{
					returnValue = new GridComponent<AddressRow>(By.CssSelector("[data-maintenance-container='Addresses']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class TelephoneNumberRow : GridRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='TelephoneNumber']")]
			private IWebElement _telephoneNumber;

			[FindsBy(How = How.CssSelector, Using = "[name$='LocationType.dropdownImitator']")]
			private IWebElement _location;

			[FindsBy(How = How.CssSelector, Using = "[name*='LearnerTelephones'][name$='UseForTextMessages']")]
			private IWebElement _ams;

			[FindsBy(How = How.CssSelector, Using = "[name*='LearnerTelephones'][name$='IsMainTelephone']")]
			private IWebElement _mainNumber;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
			private IWebElement _addNoteButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
			private IWebElement _note;

			[FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
			private IWebElement _blankCell;

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
				}
				get { return _location.GetAttribute("value"); }
			}

			public bool AMS
			{
				set
				{
					_ams.Set(value);
				}
				get { return _ams.IsChecked(); }
			}

			public bool MainNumber
			{
				set
				{
					_mainNumber.Set(value);
				}
				get { return _mainNumber.IsChecked(); }
			}

			public string Note
			{
				set
				{
					_addNoteButton.ClickByJS();
					_note.SetText(value);
				}
				get
				{
					_addNoteButton.ClickByJS();
					string result = _note.GetText();
					return result;
				}
			}
		}

		public GridComponent<TelephoneNumberRow> TelephoneNumberTable
		{
			get
			{
				GridComponent<TelephoneNumberRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<TelephoneNumberRow>(By.CssSelector("[data-maintenance-container='LearnerTelephones']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class EmailRow : GridRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='EmailAddress']")]
			private IWebElement _emailAddress;

			[FindsBy(How = How.CssSelector, Using = "[name$='LocationType.dropdownImitator']")]
			private IWebElement _location;

			[FindsBy(How = How.CssSelector, Using = "[name*='LearnerEmails'][name$='UseForTextMessages']")]
			private IWebElement _ams;

			[FindsBy(How = How.CssSelector, Using = "[name*='LearnerEmails'][name$='IsMainEmail']")]
			private IWebElement _mainEmail;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
			private IWebElement _addNoteButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
			private IWebElement _note;

			[FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
			private IWebElement _blankCell;

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
				}
				get { return _location.GetAttribute("value"); }
			}

			public bool AMS
			{
				set
				{
					_ams.Set(value);
				}
				get { return _ams.IsChecked(); }
			}

			public bool MainEmail
			{
				set
				{
					_mainEmail.Set(value);
				}
				get { return _mainEmail.IsChecked(); }
			}

			public string Note
			{
				set
				{
					_addNoteButton.ClickByJS();
					_note.SetText(value);
				}
				get
				{
					_addNoteButton.ClickByJS();
					string result = _note.GetText();
					return result;
				}
			}
		}

		public GridComponent<PupilRecordPage.EmailRow> EmailTable
		{
			get
			{
				GridComponent<EmailRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<EmailRow>(By.CssSelector("[data-maintenance-container='LearnerEmails']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public GridComponent<ContactRow> ContactTable
		{
			get
			{
				GridComponent<ContactRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<ContactRow>(By.CssSelector("[data-maintenance-container='LearnerContactRelationships']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class ContactRow : GridRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='Priority']")]
			private IWebElement _priority;

			[FindsBy(How = How.CssSelector, Using = "[id$='LearnerContactRelationshipType_dropdownImitator']")]
			private IWebElement _relationShip;

			[FindsBy(How = How.CssSelector, Using = "[name$='HasParentalResponsibility']")]
			private IWebElement _parentalReponsibility;

			[FindsBy(How = How.CssSelector, Using = "[name$='LearnerContactRelationshipsLearnerContact']")]
			private IWebElement _name;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
			private IWebElement _edit;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='remove_button']")]
			private IWebElement _remove;

			[FindsBy(How = How.CssSelector, Using = "[name$='DoNotDisclose']")]
			private IWebElement _doNotDisclose;

			#region Properties

			public string Priority
			{
				get { return _priority.Text; }
				set
				{
					Retry.Do(_priority.Click);
					_priority.SetText(value);
				}
			}

			public string RelationShip
			{
				get { return _relationShip.GetAttribute("value"); }
				set
				{
					_relationShip.ClickByJS();
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
					_relationShip.EnterForDropDown(value);
				}
			}

			public bool ParentalReponsibility
			{
				set { _parentalReponsibility.Set(value); }
				get { return _parentalReponsibility.IsCheckboxChecked(); }
			}

			public bool DoNotDisclose
			{
				set { _doNotDisclose.Set(value); }
				get { return _doNotDisclose.IsCheckboxChecked(); }
			}

			public string Name
			{
				set { _name.SetText(value); }
				get { return _name.GetValue(); }
			}

			#endregion Properties

			#region Action

			public void Edit()
			{
				_edit.Click();
				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			}

			#endregion Action
		}

		[FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='FreeMealEligibilities']")]
		private IWebElement _eligibleFreeMealTable;

		public GridComponent<EligibleFreeMealRow> EligibleFreeMeal
		{
			get
			{
				GridComponent<EligibleFreeMealRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<EligibleFreeMealRow>(By.CssSelector("[data-maintenance-container='FreeMealEligibilities']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class EligibleFreeMealRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
			private IWebElement _startDateDatePicker;

			[FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
			private IWebElement _endDateDatePicker;

			[FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
			private IWebElement _columnEnd;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
			private IWebElement _addNoteButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
			private IWebElement _noteArea;

			public string StartDate
			{
				set
				{
					_startDateDatePicker.ClickByJS();
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
					_startDateDatePicker.SetDateTime(value);

					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _startDateDatePicker.GetDateTime();
				}
			}

			public string EndDate
			{
				set
				{
					_endDateDatePicker.SetDateTime(value);
					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _endDateDatePicker.GetDateTime();
				}
			}

			public string Note
			{
				set
				{
					_addNoteButton.ClickByJS();
					_noteArea.SetText(value);
					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					_addNoteButton.ClickByJS();
					string value = _noteArea.GetText();
					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
					return value;
				}
			}
		}

		/// <summary>
		/// MealPattern
		/// </summary>

		public GridComponent<MealPatternRow> MealPattern
		{
			get
			{
				GridComponent<MealPatternRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<MealPatternRow>(By.CssSelector("[data-maintenance-container='LearnerMealPatterns']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class MealPatternRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
			private IWebElement _startDateDatePicker;

			[FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
			private IWebElement _endDateDatePicker;

			[FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
			private IWebElement _columnEnd;

			public string StartDate
			{
				set
				{
					_startDateDatePicker.ClickByJS();
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
					_startDateDatePicker.SetDateTime(value);

					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _startDateDatePicker.GetDateTime();
				}
			}

			public string EndDate
			{
				set
				{
					_endDateDatePicker.SetDateTime(value);
					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _endDateDatePicker.GetDateTime();
				}
			}
		}

		public GridComponent<MedicalPracticeRow> MedicalPractice
		{
			get
			{
				GridComponent<MedicalPracticeRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<MedicalPracticeRow>(By.CssSelector("[data-maintenance-container='LearnerMedicalPractices']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class MedicalPracticeRow : GridRow
		{
			[FindsBy(How = How.CssSelector, Using = "[id$='Doctor_dropdownImitator']")]
			private IWebElement _doctorDropdown;

			[FindsBy(How = How.CssSelector, Using = "[name$='Name']")]
			private IWebElement _practiceName;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
			private IWebElement _editLink;

			public string Doctor
			{
				get { return _doctorDropdown.GetValue(); }
				set
				{
					_doctorDropdown.EnterForDropDown(value);
				}
			}

			public string Name
			{
				get { return _practiceName.GetValue(); }
			}

			public EditMedicalPracticeDialog ClickEdit()
			{
				if (_editLink.IsExist())
				{
					_editLink.ClickByJS();
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
				}
				return new EditMedicalPracticeDialog();
			}
		}

		public GridComponent<MedicalNoteRow> MedicalNote
		{
			get
			{
				GridComponent<MedicalNoteRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<MedicalNoteRow>(By.CssSelector("[data-maintenance-container='LearnerMedicalNotes']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class MedicalNoteRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='Summary']")]
			private IWebElement _summary;

			[FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
			private IWebElement _columnEnd;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
			private IWebElement _addNoteButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
			private IWebElement _textNoteArea;

			[FindsBy(How = How.CssSelector, Using = "[data-note-editor-is-empty-message='Add Note']")]
			private IWebElement _textNotedIsAdded;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id*='documents']")]
			private IWebElement _documentButton;

			public string Summary
			{
				set
				{
					_summary.SetText(value);
					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get { return _summary.GetValue(); }
			}

			public string Note
			{
				set
				{
					_addNoteButton.ClickByJS();
					_textNoteArea.SetText(value);
					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _textNotedIsAdded.GetAttribute("title");
				}
			}

			public void AddDocument()
			{
				_documentButton.Click();
				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			}
		}

		public GridComponent<MedicalConditionRow> MedicalCondition
		{
			get
			{
				GridComponent<MedicalConditionRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<MedicalConditionRow>(By.CssSelector("[data-maintenance-container='LearnerMedicalConditions']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class MedicalConditionRow
		{
			[FindsBy(How = How.CssSelector, Using = "[id$='MedicalCondition_dropdownImitator']")]
			private IWebElement _descriptionDropDown;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id*='documents']")]
			private IWebElement _documentButton;

			public string Description
			{
				get { return _descriptionDropDown.GetValue(); }
				set
				{
					_descriptionDropDown.EnterForDropDown(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
			}

			public void AddDocument()
			{
				_documentButton.Click();
				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			}
		}

		public GridComponent<MedicalEventRow> MedicalEvent
		{
			get
			{
				GridComponent<MedicalEventRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<MedicalEventRow>(By.CssSelector("[data-maintenance-container='LearnerMedicalEvents']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class MedicalEventRow
		{
			[FindsBy(How = How.CssSelector, Using = "[id$='MedicalEventType_dropdownImitator']")]
			private IWebElement _medicalTypeDropDown;

			[FindsBy(How = How.CssSelector, Using = "[id$='MedicalEventDescription_dropdownImitator']")]
			private IWebElement _medicalEventDescriptionDropDown;

			[FindsBy(How = How.CssSelector, Using = "[name$='EventDate']")]
			private IWebElement _eventDateField;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id*='documents']")]
			private IWebElement _documentButton;

			public string Type
			{
				get { return _medicalTypeDropDown.GetValue(); }
				set
				{
					_medicalTypeDropDown.EnterForDropDown(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
			}

			public string Description
			{
				get { return _medicalEventDescriptionDropDown.GetText(); }
				set
				{
					_medicalEventDescriptionDropDown.EnterForDropDown(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
			}

			public string EventDate
			{
				get { return _eventDateField.GetDateTime(); }
				set
				{
					_eventDateField.SetDateTime(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
			}

			public void AddDocument()
			{
				_documentButton.Click();
				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			}
		}

		public GridComponent<LearnerNewcomerPeriodsRow> LearnerNewcomerPeriods
		{
			get
			{
				GridComponent<LearnerNewcomerPeriodsRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<LearnerNewcomerPeriodsRow>(By.CssSelector("[data-maintenance-container='LearnerNewcomerPeriods']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class LearnerNewcomerPeriodsRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$=StartDate]")]
			private IWebElement _startDatePicker;

			[FindsBy(How = How.CssSelector, Using = "[name$=EndDate]")]
			private IWebElement _endDatePicker;

			public string StartDate
			{
				set { _startDatePicker.SetDateTime(value); }
				get { return _startDatePicker.GetDateTime(); }
			}

			public string EndDate
			{
				set { _endDatePicker.SetDateTime(value); }
				get { return _endDatePicker.GetDateTime(); }
			}
		}

		public GridComponent<LearnerZeroRatedInformationsTableRow> LearnerZeroRatedInformationsTable
		{
			get
			{
				GridComponent<LearnerZeroRatedInformationsTableRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<LearnerZeroRatedInformationsTableRow>(By.CssSelector("[data-maintenance-container='LearnerZeroRatedInformations']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class LearnerZeroRatedInformationsTableRow
		{
			[FindsBy(How = How.CssSelector, Using = "[id$='AcademicYear_dropdownImitator']")]
			private IWebElement _academicYearField;

			public string AcademicYear
			{
				set
				{
					_academicYearField.EnterForDropDown(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _academicYearField.GetValue();
				}
			}
		}

		public GridComponent<LearnerUniformGrantEligibilitiesRow> LearnerUniformGrantEligibilities
		{
			get
			{
				GridComponent<LearnerUniformGrantEligibilitiesRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<LearnerUniformGrantEligibilitiesRow>(By.CssSelector("[data-maintenance-container='LearnerUniformGrantEligibilities']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class LearnerUniformGrantEligibilitiesRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
			private IWebElement _startDateField;

			[FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
			private IWebElement _columnEnd;

			[FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
			private IWebElement _endDateField;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
			private IWebElement _addNoteButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
			private IWebElement _textNoteArea;

			[FindsBy(How = How.CssSelector, Using = "[data-note-editor-is-empty-message='Add Note']")]
			private IWebElement _textNoteAreaAdded;

			public string StartDate
			{
				set
				{
					_startDateField.SetDateTime(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _startDateField.GetDateTime();
				}
			}

			public string EndDate
			{
				set
				{
					_endDateField.SetDateTime(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _endDateField.GetDateTime();
				}
			}

			public string Note
			{
				set
				{
					_addNoteButton.ClickByJS();

					_textNoteArea.SetText(value);
					Retry.Do(_columnEnd.Click);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _textNoteAreaAdded.GetAttribute("title");
				}
			}
		}

		[FindsBy(How = How.Name, Using = "DateOfLeaving")]
		private IWebElement _dateOfLeaving;

		public GridComponent<LearnerInCareDetailsRow> LearnerInCareDetails
		{
			get
			{
				GridComponent<LearnerInCareDetailsRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<LearnerInCareDetailsRow>(By.CssSelector("[data-maintenance-container='LearnerInCareDetails']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class LearnerInCareDetailsRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
			private IWebElement _startDateField;

			[FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
			private IWebElement _endDateField;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit_living_arrangement_button']")]
			private IWebElement _editCareArrangementButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='CareAuthority.dropdownImitator']")]
			private IWebElement _careAuthority;

			[FindsBy(How = How.CssSelector, Using = "[name$='LivingArrangement.dropdownImitator']")]
			private IWebElement _livingArrangement;

			public CareArrangementsDialog OpenEditCareArrangementsDialog()
			{
				_editCareArrangementButton.ClickByJS();
				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));

				return new CareArrangementsDialog();
			}

			public string StartDate
			{
				set
				{
					_startDateField.SetDateTime(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _startDateField.GetDateTime();
				}
			}

			public string EndDate
			{
				set
				{
					_endDateField.SetDateTime(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _endDateField.GetDateTime();
				}
			}

			public string CareAuthority
			{
				set
				{
					_careAuthority.EnterForDropDown(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _careAuthority.GetValue();
				}
			}

			public string LivingArrangement
			{
				set
				{
					_livingArrangement.EnterForDropDown(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _livingArrangement.GetValue();
				}
			}
		}

		public GridComponent<LearnerYoungCarerRow> LearnerYoungCarerPeriods
		{
			get
			{
				GridComponent<LearnerYoungCarerRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<LearnerYoungCarerRow>(By.CssSelector("[data-maintenance-container='LearnerYoungCarerRecords']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class LearnerYoungCarerRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
			private IWebElement _startDateField;

			[FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
			private IWebElement _endDateField;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
			private IWebElement _noteButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
			private IWebElement _noteArea;

			public string StartDate
			{
				set
				{
					_startDateField.SetDateTime(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _startDateField.GetDateTime();
				}
			}

			public string EndDate
			{
				set
				{
					_endDateField.SetDateTime(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _endDateField.GetDateTime();
				}
			}

			public string Notes
			{
				set
				{
					Retry.Do(_noteButton.ClickByJS, 100, 20, _noteArea.ClickByJS);
					_noteArea.SetText(value);
					_noteArea.SendKeys(Keys.Tab);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _noteArea.GetValue();
				}
			}
		}

		public GridComponent<LearnerDisabilityRow> LearnerDisabilities
		{
			get
			{
				GridComponent<LearnerDisabilityRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<LearnerDisabilityRow>(By.CssSelector("[data-maintenance-container='LearnerDisabilities']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class LearnerDisabilityRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
			private IWebElement _startDateField;

			[FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
			private IWebElement _endDateField;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
			private IWebElement _noteButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
			private IWebElement _noteArea;

			public string StartDate
			{
				set
				{
					_startDateField.SetDateTime(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _startDateField.GetDateTime();
				}
			}

			public string EndDate
			{
				set
				{
					_endDateField.SetDateTime(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _endDateField.GetDateTime();
				}
			}

			public string Notes
			{
				set
				{
					Retry.Do(_noteButton.ClickByJS, 100, 20, _noteArea.ClickByJS);
					_noteArea.SetText(value);
					_noteArea.SendKeys(Keys.Tab);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _noteArea.GetValue();
				}
			}
		}

		public GridComponent<ConsentsRow> Consents
		{
			get
			{
				GridComponent<ConsentsRow> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<ConsentsRow>(By.CssSelector("[data-maintenance-container='LearnerConsentTypes']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class ConsentsRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name^='LearnerConsentTypes'][type='Checkbox']")]
			private IWebElement _activeCheckbox;

			[FindsBy(How = How.CssSelector, Using = "[id$='ConsentStatus_dropdownImitator']")]
			private IWebElement _consentStatusDropdown;

			[FindsBy(How = How.CssSelector, Using = "[name$='ConsentDate']")]
			private IWebElement _consentDateField;

			[FindsBy(How = How.CssSelector, Using = "[name$='LearnerConsentingAdult']")]
			private IWebElement _learnerConsentingAdult;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
			private IWebElement _addNoteButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='Comments']")]
			private IWebElement _commentArea;

			[FindsBy(How = How.CssSelector, Using = "[name$='ConsentType.ReadOnlySelectorTitle']")]
			private IWebElement _readonlySelectorTitle;

			public bool Active
			{
				set
				{
					_activeCheckbox.Set(value);
				}
				get
				{
					return _activeCheckbox.IsCheckboxChecked();
				}
			}

			public string ConsentStatus
			{
				set
				{
					_consentStatusDropdown.EnterForDropDown(value);
				}
				get
				{
					return _consentStatusDropdown.GetValue();
				}
			}

			public string Date
			{
				set
				{
					_consentDateField.SetDateTime(value);
				}
				get
				{
					return _consentDateField.GetDateTime();
				}
			}

			public string ConsentSignatory
			{
				set
				{
					_learnerConsentingAdult.SetText(value);
				}
				get
				{
					return _learnerConsentingAdult.GetValue();
				}
			}

			public string Note
			{
				set
				{
					_addNoteButton.ClickByJS();
					_commentArea.SetText(value);
				}
				get
				{
					int time = 5;
					string result = String.Empty;
					do
					{
						_addNoteButton.ClickByJS();
						Wait.WaitLoading();
						result = _commentArea.GetText();
						if (result.Equals(String.Empty))
						{
							time--;
						}
						else
						{
							break;
						}
					} while (time > 0);

					_readonlySelectorTitle.Click();
					return result;
				}
			}
		}

		#endregion Table

		#region 'Registration' Section

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='LearnerYearGroupMemberships_grid_editor_dialog_button']")]
		private IWebElement _yearGroupHistoryButton;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='LearnerAttendanceModes_grid_editor_dialog_button']")]
		private IWebElement _attendanceModeHistoryDialogButton;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='LearnerPrimaryClassMemberships_grid_editor_dialog_button']")]
		private IWebElement _classHistoryButton;

		[FindsBy(How = How.Id, Using = "LearnerPrimaryClassMemberships")]
		private IWebElement _classMembershipTextBox;

		[FindsBy(How = How.Id, Using = "LearnerYearGroupMemberships")]
		private IWebElement _yearGroupMembershipTextBox;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_enrolment_status_button']")]
		private IWebElement _addEnrolmentStatusButtton;

		public string Class
		{
			get { return _classMembershipTextBox.GetValue(); }
		}

		public string YearGroup
		{
			get { return _yearGroupMembershipTextBox.GetValue(); }
		}

		public string DateOfLeaving
		{
			get { return _dateOfLeaving.GetDateTime(); }
		}

		public GridComponent<EnrolmentStatusHistory> EnrolmentStatusHistoryTable
		{
			get
			{
				GridComponent<EnrolmentStatusHistory> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<EnrolmentStatusHistory>(By.CssSelector("[data-maintenance-container$='MulitpleLearnerEnrolmentStatus']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class EnrolmentStatusHistory
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
			private IWebElement _startDate;

			[FindsBy(How = How.CssSelector, Using = "[name$='EnrolmentStatus.dropdownImitator']")]
			private IWebElement _enrolmentStatus;

			[FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
			private IWebElement _endDate;

			public string StartDate
			{
				set
				{
					_startDate.SetDateTimeByJS(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get { return _startDate.GetDateTime(); }
			}

			public string EnrolmentStatus
			{
				set
				{
					_enrolmentStatus.EnterForDropDown(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get { return _enrolmentStatus.GetValue(); }
			}

			public string EndDate
			{
				get { return _endDate.GetDateTime(); }
				set
				{
					_endDate.SetDateTimeByJS(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
			}
		}

		#endregion 'Registration' Section

		#region 'Statutory SEN' Section

		public class SENStage
		{
			internal readonly static By SenStageSelector =
				SimsBy.CssSelector("[data-maintenance-container='LearnerSENStatuses']");

			private readonly By _startDaySelector;

			private readonly By _endDaySelector;

			public SENStage()
			{
				var fieldPrefix = FreshElementHelper.GetFieldPrefix(SenStageSelector);

				_startDaySelector = FreshElementHelper.GetStartDateSelector(fieldPrefix);

				_endDaySelector = FreshElementHelper.GetEndDateSelector(fieldPrefix);
			}

			[FindsBy(How = How.CssSelector, Using = "[name$='SENStatus.dropdownImitator']")]
			private IWebElement _stage;

			public string Stage
			{
				set
				{
					_stage.EnterForDropDown(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get { return _stage.GetAttribute("value"); }
			}

			public string StartDay
			{
				get
				{
					return SeleniumHelper.GetDateTime(_startDaySelector);
				}
				set
				{
					FreshElementHelper.SetDateTime(_startDaySelector, value);
				}
			}

			public string EndDay
			{
				get
				{
					return SeleniumHelper.GetDateTime(_endDaySelector);
				}
				set
				{
					FreshElementHelper.SetDateTime(_endDaySelector, value);
				}
			}
		}

		public GridComponent<SENStage> SenStages
		{
			get
			{
				GridComponent<SENStage> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<SENStage>(By.CssSelector("[data-maintenance-container='LearnerSENStatuses']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		/// <summary>
		/// Helps access fresh (not stale) IWebElement instances by reselecting elements for each operation
		/// to mimimise removal of elements from the DOM causing stale element reference exceptions
		/// </summary>
		internal static class FreshElementHelper
		{
			private readonly static By _rowSelector = SimsBy.CssSelector("tbody > tr:first-child");

			private const string FIELD_PREFIX = "data-fieldprefix";

			internal static IWebElement GetChildElement(By parentElementBy, By by)
			{
				var parentElement = ElementRetriever.FindElementSafe(WebContext.WebDriver, parentElementBy);

				return parentElement.FindElementSafe(by);
			}

			internal static void SetDateTime(By by, string value)
			{
				SeleniumHelper.SetDateTimeByJS(by, value);

				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
			}

			internal static string GetFieldPrefix(By containerSelector)
			{
				return ElementRetriever.FindElementSafe(WebContext.WebDriver, containerSelector)
					.FindElementSafe(_rowSelector).GetAttribute(FIELD_PREFIX);
			}

			internal static By GetStartDateSelector(string fieldPrefix)
			{
				return SimsBy.CssSelector("input[name='" + fieldPrefix + ".StartDate']");
			}

			internal static By GetEndDateSelector(string fieldPrefix)
			{
				return SimsBy.CssSelector("input[name='" + fieldPrefix + ".EndDate']");
			}
		}

		public class SENNeed
		{
			internal readonly static By SenNeedSelector = SimsBy.CssSelector("[data-maintenance-container='LearnerSENNeedTypes']");

			private readonly static By _notesButtonSelector = SimsBy.CssSelector("table[data-maintenance-container='LearnerSENNeedTypes'] > tbody > tr > td:nth-child(6) > div > button");

			private readonly By _notesDescriptionSelector;

			private readonly By _startDaySelector;

			private readonly By _endDaySelector;

			[FindsBy(How = How.CssSelector, Using = "[name$='NeedType.dropdownImitator']")]
			private IWebElement _needType;

			[FindsBy(How = How.CssSelector, Using = "[name$='Rank']")]
			private IWebElement _rank;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id*='documents']")]
			private IWebElement _documents;

			public SENNeed()
			{
				var fieldPrefix = FreshElementHelper.GetFieldPrefix(SenNeedSelector);

				_startDaySelector = FreshElementHelper.GetStartDateSelector(fieldPrefix);

				_endDaySelector = FreshElementHelper.GetEndDateSelector(fieldPrefix);

				_notesDescriptionSelector = SimsBy.CssSelector("textarea[name='" + fieldPrefix + ".Description']");
			}

			public string NeedType
			{
				set
				{
					_needType.EnterForDropDown(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get { return _needType.GetAttribute("value"); }
			}

			public string Rank
			{
				set
				{
					//if (_rank.GetAttribute("value").Equals(value))
					//{
					//    return;
					//}
					//Retry.Do(_rank.Click);
					//Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
					_rank.SetText(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get { return _rank.GetAttribute("value"); }
			}

			public string Notes
			{
				set
				{
					SeleniumHelper.FindElement(_notesButtonSelector).Click();

					SeleniumHelper.FindElement(_notesDescriptionSelector).ClearText();

					SeleniumHelper.FindElement(_notesDescriptionSelector).SetText(value);

					SeleniumHelper.FindElement(_notesButtonSelector).Click();

					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					SeleniumHelper.FindElement(_notesButtonSelector).Click();

					return SeleniumHelper.FindElement(_notesDescriptionSelector).Text;
				}
			}

			public string StartDay
			{
				get
				{
					return SeleniumHelper.GetDateTime(_startDaySelector);
				}
				set
				{
					FreshElementHelper.SetDateTime(_startDaySelector, value);
				}
			}

			public string EndDay
			{
				get
				{
					return SeleniumHelper.GetDateTime(_endDaySelector);
				}
				set
				{
					FreshElementHelper.SetDateTime(_endDaySelector, value);
				}
			}


			public void AddDocument()
			{
				_documents.Click();
				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			}
		}

		public GridComponent<SENNeed> SenNeeds
		{
			get
			{
				GridComponent<SENNeed> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<SENNeed>(SENNeed.SenNeedSelector, ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class SENProvsion
		{
			internal readonly static By SenProvisionSelector = SimsBy.CssSelector("[data-maintenance-container='LearnerSENProvisionTypes']");

			[FindsBy(How = How.CssSelector, Using = "[name$='SENProvisionType.dropdownImitator']")]
			private IWebElement _provisionType;

			[FindsBy(How = How.CssSelector, Using = "[name$='Rank']")]
			private IWebElement _rank;

			[FindsBy(How = How.CssSelector, Using = "[data-popup-button]")]
			private IWebElement _commentButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='Comment']")]
			private IWebElement _comment;

			private readonly By _startDaySelector;

			private readonly By _endDaySelector;

			public SENProvsion()
			{
				var fieldPrefix = FreshElementHelper.GetFieldPrefix(SenProvisionSelector);

				_startDaySelector = FreshElementHelper.GetStartDateSelector(fieldPrefix);

				_endDaySelector = FreshElementHelper.GetEndDateSelector(fieldPrefix);
			}

			public string ProvisionType
			{
				set
				{
					_provisionType.EnterForDropDown(value);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get { return _provisionType.GetAttribute("value"); }
			}

			public string StartDay
			{
				get
				{
					return SeleniumHelper.GetDateTime(_startDaySelector);
				}
				set
				{
					FreshElementHelper.SetDateTime(_startDaySelector, value);
				}
			}

			public string EndDay
			{
				get
				{
					return SeleniumHelper.GetDateTime(_endDaySelector);
				}
				set
				{
					FreshElementHelper.SetDateTime(_endDaySelector, value);
				}
			}

			public string Comment
			{
				set
				{
					_commentButton.Click();
					_comment.ClearText();
					_comment.SetText(value);
					_commentButton.Click();
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					_commentButton.Click();
					string comment = _comment.Text;
					_comment.SendKeys(Keys.Tab);
					return comment;
				}
			}
		}

		public GridComponent<SENProvsion> SenProvisions
		{
			get
			{
				GridComponent<SENProvsion> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<SENProvsion>(By.CssSelector("[data-maintenance-container='LearnerSENProvisionTypes']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		#endregion 'Statutory SEN' Section

		#region 'Medical' Section

		[FindsBy(How = How.Name, Using = "NHSNumber")]
		private IWebElement _NHSNumber;

		public string NHSNumber
		{
			set { _NHSNumber.SetText(value); }
			get { return _NHSNumber.GetValue(); }
		}

		[FindsBy(How = How.Name, Using = "IsDisabled")]
		private IWebElement _assessCheckbox;

		public bool Assessed
		{
			set { _assessCheckbox.Set(value); }
			get { return _assessCheckbox.IsCheckboxChecked(); }
		}

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='Seafood allergy']")]
		private IWebElement _seafoodCheckbox;

		public bool SeafoodAllergy
		{
			set { _seafoodCheckbox.Set(value); }
			get { return _seafoodCheckbox.IsCheckboxChecked(); }
		}

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='Vegetarian']")]
		private IWebElement _vegetarian;

		public bool Vegetarian
		{
			set { _vegetarian.Set(value); }
			get { return _vegetarian.IsCheckboxChecked(); }
		}

		#endregion 'Medical' Section

		#region 'Ethnic' Section

		[FindsBy(How = How.Name, Using = "Ethnicity.dropdownImitator")]
		private IWebElement _ethnicity;

		public string Ethnicity
		{
			get { return _ethnicity.GetValue(); }
			set { _ethnicity.EnterForDropDown(value); }
		}

		[FindsBy(How = How.Name, Using = "Religion.dropdownImitator")]
		private IWebElement _religion;

		public string Religion
		{
			get { return _religion.GetValue(); }
			set { _religion.EnterForDropDown(value); }
		}

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='LearnerTravellersDetails_grid_editor_dialog_button']")]
		private IWebElement _accommodationTypeButton;

		[FindsBy(How = How.Name, Using = "AsylumSeeker.dropdownImitator")]
		private IWebElement _asylumStatus;

		public string AsylumStatus
		{
			get { return _asylumStatus.GetValue(); }
			set
			{
				_asylumStatus.EnterForDropDown(value);
			}
		}

		[FindsBy(How = How.Name, Using = "IsTaughtThroughIrishMedium")]
		private IWebElement _IsTaught;

		public bool IsTaughtMedium
		{
			set { _IsTaught.Set(value); }
			get { return _IsTaught.IsCheckboxChecked(); }
		}

		#endregion 'Ethnic' Section

		#region 'Addiional' Section

		[FindsBy(How = How.CssSelector, Using = "[name$='ServiceChildren.dropdownImitator']")]
		private IWebElement _ServiceChildren;

		public string ServiceChildren
		{
			get { return _ServiceChildren.GetValue(); }
			set { _ServiceChildren.EnterForDropDown(value); }
		}

		[FindsBy(How = How.CssSelector, Using = "[name$='ServiceChildrenSource.dropdownImitator']")]
		private IWebElement _ServiceChildrenSource;

		public string ServiceChildrenSource
		{
			get { return _ServiceChildrenSource.GetValue(); }
			set { _ServiceChildrenSource.EnterForDropDown(value); }
		}

		[FindsBy(How = How.CssSelector, Using = "[name$='ModeOfTravel.dropdownImitator']")]
		private IWebElement _ModeOfTravel;

		public string ModeOfTravel
		{
			get { return _ModeOfTravel.GetValue(); }
			set { _ModeOfTravel.EnterForDropDown(value); }
		}

		[FindsBy(How = How.CssSelector, Using = "[name$='TravelRoute.dropdownImitator']")]
		private IWebElement _TravelRoute;

		public string TravelRoute
		{
			get { return _TravelRoute.GetValue(); }
			set { _TravelRoute.EnterForDropDown(value); }
		}

		[FindsBy(How = How.CssSelector, Using = "[name='JobSeekerAllowance']")]
		private IWebElement _JobSeekerAllowance;

		public bool JobSeekerAllowance
		{
			get { return _JobSeekerAllowance.IsCheckboxChecked(); }
			set { _JobSeekerAllowance.Set(value); }
		}

		[FindsBy(How = How.CssSelector, Using = "[name='ELBProvidesTransport']")]
		private IWebElement _ELBProvidesTransport;

		public bool ELBProvidesTransport
		{
			get { return _ELBProvidesTransport.IsCheckboxChecked(); }
			set { _ELBProvidesTransport.Set(value); }
		}

		#endregion 'Addiional' Section

		#region 'School History' Section

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_school_history_button']")]
		private IWebElement _addSchoolHistoryButton;

		#endregion 'School History' Section

		#region 'Documents' Section

		public GridComponent<Document> DocumentNote
		{
			get
			{
				GridComponent<Document> returnValue = null;
				Retry.Do(() =>
				{
					returnValue = new GridComponent<Document>(By.CssSelector("[data-maintenance-container='LearnerNotes']"), ComponentIdentifier);
				});
				return returnValue;
			}
		}

		public class Document : GridRow
		{
			[FindsBy(How = How.CssSelector, Using = "[name$='LastUpdatedDate']")]
			private IWebElement _lastUpdated;

			[FindsBy(How = How.CssSelector, Using = "[name$='Summary']")]
			private IWebElement _summary;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
			private IWebElement _addNoteButton;

			[FindsBy(How = How.CssSelector, Using = "[name$='Note']")]
			private IWebElement _noteTextArea;

			[FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
			private IWebElement _note;

			[FindsBy(How = How.CssSelector, Using = "[data-automation-id*='documents']")]
			private IWebElement _document;

			public string LastUpdated
			{
				get
				{
					return _lastUpdated.GetDateTime();
				}
			}

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
					Retry.Do(_addNoteButton.ClickByJS, 100, 20, _note.ClickByJS);
					_noteTextArea.SetText(value);
					_noteTextArea.SendKeys(Keys.Tab);
					Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
				}
				get
				{
					return _note.GetValue();
				}
			}

			public ViewDocumentDialog AddDocument()
			{
				_document.ClickByJS();
				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
				return new ViewDocumentDialog();
			}
		}

		#endregion 'Documents' Section

		#region LinkAction

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_address_button']")]
		private IWebElement _addAddressButton;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_care_arrangement_button']")]
		private IWebElement _addCareArrangment;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_young_carer_period_button']")]
		private IWebElement _addYoungCarerPeriod;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_contact_button']")]
		private IWebElement _addContactLink;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_medical_practice_button']")]
		private IWebElement _addMedicalPracticeLink;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_telephone_number_button']")]
		private IWebElement _addTelephoneNumber;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_email_address_button']")]
		private IWebElement _addEmailAddress;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_uniform_grant_eligibility_button']")]
		private IWebElement _addUniformGrantEligibility;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_free_school_meal_eligibility_button']")]
		private IWebElement _addFreeSchoolMealEligibility;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_meal_pattern_button']")]
		private IWebElement _addMealPattern;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_medical_condition_button']")]
		private IWebElement _addMedicalCondition;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_medical_event_button']")]
		private IWebElement _addMedicalEvent;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_sen_stage_button']")]
		private IWebElement _addSENStage;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_sen_need_button']")]
		private IWebElement _addSENNeed;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_sen_provision_button']")]
		private IWebElement _addSENProvision;

		#endregion LinkAction

		#region Page Actions

		public void ClickGenerateUPN()
		{
			_GenerateUPNButton.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddUnfiormGrantEligibility()
		{
			_addUniformGrantEligibility.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddEnrolmentStatus()
		{
			_addEnrolmentStatusButtton.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddSENStage()
		{
			_addSENStage.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddSENNeed()
		{
			_addSENNeed.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddSENProvision()
		{
			_addSENProvision.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddMedicalCondition()
		{
			_addMedicalCondition.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddMedicalEvent()
		{
			_addMedicalEvent.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddFreeSchoolMealEligibility()
		{
			_addFreeSchoolMealEligibility.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddMealPattern()
		{
			_addMealPattern.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddTelephoneNumber()
		{
			_addTelephoneNumber.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickAddEmailAddress()
		{
			_addEmailAddress.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public PupilRecordPage RemoveNoteDocument()
		{
			this.SelectDocumentsTab();
			var rows = this.DocumentNote.Rows;
			for (int i = 0; i < rows.Count() - 1; i++)
			{
				rows[i].DeleteRow();
			}
			this.SavePupil();
			return new PupilRecordPage();
		}

		public SchoolHistoryDialog OpenAddSchoolHistoryDialog()
		{
			_addSchoolHistoryButton.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			return new SchoolHistoryDialog();
		}

		public void SavePupil()
		{
			Wait.WaitUntilDisplayed(By.CssSelector("[data-automation-id='well_know_action_save']"));
			_savePupilButton.ClickByJS();
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

		/// <summary>
		/// Au : An Nguyen
		/// Des : Verify success message is displayed
		/// </summary>
		/// <returns></returns>
		public bool IsSuccessMessageDisplayed()
		{
			AutomationSugar.WaitForAjaxCompletion();
			return SeleniumHelper.IsElementExists(SimsBy.AutomationId("status_success"));
		}

		public void Delete()
		{
			if (_deleteButton.IsExist())
			{
				_deleteButton.ClickByJS();
				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
				var deleteConfirmationPage = new WarningConfirmationDialog();
				deleteConfirmationPage.ConfirmDelete();
			}
		}

		public PupilRecordPage ContinueDelete()
		{
			if (_continueDeleteButton.IsExist())
			{
				_continueDeleteButton.ClickByJS();
				Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			}
			return new PupilRecordPage();
		}

		public void ClickAddAddress()
		{
			AutomationSugar.WaitFor("add_an_address_button");
			AutomationSugar.ClickOn("add_an_address_button");
			AutomationSugar.WaitForAjaxCompletion();
		}

		public CareArrangementsDialog OpenAddCareArrangmentDialog()
		{
			_addCareArrangment.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));

			return new CareArrangementsDialog();
		}

		public void ClickAddYoungCarerPeriod()
		{
			_addYoungCarerPeriod.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
		}

		public AddPupilContactTripletDialog ClickAddContact()
		{
			_addContactLink.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			return new AddPupilContactTripletDialog();
		}

		public void ClickParentalSalutation()
		{
			_parentalSalutationButton.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public void ClickParentalAddressee()
		{
			_parentalAddresseeButton.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
		}

		public MedicalPracticeTripletDialog ClickMedicalPractice()
		{
			_addMedicalPracticeLink.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			return new MedicalPracticeTripletDialog();
		}

		public CloneContactTripletDialog CloneContact()
		{
			_cloneContactLink.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			return new CloneContactTripletDialog();
		}

		public CopyContactTripletDialog CopyContact()
		{
			_copyContactLink.ClickByJS();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			return new CopyContactTripletDialog();
		}

		public void GenerateAddressee()
		{
			_generateAddresseeButton.ClickByJS();
			Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask-loading"));
		}

		public void GenerateParentalSalutation()
		{
			_generateSalutationButton.ClickByJS();
			Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask-loading"));
		}

		public AccommodationTypeDialog CreateAccommodationType()
		{
			_accommodationTypeButton.ClickByJS();

			Wait.WaitForElementReady(SimsBy.CssSelector("[id^='ui-id'][data-section-id='LearnerTravellersDetails-grid-editor-dialog']"));
			return new AccommodationTypeDialog();
		}

		public HistoryYearGroupDialog ViewYearGroupHistory()
		{
			_yearGroupHistoryButton.Click();
			return new HistoryYearGroupDialog();
		}

		public HistoryClassDialog ViewClassHistory()
		{
			_classHistoryButton.Click();
			return new HistoryClassDialog();
		}

		public HistoryAttendanceModeDialog ViewAttendanceModeHistory()
		{
			_attendanceModeHistoryDialogButton.Click();
			return new HistoryAttendanceModeDialog();
		}

		#endregion Page Actions

		#region Tab

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_more']")]
		private IWebElement _moreTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Personal']")]
		private IWebElement _personalDetailsTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Personal']")]
		private IWebElement _personalDetailsHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Registration']")]
		private IWebElement _registrationTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Registration']")]
		private IWebElement _registrationHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Addresses']")]
		private IWebElement _addressesTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Addresses']")]
		private IWebElement _addressesHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Phone/Email']")]
		private IWebElement _phoneEmailTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Phone/Email']")]
		private IWebElement _phoneEmailHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Family/Contacts']")]
		private IWebElement _familyHomeTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Family/Contacts']")]
		private IWebElement _familyHomeHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Meals']")]
		private IWebElement _mealsTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Meals']")]
		private IWebElement _mealsHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Medical']")]
		private IWebElement _medicalTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Medical']")]
		private IWebElement _medicalHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Ethnic/Cultural']")]
		private IWebElement _ethnicCulturalTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Ethnic/Cultural']")]
		private IWebElement _ethnicCulturalHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Additional']")]
		private IWebElement _additionalTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Additional']")]
		private IWebElement _additionalHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Welfare']")]
		private IWebElement _welfareTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Welfare']")]
		private IWebElement _welfareHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_School']")]
		private IWebElement _schoolHistoryTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_School']")]
		private IWebElement _schoolHistoryHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Consents']")]
		private IWebElement _consentsTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Consents']")]
		private IWebElement _consentsHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Statutory']")]
		private IWebElement _statutorySenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Statutory']")]
		private IWebElement _statutorySenHiddenTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Documents']")]
		private IWebElement _documentsTab;

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Documents']")]
		private IWebElement _documentsHiddenTab;

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Personal Details Tab
		/// </summary>
		public void SelectPersonalDetailsTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Personal']")).Displayed)
			//{
			//    Retry.Do(_personalDetailsTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_personalDetailsHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.Name("LegalForename"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Personal");
		}

		/// <summary>
		/// Au: An Nguyen
		/// Des : Select Registration Tab
		/// </summary>
		public void SelectRegistrationTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Registration']")).Displayed)
			//{
			//    Retry.Do(_registrationTab.ClickByJS);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_registrationHiddenTab.ClickByJS);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.Name("DateOfAdmission"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Registration");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Addresses Tab
		/// </summary>
		public void SelectAddressesTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Addresses']")).Displayed)
			//{
			//    Retry.Do(_addressesTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_addressesHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='Addresses']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Addresses");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Phone/Email Tab
		/// </summary>
		public void SelectPhoneEmailTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Phone/Email']")).Displayed)
			//{
			//    Retry.Do(_phoneEmailTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_phoneEmailHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerTelephones']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Phone/Email");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Family/Home Tab
		/// </summary>
		public void SelectFamilyHomeTab()
		{
			//    if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Family/Contacts']")).Displayed)
			//    {
			//        Retry.Do(_familyHomeTab.Click);
			//    }
			//    else
			//    {
			//        _moreTab.ClickByJS();
			//        Retry.Do(_familyHomeHiddenTab.Click);
			//    }
			//    Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerContactRelationships']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Family/Contacts");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Meals Tab
		/// </summary>
		public void SelectMealsTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Meals']")).Displayed)
			//{
			//    Retry.Do(_mealsTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_mealsHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='FreeMealEligibilities']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Meals");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Medical Tab
		/// </summary>
		public void SelectMedicalTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Medical']")).Displayed)
			//{
			//    Retry.Do(_medicalTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_medicalHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerMedicalPractices']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Medical");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Ethnic/Cultural Tab
		/// </summary>
		public void SelectEthnicCulturalTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Ethnic/Cultural']")).Displayed)
			//{
			//    Retry.Do(_ethnicCulturalTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_ethnicCulturalHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerNewcomerPeriods']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Ethnic/Cultural");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Additional Tab
		/// </summary>
		public void SelectAdditionalTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Additional']")).Displayed)
			//{
			//    Retry.Do(_additionalTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_additionalHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerZeroRatedInformations']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Additional");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Welfare Tab
		/// </summary>
		public void SelectWelfareTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Welfare']")).Displayed)
			//{
			//    Retry.Do(_welfareTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_welfareHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerInCareDetails']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Welfare");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select School History Tab
		/// </summary>
		public void SelectSchoolHistoryTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_School']")).Displayed)
			//{
			//    Retry.Do(_schoolHistoryTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_schoolHistoryHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerPreviousSchools']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_School History");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Consents Tab
		/// </summary>
		public void SelectConsentsTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Consents']")).Displayed)
			//{
			//    Retry.Do(_consentsTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_consentsHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerConsentTypes']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Consents");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Statutory SEN Tab
		/// </summary>
		public void SelectStatutorySenTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Statutory']")).Displayed)
			//{
			//    Retry.Do(_statutorySenTab.Click);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_statutorySenHiddenTab.Click);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerSENStatuses']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Statutory SEN");
		}

		/// <summary>
		/// Au : An Nguyen
		/// Des : Select Documents Tab
		/// </summary>
		public void SelectDocumentsTab()
		{
			//if (SeleniumHelper.Get(By.CssSelector("li [data-automation-id='section_menu_Documents']")).Displayed)
			//{
			//    Retry.Do(_documentsTab.ClickByJS);
			//}
			//else
			//{
			//    _moreTab.ClickByJS();
			//    Retry.Do(_documentsHiddenTab.ClickByJS);
			//}
			//Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerNotes']"));

			AutomationSugar.ExpandAccordionPanel("section_menu_Documents");
		}

		#endregion Tab
	}
}