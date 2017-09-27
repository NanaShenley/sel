using System;
using System.CodeDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staff.POM.Components.Staff;
using Staff.POM.Components.Staff.Pages;
using Staff.POM.Helper;
using System.Linq;
using System.Runtime.InteropServices;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using WebDriverRunner.internals;
using Environment = SeSugar.Environment;

namespace Staff.Tests
{
    [TestClass]
    public class StaffReligionCategoryTests
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
        //[TestMethod][ChromeUiTest(new[] {"9618","StaffReligionCategory", "P1", "Create", "EngStPri", "EngStSec", "EngStMult", "WelStPri", "WelStSec", "WelStMult",
        // "IndPri", "IndSec", "IndMult"})]
        public void Create_New_Staff_Religion_Category_Lookup_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(4);
            string description = Utilities.GenerateRandomString(6);
            const string displayOrder = "1";
            const string category = "Other religion";

            //Act
            LoginAndNavigate();
            var staffReligionTriplet = StaffReligionCategoryTriplet.Create();
            var gridRow = staffReligionTriplet.StaffReligionCategories.Rows.Single(x => x.Code == "");

            gridRow.Code = code;
            gridRow.Description = description;
            gridRow.DisplayOrder = displayOrder;
            gridRow.IsVisible = true;
            gridRow.Category = category;

            staffReligionTriplet.ClickSave();
            var details = Search(code);
            gridRow = details.StaffReligionCategories.Rows.First(x => x.Code == code);

            //Assert
            Assert.AreEqual(code, gridRow.Code);
            Assert.AreEqual(description, gridRow.Description);
            Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
            Assert.AreEqual("Other religion", gridRow.Category);
        }

        //[TestMethod][ChromeUiTest(new[] {"9618","StaffReligionCategory", "P1", "Read", "EngStPri", "EngStSec", "EngStMult", "WelStPri", "WelStSec", "WelStMult",
        // "IndPri", "IndSec", "IndMult"})]
        public void Read_Staff_Religion_Category_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("StaffReligion", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("StaffReligion", "Description", 10, tenantID);
            const string displayOrder = "2";

            using (new DataSetup(GetStaffReligionCategory(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.StaffReligionCategories.Rows.FirstOrDefault(x => x.Code == code);

                //Assert
                Assert.AreEqual(code, gridRow.Code);
                Assert.AreEqual(description, gridRow.Description);
                Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
                Assert.AreEqual("Other religion", gridRow.Category);
            }
        }

        //[TestMethod][ChromeUiTest(new[] {"9618","StaffReligionCategory", "P1", "Update", "EngStPri", "EngStSec", "EngStMult", "WelStPri", "WelStSec", "WelStMult",
        // "IndPri", "IndSec", "IndMult"})]
        public void Update_Staff_Religion_Category_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("StaffReligion", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("StaffReligion", "Description", 10, tenantID);
            const string displayOrder = "3";

            string newCode = CoreQueries.GetColumnUniqueString("StaffReligion", "Code", 4, tenantID);
            string newDescription = CoreQueries.GetColumnUniqueString("StaffReligion", "Description", 10, tenantID);
            const string newDisplayOrder = "4";

            using (new DataSetup(GetStaffReligionCategory(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.StaffReligionCategories.Rows.FirstOrDefault(x => x.Code == code && x.Description == description);

                gridRow.Code = newCode;
                gridRow.Description = newDescription;
                gridRow.DisplayOrder = newDisplayOrder;

                details.ClickSave();
                details = Search(newCode);
                gridRow = details.StaffReligionCategories.Rows.FirstOrDefault(x => x.Code == newCode);

                //Assert
                Assert.AreEqual(newCode, gridRow.Code);
                Assert.AreEqual(newDescription, gridRow.Description);
                Assert.AreEqual(newDisplayOrder, gridRow.DisplayOrder);
            }
        }

        //[TestMethod][ChromeUiTest(new[] {"9618","StaffReligionCategory", "P1", "Delete", "EngStPri", "EngStSec", "EngStMult", "WelStPri", "WelStSec", "WelStMult",
        // "IndPri", "IndSec", "IndMult"})]
        public void Delete_Staff_Religion_Category_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("StaffReligion", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("StaffReligion", "Description", 10, tenantID);
            const string displayOrder = "4";

            using (new DataSetup(GetStaffReligionCategory(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var additionalCategoryDetails = Search(code);
                var gridRow = additionalCategoryDetails.StaffReligionCategories.Rows.FirstOrDefault(x => x.Code == code);
                gridRow.DeleteRow();
                additionalCategoryDetails.ClickSave();
                additionalCategoryDetails = Search(code);
                gridRow = additionalCategoryDetails.StaffReligionCategories.Rows.FirstOrDefault(x => x.Code == code);

                //Assert
                Assert.AreEqual(null, gridRow);
            }
        }

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Lookups", "Staff", "Staff Religion");
        }

        private static StaffReligionCategoryDetailsPage Search(string code)
        {
            StaffReligionCategoryTriplet staffReligionCategoryTriplet = new StaffReligionCategoryTriplet();
            staffReligionCategoryTriplet.SearchCriteria.CodeOrDescription = code;
            StaffReligionCategoryDetailsPage staffReligionCategoryDetails =
                staffReligionCategoryTriplet.SearchCriteria.Search<StaffReligionCategoryDetailsPage>();
            return staffReligionCategoryDetails;
        }

        #endregion

        #region Data Setup

        private DataPackage GetStaffReligionCategory(string code, string description, string displayOrder)
        {
            return this.BuildDataPackage()
                .AddData("StaffReligion", new
                {
                    ID = Guid.NewGuid(),
                    Code = code,
                    Description = description,
                    DisplayOrder = displayOrder,
                    IsVisible = "1",
                    Parent = CoreQueries.GetLookupItem("StaffReligion", tenantID, "OT"),
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = tenantID
                });
        }

        #endregion
    }
}
