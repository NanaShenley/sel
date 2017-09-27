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
    public class FmsServiceTerm
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

        #region Tests

        [TestMethod]
        [ChromeUiTest("FMSServiceTerm", "P1", "MaxLength")]
        public void FMS_ServiceTerm_Code_MaxLengthTest()
        {
            //Arrange
            string serviceTermCode = CoreQueries.GetColumnUniqueString("ServiceTerm", "Code", 3, Environment.Settings.TenantId);

            //Act
            LoginAndNavigate("Service Terms");
            var serviceTermTriplet = new ServiceTermTriplet();
            serviceTermTriplet.ClickCreate();
            var serviceTerm = new ServiceTermPage { Code = serviceTermCode };

            //Assert
            Assert.AreEqual(serviceTermCode.Remove(serviceTermCode.Length - 1), serviceTerm.Code);
        }

        [TestMethod]
        [ChromeUiTest("FMSServiceTerm", "P1", "MaxLength")]
        public void FMS_ServiceTerm_HoursPerWeek_MaxLengthTest()
        {
            //Arrange
            //Act
            LoginAndNavigate("Service Terms");
            var serviceTermTriplet = new ServiceTermTriplet();
            serviceTermTriplet.ClickCreate();
            var serviceTerm = new ServiceTermPage { HoursWorkedPerWeek = "999.99999" };
            serviceTerm.ClickSave();

            //Assert
            var validation = serviceTerm.Validation.ToList();
            Assert.IsTrue(validation.Contains("Hours Worked/Week cannot be more than 99.9999."));
            Assert.IsTrue(validation.Contains("Service Term Hours Worked Per Week may have only 4 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMSServiceTerm", "P1", "Rounding")]
        public void FMS_ServiceTerm_HoursPerWeek_RoundingTest()
        {
            //Arrange
            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 10, Environment.Settings.TenantId);
            const string hoursWorked = "999.99999";

            using (new DataSetup(GetServiceTermPackage(serviceTermDescription)))
            {
                //Act
                LoginAndNavigate("Service Terms");
                var serviceTerm = ServiceTermSearch(serviceTermDescription);
                serviceTerm.HoursWorkedPerWeek = hoursWorked;
                serviceTerm.ClickSave();
                serviceTerm = ServiceTermSearch(serviceTermDescription);

                //Assert
                Assert.AreEqual(hoursWorked, serviceTerm.HoursWorkedPerWeek);
            }
        }

        [TestMethod]
        [ChromeUiTest("FMSServiceTerm", "P1", "MaxLength")]
        public void FMS_ServiceTerm_WeeksPerYear_MaxLength()
        {
            //Arrange
            //Act
            LoginAndNavigate("Service Terms");
            var serviceTermTriplet = new ServiceTermTriplet();
            serviceTermTriplet.ClickCreate();
            var serviceTerm = new ServiceTermPage { WeeksWorkedPerYear = "54.999999" };
            serviceTerm.ClickSave();

            //Assert
            var validation = serviceTerm.Validation.ToList();
            Assert.IsTrue(validation.Contains("Weeks Worked/Year cannot be more than 53.0000."));
            Assert.IsTrue(validation.Contains("Service Term Weeks Worked Per Year may have only 5 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMSServiceTerm", "P1", "Rounding")]
        public void FMS_ServiceTerm_WeeksPerYear_RoundingTest()
        {
            //Arrange
            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 10, Environment.Settings.TenantId);
            const string weeksWorked = "52.55555";

            using (new DataSetup(GetServiceTermPackage(serviceTermDescription)))
            {
                //Act
                LoginAndNavigate("Service Terms");
                var serviceTerm = ServiceTermSearch(serviceTermDescription);
                serviceTerm.WeeksWorkedPerYear = weeksWorked;
                serviceTerm.ClickSave();
                serviceTerm = ServiceTermSearch(serviceTermDescription);

                //Assert
                Assert.AreEqual(weeksWorked, serviceTerm.WeeksWorkedPerYear);
            }
        }

        [TestMethod]
        [ChromeUiTest("FMSServiceTerm", "P1", "Calculation")]
        public void FMS_ServiceTerm_PayPattern_Valid_CalculationTest()
        {
            //Arrange
            const string value = "1";

            //Act
            LoginAndNavigate("Service Terms");
            var serviceTermTriplet = new ServiceTermTriplet();
            serviceTermTriplet.ClickCreate();
            var serviceTerm = new ServiceTermPage();
            serviceTerm.SelectPayPatternTab();

            serviceTerm.Salaried = false;
            serviceTerm.PayPatternApr = value;
            serviceTerm.PayPatternMay = value;
            serviceTerm.PayPatternJun = value;
            serviceTerm.PayPatternJul = value;
            serviceTerm.PayPatternAug = value;
            serviceTerm.PayPatternSep = value;
            serviceTerm.PayPatternOct = value;
            serviceTerm.PayPatternNov = value;
            serviceTerm.PayPatternDec = value;
            serviceTerm.PayPatternJan = value;
            serviceTerm.PayPatternFeb = value;
            serviceTerm.PayPatternMar = value;

            //Assert
            Assert.AreEqual("12", serviceTerm.TotalWeeks);
        }

        [TestMethod]
        [ChromeUiTest("FMSServiceTerm", "P1", "Enabled")]
        public void FMS_ServiceTerm_PayPatterns_Enabled_When_SalariedUnCheckedTest()
        {
            //Arrange
            //Act
            LoginAndNavigate("Service Terms");
            var serviceTermTriplet = new ServiceTermTriplet();
            serviceTermTriplet.ClickCreate();
            var serviceTerm = new ServiceTermPage();
            serviceTerm.SelectPayPatternTab();
            serviceTerm.Salaried = false;

            //Assert
            Assert.IsTrue(serviceTerm.AprEnabled());
            Assert.IsTrue(serviceTerm.MayEnabled());
            Assert.IsTrue(serviceTerm.JunEnabled());
            Assert.IsTrue(serviceTerm.JulEnabled());
            Assert.IsTrue(serviceTerm.AugEnabled());
            Assert.IsTrue(serviceTerm.SepEnabled());
            Assert.IsTrue(serviceTerm.OctEnabled());
            Assert.IsTrue(serviceTerm.NovEnabled());
            Assert.IsTrue(serviceTerm.DecEnabled());
            Assert.IsTrue(serviceTerm.JanEnabled());
            Assert.IsTrue(serviceTerm.FebEnabled());
            Assert.IsTrue(serviceTerm.MarEnabled());
        }

        [TestMethod]
        [ChromeUiTest("FMSServiceTerm", "P1", "Clear")]
        public void FMS_ServiceTerm_PayPatterns_TotalWeeks_Reset_When_SalariedCheckedTest()
        {
            //Arrange
            const string value = "1";

            //Act
            LoginAndNavigate("Service Terms");
            var serviceTermTriplet = new ServiceTermTriplet();
            serviceTermTriplet.ClickCreate();
            var serviceTerm = new ServiceTermPage();
            serviceTerm.SelectPayPatternTab();

            serviceTerm.Salaried = false;
            serviceTerm.PayPatternApr = value;
            serviceTerm.PayPatternMay = value;
            serviceTerm.PayPatternJun = value;
            serviceTerm.PayPatternJul = value;
            serviceTerm.PayPatternAug = value;
            serviceTerm.PayPatternSep = value;
            serviceTerm.PayPatternOct = value;
            serviceTerm.PayPatternNov = value;
            serviceTerm.PayPatternDec = value;
            serviceTerm.PayPatternJan = value;
            serviceTerm.PayPatternFeb = value;
            serviceTerm.PayPatternMar = value;

            serviceTerm.Salaried = true;

            //Assert
            Assert.AreEqual("0", serviceTerm.TotalWeeks);
        }

        #endregion

        #region Helpers

        private static void LoginAndNavigate(string menu)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", menu);
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

        #endregion
    }
}
