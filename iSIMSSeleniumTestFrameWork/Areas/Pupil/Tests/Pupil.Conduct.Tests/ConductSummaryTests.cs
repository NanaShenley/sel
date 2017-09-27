using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Components.Common;
using POM.Components.Conduct.Pages;
using POM.Helper;
using Pupil.Components;
using Pupil.Components.Common;
using Pupil.Data;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using TestSettings;

namespace Pupil.Conduct.Tests
{
    class ConductSummaryTests
    {
        private readonly SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;

        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups =
                new[]
                {
                    PupilTestGroups.ConductSummary.Page,
                    PupilTestGroups.Priority.Priority2,
                    "TESTME"
                })]
        public void Pupil_LoadConductSummary()
        {
            #region Pre-Condition: Create a new pupil for test

            var surname = Utilities.GenerateRandomString(10, "ConductSummary");
            var forename = Utilities.GenerateRandomString(10, "ConductSummary");
            var dataPackage = this.BuildDataPackage();
            var learnerId = Guid.NewGuid();
            dataPackage.AddBasicLearner(learnerId, surname, forename, new DateTime(2011, 02, 02), new DateTime(2015, 02, 02));

            #endregion

            using (new DataSetup(false, true, dataPackage))
            {
                //Arrange
                SeleniumHelper.Login(LoginAs);

                // Navigate to Conduct Summary page
                new ConductNavigation().NavigateToConductSummaryFromMenu();

                var pupilName = string.Format("{0}, {1}", surname, forename);
                var pupilSearchTriplet = new PupilSearchTriplet();
                pupilSearchTriplet.SearchCriteria.PupilName = pupilName;
                var resultPupils = pupilSearchTriplet.SearchCriteria.Search();
                var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(pupilName));

                Assert.AreNotEqual(null, pupilSearchTile, "Does not found pupil");

                pupilSearchTile.Click();
                var conductSummaryPage = new ConductSummaryPage(AchievementEventData.AchievementLabel, BehaviourEventData.BehaviourLabel);
                conductSummaryPage.ClickAchievementsTab();
                Thread.Sleep(2000);
                Assert.IsTrue(conductSummaryPage.CanSeeAddAchievement, "Add New Achievement Event button is not displayed.");

                conductSummaryPage.ClickBehavioursTab();
                Thread.Sleep(2000);
                Assert.IsTrue(conductSummaryPage.CanSeeAddBehaviour, "Add New Behaviour Event button is not displayed.");
            }
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenConductSummaryFromMenuAsSchoolAdmin()
        {
            CanOpenConductSummaryAsUser(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenConductSummaryFromMenuAsClassTeacher()
        {
            CanOpenConductSummaryAsUser(SeleniumHelper.iSIMSUserType.ClassTeacher);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenConductSummaryFromMenuAsSeniorManager()
        {
            CanOpenConductSummaryAsUser(SeleniumHelper.iSIMSUserType.SeniorManagementTeam);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenConductSummaryFromMenuAsSenCo()
        {
            CanOpenConductSummaryAsUser(SeleniumHelper.iSIMSUserType.SENCoordinator);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenConductSummaryFromMenuAsReturnsManager()
        {
            CanOpenConductSummaryAsUser(SeleniumHelper.iSIMSUserType.ReturnsManager);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CannotOpenConductSummaryFromMenuAsPersonnelOffice()
        {
            CannotOpenConductSummaryAsUser(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
        }

        private void CanOpenConductSummaryAsUser(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType, false);
            AutomationSugar.WaitForAjaxCompletion();
            new ConductNavigation().NavigateToConductSummaryFromMenu();
            Assert.IsNotNull(SeleniumHelper.GetVisible(POM.Helper.SimsBy.AutomationId("pupil_conduct_summary_triplet")));
        }

        private void CannotOpenConductSummaryAsUser(SeleniumHelper.iSIMSUserType userType)
        {
            bool isAccessible = SeleniumHelper.HasMenuPermission("task_menu_conductsummary", userType: userType, enableSelection: false);
            Assert.AreEqual(false, isAccessible);
        }
    }
}
