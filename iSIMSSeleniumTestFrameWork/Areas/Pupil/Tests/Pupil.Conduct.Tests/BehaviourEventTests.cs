using System;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Components.Conduct.Pages;
using POM.Components.Pupil.Pages;
using POM.Helper;
using Pupil.Components;
using Pupil.Components.Common;
using Pupil.Data;
using Pupil.Data.Entities;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using TestSettings;

namespace Pupil.BehaviourEventTest.Tests
{
    public class BehaviourEventTest
    {
        private readonly SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;

        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups =
                new[]
                {
                    PupilTestGroups.BehaviourEvent.Page,
                    PupilTestGroups.BehaviourEvent.AddNewBehaviourEvents,
                    PupilTestGroups.Priority.Priority2,
                    "FAIL"
                })]

        public void PupilConduct_AddNewBehaviourEvent()
        {
            #region Pre-Condition: Create a new pupil for test

            var surname = Utilities.GenerateRandomString(10, "BehaviourEvent");
            var forename = Utilities.GenerateRandomString(10, "BehaviourEvent");
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

                    new ConductNavigation().NavigateToBehaviourEventsFromMenu();

                    var behaviourEventPage = new BehaviourEventRecordPage();
                    behaviourEventPage.ClickAdd();
                    var pupilPickerDialog = behaviourEventPage.ClickAddPupil();

                    // Open specific pupil record
                    pupilPickerDialog.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilPickerDialog.SearchCriteria.Search();
                    var pupilSearchTile =
                        resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    pupilSearchTile.Click();

                    pupilPickerDialog.AddSelectedPupil();
                    pupilPickerDialog.ClickOk();

                    // Wait for Pupil row to appear
                    Wait.WaitForElementDisplayed(By.CssSelector("[data-row-name='LearnerBehaviourEvents']"));

                    // Add follow up
                    behaviourEventPage.ClickFollowUpTab();

                    behaviourEventPage.FollowUpAction = Queries.GetFirstLookupField("BehaviourEventFollowUpAction", "Description");

                    behaviourEventPage.SelectPupilInvolvedForFollowUp(string.Format("{0} {1}", forename, surname));

                    behaviourEventPage.ClickApplyFollowUp();

                    behaviourEventPage.ClickSave();

                    // Verify data is saved Success
                    Assert.AreEqual(true, behaviourEventPage.IsSuccessMessageDisplayed(),
                        "Success message is not display");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteBehaviourEventForLearner(learnerId);
                }
            }
        }

        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups =
                new[]
                {
                    PupilTestGroups.BehaviourEvent.Page,
                    PupilTestGroups.BehaviourEvent.UpdateBehaviourEvents,
                    PupilTestGroups.Priority.Priority2,
                    "FAIL"
                })]
        public void PupilConduct_UpdateBehaviourEvent()
        {
            #region Pre-Condition: Create a new pupil for test
            var dataPackage = this.BuildDataPackage();

            var surname = Utilities.GenerateRandomString(10, "BehaviourEventA");
            var forename = Utilities.GenerateRandomString(10, "BehaviourEventA");
            var learnerId = Guid.NewGuid();
            dataPackage.AddBasicLearner(learnerId, surname, forename, new DateTime(2011, 02, 02), new DateTime(2015, 02, 02));

            var surnameB = Utilities.GenerateRandomString(10, "BehaviourEventB");
            var forenameB = Utilities.GenerateRandomString(10, "BehaviourEventB");
            dataPackage.AddBasicLearner(Guid.NewGuid(), surnameB, forenameB, new DateTime(2011, 02, 02), new DateTime(2015, 02, 02));

            var behaviourEventId = Guid.NewGuid();
            var learnerBehaviourEventId = Guid.NewGuid();
            var followUpId = Guid.NewGuid();
            var behaviourEventCategory = Queries.GetFirstFullConductEventLookup("Behaviour");
            var behaviourEventFollowUpAction = Queries.GetFirstLookupEntry("BehaviourEventFollowUpAction");
            dataPackage.AddBehaviourEvent(behaviourEventId, learnerId, learnerBehaviourEventId, followUpId, behaviourEventCategory.Id, behaviourEventFollowUpAction.Id);

            #endregion

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    SeleniumHelper.Login(LoginAs);

                    new ConductNavigation().NavigateToBehaviourEventsFromMenu();

                    var behaviourEventTriplet = new BehaviourEventTriplet();
                    behaviourEventTriplet.SearchCriteria.BehaviourEventCategory = behaviourEventCategory.Description;

                    var behaviourSearchResult = behaviourEventTriplet.SearchCriteria.Search();
                    var behaviourEventSearchResultTile =
                        behaviourSearchResult.FirstOrDefault(x => x.Name == behaviourEventCategory.Description);

                    Assert.AreNotEqual(null, behaviourEventSearchResultTile, "Behaviour Event does not exist.");

                    var behaviourEventPage = behaviourEventSearchResultTile.Click<BehaviourEventRecordPage>();

                    //Update event date (incase if the migrated data has got wrong date which will prevent save).
                    behaviourEventPage.BehaviourEventDate = SeleniumHelper.GetToday();

                    var pupilPickerDialog = behaviourEventPage.ClickAddPupil();

                    // Open specific pupil record
                    pupilPickerDialog.SearchCriteria.PupilName = String.Format("{0}, {1}", surnameB, forenameB);
                    var resultPupils = pupilPickerDialog.SearchCriteria.Search();
                    var pupilSearchTile =
                        resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surnameB, forenameB)));

                    pupilSearchTile.Click();

                    pupilPickerDialog.AddSelectedPupil();
                    pupilPickerDialog.ClickOk();

                    AutomationSugar.WaitForAjaxCompletion();

                    // Add follow up
                    behaviourEventPage.ClickFollowUpTab();

                    behaviourEventPage.FollowUpAction = behaviourEventFollowUpAction.Description;

                    behaviourEventPage.SelectPupilInvolvedForFollowUp(string.Format("{0} {1}", forenameB, surnameB));

                    behaviourEventPage.ClickApplyFollowUp();

                    behaviourEventPage.ClickSave();

                    // Verify data is saved Success
                    Assert.AreEqual(true, behaviourEventPage.IsSuccessMessageDisplayed(),
                        "Success message is not display");
                }
                finally
                {
                    // Deleting manually as failing to delete the event tables in the right order.
                    PurgeLinkedData.DeleteBehaviourEventForLearner(learnerId);
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
        public void Can_Add_Behaviour_From_Class_Log_Quick_Form()
        {
            string learnerId = string.Empty;

            try
            {
                // Arrange
                ClassTeacher teacher = ClassLogData.GetClassTeacherWithClassAndLearners();
                AuthorisedUser authUser =
                    ClassLogData.GetAuthorisedUserDetailsForClassTeacherUser(TestDefaults.Default.ClassTeacher);
                ClassLogData.UpdateClassTeacherWithInitialAuthUserValues(authUser, TestDefaults.Default.ClassTeacher);
                ClassLogData.UpdateClassTeacherUserWithStaffDetails(teacher, TestDefaults.Default.ClassTeacher);

                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);

                Wait.WaitForAjaxReady();

                var classLogNavigate = new ClassLogNavigation();
                classLogNavigate.NavigateToPupilClassLogFromMenu();

                ClassLogPage clogPage = new ClassLogPage();

                clogPage.SelectPupil();

                learnerId = clogPage.SelectedLearnerId;

                var behaviourDialog = clogPage.OpenBehaviourPopup();

                var behaviourType = Queries.GetFirstQuickConductEventTypeLookup("Behaviour");

                behaviourDialog.BehaviourEventType = behaviourType.Description;

                // Increment twice and then decrement once to test slider working
                behaviourDialog.ClickPointsSliderUp();
                behaviourDialog.ClickPointsSliderUp();
                behaviourDialog.ClickPointsSliderDown();

                behaviourDialog.Comments = "Selenium test event";

                behaviourDialog.Save();

                // Assert?
                SeleniumHelper.WaitUntilElementIsDisplayed(
                    "//*/div[@class='alert alert-info animated zoomInAndFade' and @role='alert']");
            }
            finally
            {
                if (learnerId != string.Empty)
                {
                    //TODO: To be re-introduced when Pupil is created as part of Data setup for the test to make this test Non-destructive.
                    // PurgeLinkedData.DeleteBehaviourEventForLearner(Guid.Parse(learnerId));
                }
            }
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenBehaviourEventsFromMenuAsSchoolAdmin()
        {
            CanOpenBehaviourEventsAsUser(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenBehaviourEventsFromMenuAsClassTeacher()
        {
            CanOpenBehaviourEventsAsUser(SeleniumHelper.iSIMSUserType.ClassTeacher);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenBehaviourEventsFromMenuAsSeniorManager()
        {
            CanOpenBehaviourEventsAsUser(SeleniumHelper.iSIMSUserType.SeniorManagementTeam);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenBehaviourEventsFromMenuAsSenCo()
        {
            CanOpenBehaviourEventsAsUser(SeleniumHelper.iSIMSUserType.SENCoordinator);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenBehaviourEventsFromMenuAsReturnsManager()
        {
            CanOpenBehaviourEventsAsUser(SeleniumHelper.iSIMSUserType.ReturnsManager);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CannotOpenBehaviourEventsFromMenuAsPersonnelOffice()
        {
            CannotOpenBehaviourEventsAsUser(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
        }

        private void CanOpenBehaviourEventsAsUser(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType, false);
            AutomationSugar.WaitForAjaxCompletion();
            new ConductNavigation().NavigateToBehaviourEventsFromMenu();
            Assert.IsNotNull(SeleniumHelper.GetVisible(POM.Helper.SimsBy.AutomationId("pupil_behaviour_detail")));
        }

        private void CannotOpenBehaviourEventsAsUser(SeleniumHelper.iSIMSUserType userType)
        {
            bool isAccessible = SeleniumHelper.HasMenuPermission("task_menu_conduct_behaviour_event", userType: userType, enableSelection: false);
            Assert.AreEqual(false, isAccessible);
        }
    }
}