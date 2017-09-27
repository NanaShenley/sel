using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Linq;

namespace Staff.Tests.FMS
{
    [TestClass]
    public class FmsPayScale
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
        //With ref. to:
        //C:\WIP\Dev\iSIMSSeleniumTestFrameWork\Areas\Staff\Tests\Staff.StaffRecord.Tests\ServiceTermTests.cs
        [TestMethod]
        [ChromeUiTest("FMS", "FMSPayScale", "P1")]
        public void FMS_ServiceTerm_PayScale_PaySpineInterval_MaxLengthTest()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();
            staffRecordTriplet.ClickCreate();
            ServiceTermPage serviceTermsPage = new ServiceTermPage();

            PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();
            PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
            PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

            paySpineDetail.InterVal = "8888.8888";
            paySpineTripletDialog.ClickSavePaySpine();

            var validation = paySpineTripletDialog.Validation.ToList();
            Assert.IsTrue(validation.Contains("Interval must be at most 999.0."));
            Assert.IsTrue(validation.Contains("Pay Spine Interval may have only 1 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPayScale", "P1")]
        public void FMS_ServiceTerm_PayScale_PaySpineInterval_RoundingTest()
        {
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string MinimumPoint = "1";
            string MaximumPoint = "2";
            string InterVal = "0.5";

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();
            staffRecordTriplet.ClickCreate();
            ServiceTermPage serviceTermsPage = new ServiceTermPage();

            PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();
            PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
            PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

            paySpineDetail.Code = paySpineCode;
            paySpineDetail.MinimumPoint = MinimumPoint;
            paySpineDetail.MaximumPoint = MaximumPoint;
            paySpineDetail.InterVal = InterVal;
            paySpineDetail.AwardDate = DateTime.Today.AddDays(-10).ToShortDateString();
            paySpineTripletDialog.ClickSavePaySpineStale();
            payScaleDialog.ClickPaySpine();
            var searchCriteria = paySpineTripletDialog.SearchCriteria;
            searchCriteria.SearchByCode = paySpineCode;
            var paySpineTripletDialogSearchResults = searchCriteria.Search();
            var paySpineTripletDialogSearchTitle = paySpineTripletDialogSearchResults.Single(t => t.Code.Equals(paySpineCode));
            var paySpineTripletDialogDetails = paySpineTripletDialogSearchTitle.Click<PaySpineDialogTriplet.PaySpineDetail>();

            Assert.AreEqual(InterVal, paySpineTripletDialogDetails.InterVal, "Payspine Interval rounding failed");
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPayScale", "P1")]
        public void FMS_ServiceTerm_PayScale_PaySpineMaximumPoint_MaxLengthTest()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();
            staffRecordTriplet.ClickCreate();
            ServiceTermPage serviceTermsPage = new ServiceTermPage();

            PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();
            PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
            PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

            paySpineDetail.MaximumPoint = "9999.99";
            paySpineTripletDialog.ClickSavePaySpine();

            var validation = paySpineTripletDialog.Validation.ToList();
            Assert.IsTrue(validation.Contains("Maximum Point must be at most 999.9"));
            Assert.IsTrue(validation.Contains("Pay Spine Maximum Point may have only 1 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPayScale", "P1")]
        public void FMS_ServiceTerm_PayScale_PaySpineMaximumPoint_RoundingTest()
        {
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string MinimumPoint = "1";
            string MaximumPoint = "1.5";
            string InterVal = "0.5";

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();
            staffRecordTriplet.ClickCreate();
            ServiceTermPage serviceTermsPage = new ServiceTermPage();

            PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();
            PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
            PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

            paySpineDetail.Code = paySpineCode;
            paySpineDetail.MinimumPoint = MinimumPoint;
            paySpineDetail.MaximumPoint = MaximumPoint;
            paySpineDetail.InterVal = InterVal;
            paySpineDetail.AwardDate = DateTime.Today.AddDays(-10).ToShortDateString();
            paySpineTripletDialog.ClickSavePaySpineStale();
            payScaleDialog.ClickPaySpine();
            var searchCriteria = paySpineTripletDialog.SearchCriteria;
            searchCriteria.SearchByCode = paySpineCode;
            var paySpineTripletDialogSearchResults = searchCriteria.Search();
            var paySpineTripletDialogSearchTitle = paySpineTripletDialogSearchResults.Single(t => t.Code.Equals(paySpineCode));
            var paySpineTripletDialogDetails = paySpineTripletDialogSearchTitle.Click<PaySpineDialogTriplet.PaySpineDetail>();

            Assert.AreEqual(MaximumPoint, paySpineTripletDialogDetails.MaximumPoint, "Payspine MaximumPoint rounding failed");
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPayScale", "P1")]
        public void FMS_ServiceTerm_PayScale_PaySpineMinimumPoint_MaxLengthTest()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();
            staffRecordTriplet.ClickCreate();
            ServiceTermPage serviceTermsPage = new ServiceTermPage();

            PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();
            PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
            PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

            paySpineDetail.MinimumPoint = "9999.99";
            paySpineTripletDialog.ClickSavePaySpine();

            var validation = paySpineTripletDialog.Validation.ToList();
            Assert.IsTrue(validation.Contains("Minimum Point must be at most 999.9."));
            Assert.IsTrue(validation.Contains("Pay Spine Minimum Point may have only 1 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPayScale", "P1")]
        public void FMS_ServiceTerm_PayScale_PaySpineMinimumPoint_RoundingTest()
        {
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string MinimumPoint = "0.5";
            string MaximumPoint = "2";
            string InterVal = "0.5";

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();
            staffRecordTriplet.ClickCreate();
            ServiceTermPage serviceTermsPage = new ServiceTermPage();

            PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();
            PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
            PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

            paySpineDetail.Code = paySpineCode;
            paySpineDetail.MinimumPoint = MinimumPoint;
            paySpineDetail.MaximumPoint = MaximumPoint;
            paySpineDetail.InterVal = InterVal;
            paySpineDetail.AwardDate = DateTime.Today.AddDays(-10).ToShortDateString();
            paySpineTripletDialog.ClickSavePaySpineStale();
            payScaleDialog.ClickPaySpine();
            var searchCriteria = paySpineTripletDialog.SearchCriteria;
            searchCriteria.SearchByCode = paySpineCode;
            var paySpineTripletDialogSearchResults = searchCriteria.Search();
            var paySpineTripletDialogSearchTitle = paySpineTripletDialogSearchResults.Single(t => t.Code.Equals(paySpineCode));
            var paySpineTripletDialogDetails = paySpineTripletDialogSearchTitle.Click<PaySpineDialogTriplet.PaySpineDetail>();

            Assert.AreEqual(MinimumPoint, paySpineTripletDialogDetails.MinimumPoint, "Payspine MinimumPoint rounding failed");
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPayScale", "P1")]
        public void FMS_ServiceTerm_PayScale_PaySpineScaleAmount_MaxLengthTest()
        {
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();
            staffRecordTriplet.ClickCreate();
            ServiceTermPage serviceTermsPage = new ServiceTermPage();

            PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();
            PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
            PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

            paySpineDetail.Code = paySpineCode;
            paySpineDetail.MinimumPoint = "1";
            paySpineDetail.MaximumPoint = "2";
            paySpineDetail.InterVal = "1";
            paySpineDetail.AwardDate = DateTime.Today.AddDays(-10).ToShortDateString();
            paySpineDetail.ClickAddScaleAwards(2);
            paySpineDetail.ScaleAwards.Rows[0].ScaleAmount = "9999999.99999";
            paySpineDetail.ScaleAwards.Rows[1].ScaleAmount = "9999999.99999";
            paySpineTripletDialog.ClickSavePaySpine();

            var validation = paySpineTripletDialog.Validation.ToList();
            Assert.IsTrue(validation.Contains("Award Amount cannot be more than 999999.9999."));
            Assert.IsTrue(validation.Contains("Pay Award Scale Amount may have only 4 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPayScale", "P1")]
        public void FMS_ServiceTerm_PayScale_PaySpineScaleAmount_RoundingTest()
        {
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string ScaleAmount1 = "2.5000";
            string ScaleAmount2 = "3.5000";

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();
            staffRecordTriplet.ClickCreate();
            ServiceTermPage serviceTermsPage = new ServiceTermPage();

            PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();
            PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
            PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

            paySpineDetail.Code = paySpineCode;
            paySpineDetail.MinimumPoint = "1";
            paySpineDetail.MaximumPoint = "2";
            paySpineDetail.InterVal = "1";
            paySpineDetail.AwardDate = DateTime.Today.AddDays(-10).ToShortDateString();
            paySpineDetail.ClickAddScaleAwards(2);
            paySpineDetail.ScaleAwards.Rows[0].ScaleAmount = ScaleAmount1;
            paySpineDetail.ScaleAwards.Rows[1].ScaleAmount = ScaleAmount2;
            paySpineTripletDialog.ClickSavePaySpineStale();
            payScaleDialog.ClickPaySpine();
            var searchCriteria = paySpineTripletDialog.SearchCriteria;
            searchCriteria.SearchByCode = paySpineCode;
            var paySpineTripletDialogSearchResults = searchCriteria.Search();
            var paySpineTripletDialogSearchTitle = paySpineTripletDialogSearchResults.Single(t => t.Code.Equals(paySpineCode));
            var paySpineTripletDialogDetails = paySpineTripletDialogSearchTitle.Click<PaySpineDialogTriplet.PaySpineDetail>();

            Assert.AreEqual(ScaleAmount1, paySpineTripletDialogDetails.ScaleAwards.Rows[0].ScaleAmount, "First grid row Payspine ScaleAmount rounding failed");
            Assert.AreEqual(ScaleAmount2, paySpineTripletDialogDetails.ScaleAwards.Rows[1].ScaleAmount, "Second grid row Payspine ScaleAmount rounding failed");
        }

        //PaySpine Scale Point has been made readonly, therefore it cannot be changed. And this test becomes invalid.
        //[TestMethod][ChromeUiTest("FMS", "FMSPayScale", "P1")]
        //public void FMS_ServiceTerm_PayScale_PaySpineScalePoint_MaxLengthTest()
        //{
        //    string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);

        //    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
        //    AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
        //    ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();
        //    staffRecordTriplet.ClickCreate();
        //    ServiceTermPage serviceTermsPage = new ServiceTermPage();

        //    PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();
        //    PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
        //    PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

        //    paySpineDetail.Code = paySpineCode;
        //    paySpineDetail.MinimumPoint = "1";
        //    paySpineDetail.MaximumPoint = "2";
        //    paySpineDetail.InterVal = "1";
        //    paySpineDetail.AwardDate = DateTime.Today.AddDays(-10).ToShortDateString();
        //    paySpineDetail.ClickAddScaleAwards(2);
        //    paySpineDetail.ScaleAwards.Rows[0].ScalePoint = "9999.99";
        //    paySpineTripletDialog.ClickSavePaySpine();

        //    Assert.Contains("Award Point cannot be more than 999.5.", paySpineTripletDialog.Validation.ToList());
        //    Assert.Contains("Pay Award Scale Point may have only 1 figure(s) after the decimal point.", paySpineTripletDialog.Validation.ToList());
        //}

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPayScale", "P1", "FMS_ServiceTerm_PayScale_PaySpineScalePoint_RoundingTest")]
        public void FMS_ServiceTerm_PayScale_PaySpineScalePoint_RoundingTest()
        {
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string ScalePoint = "1.5";

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();
            staffRecordTriplet.ClickCreate();
            ServiceTermPage serviceTermsPage = new ServiceTermPage();

            PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();
            PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
            PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

            paySpineDetail.Code = paySpineCode;
            paySpineDetail.MinimumPoint = "1";
            paySpineDetail.MaximumPoint = "2";
            paySpineDetail.InterVal = "0.5";
            paySpineDetail.AwardDate = DateTime.Today.AddDays(-10).ToShortDateString();
            paySpineDetail.ClickAddScaleAwards(3);
            paySpineDetail.ScaleAwards.Rows[0].ScaleAmount = "100";
            paySpineDetail.ScaleAwards.Rows[1].ScaleAmount = "200";
            paySpineDetail.ScaleAwards.Rows[2].ScaleAmount = "300";
            paySpineTripletDialog.ClickSavePaySpineStale();
            payScaleDialog.ClickPaySpine();
            var searchCriteria = paySpineTripletDialog.SearchCriteria;
            searchCriteria.SearchByCode = paySpineCode;
            var paySpineTripletDialogSearchResults = searchCriteria.Search();
            var paySpineTripletDialogSearchTitle = paySpineTripletDialogSearchResults.Single(t => t.Code.Equals(paySpineCode));
            var paySpineTripletDialogDetails = paySpineTripletDialogSearchTitle.Click<PaySpineDialogTriplet.PaySpineDetail>();

            Assert.AreEqual(ScalePoint, paySpineTripletDialogDetails.ScaleAwards.Rows[1].ScalePoint, "Second grid row Payspine ScalePoint rounding failed");
        }
    }
}
