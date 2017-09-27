using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedComponents.Helpers;
using TestSettings;
using Selene.Support.Attributes;
using SharedServices.Components.Common;
using SharedServices.Components.PageObjects;
using SharedServices.NonDestructive;

namespace SharedServices.Tests
{
    public class VariantTests
    {

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.EnglishVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.EnglishStatePrimary)]
        public void DefaultMenu_English()
        {
            // Arrange
            const string expected = "Standard Screen";
            NavigationHelper.Login();

            // Act
            var actual = SeleniumHelper.GetMenuText(NavigationHelper.MenuId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.WelshVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.WelshStatePrimary)]
        public void VariantMenu_Welsh()
        {
            // Arrange
            const string expected = "Variant Screen";
            NavigationHelper.Login();

            // Act
            var actual = SeleniumHelper.GetMenuText(NavigationHelper.MenuId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.NIVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void DefaultView_NI()
        {
            // Arrange
            const string expected = "variant_tester";
            NavigationHelper.NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.AutomationId;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.EnglishVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.EnglishStatePrimary)]
        public void VariantView_English()
        {
            // Arrange
            const string expected = "variant_engstpri_tester";
            NavigationHelper.NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.AutomationId;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.NIVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void FieldHidden_NI()
        {
            // Arrange
            NavigationHelper.NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.FieldPresent;

            // Assert
            Assert.IsFalse(actual, "Expected field to be hidden");
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.WelshVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.WelshStatePrimary)]
        public void FieldShown_Welsh()
        {
            // Arrange
            NavigationHelper.NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.FieldPresent;

            // Assert
            Assert.IsTrue(actual, "Expected field to be shown");
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.EnglishVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.EnglishStatePrimary)]
        public void DefaultLabel_English()
        {
            // Arrange
            const string expected = "Standard Label";
            NavigationHelper.NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.Label;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.WelshVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.WelshStatePrimary)]
        public void VariantLabel_Welsh()
        {
            // Arrange
            const string expected = "Variant Label";
            NavigationHelper.NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            var actual = screen.Label;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.NIVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void DefaultAction_NI()
        {
            // Arrange
            const string expected = "Default Action Pressed";
            NavigationHelper.NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            screen.ClickGenerate();
            var actual = screen.ActionData;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.WelshVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.WelshStatePrimary)]
        public void VariantAction_Welsh()
        {
            // Arrange
            const string expected = "Variant Action Pressed";
            NavigationHelper.NavigateToTestPage();

            // Act
            var screen = new VariantScreen();
            screen.ClickGenerate();
            var actual = screen.ActionData;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.NIVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void DefaultGrid_NI()
        {
            // Arrange
            const string invariantHeading = "StandardColumn";
            const string variantHeading = "VariantColumn";
            NavigationHelper.NavigateToTestPage();

            // Act
            var screen = new VariantScreen();

            // Assert
            Assert.IsTrue(screen.GridContains(invariantHeading), "Expected {0} to be present in grid", invariantHeading);
            Assert.IsFalse(screen.GridContains(variantHeading), "Expected {0} to be culled from grid", variantHeading);
        }

        [WebDriverTest(Groups = new[] { TestGroups.Variants, TestGroups.WelshVariant, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        [Variant(Variant.WelshStatePrimary)]
        public void VariantGrid_Welsh()
        {
            // Arrange
            const string invariantHeading = "StandardColumn";
            const string variantHeading = "VariantColumn";
            NavigationHelper.NavigateToTestPage();

            // Act
            var screen = new VariantScreen();

            // Assert
            Assert.IsTrue(screen.GridContains(invariantHeading), "Expected {0} to be present in grid", invariantHeading);
            Assert.IsTrue(screen.GridContains(variantHeading), "Expected {0} to be present in grid", variantHeading);
        }

    }
}
