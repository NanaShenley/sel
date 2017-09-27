using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class NationalIdentityTests
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

        //[TestMethod][ChromeUiTest(new[] {"9618","NationalIdentity", "P1", "Create", "WelStPri", "WelStSec", "WelStMult"})]
        public void Create_New_National_Identity_Lookup_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(4);
            string description = Utilities.GenerateRandomString(6);
            const string displayOrder = "1";
            const string category = "Other";

            //Act
            LoginAndNavigate();
            var apcTriplet = NationalIdentityTriplet.Create();
            var gridRow = apcTriplet.NationalIdentities.Rows.Single(x => x.Code == "");

            gridRow.Code = code;
            gridRow.Description = description;
            gridRow.DisplayOrder = displayOrder;
            gridRow.IsVisible = true;
            
            apcTriplet.ClickSave();
            var details = Search(code);
            gridRow = details.NationalIdentities.Rows.First(x => x.Code == code);

            //Assert
            Assert.AreEqual(code, gridRow.Code);
            Assert.AreEqual(description, gridRow.Description);
            Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
        }

        //[TestMethod][ChromeUiTest(new[] {"9618","NationalIdentity", "P1", "Read", "WelStPri", "WelStSec", "WelStMult"})]
        public void Read_National_Identity_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("NationalIdentity", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("NationalIdentity", "Description", 10, tenantID);
            const string displayOrder = "2";

            using (new DataSetup(GetNationalIdentity(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.NationalIdentities.Rows.FirstOrDefault(x => x.Code == code);

                //Assert
                Assert.AreEqual(code, gridRow.Code);
                Assert.AreEqual(description, gridRow.Description);
                Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
            }
        }

        //[TestMethod][ChromeUiTest(new[] {"9618","NationalIdentity", "P1", "Update", "WelStPri", "WelStSec", "WelStMult"})]
        public void Update_National_Identity_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("NationalIdentity", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("NationalIdentity", "Description", 10, tenantID);
            const string displayOrder = "3";

            string newCode = CoreQueries.GetColumnUniqueString("NationalIdentity", "Code", 4, tenantID);
            string newDescription = CoreQueries.GetColumnUniqueString("NationalIdentity", "Description", 10, tenantID);
            const string newDisplayOrder = "4";

            using (new DataSetup(GetNationalIdentity(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.NationalIdentities.Rows.FirstOrDefault(x => x.Code == code && x.Description == description);

                gridRow.Code = newCode;
                gridRow.Description = newDescription;
                gridRow.DisplayOrder = newDisplayOrder;

                details.ClickSave();
                details = Search(newCode);
                gridRow = details.NationalIdentities.Rows.FirstOrDefault(x => x.Code == newCode);

                //Assert
                Assert.AreEqual(newCode, gridRow.Code);
                Assert.AreEqual(newDescription, gridRow.Description);
                Assert.AreEqual(newDisplayOrder, gridRow.DisplayOrder);
            }
        }

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Lookups", "Staff", "National Identity");
        }

        private static NationalIdentityDetailsPage Search(string code)
        {
            NationalIdentityTriplet NationalIdnetityTriplet = new NationalIdentityTriplet();
            NationalIdnetityTriplet.SearchCriteria.CodeOrDescription = code;
            NationalIdentityDetailsPage NationalIdentityDetails =
                NationalIdnetityTriplet.SearchCriteria.Search<NationalIdentityDetailsPage>();
            return NationalIdentityDetails;
        }

        #endregion

        #region Data Setup

        private DataPackage GetNationalIdentity(string code, string description, string displayOrder)
        {
            return this.BuildDataPackage()
                .AddData("NationalIdentity", new
                {
                    ID = Guid.NewGuid(),
                    Code = code,
                    Description = description,
                    DisplayOrder = displayOrder,
                    IsVisible = "1",
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = tenantID
                });
        }

        #endregion
    }
}
