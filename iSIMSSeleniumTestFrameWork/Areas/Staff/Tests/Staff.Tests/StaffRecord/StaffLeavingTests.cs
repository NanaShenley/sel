using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Staff.Tests.StaffRecord
{
    [TestClass]
    public class StaffLeavingTests
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
        [ChromeUiTest("StaffLeavingTests", "P1", "Can_make_a_current_member_of_Staff_with_Pay_Scales_a_Leaver_as_PO")]
        public void Can_make_a_current_member_of_Staff_with_Pay_Scales_a_Leaver_as_PO()
        {
            Guid staffId, serviceTermId, employmentContractId, serviceRecordId;
            DateTime dateOfArrival = DateTime.Today.AddDays(-100);
            DateTime dateOfLeaving = DateTime.Today.AddDays(-20);

            string destination = "Other";

            Action<StaffRecordPage, string> assertions = (staffRecord, expectedDol) =>
            {
                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows, "Could not bind POM for the Service Record table.");
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count, "There is more than one Service Record entry.");
                //Assert.AreEqual(expectedDol, staffRecord.ServiceRecordTable.Rows[0].DOL, "The DOL in the Service Record table is incorrect.");
                //Assert.AreEqual(expectedDol, staffRecord.StaffRecordDateOfLeaving, "The DOL on the \"Service Details\" pane.");
                //Assert.IsTrue(staffRecord.IsReAdmitDisplayed, "The re-admit button should be displayed at the top of the Staff record.");
            };

            Action<EditContractDialog, string, string> contractAssertions = ContractAssertions();
            Action<EditContractDialog, string, string> payScaleAssertions = PayScaleAssertions(dateOfArrival, dateOfLeaving);

            DataPackage currentStaff = this.BuildDataPackage();

            GenerateStaffServiceRecordAndEmploymentContractDataPackage(currentStaff, dateOfArrival, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId);
            GeneratePayScaleDataPackage(currentStaff, serviceTermId, employmentContractId, dateOfArrival);

            using (new DataSetup(currentStaff))
            {
                Make_leaver_common(assertions, new List<Action<EditContractDialog, string, string>> { contractAssertions, payScaleAssertions }, dateOfArrival, dateOfLeaving, destination, staffId);
            }
        }

        //[TestMethod][ChromeUiTest("StaffLeavingTests", "P1", "Can_make_a_current_member_of_Staff_with_Salary_Ranges_a_Leaver_as_PO")]
        public void Can_make_a_current_member_of_Staff_with_Salary_Ranges_a_Leaver_as_PO()
        {
            Guid staffId, serviceTermId, employmentContractId, serviceRecordId;
            DateTime dateOfArrival = DateTime.Today.AddDays(-100);
            DateTime dateOfLeaving = DateTime.Today.AddDays(-20);
            string destination = "Other";

            Action<StaffRecordPage, string> assertions = (staffRecord, expectedDol) =>
            {
                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows, "Could not bind POM for the Service Record table.");
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count, "There is more than one Service Record entry.");
                Assert.AreEqual(expectedDol, staffRecord.ServiceRecordTable.Rows[0].DOL, "The DOL in the Service Record table is incorrect.");
                Assert.AreEqual(expectedDol, staffRecord.StaffRecordDateOfLeaving, "The DOL on the \"Service Details\" pane.");
                Assert.IsTrue(staffRecord.IsReAdmitDisplayed, "The re-admit button should be displayed at the top of the Staff record.");
            };

            Action<EditContractDialog, string, string> contractAssertions = ContractAssertions();
            Action<EditContractDialog, string, string> salaryRangeAssertions = SalaryRangeAssertions(dateOfArrival, dateOfLeaving);

            DataPackage currentStaff = this.BuildDataPackage();

            GenerateStaffServiceRecordAndEmploymentContractDataPackage(currentStaff, dateOfArrival, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId);
            GenerateSalaryRangeDataPackage(currentStaff, serviceTermId, employmentContractId, dateOfArrival);

            using (new DataSetup(currentStaff))
            {
                Make_leaver_common(assertions, new List<Action<EditContractDialog, string, string>> { contractAssertions, salaryRangeAssertions }, dateOfArrival, dateOfLeaving, destination, staffId, true);
            }
        }

        //[TestMethod][ChromeUiTest("StaffLeavingTests", "P1", "Can_make_a_current_member_of_Staff_with_Pay_Scales_and_Salary_Ranges_a_Leaver_as_PO")]
        public void Can_make_a_current_member_of_Staff_with_Pay_Scales_and_Salary_Ranges_a_Leaver_as_PO()
        {
            Guid staffId, serviceTermId, employmentContractId, serviceRecordId;
            DateTime dateOfArrival = DateTime.Today.AddDays(-100);
            DateTime salaryRangeStartDate = DateTime.Today.AddDays(-40);
            DateTime payScaleEndDate = salaryRangeStartDate.AddDays(-1);
            DateTime dateOfLeaving = DateTime.Today.AddDays(-20);
            string destination = "Other";

            Action<StaffRecordPage, string> assertions = (staffRecord, expectedDol) =>
            {
                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows, "Could not bind POM for the Service Record table.");
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count, "There is more than one Service Record entry.");
                Assert.AreEqual(expectedDol, staffRecord.ServiceRecordTable.Rows[0].DOL, "The DOL in the Service Record table is incorrect.");
                Assert.AreEqual(expectedDol, staffRecord.StaffRecordDateOfLeaving, "The DOL on the \"Service Details\" pane.");
                Assert.IsTrue(staffRecord.IsReAdmitDisplayed, "The re-admit button should be displayed at the top of the Staff record.");
            };

            Action<EditContractDialog, string, string> contractAssertions = ContractAssertions();
            Action<EditContractDialog, string, string> payScaleAssertions = PayScaleAssertions(dateOfArrival, payScaleEndDate);
            Action<EditContractDialog, string, string> salaryRangeAssertions = SalaryRangeAssertions(salaryRangeStartDate, dateOfLeaving);

            DataPackage currentStaff = this.BuildDataPackage();

            GenerateStaffServiceRecordAndEmploymentContractDataPackage(currentStaff, dateOfArrival, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId);
            GeneratePayScaleDataPackage(currentStaff, serviceTermId, employmentContractId, dateOfArrival, payScaleEndDate);
            GenerateSalaryRangeDataPackage(currentStaff, serviceTermId, employmentContractId, salaryRangeStartDate);

            using (new DataSetup(currentStaff))
            {
                Make_leaver_common(assertions, new List<Action<EditContractDialog, string, string>> { contractAssertions, payScaleAssertions, salaryRangeAssertions }, dateOfArrival, dateOfLeaving, destination, staffId, true);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffLeavingTests", "P1", "Can_make_a_current_member_of_Staff_with_Pay_Scales_a_Leaver_in_the_future_as_PO")]
        public void Can_make_a_current_member_of_Staff_with_Pay_Scales_a_Leaver_in_the_future_as_PO()
        {
            Guid staffId, serviceTermId, employmentContractId, serviceRecordId;
            DateTime dateOfArrival = DateTime.Today.AddDays(-100);
            DateTime dateOfLeaving = DateTime.Today.AddDays(-20);
            string destination = "Other";

            Action<StaffRecordPage, string> assertions = (staffRecord, expectedDol) =>
            {
                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows, "Could not bind POM for the Service Record table.");
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count, "Unexpected number of ");
                Assert.AreEqual(expectedDol, staffRecord.ServiceRecordTable.Rows[0].DOL, "The DOL in the Service Record table is incorrect.");
                Assert.AreEqual(expectedDol, staffRecord.StaffRecordDateOfLeaving, "The DOL on the \"Service Details\" pane.");
            };

            Action<EditContractDialog, string, string> contractAssertions = ContractAssertions();
            Action<EditContractDialog, string, string> payScaleAssertions = PayScaleAssertions(dateOfArrival, dateOfLeaving);

            DataPackage currentStaff = this.BuildDataPackage();

            GenerateStaffServiceRecordAndEmploymentContractDataPackage(currentStaff, dateOfArrival, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId);
            GeneratePayScaleDataPackage(currentStaff, serviceTermId, employmentContractId, dateOfArrival);

            using (new DataSetup(currentStaff))
            {
                Make_leaver_common(assertions, new List<Action<EditContractDialog, string, string>> { contractAssertions, payScaleAssertions }, dateOfArrival, dateOfLeaving, destination, staffId);
            }
        }

        //[TestMethod][ChromeUiTest("StaffLeavingTests", "P1", "Can_make_a_current_member_of_Staff_with_Salary_Ranges_a_Leaver_in_the_future_as_PO")]
        public void Can_make_a_current_member_of_Staff_with_Salary_Ranges_a_Leaver_in_the_future_as_PO()
        {
            Guid staffId, serviceTermId, employmentContractId, serviceRecordId;
            DateTime dateOfArrival = DateTime.Today.AddDays(-100);
            DateTime dateOfLeaving = DateTime.Today.AddDays(20);
            string destination = "Other";

            Action<StaffRecordPage, string> assertions = (staffRecord, expectedDol) =>
            {
                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows, "Could not bind POM for the Service Record table.");
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count, "Unexpected number of ");
                Assert.AreEqual(expectedDol, staffRecord.ServiceRecordTable.Rows[0].DOL, "The DOL in the Service Record table is incorrect.");
                Assert.AreEqual(expectedDol, staffRecord.StaffRecordDateOfLeaving, "The DOL on the \"Service Details\" pane.");
            };

            Action<EditContractDialog, string, string> contractAssertions = ContractAssertions();
            Action<EditContractDialog, string, string> salaryRangeAssertions = SalaryRangeAssertions(dateOfArrival, dateOfLeaving);

            DataPackage currentStaff = this.BuildDataPackage();

            GenerateStaffServiceRecordAndEmploymentContractDataPackage(currentStaff, dateOfArrival, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId);
            GenerateSalaryRangeDataPackage(currentStaff, serviceTermId, employmentContractId, dateOfArrival);

            using (new DataSetup(currentStaff))
            {
                Make_leaver_common(assertions, new List<Action<EditContractDialog, string, string>> { contractAssertions, salaryRangeAssertions }, dateOfArrival, dateOfLeaving, destination, staffId, true);
            }
        }

        //[TestMethod][ChromeUiTest("StaffLeavingTests", "P1", "Can_make_a_current_member_of_Staff_with_Pay_Scales_and_Salary_Ranges_a_Leaver_in_the_future_as_PO")]
        public void Can_make_a_current_member_of_Staff_with_Pay_Scales_and_Salary_Ranges_a_Leaver_in_the_future_as_PO()
        {
            Guid staffId, serviceTermId, employmentContractId, serviceRecordId;
            DateTime dateOfArrival = DateTime.Today.AddDays(-100);
            DateTime salaryRangeStartDate = DateTime.Today.AddDays(-40);
            DateTime payScaleEndDate = salaryRangeStartDate.AddDays(-1);
            DateTime dateOfLeaving = DateTime.Today.AddDays(-20);
            string destination = "Other";

            Action<StaffRecordPage, string> assertions = (staffRecord, expectedDol) =>
            {
                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows, "Could not bind POM for the Service Record table.");
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count, "Unexpected number of ");
                Assert.AreEqual(expectedDol, staffRecord.ServiceRecordTable.Rows[0].DOL, "The DOL in the Service Record table is incorrect.");
                Assert.AreEqual(expectedDol, staffRecord.StaffRecordDateOfLeaving, "The DOL on the \"Service Details\" pane.");
            };

            Action<EditContractDialog, string, string> contractAssertions = ContractAssertions();
            Action<EditContractDialog, string, string> payScaleAssertions = PayScaleAssertions(dateOfArrival, payScaleEndDate);
            Action<EditContractDialog, string, string> salaryRangeAssertions = SalaryRangeAssertions(salaryRangeStartDate, dateOfLeaving);

            DataPackage currentStaff = this.BuildDataPackage();

            GenerateStaffServiceRecordAndEmploymentContractDataPackage(currentStaff, dateOfArrival, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId);
            GeneratePayScaleDataPackage(currentStaff, serviceTermId, employmentContractId, dateOfArrival, payScaleEndDate);
            GenerateSalaryRangeDataPackage(currentStaff, serviceTermId, employmentContractId, salaryRangeStartDate);

            using (new DataSetup(currentStaff))
            {
                Make_leaver_common(assertions, new List<Action<EditContractDialog, string, string>> { contractAssertions, payScaleAssertions, salaryRangeAssertions }, dateOfArrival, dateOfLeaving, destination, staffId, true);
            }
        }

        private Action<EditContractDialog, string, string> ContractAssertions()
        {
            return (employmentContract, expectedDol, expectedDestination) =>
            {
                Assert.AreEqual(expectedDol, employmentContract.EndDate, "The end date of the contract is incorrect");
                Assert.AreEqual(expectedDestination, employmentContract.EmploymentContractDestination);
            };
        }

        private Action<EditContractDialog, string, string> PayScaleAssertions(DateTime? payScaleStartDate, DateTime? payScaleEndDate)
        {
            return (employmentContract, expectedDol, expectDestination) =>
            {
                Assert.IsNotNull(employmentContract.PayScalesTable, "Could not bind POM for the Pay Scale table.");
                Assert.IsNotNull(employmentContract.PayScalesTable.Rows, "Could not bind POM for the Pay Scale table.");
                Assert.AreEqual(1, employmentContract.PayScalesTable.Rows.Count, "Unexpected number of Pay Scale records");
                Assert.AreEqual(payScaleStartDate.HasValue ? payScaleStartDate.Value.ToShortDateString() : String.Empty, employmentContract.PayScalesTable.Rows[0].StartDate);
                Assert.AreEqual(payScaleEndDate.HasValue ? payScaleEndDate.Value.ToShortDateString() : String.Empty, employmentContract.PayScalesTable.Rows[0].EndDate);
            };
        }

        private Action<EditContractDialog, string, string> SalaryRangeAssertions(DateTime? salaryRangeStartDate, DateTime? salaryRangeEndDate)
        {
            return (employmentContract, expectedDol, expectDestination) =>
            {
                Assert.IsNotNull(employmentContract.SalaryRangesTable, "Could not bind POM for the Salary Range table.");
                Assert.IsNotNull(employmentContract.SalaryRangesTable.Rows, "Could not bind POM for the Salary Range table.");
                Assert.AreEqual(1, employmentContract.SalaryRangesTable.Rows.Count, "Unexpected number of Salary Range records");
                Assert.AreEqual(salaryRangeStartDate.HasValue ? salaryRangeStartDate.Value.ToShortDateString() : String.Empty, employmentContract.SalaryRangesTable.Rows[0].StartDate);
                Assert.AreEqual(salaryRangeEndDate.HasValue ? salaryRangeEndDate.Value.ToShortDateString() : String.Empty, employmentContract.SalaryRangesTable.Rows[0].EndDate);
            };
        }

        private void Make_leaver_common(Action<StaffRecordPage, string> staffRecordAssertions, IEnumerable<Action<EditContractDialog, string, string>> contractAssertions, DateTime dateOfArrival, DateTime dateOfLeaving, string destination, Guid staffId, bool turnSalaryRangesOn = false)
        {
            Guid staffReasonForLeavingId;
            string dateOfLeavingStr = dateOfLeaving.ToShortDateString();
            string staffReasonForLeavingCode = Utilities.GenerateRandomString(4);
            string staffReasonForLeavingDescription = Utilities.GenerateRandomString(20);

            DataPackage dataPackage = new DataPackage();

            dataPackage.AddData("StaffReasonForLeaving", DataPackageHelper.GernateStaffReasonForLeaving(out staffReasonForLeavingId, staffReasonForLeavingCode, staffReasonForLeavingDescription));

            using (new DataSetup(dataPackage))
            {
                //Navigate to injected staff

                if (turnSalaryRangesOn)
                {
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
                }
                else
                {
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                }

                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                //Make the Staff a leaver
                StaffLeavingDetailPage staffLeavingDetailPage = staffRecord.NavigateToStaffLeavingDetail();
                staffLeavingDetailPage.DateOfLeaving = dateOfLeavingStr;
                staffLeavingDetailPage.ReasonForLeaving = staffReasonForLeavingDescription;
                staffLeavingDetailPage.EmploymentContractDestination = destination;
                ConfirmationDialog dialog = staffLeavingDetailPage.SaveValue();
                dialog.ClickOk();

                //Keep reloading the Staff record until the DOL textbox appears
                var timeOut = TimeSpan.FromMinutes(5);
                Stopwatch timer = new Stopwatch();
                timer.Start();
                while (timer.Elapsed < timeOut)
                {
                    staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                    if (staffRecord.IsDolDisplayed)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                timer.Stop();

                if (timer.Elapsed > timeOut)
                    Assert.Inconclusive(string.Format("Could not establish whether Staff member is a leaver after {0}. This could mean the queued item is yet to be processed.", timeOut));

                staffRecordAssertions(staffRecord, dateOfLeavingStr);

                if (contractAssertions != null)
                {
                    Assert.IsNotNull(staffRecord.ContractsTable);
                    Assert.IsNotNull(staffRecord.ContractsTable.Rows);
                    Assert.AreEqual(1, staffRecord.ContractsTable.Rows.Count);

                    EditContractDialog editDialog = staffRecord.ContractsTable.Rows[0].Edit();

                    foreach (var contractAssertion in contractAssertions)
                    {
                        contractAssertion(editDialog, dateOfLeavingStr, destination);
                    }
                }
            }
        }

        public void GenerateStaffServiceRecordAndEmploymentContractDataPackage(DataPackage dataPackage, DateTime dateOfArrival, out Guid staffId, out Guid serviceTermId, out Guid employmentContractId, out Guid serviceRecordId)
        {
            Guid employeeId;
            Guid postTypeId, statutoryPostTypeId, serviceTermPostTypeId, employmentContractRoleId;

            string forename_current = "StaffLeaverTest";
            string surname_current = Utilities.GenerateRandomString(6, "Selenium");

            dataPackage
                .AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId))
                .AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId))
                .AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, statutoryPostTypeId: statutoryPostTypeId))
                .AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermPostTypeId, postTypeId, serviceTermId))
                .AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId))
                .AddData("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, dateOfArrival, postTypeId: postTypeId))
                .AddData("EmploymentContractRole", DataPackageHelper.GenerateEmploymentContractStaffRole(out employmentContractRoleId, employmentContractId, dateOfArrival))
                .AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, surname_current, forename: forename_current, employeeId: employeeId))
                .AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, dateOfArrival, null));
        }

        public void GeneratePayScaleDataPackage(DataPackage existingDataPackage, Guid serviceTermId, Guid employmentContractId, DateTime startDate, DateTime? endDate = null)
        {
            Guid paySpineId, payAwardId, payScaleId, employmentContractPayScale;

            existingDataPackage
                .AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, 1m, 10m, 1m))
                .AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1m))
                .AddData("PayScale", DataPackageHelper.GeneratePayScale(out payScaleId, serviceTermId, paySpineId))
                .AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(out employmentContractPayScale, payScaleId, employmentContractId, startDate, endDate));
        }

        public void GenerateSalaryRangeDataPackage(DataPackage existingDataPackage, Guid serviceTermId, Guid employmentContractId, DateTime startDate, DateTime? endDate = null)
        {
            Guid payLevelId, regionalWeightingId, salaryAwardid;
            Guid serviceTermSalaryRangeId;
            Guid salaryRangeId;
            Guid employmentContractSalaryRangeId;

            string payLevelCode = Utilities.GenerateRandomString(20);
            string payLevelDescription = Utilities.GenerateRandomString(20);
            string regionalWeightingCode = Utilities.GenerateRandomString(20);
            string regionalWeightingDescription = Utilities.GenerateRandomString(20);
            string salaryRangeCode = Utilities.GenerateRandomString(8);
            string salaryRangeDescription = Utilities.GenerateRandomString(100);
            DateTime awardDateTime = DateTime.Now.AddDays(1);
            string awardDate = awardDateTime.ToString("dd/MM/yyyy");
            decimal maximumAmount = 200, minimumAmount = 20;

            existingDataPackage.AddData("PayLevel", DataPackageHelper.GeneratePayLevel(out payLevelId, payLevelCode, payLevelDescription))
                .AddData("RegionalWeighting", DataPackageHelper.GenerateRegionalWeighting(out regionalWeightingId, regionalWeightingCode, regionalWeightingDescription))
                .AddData("SalaryRange", DataPackageHelper.GenerateSalaryRange(out salaryRangeId, salaryRangeCode, salaryRangeDescription, payLevelId, regionalWeightingId))
                .AddData("SalaryAward", DataPackageHelper.GenerateSalaryAward(out salaryAwardid, awardDateTime, maximumAmount, minimumAmount, salaryRangeId))
                .AddData("ServiceTermSalaryRange", DataPackageHelper.GenerateServiceTermSalaryRange(out serviceTermSalaryRangeId, serviceTermId, salaryRangeId))
                .AddData("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(out employmentContractSalaryRangeId, employmentContractId, salaryRangeId, 1000m, startDate));
        }

    }
}
