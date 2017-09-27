using NUnit.Framework;
using OpenQA.Selenium;
using POM.Components.Conduct.Pages;
using POM.Helper;
using Pupil.Components;
using Pupil.Components.Common;
using Pupil.Data;
using Selene.Support.Attributes;
using SeSugar.Automation;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Pupil.Conduct.Tests
{
    class ConductConfigurationTests
    {
        private readonly SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;

        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups =
                new[]
                {
                    PupilTestGroups.ConductConfiguration.Page,
                    PupilTestGroups.Priority.Priority2,
                    "Pupil_LoadConductConfiguration"
                })]
        public void Pupil_LoadConductConfiguration()
        {
            //Arrange
            SeleniumHelper.Login(LoginAs);

            // Navigate to Conduct Summary page
            new ConductNavigation().NavigateToConductConfigurationFromMenu();

            var conductConfigurationPage = new ConductConfigurationPage
            {
                AchievementLabel = "Merit",
                BehaviourLabel = "Sanction"
            };

            if (conductConfigurationPage.DoesAchievementUiConfigExist)
            {
                var currentAchievementUiConfig = conductConfigurationPage.AchievementUiConfig;
                conductConfigurationPage.AchievementUiConfig = !currentAchievementUiConfig;
            }

            if (conductConfigurationPage.DoesBehaviourUiConfigExist)
            {
                var currentBehaviourUiConfig = conductConfigurationPage.BehaviourUiConfig;
                conductConfigurationPage.BehaviourUiConfig = !currentBehaviourUiConfig;
            }

            var defaultAchievementType = conductConfigurationPage.DefaultAchievementType = Queries.GetFirstFullConductEventLookup("Achievement").Description;
            var defaultBehaviourType = conductConfigurationPage.DefaultBehaviourType = Queries.GetFirstFullConductEventLookup("Behaviour").Description;
            conductConfigurationPage.PointsIncrement = "1";

            conductConfigurationPage.ClickSave();

            Assert.IsTrue(conductConfigurationPage.IsSuccessMessageDisplayed(), "Success message not displayed.");

            // Clear unload javascript and Refresh the browser
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript("window.onbeforeunload = null;");
            WebContext.WebDriver.Navigate().Refresh();
            Wait.WaitForDocumentReady();

            AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Merit Events");
            var achievementEventRecord = AchievementEventRecordPage.Create();

            achievementEventRecord.ClickAdd();
            Assert.AreEqual(defaultAchievementType, achievementEventRecord.AchievementEventCategory, "Default Achievement Type doesn't match the configured value.");

            AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Sanction Events");
            var behaviourEventRecord = BehaviourEventRecordPage.Create();

            behaviourEventRecord.ClickAdd();
            Assert.AreEqual(defaultBehaviourType, behaviourEventRecord.BehaviourEventCategory, "Default Behaviour Type doesn't match the configured value.");
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ConductConfiguration.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CanOpenConductConfigurationFromMenuAsSchoolAdmin()
        {
            CanOpenConductConfigurationAsUser(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ConductConfiguration.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CannotOpenConductConfigurationFromMenuAsPersonnelOffice()
        {
            CannotOpenConductConfigurationAsUser(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ConductConfiguration.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CannotOpenConductConfigurationFromMenuAsClassTeacher()
        {
            CannotOpenConductConfigurationAsUser(SeleniumHelper.iSIMSUserType.ClassTeacher);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ConductConfiguration.Page, PupilTestGroups.Priority.Priority2, "PP" })]
        public void CannotOpenConductConfigurationFromMenuAsSeniorManager()
        {
            CannotOpenConductConfigurationAsUser(SeleniumHelper.iSIMSUserType.SeniorManagementTeam);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ConductConfiguration.Page, PupilTestGroups.Priority.Priority2, "LookupPP" })]
        public void CanOpenConductLookupFromMenuAsSchoolAdmin()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.NavigateMenu("Lookups", "Pupil Conduct", "Conduct Event Pupil Role");
            Assert.AreEqual("Conduct Event Pupil Role", SeleniumHelper.FindElement(POM.Helper.SimsBy.AutomationId("lookups_header_display_name")).GetText());
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ConductConfiguration.Page, PupilTestGroups.Priority.Priority2, "LookupPP" })]
        public void CannotOpenConductLookupFromMenuAsTeacher()
        {
            bool isAccessible = SeleniumHelper.HasMenuPermission("task_menu_conduct_staff_involvement", userType: SeleniumHelper.iSIMSUserType.ClassTeacher, enableSelection:false);
            Assert.AreEqual(false, isAccessible);
        }

        private void CanOpenConductConfigurationAsUser(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType, false);
            AutomationSugar.WaitForAjaxCompletion();
            new ConductNavigation().NavigateToConductConfigurationFromMenu();
            Assert.IsNotNull(SeleniumHelper.GetVisible(POM.Helper.SimsBy.AutomationId("pupil_conduct_configuration_detail")));
        }

        private void CannotOpenConductConfigurationAsUser(SeleniumHelper.iSIMSUserType userType)
        {
            bool isAccessible = SeleniumHelper.HasMenuPermission("task_menu_conduct_configuration", userType: userType, enableSelection: false);
            Assert.AreEqual(false, isAccessible);
        }
    }
}
