using Migration.POM.Helpers;

namespace Migration.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Selene.Support.Attributes;

    [TestClass]
    public class MigrationPortalMainScreenTests
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
        [ChromeUiTest(new[] { "MigrationPortalMainScreenTests", "P1", "VerifyAccessToMigrationPortalMainScreen" })]
        public void VerifyAccessToMigrationPortalMainScreen()
        {
            // Arrange
            LoginHelper.Login();

            // Act
            NavigationHelper.NavigateToMangeUpgrades();
        }
    }
}
