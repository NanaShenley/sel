using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeSugar.Automation;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using SharedServices.Components.PageObjects;
using TestSettings;
using Selene.Support.Attributes;

namespace SharedServices.Tests.Lookups
{
    public class ShowResourceProviderForDuplicateLookupValues
    {
        [WebDriverTest(Groups = new[] { "Story 6009" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DuplicateLookupShowsResourceProvider_Standard()
        {
            //Arrange
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: Constants.SeleniumOnlyFeature);
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Lookup");
            
            // Act
            var screen = new LookupScreen();

            // Assert
            Assert.IsTrue(screen.LookupContains("One (RP1)"));
            Assert.IsTrue(screen.LookupContains("One (RP2)"));
            Assert.IsTrue(screen.LookupContains("Two"));
        }

        [WebDriverTest(Groups = new[] { "Story 6009" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DuplicateLookupShowsResourceProvider_Lazy()
        {
            //Arrange
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: Constants.SeleniumOnlyFeature);
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Lookup");

            // Act
            var screen = new LookupScreen();
            screen.LazyLookup.OpenSelector();

            // Assert
            Assert.IsTrue(screen.LazyLookupContains("One (RP1)"));
            Assert.IsTrue(screen.LazyLookupContains("One (RP2)"));
            Assert.IsTrue(screen.LazyLookupContains("Two"));
        }
    }
}
