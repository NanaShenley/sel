using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Linq;
using SeSugar.Automation;
using SeSugar.Data;
using WebDriverRunner.internals;
using Environment = SeSugar.Environment;
using Selene.Support.Attributes;

namespace Staff.Tests
{
    [TestClass]
    public class SuperannuationSchemeTests
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
        private readonly DateTime applicationDate = DateTime.Today;
        private const decimal value = 99.99m;

        #endregion

        #region Tests

        [TestMethod]
        [ChromeUiTest(new[] { "SuperannuationScheme", "P1", "Create" })]
        public void Create_a_superannuation_scheme_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Description", 10, tenantID);

            //Act
            LoginAndNavigate();
            var superannuationSchemeTriplet = new SuperannuationSchemesTriplet();
            superannuationSchemeTriplet.ClickCreate();
            var superannuationScheme = new SuperannuationSchemesPage();
            superannuationScheme.Code = code;
            superannuationScheme.Description = description;
            superannuationScheme.ClickAddSchemeValues();
            var gridRow = superannuationScheme.SchemeValues.Rows[0];
            gridRow.ApplicationDate = applicationDate.ToShortDateString();
            gridRow.Value = value.ToString();
            superannuationScheme.ClickSave();
            superannuationScheme = Search(code);
            gridRow = superannuationScheme.SchemeValues.Rows[0];

            //Assert
            Assert.AreEqual(code, superannuationScheme.Code);
            Assert.AreEqual(description, superannuationScheme.Description);
            Assert.AreEqual(applicationDate.ToShortDateString(), gridRow.ApplicationDate);
            Assert.AreEqual(value.ToString(), gridRow.Value);
        }

        [TestMethod]
        [ChromeUiTest(new[] { "SuperannuationScheme", "P1", "Read" })]
        public void Read_Superannuation_scheme_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Description", 10, tenantID);

            using (new DataSetup(GetSuperannuationScheme(code, description)))
            {
                //Act
                LoginAndNavigate();
                var superannuationScheme = Search(code);
                var gridRow = superannuationScheme.SchemeValues.Rows[0];

                //Assert
                Assert.AreEqual(code, superannuationScheme.Code);
                Assert.AreEqual(description, superannuationScheme.Description);
                Assert.AreEqual(applicationDate.ToShortDateString(), gridRow.ApplicationDate);
                Assert.AreEqual(value.ToString(), gridRow.Value);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "SuperannuationScheme", "P1", "Update" })]
        public void Update_Superannuation_scheme_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Description", 10, tenantID);

            string newDescription = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Description", 10, tenantID);
            string newApplicationDate = applicationDate.AddDays(1).ToShortDateString();
            const string newValue = "88.88";

            using (new DataSetup(GetSuperannuationScheme(code, description)))
            {
                //Act
                LoginAndNavigate();
                var superannuationScheme = Search(code);
                superannuationScheme.Description = newDescription;
                var gridRow = superannuationScheme.SchemeValues.Rows[0];
                gridRow.ApplicationDate = newApplicationDate;
                gridRow.Value = newValue;
                superannuationScheme.ClickSave();
                superannuationScheme = Search(code);
                gridRow = superannuationScheme.SchemeValues.Rows[0];

                //Assert
                Assert.AreEqual(newDescription, superannuationScheme.Description);
                Assert.AreEqual(newApplicationDate, gridRow.ApplicationDate);
                Assert.AreEqual(newValue, gridRow.Value);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "SuperannuationScheme", "P1", "Delete" })]
        public void Delete_Superannuation_scheme_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Description", 10, tenantID);

            using (new DataSetup(GetSuperannuationScheme(code, description)))
            {
                //Act
                LoginAndNavigate();
                var superannuationScheme = Search(code);
                superannuationScheme.ClickDelete();
                var superannuationTriplet = new SuperannuationSchemesTriplet();
                superannuationTriplet.SearchCriteria.CodeOrDecription = code;
                var superannuationResult = superannuationTriplet.SearchCriteria.Search().SingleOrDefault(t => t.Code.Equals(code));

                //Assert
                Assert.AreEqual(null, superannuationResult);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "SuperannuationScheme", "P1", "Delete" })]
        public void Delete_Superannuation_scheme_value_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Description", 10, tenantID);

            using (new DataSetup(GetSuperannuationScheme(code, description)))
            {
                //Act
                LoginAndNavigate();
                var superannuationScheme = Search(code);
                var gridRow = superannuationScheme.SchemeValues.Rows[0];
                gridRow.DeleteRow();
                superannuationScheme.ClickSave();
                superannuationScheme = Search(code);
                gridRow = superannuationScheme.SchemeValues.Rows.FirstOrDefault();

                //Assert
                Assert.AreEqual(null, gridRow);
            }
        }

        #endregion

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Superannuation Schemes");
        }

        private static SuperannuationSchemesPage Search(string code)
        {
            SuperannuationSchemesTriplet superannuationTriplet = new SuperannuationSchemesTriplet();
            superannuationTriplet.SearchCriteria.CodeOrDecription = code;
            var superannuationResult = superannuationTriplet.SearchCriteria.Search().Single(t => t.Code.Equals(code));
            var superannuationScheme = superannuationResult.Click<SuperannuationSchemesPage>();

            return superannuationScheme;
        }

        #endregion

        #region Data Setup

        private DataPackage GetSuperannuationScheme(string code, string description)
        {
            Guid ssID = Guid.NewGuid();

            return this.BuildDataPackage()
                .AddData("SuperannuationScheme", new
                {
                    ID = ssID,
                    Code = code,
                    Description = description,
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = tenantID
                })
                .AddData("SuperannuationSchemeDetail", new
                {
                    ID = Guid.NewGuid(),
                    ApplicationDate = applicationDate,
                    Value = value,
                    SuperannuationScheme = ssID,
                    TenantID = tenantID
                });
        }

        #endregion
    }
}
