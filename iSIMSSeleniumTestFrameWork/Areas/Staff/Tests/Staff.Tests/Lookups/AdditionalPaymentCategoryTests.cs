using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System.Linq;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using WebDriverRunner.internals;
using Environment = SeSugar.Environment;
using Selene.Support.Attributes;

namespace Staff.Tests
{
    [TestClass]
    public class AdditionalPaymentCategoryTests
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

        [TestMethod]
        [ChromeUiTest(new[] { "AdditionalPaymentCategory", "P1", "Create" })]
        public void Create_New_Additional_Payment_Category_Lookup_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(4);
            string description = Utilities.GenerateRandomString(6);
            const string displayOrder = "1";
            const string category = "Other";

            //Act
            LoginAndNavigate();
            var apcTriplet = AdditionalPaymentCategoryTriplet.Create();
            var gridRow = apcTriplet.AdditionalPaymentCategories.Rows.Single(x => x.Code == "");

            gridRow.Code = code;
            gridRow.Description = description;
            gridRow.DisplayOrder = displayOrder;
            gridRow.IsVisible = true;
            gridRow.Category = category;

            apcTriplet.ClickSave();
            var details = Search(code);
            gridRow = details.AdditionalPaymentCategories.Rows.First(x => x.Code == code);

            //Assert
            Assert.AreEqual(code, gridRow.Code);
            Assert.AreEqual(description, gridRow.Description);
            Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
            Assert.AreEqual("Other", gridRow.Category);
        }

        [TestMethod]
        [ChromeUiTest(new[] { "AdditionalPaymentCategory", "P1", "Read" })]
        public void Read_Additional_Payment_Category_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("AdditionalPaymentCategory", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("AdditionalPaymentCategory", "Description", 10, tenantID);
            const string displayOrder = "2";

            using (new DataSetup(GetAdditonalPaymentCategory(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.AdditionalPaymentCategories.Rows.FirstOrDefault(x => x.Code == code);

                //Assert
                Assert.AreEqual(code, gridRow.Code);
                Assert.AreEqual(description, gridRow.Description);
                Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
                Assert.AreEqual("Other", gridRow.Category);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "AdditionalPaymentCategory", "P1", "Update" })]
        public void Update_Additional_Payment_Category_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("AdditionalPaymentCategory", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("AdditionalPaymentCategory", "Description", 10, tenantID);
            const string displayOrder = "3";

            string newCode = CoreQueries.GetColumnUniqueString("AdditionalPaymentCategory", "Code", 4, tenantID);
            string newDescription = CoreQueries.GetColumnUniqueString("AdditionalPaymentCategory", "Description", 10, tenantID);
            const string newDisplayOrder = "4";

            using (new DataSetup(GetAdditonalPaymentCategory(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.AdditionalPaymentCategories.Rows.FirstOrDefault(x => x.Code == code && x.Description == description);

                gridRow.Code = newCode;
                gridRow.Description = newDescription;
                gridRow.DisplayOrder = newDisplayOrder;

                details.ClickSave();
                details = Search(newCode);
                gridRow = details.AdditionalPaymentCategories.Rows.FirstOrDefault(x => x.Code == newCode);

                //Assert
                Assert.AreEqual(newCode, gridRow.Code);
                Assert.AreEqual(newDescription, gridRow.Description);
                Assert.AreEqual(newDisplayOrder, gridRow.DisplayOrder);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "AdditionalPaymentCategory", "P1", "Delete" })]
        public void Delete_Additional_Payment_Category_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("AdditionalPaymentCategory", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("AdditionalPaymentCategory", "Description", 10, tenantID);
            const string displayOrder = "4";

            using (new DataSetup(GetAdditonalPaymentCategory(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var additionalCategoryDetails = Search(code);
                var gridRow = additionalCategoryDetails.AdditionalPaymentCategories.Rows.FirstOrDefault(x => x.Code == code);
                gridRow.DeleteRow();
                additionalCategoryDetails.ClickSave();
                additionalCategoryDetails = Search(code);
                gridRow = additionalCategoryDetails.AdditionalPaymentCategories.Rows.FirstOrDefault(x => x.Code == code);

                //Assert
                Assert.AreEqual(null, gridRow);
            }
        }

        #endregion

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Lookups", "Staff", "Additional Payment Category");
        }

        private static AdditionalPaymentCategoryDetailsPage Search(string code)
        {
            AdditionalPaymentCategoryTriplet additionalCategoryTriplet = new AdditionalPaymentCategoryTriplet();
            additionalCategoryTriplet.SearchCriteria.CodeOrDescription = code;
            AdditionalPaymentCategoryDetailsPage additionalCategoryDetails = additionalCategoryTriplet.SearchCriteria.Search<AdditionalPaymentCategoryDetailsPage>();
            return additionalCategoryDetails;
        }

        #endregion

        #region Data Setup

        private DataPackage GetAdditonalPaymentCategory(string code, string description, string displayOrder)
        {
            return this.BuildDataPackage()
                .AddData("AdditionalPaymentCategory", new
                {
                    ID = Guid.NewGuid(),
                    Code = code,
                    Description = description,
                    DisplayOrder = displayOrder,
                    IsVisible = "1",
                    Parent = CoreQueries.GetLookupItem("AdditionalPaymentCategory", tenantID, "OTH"),
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = tenantID
                });
        }

        #endregion
    }
}
