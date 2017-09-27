using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner.internals;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Selene.Support.Attributes;

namespace Staff.Tests.FMS
{
    [TestClass]
    public class FmsPaySpine
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
        //C:\WIP\Dev\iSIMSSeleniumTestFrameWork\Areas\Staff\Tests\Staff.StaffRecord.Tests\PaySpineTests.cs
        [TestMethod]
        [ChromeUiTest("FMS", "FMSPaySpine", "P1")]
        public void FMS_PaySpineInterval_MaxLengthTest()
        {
            int tenantID = SeSugar.Environment.Settings.TenantId;
            string code = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, tenantID);
            string InterVal = "8888.8888";

            LoginAndNavigate();
            var paySpineTriplet = new PaySpineTriplet();
            paySpineTriplet.ClickAdd();

            var paySpine = new PaySpinePage
            {
                Code = code,
                InterVal = InterVal
            };

            paySpineTriplet.ClickSave();
            var validation = paySpineTriplet.Validation.ToList();
            Assert.IsTrue(validation.Contains("Interval must be at most 999.0."));
            Assert.IsTrue(validation.Contains("Pay Spine Interval may have only 1 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPaySpine", "P1")]
        public void FMS_PaySpineMaximumPoint_MaxLengthTest()
        {
            int tenantID = SeSugar.Environment.Settings.TenantId;
            string code = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, tenantID);
            string maximumPoint = "9999.99";

            LoginAndNavigate();
            var paySpineTriplet = new PaySpineTriplet();
            paySpineTriplet.ClickAdd();

            var paySpine = new PaySpinePage
            {
                Code = code,
                MaximumPoint = maximumPoint
            };

            paySpineTriplet.ClickSave();
            var validation = paySpineTriplet.Validation.ToList();
            Assert.IsTrue(validation.Contains("Maximum Point must be at most 999.9"));
            Assert.IsTrue(validation.Contains("Pay Spine Maximum Point may have only 1 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPaySpine", "P1")]
        public void FMS_PaySpineMinimumPoint_MaxLengthTest()
        {
            int tenantID = SeSugar.Environment.Settings.TenantId;
            string code = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, tenantID);
            string minimumPoint = "9999.99";

            LoginAndNavigate();
            var paySpineTriplet = new PaySpineTriplet();
            paySpineTriplet.ClickAdd();

            var paySpine = new PaySpinePage
            {
                Code = code,
                MinimumPoint = minimumPoint
            };

            paySpineTriplet.ClickSave();
            var validation = paySpineTriplet.Validation.ToList();
            Assert.IsTrue(validation.Contains("Minimum Point must be at most 999.9."));
            Assert.IsTrue(validation.Contains("Pay Spine Minimum Point may have only 1 figure(s) after the decimal point."));
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FMSPaySpine", "P1")]
        public void FMS_PaySpineScaleAmount_MaxLengthTest()
        {
            int tenantID = SeSugar.Environment.Settings.TenantId;
            string code = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, tenantID);
            string minimumPoint = "1.0";
            string maximumPoint = "2.0";
            string interval = "1.0";
            DateTime awardDate = DateTime.Today;
            string scaleAmount = "9999999.99999";

            LoginAndNavigate();
            var paySpineTriplet = new PaySpineTriplet();
            paySpineTriplet.ClickAdd();

            var paySpine = new PaySpinePage
            {
                Code = code,
                MinimumPoint = minimumPoint,
                MaximumPoint = maximumPoint,
                InterVal = interval,
                AwardDate = awardDate.ToShortDateString()
            };

            paySpine.ClickAddScaleAwards();
            var gridRow = paySpine.ScaleAwards.Rows[0];
            gridRow.ScaleAmount = scaleAmount;
            paySpineTriplet.ClickSave();
            var validation = paySpineTriplet.Validation.ToList();
            Assert.IsTrue(validation.Contains("Award Amount cannot be more than 999999.9999."));
            Assert.IsTrue(validation.Contains("Pay Award Scale Amount may have only 4 figure(s) after the decimal point."));
        }

        //PaySpine Scale Point has been made readonly, therefore it cannot be changed. And this test becomes invalid.
        //[TestMethod][ChromeUiTest("FMS", "FMSPaySpine", "P1")]
        //public void FMS_PaySpineScalePoint_MaxLengthTest()
        //{
        //    int tenantID = SeSugar.Environment.Settings.TenantId;
        //    string code = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, tenantID);
        //    string minimumPoint = "1.0";
        //    string maximumPoint = "2.0";
        //    string interval = "1.0";
        //    DateTime awardDate = DateTime.Today;
        //    string scalePoint = "9999.99";

        //    LoginAndNavigate();
        //    var paySpineTriplet = new PaySpineTriplet();
        //    paySpineTriplet.ClickAdd();

        //    var paySpine = new PaySpinePage
        //    {
        //        Code = code,
        //        MinimumPoint = minimumPoint,
        //        MaximumPoint = maximumPoint,
        //        InterVal = interval,
        //        AwardDate = awardDate.ToShortDateString()
        //    };

        //    paySpine.ClickAddScaleAwards();
        //    var gridRow = paySpine.ScaleAwards.Rows[0];
        //    gridRow.ScalePoint = scalePoint;
        //    paySpineTriplet.ClickSave();
        //    Assert.Contains("Award Point cannot be more than 999.5.", paySpineTriplet.Validation.ToList());
        //    Assert.Contains("Pay Award Scale Point may have only 1 figure(s) after the decimal point.", paySpineTriplet.Validation.ToList());
        //}

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Pay Spines");
        }

        #endregion
    }
}
