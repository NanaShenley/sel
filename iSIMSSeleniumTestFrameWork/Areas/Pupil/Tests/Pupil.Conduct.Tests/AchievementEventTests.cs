using NUnit.Framework;
using POM.Components.Pupil.Pages;
using POM.Helper;
using Pupil.Components;
using Pupil.Components.Common;
using Pupil.Data;
using Pupil.Data.Entities;
using Selene.Support.Attributes;
using SeSugar.Data;
using System;
using OpenQA.Selenium;
using POM.Components.Conduct.Pages;
using SeSugar;
using SeSugar.Automation;
using TestSettings;

namespace Pupil.Achievements.Tests
{
    public class AchievementEventTests
    {        
        private readonly SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;

        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups =
                new[]
                {
                    PupilTestGroups.AchievementEvent.Page,
                    PupilTestGroups.AchievementEvent.AddNewAchievementEvents,
                    PupilTestGroups.Priority.Priority2,
                    "FAIL"
                })]

        public void PupilConduct_AddNewAchievementEvent()
        {
            #region Pre-Condition: Create a new pupil for test

            var surname = Utilities.GenerateRandomString(10, "AchievementEvent");
            var forename = Utilities.GenerateRandomString(10, "AchievementEvent");
            var dataPackage = this.BuildDataPackage();
            var learnerId = Guid.NewGuid();
            dataPackage.AddBasicLearner(learnerId, surname, forename, new DateTime(2011, 02, 02), new DateTime(2015, 02, 02));

            #endregion

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    SeleniumHelper.Login(LoginAs);

                    // Navigate to Pupil Record
                    // TODO: Menu item will have user configured name instead of Achievement. Needs to retrieve configured value from DB if it's run against live.
                    new ConductNavigation().NavigateToAchievementEventsFromMenu();

                    var achievementEventPage = new AchievementEventRecordPage();
                    achievementEventPage.ClickAdd();
                    var pupilPickerDialog = achievementEventPage.ClickAddPupil();

                    // Open specific pupil record
                    pupilPickerDialog.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilPickerDialog.SearchCriteria.Search();
                    var pupilSearchTile =
                        resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    pupilSearchTile.Click();

                    pupilPickerDialog.AddSelectedPupil();
                    pupilPickerDialog.ClickOk();

                    AutomationSugar.WaitForAjaxCompletion();

                    achievementEventPage.ClickSave();

                    // Verify data is saved Success
                    Assert.AreEqual(true, achievementEventPage.IsSuccessMessageDisplayed(),
                        "Success message is not display");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAchievementEventForLearner(learnerId);
                }
            }
        }

        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups =
                new[]
                {
                    PupilTestGroups.AchievementEvent.Page,
                    PupilTestGroups.AchievementEvent.UpdateAchievementEvents,
                    PupilTestGroups.Priority.Priority2,
                    "FAIL"
                })]
        public void PupilConduct_UpdateAchievementEvent()
        {
            #region Pre-Condition: Create a new pupil for test
            var dataPackage = this.BuildDataPackage();

            var surname = Utilities.GenerateRandomString(10, "AchievementEventA");
            var forename = Utilities.GenerateRandomString(10, "AchievementEventA");
            var learnerId = Guid.NewGuid();
            dataPackage.AddBasicLearner(learnerId, surname, forename, new DateTime(2011, 02, 02), new DateTime(2015, 02, 02));

            var surnameB = Utilities.GenerateRandomString(10, "AchievementEventB");
            var forenameB = Utilities.GenerateRandomString(10, "AchievementEventB");
            dataPackage.AddBasicLearner(Guid.NewGuid(), surnameB, forenameB, new DateTime(2011, 02, 02), new DateTime(2015, 02, 02));

            var achievementEventId = Guid.NewGuid();
            var learnerAchievementEventId = Guid.NewGuid();
            var followUpId = Guid.NewGuid();
            var achievementEventCategory = Queries.GetFirstFullConductEventLookup("Achievement");
            dataPackage.AddAchievementEvent(achievementEventId, learnerId, learnerAchievementEventId, followUpId, achievementEventCategory.Id);

            #endregion

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    SeleniumHelper.Login(LoginAs);

                    // Navigate to Pupil Record
                    new ConductNavigation().NavigateToAchievementEventsFromMenu();

                    var achievementEventTriplet = new AchievementEventTriplet();
                    achievementEventTriplet.SearchCriteria.AchievementEventCategory = achievementEventCategory.Description;

                    var achievementSearchResult = achievementEventTriplet.SearchCriteria.Search();
                    var achievementEventSearchResultTile =
                        achievementSearchResult.FirstOrDefault(x => x.Name == achievementEventCategory.Description);

                    Assert.AreNotEqual(null, achievementEventSearchResultTile, "Achievement Event does not exist.");

                    var achievementEventPage = achievementEventSearchResultTile.Click<AchievementEventRecordPage>();

                    //Update event date (incase if the migrated data has got wrong date which will prevent save).
                    achievementEventPage.AchievementEventDate = SeleniumHelper.GetToday();

                    var pupilPickerDialog = achievementEventPage.ClickAddPupil();

                    // Open specific pupil record
                    pupilPickerDialog.SearchCriteria.PupilName = String.Format("{0}, {1}", surnameB, forenameB);
                    var resultPupils = pupilPickerDialog.SearchCriteria.Search();
                    var pupilSearchTile =
                        resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surnameB, forenameB)));

                    pupilSearchTile.Click();

                    pupilPickerDialog.AddSelectedPupil();
                    pupilPickerDialog.ClickOk();

                    AutomationSugar.WaitForAjaxCompletion();

                    achievementEventPage.ClickSave();

                    // Verify data is saved Success
                    Assert.AreEqual(true, achievementEventPage.IsSuccessMessageDisplayed(),
                        "Success message is not display");
                }
                finally
                {
                    // Deleting manually as failing to delete the event tables in the right order.
                    PurgeLinkedData.DeleteAchievementEventForLearner(learnerId);
                }
            }
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
             Groups =
                 new[]
                 {
                     PupilTestGroups.ClassLog.Conduct,
                     PupilTestGroups.Priority.Priority2,
                     "CLOG"
                 })]
        public void Can_Add_Achivement_From_Class_Log_Quick_Form()
        {
            string learnerId = string.Empty;

            try
            {
                // Arrange
                ClassTeacher teacher = ClassLogData.GetClassTeacherWithClassAndLearners();
                AuthorisedUser authUser = ClassLogData.GetAuthorisedUserDetailsForClassTeacherUser(TestDefaults.Default.ClassTeacher);
                ClassLogData.UpdateClassTeacherWithInitialAuthUserValues(authUser, TestDefaults.Default.ClassTeacher);
                ClassLogData.UpdateClassTeacherUserWithStaffDetails(teacher, TestDefaults.Default.ClassTeacher);

                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
                Wait.WaitForAjaxReady();

                var classLogNavigate = new ClassLogNavigation();
                classLogNavigate.NavigateToPupilClassLogFromMenu();

                ClassLogPage clogPage = new ClassLogPage();

                clogPage.SelectPupil();
                learnerId = clogPage.SelectedLearnerId;

                var achievementDialog = clogPage.OpenAchievementPopup();

                var achievementEvent = Queries.GetFirstQuickConductEventTypeLookup("Achievement");

                achievementDialog.AchievementType = achievementEvent.Description;

                // Increment twice and then decrement once to test slider working
                achievementDialog.ClickPointsSliderUp();
                achievementDialog.ClickPointsSliderUp();

                achievementDialog.ClickPointsSliderDown();

                achievementDialog.Comments = "Selenium test event";

                achievementDialog.Save();

                // Assert?
                SeleniumHelper.WaitUntilElementIsDisplayed("//*/div[@class='alert alert-info animated zoomInAndFade' and @role='alert']");
            }
            finally
            {
                if (learnerId != string.Empty)
                {
                    //TODO: To be re-introduced when Pupil is created as part of Data setup for the test to make this test Non-destructive.
                    //PurgeLinkedData.DeleteAchievementEventForLearner(Guid.Parse(learnerId));
                }
            }
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.AchievementEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenAchievementEventsFromMenuAsSchoolAdmin()
        {
            CanOpenAchievementEventsAsUser(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.AchievementEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenAchievementEventsFromMenuAsClassTeacher()
        {
            CanOpenAchievementEventsAsUser(SeleniumHelper.iSIMSUserType.ClassTeacher);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.AchievementEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenAchievementEventsFromMenuAsSeniorManager()
        {
            CanOpenAchievementEventsAsUser(SeleniumHelper.iSIMSUserType.SeniorManagementTeam);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.AchievementEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenAchievementEventsFromMenuAsSenCo()
        {
            CanOpenAchievementEventsAsUser(SeleniumHelper.iSIMSUserType.SENCoordinator);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.AchievementEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenAchievementEventsFromMenuAsReturnsManager()
        {
            CanOpenAchievementEventsAsUser(SeleniumHelper.iSIMSUserType.ReturnsManager);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.AchievementEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CannotOpenAchievementEventsFromMenuAsPersonnelOffice()
        {
            CannotOpenAchievementEventsAsUser(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
        }

        private void CanOpenAchievementEventsAsUser(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType, false);
            AutomationSugar.WaitForAjaxCompletion();
            new ConductNavigation().NavigateToAchievementEventsFromMenu();
            Assert.IsNotNull(SeleniumHelper.GetVisible(POM.Helper.SimsBy.AutomationId("pupil_achievement_detail")));
        }

        private void CannotOpenAchievementEventsAsUser(SeleniumHelper.iSIMSUserType userType)
        {
            bool isAccessible = SeleniumHelper.HasMenuPermission("task_menu_conduct_achievement_event", userType: userType, enableSelection: false);
            Assert.AreEqual(false, isAccessible);
        }
    }
}
