using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Linq;
using WebDriverRunner.internals;
using Environment = SeSugar.Environment;

namespace Staff.Tests
{
    [TestClass]
    public class EmploymentContractOriginTests
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

        #region Private Parameters

        private int tenantID { get { return Environment.Settings.TenantId; } }

        #endregion

        #region Tests

        [TestMethod][ChromeUiTest(new[] { "EmploymentContractOriginTests", "P1", "Create_New_EmploymentContractOrigin_Lookup_as_PO" })]
        public void Create_New_EmploymentContractOrigin_Lookup_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(4);
            string description = Utilities.GenerateRandomString(6);
            const string displayOrder = "1";
            const string category = "Not known";

            //Act
            LoginAndNavigate();
            EmploymentContractOriginDetailsPage page = EmploymentContractOriginTriplet.Create();
            EmploymentContractOriginDetailsPage.EmploymentContractOriginRow gridRow = page.LookupGrid.Rows.Single(x => x.Code == "");

            gridRow.Code = code;
            gridRow.Description = description;
            gridRow.DisplayOrder = displayOrder;
            gridRow.IsVisible = true;
            gridRow.Category = category;

            page.ClickSave();
            var details = Search(code);
            gridRow = details.LookupGrid.Rows.First(x => x.Code == code);

            //Assert
            Assert.AreEqual(code, gridRow.Code);
            Assert.AreEqual(description, gridRow.Description);
            Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
            Assert.AreEqual("Not known", gridRow.Category);
        }

        [TestMethod][ChromeUiTest(new[] { "EmploymentContractOriginTests", "P1", "Read_EmploymentContractOrigin_Lookup_as_PO" })]
        public void Read_EmploymentContractOrigin_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("EmploymentContractOrigin", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("EmploymentContractOrigin", "Description", 10, tenantID);
            const string displayOrder = "2";

            using (new DataSetup(GetDataPackage(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.LookupGrid.Rows.FirstOrDefault(x => x.Code == code);

                //Assert
                Assert.AreEqual(code, gridRow.Code);
                Assert.AreEqual(description, gridRow.Description);
                Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
                Assert.AreEqual("Other", gridRow.Category);
            }
        }

        [TestMethod][ChromeUiTest(new[] { "EmploymentContractOriginTests", "P1", "Update_EmploymentContractOrigin_Lookup_as_PO" })]
        public void Update_EmploymentContractOrigin_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("EmploymentContractOrigin", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("EmploymentContractOrigin", "Description", 10, tenantID);
            const string displayOrder = "3";

            string newCode = CoreQueries.GetColumnUniqueString("EmploymentContractOrigin", "Code", 4, tenantID);
            string newDescription = CoreQueries.GetColumnUniqueString("EmploymentContractOrigin", "Description", 10, tenantID);
            const string newDisplayOrder = "4";

            using (new DataSetup(GetDataPackage(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.LookupGrid.Rows.FirstOrDefault(x => x.Code == code && x.Description == description);

                gridRow.Code = newCode;
                gridRow.Description = newDescription;
                gridRow.DisplayOrder = newDisplayOrder;

                details.ClickSave();
                details = Search(newCode);
                gridRow = details.LookupGrid.Rows.FirstOrDefault(x => x.Code == newCode);

                //Assert
                Assert.AreEqual(newCode, gridRow.Code);
                Assert.AreEqual(newDescription, gridRow.Description);
                Assert.AreEqual(newDisplayOrder, gridRow.DisplayOrder);
            }
        }

        [TestMethod][ChromeUiTest(new[] { "EmploymentContractOriginTests", "P1", "Delete_EmploymentContractOrigin_Lookup_as_PO" })]
        public void Delete_EmploymentContractOrigin_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("EmploymentContractOrigin", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("EmploymentContractOrigin", "Description", 10, tenantID);
            const string displayOrder = "4";

            using (new DataSetup(GetDataPackage(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.LookupGrid.Rows.FirstOrDefault(x => x.Code == code);
                gridRow.DeleteRow();
                details.ClickSave();
                details = Search(code);
                gridRow = details.LookupGrid.Rows.FirstOrDefault(x => x.Code == code);

                //Assert
                Assert.AreEqual(null, gridRow);
            }
        }

        #endregion

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Lookups", "Staff", "Contract Origin");
        }

        private static EmploymentContractOriginDetailsPage Search(string code)
        {
            EmploymentContractOriginTriplet triplet = new EmploymentContractOriginTriplet();
            triplet.SearchCriteria.CodeOrDescription = code;
            EmploymentContractOriginDetailsPage details = triplet.SearchCriteria.Search<EmploymentContractOriginDetailsPage>();
            return details;
        }

        #endregion

        #region Data Setup

        private DataPackage GetDataPackage(string code, string description, string displayOrder)
        {
            return this.BuildDataPackage()
                .AddData("EmploymentContractOrigin", new
                {
                    ID = Guid.NewGuid(),
                    Code = code,
                    Description = description,
                    DisplayOrder = displayOrder,
                    IsVisible = "1",
                    Parent = CoreQueries.GetLookupItem("EmploymentContractOrigin", tenantID, "OTHERR"),
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = tenantID
                });
        }

        #endregion
    }
}
