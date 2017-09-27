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
    public class PaySpineTests
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
        private const decimal minimumPoint = 1.0m;
        private const decimal maximumPoint = 2.0m;
        private const decimal interval = 1.0m;
        private DateTime awardDate = DateTime.Today;
        private const decimal scaleAmount1 = 100.0000m;
        private const decimal scaleAmount2 = 200.0000m;

        #endregion

        #region Tests

        [TestMethod]
        [ChromeUiTest("PaySpine", "P1", "Create")]
        public void Create_new_Pay_Spine_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, tenantID);

            //Act
            LoginAndNavigate();
            var paySpineTriplet = new PaySpineTriplet();
            paySpineTriplet.ClickAdd();

            var paySpine = new PaySpinePage
            {
                Code = code,
                MinimumPoint = minimumPoint.ToString(),
                MaximumPoint = maximumPoint.ToString(),
                InterVal = interval.ToString(),
                AwardDate = awardDate.ToShortDateString()
            };

            paySpine.ClickAddScaleAwards();
            var gridRow1 = paySpine.ScaleAwards.Rows[0];
            gridRow1.ScaleAmount = scaleAmount1.ToString();
            var gridRow2 = paySpine.ScaleAwards.Rows[1];
            gridRow2.ScaleAmount = scaleAmount2.ToString();
            paySpineTriplet.ClickSave();
            paySpine = Search(code);
            gridRow1 = paySpine.ScaleAwards.Rows[0];
            gridRow2 = paySpine.ScaleAwards.Rows[1];

            //Assert
            Assert.AreEqual(code, paySpine.Code);
            Assert.AreEqual(minimumPoint.ToString(), paySpine.MinimumPoint);
            Assert.AreEqual(maximumPoint.ToString(), paySpine.MaximumPoint);
            Assert.AreEqual(interval.ToString(), paySpine.InterVal);

            Assert.AreEqual(awardDate.ToShortDateString(), gridRow1.AwardDate);
            Assert.AreEqual(minimumPoint.ToString(), gridRow1.ScalePoint);
            Assert.AreEqual(scaleAmount1.ToString(), gridRow1.ScaleAmount);

            Assert.AreEqual(awardDate.ToShortDateString(), gridRow2.AwardDate);
            Assert.AreEqual(maximumPoint.ToString(), gridRow2.ScalePoint);
            Assert.AreEqual(scaleAmount2.ToString(), gridRow2.ScaleAmount);
        }

        [TestMethod]
        [ChromeUiTest("PaySpine", "P1", "Read")]
        public void Read_existing_Pay_Spine_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, tenantID);

            using (new DataSetup(GetPaySpine(code)))
            {
                //Act
                LoginAndNavigate();
                var paySpine = Search(code);
                var gridRow1 = paySpine.ScaleAwards.Rows[0];
                var gridRow2 = paySpine.ScaleAwards.Rows[1];

                //Assert
                Assert.AreEqual(code, paySpine.Code);
                Assert.AreEqual(minimumPoint.ToString(), paySpine.MinimumPoint);
                Assert.AreEqual(maximumPoint.ToString(), paySpine.MaximumPoint);
                Assert.AreEqual(interval.ToString(), paySpine.InterVal);

                Assert.AreEqual(awardDate.ToShortDateString(), gridRow1.AwardDate);
                Assert.AreEqual(minimumPoint.ToString(), gridRow1.ScalePoint);
                Assert.AreEqual(scaleAmount1.ToString(), gridRow1.ScaleAmount);

                Assert.AreEqual(awardDate.ToShortDateString(), gridRow2.AwardDate);
                Assert.AreEqual(maximumPoint.ToString(), gridRow2.ScalePoint);
                Assert.AreEqual(scaleAmount2.ToString(), gridRow2.ScaleAmount);
            }
        }

        [TestMethod]
        [ChromeUiTest("PaySpine", "P1", "Update")]
        public void Update_existing_Pay_Spine_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, tenantID);
            const string newMinimumPoint = "3.0";
            const string newMaximumPoint = "4.0";
            string newAwardDate = awardDate.AddDays(1).ToShortDateString();
            const string newScaleAmount1 = "300.0000";
            const string newScaleAmount2 = "400.0000";

            using (new DataSetup(GetPaySpine(code)))
            {
                //Act
                LoginAndNavigate();
                var paySpine = Search(code);
                paySpine.MinimumPoint = newMinimumPoint;
                paySpine.MaximumPoint = newMaximumPoint;
                paySpine.AwardDate = newAwardDate;
                paySpine.ClickAddScaleAwards();
                var gridRow1 = paySpine.ScaleAwards.Rows[0];
                gridRow1.ScaleAmount = newScaleAmount1;
                var gridRow2 = paySpine.ScaleAwards.Rows[1];
                gridRow2.ScaleAmount = newScaleAmount2;
                paySpine.ClickSave();
                paySpine = Search(code);
                gridRow1 = paySpine.ScaleAwards.Rows[0];
                gridRow2 = paySpine.ScaleAwards.Rows[1];

                //Assert
                Assert.AreEqual(newMinimumPoint, paySpine.MinimumPoint);
                Assert.AreEqual(newMaximumPoint, paySpine.MaximumPoint);

                Assert.AreEqual(newAwardDate, gridRow1.AwardDate);
                Assert.AreEqual(newMinimumPoint, gridRow1.ScalePoint);
                Assert.AreEqual(newScaleAmount1, gridRow1.ScaleAmount);

                Assert.AreEqual(newAwardDate, gridRow2.AwardDate);
                Assert.AreEqual(newMaximumPoint, gridRow2.ScalePoint);
                Assert.AreEqual(newScaleAmount2, gridRow2.ScaleAmount);
            }
        }

        [TestMethod]
        [ChromeUiTest("PaySpine", "P1", "Delete")]
        public void Delete_Pay_Spine_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, tenantID);

            using (new DataSetup(GetPaySpine(code)))
            {
                //Act
                LoginAndNavigate();
                var paySpine = Search(code);
                paySpine.ClickDelete();
                PaySpineTriplet paySpineTriplet = new PaySpineTriplet();
                paySpineTriplet.SearchCriteria.SearchByCode = code;
                var paySpineResult = paySpineTriplet.SearchCriteria.Search().SingleOrDefault(t => t.Code.Equals(code));

                //Assert
                Assert.AreEqual(null, paySpineResult);
            }
        }

        [TestMethod]
        [ChromeUiTest("PaySpine", "P1", "Delete")]
        public void Delete_Scale_Award_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, tenantID);

            using (new DataSetup(GetPaySpine(code)))
            {
                //Act
                LoginAndNavigate();
                var paySpine = Search(code);
                var gridRow1 = paySpine.ScaleAwards.Rows.FirstOrDefault();
                gridRow1.DeleteRow();
                var gridRow2 = paySpine.ScaleAwards.Rows.FirstOrDefault();
                gridRow2.DeleteRow();
                paySpine.ClickSave();
                paySpine = Search(code);
                gridRow1 = paySpine.ScaleAwards.Rows.FirstOrDefault(x => x.ScaleAmount == scaleAmount1.ToString());
                gridRow2 = paySpine.ScaleAwards.Rows.FirstOrDefault(x => x.ScaleAmount == scaleAmount2.ToString());

                //Assert
                Assert.AreEqual(null, gridRow1);
                Assert.AreEqual(null, gridRow2);
            }
        }

        #endregion

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Pay Spines");
        }

        private static PaySpinePage Search(string code)
        {
            PaySpineTriplet paySpineTriplet = new PaySpineTriplet();
            paySpineTriplet.SearchCriteria.SearchByCode = code;
            var paySpineResult = paySpineTriplet.SearchCriteria.Search().Single(t => t.Code.Equals(code));
            var paySpine = paySpineResult.Click<PaySpinePage>();

            return paySpine;
        }

        #endregion

        #region Data Setup

        private DataPackage GetPaySpine(string code)
        {
            Guid psID = Guid.NewGuid();

            return this.BuildDataPackage()
                .AddData("PaySpine", new
                {
                    ID = psID,
                    Code = code,
                    MinimumPoint = minimumPoint,
                    MaximumPoint = maximumPoint,
                    Interval = interval,
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = tenantID
                })
                .AddData("PayAward", new
                {
                    ID = Guid.NewGuid(),
                    ScalePoint = minimumPoint,
                    ScaleAmount = scaleAmount1,
                    Date = awardDate,
                    PaySpine = psID,
                    TenantID = tenantID
                })
                .AddData("PayAward", new
                {
                    ID = Guid.NewGuid(),
                    ScalePoint = maximumPoint,
                    ScaleAmount = scaleAmount2,
                    Date = awardDate,
                    PaySpine = psID,
                    TenantID = tenantID
                });
        }

        #endregion
    }
}
