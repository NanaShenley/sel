using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using TestSettings;
using WebDriverRunner.internals;
using Environment = SeSugar.Environment;

namespace Staff.Tests
{
    [TestClass]
    public class RegionalWeightingTests
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
        private readonly string schoolName = TestDefaults.Default.SchoolName;
        private readonly string menuRoute = "Salary Range Regional Weighting";

        #endregion

        #region Tests

        #region Read


        //[TestMethod][ChromeUiTest(new[] { "9611", "RegionalWeighting", "P1", "Read", "PersonnelOfficer", 
        //    "EngStPri", "EngStSec", "EngStMult" })]
        public void Read_existing_Regional_Weighting_Lookup_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            using (new DataSetup(GetRegionalWeighting(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, menuRoute);
                var regionalWeightingDetail = Search(code);
                var gridRow = regionalWeightingDetail.RegionalWeightings.Rows.First(x => x.Code == code);

                //Assert
                Assert.AreEqual(code, gridRow.Code);
                Assert.AreEqual(description, gridRow.Description);
                Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
                Assert.AreEqual(true, gridRow.IsVisible);
            }
        }

        // [TestMethod][ChromeUiTest(new[] { "9611", "RegionalWeighting", "P1", "Read", "SchoolAdministrator", 
        //    "EngStPri", "EngStSec", "EngStMult" })]
        public void Read_existing_Regional_Weighting_Lookup_as_SA()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            using (new DataSetup(GetRegionalWeighting(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, menuRoute);
                var regionalWeightingDetail = Search(code);
                var gridRow = regionalWeightingDetail.RegionalWeightings.Rows.First(x => x.Code == code);

                //Assert
                Assert.AreEqual(code, gridRow.Code);
                Assert.AreEqual(description, gridRow.Description);
                Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
                Assert.AreEqual(true, gridRow.IsVisible);
            }
        }

        #endregion

        #endregion

        #region Data Setup

        private DataPackage GetRegionalWeighting(string code, string description, string displayOrder)
        {
            return this.BuildDataPackage()
                .AddData("RegionalWeighting", new
                {
                    ID = Guid.NewGuid(),
                    Code = code,
                    Description = description,
                    DisplayOrder = displayOrder,
                    IsVisible = true,
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = tenantID
                });
        }

        #endregion

        #region Helpers

        private static RegionalWeightingDetailsPage.RegionalWeightingRow CreateRegionalWeighting(string code, string description, string displayOrder)
        {
            var regionalWeigthingTriplet = new RegionalWeightingTriplet();
            regionalWeigthingTriplet.ClickCreate();
            var regionalWeightingDetail = new RegionalWeightingDetailsPage();
            var gridRow = regionalWeightingDetail.RegionalWeightings.Rows.Single(x => x.Code == "");
            gridRow.Code = code;
            gridRow.Description = description;
            gridRow.DisplayOrder = displayOrder;
            gridRow.IsVisible = true;
            regionalWeightingDetail.ClickSave();
            regionalWeightingDetail = Search(code);
            gridRow = regionalWeightingDetail.RegionalWeightings.Rows.First(x => x.Code == code);

            return gridRow;
        }

        private static RegionalWeightingDetailsPage.RegionalWeightingRow UpdateRegionalWeighting(string code, string newCode, string newDescription, string newDdisplayOrder)
        {
            var regionalWeigthingTriplet = new QTSRouteTriplet();
            var regionalWeightingDetail = Search(code);
            var gridRow = regionalWeightingDetail.RegionalWeightings.Rows.First(x => x.Code == code);
            gridRow.Code = newCode;
            gridRow.Description = newDescription;
            gridRow.DisplayOrder = newDdisplayOrder;
            gridRow.IsVisible = false;
            regionalWeigthingTriplet.ClickSave();
            regionalWeightingDetail = Search(newCode);
            gridRow = regionalWeightingDetail.RegionalWeightings.Rows.First(x => x.Code == newCode);
            return gridRow;
        }

        private static RegionalWeightingDetailsPage.RegionalWeightingRow DeleteRegionalWeighting(string code)
        {
            var regionalWeigthingTriplet = new QTSRouteTriplet();
            var regionalWeightingDetail = Search(code);
            var gridRow = regionalWeightingDetail.RegionalWeightings.Rows.First(x => x.Code == code);
            gridRow.DeleteRow();
            regionalWeigthingTriplet.ClickSave();
            regionalWeightingDetail = Search(code);
            gridRow = regionalWeightingDetail.RegionalWeightings.Rows.FirstOrDefault(x => x.Code == code);
            return gridRow;
        }

        private static void LoginAndNavigate(SeleniumHelper.iSIMSUserType profile, string menuRoute)
        {
            SeleniumHelper.Login(profile);
            AutomationSugar.NavigateMenu("Lookups", "Staff", menuRoute);
        }

        private static RegionalWeightingDetailsPage Search(string code)
        {
            RegionalWeightingTriplet regionalWeigthingTriplet = new RegionalWeightingTriplet();
            regionalWeigthingTriplet.SearchCriteria.CodeOrDescription = code;
            RegionalWeightingDetailsPage regionalWeightingDetail = regionalWeigthingTriplet.SearchCriteria.Search<RegionalWeightingDetailsPage>();
            return regionalWeightingDetail;
        }

        #endregion
    }
}
