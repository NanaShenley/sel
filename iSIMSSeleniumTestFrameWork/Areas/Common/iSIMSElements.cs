
using System;
using OpenQA.Selenium;
using WebDriverRunner.webdriver;

namespace SharedComponents
{
	[Obsolete("Please create element definitions in your own areas, eg Staff, Pupil, Attendance etc. as this Struct will soon become rather large.")]
	public struct iSIMSElements
	{
		/// <summary>
		/// 
		/// </summary>
		public struct QuickLinks
		{
			public const string StaffRecord = "#quick-links > div > ul > li:nth-child(5) > a";

			public const string PupilRecord = "#quick-links > div > ul > li:nth-child(2) > a";

			public const string PupilLog = "#quick-links > div > ul > li:nth-child(3) > a";

			public const string PupilContact = "#quick-links > div > ul > li:nth-child(4) > a";
		}

		/// <summary>
		/// 
		/// </summary>
		public struct Menu
		{
			public const string MenuButton = "#shell-menu > div > div.shell-menu > div:nth-child(1) > ul > li:nth-child(1) > a";

			public const string StaffHeader = "#task-menu > div:nth-child(1) > div > div:nth-child(13) > div.panel-heading > h4 > a > span";
			public const string DeleteStaffButton = ".shell-task-menu-item[data-ajax-url*=\"/Staff/SIMS8DeleteStaffScreenStaff/Details\"]";
			//public const string DeleteStaffButton = ".shell-task-menu-item[data-ajax-url=\"/" + TestDefaults.DefaultPath + "/Staff/SIMS8DeleteStaffScreenStaff/Details\"]";
			public const string StatutoryReturnHeader = "#task-menu > div:nth-child(1) > div > div:nth-child(8) > div.panel-heading > h4 > a > span";
			public const string ManageStatutoryReturnButton = "#task-menu > div:nth-child(1) > div > div:nth-child(8) > div:nth-child(2)>div>ul>li:nth-child(1)>a";

			public const string SchoolGroupsHeader = "#task-menu > div:nth-child(1) > div > div:nth-child(11) > div.panel-heading > h4 > a > span";
			public const string AllocatePupilToGroupButton = ".shell-task-menu-item[data-ajax-url*=\"/BulkPupils/AllocatePupilsToGroups/Details\"]";

			public const string DataOutHeader = "#task-menu > div:nth-child(1) > div > div:nth-child(7) > div.panel-heading > h4 > a > span";
			public const string CTFExportButton = ".shell-task-menu-item[data-ajax-url=\"/iSIMSMVCClientFarm1/DataOut/SIMS8CTFExportTripletCTFExportLog/Details\"]";
			public const string FuturePupilsButton = ".shell-task-menu-item[data-ajax-url=\"/iSIMSMVCClientFarm1/BulkPupils/AllocateNewIntake/Details\"]";
			public const string StatutoryReturnMenuButton = ".shell-task-menu-item[data-ajax-url=\"/iSIMSMVCClientFarm1/StatutoryReturn/StatutoryReturnLog/Details\"]";
			public const string LOPExport = "#task-menu > div:nth-child(1) > div > div:nth-child(7) > div:nth-child(2)>div>ul>li:nth-child(3)>a";

			public const string DataOutCBAExport = ".shell-task-menu-item[data-ajax-url=\"/iSIMSMVCClientFarm1/DataOut/SIMS8CBAExportTripletCBAExport/Details\"]";
			public const string DataOutLOPExport = ".shell-task-menu-item[data-ajax-url=\"/iSIMSMVCClientFarm1/DataOut/SIMS8LevelsOfProgressionExportMaintenanceTripleLoPExport/Details\"]";
		}


		/// <summary>
		/// 
		/// </summary>
		public struct Tabs
		{
			public const string CloseTabButton = "#shell-footer > div > div > div > ul > li.current > a.layout-col.tab-close > i";
		}

		/// <summary>
		/// 
		/// </summary>
		public struct StatutoryReturn
		{
			//selector

			public const string CreateDeniButton = "a[data-ajax-url='/iSIMSMVCClientFarm1/StatutoryReturn/StatutoryReturnLog/CreateReturn']";
			public const string SearchButton = "button[data-ajax-url='/iSIMSMVCClientFarm1/StatutoryReturn/StatutoryReturnLog/Search']";
			public const string SearchResult = "#screen > div > div.layout-col.search.pane > div > div.form-body > div > div > div:nth-child(2) > div > div.list-group.search-result-tiles > div:nth-child(1)";

			public const string DetailReportsButton = "#screen > div > div.layout-col.main.pane > div > div > div > div.form-header > div > div > ul > li:nth-child(8) > a";//goes for first occurence of dropdown which is for create deni
			public const string IssuesAndQueriesReport = "#screen > div > div.layout-col.main.pane > div > div > div > div.form-header > div > div > ul > li:nth-child(8) >ul > li:nth-child(11) > a";

		}

		public struct Staff
		{

			public struct StaffRecord
			{
				public const string SearchButton = "button[type='submit']";
				public const string DeleteStaffSearchButton = "#screen > div > div.layout-col.search.pane > div > div.form-body > div > div > div:nth-child(1) > form > div.search-criteria-form-action > button";
				public const string SaveButton = "#screen > div > div.layout-col.main.pane > div > div > div > div.form-header > div > div > ul > li:nth-child(4) > a";
				public const string DeleteButton = "#screen > div > div.layout-col.main.pane > div > div > div > div.form-header > div > div > ul > li:nth-child(4) > a";
				public const string ConfirmDeleteButton = "#palette-editor-container > div > div.modal-dialog > div > div.modal-footer > button.btn.btn-default.btn-outline";

				public const string SearchResultFormat = "div[data-ajax-url*='/Staff/SIMS8StaffMaintenanceTripleStaff/ReadDetail/{0}']";
				public const string DeleteStaffSearchResultFormat = "div[data-ajax-url*='/Staff/SIMS8DeleteStaffScreenStaff/ReadDetail/{0}']";
				public const string Absences = "#editableData > div > div:nth-child(6) > div.panel-heading > h4 > a > span";
				public const string AddStaffAbsence = "[title=\"Add New Linked Staff Absence\"]";
				public const string Savebtn = "[title=\"Save Record\"]";
				public const string Editbtn = " #editableData > div > div:nth-child(6) [title='Edit full record']";

				/// <summary>
				/// 
				/// </summary>
				public struct StaffAbsencePopup
				{
					public const string AddAbsenceCertificate = "[title=\"Add Absence Certificate\"]";

					//Selectors
					public const string AbsenceType = "[name=\"AbsenceType.dropdownImitator\"]";
					public const string IllnessCategory = "[name=\"IllnessCategory.dropdownImitator\"]";
					public const string AbsencePayRate = "[name=\"AbsencePayRate.dropdownImitator\"]";
					public const string PayrollAbsenceCategory = "[name=\"PayrollAbsenceCategory.dropdownImitator\"]";

					//Check Boxes
					public const string AnnualLeave = "[name=\"AnnualLeave\"]";
					public const string IndustrialInjury = "[name=\"IndustrialInjury\"]";
					public const string SSPExclusion = "[name=\"SSPExclusion\"]";

					//Input
					public const string FirstDay = "#dialog-palette-editor [name='StartDate']";
					public const string LastDay = "#dialog-palette-editor [name='EndDate']";
					public const string ExpectedReturn = "[name=\"ExpectedReturnDate\"]";
					public const string ActualReturn = "[name=\"ActualReturnDate\"]";
					public const string WorkingDaysLost = "[name=\"WorkingDaysLost\"]";
					public const string WorkingHoursLost = "[name=\"WorkingHoursLost\"]";
					public const string Notes = "#dialog-palette-editor [name='Notes']";

					public const string OKbtn =
						"#dialog-palette-editor > div.modal-dialog.layout-page > div > div.modal-footer.pane-footer.layout-row > button:nth-child(1)";
					public const string Cancelbtn =
						"#dialog-palette-editor > div.modal-dialog.layout-page > div > div.modal-footer.pane-footer.layout-row > button.btn.btn-default.btn-outline";
					public const string Editbtn =
						"#dialog-editableData > div > div:nth-child(8) > div > div > div > div.table-scroll.grid-height-sm > div > table > tbody > tr > td:nth-child(2) > button";

					/// <summary>
					/// 
					/// </summary>
					public struct StaffAbsenceCertificatePopup
					{
						//Selectors
						public const string SignatoryType = "[name=\"SignatoryType.dropdownImitator\"]";
						public const string CertificateAdvice = "[name=\"CertificateAdvice.dropdownImitator\"]";

						//Check Boxes
						public const string APhasedReturnToWork = "#ui-id-10 > div:nth-child(5) > div > div > div > div > div:nth-child(1) > input:nth-child(1)";
						public const string AmendedDuties = "#ui-id-10 > div:nth-child(5) > div > div > div > div > div:nth-child(1) > input:nth-child(4)";
						public const string AlteredHours = "#ui-id-10 > div:nth-child(5) > div > div > div > div > div:nth-child(1) > input:nth-child(6)";
						public const string WorkplaceAdaptions = "#ui-id-10 > div:nth-child(5) > div > div > div > div > div:nth-child(1) > input:nth-child(8)";
						//Input
						public const string DateReceived = "[name=\"DateReceived\"]";
						public const string DateSigned = "[name=\"DateSigned\"]";
						public const string SignedBy = "[name=\"SignedBy\"]";
						public const string CertificateStartDate = "#ui-id-10 > div:nth-child(3) > div > div:nth-child(1) > div > div > input:nth-child(1)";
						public const string CertificateEndDate = "#ui-id-10 > div:nth-child(3) > div > div:nth-child(2) > div > div > input:nth-child(1)";
						public const string CertificateDuration = "[name=\"Duration\"]";

						public const string OKbtn =
							"#dialog-dialog-palette-editor > div.modal-dialog.layout-page > div > div.modal-footer.pane-footer.layout-row > button:nth-child(1)";
						public const string Cancelbtn =
							"#dialog-dialog-palette-editor > div.modal-dialog.layout-page > div > div.modal-footer.pane-footer.layout-row > button.btn.btn-default.btn-outline";
					}
				}
			}
		}

	}
}
