using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Base;
using Staff.POM.Components.Staff;
using Staff.POM.Components.Staff.Dialogs;
using Staff.POM.Helper;
using System;

namespace Staff.Tests.StaffRecord
{
    [TestClass]
    public class StaffContractsTests
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
        [ChromeUiTest("StaffContracts", "Create_basic_Contract_using_existing_Service_Term_and_Staff_as_PO")]
        public void Create_basic_Contract_using_existing_Service_Term_and_Staff_as_PO()
        {
            Guid staffId, serviceTermId, serviceRecordId;
            Guid paySpineId, payAwardId, payscaleId;
            Guid postTypeId, statutoryPostTypeId, serviceTermsPostTypeId;

            string serviceTermCode = CoreQueries.GetColumnUniqueString("ServiceTerm", "Code", 2);
            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string payScaleDescription = CoreQueries.GetColumnUniqueString("PayScale", "Description", 20);
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");
            string statutoryOrigin = "Break for family reasons";
            string staffRoleDescription = "Other School Admin";

            DateTime staffStartDate = DateTime.Today;
            decimal weeksPerYear = 50m, hoursPerWeek = 40m, annualLeaveEntitlementDays = 25m;

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, serviceTermCode, serviceTermDescription, weeksPerYear, hoursPerWeek));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId, description: payScaleDescription));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname, forename: staffForename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, staffStartDate, null));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                staffRecordTriplet.SearchCriteria.IsCurrent = true;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();
                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                AddContractDetailDialog addContractDetailDialog = staffRecord.ClickAddContract();
                addContractDetailDialog.ServiceTermCombobox = serviceTermDescription;
                addContractDetailDialog.EmploymentTypeCombobox = "Permanent";
                addContractDetailDialog.PostTypeCombobox = postTypeDescription;
                addContractDetailDialog.AnnualLeaveEntitlementDays = annualLeaveEntitlementDays.ToString();
                addContractDetailDialog.EmploymentContractOrigin = statutoryOrigin;

                addContractDetailDialog.ClickAddStaffRole();
                var gridRow = addContractDetailDialog.StaffRoles.Rows[0];
                gridRow.StaffRole = staffRoleDescription;
                gridRow.StartDate = staffStartDate.ToShortDateString();

                SelectPayScaleDialog payScaleDialog = addContractDetailDialog.ClickAddPayScale();
                payScaleDialog.ScaleField = payScaleDescription;
                payScaleDialog.PointField = 1m.ToString();
                payScaleDialog.ClickOk();

                addContractDetailDialog.ClickOk();
                staffRecordTriplet.ClickSave();

                Assert.IsTrue(staffRecord.IsSuccessMessageDisplayed());
                Assert.IsNotNull(staffRecord.ContractsTable.Rows);
                Assert.AreEqual(1, staffRecord.ContractsTable.Rows.Count);

                EditContractDialog editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();

                Assert.AreEqual(statutoryOrigin, editContractDialog.EmploymentContractOrigin);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffContracts", "P1", "Read_Contract_from_existing_Staff_as_PO")]
        public void Read_Contract_from_existing_Staff_as_PO()
        {
            Guid staffId, employmentContractId, employeeId, serviceTermId, serviceRecordId;
            Guid paySpineId, payAwardId, payScaleId, employmentContractPayScaleId;
            Guid postTypeId, statutoryPostTypeId, serviceTermsPostTypeId;
            Guid employmentContractRoleId;

            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string payScaleDescription = CoreQueries.GetColumnUniqueString("PayScale", "Description", 20);
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");

            DateTime staffStartDate = DateTime.Today;

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, description: serviceTermDescription));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payScaleId, serviceTermId, paySpineId, description: payScaleDescription));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));
            testData.AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname, employeeId, forename: staffForename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, staffStartDate));
            testData.AddData("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));
            testData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(out employmentContractPayScaleId, payScaleId, employmentContractId, staffStartDate));
            testData.AddData("EmploymentContractRole", DataPackageHelper.GenerateEmploymentContractStaffRole(out employmentContractRoleId, employmentContractId, staffStartDate));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                Assert.AreEqual(1, searchResultTiles.Count());

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                Assert.IsNotNull(staffRecord.ContractsTable.Rows);
                Assert.AreEqual(1, staffRecord.ContractsTable.Rows.Count);
                Assert.AreEqual(serviceTermDescription, staffRecord.ContractsTable.Rows[0].ServiceTerm);
                Assert.AreEqual(staffStartDate.ToShortDateString(), staffRecord.ContractsTable.Rows[0].StartDate);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffContracts", "P1", "Update_existing_Contract_from_Staff_as_PO")]
        public void Update_existing_Contract_from_Staff_as_PO()
        {
            Guid staffId, employmentContractId, employeeId, serviceTermId, serviceRecordId;
            Guid paySpineId, payAwardId, payScaleId, employmentContractPayScaleId;
            Guid postTypeId, statutoryPostTypeId, serviceTermsPostTypeId;
            Guid employmentContractRoleId;

            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string payScaleDescription = CoreQueries.GetColumnUniqueString("PayScale", "Description", 20);
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");
            string newEmploymentType = "Fixed Term";
            string newStatutoryOrigin = "Break for family reasons";
            string newStatutoryDestination = "Sixth form college - same LA area";

            DateTime staffStartDate = DateTime.Today, staffEndDate = DateTime.Today.AddDays(10);

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, description: serviceTermDescription));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payScaleId, serviceTermId, paySpineId, description: payScaleDescription));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));
            testData.AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname, employeeId, forename: staffForename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, staffStartDate, null));
            testData.AddData("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, employmentTypeCode: "PRM", statutoryOriginCode: "OTHERR", statutoryDestinationCode: "OTHERR", postTypeId: postTypeId));
            testData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(out employmentContractPayScaleId, payScaleId, employmentContractId, staffStartDate, point: 1m));
            testData.AddData("EmploymentContractRole", DataPackageHelper.GenerateEmploymentContractStaffRole(out employmentContractRoleId, employmentContractId, staffStartDate));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                Assert.AreEqual(1, searchResultTiles.Count());

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                EditContractDialog editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                editContractDialog.EmploymentTypeCombobox = newEmploymentType;
                editContractDialog.EndDate = staffEndDate.ToShortDateString();
                editContractDialog.EmploymentContractOrigin = newStatutoryOrigin;
                editContractDialog.EmploymentContractDestination = newStatutoryDestination;
                editContractDialog.ClickOk();

                ConfirmRequiredDialog endContractConfirmationDialog = new ConfirmRequiredDialog();
                endContractConfirmationDialog.ClickYes();

                staffRecord.SaveStaff();

                Assert.IsTrue(staffRecord.IsSuccessMessageDisplayed());
                Assert.AreEqual(serviceTermDescription, staffRecord.ContractsTable.Rows[0].ServiceTerm);
                Assert.AreEqual(staffStartDate.ToShortDateString(), staffRecord.ContractsTable.Rows[0].StartDate);

                editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();

                Assert.AreEqual(staffStartDate.ToShortDateString(), editContractDialog.StartDate);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.EndDate);
                Assert.AreEqual(newEmploymentType, editContractDialog.EmploymentTypeCombobox);
                Assert.AreEqual(newStatutoryOrigin, editContractDialog.EmploymentContractOrigin);
                Assert.AreEqual(newStatutoryDestination, editContractDialog.EmploymentContractDestination);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.PayScalesTable.Rows[0].EndDate);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffContracts", "P1", "Terminate_Contract_With_Dated_Records_With_Pay_Scales_from_Staff_as_PO")]
        public void Terminate_Contract_With_Dated_Records_With_Pay_Scales_from_Staff_as_PO()
        {
            Guid staffId, employmentContractId, employeeId, serviceTermId, serviceRecordId;
            Guid paySpineId, payAwardId, payScaleId, employmentContractPayScaleId;
            Guid postTypeId, statutoryPostTypeId, serviceTermsPostTypeId;
            Guid addPayCatId, allowanceId, serviceTermAllowanceId, ecAllowanceId;
            Guid employmentContractStaffRoleId;

            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string payScaleDescription = CoreQueries.GetColumnUniqueString("PayScale", "Description", 20);
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");
            string newEmploymentType = "Fixed Term";
            string newStatutoryOrigin = "Break for family reasons";
            string newStatutoryDestination = "Sixth form college - same LA area";

            DateTime staffStartDate = DateTime.Today, staffEndDate = DateTime.Today.AddDays(10);

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, description: serviceTermDescription));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payScaleId, serviceTermId, paySpineId, description: payScaleDescription));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));
            testData.AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname, employeeId, forename: staffForename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, staffStartDate, null));
            testData.AddData("AdditionalPaymentCategory", DataPackageHelper.GenerateAdditionalPaymentCategory(out addPayCatId));
            testData.AddData("Allowance", DataPackageHelper.GenerateAllowance(out allowanceId, addPayCatId));
            testData.AddData("ServiceTermAllowance", DataPackageHelper.GenerateServiceTermAllowance(out serviceTermAllowanceId, allowanceId, serviceTermId));

            testData.AddData("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, employmentTypeCode: "PRM", statutoryOriginCode: "OTHERR", statutoryDestinationCode: "OTHERR", postTypeId: postTypeId));
            testData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(out employmentContractPayScaleId, payScaleId, employmentContractId, staffStartDate, point: 1m));
            testData.AddData("EmploymentContractAllowance", DataPackageHelper.GenerateEmploymentContractAllowance(out ecAllowanceId, allowanceId, employmentContractId, staffStartDate));
            testData.AddData("EmploymentContractRole", DataPackageHelper.GenerateEmploymentContractStaffRole(out employmentContractStaffRoleId, employmentContractId, staffStartDate));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                Assert.AreEqual(1, searchResultTiles.Count());

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                EditContractDialog editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                editContractDialog.EmploymentTypeCombobox = newEmploymentType;
                editContractDialog.EndDate = staffEndDate.ToShortDateString();
                editContractDialog.EmploymentContractOrigin = newStatutoryOrigin;
                editContractDialog.EmploymentContractDestination = newStatutoryDestination;
                editContractDialog.ClickOk();

                ConfirmRequiredDialog endContractConfirmationDialog = new ConfirmRequiredDialog();
                endContractConfirmationDialog.ClickYes();

                staffRecord.SaveStaff();

                Assert.IsTrue(staffRecord.IsSuccessMessageDisplayed());
                Assert.AreEqual(serviceTermDescription, staffRecord.ContractsTable.Rows[0].ServiceTerm);
                Assert.AreEqual(staffStartDate.ToShortDateString(), staffRecord.ContractsTable.Rows[0].StartDate);

                editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();

                Assert.AreEqual(staffStartDate.ToShortDateString(), editContractDialog.StartDate);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.EndDate);
                Assert.AreEqual(newEmploymentType, editContractDialog.EmploymentTypeCombobox);
                Assert.AreEqual(newStatutoryOrigin, editContractDialog.EmploymentContractOrigin);
                Assert.AreEqual(newStatutoryDestination, editContractDialog.EmploymentContractDestination);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.PayScalesTable.Rows[0].EndDate);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.AllowancesTable.Rows[0].EndDate);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.StaffRolesTable.Rows[0].EndDate);
            }
        }

        //[TestMethod][ChromeUiTest("StaffContracts", "P1", "Terminate_Contract_With_Dated_Records_With_Salary_Ranges_from_Staff_as_PO")]
        public void Terminate_Contract_With_Dated_Records_With_Salary_Ranges_from_Staff_as_PO()
        {
            Guid staffId, employmentContractId, serviceTermId, serviceRecordId;
            Guid addPayCatId, allowanceId, serviceTermAllowanceId, ecAllowanceId;
            Guid employmentContractStaffRoleId;

            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");
            string newEmploymentType = "Fixed Term";
            string newStatutoryOrigin = "Break for family reasons";
            string newStatutoryDestination = "Sixth form college - same LA area";

            DateTime staffStartDate = DateTime.Today, staffEndDate = DateTime.Today.AddDays(10);

            DataPackage testData = new DataPackage();

            GenerateStaffServiceRecordAndEmploymentContractData(testData, serviceTermDescription, staffSurname, staffForename, staffStartDate, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId);
            GenerateSalaryRangeData(testData, serviceTermId, employmentContractId, staffStartDate);
            GenerateAllowanceData(testData, serviceTermId, employmentContractId, staffStartDate, null, out addPayCatId, out allowanceId, out serviceTermAllowanceId, out ecAllowanceId);
            GenerateStaffRoleData(testData, out employmentContractStaffRoleId, employmentContractId, staffStartDate);

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                Assert.AreEqual(1, searchResultTiles.Count());

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                EditContractDialog editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                editContractDialog.EmploymentTypeCombobox = newEmploymentType;
                editContractDialog.EndDate = staffEndDate.ToShortDateString();
                editContractDialog.EmploymentContractOrigin = newStatutoryOrigin;
                editContractDialog.EmploymentContractDestination = newStatutoryDestination;
                editContractDialog.ClickOk();

                ConfirmRequiredDialog endContractConfirmationDialog = new ConfirmRequiredDialog();
                endContractConfirmationDialog.ClickYes();

                staffRecord.SaveStaff();

                Assert.IsTrue(staffRecord.IsSuccessMessageDisplayed());
                Assert.AreEqual(serviceTermDescription, staffRecord.ContractsTable.Rows[0].ServiceTerm);
                Assert.AreEqual(staffStartDate.ToShortDateString(), staffRecord.ContractsTable.Rows[0].StartDate);

                editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();

                Assert.AreEqual(staffStartDate.ToShortDateString(), editContractDialog.StartDate);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.EndDate);
                Assert.AreEqual(newEmploymentType, editContractDialog.EmploymentTypeCombobox);
                Assert.AreEqual(newStatutoryOrigin, editContractDialog.EmploymentContractOrigin);
                Assert.AreEqual(newStatutoryDestination, editContractDialog.EmploymentContractDestination);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.SalaryRangesTable.Rows[0].EndDate);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.AllowancesTable.Rows[0].EndDate);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.StaffRolesTable.Rows[0].EndDate);
            }
        }

        //[TestMethod][ChromeUiTest("StaffContracts", "P1", "Terminate_Contract_With_Future_Dated_Records_With_Salary_Ranges_from_Staff_as_PO")]
        public void Terminate_Contract_With_Future_Dated_Records_With_Salary_Ranges_from_Staff_as_PO()
        {
            Guid staffId, employmentContractId, serviceTermId, serviceRecordId;
            Guid addPayCatId, allowanceId, serviceTermAllowanceId, ecAllowanceId;
            Guid employmentContractStaffRoleId;

            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");
            string newEmploymentType = "Fixed Term";
            string newStatutoryOrigin = "Break for family reasons";
            string newStatutoryDestination = "Sixth form college - same LA area";

            DateTime staffStartDate = DateTime.Today, staffEndDate = DateTime.Today.AddDays(10);
            DateTime datedRecordsFutureStartDate = staffStartDate.AddDays(20);

            DataPackage testData = new DataPackage();

            GenerateStaffServiceRecordAndEmploymentContractData(testData, serviceTermDescription, staffSurname, staffForename, staffStartDate, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId);
            GenerateSalaryRangeData(testData, serviceTermId, employmentContractId, datedRecordsFutureStartDate);
            GenerateAllowanceData(testData, serviceTermId, employmentContractId, datedRecordsFutureStartDate, null, out addPayCatId, out allowanceId, out serviceTermAllowanceId, out ecAllowanceId);
            GenerateStaffRoleData(testData, out employmentContractStaffRoleId, employmentContractId, datedRecordsFutureStartDate);

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                Assert.AreEqual(1, searchResultTiles.Count());

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                EditContractDialog editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                editContractDialog.EmploymentTypeCombobox = newEmploymentType;
                editContractDialog.EndDate = staffEndDate.ToShortDateString();
                editContractDialog.EmploymentContractOrigin = newStatutoryOrigin;
                editContractDialog.EmploymentContractDestination = newStatutoryDestination;
                editContractDialog.ClickOk();

                ConfirmRequiredDialog endContractConfirmationDialog = new ConfirmRequiredDialog();
                endContractConfirmationDialog.ClickYes();

                staffRecord.SaveStaff();

                Assert.IsTrue(staffRecord.IsSuccessMessageDisplayed());
                Assert.AreEqual(serviceTermDescription, staffRecord.ContractsTable.Rows[0].ServiceTerm);
                Assert.AreEqual(staffStartDate.ToShortDateString(), staffRecord.ContractsTable.Rows[0].StartDate);

                editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();

                Assert.AreEqual(staffStartDate.ToShortDateString(), editContractDialog.StartDate);
                Assert.AreEqual(staffEndDate.ToShortDateString(), editContractDialog.EndDate);
                Assert.AreEqual(newEmploymentType, editContractDialog.EmploymentTypeCombobox);
                Assert.AreEqual(newStatutoryOrigin, editContractDialog.EmploymentContractOrigin);
                Assert.AreEqual(newStatutoryDestination, editContractDialog.EmploymentContractDestination);
                Assert.AreEqual(0, editContractDialog.SalaryRangesTable.Rows.Count);
                Assert.AreEqual(0, editContractDialog.AllowancesTable.Rows.Count);
                Assert.AreEqual(0, editContractDialog.StaffRolesTable.Rows.Count);
            }
        }

        //[TestMethod][ChromeUiTest("StaffContracts", "P1", "Change_End_Date_Of_Terminated_Contract_With_Dated_Records_Inc_Salary_Ranges_from_Staff_as_PO")]
        public void Change_End_Date_Of_Terminated_Contract_With_Dated_Records_Inc_Salary_Ranges_from_Staff_as_PO()
        {
            Guid staffId, employmentContractId, serviceTermId, serviceRecordId;
            Guid addPayCatId, allowanceId, serviceTermAllowanceId, ecAllowanceId;
            Guid employmentContractStaffRoleId;

            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");
            string newEmploymentType = "Fixed Term";
            string newStatutoryOrigin = "Break for family reasons";
            string newStatutoryDestination = "Sixth form college - same LA area";

            DateTime staffStartDate = DateTime.Today, staffEndDate = DateTime.Today.AddDays(20), newStaffEndDate = staffStartDate.AddDays(10);
            DateTime datedRecordsStartDate = staffStartDate, datedRecordsEndDate = staffEndDate;

            DataPackage testData = new DataPackage();

            GenerateStaffServiceRecordAndEmploymentContractData(testData, serviceTermDescription, staffSurname, staffForename, staffStartDate, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId, datedRecordsEndDate);
            GenerateSalaryRangeData(testData, serviceTermId, employmentContractId, datedRecordsStartDate, datedRecordsEndDate);
            GenerateAllowanceData(testData, serviceTermId, employmentContractId, datedRecordsStartDate, datedRecordsEndDate, out addPayCatId, out allowanceId, out serviceTermAllowanceId, out ecAllowanceId);
            GenerateStaffRoleData(testData, out employmentContractStaffRoleId, employmentContractId, datedRecordsStartDate, datedRecordsEndDate);

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                Assert.AreEqual(1, searchResultTiles.Count());

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                EditContractDialog editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                editContractDialog.EmploymentTypeCombobox = newEmploymentType;
                editContractDialog.EndDate = newStaffEndDate.ToShortDateString();
                editContractDialog.EmploymentContractOrigin = newStatutoryOrigin;
                editContractDialog.EmploymentContractDestination = newStatutoryDestination;
                editContractDialog.ClickOk();

                staffRecord.SaveStaff();

                Assert.IsTrue(staffRecord.IsSuccessMessageDisplayed());
                Assert.AreEqual(serviceTermDescription, staffRecord.ContractsTable.Rows[0].ServiceTerm);
                Assert.AreEqual(staffStartDate.ToShortDateString(), staffRecord.ContractsTable.Rows[0].StartDate);

                editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();

                Assert.AreEqual(staffStartDate.ToShortDateString(), editContractDialog.StartDate);
                Assert.AreEqual(newStaffEndDate.ToShortDateString(), editContractDialog.EndDate);
                Assert.AreEqual(newEmploymentType, editContractDialog.EmploymentTypeCombobox);
                Assert.AreEqual(newStatutoryOrigin, editContractDialog.EmploymentContractOrigin);
                Assert.AreEqual(newStatutoryDestination, editContractDialog.EmploymentContractDestination);
                Assert.AreEqual(newStaffEndDate.ToShortDateString(), editContractDialog.SalaryRangesTable.Rows[0].EndDate);
                Assert.AreEqual(newStaffEndDate.ToShortDateString(), editContractDialog.AllowancesTable.Rows[0].EndDate);
                Assert.AreEqual(newStaffEndDate.ToShortDateString(), editContractDialog.StaffRolesTable.Rows[0].EndDate);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffContracts", "P1", "Update_Existing_Allowance_For_Change_Tracking_Bug")]
        public void Update_Existing_Allowance_For_Change_Tracking_Bug()
        {
            Guid staffId, employmentContractId, serviceTermId, serviceRecordId;
            Guid addPayCatId, allowanceId, serviceTermAllowanceId, ecAllowanceId;
            Guid employmentContractStaffRoleId;

            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");

            DateTime staffStartDate = DateTime.Today, staffEndDate = DateTime.Today.AddDays(20), newStaffEndDate = staffStartDate.AddDays(10);
            DateTime datedRecordsStartDate = staffStartDate, datedRecordsEndDate = staffEndDate;

            string newReason = "Reason";
            DateTime newAllowanceEndDate = datedRecordsEndDate.AddDays(-1);

            DataPackage testData = new DataPackage();

            GenerateStaffServiceRecordAndEmploymentContractData(testData, serviceTermDescription, staffSurname, staffForename, staffStartDate, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId, datedRecordsEndDate);
            GeneratePayScaleData(testData, serviceTermId, employmentContractId, datedRecordsStartDate, datedRecordsEndDate);
            GenerateAllowanceData(testData, serviceTermId, employmentContractId, datedRecordsStartDate, datedRecordsEndDate, out addPayCatId, out allowanceId, out serviceTermAllowanceId, out ecAllowanceId);
            GenerateStaffRoleData(testData, out employmentContractStaffRoleId, employmentContractId, datedRecordsStartDate, datedRecordsEndDate);

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                Assert.AreEqual(1, searchResultTiles.Count());

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                EditContractDialog editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                StaffContractAllowanceDialog allowanceDialog = editContractDialog.AllowancesTable.Rows[0].Edit();

                allowanceDialog.Reason = newReason;
                allowanceDialog.ClickOk();

                allowanceDialog = editContractDialog.AllowancesTable.Rows[0].Edit();
                allowanceDialog.EndDate = newAllowanceEndDate.ToShortDateString();
                allowanceDialog.ClickOk();

                editContractDialog.ClickOk();

                staffRecord.SaveStaff();

                Assert.IsTrue(staffRecord.IsSuccessMessageDisplayed());

                editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                allowanceDialog = editContractDialog.AllowancesTable.Rows[0].Edit();

                Assert.AreEqual(newReason, allowanceDialog.Reason);
                Assert.AreEqual(newAllowanceEndDate.ToShortDateString(), allowanceDialog.EndDate);
            }
        }

        //[TestMethod][ChromeUiTest("StaffContracts", "P1", "Update_Existing_Salary_Range_For_Change_Tracking_Bug")]
        public void Update_Existing_Salary_Range_For_Change_Tracking_Bug()
        {
            Guid staffId, employmentContractId, serviceTermId, serviceRecordId;
            Guid addPayCatId, allowanceId, serviceTermAllowanceId, ecAllowanceId;
            Guid employmentContractStaffRoleId;

            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");

            DateTime staffStartDate = DateTime.Today, staffEndDate = DateTime.Today.AddDays(20), newStaffEndDate = staffStartDate.AddDays(10);
            DateTime datedRecordsStartDate = staffStartDate, datedRecordsEndDate = staffEndDate;

            Decimal newSalary = 150m;
            DateTime newSalaryEndDate = datedRecordsEndDate.AddDays(-1);

            DataPackage testData = new DataPackage();

            GenerateStaffServiceRecordAndEmploymentContractData(testData, serviceTermDescription, staffSurname, staffForename, staffStartDate, out staffId, out serviceTermId, out employmentContractId, out serviceRecordId, datedRecordsEndDate);
            GenerateSalaryRangeData(testData, serviceTermId, employmentContractId, datedRecordsStartDate, datedRecordsEndDate);
            GenerateAllowanceData(testData, serviceTermId, employmentContractId, datedRecordsStartDate, datedRecordsEndDate, out addPayCatId, out allowanceId, out serviceTermAllowanceId, out ecAllowanceId);
            GenerateStaffRoleData(testData, out employmentContractStaffRoleId, employmentContractId, datedRecordsStartDate, datedRecordsEndDate);

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                Assert.AreEqual(1, searchResultTiles.Count());

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                EditContractDialog editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                SelectSalaryRangeDialog salaryRangeDialog = editContractDialog.SalaryRangesTable.Rows[0].EditSalaryRange();

                salaryRangeDialog.AnnualSalary = newSalary.ToString();
                salaryRangeDialog.ClickOk();

                salaryRangeDialog = editContractDialog.SalaryRangesTable.Rows[0].EditSalaryRange();
                salaryRangeDialog.EndDate = newSalaryEndDate.ToShortDateString();
                salaryRangeDialog.ClickOk();

                editContractDialog.ClickOk();

                staffRecord.SaveStaff();

                Assert.IsTrue(staffRecord.IsSuccessMessageDisplayed());

                editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                salaryRangeDialog = editContractDialog.SalaryRangesTable.Rows[0].EditSalaryRange();

                Assert.AreEqual(newSalary, Decimal.Parse(salaryRangeDialog.AnnualSalary));
                Assert.AreEqual(newSalaryEndDate.ToShortDateString(), salaryRangeDialog.EndDate);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffContracts", "P1", "Delete_Contract_from_Staff_as_PO")]
        public void Delete_Contract_from_Staff_as_PO()
        {
            Guid staffId, employmentContractId, employeeId, serviceTermId, serviceRecordId;
            Guid paySpineId, payAwardId, payScaleId, employmentContractPayScaleId;
            Guid postTypeId, statutoryPostTypeId, serviceTermsPostTypeId;
            Guid employmentContractRoleId;

            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string payScaleDescription = CoreQueries.GetColumnUniqueString("PayScale", "Description", 20);
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");

            DateTime staffStartDate = DateTime.Today;

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, description: serviceTermDescription));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payScaleId, serviceTermId, paySpineId, description: payScaleDescription));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));
            testData.AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname, employeeId, forename: staffForename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, staffStartDate, null));
            testData.AddData("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));
            testData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(out employmentContractPayScaleId, payScaleId, employmentContractId, staffStartDate, point: 1m));
            testData.AddData("EmploymentContractRole", DataPackageHelper.GenerateEmploymentContractStaffRole(out employmentContractRoleId, employmentContractId, staffStartDate));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                Assert.AreEqual(1, searchResultTiles.Count());

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                staffRecord.ContractsTable.Rows[0].DeleteRow();
                staffRecord.SaveStaff();

                Assert.IsTrue(staffRecord.IsSuccessMessageDisplayed());
                Assert.IsNotNull(staffRecord.ContractsTable.Rows);
                Assert.AreEqual(0, staffRecord.ContractsTable.Rows.Count);
            }
        }

        public static void GenerateStaffServiceRecordAndEmploymentContractData(DataPackage dataPackage, string serviceTermDescription, string staffForename, string staffSurname, DateTime dateOfArrival, out Guid staffId, out Guid serviceTermId, out Guid employmentContractId, out Guid serviceRecordId, DateTime? dateOfLeaving = null)
        {
            Guid employeeId;
            Guid postTypeId, statutoryPostTypeId, serviceTermPostTypeId;

            dataPackage
                .AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, description: serviceTermDescription))
                .AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId))
                .AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, statutoryPostTypeId: statutoryPostTypeId))
                .AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermPostTypeId, postTypeId, serviceTermId))
                .AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId))
                .AddData("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, dateOfArrival, dateOfLeaving, postTypeId: postTypeId))
                .AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname, forename: staffForename, employeeId: employeeId))
                .AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, dateOfArrival, null));
        }

        public static void GenerateAllowanceData(DataPackage dataPackage, Guid serviceTermId, Guid employmentContractId, DateTime startDate, DateTime? endDate, out Guid addionalPaymentCategoryId, out Guid allowanceId, out Guid serviceTermAllowanceId, out Guid employmentContractAllowanceId)
        {
            dataPackage.AddData("AdditionalPaymentCategory", DataPackageHelper.GenerateAdditionalPaymentCategory(out addionalPaymentCategoryId));
            dataPackage.AddData("Allowance", DataPackageHelper.GenerateAllowance(out allowanceId, addionalPaymentCategoryId));
            dataPackage.AddData("ServiceTermAllowance", DataPackageHelper.GenerateServiceTermAllowance(out serviceTermAllowanceId, allowanceId, serviceTermId));
            dataPackage.AddData("EmploymentContractAllowance", DataPackageHelper.GenerateEmploymentContractAllowance(out employmentContractAllowanceId, allowanceId, employmentContractId, startDate, endDate));
        }

        public static void GeneratePayScaleData(DataPackage existingDataPackage, Guid serviceTermId, Guid employmentContractId, DateTime startDate, DateTime? endDate = null)
        {
            Guid paySpineId, payAwardId, payScaleId, employmentContractPayScale;

            existingDataPackage
                .AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, 1m, 10m, 1m))
                .AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1m))
                .AddData("PayScale", DataPackageHelper.GeneratePayScale(out payScaleId, serviceTermId, paySpineId))
                .AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(out employmentContractPayScale, payScaleId, employmentContractId, startDate, endDate));
        }

        public static void GenerateSalaryRangeData(DataPackage existingDataPackage, Guid serviceTermId, Guid employmentContractId, DateTime startDate, DateTime? endDate = null)
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
                .AddData("EmploymentContractSalaryRange", DataPackageHelper.GenerateEmploymentContractSalaryRange(out employmentContractSalaryRangeId, employmentContractId, salaryRangeId, 100m, startDate, endDate));
        }

        public static void GenerateStaffRoleData(DataPackage existingDataPackage, out Guid employmentContractStaffRoleId, Guid employmentContractId, DateTime startDate, DateTime? endDate = null)
        {
            existingDataPackage.AddData("EmploymentContractRole", DataPackageHelper.GenerateEmploymentContractStaffRole(out employmentContractStaffRoleId, employmentContractId, startDate, endDate));
        }
    }
}
