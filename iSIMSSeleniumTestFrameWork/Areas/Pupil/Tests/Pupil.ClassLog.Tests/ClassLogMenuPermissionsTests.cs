using System.Security.Principal;
using NUnit.Framework;
using OpenQA.Selenium;
using Pupil.Components;
using Pupil.Components.Common;
using TestSettings;
using WebDriverRunner.internals;
using POM.Helper;
using Selene.Support.Attributes;
using SeSugar.Automation;


namespace Pupil.ClassLog.Tests
{
    /*
    NOTE: Class Log tests are not currently included in the overnight selenium test runner as this feature is not yet live.
    However, these tests should still be run locally to ensure we don't break any P1/P2 functionality as we develop the feature
    */

    public class ClassLogMenuPermissionsTests
    {
        private const string ClassLogFeature = "Class Log";

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2 })]
        public void CanOpenClassLogFromMenuAsClassTeacher()
        {
            var tabElement = HasPermissionsToAccessMenu(SeleniumHelper.iSIMSUserType.ClassTeacher);
            Assert.IsNotNull(tabElement);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2 })]
        public void CanOpenClassLogFromMenuAsSchoolAdministrator()
        {
            var tabElement = HasPermissionsToAccessMenu(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Assert.IsNotNull(tabElement);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2 })]
        public void CanOpenClassLogFromMenuAsSENCoordinator()
        {
            var tabElement = HasPermissionsToAccessMenu(SeleniumHelper.iSIMSUserType.SENCoordinator);
            Assert.IsNotNull(tabElement);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2})]
        public void CanOpenClassLogFromMenuAsAssessmentCoordinator()
        {
           var tabElement= HasPermissionsToAccessMenu(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            Assert.IsNotNull(tabElement);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2 })]
        public void CanOpenClassLogFromMenuAsSeniorManagementTeam()
        {

            var tabElement = HasPermissionsToAccessMenu(SeleniumHelper.iSIMSUserType.SeniorManagementTeam);
            Assert.IsNotNull(tabElement);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2 })]
        public void CanOpenClassLogFromQuickLinkAsAssessmentCoordinator()
        {
            var tabElement= HasPermissionsToAccessQuicklink(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            Assert.IsNotNull(tabElement);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2 })]
        public void CanOpenClassLogFromQuickLinkAsSeniorManagementTeam()
        {
            var tabElement = HasPermissionsToAccessQuicklink(SeleniumHelper.iSIMSUserType.SeniorManagementTeam);
            Assert.IsNotNull(tabElement);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2 })]
        public void CanOpenClassLogFromQuickLinkAsSENCoordinator()
        {
            var tabElement = HasPermissionsToAccessQuicklink(SeleniumHelper.iSIMSUserType.SENCoordinator);
            Assert.IsNotNull(tabElement);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2 })]
        public void CanOpenClassLogFromQuickLinkAsSchoolAdministrator()
        {
            var tabElement = HasPermissionsToAccessQuicklink(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Assert.IsNotNull(tabElement);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2 })]
        public void CanOpenClassLogFromQuickLinkAsClassTeacher()
        {
            var tabElement = HasPermissionsToAccessQuicklink(SeleniumHelper.iSIMSUserType.ClassTeacher);
            Assert.IsNotNull(tabElement);
        }

        #region Helper Methods

        private static IWebElement HasPermissionsToAccessQuicklink(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType, false, ClassLogFeature);
            AutomationSugar.WaitForAjaxCompletion();
            Wait.WaitForDocumentReady();
            var classLogNavigate = new ClassLogNavigation();
            classLogNavigate.NavigateToPupilClassLogFromQuickLink();
            return SeleniumHelper.GetVisible(By.CssSelector(SeleniumHelper.AutomationId("tab_Class")));
        }

        private static IWebElement HasPermissionsToAccessMenu(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType, false, ClassLogFeature);
            AutomationSugar.WaitForAjaxCompletion();
            Wait.WaitForDocumentReady();
            var classLogNavigate = new ClassLogNavigation();
            classLogNavigate.NavigateToPupilClassLogFromMenu();
            return SeleniumHelper.GetVisible(By.CssSelector(SeleniumHelper.AutomationId("tab_Class")));
        }

        #endregion


    }
}