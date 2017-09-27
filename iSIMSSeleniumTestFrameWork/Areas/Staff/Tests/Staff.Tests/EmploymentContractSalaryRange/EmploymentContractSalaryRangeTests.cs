using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Components.Staff.Dialogs;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Staff.Tests.EmploymentContractSalaryRange
{
    [TestClass]
    public class EmploymentContractSalaryRangeTests
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
        // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Create_new_Employment_Contract_Salary_Range_as_PO")]
        public void Create_new_Employment_Contract_Salary_Range_as_PO()
        {
            Create_new_Employment_Contract_Salary_Range();
        }

        // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Read_Employment_Contract_Salary_Range_as_PO")]
        public void Read_Employment_Contract_Salary_Range_as_PO()
        {
            Read_Employment_Contract_Salary_Range();
        }

        // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Update_Employment_Contract_Salary_Range_as_PO")]
        public void Update_Employment_Contract_Salary_Range_as_PO()
        {
            Edit_Employment_Contract_Salary_Range();
        }

        /// <summary>
        /// Checks the update of the employment contract salary range creates one history record.
        /// </summary>
        // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Update_Employment_Contract_Salary_Range_Check_History_as_PO")]
        public void Update_Employment_Contract_Salary_Range_Check_History_as_PO()
        {
            Edit_Employment_Contract_Salary_Range_Check_History();
        }

        ///<summary>
        ///Delete the employment contract salary range.
        ///</summary>
        // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Delete_Employment_Contract_Salary_Range_as_PO")]
        public void Delete_Employment_Contract_Salary_Range_as_PO()
        {
            Delete_Employment_Contract_Salary_Range();
        }

        // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "End_Open_Ended_Emplyment_Contract_Salary_Record_as_PO")]
        public void End_Open_Ended_Emplyment_Contract_Salary_Record_as_PO()
        {
            End_Open_Ended_Emplyment_Contract_Salary_Record();
        }

        // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Check_Emplyment_Contract_Salary_Record_Exists_Within_Range_as_PO")]
        public void Check_Emplyment_Contract_Salary_Record_Exists_Within_Range_as_PO()
        {
            Check_Emplyment_Contract_Salary_Record_Exists_Within_Range();
        }

        //[TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Check_Emplyment_Contract_Salary_Record_Date_Overlap_as_PO")]
        public void Check_Emplyment_Contract_Salary_Record_Date_Overlap_as_PO()
        {
            Check_Employment_Contract_Salary_Range_Date_Overlap();
        }

        // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Check_Emplyment_Contract_Salary_Record_Sequential_as_PO")]
        public void Check_Emplyment_Contract_Salary_Record_Sequential_as_PO()
        {
            Check_Employment_Contract_Salary_Range_Sequential();
        }

        private void Create_new_Employment_Contract_Salary_Range()
        {
            Guid employmentContractId, serviceTermId, salaryRangeId, staffId, employeeId, postTypeId;
            string serviceTermDescription, salaryRangeCode, salaryRangeDescription, payLevelCode, payLevelDescription,
                    regionalWeightingDescription, postTypeDescription, statutoryOrigin;

            using (new DataSetup(SetupTestData(
                        out employmentContractId,
                        out serviceTermId,
                        out salaryRangeId,
                        out staffId,
                        out employeeId,
                        out postTypeId,
                        out serviceTermDescription,
                        out salaryRangeCode,
                        out salaryRangeDescription,
                        out payLevelCode,
                        out payLevelDescription,
                        out regionalWeightingDescription,
                        out postTypeDescription
                        )))
            {
                InitialiseToStaffRecordPage();

                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                staffRecord.ClickAddContract();

                AddContractDetailDialog addContractDialog = new AddContractDetailDialog();

                addContractDialog.ServiceTermCombobox = serviceTermDescription;
                addContractDialog.EmploymentTypeCombobox = "Permanent";
                addContractDialog.PostTypeCombobox = postTypeDescription;
                // addContractDialog.AnnualLeaveEntitlementDays = "0.0";
                addContractDialog.EmploymentContractOrigin = "Other";
                addContractDialog.StartDate = DateTime.Now.ToShortDateString();

                SelectSalaryRangeDialog selectPayScaleDialog = addContractDialog.ClickAddSalaryRange();

                selectPayScaleDialog.SalaryRange = salaryRangeDescription;



                selectPayScaleDialog.AnnualSalary = "200";
                selectPayScaleDialog.StartDate = DateTime.Now.ToShortDateString();
                selectPayScaleDialog.Notes = "Ths is a note (don't forget :->)";

                selectPayScaleDialog.ClickOk();

                addContractDialog.Refresh();

                addContractDialog.ClickOk();

                staffRecord.SaveStaff();
            }
        }

        private void Read_Employment_Contract_Salary_Range()
        {
            Guid employmentContractId, serviceTermId, salaryRangeId, staffId, employmentContractSlaryRangeId, employeeId, postTypeId;
            string serviceTermDescription, salaryRangeCode, salaryRangeDescription,
                payLevelCode, payLevelDescription, regionalWeightingDescription,
                postTypeDescription;

            DataPackage initialData = SetupTestData(
                                out employmentContractId,
                                out serviceTermId,
                                out salaryRangeId,
                                out staffId,
                                out employeeId,
                                out postTypeId,
                                out serviceTermDescription,
                                out salaryRangeCode,
                                out salaryRangeDescription,
                                out payLevelCode,
                                out payLevelDescription,
                                out regionalWeightingDescription,
                                out postTypeDescription
                                );

            initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));

            decimal salary = 123456;
            string notes = Utilities.GenerateRandomString(200);
            DateTime? expectedStartDate = DateTime.Now;
            initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                    out employmentContractSlaryRangeId,
                    employmentContractId,
                    salaryRangeId,
                    salary,
                    expectedStartDate,
                    null,
                    false,
                    false,
                    notes)
                );

            using (new DataSetup(initialData))
            {
                InitialiseToStaffRecordPage();

                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                staffRecord.ContractsTable.Rows[0].ClickEdit();

                EditContractDialog editContractDialog = new EditContractDialog();
                SelectSalaryRangeDialog selectSalaryRangeDialog = editContractDialog.SalaryRangesTable[0].EditSalaryRange();

                decimal actualSalary = 0;
                decimal.TryParse(selectSalaryRangeDialog.AnnualSalary, out actualSalary);

                Assert.AreEqual(salaryRangeDescription, selectSalaryRangeDialog.SalaryRange);

                Assert.AreEqual(salary, actualSalary);
                Assert.AreEqual(notes, selectSalaryRangeDialog.Notes);
                Assert.AreEqual(expectedStartDate.Value.ToShortDateString(), selectSalaryRangeDialog.StartDate);
            }
        }

        private void Edit_Employment_Contract_Salary_Range()
        {
            Guid employmentContractId, serviceTermId, salaryRangeId, staffId, employmentContractSlaryRangeId, employeeId, postTypeId;
            string serviceTermDescription, salaryRangeCode, salaryRangeDescription,
                payLevelCode, payLevelDescription, regionalWeightingDescription,
                postTypeDescription;

            DataPackage initialData = SetupTestData(
                                out employmentContractId,
                                out serviceTermId,
                                out salaryRangeId,
                                out staffId,
                                out employeeId,
                                out postTypeId,
                                out serviceTermDescription,
                                out salaryRangeCode,
                                out salaryRangeDescription,
                                out payLevelCode,
                                out payLevelDescription,
                                out regionalWeightingDescription,
                                out postTypeDescription
                                );

            initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));

            decimal salary = 123456;
            string notes = Utilities.GenerateRandomString(200);
            DateTime? expectedStartDate = DateTime.Now;
            initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                    out employmentContractSlaryRangeId,
                    employmentContractId,
                    salaryRangeId,
                    salary,
                    expectedStartDate,
                    null,
                    false,
                    false,
                    notes)
                );

            using (new DataSetup(initialData))
            {
                InitialiseToStaffRecordPage();

                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                staffRecord.ContractsTable.Rows[0].ClickEdit();
                EditContractDialog editContractDialog = new EditContractDialog();

                SelectSalaryRangeDialog salaryRangeDialog = editContractDialog.SalaryRangesTable[0].EditSalaryRange();

                double newAnnualSalary = 200;
                string newNotes = "Update Notes";

                salaryRangeDialog.AnnualSalary = newAnnualSalary.ToString();
                salaryRangeDialog.Notes = newNotes;
                // Dont change the dates or the SalaryRange as this may click off validation that are not defined yet.
                // thus making this test technical debt.

                salaryRangeDialog.ClickOk();

                // Set up the staff role to enable saving
                string staffRoleDescription = "Other School Admin";
                editContractDialog.ClickAddStaffRole();
                var gridRow = editContractDialog.StaffRolesTable.Rows[0];
                gridRow.StaffRole = staffRoleDescription;
                gridRow.StartDate = DateTime.Today.ToShortDateString();

                editContractDialog.ClickOk();

                staffRecord.SaveStaff();

                // Reload and assert
                StaffRecordPage newStaffRecord = StaffRecordPage.LoadStaffDetail(staffId);
                newStaffRecord.ContractsTable.Rows[0].ClickEdit();

                EditContractDialog newEditContractDialog = new EditContractDialog();
                SelectSalaryRangeDialog newSalaryRangeDialog = newEditContractDialog.SalaryRangesTable[0].EditSalaryRange();

                Assert.AreEqual(newAnnualSalary, decimal.Parse(newSalaryRangeDialog.AnnualSalary));
                Assert.AreEqual(newNotes, newSalaryRangeDialog.Notes);
            }
        }

        private void Edit_Employment_Contract_Salary_Range_Check_History()
        {
            Guid employmentContractId, serviceTermId, salaryRangeId, staffId, employmentContractSlaryRangeId, employeeId, postTypeId;
            string serviceTermDescription, salaryRangeCode, salaryRangeDescription,
                payLevelCode, payLevelDescription, regionalWeightingDescription,
                postTypeDescription;

            DataPackage initialData = SetupTestData(
                                out employmentContractId,
                                out serviceTermId,
                                out salaryRangeId,
                                out staffId,
                                out employeeId,
                                out postTypeId,
                                out serviceTermDescription,
                                out salaryRangeCode,
                                out salaryRangeDescription,
                                out payLevelCode,
                                out payLevelDescription,
                                out regionalWeightingDescription,
                                out postTypeDescription
                                );

            initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));

            decimal salary = 123456;
            string notes = Utilities.GenerateRandomString(200);
            DateTime? expectedStartDate = DateTime.Now;
            initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                    out employmentContractSlaryRangeId,
                    employmentContractId,
                    salaryRangeId,
                    salary,
                    expectedStartDate,
                    null,
                    false,
                    false,
                    notes)
                );

            using (new DataSetup(initialData))
            {
                InitialiseToStaffRecordPage();

                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                staffRecord.ContractsTable.Rows[0].ClickEdit();
                EditContractDialog editContractDialog = new EditContractDialog();

                SelectSalaryRangeDialog salaryRangeDialog = editContractDialog.SalaryRangesTable[0].EditSalaryRange();

                double newAnnualSalary = 200;
                string newNotes = "Update Notes";
                DateTime newStartDate = DateTime.Today;

                salaryRangeDialog.AnnualSalary = newAnnualSalary.ToString();
                salaryRangeDialog.Notes = newNotes;
                // Dont change the dates or the SalaryRange as this may click off validation that are not defined yet.
                // thus making this test technical debt.

                salaryRangeDialog.ClickOk();

                // Set up the staff role to enable saving
                string staffRoleDescription = "Other School Admin";
                editContractDialog.ClickAddStaffRole();
                var gridRow = editContractDialog.StaffRolesTable.Rows[0];
                gridRow.StaffRole = staffRoleDescription;
                gridRow.StartDate = newStartDate.ToShortDateString();

                editContractDialog.ClickOk();

                staffRecord.SaveStaff();

                // Reload and assert
                StaffRecordPage newStaffRecord = StaffRecordPage.LoadStaffDetail(staffId);
                newStaffRecord.ContractsTable.Rows[0].ClickEdit();

                EditContractDialog newEditContractDialog = new EditContractDialog();

                SalaryRangeHistoryPopup salaryRangeHistoryPopup = null;

                //Keep reloading the Staff record until the popup shows the salaryRecord history change
                // This is because the audit is writen via a queue service and may take a little time.
                var timeOut = TimeSpan.FromMinutes(5);
                Stopwatch timer = new Stopwatch();
                timer.Start();
                while (timer.Elapsed < timeOut)
                {
                    SelectSalaryRangeDialog editSalaryRange = newEditContractDialog.SalaryRangesTable[0].EditSalaryRange();
                    salaryRangeHistoryPopup = editSalaryRange.ViewSalaryRangeHistory();

                    if (salaryRangeHistoryPopup.SalaryHistoryRecords.Any())
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
                    Assert.Inconclusive(string.Format("Could not establish whether Salary Range was updated after {0}. This could mean the queued item is yet to be processed.", timeOut));
                else
                {
                    // verify 
                    Assert.IsTrue(salaryRangeHistoryPopup.Title == "Salary Record Change History");
                    Assert.IsTrue(salaryRangeHistoryPopup.SalaryHistoryRecords.Count() == 1);

                    var onlySalaryRecordChange = salaryRangeHistoryPopup.SalaryHistoryRecords.First();

                    // cant check the username and date as they are set by the system.
                    Assert.IsTrue(double.Parse(onlySalaryRecordChange.Salary) == newAnnualSalary);
                }
                salaryRangeHistoryPopup.ClickClose();
            }
        }


        private void Delete_Employment_Contract_Salary_Range()
        {
            Guid employmentContractId, serviceTermId, salaryRangeId, staffId, employmentContractSlaryRangeId, employeeId, postTypeId;
            string serviceTermDescription, salaryRangeCode, salaryRangeDescription,
                payLevelCode, payLevelDescription, regionalWeightingDescription,
                postTypeDescription;

            DataPackage initialData = SetupTestData(
                                out employmentContractId,
                                out serviceTermId,
                                out salaryRangeId,
                                out staffId,
                                out employeeId,
                                out postTypeId,
                                out serviceTermDescription,
                                out salaryRangeCode,
                                out salaryRangeDescription,
                                out payLevelCode,
                                out payLevelDescription,
                                out regionalWeightingDescription,
                                out postTypeDescription
                                );

            initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));

            decimal salary = 123456;
            string notes = Utilities.GenerateRandomString(200);
            DateTime? expectedStartDate = DateTime.Now;
            initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                    out employmentContractSlaryRangeId,
                    employmentContractId,
                    salaryRangeId,
                    salary,
                    expectedStartDate,
                    null,
                    false,
                    false,
                    notes)
                );

            using (new DataSetup(initialData))
            {
                InitialiseToStaffRecordPage();

                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                staffRecord.ContractsTable.Rows[0].ClickEdit();

                EditContractDialog editContractDialog = new EditContractDialog();

                int salaryRangeCount = editContractDialog.SalaryRangesTable.Rows.Count();

                editContractDialog.SalaryRangesTable[0].DeleteRow();

                editContractDialog.Refresh();
                int newSalaryRangeCount = editContractDialog.SalaryRangesTable.Rows.Count();

                Assert.IsTrue(salaryRangeCount == 1);
                Assert.IsTrue(newSalaryRangeCount == 0);

            }
        }

        private void InitialiseToStaffRecordPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
            AutomationSugar.WaitForAjaxCompletion();
        }

        private static DataPackage SetupTestData(
            out Guid employmentContractId,
            out Guid serviceTermId,
            out Guid salaryRangeId,
            out Guid staffId,
            out Guid employeeId,
            out Guid postTypeId,
            out string serviceTermDescription,
            out string salaryRangeCode,
            out string salaryRangeDescription,
            out string payLevelCode,
            out string payLevelDescription,
            out string regionalWeightingDescription,
            out string postTypeDescription,
            bool omitPayScales = false)
        {
            DataPackage testData = new DataPackage();

            employmentContractId = Guid.Empty;

            Guid paySpineId, payAwardId, payscaleId, statutoryPostTypeId, serviceTermsPostTypeId;
            string postTypeCode = Utilities.GenerateRandomString(4);
            postTypeDescription = Utilities.GenerateRandomString(20);
            string paySpineCode = Utilities.GenerateRandomString(4);
            string serviceTermCode = Utilities.GenerateRandomString(2);
            serviceTermDescription = Utilities.GenerateRandomString(20);
            decimal weeksPerYear = 50m, hoursPerWeek = 40m;

            salaryRangeCode = Utilities.GenerateRandomString(8);
            salaryRangeDescription = Utilities.GenerateRandomString(100);
            DateTime awardDateTime = DateTime.Now;
            string awardDate = awardDateTime.ToString("dd/MM/yyyy");
            decimal maximumAmount = 200, minimumAmount = 20;

            Guid payLevelId, regionalWeightingId, salaryAwardid;
            Guid serviceTermSalaryRangeId;

            payLevelCode = Utilities.GenerateRandomString(20);
            payLevelDescription = Utilities.GenerateRandomString(20);
            string regionalWeightingCode = Utilities.GenerateRandomString(20);
            regionalWeightingDescription = Utilities.GenerateRandomString(20);

            Guid serviceRecordId;
            string staffSurname = Utilities.GenerateRandomString(100);
            string staffForename = Utilities.GenerateRandomString(100);

            testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, serviceTermCode, serviceTermDescription, weeksPerYear, hoursPerWeek));

            testData.AddData("PayLevel", DataPackageHelper.GeneratePayLevel(out payLevelId, payLevelCode, payLevelDescription));
            testData.AddData("RegionalWeighting", DataPackageHelper.GenerateRegionalWeighting(out regionalWeightingId, regionalWeightingCode, regionalWeightingDescription));
            testData.AddData("SalaryRange", DataPackageHelper.GenerateSalaryRange(out salaryRangeId, salaryRangeCode, salaryRangeDescription, payLevelId, regionalWeightingId));
            testData.AddData("SalaryAward", DataPackageHelper.GenerateSalaryAward(out salaryAwardid, awardDateTime, maximumAmount, minimumAmount, salaryRangeId));

            testData.AddData("ServiceTermSalaryRange", DataPackageHelper.GenerateServiceTermSalaryRange(out serviceTermSalaryRangeId, serviceTermId, salaryRangeId));

            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            if (!omitPayScales)
                testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            testData.AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname, employeeId, forename: staffForename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, DateTime.Now, null));

            return testData;
        }

        private void End_Open_Ended_Emplyment_Contract_Salary_Record()
        {
            Guid employmentContractId, serviceTermId, salaryRangeId, staffId, employmentContractSlaryRangeId, employeeId, postTypeId;
            string serviceTermDescription, salaryRangeCode, salaryRangeDescription,
                payLevelCode, payLevelDescription, regionalWeightingDescription,
                postTypeDescription;

            DataPackage initialData = SetupTestData(
                                out employmentContractId,
                                out serviceTermId,
                                out salaryRangeId,
                                out staffId,
                                out employeeId,
                                out postTypeId,
                                out serviceTermDescription,
                                out salaryRangeCode,
                                out salaryRangeDescription,
                                out payLevelCode,
                                out payLevelDescription,
                                out regionalWeightingDescription,
                                out postTypeDescription
                                );

            initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));

            decimal salary = 123456;
            string notes = Utilities.GenerateRandomString(200);
            DateTime? expectedStartDate = DateTime.Now;
            initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                    out employmentContractSlaryRangeId,
                    salaryRangeId,
                    employmentContractId,
                    salary,
                    expectedStartDate,
                    null,
                    false,
                    false,
                    notes)
                );

            using (new DataSetup(initialData))
            {
                InitialiseToStaffRecordPage();

                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                staffRecord.ContractsTable.Rows[0].ClickEdit();
                EditContractDialog editContractDialog = new EditContractDialog();

                SelectSalaryRangeDialog salaryRangeDialog = editContractDialog.SalaryRangesTable[0].EditSalaryRange();

                double newAnnualSalary = 100;
                string newNotes = "Update Notes";

                salaryRangeDialog.AnnualSalary = newAnnualSalary.ToString();
                salaryRangeDialog.Notes = newNotes;
                salaryRangeDialog.ClickOk();
                salaryRangeDialog.EditWarningDialog();
                editContractDialog.ClickOk();

                staffRecord.SaveStaff();

                // Reload and assert
                StaffRecordPage newStaffRecord = StaffRecordPage.LoadStaffDetail(staffId);
                newStaffRecord.ContractsTable.Rows[0].ClickEdit();

                EditContractDialog newEditContractDialog = new EditContractDialog();
                SelectSalaryRangeDialog newSalaryRangeDialog = newEditContractDialog.SalaryRangesTable[0].EditSalaryRange();

                Assert.AreEqual(newAnnualSalary, decimal.Parse(newSalaryRangeDialog.AnnualSalary));
                Assert.AreEqual(newNotes, newSalaryRangeDialog.Notes);
            }
        }

        private void Check_Employment_Contract_Salary_Range_Date_Overlap()
        {
            Guid employmentContractId, serviceTermId, salaryRangeId, staffId, employmentContractSlaryRangeId, employeeId, PayScaleId, postTypeId;
            string serviceTermDescription, salaryRangeCode, salaryRangeDescription,
                payLevelCode, payLevelDescription, regionalWeightingDescription,
                postTypeDescription;

            DataPackage initialData = SetupTestData(
                                out employmentContractId,
                                out serviceTermId,
                                out salaryRangeId,
                                out staffId,
                                out employeeId,
                                out postTypeId,
                                out serviceTermDescription,
                                out salaryRangeCode,
                                out salaryRangeDescription,
                                out payLevelCode,
                                out payLevelDescription,
                                out regionalWeightingDescription,
                                out postTypeDescription
                                );

            initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));

            decimal salary = 123456;
            string notes = Utilities.GenerateRandomString(200);
            DateTime? expectedStartDate = DateTime.Now;
            initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                    out employmentContractSlaryRangeId,
                    employmentContractId,
                    salaryRangeId,
                    salary,
                    expectedStartDate,
                    null,
                    false,
                    false,
                    notes)
                );

            using (new DataSetup(initialData))
            {
                InitialiseToStaffRecordPage();

                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                staffRecord.ContractsTable.Rows[0].ClickEdit();
                EditContractDialog editContractDialog = new EditContractDialog();

                SelectSalaryRangeDialog salaryRangeDialog = editContractDialog.SalaryRangesTable[0].EditSalaryRange();

                double newAnnualSalary = 100;
                string newNotes = "Update Notes";

                salaryRangeDialog.AnnualSalary = newAnnualSalary.ToString();
                var endDate = DateTime.Today.AddDays(5).ToShortDateString();
                salaryRangeDialog.EndDate = endDate;
                salaryRangeDialog.Notes = newNotes;
                salaryRangeDialog.ClickOk();
                salaryRangeDialog.EditWarningDialog();
                editContractDialog.ClickOk();

                staffRecord.SaveStaff();

                // Reload and assert
                StaffRecordPage newStaffRecord = StaffRecordPage.LoadStaffDetail(staffId);
                newStaffRecord.ContractsTable.Rows[0].ClickEdit();

                EditContractDialog newEditContractDialog = new EditContractDialog();
                AddContractDetailDialog addContractDialog = new AddContractDetailDialog();
                SelectSalaryRangeDialog addNewsalaryRangeDialog = addContractDialog.ClickAddSalaryRange();
                addNewsalaryRangeDialog.SalaryRange = salaryRangeDescription;
                addNewsalaryRangeDialog.AnnualSalary = newAnnualSalary.ToString();
                addNewsalaryRangeDialog.StartDate = endDate;
                addNewsalaryRangeDialog.Notes = newNotes;
                addNewsalaryRangeDialog.ClickOk();
                editContractDialog.ClickOk();

                List<string> validation = addContractDialog.Validation.ToList();
                Assert.IsTrue(validation.Contains("Salary Range salary records cannot overlap."));
            }
        }

        private void Check_Employment_Contract_Salary_Range_Sequential()
        {
            Guid employmentContractId, serviceTermId, salaryRangeId, staffId, employmentContractSlaryRangeId, employeeId, PayScaleId, postTypeId;
            string serviceTermDescription, salaryRangeCode, salaryRangeDescription,
                payLevelCode, payLevelDescription, regionalWeightingDescription,
                postTypeDescription;

            DataPackage initialData = SetupTestData(
                                out employmentContractId,
                                out serviceTermId,
                                out salaryRangeId,
                                out staffId,
                                out employeeId,
                                out postTypeId,
                                out serviceTermDescription,
                                out salaryRangeCode,
                                out salaryRangeDescription,
                                out payLevelCode,
                                out payLevelDescription,
                                out regionalWeightingDescription,
                                out postTypeDescription
                                );

            initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));

            decimal salary = 123456;
            string notes = Utilities.GenerateRandomString(200);
            DateTime? expectedStartDate = DateTime.Now;
            initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                    out employmentContractSlaryRangeId,
                    employmentContractId,
                    salaryRangeId,
                    salary,
                    expectedStartDate,
                    null,
                    false,
                    false,
                    notes)
                );

            using (new DataSetup(initialData))
            {
                InitialiseToStaffRecordPage();

                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                staffRecord.ContractsTable.Rows[0].ClickEdit();
                EditContractDialog editContractDialog = new EditContractDialog();

                SelectSalaryRangeDialog salaryRangeDialog = editContractDialog.SalaryRangesTable[0].EditSalaryRange();

                double newAnnualSalary = 100;
                string newNotes = "Update Notes";

                salaryRangeDialog.AnnualSalary = newAnnualSalary.ToString();
                var endDate = DateTime.Today.AddDays(5).ToShortDateString();
                var nextStartDate = DateTime.Today.AddDays(10).ToShortDateString();
                salaryRangeDialog.EndDate = endDate;
                salaryRangeDialog.Notes = newNotes;
                salaryRangeDialog.ClickOk();
                salaryRangeDialog.EditWarningDialog();
                editContractDialog.ClickOk();

                staffRecord.SaveStaff();

                // Reload and assert
                StaffRecordPage newStaffRecord = StaffRecordPage.LoadStaffDetail(staffId);
                newStaffRecord.ContractsTable.Rows[0].ClickEdit();

                EditContractDialog newEditContractDialog = new EditContractDialog();
                AddContractDetailDialog addContractDialog = new AddContractDetailDialog();
                SelectSalaryRangeDialog addNewsalaryRangeDialog = addContractDialog.ClickAddSalaryRange();
                addNewsalaryRangeDialog.SalaryRange = salaryRangeDescription;
                addNewsalaryRangeDialog.AnnualSalary = newAnnualSalary.ToString();
                addNewsalaryRangeDialog.StartDate = nextStartDate;
                addNewsalaryRangeDialog.Notes = newNotes;
                addNewsalaryRangeDialog.ClickOk();
                editContractDialog.ClickOk();

                List<string> validation = addContractDialog.Validation.ToList();
                Assert.IsTrue(validation.Contains("There cannot be a gap between the end date of a salary record and the start date of the next salary record."));
            }
        }

        private void Check_Emplyment_Contract_Salary_Record_Exists_Within_Range()
        {
            Guid employmentContractId, serviceTermId, salaryRangeId, staffId, employmentContractSlaryRangeId, employeeId, PayScaleId, postTypeId;
            string serviceTermDescription, salaryRangeCode, salaryRangeDescription,
                payLevelCode, payLevelDescription, regionalWeightingDescription,
                postTypeDescription;

            DataPackage initialData = SetupTestData(
                                out employmentContractId,
                                out serviceTermId,
                                out salaryRangeId,
                                out staffId,
                                out employeeId,
                                out postTypeId,
                                out serviceTermDescription,
                                out salaryRangeCode,
                                out salaryRangeDescription,
                                out payLevelCode,
                                out payLevelDescription,
                                out regionalWeightingDescription,
                                out postTypeDescription
                                );

            initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));

            decimal salary = 123456;
            string notes = Utilities.GenerateRandomString(200);
            DateTime? expectedStartDate = DateTime.Now;
            initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                    out employmentContractSlaryRangeId,
                    employmentContractId,
                    salaryRangeId,
                    salary,
                    expectedStartDate,
                    null,
                    false,
                    false,
                    notes)
                );

            using (new DataSetup(initialData))
            {
                InitialiseToStaffRecordPage();

                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(staffId);

                staffRecord.ContractsTable.Rows[0].ClickEdit();
                EditContractDialog editContractDialog = new EditContractDialog();

                SelectSalaryRangeDialog salaryRangeDialog = editContractDialog.SalaryRangesTable[0].EditSalaryRange();

                double newAnnualSalary = 100;
                string newNotes = "Update Notes";

                salaryRangeDialog.AnnualSalary = newAnnualSalary.ToString();
                var endDate = DateTime.Today.AddDays(5).ToShortDateString();
                salaryRangeDialog.StartDate = DateTime.Today.AddDays(-1).ToShortDateString();
                salaryRangeDialog.EndDate = endDate;
                salaryRangeDialog.Notes = newNotes;
                salaryRangeDialog.ClickOk();
                salaryRangeDialog.EditWarningDialog();
                editContractDialog.ClickOk();

                List<string> validation = editContractDialog.Validation.ToList();
                Assert.IsTrue(validation.Contains("Salary Range dates must be within the date range of the Contract."));
            }
        }
    }
}
