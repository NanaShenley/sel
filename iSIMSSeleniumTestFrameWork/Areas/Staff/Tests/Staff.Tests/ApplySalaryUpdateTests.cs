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
    public class ApplySalaryUpdateTests
    {
        #region NUnit Test support
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

        private readonly DateTime startDate = new DateTime(DateTime.Today.Year, 2, 1);

        #endregion

        [TestMethod]
        [ChromeUiTest("StaffApplySalaryUpdate", "P1", "Update")]
        public void Can_Apply_Salary_Update_to_Staff_as_PO()//string quote)
        {
            //System.Diagnostics.Debug.WriteLine(quote);
            //Arrange
            Guid employeeId,
                staffId,
                serviceRecordId,
                serviceTermId,
                paySpineId,
                postTypeId,
                statutoryPostTypeId,
                payAwardId,
                employmentContractId,
                ecPayScaleId,
                statPayScaleId,
                pscaleId;

            const decimal minimumPoint = 1.0m;
            const decimal maximumPoint = 2.0m;

            const int incrementDay = 1;
            const int incrementMonth = 4;

            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4, Environment.Settings.TenantId);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 4, Environment.Settings.TenantId);
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4, Environment.Settings.TenantId);
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);
            string pscaleDescription = CoreQueries.GetColumnUniqueString("PayScale", "Description", 10, Environment.Settings.TenantId);
            string staffName = string.Format("{0}, {1}", surname, forename);
            string serviceTermDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 10, Environment.Settings.TenantId);

            DataPackage testData = new DataPackage();

            testData.AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, surname, employeeId, forename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, startDate));

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, description: serviceTermDescription));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, minimumPoint, maximumPoint, minimumPoint, paySpineCode));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, minimumPoint, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, maximumPoint, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, null, postTypeCode, postTypeDescription, statutoryPostTypeId));
            testData.AddData("StatutoryPayScale", DataPackageHelper.GenerateStatutoryPayScale(out statPayScaleId));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out pscaleId, serviceTermId, paySpineId, statPayScaleId, pscaleDescription));

            testData.AddData("EmploymentContract", DataPackageHelper.GenerateEmploymentContract(out employmentContractId, serviceTermId, employeeId, startDate, postTypeId: postTypeId));
            testData.AddData("EmploymentContractPayScale", DataPackageHelper.GenerateEmploymentContractPayScale(out ecPayScaleId, pscaleId, employmentContractId, startDate));

            using (new DataSetup(testData))
            {
                //Act          
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Annual Increment");
                var applySalaryUpdate = SalarySearch(serviceTermDescription, (DateTime.Today.Year + 1).ToString());
                var gridRow = applySalaryUpdate.SalaryUpdates.Rows.First(x => x.Name == staffName);
                gridRow.Select = true;
                applySalaryUpdate.ClickSave();
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staff = StaffSearch(staffId);
                var contractGridRow = staff.ContractsTable.Rows[0];
                contractGridRow.ClickEdit();
                var contract = new EditContractDialog();
                var psGridRows = contract.PayScalesTable.Rows;

                //Assert
                Assert.AreEqual(2, psGridRows.Count);
                Assert.AreEqual(new DateTime(DateTime.Today.Year + 1, incrementMonth, incrementDay).ToShortDateString(), psGridRows[0].StartDate);
                Assert.AreEqual("2.0", psGridRows[0].ScalePoint);
                Assert.AreEqual(pscaleDescription, psGridRows[0].Scale);
                Assert.AreEqual(new DateTime(DateTime.Today.Year + 1, 3, 31).ToShortDateString(), psGridRows[1].EndDate);
            }
        }

        #region Helpers

        private static SalaryUpdatePage SalarySearch(string serviceTermDescription, string year)
        {
            ApplySalaryUpdateTriplet staffRecordTriplet = new ApplySalaryUpdateTriplet();
            staffRecordTriplet.SearchCriteria.SearchTerm = serviceTermDescription;
            staffRecordTriplet.SearchCriteria.AwardYear = year;
            SalaryUpdatePage result = staffRecordTriplet.SearchCriteria.Search<SalaryUpdatePage>();
            return result;
        }

        private static StaffRecordPage StaffSearch(Guid staffId)
        {
            return StaffRecordPage.LoadStaffDetail(staffId);
        }

        #endregion
    }
}