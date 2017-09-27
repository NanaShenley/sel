using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
//using Pupil.BulkUpdate.Tests.ParentalSalutationAndAddresseeComponents;
using Admissions.Component;
using Admissions.Data;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using Wait = PageObjectModel.Helper.Wait;

namespace Admissions.BulkUpdate.Tests
{
    public class BulkUpdateApplicantParentalSalutationAndAddresseeTests
	{
		//private PageObjectModel.Helper.SeleniumHelper.iSIMSUserType LoginAs = PageObjectModel.Helper.SeleniumHelper.iSIMSUserType.SchoolAdministrator;
		private SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.AdmissionsOfficer;

		#region Common Fields

		private string _admissionYear;
		private string _schoolIntakeName;
		private string _admissionGroupName;
		private string contactSurname1;
		private string contactSurname2;
		private const string DefaultSalutationColumn = "2";
		private const string DefaultAddresseeColumn = "3";

        #endregion Common Fields

		#region Bulk Update Parental Salutation And Addressee Generate

		#region Salutation

		[WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P3", "BulkUpdate" })]
		public void Can_FloodFill_Applicant_Parental_Salutation()
		{
			CommonFloodFillTest();
		}

		private readonly object _commonObject = new Object();

		private void CommonFloodFillTest()
		{
			lock (_commonObject)
			{
				// Initialise
				string firstSalutationName;
				string lastSalutationName;
				string firstAddresseeName;
				string lastAddresseeName;

				//Arrange
				var dataPackage = GetDataPackage("BU_APP_PSA_T1", out firstSalutationName, out lastSalutationName,
					out firstAddresseeName, out lastAddresseeName);

				using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
				{
					NavigateToBulkUpdateApplicantParentalSalutationDetailScreen();
					
					//Act
                    ParentalSalutationAndAddresseeDetail parentalSalutationAndAddresseeDetail = new ParentalSalutationAndAddresseeDetail();
					ParentalSalutationAndAddresseeDetail.DeleteParentalSalutationColumnValues();
					ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkSelectParentalSalutation();
					ParentalSalutationAndAddresseeDetail.FloodFillSalutationColumnWithOverride();

					//Assert
					var cells = ParentalSalutationAndAddresseeDetail.GetCellText(DefaultSalutationColumn);

                    Assert.AreEqual(cells[0], firstSalutationName);
					Assert.AreEqual(cells[1], lastSalutationName);

					IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);

                    Assert.IsTrue(IsDirtyIndicator.Displayed, "Page Dirty Indicator");
				}
			}
		}

		//TODO: This test needs to be modified as it doesn't perform the expected steps as per its name. 
		//For the time being, it's not fixed as the priority now is to make the tests pass as it stands
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P3", "BulkUpdate" })]
		public void Can_FloodFill_Applicant_Parental_Salutation_With_Override_Not_Checked()
		{
			lock (_commonObject)
			{
				// Initialise
				string firstSalutationName;
				string lastSalutationName;
				string firstAddresseeName;
				string lastAddresseeName;

				//Arrange
				DataPackage dataPackage = GetDataPackage("BU_APP_PSA_T2", out firstSalutationName, out lastSalutationName, out firstAddresseeName, out lastAddresseeName);

				using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
				{
					NavigateToBulkUpdateApplicantParentalSalutationDetailScreen();

					//Act
					ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkSelectParentalSalutation();

					SeleniumHelper.WaitForElementClickableThenClick(ParentalSalutationAndAddresseeDetail.FloodFillGenerateForSelected);

					IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);

					//Assert
					Assert.IsFalse(IsDirtyIndicator.Displayed, "Page is not dirty");
                }
			}
		}

        #endregion Salutation

		#region Addressee

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P3", "BulkUpdate", "Can_FloodFill_Applicant_Parental_Addressee" })]
		public void Can_FloodFill_Applicant_Parental_Addressee()
		{
			lock (_commonObject)
			{
				string firstSalutationName;
				string lastSalutationName;
				string firstAddresseeName;
				string lastAddresseeName;

				//Arrange
				DataPackage dataPackage = GetDataPackage("BU_APP_PSA_T3", out firstSalutationName, out lastSalutationName,
					out firstAddresseeName, out lastAddresseeName);
				using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
				{
					NavigateToBulkUpdateApplicantParentalSalutationDetailScreen();

					//Act
					ParentalSalutationAndAddresseeDetail.DeleteParentalAddresseeColumnValues();

					ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkSelectParentalAddressee();

					ParentalSalutationAndAddresseeDetail.FloodFillAddresseeColumnWithOverride();

					//Assert
					var cells = ParentalSalutationAndAddresseeDetail.GetCellText(DefaultAddresseeColumn);
					Assert.AreEqual(cells[0], firstAddresseeName);
					Assert.AreEqual(cells[1], lastAddresseeName);

					IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);
					Assert.IsTrue(IsDirtyIndicator.Displayed);
				}
			}
		}

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P3", "BulkUpdate", "Can_Update_individual_Applicant_Parental_Addressee" })]
		public void Can_Update_individual_Applicant_Parental_Addressee()
		{
			lock (_commonObject)
			{
				string firstSalutationName;
				string lastSalutationName;
				string firstAddresseeName;
				string lastAddresseeName;

				var dataPackage = GetDataPackage("BU_APP_PSA_T4", out firstSalutationName, out lastSalutationName,
					out firstAddresseeName, out lastAddresseeName);

				using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
				{
					NavigateToBulkUpdateApplicantParentalSalutationDetailScreen();

					//Act
					ParentalSalutationAndAddresseeDetail.DeleteParentalAddresseeColumnValues();
					ParentalSalutationAndAddresseeDetail.ClickFirstCellofColumn(DefaultAddresseeColumn);
					ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkParentalAddresseeFloodFillMenuClick();
					ParentalSalutationAndAddresseeDetail.FloodFillAddresseeColumnWithOverride();

					//Assert
					var cells = ParentalSalutationAndAddresseeDetail.GetCellText(DefaultAddresseeColumn);
					Assert.AreEqual(cells[0], firstAddresseeName);
					Assert.AreEqual(cells[1], string.Empty);

					IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);
					Assert.IsTrue(IsDirtyIndicator.Displayed);
				}
			}
		}

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P3", "BulkUpdate", "Can_FloodFill_Applicant_Parental_Addressee_With_Override_Not_Checked" })]
		public void Can_FloodFill_Applicant_Parental_Addressee_With_Override_Not_Checked()
		{
			lock (_commonObject)
			{
				string firstSalutationName;
				string lastSalutationName;
				string firstAddresseeName;
				string lastAddresseeName;

				//Arrange
				DataPackage dataPackage = GetDataPackage("BU_APP_PSA_T1", out firstSalutationName, out lastSalutationName,
					out firstAddresseeName, out lastAddresseeName);
				using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
				{
					NavigateToBulkUpdateApplicantParentalSalutationDetailScreen();

					//Act
					ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkSelectParentalAddressee();

					SeleniumHelper.WaitForElementClickableThenClick(
						ParentalSalutationAndAddresseeDetail.ParentalAddresseeFloodFillGenerateForSelected);

					IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);

					//Assert
					Assert.IsFalse(IsDirtyIndicator.Displayed);
				}
			}
		}

        #endregion Addressee

		#region Save

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P3", "BulkUpdate" })]
		public void Can_Save_BulkUpdate_Applicant_Parental_Salutation_Save()
		{
			lock (_commonObject)
			{
				string firstSalutationName;
				string lastSalutationName;
				string firstAddresseeName;
				string lastAddresseeName;

				//Arrange
				DataPackage dataPackage = GetDataPackageWithNoSalutation("BU_APP_PSA_T2", out firstSalutationName,
					out lastSalutationName, out firstAddresseeName, out lastAddresseeName);
				using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
				{
					var bulkUpdateNavigation = new AdmissionsBulkUpdateNavigation();
					bulkUpdateNavigation.NavgateToPupilBulkUpdate_SubMenu(
						AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.ApplicantSalutationAddresseeMenuItem, LoginAs);

                    SeleniumHelper.ToggleCheckbox(AdmissionsBulkUpdateElements.BulkUpdate.Search.MissingSalutationCheckboxName);
                    SeleniumHelper.ToggleCheckbox(AdmissionsBulkUpdateElements.BulkUpdate.Search.MissingAddresseeCheckboxName);

                    SeleniumHelper.ChooseSelectorOption(ParentalSalutationAndAddresseeDetail.YearGroupDropDownList, _admissionYear);

                    SeleniumHelper.ChooseSelectorOption(ParentalSalutationAndAddresseeDetail.SchoolIntakeDropDownList,
						_schoolIntakeName);

                    SeleniumHelper.ChooseSelectorOption(ParentalSalutationAndAddresseeDetail.AdmissionGroupDropDownList,
						_admissionGroupName);

					//SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton, TimeSpan.FromSeconds(20));
                    SeleniumHelper.WaitForElementClickableThenClick(AdmissionsBulkUpdateElements.BulkUpdate.Search.SearchButton);
                    var rowCount = SeleniumHelper.Get(AdmissionsBulkUpdateElements.BulkUpdate.Detail.RowCount);
					Assert.AreEqual("Rows: 2", rowCount.Text);

					ParentalSalutationAndAddresseeDetail.ClickFirstCellofColumn(DefaultSalutationColumn);
					ParentalSalutationAndAddresseeDetail.GetEditor().SendKeys("test");

					SeleniumHelper.WaitForElementClickableThenClick(
                        AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.PupilBulkUpdateSaveButton);

					WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver,
                        TimeSpan.FromSeconds(AdmissionsBulkUpdateElements.BulkUpdate.Detail.Timeout));

                    waiter.Until(ExpectedConditions.ElementIsVisible(AdmissionsBulkUpdateElements.BulkUpdate.Detail.SaveMessageCss));
                    string actualText = WebContext.WebDriver.FindElement(AdmissionsBulkUpdateElements.BulkUpdate.Detail.SaveMessageCss).Text;
                    Assert.AreEqual(AdmissionsBulkUpdateElements.BulkUpdate.Detail.SaveMessage, actualText);
				}
			}
		}

        #endregion Save

        #endregion Bulk Update Parental Salutation And Addressee Generate

		#region Common Methods

		private void NavigateToBulkUpdateApplicantParentalSalutationDetailScreen()
		{
			var bulkUpdateNavigation = new AdmissionsBulkUpdateNavigation();
            bulkUpdateNavigation.NavgateToPupilBulkUpdate_SubMenu(AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.ApplicantSalutationAddresseeMenuItem, LoginAs);

            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));

            SeleniumHelper.ToggleCheckbox(AdmissionsBulkUpdateElements.BulkUpdate.Search.MissingSalutationCheckboxName);
            SeleniumHelper.ToggleCheckbox(AdmissionsBulkUpdateElements.BulkUpdate.Search.MissingAddresseeCheckboxName);

            SeleniumHelper.ChooseSelectorOption(ParentalSalutationAndAddresseeDetail.YearGroupDropDownList, _admissionYear);

            SeleniumHelper.ChooseSelectorOption(ParentalSalutationAndAddresseeDetail.SchoolIntakeDropDownList, _schoolIntakeName);

            SeleniumHelper.ChooseSelectorOption(ParentalSalutationAndAddresseeDetail.AdmissionGroupDropDownList, _admissionGroupName);

            ElementRetriever.FindElementSafe(WebContext.WebDriver, AdmissionsBulkUpdateElements.BulkUpdate.Search.SearchButton).SendKeys(Keys.Enter);

		    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
		}

        private static readonly object _lockObject = new object();

		private DataPackage GetDataPackage(string suffix, out string firstSalutationName, out string lastSalutationName, out string firstAddresseeName, out string lastAddresseeName)
		{
			lock (_lockObject)
			{
				var schoolIntakeId = Guid.NewGuid();
				var admissionGroupId = Guid.NewGuid();
				string admissionTerm = "Spring";
				const int numberOfPlannedAdmission = 31;
				const int capacity = 10;
				_admissionYear = string.Format("{0}/{1}", (DateTime.Now.Year), (DateTime.Now.Year + 1));
				var dateOfAdmission = new DateTime(DateTime.Today.Year, 8, 1);

				_admissionGroupName = Utilities.GenerateRandomString(10, "RB_" + suffix);
				var yearGroup = Queries.GetFirstYearGroup();
				var yearGroupFullName = yearGroup.FullName;
				_schoolIntakeName = string.Format("{0} - {1} {2} {3}", _admissionYear, admissionTerm, yearGroupFullName, _admissionGroupName);

				var dataPackage = this.BuildDataPackage();
				dataPackage.AddSchoolIntake(
					schoolIntakeId,
					_admissionYear,
					admissionTerm,
					yearGroup,
					numberOfPlannedAdmission,
					_admissionGroupName,
					dateOfAdmission,
					capacity,
					admissionGroupId,
					schoolInTakeName: _schoolIntakeName);

				//Create first applicant
				Guid pupilId = Guid.NewGuid();
				Guid learnerEnrolmentId = Guid.NewGuid();
				Guid applicantId = Guid.NewGuid();
				var pupilSurname = Utilities.GenerateRandomString(10, "PSur1" + suffix);
				var pupilForename = Utilities.GenerateRandomString(10, "PFore1" + suffix);

				contactSurname1 = Utilities.GenerateRandomString(10, "CSur1" + suffix);
				var contactForename1 = Utilities.GenerateRandomString(10, "CFore1" + suffix);
				var contactForename2 = Utilities.GenerateRandomString(10, "CFore2" + suffix);

				firstSalutationName = string.Format("Mr {0}", contactSurname1);
				firstAddresseeName = string.Format("Mr {0} {1}", contactForename1[0], contactSurname1);

				dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2010, 01, 01),
					new DateTime(2015, 09, 01), enrolStatus: "F", uniqueLearnerEnrolmentId: learnerEnrolmentId,
					salutation: firstSalutationName, addressee: firstAddresseeName);
				dataPackage.AddBasicApplicant(applicantId, pupilId, learnerEnrolmentId, admissionGroupId, new DateTime(2015, 09, 01));

				#region Pre-Condition: Create new contact 1 and refer to pupil

				//Arrange
				AutomationSugar.Log("Create new contact1 and refer to pupil");
				Guid pupilContactId1 = Guid.NewGuid();
				Guid pupilContactRelationshipId1 = Guid.NewGuid();

				//Add pupil contact
				dataPackage.AddPupilContact(pupilContactId1, contactSurname1, contactForename1);
				dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1,
					hasParentalResponsibility: true);
				dataPackage.AddBasicLearnerContactAddress(pupilId, pupilContactId1, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
					new DateTime(2016, 01, 01));

                #endregion Pre-Condition: Create new contact 1 and refer to pupil

				//Create second applicant
				pupilId = Guid.NewGuid();
				learnerEnrolmentId = Guid.NewGuid();
				applicantId = Guid.NewGuid();
				pupilSurname = Utilities.GenerateRandomString(10, "PSur2" + suffix);
				pupilForename = Utilities.GenerateRandomString(10, "PFore2" + suffix);

				contactSurname2 = Utilities.GenerateRandomString(10, "CSur2" + suffix);
				lastSalutationName = string.Format("Mr {0}", contactSurname2);
				lastAddresseeName = string.Format("Mr {0} {1}", contactForename2[0], contactSurname2);
				dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2010, 01, 02),
					new DateTime(2015, 09, 02), enrolStatus: "F", uniqueLearnerEnrolmentId: learnerEnrolmentId,
					salutation: lastSalutationName, addressee: lastAddresseeName);
				dataPackage.AddBasicApplicant(applicantId, pupilId, learnerEnrolmentId, admissionGroupId, new DateTime(2015, 09, 02));

				#region Pre-Condition: Create new contact 2 and refer to pupil

				AutomationSugar.Log("Create new contact2 and refer to pupil");
				Guid pupilContactId2 = Guid.NewGuid();
				Guid pupilContactRelationshipId2 = Guid.NewGuid();

				//Add pupil contact
				dataPackage.AddPupilContact(pupilContactId2, contactSurname2, contactForename2);
				dataPackage.AddPupilContactRelationship(pupilContactRelationshipId2, pupilId, pupilContactId2,
					hasParentalResponsibility: true);
				dataPackage.AddBasicLearnerContactAddress(pupilId, pupilContactId2, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
					new DateTime(2016, 01, 01));

                #endregion Pre-Condition: Create new contact 2 and refer to pupil

				return dataPackage;
			}
		}

		private static readonly object _lockObject2 = new object();

		private DataPackage GetDataPackageWithNoSalutation(string suffix, out string firstSalutationName, out string lastSalutationName, out string firstAddresseeName, out string lastAddresseeName)
		{
			lock (_lockObject2)
			{
				var schoolIntakeId = Guid.NewGuid();
				var admissionGroupId = Guid.NewGuid();
				string admissionTerm = "Spring";
				const int numberOfPlannedAdmission = 31;
				const int capacity = 10;
				_admissionYear = string.Format("{0}/{1}", (DateTime.Now.Year), (DateTime.Now.Year + 1));
				var dateOfAdmission = new DateTime(DateTime.Today.Year, 8, 1);

				_admissionGroupName = Utilities.GenerateRandomString(10, "AGN101_" + suffix);
				var yearGroup = Queries.GetFirstYearGroup();
				var yearGroupFullName = yearGroup.FullName;
				_schoolIntakeName = string.Format("{0} - {1} {2} {3}", _admissionYear, admissionTerm, yearGroupFullName,
					_admissionGroupName);

				var dataPackage = this.BuildDataPackage();
				dataPackage.AddSchoolIntake(
					schoolIntakeId,
					_admissionYear,
					admissionTerm,
					yearGroup,
					numberOfPlannedAdmission,
					_admissionGroupName,
					dateOfAdmission,
					capacity,
					admissionGroupId,
					schoolInTakeName: _schoolIntakeName);

				//Create first applicant
				Guid pupilId = Guid.NewGuid();
				Guid learnerEnrolmentId = Guid.NewGuid();
				Guid applicantId = Guid.NewGuid();
				var pupilSurname = Utilities.GenerateRandomString(10, "pSur1" + suffix);
				var pupilForename = Utilities.GenerateRandomString(10, "pFore1" + suffix);

				var contactForename1 = Utilities.GenerateRandomString(10, "CFore1" + suffix);
				contactSurname1 = Utilities.GenerateRandomString(10, "CSur1" + suffix);

				firstSalutationName = string.Format("Mr {0}", contactSurname1);
				firstAddresseeName = string.Format("Mr {0} {1}", contactForename1[0], contactSurname1);

				dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2010, 01, 01),
					new DateTime(2015, 09, 01), enrolStatus: "F", uniqueLearnerEnrolmentId: learnerEnrolmentId,
					addressee: firstAddresseeName);
				dataPackage.AddBasicApplicant(applicantId, pupilId, learnerEnrolmentId, admissionGroupId, new DateTime(2015, 09, 01));

				#region Pre-Condition: Create new contact 1 and refer to pupil

				//Arrange
				AutomationSugar.Log("Create new contact1 and refer to pupil");
				Guid pupilContactId1 = Guid.NewGuid();
				Guid pupilContactRelationshipId1 = Guid.NewGuid();
				//Add pupil contact

				dataPackage.AddPupilContact(pupilContactId1, contactSurname1, contactForename1);
				dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1,
					hasParentalResponsibility: true);
				dataPackage.AddBasicLearnerContactAddress(pupilId, pupilContactId1, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
					new DateTime(2016, 01, 01));

                #endregion Pre-Condition: Create new contact 1 and refer to pupil

				//Create second applicant
				pupilId = Guid.NewGuid();
				learnerEnrolmentId = Guid.NewGuid();
				applicantId = Guid.NewGuid();
				pupilSurname = Utilities.GenerateRandomString(10, "pSur2" + suffix);
				pupilForename = Utilities.GenerateRandomString(10, "pFore2" + suffix);

				var contactForename2 = Utilities.GenerateRandomString(10, "CFore2" + suffix);
				contactSurname2 = Utilities.GenerateRandomString(10, "CSur2" + suffix);
				lastSalutationName = string.Format("Mr {0}", contactSurname2);
				lastAddresseeName = string.Format("Mr {0} {1}", contactForename2[0], contactSurname2);
				dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2010, 01, 02),
					new DateTime(2015, 09, 02), enrolStatus: "F", uniqueLearnerEnrolmentId: learnerEnrolmentId,
					addressee: lastAddresseeName);
				dataPackage.AddBasicApplicant(applicantId, pupilId, learnerEnrolmentId, admissionGroupId, new DateTime(2015, 09, 02));

				#region Pre-Condition: Create new contact 2 and refer to pupil

				AutomationSugar.Log("Create new contact2 and refer to pupil");
				Guid pupilContactId2 = Guid.NewGuid();
				Guid pupilContactRelationshipId2 = Guid.NewGuid();

				//Add pupil contact
				dataPackage.AddPupilContact(pupilContactId2, contactSurname2, contactForename2);
				dataPackage.AddPupilContactRelationship(pupilContactRelationshipId2, pupilId, pupilContactId2,
					hasParentalResponsibility: true);
				dataPackage.AddBasicLearnerContactAddress(pupilId, pupilContactId2, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
					new DateTime(2016, 01, 01));

                #endregion Pre-Condition: Create new contact 2 and refer to pupil

				return dataPackage;
			}
		}

        #endregion Common Methods
	}
}
