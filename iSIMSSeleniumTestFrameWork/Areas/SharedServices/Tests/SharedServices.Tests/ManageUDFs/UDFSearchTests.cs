using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using SharedServices.TestData;
using TestSettings;

namespace SharedServices.Tests.ManageUDFs
{
    [TestClass]
    public class UDFSearchTests
    {
        private const string udfEntityName = "Selenium:Test";

        [WebDriverTest(Groups = new[] { TestGroups.ManageUDFs, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, TimeoutSeconds = 5000)]
        public void Default_Search_Shows_Active_UDFs()
        {
            // Arrange
            var manageUDFs = new SharedServices.Components.UDFs.ManageUDFs();
            string visibleUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Visible:");
            string hiddenUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Hidden:");

            using (new DataSetup(false, true, this.BuildDataPackage().AddUDFDefinitionsForTest(visibleUDFDefinitionDescription, hiddenUDFDefinitionDescription, udfEntityName)))
            {
                // Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Manage UDFs");
                AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage User Defined Fields");
                manageUDFs.SearchComponent.Search();
                SearchResults.WaitForResults();
            }

            // Assert
            Assert.IsTrue(SearchResults.HasResults());
            Assert.IsTrue(manageUDFs.HasSearchResultFor(visibleUDFDefinitionDescription));
            Assert.IsFalse(manageUDFs.HasSearchResultFor(hiddenUDFDefinitionDescription));
        }

        [WebDriverTest(Groups = new[] { TestGroups.ManageUDFs, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, TimeoutSeconds = 5000)]
        public void All_UDFs_Available_When_Include_Inactive_Flag_Is_Set()
        {
            // Arrange
            var manageUDFs = new SharedServices.Components.UDFs.ManageUDFs();
            string visibleUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Visible:");
            string hiddenUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Hidden:");

            using (new DataSetup(false, true, this.BuildDataPackage().AddUDFDefinitionsForTest(visibleUDFDefinitionDescription, hiddenUDFDefinitionDescription, udfEntityName)))
            {
                // Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Manage UDFs");
                AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage User Defined Fields");
                manageUDFs.SearchComponent.Search(includeInactive: true);
                SearchResults.WaitForResults();
            }

            // Assert
            Assert.IsTrue(SearchResults.HasResults());
            Assert.IsTrue(manageUDFs.HasSearchResultFor(visibleUDFDefinitionDescription));
            Assert.IsTrue(manageUDFs.HasSearchResultFor(hiddenUDFDefinitionDescription));
        }

        //[WebDriverTest(Groups = new[] { TestGroups.ManageUDFs, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        //public void UDFs_For_Selected_Entity_Available_When_Searched_By_Screen()
        //{
        //    // Arrange
        //    var manageUDFs = new SharedServices.Components.UDFs.ManageUDFs();
        //    string visibleUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Visible:");
        //    string hiddenUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Hidden:");

        //    using (new DataSetup(false, true, this.BuildDataPackage().AddUDFDefinitionsForTest(visibleUDFDefinitionDescription, hiddenUDFDefinitionDescription, udfEntityName)))
        //    {
        //        // Act
        //        SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Manage UDFs");
        //        AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage User Defined Fields");
        //        manageUDFs.SearchComponent.Search(screen: string.Format(UDFData.Prefix, udfEntityName));
        //        SearchResults.WaitForResults();
        //    }

        //    // Assert
        //    Assert.IsTrue(SearchResults.HasResults(1));
        //    Assert.IsTrue(manageUDFs.HasSearchResultFor(visibleUDFDefinitionDescription));
        //}

        [WebDriverTest(Groups = new[] { TestGroups.ManageUDFs, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, TimeoutSeconds = 5000)]
        public void UDFs_For_Selected_Name_Available_When_Searched_By_Name()
        {
            // Arrange
            var manageUDFs = new SharedServices.Components.UDFs.ManageUDFs();
            string visibleUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Visible:");
            string hiddenUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Hidden:");

            using (new DataSetup(false, true, this.BuildDataPackage().AddUDFDefinitionsForTest(visibleUDFDefinitionDescription, hiddenUDFDefinitionDescription, udfEntityName)))
            {
                // Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Manage UDFs");
                AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage User Defined Fields");
                manageUDFs.SearchComponent.Search(name: string.Format(UDFData.Prefix, visibleUDFDefinitionDescription));
                SearchResults.WaitForResults();
            }

            // Assert
            Assert.IsTrue(SearchResults.HasResults(1));
            Assert.IsTrue(manageUDFs.HasSearchResultFor(visibleUDFDefinitionDescription));
        }

    }
}
