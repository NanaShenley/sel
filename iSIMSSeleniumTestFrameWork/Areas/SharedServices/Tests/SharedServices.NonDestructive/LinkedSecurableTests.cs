using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedComponents.Helpers;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SharedServices.Components.Common;
using SharedServices.Components.PageObjects;

namespace SharedServices.NonDestructive
{
    public class LinkedSecurableTests
    {
        [WebDriverTest( Groups = new[] { TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void JobStep_With_Valid_Securable_Not_Visible_For_Class_Teacher()
        {
            // Arrange
            NavigationHelper.NavigateToTestPage(SeleniumHelper.iSIMSUserType.ClassTeacher);

            // Act
            var screen = new VariantScreen();

            // Assert
            Assert.AreEqual(false, screen.JobStepTestLinkedSecurableExists);
        }

        [WebDriverTest( Groups = new[] { TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void TestJobStep_With_Valid_Securable_Visible_For_System_Manager()
        {
            // Arrange
            NavigationHelper.NavigateToTestPage(SeleniumHelper.iSIMSUserType.SystemManger);

            // Act
            var screen = new VariantScreen();

            // Assert
            Assert.AreEqual(true, screen.JobStepTestLinkedSecurableExists);
        }

        [WebDriverTest( Groups = new[] { TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void JobStep_With_Invalid_Securable_Not_Visible_For_Class_Teacher()
        {
            // Arrange
            NavigationHelper.NavigateToTestPage(SeleniumHelper.iSIMSUserType.ClassTeacher);

            // Act
            var screen = new VariantScreen();

            // Assert
            Assert.AreEqual(false, screen.JobStepTestLinkedSecurableForInvalidSecurityDomainExists);
        }

        [WebDriverTest( Groups = new[] { TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void TestJobStep_With_Invalid_Securable_Not_Visible_For_System_Manager()
        {
            // Arrange
            NavigationHelper.NavigateToTestPage(SeleniumHelper.iSIMSUserType.SystemManger);

            // Act
            var screen = new VariantScreen();

            // Assert
            Assert.AreEqual(false, screen.JobStepTestLinkedSecurableForInvalidSecurityDomainExists);
        }
    }
}
