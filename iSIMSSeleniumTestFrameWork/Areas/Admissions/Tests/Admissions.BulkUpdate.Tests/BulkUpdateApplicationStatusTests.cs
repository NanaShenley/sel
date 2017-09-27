using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
//using Pupil.BulkUpdate.Tests.ApplicationStatusComponents;

//using Pupil.Components.Common;
using Admissions.Data;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using Admissions.Component;

namespace Admissions.BulkUpdate.Tests
{
    public class BulkUpdateApplicationStatusTests
    {
        //private SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;
        private SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.AdmissionsOfficer;

        #region private stuffs

        private readonly object _lockObject = new Object();
        private string _admissionYear;
        private string _schoolIntakeName;
        private string _admissionGroupName;

        private DataPackage GetApplicationStatusDataPackage()
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
                _admissionGroupName = Utilities.GenerateRandomString(10,
                    string.Format("SeAdmissionGroup{0}", Thread.CurrentThread.ManagedThreadId));
                var yearGroup = Queries.GetFirstYearGroup();
                var yearGroupFullName = yearGroup.FullName;
                _schoolIntakeName = string.Format("{0} - {1} {2}_{3}", _admissionYear, admissionTerm, yearGroupFullName, Utilities.GenerateRandomString(10));

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
                var pupilSurname = Utilities.GenerateRandomString(10, "TestPupil1_Surname" + Thread.CurrentThread.ManagedThreadId);
                var pupilForename = Utilities.GenerateRandomString(10, "TestPupil1_Forename" + Thread.CurrentThread.ManagedThreadId);
                dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2010, 01, 01),
                    new DateTime(2015, 09, 01), enrolStatus: "F", uniqueLearnerEnrolmentId: learnerEnrolmentId);
                dataPackage.AddBasicApplicant(applicantId, pupilId, learnerEnrolmentId, admissionGroupId, new DateTime(2015, 09, 01));

                //Create second applicant
                pupilId = Guid.NewGuid();
                learnerEnrolmentId = Guid.NewGuid();
                applicantId = Guid.NewGuid();
                pupilSurname = Utilities.GenerateRandomString(10, "TestPupil2_Surname" + Thread.CurrentThread.ManagedThreadId);
                pupilForename = Utilities.GenerateRandomString(10, "TestPupil2_Forename" + Thread.CurrentThread.ManagedThreadId);
                dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2010, 01, 02),
                    new DateTime(2015, 09, 02), enrolStatus: "F", uniqueLearnerEnrolmentId: learnerEnrolmentId);
                dataPackage.AddBasicApplicant(applicantId, pupilId, learnerEnrolmentId, admissionGroupId, new DateTime(2015, 09, 02));

                return dataPackage;
            }
        }

        private ApplicationStatusDetail NavigateAndSetupCriteriaAndDoTheSearchAndGetDetail()
        {
            AutomationSugar.Log("Navigate to Application Status Bulk Update Screen");
            var bulkUpdateNavigation = new AdmissionsBulkUpdateNavigation();
            bulkUpdateNavigation.NavigateToBulkUpdateApplicationStatus(LoginAs);

            AutomationSugar.Log("Setup search criteria");
            var appStatusSearchScreen = new ApplicationStatusSearch();
            PageObjectModel.Helper.Wait.WaitForAjaxReady(By.ClassName("locking-mask-loading"));
            appStatusSearchScreen.AdmissionYear = _admissionYear;
            PageObjectModel.Helper.Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
            appStatusSearchScreen.IntakeGroup = _schoolIntakeName;
            PageObjectModel.Helper.Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
            appStatusSearchScreen.AdmissionGroup = _admissionGroupName;
            PageObjectModel.Helper.Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
            AutomationSugar.Log("Do search");
            ApplicationStatusDetail detail = appStatusSearchScreen.ClickOnSearch();
            ApplicationStatusDetail appDetail = new ApplicationStatusDetail();

            return appDetail;
        }

        private bool hasGridResultsSuceeded(string columnNumber, string valueToBeTested)
        {
            List<IWebElement> cells = (ApplicationStatusDetail.FindcellsForColumn(columnNumber));
            foreach (IWebElement cell in cells)
            {
                return cell.Text == valueToBeTested;
            }
            return false;
        }

        #endregion private stuffs

        #region Tests

        [WebDriverTest(Enabled = true,
             Groups = new[] { "P3", "BulkUpdate", "Can_FloodFill_ApplicationStatus_In_BulkUpdate_Screen" },
             Browsers = new[] { BrowserDefaults.Chrome })]
        public void Can_FloodFill_ApplicationStatus_In_BulkUpdate_Screen()
        {
            //Arrange
            DataPackage dataPackage = GetApplicationStatusDataPackage();
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                //Act
                ApplicationStatusDetail appDetail = NavigateAndSetupCriteriaAndDoTheSearchAndGetDetail();
                AutomationSugar.Log("Flood fill the application status column");
                ApplicationStatusDetail.ScrollToApplicationStatus();
                appDetail.FloodFillApplicationStatusColumnWith("Reserved");

                //Assert
                AutomationSugar.Log("Verify the results");
                Assert.IsTrue(hasGridResultsSuceeded(ApplicationStatusDetail.ApplicationStatusDefaultColumnNumber, "Reserved"));
            }
        }

        [WebDriverTest(Enabled = true,
             Groups = new[] { "P3", "BulkUpdate","Can_Delete_ApplicationStatus_In_BulkUpdate_Screen"},
             Browsers = new[] { BrowserDefaults.Chrome })]
        public void Can_Delete_ApplicationStatus_In_BulkUpdate_Screen()
        {
            //Arrange
            DataPackage dataPackage = GetApplicationStatusDataPackage();
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                //Act
                ApplicationStatusDetail appDetail = NavigateAndSetupCriteriaAndDoTheSearchAndGetDetail();
                AutomationSugar.Log("Delete all the previously flood filled application status");
                appDetail.DeleteApplicationStatusColumnValues();

                //Assert
                AutomationSugar.Log("Verify application status");
                Assert.IsTrue(hasGridResultsSuceeded(ApplicationStatusDetail.ApplicationStatusDefaultColumnNumber, string.Empty));
            }
        }

        [WebDriverTest(Enabled = true,
             Groups = new[] { "P3", "BulkUpdate"},
             Browsers = new[] { BrowserDefaults.Chrome })]
        public void Can_Update_ApplicationStatus_As_Keyboard_Entries()
        {
            //Arrange
            DataPackage dataPackage = GetApplicationStatusDataPackage();
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                //Act
                NavigateAndSetupCriteriaAndDoTheSearchAndGetDetail();
                ApplicationStatusDetail.ScrollToApplicationStatus();
                ApplicationStatusDetail.ClickFirstCellofColumn(ApplicationStatusDetail.ApplicationStatusDefaultColumnNumber);
                ApplicationStatusDetail.GetEditor().SendKeys("Withdrawn");
                ApplicationStatusDetail.GetEditor().SendKeys(Keys.Down);
                ApplicationStatusDetail.GetEditor().SendKeys(Keys.Enter);
                //                ApplicationStatusDetail.ClickFirstCellofColumn("1");
                //                ApplicationStatusDetail.ScrollToApplicantName();

                //Assert and Verify
                AutomationSugar.Log("Verify the first cell update");
                List<IWebElement> cells = (ApplicationStatusDetail.FindcellsForColumn(ApplicationStatusDetail.ApplicationStatusDefaultColumnNumber));

                string expected = "Withdrawn";
                string actual = cells[0].Text;

                Assert.AreEqual(expected, actual);
            }
        }

        [WebDriverTest(Enabled = true,
             Groups = new[] { "AdmissionsBulkUpdateElementsP2"},
             Browsers = new[] { BrowserDefaults.Chrome })]
        public void Can_Update_ApplicationStatus_As_Mouse_Clicks()
        {
            //Arrange
            DataPackage dataPackage = GetApplicationStatusDataPackage();
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                //Act
                ApplicationStatusDetail appDetail = NavigateAndSetupCriteriaAndDoTheSearchAndGetDetail();
                ApplicationStatusDetail.ScrollToApplicationStatus();
                ApplicationStatusDetail.ClickFirstCellofColumn(ApplicationStatusDetail.ApplicationStatusDefaultColumnNumber);

                //IWebElement dropDown = SeleniumHelper.Get(ApplicationStatusDetail.ApplicationStatusSelector);
                //SeleniumHelper.ScrollToByAction(dropDown);
                //Thread.Sleep(TimeSpan.FromSeconds(2));
                //dropDown.Click();

                appDetail.SetCellApplicationStatusDropDown("Rejected");

                //Assert and Verify
                AutomationSugar.Log("Verify the results");
                List<IWebElement> cells = (ApplicationStatusDetail.FindcellsForColumn(ApplicationStatusDetail.ApplicationStatusDefaultColumnNumber));

                string expected = "Rejected";
                string actual = cells[0].Text;

                Assert.AreEqual(expected, actual);
            }
        }

        [WebDriverTest(Enabled = true,
            Groups = new[] {  "P2", "BulkUpdate", "ApplcationStatusSaveDialog" },
                                    Browsers = new[] { BrowserDefaults.Chrome })]
        public void Checks_Whether_ApplcationStatus_Save_Dialog_Works_As_Expected()
        {
            //Arrange
            DataPackage dataPackage = GetApplicationStatusDataPackage();
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
            {
                //Act
                ApplicationStatusDetail appDetail = NavigateAndSetupCriteriaAndDoTheSearchAndGetDetail();
                ApplicationStatusDetail.ScrollToApplicationStatus();
                List<IWebElement> cells =
                    (ApplicationStatusDetail.FindcellsForColumn(
                        ApplicationStatusDetail.ApplicationStatusDefaultColumnNumber));
                string originalStatus = cells[0].Text;
                ApplicationStatusDetail.ClickFirstCellofColumn(
                    ApplicationStatusDetail.ApplicationStatusDefaultColumnNumber);
                
                //IWebElement dropDown = SeleniumHelper.Get(ApplicationStatusDetail.ApplicationStatusSelector);
                //SeleniumHelper.ScrollToByAction(dropDown);
                //Thread.Sleep(TimeSpan.FromSeconds(2));
                //dropDown.Click();

                appDetail.SetCellApplicationStatusDropDown("Admitted");
                ApplicationStatusConfirmDialogOnSave confirmDialog = appDetail.ClickOnSave();
                confirmDialog.ClickOnCancelButton();

                if (
                    !PageObjectModel.Helper.SeleniumHelper.IsElementDisplayed(
                        PageObjectModel.Helper.SeleniumHelper.Get(Admissions.Component.ApplicationStatusSearch.SearchButton)))
                {
                    SeleniumHelper.WaitForElementClickableThenClick(ApplicationStatusDetail.FiltersButton);
                }

                //Verify
                new ApplicationStatusSearch().ClickOnSearch();
                confirmDialog.ClickOnButtonDontSave();
                ApplicationStatusDetail.ScrollToApplicationStatus();
                cells =
                    (ApplicationStatusDetail.FindcellsForColumn(
                        ApplicationStatusDetail.ApplicationStatusDefaultColumnNumber));
                Assert.IsTrue(cells[0].Text == originalStatus);

                string expected = originalStatus;
                string actual = cells[0].Text;

                Assert.AreEqual(expected, actual);
            }
        }

        #endregion Tests
    }
}