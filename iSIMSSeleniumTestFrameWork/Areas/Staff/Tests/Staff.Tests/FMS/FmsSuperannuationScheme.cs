using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using WebDriverRunner.internals;
using Environment = SeSugar.Environment;
using Selene.Support.Attributes;

namespace Staff.Tests.FMS
{
    [TestClass]
    public class FmsSuperannuationScheme
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

        [TestMethod]
        [ChromeUiTest("FMSSuperannuationScheme", "P1", "MaxLength")]
        public void FMS_SuperannuationSchemeValue_MaxLengthTest()
        {
            //Arrange
            //Act
            LoginAndNavigate("Superannuation Schemes");
            var superannuationSchemeTriplet = new SuperannuationSchemesTriplet();
            superannuationSchemeTriplet.ClickCreate();
            var superannuationScheme = new SuperannuationSchemesPage();
            superannuationScheme.ClickAddSchemeValues();
            var gridRow = superannuationScheme.SchemeValues.Rows[0];
            gridRow.Value = "101.001";
            superannuationScheme.ClickSave();

            //Assert
            Assert.IsTrue(superannuationScheme.Validation.ToList().Contains("Value cannot be more than 100."));
            Assert.IsTrue(superannuationScheme.Validation.ToList().Contains("Superannuation Scheme Detail Value may have only 2 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMSSuperannuationScheme", "P1", "Rounding")]
        public void FMS_SuperannuationSchemeValue_RoundingTest()
        {
            //Arrange
            Guid superannautionSchemeId, superannautionSchemeDetailId;
            string description = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Description", 10, tenantID);
            const string value = "99.99";

            DataPackage testData = new DataPackage();
            testData.AddData("SuperannuationScheme", DataPackageHelper.GenerateSuperannuationScheme(out superannautionSchemeId, description: description));
            testData.AddData("SuperannuationSchemeDetail", DataPackageHelper.GenerateSuperannuationSchemeDetail(out superannautionSchemeDetailId, superannautionSchemeId, DateTime.Today, 0));

            using (new DataSetup(testData))
            {
                //Act
                LoginAndNavigate("Superannuation Schemes");
                var superannuationScheme = Search(description);
                var gridRow = superannuationScheme.SchemeValues.Rows[0];
                gridRow.Value = value;
                superannuationScheme.ClickSave();
                superannuationScheme = Search(description);
                gridRow = superannuationScheme.SchemeValues.Rows[0];

                //Assert
                Assert.AreEqual(value, gridRow.Value);
            }
        }

        [TestMethod]
        [ChromeUiTest("FMSSuperannuationScheme", "P1", "MaxLength")]
        public void FMS_ServiceTerm_SuperannuationScheme_SchemeValues_Value_MaxLengthTest()
        {
            //Arrange
            //Act
            LoginAndNavigate("Service Terms");
            var serviceTermTriplet = new ServiceTermTriplet();
            serviceTermTriplet.ClickCreate();
            var serviceTerm = new ServiceTermPage();
            serviceTerm.AddSuperannuationScheme();
            var superannuationSchemeTriplet = new SupperannuationSchemeTripletDialog();
            superannuationSchemeTriplet.ClickAdd();
            var superannautionSchemeDetails = new SuperannuationSchemesDetailsDialog();
            superannautionSchemeDetails.ClickAddSchemeValues();
            var gridRow = superannautionSchemeDetails.SchemeValues.Rows[0];
            gridRow.Value = "101.001";
            superannuationSchemeTriplet.ClickOk();

            //Assert
            Assert.IsTrue(superannautionSchemeDetails.Validation.ToList().Contains("Value cannot be more than 100."));
            Assert.IsTrue(superannautionSchemeDetails.Validation.ToList().Contains("Superannuation Scheme Detail Value may have only 2 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMSSuperannuationScheme", "P1", "Rounding", "FMS_ServiceTerm_SuperannuationScheme_SchemeValues_Value_RoundingTest")]
        public void FMS_ServiceTerm_SuperannuationScheme_SchemeValues_Value_RoundingTest()
        {
            //Arrange
            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 10, tenantID);
            string superannuationSchemeCode = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Code", 4, tenantID);
            string superannuationSchemeDescription = CoreQueries.GetColumnUniqueString("SuperannuationScheme", "Description", 10, tenantID);
            const string value = "99.99";

            using (new DataSetup(GetServiceTermPackage(serviceTermDescription)))
            {
                //Act
                LoginAndNavigate("Service Terms");
                var serviceTerm = ServiceTermSearch(serviceTermDescription);
                serviceTerm.AddSuperannuationScheme();
                var superannuationSchemeTriplet = new SupperannuationSchemeTripletDialog();
                superannuationSchemeTriplet.ClickAdd();

                var superannautionSchemeDetails = new SuperannuationSchemesDetailsDialog
                {
                    Code = superannuationSchemeCode,
                    Description = superannuationSchemeDescription
                };

                superannautionSchemeDetails.ClickAddSchemeValues();
                var gridRow = superannautionSchemeDetails.SchemeValues.Rows[0];
                gridRow.Value = value;
                gridRow.ApplicationDate = DateTime.Today.ToShortDateString();
                superannuationSchemeTriplet.ClickOk();
                serviceTerm.ClickSave();
                serviceTerm = ServiceTermSearch(serviceTermDescription);
                var superannuationGridRow = serviceTerm.SuperannuationSchemesTable.Rows[0];
                superannuationGridRow.ClickEdit();
                var superannautionSchemeDetail = new SuperannuationSchemesDialog();
                var schemeValueGridRow = superannautionSchemeDetail.SchemeValues.Rows[0];

                //Assert
                Assert.AreEqual(value, schemeValueGridRow.Value);
            }
        }

        private static void LoginAndNavigate(string menu)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", menu);
        }

        private static SuperannuationSchemesPage Search(string description)
        {
            SuperannuationSchemesTriplet superannuationTriplet = new SuperannuationSchemesTriplet();
            superannuationTriplet.SearchCriteria.CodeOrDecription = description;
            var superannuationResult = superannuationTriplet.SearchCriteria.Search().Single();
            var superannuationScheme = superannuationResult.Click<SuperannuationSchemesPage>();

            return superannuationScheme;
        }

        private static ServiceTermPage ServiceTermSearch(string description)
        {
            ServiceTermTriplet serviceTermTriplet = new ServiceTermTriplet();
            serviceTermTriplet.SearchCriteria.DescriptionSearch = description;
            var serviceTermSearch = serviceTermTriplet.SearchCriteria.Search();
            var serviceTermResult = serviceTermSearch.Single();
            var serviceTermPage = serviceTermResult.Click<ServiceTermPage>();

            return serviceTermPage;
        }

        private DataPackage GetServiceTermPackage(string description)
        {
            Guid serviceTermId,
                 paySpineId,
                 payAwardId,
                 statPayScaleId,
                 pscaleId,
                 postTypeId,
                 statutoryPostTypeId,
                 serviceTermsPostTypeId;

            const decimal minimumPoint = 1.0m;
            const decimal maximumPoint = 2.0m;

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, description: description));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, minimumPoint, maximumPoint, 0.5m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, minimumPoint, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, maximumPoint, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("StatutoryPayScale", DataPackageHelper.GenerateStatutoryPayScale(out statPayScaleId));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out pscaleId, serviceTermId, paySpineId, statPayScaleId));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            return testData;
        }
    }
}
