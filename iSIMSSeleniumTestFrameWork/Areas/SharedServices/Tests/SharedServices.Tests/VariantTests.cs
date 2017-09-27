using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedComponents.Helpers;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SharedServices.Components.Common;
using SharedServices.Components.PageObjects;

namespace SharedServices.Tests
{
    public class VariantTests
    {
        private const string MenuId = "task_menu_section_ss_variant";

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.EnglishStatePrimary)]
        public void DefaultMenu()
        {
            // Arrange
            const string expected = "Standard Screen";
            Login();

            // Act
            var actual = SeleniumHelper.GetMenuText(MenuId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.WelshStatePrimary)]
        public void VariantMenu()
        {
            // Arrange
            const string expected = "Variant Screen";
            Login();

            // Act
            var actual = SeleniumHelper.GetMenuText(MenuId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void DefaultView()
        {
            // Arrange
            const string expected = "variant_tester";
            NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.AutomationId;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.EnglishStatePrimary)]
        public void VariantView()
        {
            // Arrange
            const string expected = "variant_engstpri_tester";
            NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.AutomationId;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void FieldHidden()
        {
            // Arrange
            NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.FieldPresent;

            // Assert
            Assert.IsFalse(actual, "Expected field to be hidden");
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.WelshStatePrimary)]
        public void FieldShown()
        {
            // Arrange
            NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.FieldPresent;

            // Assert
            Assert.IsTrue(actual, "Expected field to be shown");
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.EnglishStatePrimary)]
        public void DefaultLabel()
        {
            // Arrange
            const string expected = "Standard Label";
            NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.Label;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.WelshStatePrimary)]
        public void VariantLabel()
        {
            // Arrange
            const string expected = "Variant Label";
            NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.Label;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void DefaultAction()
        {
            // Arrange
            const string expected = "Default Action Pressed";
            NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            screen.ClickGenerate();
            var actual = screen.ActionData;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.WelshStatePrimary)]
        public void VariantAction()
        {
            // Arrange
            const string expected = "Variant Action Pressed";
            NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            screen.ClickGenerate();
            var actual = screen.ActionData;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void DefaultGrid()
        {
            // Arrange
            const string invariantHeading = "StandardColumn";
            const string variantHeading = "VariantColumn";
            NavigateToTestPage();

            // Act
            var screen = new VariantScreen();

            // Assert
            Assert.IsTrue(screen.GridContains(invariantHeading), "Expected {0} to be present in grid", invariantHeading);
            Assert.IsFalse(screen.GridContains(variantHeading), "Expected {0} to be culled from grid", variantHeading);
        }

        [WebDriverTest(Groups = new[] { "Variants" }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.WelshStatePrimary)]
        public void VariantGrid()
        {
            // Arrange
            const string invariantHeading = "StandardColumn";
            const string variantHeading = "VariantColumn";
            NavigateToTestPage();

            // Act
            var screen = new VariantScreen();

            // Assert
            Assert.IsTrue(screen.GridContains(invariantHeading), "Expected {0} to be present in grid", invariantHeading);
            Assert.IsTrue(screen.GridContains(variantHeading), "Expected {0} to be present in grid", variantHeading);
        }

        private static void Login()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: Constants.SeleniumOnlyFeature);
            AutomationSugar.WaitForAjaxCompletion();
        }

        private static void NavigateToTestPage()
        {
            Login();
            SeleniumHelper.NavigateMenu(MenuId);
        }
    }
}
