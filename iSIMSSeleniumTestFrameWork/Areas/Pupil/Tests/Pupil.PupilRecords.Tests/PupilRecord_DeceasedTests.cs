using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using POM.Components.Common;
using POM.Components.Pupil;
using POMSeleniumHelper = POM.Helper;
using Pupil.Components;
using Pupil.Components.Common;
using Pupil.Data;
using SeSugar;
using SeSugar.Data;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using Selene.Support.Attributes;
using SeSugar.Automation;

namespace Pupil.PupilRecords.Tests
{
	public class PupilRecord_DeceasedTests
	{
		private string _pattern = "d/M/yyyy";

		[WebDriverTest(Groups = new[] { PupilTestGroups.PupilRecord.Deceased, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 },
			Browsers = new[] { BrowserDefaults.Chrome })]
		public void Pupil_Deceased_Date_Before_Admission_Returns_Validation_Error()
		{
			//Arrange
			// Add Basic Learner
			var pupilRecord = this.BuildDataPackage();
			var learnerId = Guid.NewGuid();
			var surname = "SelDeceasedPupil" + Utilities.GenerateRandomString(5);
			var forename = Utilities.GenerateRandomString(15);
			var dateOfBirth = new DateTime(2009, 05, 31);
			var dateOfAdmission = new DateTime(2011, 10, 10);
			var dateOfLeaving = new DateTime(2011, 1, 1);
			var reasonForLeaving = "Deceased";

			pupilRecord.AddBasicLearner(learnerId, surname, forename, dateOfBirth, dateOfAdmission);

			// Act
			using (new DataSetup(false, true, pupilRecord))
			{
				try
				{
					// Login as school admin
					POM.Helper.SeleniumHelper.Login(POM.Helper.SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
					AutomationSugar.WaitForAjaxCompletion();
					Thread.Sleep(2000);

					AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
					AutomationSugar.WaitForAjaxCompletion();

					// Search a pupil
					var pupilRecordTriplet = new PupilSearchTriplet();
					pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
					pupilRecordTriplet.SearchCriteria.IsCurrent = true;
					pupilRecordTriplet.SearchCriteria.IsFuture = false;
					pupilRecordTriplet.SearchCriteria.IsLeaver = false;
					var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
					AutomationSugar.WaitForAjaxCompletion();

					// Open pupil page
					var pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename))
						.Click<PupilRecordPage>();
					AutomationSugar.WaitForAjaxCompletion();

					// Navigate to Pupil Leaving Page
					var pupilLeavingDetailsPage =
						POMSeleniumHelper.SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Make Pupil a Leaver");
					AutomationSugar.WaitForAjaxCompletion();

					// Add pupil DOL as Deceased
					pupilLeavingDetailsPage.DOL =
						DateTime.ParseExact(dateOfLeaving.ToString(_pattern), _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
					pupilLeavingDetailsPage.ReasonForLeaving = reasonForLeaving;

					pupilLeavingDetailsPage.ClickSave(false);
					AutomationSugar.WaitForAjaxCompletion();

					//Assert
					Assert.IsTrue(pupilLeavingDetailsPage.IsDateOfAdmissionWarningMessageDisplayed());
				}
				finally
				{
					// Clean up
				}
			}
		}

		[WebDriverTest(Groups = new[] { PupilTestGroups.PupilRecord.Deceased, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 },
			Browsers = new[] { BrowserDefaults.Chrome })]
		public void Pupil_Adding_Deceased_Date_After_Leaving_Date_Should_Be_Successful()
		{
			//Arrange
			// Add Basic Learner
			var pupilRecord = this.BuildDataPackage();
			var learnerId = Guid.NewGuid();
			var surname = "SelDeceasedPupil" + Utilities.GenerateRandomString(5);
			var forename = Utilities.GenerateRandomString(15);
			var dateOfBirth = new DateTime(2009, 05, 01);
			var dateOfAdmission = new DateTime(2011, 09, 05);
			var dateOfLeaving = new DateTime(2011, 11, 11);
			var reasonForLeaving = "Deceased";
			pupilRecord.AddBasicLearner(learnerId, surname, forename, dateOfBirth, dateOfAdmission);

			// Act
			using (new DataSetup(false, true, pupilRecord))
			{
				try
				{
					// Login as school admin
					POM.Helper.SeleniumHelper.Login(POM.Helper.SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
					AutomationSugar.WaitForAjaxCompletion();

					AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
					AutomationSugar.WaitForAjaxCompletion();

					// Search a pupil
					var pupilRecordTriplet = new PupilSearchTriplet();
					pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
					pupilRecordTriplet.SearchCriteria.IsCurrent = true;
					pupilRecordTriplet.SearchCriteria.IsFuture = false;
					pupilRecordTriplet.SearchCriteria.IsLeaver = false;
					var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
					AutomationSugar.WaitForAjaxCompletion();

					pupilResults.FirstOrDefault(x => x.Name.Trim()
						.Equals(surname + ", " + forename))
						.Click<PupilRecordPage>();
					AutomationSugar.WaitForAjaxCompletion();

					var pupilLeavingDetailsPage =
						POMSeleniumHelper.SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Make Pupil a Leaver");
					AutomationSugar.WaitForAjaxCompletion();

					pupilLeavingDetailsPage.DOL =
						DateTime.ParseExact(dateOfLeaving.ToString(_pattern), _pattern, CultureInfo.InvariantCulture)
							.ToString(_pattern);

					pupilLeavingDetailsPage.ReasonForLeaving = reasonForLeaving;

					var confirmationDialog = pupilLeavingDetailsPage.ClickSave();
					AutomationSugar.WaitForAjaxCompletion();

					confirmationDialog.ClickOk();
					AutomationSugar.WaitForAjaxCompletion();

					var leaverBackgroundProcessSubmitDialog = new LeaverBackgroundProcessSubmitDialog();
					leaverBackgroundProcessSubmitDialog.ClickOk();
					AutomationSugar.WaitForAjaxCompletion();
				}
				finally
				{
					//Purge linked data
					PurgeLinkedData.DeleteLearnerPreviousSchool(learnerId);
				}
			}
		}
	}
}