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
    public class FmsAllowance
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

        [TestMethod]
        [ChromeUiTest("FMSAllowance", "P1", "MaxLength")]
        public void FMS_AllowanceAmount_MaxLengthTest()
        {
            //Arrange
            //Act
            LoginAndNavigate("Allowances");
            var allowanceTriplet = new AllowanceTriplet();
            allowanceTriplet.ClickCreate();

            var allowance = new AllowanceDialog();
            allowance.ClickAddAward();

            var gridRow = allowance.Awards.Rows[0];
            gridRow.Amount = "9999999.999";
            allowance.ClickOk();

            //Assert
            Assert.IsTrue(allowance.Validation.ToList().Contains("Award Amount cannot be more than 99999.99."));
            Assert.IsTrue(allowance.Validation.ToList().Contains("Allowance Award Amount may have only 2 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMSAllowance", "P1", "Rounding")]
        public void FMS_AllowanceAmount_RoundingTest()
        {
            //Arrange
            Guid Id;
            string description = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);
            DataPackage package = GetAllowancePackage(out Id, description);
            string amount = "25.55";

            using (new DataSetup(package))
            {
                //Act
                LoginAndNavigate("Allowances");
                var allowanceDetail = AllowanceSearch(description);
                var allowanceGridRow = allowanceDetail.Allowances.Rows[0];
                allowanceGridRow.ClickEdit();

                var allowance = new AllowanceDialog();
                var gridRow = allowance.Awards.Rows[0];
                gridRow.Amount = amount;
                allowance.ClickOk();
                allowanceDetail.ClickSave();

                allowanceDetail = AllowanceSearch(description);
                allowanceGridRow = allowanceDetail.Allowances.Rows[0];
                allowanceGridRow.ClickEdit();

                allowance = new AllowanceDialog();
                gridRow = allowance.Awards.Rows[0];

                //Assert
                Assert.AreEqual(amount, gridRow.Amount);
            }
        }

        [TestMethod]
        [ChromeUiTest("FMSAllowance", "P1", "MaxLength")]
        public void FMS_ServiceTerm_Allowance_AllowanceAwards_Amount_MaxLengthTest()
        {
            //Arrange
            //Act
            LoginAndNavigate("Service Terms");
            var serviceTermTriplet = new ServiceTermTriplet();
            serviceTermTriplet.ClickCreate();

            var serviceTerm = new ServiceTermPage();
            serviceTerm.AddAllowance();

            var allowanceDialog = new AllowanceTripletDialog();
            allowanceDialog.ClickCreate();

            var allowance = new AllowanceDetailsDialog();
            allowance.ClickAddAward();

            var gridRow = allowance.Awards.Rows[0];
            gridRow.Amount = "9999999.999";
            allowanceDialog.ClickOk();

            //Assert
            var validation = allowance.Validation.ToList();
            Assert.IsTrue(validation.Contains("Award Amount cannot be more than 99999.99."));
            Assert.IsTrue(validation.Contains("Allowance Award Amount may have only 2 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMSAllowance", "P1", "Rounding", "FMS_ServiceTerm_Allowance_AllowanceAwards_Amount_RoundingTest")]
        public void FMS_ServiceTerm_Allowance_AllowanceAwards_Amount_RoundingTest()
        {
            //Arrange
            Guid addPayCatId;
            string allowanceCode = CoreQueries.GetColumnUniqueString("Allowance", "Code", 4, Environment.Settings.TenantId);
            string allowanceDescription = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);
            string addPayCatDescription = CoreQueries.GetColumnUniqueString("AdditionalPaymentCategory", "Description", 10, Environment.Settings.TenantId);
            string description = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 10, Environment.Settings.TenantId);

            DataPackage package = GetServiceTermPackage(description);
            package.AddData("AdditionalPaymentCategory", DataPackageHelper.GenerateAdditionalPaymentCategory(out addPayCatId, description: addPayCatDescription));

            string amount = "25.55";

            using (new DataSetup(package))
            {
                //Act
                LoginAndNavigate("Service Terms");
                var serviceTerm = ServiceTermSearch(description);
                serviceTerm.AddAllowance();

                var allowanceDialog = new AllowanceTripletDialog();
                allowanceDialog.ClickCreate();

                var allowance = new AllowanceDetailsDialog
                {
                    Code = allowanceCode,
                    Description = allowanceDescription,
                    IsVisible = true,
                    DisplayOrder = "1",
                    Category = addPayCatDescription,
                    FixedAllowance = true
                };
                allowance.ClickAddAward();

                var gridRow = allowance.Awards.Rows[0];
                gridRow.Amount = amount;
                gridRow.AwardDate = DateTime.Today.ToShortDateString();
                allowanceDialog.ClickOk();

                serviceTerm.ClickSave();
                serviceTerm = ServiceTermSearch(description);
                var allowanceGridRow = serviceTerm.AllowanceTable.Rows[0];
                allowanceGridRow.ClickEdit();

                var allowanceDetails = new ServiceTermAllowanceDialog();
                var awardsGridRow = allowanceDetails.Awards.Rows[0];

                //Assert
                Assert.AreEqual(amount, awardsGridRow.Amount);
            }
        }

        #region Helpers

        private static void LoginAndNavigate(string menu)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", menu);
        }

        private static AllowanceDetailsPage AllowanceSearch(string term)
        {
            AllowanceTriplet allowanceTriplet = new AllowanceTriplet();
            allowanceTriplet.SearchCriteria.CodeOrDecription = term;
            var allowance = allowanceTriplet.SearchCriteria.Search<AllowanceDetailsPage>();

            return allowance;
        }

        private static ServiceTermPage ServiceTermSearch(string desc)
        {
            ServiceTermTriplet serviceTermTriplet = new ServiceTermTriplet();
            serviceTermTriplet.SearchCriteria.DescriptionSearch = desc;
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

        private DataPackage GetAllowancePackage(out Guid ID, string description)
        {
            Guid addPayCatId, allowanceId, allowanceAwardId;
            DataPackage testData = new DataPackage();

            testData.AddData("AdditionalPaymentCategory", DataPackageHelper.GenerateAdditionalPaymentCategory(out addPayCatId));
            testData.AddData("Allowance", DataPackageHelper.GenerateFixedAllowance(out allowanceId, addPayCatId, description: description));
            testData.AddData("AllowanceAward", DataPackageHelper.GenerateAllowanceAward(out allowanceAwardId, allowanceId));

            ID = allowanceId;

            return testData;
        }

        #endregion
    }
}
