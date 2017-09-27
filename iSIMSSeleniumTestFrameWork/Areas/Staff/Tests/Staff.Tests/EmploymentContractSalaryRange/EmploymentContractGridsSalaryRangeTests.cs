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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.Tests.EmploymentContractGridsSalaryRange
{
    [TestClass]
    public class EmploymentContractGridsSalaryRangeTests
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
        private enum ServiceTermType
        {
            WithSalaryRanges,
            WithPayScales,
            WithSalaryRangesAndPayScales
    }

    /// <summary>
    /// Add_new_Employment_Contract_for_Service_Term_containing_Salary_Ranges_as_PO()
    /// Pre-condition: When adding a new Employment Contract, the selecetd service term has at least one Salary Ranges record.
    /// Expected result: Employment Contact should only display the Salary Range grid (Pay Scales grid should not be shown)
    /// </summary>
    //[TestMethod][ChromeUiTest("EmploymentContractGridsSalaryRangeTests", "P1", "Add_new_Employment_Contract_for_Service_Term_containing_Salary_Ranges_as_PO")]
    public void Add_new_Employment_Contract_for_Service_Term_containing_Salary_Ranges_as_PO()
    {
        ServiceTermDetails serviceTermDetails = new ServiceTermDetails();

        using (new DataSetup(Create_new_Service_Term(ServiceTermType.WithSalaryRanges, serviceTermDetails)))
        {
            InitialiseToStaffRecordPage();
            StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(serviceTermDetails.StaffId);

            staffRecord.ClickAddContract();
            AddContractDetailDialog addContractDialog = new AddContractDetailDialog();
            addContractDialog.ServiceTermCombobox = serviceTermDetails.ServiceTermDescription;

            addContractDialog.ClickOk();
            List<string> validation = addContractDialog.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("At least one Salary Range record should be present."), "Validation not shown: 'At least one Salary Range record should be present.'");
            Assert.IsTrue(addContractDialog.DoesSalaryRangeAddButtonExist(), "Add button on Salary Range grid not found (Salary Range grid does not exist)");
            Assert.IsFalse(addContractDialog.DoesPayScaleAddButtonExist(), "Add button on Pay Scale grid should not be present (Pay Scale grid should not be displayed)");
        }
    }

    /// <summary>
    /// Add_new_Employment_Contract_for_Service_Term_containing_Pay_Scales_as_PO()
    /// Pre-condition: When adding a new Employment Contract, the selecetd service term has at least one Pay Scales record.
    /// Expected result: Employment Contact should only display the Pay Scales grid (Salary Ranges grid should not be shown)
    /// </summary>
    //[TestMethod][ChromeUiTest("EmploymentContractGridsSalaryRangeTests", "P1", "Add_new_Employment_Contract_for_Service_Term_containing_Pay_Scales_as_PO")]
    public void Add_new_Employment_Contract_for_Service_Term_containing_Pay_Scales_as_PO()
    {
        ServiceTermDetails serviceTermDetails = new ServiceTermDetails();

        using (new DataSetup(Create_new_Service_Term(ServiceTermType.WithPayScales, serviceTermDetails)))
        {
            InitialiseToStaffRecordPage();
            StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(serviceTermDetails.StaffId);

            staffRecord.ClickAddContract();
            AddContractDetailDialog addContractDialog = new AddContractDetailDialog();
            addContractDialog.ServiceTermCombobox = serviceTermDetails.ServiceTermDescription;

            addContractDialog.ClickOk();
            List<string> validation = addContractDialog.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("At least one Pay Scale record should be present."), "Validation not shown: 'At least one Pay Scale record should be present.'");
            Assert.IsTrue(addContractDialog.DoesPayScaleAddButtonExist(), "Add button on Pay Scale grid not found (Pay Scale grid does not exist)");
            Assert.IsFalse(addContractDialog.DoesSalaryRangeAddButtonExist(), "Add button on Salary Range grid should not be present (Salary Range grid should not be displayed)");
        }
    }

    /// <summary>
    /// Add_new_Employment_Contract_for_Service_Term_containing_Pay_Scales_and_Salary_Ranges_as_PO()
    /// Pre-condition: When adding a new Employment Contract, the selecetd service term has at least one Salary Ranges record.
    /// Expected result: Employment Contact should only display the Salary Range grid (Pay Scales grid should not be shown)
    /// </summary>
    //[TestMethod][ChromeUiTest("EmploymentContractGridsSalaryRangeTests", "P1", "Add_new_Employment_Contract_for_Service_Term_containing_Pay_Scales_and_Salary_Ranges_as_PO")]
    public void Add_new_Employment_Contract_for_Service_Term_containing_Pay_Scales_and_Salary_Ranges_as_PO()
    {
        ServiceTermDetails serviceTermDetails = new ServiceTermDetails();

        using (new DataSetup(Create_new_Service_Term(ServiceTermType.WithSalaryRangesAndPayScales, serviceTermDetails)))
        {
            InitialiseToStaffRecordPage();
            StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(serviceTermDetails.StaffId);

            staffRecord.ClickAddContract();
            AddContractDetailDialog addContractDialog = new AddContractDetailDialog();
            addContractDialog.ServiceTermCombobox = serviceTermDetails.ServiceTermDescription;

            addContractDialog.ClickOk();
            List<string> validation = addContractDialog.Validation.ToList();

            Assert.IsTrue(validation.Contains("At least one Salary Range record should be present."), "Validation not shown: 'At least one Salary Range record should be present.'");
            Assert.IsTrue(addContractDialog.DoesSalaryRangeAddButtonExist(), "Add button on Salary Range grid not found (Salary Range grid does not exist)");
            Assert.IsFalse(addContractDialog.DoesPayScaleAddButtonExist(), "Add button on Pay Scale grid should not be present (Pay Scale grid should not be displayed)");
        }
    }

    /// <summary>
    /// Edit_Employment_Contract_using_Service_Term_containing_Salary_Ranges_only_as_PO()
    /// Pre-condition: Employment Contract contains a service term that has at least one Salary Range record but no Pay Scales records.
    /// Expected result: Employment Contact should only display the Salary Range grid (Pay Scales grid should not be shown)
    /// </summary>
    //[TestMethod][ChromeUiTest("EmploymentContractGridsSalaryRangeTests", "P1", "Edit_Employment_Contract_using_Service_Term_containing_Salary_Ranges_only_as_PO")]
    public void Edit_Employment_Contract_using_Service_Term_containing_Salary_Ranges_only_as_PO()
    {
        ServiceTermDetails serviceTermDetails = new ServiceTermDetails();

        DataPackage initialData = Create_new_Service_Term(ServiceTermType.WithSalaryRanges, serviceTermDetails);
        initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out serviceTermDetails.EmploymentContractId,
                        serviceTermDetails.ServiceTermId, serviceTermDetails.EmployeeId, DateTime.Today));

        //Add Salary Range row to employment contract
        decimal salary = 123456;
        string notes = Utilities.GenerateRandomString(200);
        DateTime? expectedStartDate = DateTime.Now;
        initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                out serviceTermDetails.EmploymentContractSalaryRangeId,
                serviceTermDetails.EmploymentContractId,
                serviceTermDetails.SalaryRangeId,
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

            StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(serviceTermDetails.StaffId);

            staffRecord.ContractsTable.Rows[0].ClickEdit();
            EditContractDialog editContractDialog = new EditContractDialog();

            var salaryRangesGrid = editContractDialog.SalaryRangesTable;
            var payScalesGrid = editContractDialog.PayScalesTable;

            Assert.IsNotNull(salaryRangesGrid, "Salary Range grid not found - it should be visible.");
            Assert.IsTrue(salaryRangesGrid.LastInsertRow.IsAddNewRecordButtonEnabled, "Add Salary Range button on Salary Range grid should be enabled but is disabled.");
            Assert.IsNull(payScalesGrid, "Pay Scales grid should not be found but it was.");
        }
    }

    /// <summary>
    /// Edit_Employment_Contract_using_Service_Term_containing_Pay_Scales_only_as_PO()
    /// Pre-condition: Employment Contract contains a service term that has at least one Pay Scale but no Salary Range records.
    /// Expected result: Employment Contact should only display the Pay Scale grid (Salary Range grid should not be shown)
    /// </summary>
    //[TestMethod][ChromeUiTest("EmploymentContractGridsSalaryRangeTests", "P1", "Edit_Employment_Contract_using_Service_Term_containing_Pay_Scales_only_as_PO")]
    public void Edit_Employment_Contract_using_Service_Term_containing_Pay_Scales_only_as_PO()
    {
        ServiceTermDetails serviceTermDetails = new ServiceTermDetails();

        DataPackage initialData = Create_new_Service_Term(ServiceTermType.WithPayScales, serviceTermDetails);
        initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out serviceTermDetails.EmploymentContractId,
                        serviceTermDetails.ServiceTermId, serviceTermDetails.EmployeeId, DateTime.Today));

        //Add Pay Scale row to employment contract
        initialData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(
                out serviceTermDetails.EmploymentContractPayScaleId,
                serviceTermDetails.PayScaleId,
                serviceTermDetails.EmploymentContractId,
                DateTime.Today,
                point: 1m
            ));

        using (new DataSetup(initialData))
        {
            InitialiseToStaffRecordPage();

            StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(serviceTermDetails.StaffId);

            staffRecord.ContractsTable.Rows[0].ClickEdit();
            EditContractDialog editContractDialog = new EditContractDialog();

            var salaryRangesGrid = editContractDialog.SalaryRangesTable;
            var payScalesGrid = editContractDialog.PayScalesTable;

            Assert.IsNotNull(payScalesGrid, "Pay Scales grid not found - it should be visible.");
            Assert.IsTrue(payScalesGrid.LastInsertRow.IsAddNewRecordButtonEnabled, "Add Pay Scale button on Pay Scale grid should be enabled but is disabled.");
            Assert.IsNull(salaryRangesGrid, "Salary Range grid should not be found but it was.");
        }
    }

    // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Check_Emplyment_Contract_Pay_Level_Overlap_as_PO")]
    public void Check_Emplyment_Contract_Pay_Level_Date_Overlap_as_PO()
    {
        Check_Emplyment_Contract_Pay_Level_Date_Overlap();
    }

    // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Check_Emplyment_Contract_Pay_Level_Sequential_as_PO")]
    public void Check_Emplyment_Contract_Pay_Level_Sequential_as_PO()
    {
        Check_Emplyment_Contract_Pay_Level_Sequential();
    }

    // [TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Check_Emplyment_Contract_Pay_Scale_And_Salary_Range_Sequential_Validation_as_PO")]
    public void Check_Emplyment_Contract_Pay_Scale_And_Salary_Range_Sequential_Validation_as_PO()
    {
        Check_Emplyment_Contract_Pay_Scale_And_Salary_Range_Sequential_Validation();
    }

    //[TestMethod][ChromeUiTest("EmploymentContractSalaryRangeTests", "P1", "Check_Emplyment_Contract_Pay_Scale_And_Salary_Range_EndDate_MustExists_Validation_as_PO")]
    public void Check_Emplyment_Contract_Pay_Scale_And_Salary_Range_EndDate_MustExists_Validation_as_PO()
    {
        Check_Emplyment_Contract_Pay_Scale_And_Salary_Range_EndDate_MustExists_Validation();
    }

    public void Check_Emplyment_Contract_Pay_Scale_And_Salary_Range_EndDate_MustExists_Validation()
    {
        ServiceTermDetails serviceTermDetails = new ServiceTermDetails();

        DataPackage initialData = Create_new_Service_Term(ServiceTermType.WithSalaryRangesAndPayScales, serviceTermDetails);
        initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out serviceTermDetails.EmploymentContractId,
                        serviceTermDetails.ServiceTermId, serviceTermDetails.EmployeeId, DateTime.Today));

        //Add Salary Range row to employment contract
        decimal salary = 123456;
        string notes = Utilities.GenerateRandomString(200);
        DateTime? expectedStartDate = DateTime.Now;
        initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                out serviceTermDetails.EmploymentContractSalaryRangeId,
                serviceTermDetails.EmploymentContractId,
                serviceTermDetails.SalaryRangeId,
                salary,
                expectedStartDate,
                null,
                false,
                false,
                notes)
            );

        //Add Pay Scale row to employment contract
        initialData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(
                out serviceTermDetails.EmploymentContractPayScaleId,
                serviceTermDetails.PayScaleId,
                serviceTermDetails.EmploymentContractId,
                DateTime.Today,
                null,
                point: 1m
            ));

        using (new DataSetup(initialData))
        {
            InitialiseToStaffRecordPage();
            StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(serviceTermDetails.StaffId);
            staffRecord.ContractsTable.Rows[0].ClickEdit();
            EditContractDialog editContractDialog = new EditContractDialog();
            editContractDialog.ClickOk();

            List<string> validation = editContractDialog.Validation.ToList();
            Assert.IsTrue(validation.Contains("A Pay Scale end date must be entered before adding a new Salary Range Salary Record."));
        }
    }

    public void Check_Emplyment_Contract_Pay_Scale_And_Salary_Range_Sequential_Validation()
    {
        ServiceTermDetails serviceTermDetails = new ServiceTermDetails();

        DataPackage initialData = Create_new_Service_Term(ServiceTermType.WithSalaryRangesAndPayScales, serviceTermDetails);
        initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out serviceTermDetails.EmploymentContractId,
                        serviceTermDetails.ServiceTermId, serviceTermDetails.EmployeeId, DateTime.Today));

        //Add Salary Range row to employment contract
        decimal salary = 123456;
        string notes = Utilities.GenerateRandomString(200);
        DateTime? expectedStartDate = DateTime.Now;
        initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                out serviceTermDetails.EmploymentContractSalaryRangeId,
                serviceTermDetails.EmploymentContractId,
                serviceTermDetails.SalaryRangeId,
                salary,
                expectedStartDate,
                null,
                false,
                false,
                notes)
            );

        //Add Pay Scale row to employment contract
        initialData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(
                out serviceTermDetails.EmploymentContractPayScaleId,
                serviceTermDetails.PayScaleId,
                serviceTermDetails.EmploymentContractId,
                DateTime.Today,
                DateTime.Today.AddDays(10),
                point: 1m
            ));

        using (new DataSetup(initialData))
        {
            InitialiseToStaffRecordPage();
            StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(serviceTermDetails.StaffId);
            staffRecord.ContractsTable.Rows[0].ClickEdit();
            EditContractDialog editContractDialog = new EditContractDialog();
            editContractDialog.ClickOk();

            List<string> validation = editContractDialog.Validation.ToList();
            Assert.IsTrue(validation.Contains("There cannot be a gap between the end date of a salary record and the start date of the next salary record."));
        }
    }

    public void Check_Emplyment_Contract_Pay_Level_Date_Overlap()
    {
        ServiceTermDetails serviceTermDetails = new ServiceTermDetails();

        DataPackage initialData = Create_new_Service_Term(ServiceTermType.WithPayScales, serviceTermDetails);
        initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out serviceTermDetails.EmploymentContractId,
                        serviceTermDetails.ServiceTermId, serviceTermDetails.EmployeeId, DateTime.Today));

        //Add Pay Scale row to employment contract
        initialData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(
                out serviceTermDetails.EmploymentContractPayScaleId,
                serviceTermDetails.PayScaleId,
                serviceTermDetails.EmploymentContractId,
                DateTime.Today,
                DateTime.Today.AddDays(5),
                point: 1m
            ));

        //Add second Pay Scale row to employment contract
        initialData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(
                out serviceTermDetails.EmploymentContractPayScaleId,
                serviceTermDetails.PayScaleId,
                serviceTermDetails.EmploymentContractId,
                DateTime.Today.AddDays(5),
                point: 1m
            ));

        using (new DataSetup(initialData))
        {
            InitialiseToStaffRecordPage();

            StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(serviceTermDetails.StaffId);

            staffRecord.ContractsTable.Rows[0].ClickEdit();
            EditContractDialog editContractDialog = new EditContractDialog();
            editContractDialog.ClickOk();

            List<string> validation = editContractDialog.Validation.ToList();
            Assert.IsTrue(validation.Contains("Pay Scales cannot overlap."));
        }
    }

    public void Check_Emplyment_Contract_Pay_Level_Sequential()
    {
        ServiceTermDetails serviceTermDetails = new ServiceTermDetails();

        DataPackage initialData = Create_new_Service_Term(ServiceTermType.WithPayScales, serviceTermDetails);
        initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out serviceTermDetails.EmploymentContractId,
                        serviceTermDetails.ServiceTermId, serviceTermDetails.EmployeeId, DateTime.Today));

        //Add Pay Scale row to employment contract
        initialData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(
                out serviceTermDetails.EmploymentContractPayScaleId,
                serviceTermDetails.PayScaleId,
                serviceTermDetails.EmploymentContractId,
                DateTime.Today,
                 DateTime.Today.AddDays(5),
                point: 1m
            ));

        //Add second Pay Scale row to employment contract
        initialData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(
                out serviceTermDetails.EmploymentContractPayScaleId,
                serviceTermDetails.PayScaleId,
                serviceTermDetails.EmploymentContractId,
                DateTime.Today.AddDays(10),
                point: 1m
            ));

        using (new DataSetup(initialData))
        {
            InitialiseToStaffRecordPage();

            StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(serviceTermDetails.StaffId);

            staffRecord.ContractsTable.Rows[0].ClickEdit();
            EditContractDialog editContractDialog = new EditContractDialog();
            editContractDialog.ClickOk();

            List<string> validation = editContractDialog.Validation.ToList();
            Assert.IsTrue(validation.Contains("There cannot be a gap between the end date of a salary record and the start date of the next salary record."));
        }
    }

    /// <summary>
    /// Edit_Employment_Contract_using_Service_Term_containing_Pay_Scales_and_Salary_Range_as_PO()
    /// Pre-condition: Employment Contract contains a service term that has at least one Pay Scale and at least one Salary Range record.
    /// Expected result: Employment Contact should display both the Pay Scale grid and the Salary Range grid.
    ///                  Salary Range grid "Add" button should be enabled. Pay Scale grid "Add" button should be disabled. 
    ///                  Pay Scale row delete button should be disabled.
    /// </summary>
    //[TestMethod][ChromeUiTest("EmploymentContractGridsSalaryRangeTests", "P1", "Edit_Employment_Contract_using_Service_Term_containing_Pay_Scales_and_Salary_Range_as_PO")]
    public void Edit_Employment_Contract_using_Service_Term_containing_Pay_Scales_and_Salary_Range_as_PO()
    {
        ServiceTermDetails serviceTermDetails = new ServiceTermDetails();

        DataPackage initialData = Create_new_Service_Term(ServiceTermType.WithSalaryRangesAndPayScales, serviceTermDetails);
        initialData.Add("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out serviceTermDetails.EmploymentContractId,
                        serviceTermDetails.ServiceTermId, serviceTermDetails.EmployeeId, DateTime.Today));

        //Add Salary Range row to employment contract
        decimal salary = 123456;
        string notes = Utilities.GenerateRandomString(200);
        DateTime? expectedStartDate = DateTime.Now;
        initialData.Add("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(
                out serviceTermDetails.EmploymentContractSalaryRangeId,
                serviceTermDetails.EmploymentContractId,
                serviceTermDetails.SalaryRangeId,
                salary,
                expectedStartDate,
                null,
                false,
                false,
                notes)
            );

        //Add Pay Scale row to employment contract
        initialData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(
                out serviceTermDetails.EmploymentContractPayScaleId,
                serviceTermDetails.PayScaleId,
                serviceTermDetails.EmploymentContractId,
                DateTime.Today,
                point: 1m
            ));

        using (new DataSetup(initialData))
        {
            InitialiseToStaffRecordPage();

            StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(serviceTermDetails.StaffId);

            staffRecord.ContractsTable.Rows[0].ClickEdit();
            EditContractDialog editContractDialog = new EditContractDialog();

            var salaryRangesGrid = editContractDialog.SalaryRangesTable;
            var payScalesGrid = editContractDialog.PayScalesTable;

            Assert.IsNotNull(salaryRangesGrid, "Salary Ranges grid not found - it should be visible.");
            Assert.IsTrue(salaryRangesGrid.LastInsertRow.IsAddNewRecordButtonEnabled, "Add New Salary Range button on Salary Range grid should be enabled but is disabled.");
            Assert.IsTrue(salaryRangesGrid.LastInsertRow.IsRowEditButtonEnabled, "Edit row button on Salary Range grid should be enabled but is disabled.");
            Assert.IsTrue(salaryRangesGrid.LastInsertRow.IsRowDeleteButtonEnabled, "Delete row button on Salary Range grid should be enabled but is disabled.");

            Assert.IsNotNull(payScalesGrid, "Pay Scales grid not found - it should be visible.");
            Assert.IsFalse(payScalesGrid.LastInsertRow.IsAddNewRecordButtonEnabled, "Add New Pay Scale button on Pay Scale grid should be disabled but is enabled.");
            Assert.IsTrue(payScalesGrid.LastInsertRow.IsRowEditButtonEnabled, "Edit row button on Pay Scale grid should be enabled but is disabled.");
            Assert.IsFalse(payScalesGrid.LastInsertRow.IsRowDeleteButtonEnabled, "Delete row button on Pay Scale grid should be disabled but is enabled.");
        }
    }

    private DataPackage Create_new_Service_Term(ServiceTermType serviceTermType, ServiceTermDetails serviceTermDetails)
    {
        bool omitSalaryRanges = false;
        bool omitPayScales = false;

        if (serviceTermType != ServiceTermType.WithSalaryRangesAndPayScales)
        {
            omitSalaryRanges = serviceTermType == ServiceTermType.WithPayScales;
            omitPayScales = serviceTermType == ServiceTermType.WithSalaryRanges;
        }

        return SetupTestData(
                    serviceTermDetails,
                    omitPayScales: omitPayScales,
                    omitSalaryRanges: omitSalaryRanges
                    );
    }

    private void InitialiseToStaffRecordPage()
    {
        SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
        AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
        AutomationSugar.WaitForAjaxCompletion();
    }

    private static DataPackage SetupTestData(ServiceTermDetails serviceTermDetails, bool omitPayScales = false, bool omitSalaryRanges = false)
    {
        DataPackage testData = new DataPackage();

        Guid paySpineId, payAwardId, postTypeId, statutoryPostTypeId, serviceTermsPostTypeId;
        string postTypeCode = Utilities.GenerateRandomString(4);
        string paySpineCode = Utilities.GenerateRandomString(4);
        string serviceTermCode = Utilities.GenerateRandomString(2);
        decimal weeksPerYear = 50m, hoursPerWeek = 40m;

        DateTime awardDateTime = DateTime.Now;
        string awardDate = awardDateTime.ToString("dd/MM/yyyy");
        decimal maximumAmount = 200, minimumAmount = 20;
        string regionalWeightingCode = Utilities.GenerateRandomString(20);

        Guid payLevelId, regionalWeightingId, salaryAwardid;
        Guid serviceTermSalaryRangeId;
        Guid serviceRecordId;
        string staffSurname = Utilities.GenerateRandomString(100);
        string staffForename = Utilities.GenerateRandomString(100);

        serviceTermDetails.EmploymentContractId = Guid.Empty;
        serviceTermDetails.PostTypeDescription = Utilities.GenerateRandomString(20);
        serviceTermDetails.ServiceTermDescription = Utilities.GenerateRandomString(20);
        serviceTermDetails.SalaryRangeCode = Utilities.GenerateRandomString(8);
        serviceTermDetails.SalaryRangeDescription = Utilities.GenerateRandomString(100);
        serviceTermDetails.PayLevelCode = Utilities.GenerateRandomString(20);
        serviceTermDetails.PayLevelDescription = Utilities.GenerateRandomString(20);
        serviceTermDetails.RegionalWeightingDescription = Utilities.GenerateRandomString(20);

        testData = new DataPackage();

        testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermDetails.ServiceTermId, serviceTermCode, serviceTermDetails.ServiceTermDescription, weeksPerYear, hoursPerWeek));

        testData.AddData("PayLevel", DataPackageHelper.GeneratePayLevel(out payLevelId, serviceTermDetails.PayLevelCode, serviceTermDetails.PayLevelDescription));
        testData.AddData("RegionalWeighting", DataPackageHelper.GenerateRegionalWeighting(out regionalWeightingId, regionalWeightingCode, serviceTermDetails.RegionalWeightingDescription));

        if (!omitSalaryRanges)
        {
            testData.AddData("SalaryRange", DataPackageHelper.GenerateSalaryRange(out serviceTermDetails.SalaryRangeId, serviceTermDetails.SalaryRangeCode, serviceTermDetails.SalaryRangeDescription, payLevelId, regionalWeightingId));
            testData.AddData("SalaryAward", DataPackageHelper.GenerateSalaryAward(out salaryAwardid, awardDateTime, maximumAmount, minimumAmount, serviceTermDetails.SalaryRangeId));
            testData.AddData("ServiceTermSalaryRange", DataPackageHelper.GenerateServiceTermSalaryRange(out serviceTermSalaryRangeId, serviceTermDetails.ServiceTermId, serviceTermDetails.SalaryRangeId));
        }
        else
        {
            serviceTermDetails.SalaryRangeId = Guid.Empty;
        }

        testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
        testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
        if (!omitPayScales)
        {
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out serviceTermDetails.PayScaleId, serviceTermDetails.ServiceTermId, paySpineId));
        }
        testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
        testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: serviceTermDetails.PostTypeDescription, statutoryPostTypeId : statutoryPostTypeId));
        testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermDetails.ServiceTermId));

        testData.AddData("Employee", DataPackageHelper.GenerateEmployee(out serviceTermDetails.EmployeeId));
        testData.AddData("Staff", DataPackageHelper.GenerateStaff(out serviceTermDetails.StaffId, staffSurname, serviceTermDetails.EmployeeId, forename: staffForename));
        testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, serviceTermDetails.StaffId, DateTime.Now, null));

        return testData;
    }
}

internal class ServiceTermDetails
{
    public Guid EmploymentContractId, ServiceTermId, SalaryRangeId, StaffId, EmployeeId, EmploymentContractSalaryRangeId,
                EmploymentContractPayScaleId, PayScaleId;
    public string ServiceTermDescription, SalaryRangeCode, SalaryRangeDescription, PayLevelCode, PayLevelDescription,
                  RegionalWeightingDescription, PostTypeDescription;
}

}
