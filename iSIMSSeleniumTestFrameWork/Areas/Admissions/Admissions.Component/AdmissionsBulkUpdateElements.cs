using OpenQA.Selenium;

using SharedComponents.Helpers;

namespace Admissions.Component
{
	public struct AdmissionsBulkUpdateElements
	{
		public struct Menu
		{
			public static By MenuButton = By.CssSelector(SeleniumHelper.AutomationId("task_menu"));
			public static By Pupils = By.CssSelector(SeleniumHelper.AutomationId("section_menu_Pupils"));
			public static By MenuBulkUpdatePupilDetail = By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_pupil_bulk_update"));
			public static By MenuBulkUpdateApplicationDetail = By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_admissions_bulk_update"));
		}

		public struct BulkUpdate
		{
			public struct MenuItems
			{
				public static By PupilBasicDetailsMenuItem = By.CssSelector(SeleniumHelper.AutomationId("bulk_update_sub_menu_pupil_basic_details"));
				public static By PupilConsentsMenuItem = By.CssSelector(SeleniumHelper.AutomationId("bulk_update_sub_menu_pupil_consents"));
				public static By PupilSalutationAddresseeMenuItem = By.CssSelector(SeleniumHelper.AutomationId("bulk_update_sub_menu_pupil_parental_salutation_addressee"));

				public static By ApplicantApplicationStatusMenuItem = By.CssSelector(SeleniumHelper.AutomationId("bulk_update_sub_menu_applicant_application_status"));
				public static By ApplicantSalutationAddresseeMenuItem = By.CssSelector(SeleniumHelper.AutomationId("bulk_update_sub_menu_applicant_parental_salutation_addressee"));

				public static By PupilBulkUpdateBackButton = By.CssSelector(SeleniumHelper.AutomationId("bulk_update_action_backtomain"));
				public static By PupilBulkUpdateSaveButton = By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"));
			}

			public struct Search
			{
				public static By SearchCriteria = By.CssSelector(SeleniumHelper.AutomationId("search_criteria"));
                public const string MissingSalutationCheckboxName = "MissingParentalSalutation";
                public const string MissingAddresseeCheckboxName = "MissingParentalAddressee";
                public static By SearchButton = By.CssSelector(SeleniumHelper.AutomationId("search_criteria_submit"));
			}

			public struct Detail
			{
				public static By SaveMessageCss = By.CssSelector("div[data-automation-id='status_success']");
				public const string SaveMessage = "Records Saved";
				public static readonly int Timeout = 30;
				public static By RowCount = By.CssSelector("span[data-automation-id='data-bulkupdate-row-count']");

				public static By SearchResults = By.CssSelector("[data-section-id='searchResults']");

				public static By IdentifierButton = By.CssSelector("a[data-invoke-dialog='pupil-bulkUpdate-identifiers-dialog']");
				public static By DataItemsButton = By.CssSelector("a[data-invoke-dialog='pupil-bulkUpdate-dataItems-dialog']");

				public struct IdentifierDialog
				{
					public static By PersonalDetails = By.CssSelector("#fixedcolumntreenode > div > div > div > div:nth-child(1) > div.webix_tree_item > input");
					public static By DateOfBirth = By.CssSelector("div[webix_tm_id='Learner.DateOfBirth']");
					public static By Gender = By.CssSelector("div[webix_tm_id='Learner.Gender.Description']");
					public static By LegalName = By.CssSelector("div[webix_tm_id='Learner.PreferredName']");

					public static By RegistrationDetails = By.CssSelector("#fixedcolumntreenode > div > div > div > div:nth-child(2) > div.webix_tree_item > input");

					public static By AdmissionNumber = By.CssSelector("div[webix_tm_id='Learner.AdmissionNumber']");
					public static By Class = By.CssSelector("div[webix_tm_id='Learner.PrimaryClass.FullName']");
					public static By DOA = By.CssSelector("div[webix_tm_id='Learner.LearnerEnrolments.DOA']");
					public static By YearGroup = By.CssSelector("div[webix_tm_id='Learner.YearGroup.FullName']");


					public static By DateOfBirthCheckBox = By.CssSelector("div[webix_tm_id='Learner.DateOfBirth'] > input");
					public static By LegalNameCheckBox = By.CssSelector("div[webix_tm_id='Learner.PreferredName'] > input");
					public static By GenderCheckBox = By.CssSelector("div[webix_tm_id='Learner.Gender.Description'] > input");
					public static By AdmissionNumberCheckBox = By.CssSelector("div[webix_tm_id='Learner.AdmissionNumber'] > input");
					public static By ClassCheckBox = By.CssSelector("div[webix_tm_id='Learner.PrimaryClass.FullName'] > input");
					public static By DOACheckBox = By.CssSelector("div[webix_tm_id='Learner.LearnerEnrolments.DOA'] > input");
					public static By YearGroupCheckBox = By.CssSelector("div[webix_tm_id='Learner.YearGroup.FullName'] > input");

					public static By ClearSelection = By.CssSelector("a[data-clear-container-id='fixedcolumntreenode']");

					public static By OkButton = By.CssSelector("#identifiers-ok-button-id");
				}

				public struct DataItemsDialog
				{
					public static By DialogCancelButton = By.CssSelector("#data-items-cancel-button-id");
					public static By SectionPanelsGroup = By.CssSelector("[data-automation-id='section_menu_Pupil Data Items'] > span.title");
					//public static By SectionPanelHeading = By.CssSelector("form#editableData #palette-editor > div.modal-dialog.layout-page > div > div.modal-body.layout-row.pane-body > div > div.panel-default.panel > div.panel-heading");
					public static By DataItemsPanelHeaderTitle = By.CssSelector("#data-items-accordion-panel-title-id > span");
				}

				public struct GridColumns
				{
					public static By DateOfBirth = By.CssSelector("div[data-menu-column-id='_DateOfBirth']");
					public static By Gender = By.CssSelector("div[data-menu-column-id='_Gender']");
					public static By LegalName = By.CssSelector("div[data-menu-column-id='_PreferredName']");
					public static By AdmissionNumber = By.CssSelector("div[data-menu-column-id='_AdmissionNumber']");
					public static By Class = By.CssSelector("div[data-menu-column-id='_PrimaryClass']");
					public static By DateOfAmission = By.CssSelector("div[data-menu-column-id='_DOA']");
					public static By YearGroup = By.CssSelector("div[data-menu-column-id='_YearGroup']");
				}

				public struct Consents
				{
					public static By ConsentTypeDropDownButton = By.CssSelector(SeleniumHelper.AutomationId("Button_DropdownRadio_Description"));
				}
			}
		}
	}
}
