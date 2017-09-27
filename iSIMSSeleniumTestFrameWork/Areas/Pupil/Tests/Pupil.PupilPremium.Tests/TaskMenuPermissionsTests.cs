using System;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Helper;
using Pupil.Components;
using Pupil.Components.Common;
using Selene.Support.Attributes;
using SeSugar.Automation;
using TestSettings;

namespace Pupil.PupilPremium.Tests
{
    public class PupilPremiumMenuPermissionsTests
    {
        private const string PupilPremiumFeature = "Pupil Premium";

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP" })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant)]
        public void CannotOpenPupilPremiumFromMenuAsClassTeacher()
        {
            CannotOpenPupilPremiumAsUser(SeleniumHelper.iSIMSUserType.ClassTeacher);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2, "PP" })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant)]
        public void CannotOpenPupilPremiumFromMenuAsSchoolAdministrator()
        {
            CannotOpenPupilPremiumAsUser(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP" })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant)]
        public void CannotOpenPupilPremiumFromMenuAsSenCoordinator()
        {
            CannotOpenPupilPremiumAsUser(SeleniumHelper.iSIMSUserType.SENCoordinator);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP" })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant)]
        public void CannotOpenPupilPremiumFromMenuAsAssessmentCoordinator()
        {
            CannotOpenPupilPremiumAsUser(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP" })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant)]
        public void CanOpenPupilPremiumFromMenuAsSeniorManagementTeam()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam, false, PupilPremiumFeature);
            AutomationSugar.WaitForAjaxCompletion();
            Wait.WaitForDocumentReady();
            var pupilPremiumNavigate = new PupilPremiumNavigation();
            pupilPremiumNavigate.NavigateToPupilPremiumFromMenu();
            Assert.IsNotNull(SeleniumHelper.GetVisible(By.CssSelector(SeleniumHelper.AutomationId("tab_Pupil_Premium_Record"))));
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP" })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant)]
        public void CanOpenPupilPremiumFromMenuAsReturnsManager()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager, false, PupilPremiumFeature);
            AutomationSugar.WaitForAjaxCompletion();
            Wait.WaitForDocumentReady();
            var pupilPremiumNavigate = new PupilPremiumNavigation();
            pupilPremiumNavigate.NavigateToPupilPremiumFromMenu();
            Assert.IsNotNull(SeleniumHelper.GetVisible(By.CssSelector(SeleniumHelper.AutomationId("tab_Pupil_Premium_Record"))));
        }

        private static void CannotOpenPupilPremiumAsUser(SeleniumHelper.iSIMSUserType userType)
        {
            bool isAccessible = SeleniumHelper.HasMenuPermission("task_menu_section_pupil_pupil_premium", userType: userType, enableSelection:false);
            Assert.AreEqual(false, isAccessible);
        }

    }
}