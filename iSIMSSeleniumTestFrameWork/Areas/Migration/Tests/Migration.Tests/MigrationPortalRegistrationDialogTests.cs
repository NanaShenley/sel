namespace Migration.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Migration.POM.Helpers;
    using Selene.Support.Attributes;

    [TestClass]
    public class MigrationPortalRegistrationDialogTests
    {
        #region MS Unit Testing support
        public TestContext TestContext { get; set; }
        [TestInitialize]
        public void Init()
        {
            TestRunner.VSSeleniumTest.Init(this, TestContext);
        }
        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.VSSeleniumTest.Cleanup(TestContext);
        }

        #endregion

        [TestMethod]
        [ChromeUiTest(new[] { "MigrationPortalRegistrationDialogTests", "P1", "VerifyAccessToMigrationPortalRegistrationDialog" })]
        public void VerifyAccessToMigrationPortalRegistrationDialog()
        {
            // Arrange
            LoginHelper.Login();
            NavigationHelper.NavigateToMangeUpgrades();

            // Act
            AttributeHelper.WaitForGetByAttribute("a", "data-automation-id", "register_new_site_button", 10000).Click();

            // Assert 
            var searchButton = AttributeHelper.WaitForGetByAttribute("button", "data-automation-id",
                "search_criteria_submit", 5000);
        }
    }
}
