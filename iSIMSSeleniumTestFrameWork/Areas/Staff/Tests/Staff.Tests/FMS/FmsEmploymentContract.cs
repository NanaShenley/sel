using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeSugar.Automation;
using SeSugar.Data;
using System;
using System.Linq;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using WebDriverRunner.internals;
using Environment = SeSugar.Environment;
using Selene.Support.Attributes;

namespace Staff.Tests
{
    [TestClass]
    public class FmsEmploymentContract
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
        private readonly DateTime startDate = new DateTime(2016, 2, 1);

        #endregion

        #region Tests

        [TestMethod]
        [ChromeUiTest("FMS", "FmsEmploymentContractAllowance", "P1", "MaxLength")]
        public void FMS_EmploymentContract_AllowancePayFactor_MaxLengthTest()
        {
            //Arrange
            Guid staffId;
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);

            DataPackage package = GetPackage(out staffId, forename, surname);

            using (new DataSetup(package))
            {
                //Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staff = StaffSearch(staffId);
                var contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();
                var contract = new EditContractDialog();
                var gridRow = contract.AllowancesTable.Rows[0];
                gridRow.ClickEdit();

                var allowance = new StaffContractAllowanceDialog
                {
                    PayFactor = "99.99999"
                };

                allowance.ClickOk();

                //Assert
                Assert.IsTrue(allowance.Validation.ToList().Contains("Pay Factor cannot be more than 1.0000"));
                Assert.IsTrue(allowance.Validation.ToList().Contains("Employment Contract Allowance Pay Factor may have only 4 figure(s) after the decimal point."));
            }
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FmsEmploymentContractAllowance", "P1", "Rounding")]
        public void FMS_EmploymentContract_AllowancePayFactor_RoundingTest()
        {
            //Arrange
            Guid staffId;
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            const string payFactor = "0.5555";

            DataPackage package = GetPackage(out staffId, forename, surname);

            using (new DataSetup(package))
            {
                //Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staff = StaffSearch(staffId);
                var contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();
                var contract = new EditContractDialog();
                var gridRow = contract.AllowancesTable.Rows[0];
                gridRow.ClickEdit();

                var allowance = new StaffContractAllowanceDialog
                {
                    PayFactor = payFactor
                };

                allowance.ClickOk();
                contract.ClickOk();

                staff.ClickSave();
                staff = StaffSearch(staffId);
                contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();
                contract = new EditContractDialog();
                gridRow = contract.AllowancesTable.Rows[0];
                gridRow.ClickEdit();

                //Assert
                Assert.AreEqual(payFactor, allowance.PayFactor);
            }
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FmsEmploymentContract", "P1", "MaxLength")]
        public void FMS_EmploymentContract_HoursPerWeek_MaxLengthTest()
        {
            //Arrange
            Guid staffId;
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);

            DataPackage package = GetPackage(out staffId, forename, surname);

            using (new DataSetup(package))
            {
                //Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staff = StaffSearch(staffId);
                var contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();
                var contract = new EditContractDialog { HoursPerWeek = "9999.99999" };
                contract.ClickOk();

                //Assert
                Assert.IsTrue(contract.Validation.ToList().Contains("Hours/Week cannot be more than 999.9999."));
                Assert.IsTrue(contract.Validation.ToList().Contains("Employment Contract Hours worked per Week may have only 4 figure(s) after the decimal point."));
            }
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FmsEmploymentContract", "P1", "Rounding")]
        public void FMS_EmploymentContract_HoursPerWeek_RoundingTest()
        {
            //Arrange
            Guid staffId;
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            const string hoursPerWeek = "29.5555";
            DataPackage package = GetPackage(out staffId, forename, surname);

            using (new DataSetup(package))
            {
                //Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staff = StaffSearch(staffId);
                var contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();
                var contract = new EditContractDialog { HoursPerWeek = hoursPerWeek };
                contract.ClickOk();
                staff.ClickSave();
                staff = StaffSearch(staffId);
                contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();

                //Assert
                Assert.AreEqual(hoursPerWeek, contract.HoursPerWeek);
            }
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FmsEmploymentContract", "P1", "MaxLength")]
        public void FMS_EmploymentContract_WeeksPerYear_MaxLengthTest()
        {
            //Arrange
            Guid staffId;
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);

            DataPackage package = GetPackage(out staffId, forename, surname);

            using (new DataSetup(package))
            {
                //Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staff = StaffSearch(staffId);
                var contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();
                var contract = new EditContractDialog { WeeksPerYear = "54.00005" };
                contract.ClickOk();

                //Assert
                Assert.IsTrue(contract.Validation.ToList().Contains("Weeks/Year cannot be more than 53.0000."));
                Assert.IsTrue(contract.Validation.ToList().Contains("Employment Contract Weeks worked per Year may have only 4 figure(s) after the decimal point."));
            }
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FmsEmploymentContract", "P1", "Rounding")]
        public void FMS_EmploymentContract_WeeksPerYear_RoundingTest()
        {
            //Arrange
            Guid staffId;
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            const string weeksPerYear = "52.5555";
            DataPackage package = GetPackage(out staffId, forename, surname);

            using (new DataSetup(package))
            {
                //Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staff = StaffSearch(staffId);
                var contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();

                var contract = new EditContractDialog { WeeksPerYear = weeksPerYear };
                contract.ClickOk();
                staff.ClickSave();
                staff = StaffSearch(staffId);
                contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();

                //Assert
                Assert.AreEqual(weeksPerYear, contract.WeeksPerYear);
            }
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FmsEmploymentContractPayScale", "P1", "MaxLength")]
        public void FMS_EmploymentContract_PayScalePoint_MaxLengthTest()
        {
            //Arrange
            Guid staffId;
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);

            DataPackage package = GetPackage(out staffId, forename, surname);

            using (new DataSetup(package))
            {
                //Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staff = StaffSearch(staffId);
                var contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();
                var contract = new EditContractDialog();
                var gridRow = contract.PayScalesTable.Rows[0];
                gridRow.ClickEdit();

                var payScale = new PayScaleOnContractDialog { Point = "1.99" };
                payScale.ClickOk();

                //Assert
                Assert.IsTrue(payScale.Validation.ToList().Contains(
                    "Employment Contract Pay Scale Point may have only 1 figure(s) after the decimal point."));
            }
        }

        [TestMethod]
        [ChromeUiTest("FMS", "FmsEmploymentContractPayScale", "P1", "Rounding")]
        public void FMS_EmploymentContract_PayScalePoint_RoundingTest()
        {
            //Arrange
            Guid staffId;

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            const string point = "1.5";
            string staffRoleDescription = "Other School Admin";
            DataPackage package = GetPackage(out staffId, forename, surname);

            using (new DataSetup(package))
            {
                //Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staff = StaffSearch(staffId);
                var contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();
                var contract = new EditContractDialog();

                contract.ClickAddStaffRole();
                var roleGridRow = contract.StaffRolesTable.Rows[0];
                roleGridRow.StaffRole = staffRoleDescription;
                roleGridRow.StartDate = DateTime.Today.ToShortDateString();

                var gridRow = contract.PayScalesTable.Rows[0];
                gridRow.ClickEdit();

                var payScale = new PayScaleOnContractDialog
                {
                    Point = point
                };

                payScale.ClickOk();
                contract.ClickOk();
                staff.ClickSave();
                staff = StaffSearch(staffId);
                contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();
                contract = new EditContractDialog();
                gridRow = contract.PayScalesTable.Rows[0];
                gridRow.ClickEdit();

                //Assert
                Assert.AreEqual(point, payScale.Point);
            }
        }

        #endregion

        #region Helpers

        private static StaffRecordPage StaffSearch(Guid staffId)
        {
            return StaffRecordPage.LoadStaffDetail(staffId);
        }

        private DataPackage GetPackage(out Guid ID, string forename, string surname)
        {
            Guid employeeId,
                staffId,
                serviceRecordId,
                serviceTermId,
                allowanceId,
                addPayCatId,
                serviceTermAllowanceId,
                paySpineId,
                payAwardId,
                employmentContractId,
                ecPayScaleId,
                statPayScaleId,
                pscaleId,
                ecAllowanceId,
                ecRoleId,
                postTypeId,
                statutoryPostTypeId;

            const decimal minimumPoint = 1.0m;
            const decimal maximumPoint = 2.0m;
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4, Environment.Settings.TenantId);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 4, Environment.Settings.TenantId);

            DataPackage testData = new DataPackage();

            testData.AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, surname, employeeId, forename));

            ID = staffId;

            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, startDate));
            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId));

            testData.AddData("AdditionalPaymentCategory", DataPackageHelper.GenerateAdditionalPaymentCategory(out addPayCatId));
            testData.AddData("Allowance", DataPackageHelper.GenerateAllowance(out allowanceId, addPayCatId));
            testData.AddData("ServiceTermAllowance", DataPackageHelper.GenerateServiceTermAllowance(out serviceTermAllowanceId, allowanceId, serviceTermId));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, null, postTypeCode, postTypeDescription, statutoryPostTypeId));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, minimumPoint, maximumPoint, 0.5m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, minimumPoint, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, maximumPoint, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("StatutoryPayScale", DataPackageHelper.GenerateStatutoryPayScale(out statPayScaleId));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out pscaleId, serviceTermId, paySpineId, statPayScaleId));
            testData.AddData("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, startDate, postTypeId: postTypeId));
            testData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(out ecPayScaleId, pscaleId, employmentContractId, startDate));
            testData.AddData("EmploymentContractAllowance", DataPackageHelper.GenerateEmploymentContractAllowance(out ecAllowanceId, allowanceId, employmentContractId, startDate));
            testData.AddData("EmploymentContractRole", DataPackageHelper.GenerateEmploymentContractStaffRole(out ecRoleId, employmentContractId, startDate));

            return testData;
        }

        #endregion
    }
}
