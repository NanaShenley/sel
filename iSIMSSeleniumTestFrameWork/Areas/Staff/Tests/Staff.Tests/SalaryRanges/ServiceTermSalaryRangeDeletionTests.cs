using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Base;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.Tests.SalaryRanges
{
    [TestClass]
    public class ServiceTermSalaryRangeDeletionTests
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
        /// <summary>
        /// Chrome test - create new service agreement for existing staff as po.
        /// </summary>
        //[TestMethod][ChromeUiTest("Chrome", "ServiceTermSalaryRangeDeletionTests", "P1", "Confirm_Can_Delete_Salary_Range")]
        public void Confirm_Can_Delete_Salary_Range()
        {
            ConfirmCanDeleteSalaryRange();
        }

        /// <summary>
        /// Chrome_s the read_ new_ salary_ range_ as_ po.
        /// </summary>
        //[TestMethod][ChromeUiTest("Chrome", "ServiceTermSalaryRangeDeletionTests", "P1", "Confirm_Cannot_Delete_Salary_Range")]
        public void Confirm_Cannot_Delete_Salary_Range()
        {
            ConfirmCannotDeleteSalaryRange();
        }

        private void ConfirmCannotDeleteSalaryRange()
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
                InitialiseToServiceTermPage();

                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;

                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                int currentCount = serviceTermsPage.SalaryRangeTable.Rows.Count();
                serviceTermsPage.SelectPayAdwardsTab();
                serviceTermsPage.SalaryRangeTable.Rows[0].DeleteRow();

                staffRecordTriplet.ClickSave();
                List<string> validation = serviceTermsPage.Validation.ToList();
                serviceTermsPage.Refresh();

                Assert.IsTrue(validation.Count != 0, "No Service Term Salary Range deletion warning given when it should have been.");
            }
        }

        private void ConfirmCanDeleteSalaryRange()
        {
            Guid employmentContractId, serviceTermId, salaryRangeId, staffId, employeeId, postTypeId;
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

            using (new DataSetup(initialData))
            {
                InitialiseToServiceTermPage();

                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;

                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                int currentCount = serviceTermsPage.SalaryRangeTable.Rows.Count();
                serviceTermsPage.SalaryRangeTable.Rows[0].DeleteRow();

                staffRecordTriplet.ClickSave();

                serviceTermsPage.Refresh();

                // As there is no emplouyment contract and EmploymentContractSalaryRange record associated with the SalaryRecord,
                // it should succeed and save. Therefore the - SalaryRangeTable.Rows.Count() == currentCount -1
                Assert.IsTrue(serviceTermsPage.SalaryRangeTable.Rows.Count() == (currentCount - 1));
            }
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
            {
                testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            }
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            testData.AddData("Employee", DataPackageHelper.GenerateEmployee(out employeeId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname, employeeId, forename: staffForename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, DateTime.Now, null));

            return testData;
        }

        private void InitialiseToServiceTermPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            AutomationSugar.WaitForAjaxCompletion();
        }

    }
}
