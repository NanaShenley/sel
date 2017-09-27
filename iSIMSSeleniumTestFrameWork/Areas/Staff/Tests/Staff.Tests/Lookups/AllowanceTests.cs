using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Base;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;

namespace Staff.Tests.Lookups
{
    [TestClass]
    public class AllowanceTests
    {
        [TestMethod]
        [ChromeUiTest("Allowances", "P1", "Update_Allowance_Award_Amount_as_PO")]
        public void Update_Allowance_Award_Amount_as_PO()
        {
            //Arrange
            Guid serviceTermId, employeeId, staffId, serviceRecordId, employmentContractId, employmentContractPayScaleId, employmentContractRoleId;
            Guid paySpineId, payAwardId, payscaleId, allowanceId, allowanceAwardId, ecAllowanceId;
            Guid postTypeId, serviceTermsPostTypeId, serviceTermsAllowanceId;

            string serviceTermCode = Utilities.GenerateRandomString(2);
            string serviceTermDescription = Utilities.GenerateRandomString(20);
            decimal weeksPerYear = 50m, hoursPerWeek = 40m;
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");

            string allowanceDescription = Utilities.GenerateRandomString(10);

            string newAmount = "20.00";
            DateTime staffStartDate = DateTime.Today;

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, serviceTermCode, serviceTermDescription, weeksPerYear, hoursPerWeek));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            testData.AddData("Allowance", DataPackageHelper.GenerateFixedAllowance(out allowanceId, description: allowanceDescription));
            testData.AddData("AllowanceAward", DataPackageHelper.GenerateAllowanceAward(out allowanceAwardId, allowanceId));
            testData.AddData("ServiceTermAllowance", DataPackageHelper.GenerateServiceTermAllowance(out serviceTermsAllowanceId, allowanceId, serviceTermId));

            testData.AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname, employeeId, forename: staffForename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, staffStartDate));
            testData.AddData("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, DateTime.Today, postTypeId: postTypeId));
            testData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(out employmentContractPayScaleId, payscaleId, employmentContractId, staffStartDate));
            testData.AddData("EmploymentContractRole", DataPackageHelper.GenerateEmploymentContractStaffRole(out employmentContractRoleId, employmentContractId, staffStartDate));
            testData.AddData("EmploymentContractAllowance", DataPackageHelper.GenerateEmploymentContractAllowance(out ecAllowanceId, allowanceId, employmentContractId, staffStartDate));
         
            using (new DataSetup(testData))
            {
                //Act
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Allowances");
                var allowanceTriplet = new AllowanceTriplet();

                allowanceTriplet.SearchCriteria.CodeOrDecription = allowanceDescription;
                AllowanceDetailsPage allowanceDetail = allowanceTriplet.SearchCriteria.Search<AllowanceDetailsPage>();

                var gridRow = allowanceDetail.Allowances.Rows[0];
                gridRow.ClickEdit();

                AllowanceDialog allowanceDialog = new AllowanceDialog();
                var gridRow2 = allowanceDialog.Awards.Rows[0];
                gridRow2.Amount = "20.00";

                allowanceDialog.ClickOk();
                allowanceTriplet.ClickSave();

                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                EditContractDialog editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                var gridRow3 = editContractDialog.AllowancesTable.Rows[0];

                //Assert
                Assert.AreEqual(newAmount, gridRow3.Amount);
            }
        }
    }
}
