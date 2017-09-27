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
using Selene.Support.Attributes;

namespace Staff.Tests
{
    [TestClass]
    public class QTSRouteTests
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
        private readonly string menuRoute = "Qualified Teacher Route";

        #endregion

        #region Tests

        #region Create

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QTSRoute", "P1", "Create", "PersonnelOfficer",
            "NIStPri", "NIStSec", "NIStMult",
            "WelStPri", "WelStSec", "WelStMult",
            "IndPri", "IndSec", "IndMult" })]
        public void Create_New_Qualified_Teacher_Route_Lookup_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            //Act
            LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, menuRoute);
            var gridRow = CreateQTSRoute(code, description, displayOrder);

            //Assert
            Assert.AreEqual(code, gridRow.Code);
            Assert.AreEqual(description, gridRow.Description);
            Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
            Assert.AreEqual(true, gridRow.IsVisible);
            Assert.AreEqual(schoolName, gridRow.ResourceProvider);
        }

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QTSRoute", "P1", "Create", "SchoolAdministrator",
            "NIStPri", "NIStSec", "NIStMult",
            "WelStPri", "WelStSec", "WelStMult",
            "IndPri", "IndSec", "IndMult" })]
        public void Create_New_Qualified_Teacher_Route_Lookup_as_SA()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            //Act
            LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, menuRoute);
            var gridRow = CreateQTSRoute(code, description, displayOrder);

            //Assert
            Assert.AreEqual(code, gridRow.Code);
            Assert.AreEqual(description, gridRow.Description);
            Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
            Assert.AreEqual(true, gridRow.IsVisible);
            Assert.AreEqual(schoolName, gridRow.ResourceProvider);
        }

        #endregion

        #region Read

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QTSRoute", "P1", "Read", "PersonnelOfficer",
            "NIStPri", "NIStSec", "NIStMult",
            "WelStPri", "WelStSec", "WelStMult",
            "IndPri", "IndSec", "IndMult" })]
        public void Read_existing_Qualified_Teacher_Route_Lookup_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            using (new DataSetup(GetQTSRoute(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, menuRoute);
                var qtsRouteDetail = Search(code);
                var gridRow = qtsRouteDetail.QTSRoutes.Rows.First(x => x.Code == code);

                //Assert
                Assert.AreEqual(code, gridRow.Code);
                Assert.AreEqual(description, gridRow.Description);
                Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
                Assert.AreEqual(true, gridRow.IsVisible);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QTSRoute", "P1", "Read", "SchoolAdministrator",
            "NIStPri", "NIStSec", "NIStMult",
            "WelStPri", "WelStSec", "WelStMult",
            "IndPri", "IndSec", "IndMult" })]
        public void Read_existing_Qualified_Teacher_Route_Lookup_as_SA()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            using (new DataSetup(GetQTSRoute(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, menuRoute);
                var qtsRouteDetail = Search(code);
                var gridRow = qtsRouteDetail.QTSRoutes.Rows.First(x => x.Code == code);

                //Assert
                Assert.AreEqual(code, gridRow.Code);
                Assert.AreEqual(description, gridRow.Description);
                Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
                Assert.AreEqual(true, gridRow.IsVisible);
            }
        }

        #endregion

        #region Update

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QTSRoute", "P1", "Update", "PersonnelOfficer",
            "NIStPri", "NIStSec", "NIStMult",
            "WelStPri", "WelStSec", "WelStMult",
            "IndPri", "IndSec", "IndMult" })]
        public void Update_existing_Qualified_Teacher_Route_Lookup_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            string newCode = Utilities.GenerateRandomString(5);
            string newDescription = Utilities.GenerateRandomString(10);
            const string newDisplayOrder = "2";

            using (new DataSetup(GetQTSRoute(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, menuRoute);
                var gridRow = UpdateQTSRoute(code, newCode, newDescription, newDisplayOrder);

                //Assert
                Assert.AreEqual(newCode, gridRow.Code);
                Assert.AreEqual(newDescription, gridRow.Description);
                Assert.AreEqual(newDisplayOrder, gridRow.DisplayOrder);
                Assert.AreEqual(false, gridRow.IsVisible);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QTSRoute", "P1", "Update", "SchoolAdministrator",
            "NIStPri", "NIStSec", "NIStMult",
            "WelStPri", "WelStSec", "WelStMult",
            "IndPri", "IndSec", "IndMult" })]
        public void Update_existing_Qualified_Teacher_Route_Lookup_as_SA()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            string newCode = Utilities.GenerateRandomString(5);
            string newDescription = Utilities.GenerateRandomString(10);
            const string newDisplayOrder = "2";

            using (new DataSetup(GetQTSRoute(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, menuRoute);
                var gridRow = UpdateQTSRoute(code, newCode, newDescription, newDisplayOrder);

                //Assert
                Assert.AreEqual(newCode, gridRow.Code);
                Assert.AreEqual(newDescription, gridRow.Description);
                Assert.AreEqual(newDisplayOrder, gridRow.DisplayOrder);
                Assert.AreEqual(false, gridRow.IsVisible);
            }
        }

        #endregion

        #region Delete

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QTSRoute", "P1", "Delete", "PersonnelOfficer",
            "NIStPri", "NIStSec", "NIStMult",
            "WelStPri", "WelStSec", "WelStMult",
            "IndPri", "IndSec", "IndMult" })]
        public void Delete_existing_Qualified_Teacher_Route_Lookup_as_PO_NI_Pri()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            using (new DataSetup(GetQTSRoute(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, menuRoute);
                var gridRow = DeleteQTSRoute(code);

                //Assert
                Assert.AreEqual(null, gridRow);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QTSRoute", "P1", "Delete", "SchoolAdministrator",
            "NIStPri", "NIStSec", "NIStMult",
            "WelStPri", "WelStSec", "WelStMult",
            "IndPri", "IndSec", "IndMult" })]
        public void Delete_existing_Qualified_Teacher_Route_Lookup_as_SA_NI_Pri()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            using (new DataSetup(GetQTSRoute(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, menuRoute);
                var gridRow = DeleteQTSRoute(code);

                //Assert
                Assert.AreEqual(null, gridRow);
            }
        }

        #endregion

        #endregion

        #region Data Setup

        private DataPackage GetQTSRoute(string code, string description, string displayOrder)
        {
            return this.BuildDataPackage()
                .AddData("QTSRoute", new
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

        private static QTSRouteDetailsPage.QTSRouteRow CreateQTSRoute(string code, string description, string displayOrder)
        {
            var qtsRouteTriplet = new QTSRouteTriplet();
            qtsRouteTriplet.ClickCreate();
            var qtsRouteDetail = new QTSRouteDetailsPage();
            var gridRow = qtsRouteDetail.QTSRoutes.Rows.Single(x => x.Code == "");
            gridRow.Code = code;
            gridRow.Description = description;
            gridRow.DisplayOrder = displayOrder;
            gridRow.IsVisible = true;
            qtsRouteDetail.ClickSave();
            qtsRouteDetail = Search(code);
            gridRow = qtsRouteDetail.QTSRoutes.Rows.First(x => x.Code == code);

            return gridRow;
        }

        private static QTSRouteDetailsPage.QTSRouteRow UpdateQTSRoute(string code, string newCode, string newDescription, string newDdisplayOrder)
        {
            var qtsRouteTriplet = new QTSRouteTriplet();
            var qtsRouteDetail = Search(code);
            var gridRow = qtsRouteDetail.QTSRoutes.Rows.First(x => x.Code == code);
            gridRow.Code = newCode;
            gridRow.Description = newDescription;
            gridRow.DisplayOrder = newDdisplayOrder;
            gridRow.IsVisible = false;
            qtsRouteTriplet.ClickSave();
            qtsRouteDetail = Search(newCode);
            gridRow = qtsRouteDetail.QTSRoutes.Rows.First(x => x.Code == newCode);
            return gridRow;
        }

        private static QTSRouteDetailsPage.QTSRouteRow DeleteQTSRoute(string code)
        {
            var qtsRouteTriplet = new QTSRouteTriplet();
            var qtsRouteDetail = Search(code);
            var gridRow = qtsRouteDetail.QTSRoutes.Rows.First(x => x.Code == code);
            gridRow.DeleteRow();
            qtsRouteTriplet.ClickSave();
            qtsRouteDetail = Search(code);
            gridRow = qtsRouteDetail.QTSRoutes.Rows.FirstOrDefault(x => x.Code == code);
            return gridRow;
        }

        private static void LoginAndNavigate(SeleniumHelper.iSIMSUserType profile, string menuRoute)
        {
            SeleniumHelper.Login(profile);
            AutomationSugar.NavigateMenu("Lookups", "Staff", menuRoute);
        }

        private static QTSRouteDetailsPage Search(string code)
        {
            QTSRouteTriplet qtsRouteTriplet = new QTSRouteTriplet();
            qtsRouteTriplet.SearchCriteria.CodeOrDescription = code;
            QTSRouteDetailsPage qtsRouteDetail = qtsRouteTriplet.SearchCriteria.Search<QTSRouteDetailsPage>();
            return qtsRouteDetail;
        }

        #endregion
    }
}
