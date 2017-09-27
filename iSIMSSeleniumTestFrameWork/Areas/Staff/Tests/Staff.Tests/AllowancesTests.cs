using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Linq;
using SeSugar.Automation;
using SeSugar.Data;
using Environment = SeSugar.Environment;
using Selene.Support.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Staff.Tests
{
    [TestClass]
    public class AllowancesTests
    {
        #region MS Unit Testing support
        public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext { get; set; }
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

        private readonly DateTime awardDate = DateTime.Today;
        private const decimal amount = 10.00m;
        private const string additionalPaymentCategory = "Acting";
        private const int displayOrder = 1;

        #endregion

        #region Tests

        [TestMethod]
        [ChromeUiTest(new[] { "Allowance", "P1", "Create" })]
        public void Create_new_Personal_Allowance_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("Allowance", "Code", 4, Environment.Settings.TenantId);
            string description = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);

            //Act
            LoginAndNavigate();
            var allowanceTriplet = new AllowanceTriplet();
            allowanceTriplet.ClickCreate();

            var allowance = new AllowanceDialog
            {
                Code = code,
                Description = description,
                IsVisible = true,
                DisplayOrder = displayOrder.ToString(),
                Category = additionalPaymentCategory,
                FixedAllowance = true
            };

            allowance.ClickAddAward();
            var aaGridRow = allowance.Awards.Rows[0];
            aaGridRow.AwardDate = awardDate.ToShortDateString();
            aaGridRow.Amount = amount.ToString();
            allowance.ClickOk();
            allowanceTriplet.ClickSave();
            var details = Search(code);
            var aGridRow = details.Allowances.Rows[0];
            aGridRow.ClickEdit();
            allowance = new AllowanceDialog();
            aaGridRow = allowance.Awards.Rows[0];

            //Assert
            Assert.AreEqual(code, allowance.Code);
            Assert.AreEqual(description, allowance.Description);
            Assert.AreEqual(displayOrder.ToString(), allowance.DisplayOrder);
            Assert.AreEqual(true, allowance.IsVisible);
            Assert.AreEqual(additionalPaymentCategory, allowance.Category);
            Assert.AreEqual(true, allowance.FixedAllowance);
            Assert.AreEqual(awardDate.ToShortDateString(), aaGridRow.AwardDate);
            Assert.AreEqual(amount.ToString(), aaGridRow.Amount);
        }

        [TestMethod]
        [ChromeUiTest(new[] { "Allowance", "P1", "Create" })]
        public void Create_new_Fixed_Allownace_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("Allowance", "Code", 4, Environment.Settings.TenantId);
            string description = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);

            //Act
            LoginAndNavigate();
            var allowanceTriplet = new AllowanceTriplet();
            allowanceTriplet.ClickCreate();

            var allowance = new AllowanceDialog
            {
                Code = code,
                Description = description,
                IsVisible = true,
                DisplayOrder = displayOrder.ToString(),
                Category = additionalPaymentCategory,
                PersonalAllowance = true
            };

            allowance.ClickOk();
            allowanceTriplet.ClickSave();
            var details = Search(code);
            var aGridRow = details.Allowances.Rows[0];
            aGridRow.ClickEdit();
            allowance = new AllowanceDialog();

            //Assert
            Assert.AreEqual(code, allowance.Code);
            Assert.AreEqual(description, allowance.Description);
            Assert.AreEqual(displayOrder.ToString(), allowance.DisplayOrder);
            Assert.AreEqual(true, allowance.IsVisible);
            Assert.AreEqual(additionalPaymentCategory, allowance.Category);
            Assert.AreEqual(true, allowance.PersonalAllowance);
        }

        [TestMethod]
        [ChromeUiTest(new[] { "Allowance", "P1", "Read" })]
        public void Read_Allowance_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("Allowance", "Code", 4, Environment.Settings.TenantId);
            string description = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);

            using (new DataSetup(GetAllowance(code, description)))
            {
                //Act
                LoginAndNavigate();
                var allowanceDetail = Search(code);
                var gridRow = allowanceDetail.Allowances.Rows[0];
                gridRow.ClickEdit();

                var allowance = new AllowanceDialog();
                var aaGridRow = allowance.Awards.Rows[0];

                //Assert
                Assert.AreEqual(code, allowance.Code);
                Assert.AreEqual(description, allowance.Description);
                Assert.AreEqual(displayOrder.ToString(), allowance.DisplayOrder);
                Assert.AreEqual(true, allowance.IsVisible);
                Assert.AreEqual(additionalPaymentCategory, allowance.Category);
                Assert.AreEqual(true, allowance.FixedAllowance);
                Assert.AreEqual(awardDate.ToShortDateString(), aaGridRow.AwardDate);
                Assert.AreEqual(amount.ToString(), aaGridRow.Amount);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "Allowance", "P1", "Update" })]
        public void Update_Allowance_Dialog_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("Allowance", "Code", 4, Environment.Settings.TenantId);
            string description = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);

            string newDescription = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);
            const string newDisplayOrder = "2";
            string newAwardDate = DateTime.Today.AddDays(1).ToShortDateString();
            const string newAmount = "15.99";
            const string newCategory = "Welcome Back";

            using (new DataSetup(GetAllowance(code, description)))
            {
                //Act
                LoginAndNavigate();
                var allowanceDetail = Search(code);
                var gridRow = allowanceDetail.Allowances.Rows[0];
                gridRow.ClickEdit();

                var allowance = new AllowanceDialog
                {
                    Description = newDescription,
                    DisplayOrder = newDisplayOrder,
                    IsVisible = false,
                    Category = newCategory
                };

                var aaGridRow = allowance.Awards.Rows[0];
                aaGridRow.AwardDate = newAwardDate;
                aaGridRow.Amount = newAmount;
                allowance.ClickOk();
                allowanceDetail.ClickSave();
                allowanceDetail = Search(code);
                gridRow = allowanceDetail.Allowances.Rows[0];
                gridRow.ClickEdit();
                allowance = new AllowanceDialog();
                aaGridRow = allowance.Awards.Rows[0];

                //Assert
                Assert.AreEqual(newDescription, allowance.Description);
                Assert.AreEqual(newDisplayOrder, allowance.DisplayOrder);
                Assert.AreEqual(false, allowance.IsVisible);
                Assert.AreEqual(newCategory, allowance.Category);
                Assert.AreEqual(newAwardDate, aaGridRow.AwardDate);
                Assert.AreEqual(newAmount, aaGridRow.Amount);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "Allowance", "P1", "Update" })]
        public void Update_Allowance_Grid_Row_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("Allowance", "Code", 4, Environment.Settings.TenantId);
            string description = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);

            string newDescription = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);
            const string newDisplayOrder = "2";
            const string newCategory = "Welcome Back";

            using (new DataSetup(GetAllowance(code, description)))
            {
                //Act
                LoginAndNavigate();
                var allowanceDetail = Search(code);
                var gridRow = allowanceDetail.Allowances.Rows[0];
                gridRow.Description = newDescription;
                gridRow.DisplayOrder = newDisplayOrder;
                gridRow.IsVisible = false;
                gridRow.Category = newCategory;
                allowanceDetail.ClickSave();
                allowanceDetail = Search(code);
                gridRow = allowanceDetail.Allowances.Rows[0];

                //Assert
                Assert.AreEqual(newDescription, gridRow.Description);
                Assert.AreEqual(newDisplayOrder, gridRow.DisplayOrder);
                Assert.AreEqual(false, gridRow.IsVisible);
                Assert.AreEqual(newCategory, gridRow.Category);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "Allowance", "P1", "Delete" })]
        public void Delete_Allowance_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("Allowance", "Code", 4, Environment.Settings.TenantId);
            string description = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);

            using (new DataSetup(GetAllowance(code, description)))
            {
                //Act
                LoginAndNavigate();
                var allowanceDetail = Search(code);
                var gridRow = allowanceDetail.Allowances.Rows[0];
                gridRow.DeleteRow();

                allowanceDetail.ClickSave();
                allowanceDetail = Search(code);
                gridRow = allowanceDetail.Allowances.Rows.FirstOrDefault();

                //Assert
                Assert.AreEqual(null, gridRow);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "Allowance", "P1", "Delete" })]
        public void Delete_Award_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("Allowance", "Code", 4, Environment.Settings.TenantId);
            string description = CoreQueries.GetColumnUniqueString("Allowance", "Description", 10, Environment.Settings.TenantId);

            using (new DataSetup(GetAllowance(code, description)))
            {
                //Act
                LoginAndNavigate();
                var allowanceDetail = Search(code);
                var gridRow = allowanceDetail.Allowances.Rows[0];
                gridRow.ClickEdit();
                var allowance = new AllowanceDialog();
                var aagridRow = allowance.Awards.Rows[0];
                aagridRow.DeleteRow();
                allowance.ClickOk();
                allowanceDetail.ClickSave();
                allowanceDetail = Search(code);
                gridRow = allowanceDetail.Allowances.Rows[0];
                gridRow.ClickEdit();
                allowance = new AllowanceDialog();

                //Assert
                Assert.AreEqual(1, allowance.Awards.Rows.Count);
            }
        }

        #endregion

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Allowances");
        }

        private static AllowanceDetailsPage Search(string code)
        {
            AllowanceTriplet allowanceTriplet = new AllowanceTriplet();
            allowanceTriplet.SearchCriteria.CodeOrDecription = code;
            var allowance = allowanceTriplet.SearchCriteria.Search<AllowanceDetailsPage>();

            return allowance;
        }

        #endregion

        #region Data Setup

        private DataPackage GetAllowance(string code, string description)
        {
            Guid allID = Guid.NewGuid();

            return this.BuildDataPackage()
                .AddData("Allowance", new
                {
                    ID = allID,
                    Code = code,
                    Description = description,
                    isVisible = true,
                    DisplayOrder = displayOrder,
                    AllowanceAwardAttached = true,
                    AdditionalPaymentCategory = CoreQueries.GetLookupItem("AdditionalPaymentCategory", Environment.Settings.TenantId, description: additionalPaymentCategory),
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = Environment.Settings.TenantId
                })
                .AddData("AllowanceAward", new
                {
                    ID = Guid.NewGuid(),
                    AwardDate = awardDate,
                    Amount = amount,
                    Allowance = allID,
                    TenantID = Environment.Settings.TenantId
                })
                .AddData("AllowanceAward", new
                {
                    ID = Guid.NewGuid(),
                    AwardDate = awardDate,
                    Amount = amount,
                    Allowance = allID,
                    TenantID = Environment.Settings.TenantId
                });
        }

        #endregion
    }
}
