using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Pupil.Components;
using Pupil.Components.Common;
using Pupil.Components.PupilRecord;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;


namespace Pupil.PupilRecords.Tests
{
    public class DisabledTests : BaseSeleniumComponents
    {

        private PupilRecordNavigation _navigation;

       
        //[WebDriverTest(Groups = new[] { PupilTestGroups.Application.ApplicationDOAChange, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void GoTo_Admissions_Application_Screen()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

            ShellAction.OpenTaskMenu();

            TaskMenuActions.OpenMenuSection("section_menu_Admissions");

            TaskMenuActions.ClickMenuItem("task_menu_section_admissions_applications");

            SearchCriteria.SetCriteria("ApplicantName", PupilAreaConstants.ApplicantNameSearchString);

            SearchCriteria.Search();
            Thread.Sleep(2000);
            SearchResults.WaitForResults();
            Thread.Sleep(2000);
            Assert.IsTrue(SearchResults.HasResults());
            Assert.IsTrue(SearchResults.HasResults(1));

            SearchResults.Click(PupilAreaConstants.ApplicationId);

            Detail.WaitForDetail();

            // Now we are ready to change Admission Date / Group etc
            var originalDOAinput = SeleniumHelper.Get(By.CssSelector("input[name='LearnerApplication.DateOfAdmission']"));

            var originalDOA = originalDOAinput.GetValue();
            Debug.WriteLine(originalDOA);

            var dateElems = originalDOA.Split('/');
            var newDOA = new DateTime(int.Parse(dateElems[2]), int.Parse(dateElems[1]), int.Parse(dateElems[0]));

            // Adjust the DOA to a new Date
            newDOA = newDOA.AddDays(10);
            var newDOADisplay = newDOA.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            Debug.WriteLine(newDOA.ToString(newDOADisplay));
            originalDOAinput.SetText(newDOA.ToString(newDOADisplay));

            // Give AJAX routines a chance to run
            Thread.Sleep(5000);

            // check all Dated Memberships now reflect the new date
            SeleniumHelper.WaitForElementClickableThenClick(
                By.CssSelector("button[data-history-editor-dialog='#LearnerNCYearMemberships-grid-editor-dialog']"));

            // Wait for pop up grid to appear
            Thread.Sleep(2000);

            var ncYearStartDate = SeleniumHelper.Get(By.CssSelector("[name$='.StartDate']")).GetValue();

            Assert.IsTrue(ncYearStartDate == newDOA.ToString("d"));
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }


        //[WebDriverTest(Groups = new[] { PupilTestGroups.PupilRecord.SaveAlertTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldDisplay_PupilLogNoteSavedAlert_OnSavingAPupilLogNote()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            GetNavigation();

            By contextualLink = By.CssSelector(SeleniumHelper.AutomationId("create-plog-contextual-link"));
            WaitForElement(contextualLink);
            PageFactory.InitElements(WebContext.WebDriver, this);

            SeleniumHelper.FindAndClick(contextualLink);
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("plog-generalcategory-contextlink"));

            By noteText = By.CssSelector(PupilElements.PupilLog.Detail.Note.NoteText);
            WaitForElement(noteText);

            ControlsHelper.UpdateValue(PupilElements.PupilLog.Detail.Note.NoteText, "Notes added by test");
            SeleniumHelper.FindAndClick(By.CssSelector(String.Format(PupilElements.PupilLog.Detail.Note.SaveBtn)));

            var a = SeleniumHelper.Get(By.CssSelector(PupilElements.PupilLog.Detail.DataConfirmationContainer));

            By dataConfirmContainer = By.ClassName("alert");
            WaitForElement(dataConfirmContainer);

            Assert.IsTrue(a.FindChild(By.ClassName("alert")).Text == "Pupil Log Note saved");
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        //[WebDriverTest(Groups = new[] { PupilTestGroups.PupilRecord.SaveAlertTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldNotDisplay_PupilLogNoteSavedAlert_OnCancellingAPupilLogNote()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            GetNavigation();

            By contextualLink = By.CssSelector(SeleniumHelper.AutomationId("create-plog-contextual-link"));
            WaitForElement(contextualLink);
            PageFactory.InitElements(WebContext.WebDriver, this);

            SeleniumHelper.FindAndClick(contextualLink);
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("plog-generalcategory-contextlink"));

            By noteText = By.CssSelector(PupilElements.PupilLog.Detail.Note.NoteText);
            WaitForElement(noteText);

            ControlsHelper.UpdateValue(PupilElements.PupilLog.Detail.Note.NoteText, "Notes added by test");
            SeleniumHelper.FindAndClick(By.CssSelector(String.Format(PupilElements.PupilLog.Detail.Note.CancelBtn)));

            var dataConfirmContainer = SeleniumHelper.Get(By.CssSelector(PupilElements.PupilLog.Detail.DataConfirmationContainer));

            Assert.IsTrue(dataConfirmContainer.FindElements(By.ClassName("alert")).Count == 0);
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        //[WebDriverTest(Groups = new[] { PupilTestGroups.PupilRecord.ContextualActionsTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SENRecordContextualActionLoadTest()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //Arrange
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            var pupilNavigation = new PupilRecordNavigation();

            //Act
            var SENDetails = pupilNavigation.NavigateToPupilRecord_ContextualActions_SENDetail(PupilAreaConstants.LearnerId);

            //Assert
            Assert.AreEqual(PupilAreaConstants.LearnerListName, SENDetails.PreferredListName.Text);
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        //[WebDriverTest(Groups = new[] { PupilTestGroups.PupilRecord.DeceasedTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Pupil_Deceased_Date_Cleared_Returns_Success()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            GetNavigation();
            // Enter bad date (before DoA)
            _navigation.NavigateAndSetDeceasedDate("");
            _navigation.SaveDeceasedDate();
            var reply = _navigation.FindSaveSuccessMessage();
            Assert.IsTrue(reply);
            ResetDeceasedDate();
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }
        //[WebDriverTest(Groups = new[] { PupilTestGroups.PupilRecord.SearchTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CanSearchLoadPupil()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            var pupilNavigation = new PupilRecordNavigation();
            pupilNavigation.NavigateToPupilRecord_PupilDetailPage(PupilAreaConstants.LearnerId);
            Thread.Sleep(2000);
            Assert.AreEqual(PupilAreaConstants.LearnerLegalSurname, SeleniumHelper.Get(PupilElements.PupilRecord.LegalSurname).GetValue());
        }

        //[WebDriverTest(Groups = new[] { PupilTestGroups.PupilRecord.DetailPageTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CanLoadPupil_AndCheckRegistrationDetails()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            var pupilNavigation = new PupilRecordNavigation();
            pupilNavigation.NavigateToPupilRecord_PupilDetailPage(PupilAreaConstants.LearnerId);
            Thread.Sleep(2000);
            Assert.AreEqual(PupilAreaConstants.LearnerYearGroup3, SeleniumHelper.Get(PupilElements.PupilRecord.LearnerYearGroupMemberships).GetValue());
            Assert.AreEqual(PupilAreaConstants.LearnerDateOfAdmission, SeleniumHelper.Get(PupilElements.PupilRecord.DateOfAdmission).GetValue());
        }

        //[WebDriverTest(Groups = new[] { PupilTestGroups.PupilRecord.UpdateTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void UpdatePupilsEthnicity()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(BrowserDefaults.ElementTimeOut);
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            ShellAction.OpenTaskMenu();

            TaskMenuActions.OpenMenuSection("section_menu_Pupils");

            TaskMenuActions.ClickMenuItem("task_menu_section_pupils_pupil_record");
            SearchCriteria.SetCriteria("LegalSurname", "phi");

            SearchCriteria.ShowAdvanced();

            SearchCriteria.SetCriteria("PrimaryClass.dropdownImitator", "Robin");

            SearchCriteria.Search();

            SearchResults.WaitForResults();

            Assert.IsTrue(SearchResults.HasResults());
            Assert.IsTrue(SearchResults.HasResults(2));

            SearchResults.Click(PupilAreaConstants.LearnerId);

            Detail.WaitForDetail();

            Detail.OpenSection(PupilAreaConstants.EthnicCulturalSectionLinkId);

            Assert.IsTrue(Detail.IsSectionOpen(PupilAreaConstants.EthnicCulturalSectionLinkId));

            Detail.UpdateValue("Ethnicity.dropdownImitator", "White");
            Detail.UpdateValue("Language.dropdownImitator", "Finnish");
            Detail.UpdateValue("Religion.dropdownImitator", "No Religion");
            //Detail.UpdateValue("IsTaughtThroughIrishMedium", true); --Need to revisit as it's only for NI* schools it seems

            Detail.Save();
            Detail.WaitForStatus();

            Assert.IsTrue(Detail.HasConfirmedSave());
            Assert.IsTrue(Detail.IsSectionOpen(PupilAreaConstants.EthnicCulturalSectionLinkId));
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        //[WebDriverTest(Enabled = true,
        //    Groups = new[] { PupilTestGroups.PupilRecord.DetailPageTests, PupilTestGroups.Priority.Priority3 },
        //    Browsers = new[] 
        //    { 
        //        BrowserDefaults.Chrome, 
        //        BrowserDefaults.Ie 
        //    })]
        public void Add_Delete_Pupil_Enrolment_Status_History()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //Arrange
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            var pupilDetail = PupilRecordMenuLink.Search.SearchAndSelectPupil();

            //Act
            pupilDetail.AddNewEnrolmentStatusHistory().Save();

            //Assert
            Assert.IsTrue(pupilDetail.HasSavedSuccessfully());

            //Arrange
            pupilDetail.CloseSuccessMessage();

            //Act
            pupilDetail.DeleteNewEnrolmentStatusHistory().Save();

            //Assert
            Assert.IsTrue(pupilDetail.HasSavedSuccessfully());
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        //[WebDriverTest(Enabled = true,
        //   Groups = new[] { PupilTestGroups.PupilRecord.DetailPageTests, PupilTestGroups.Priority.Priority3 },
        //   Browsers = new[] 
        //    { 
        //        BrowserDefaults.Chrome
        //    })]
        public void Add_Delete_Pupil_Previous_School_History()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //Arrange
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            var pupilDetail = PupilRecordMenuLink.Search.SearchAndSelectPupil();

            //Act
            pupilDetail.AddNewPreviousSchoolHistory().Save();

            //Assert
            Assert.IsTrue(pupilDetail.HasSavedSuccessfully());

            //Arrange
            pupilDetail.CloseSuccessMessage();

            //Act
            pupilDetail.DeleteNewPreviousSchoolHistory().Save();

            //Assert
            Assert.IsTrue(pupilDetail.HasSavedSuccessfully());
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Setup Deceased Date screen
        /// </summary>


        //TODO: Duplication
        //[WebDriverTest(Groups = new[] { PupilTestGroups.PupilRecord.DeceasedTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Pupil_Deceased_Date_Before_LeavingDate_Returns_Validation_Error()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            GetNavigation();
            // Enter bad date (before DoA)
            _navigation.NavigateAndSetDeceasedDate("19/05/2015");
            _navigation.SaveDeceasedDate();
            var reply = _navigation.FindSaveDeceasedValidationWarning();
            Assert.IsTrue(reply);
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }


        private void GetNavigation()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            _navigation = new PupilRecordNavigation();

            // Load pupils, leavers, select above student
            _navigation.NavigateToPupilRecords();

            SeleniumHelper.FindAndClick(PupilElements.PupilRecord.PupilSearchPanel.SearchButton);

            _navigation.SelectSpecificPupil(PupilAreaConstants.LearnerId);
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }
        /// <summary>
        /// Blank DOD
        /// </summary>
        private void ResetDeceasedDate()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            _navigation.NavigateAndSetDeceasedDate("");
            _navigation.SaveDeceasedDate();
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

    }
}
