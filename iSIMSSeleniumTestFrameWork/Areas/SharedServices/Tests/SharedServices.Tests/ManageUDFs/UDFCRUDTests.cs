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
    public class UDFCRUDTests
    {
        private const string udfEntityName = "Selenium:Test";


        [WebDriverTest(Groups = new[] { TestGroups.ManageUDFs, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        public void Newly_Created_UDF_Appears_Under_Search_Results()
        {
            // Arrange
            var manageUDFs = new Components.UDFs.ManageUDFs();
            string newUDFDefinitionDescription = string.Format(UDFData.Prefix, Utilities.GenerateRandomString(15, "New:"));

            try
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Manage UDFs");
                AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage User Defined Fields");

                // Act
                manageUDFs.DetailComponent.AddNew();
                manageUDFs.DetailComponent.Populate(newUDFDefinitionDescription);
                manageUDFs.DetailComponent.Save();

                manageUDFs.SearchComponent.Search(name: newUDFDefinitionDescription);
                SearchResults.WaitForResults();

                // Assert
                Assert.IsTrue(SearchResults.HasResults(1));
                Assert.IsTrue(manageUDFs.HasSearchResultFor(newUDFDefinitionDescription));
            }
            finally
            {
                //Cleanup
                UDFData.DeleteUDFDefinition(newUDFDefinitionDescription);
            }
        }


        [WebDriverTest(Groups = new[] { TestGroups.ManageUDFs, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, TimeoutSeconds = 5000)]
        public void FieldType_Screen_Section_Not_Editable_When_UDF_Field_Has_Value()
        {
            // Arrange
            var manageUDFs = new Components.UDFs.ManageUDFs();
            string visibleUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Visible:");
            string hiddenUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Hidden:");

            using (new DataSetup(false, true, this.BuildDataPackage().AddUDFDefinitionsForTest(visibleUDFDefinitionDescription, hiddenUDFDefinitionDescription, udfEntityName, insertValues: true)))
            {
                // Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Manage UDFs");
                AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage User Defined Fields");

                manageUDFs.SearchComponent.Search(name: string.Format(UDFData.Prefix, visibleUDFDefinitionDescription));
                SearchResults.WaitForResults();

                manageUDFs.SearchComponent.Select();
            }

            // Assert
            Assert.IsTrue(manageUDFs.DetailComponent.DescriptionEnabled);
            Assert.IsFalse(manageUDFs.DetailComponent.FieldTypesEnabled);
            Assert.IsFalse(manageUDFs.DetailComponent.ScreenEnabled);
            Assert.IsFalse(manageUDFs.DetailComponent.SectionEnabled);
            Assert.IsTrue(manageUDFs.DetailComponent.SaveButtonAvailable);
        }

        [WebDriverTest(Groups = new[] { TestGroups.ManageUDFs, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, TimeoutSeconds = 5000)]
        public void UDF_Not_Editable_When_ResourceProvider_Is_Not_Current()
        {
            // Arrange
            var manageUDFs = new Components.UDFs.ManageUDFs();
            string visibleUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Visible:");
            string hiddenUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Hidden:");

            using (new DataSetup(false, true, this.BuildDataPackage().AddUDFDefinitionsForTest(visibleUDFDefinitionDescription, hiddenUDFDefinitionDescription, udfEntityName, insertValues: true, useResourceProvider: false)))
            {
                // Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Manage UDFs");
                AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage User Defined Fields");

                manageUDFs.SearchComponent.Search(name: string.Format(UDFData.Prefix, visibleUDFDefinitionDescription));
                SearchResults.WaitForResults();

                manageUDFs.SearchComponent.Select();
            }

            // Assert
            Assert.IsFalse(manageUDFs.DetailComponent.DescriptionEnabled);
            Assert.IsFalse(manageUDFs.DetailComponent.FieldTypesEnabled);
            Assert.IsFalse(manageUDFs.DetailComponent.ScreenEnabled);
            Assert.IsFalse(manageUDFs.DetailComponent.SectionEnabled);
            Assert.IsFalse(manageUDFs.DetailComponent.SaveButtonAvailable);
        }

        [WebDriverTest(Groups = new[] { TestGroups.ManageUDFs, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, TimeoutSeconds = 5000)]
        public void UDF_Field_Can_Be_Deleted_When_It_Does_Not_Have_Value()
        {
            // Arrange
            var manageUDFs = new Components.UDFs.ManageUDFs();
            string visibleUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Visible:");
            string hiddenUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Hidden:");

            using (new DataSetup(false, true, this.BuildDataPackage().AddUDFDefinitionsForTest(visibleUDFDefinitionDescription, hiddenUDFDefinitionDescription, udfEntityName)))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Manage UDFs");
                AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage User Defined Fields");

                manageUDFs.SearchComponent.Search(string.Format(UDFData.Prefix, visibleUDFDefinitionDescription));
                SearchResults.WaitForResults();

                manageUDFs.SearchComponent.Select();

                // Act
                manageUDFs.DetailComponent.Delete();
            }

            // Assert

            // Verify delete success message appears.
            Assert.IsNotNull(manageUDFs.DetailComponent.GetSuccessMessage());
        }

        [WebDriverTest(Groups = new[] { TestGroups.ManageUDFs, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, TimeoutSeconds = 5000)]
        public void UDF_Field_Can_Be_Deleted_When_It_Has_Value()
        {
            // Arrange
            var manageUDFs = new Components.UDFs.ManageUDFs();
            string visibleUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Visible:");
            string hiddenUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Hidden:");

            using (new DataSetup(false, true, this.BuildDataPackage().AddUDFDefinitionsForTest(visibleUDFDefinitionDescription, hiddenUDFDefinitionDescription, udfEntityName, insertValues: true)))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Manage UDFs");
                AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage User Defined Fields");

                manageUDFs.SearchComponent.Search(string.Format(UDFData.Prefix, visibleUDFDefinitionDescription));
                SearchResults.WaitForResults();

                manageUDFs.SearchComponent.Select();

                // Act
                manageUDFs.DetailComponent.Delete();

                // Assert

                // Verify delete success message appears.
                Assert.IsNotNull(manageUDFs.DetailComponent.GetSuccessMessage());
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.ManageUDFs, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, TimeoutSeconds = 5000)]
        public void UDF_Field_Cannot_Be_Deleted_When_ResourceProvider_Is_Not_Current()
        {
            // Arrange
            var manageUDFs = new Components.UDFs.ManageUDFs();
            string visibleUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Visible:");
            string hiddenUDFDefinitionDescription = Utilities.GenerateRandomString(15, "Hidden:");

            using (new DataSetup(false, true, this.BuildDataPackage().AddUDFDefinitionsForTest(visibleUDFDefinitionDescription, hiddenUDFDefinitionDescription, udfEntityName, insertValues: false, useResourceProvider: false)))
            {
                // Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Manage UDFs");
                AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage User Defined Fields");

                manageUDFs.SearchComponent.Search(name: string.Format(UDFData.Prefix, visibleUDFDefinitionDescription));
                SearchResults.WaitForResults();

                manageUDFs.SearchComponent.Select();
            }

            // Assert
            Assert.IsFalse(manageUDFs.DetailComponent.DeleteButtonAvailable);
        }
    }
}
