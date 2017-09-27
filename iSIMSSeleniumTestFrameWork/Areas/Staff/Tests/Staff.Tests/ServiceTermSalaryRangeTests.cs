using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Base;
using Staff.POM.Components.Staff;
using Staff.POM.Components.Staff.Dialogs;
using Staff.POM.Components.Staff.Pages;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.internals;

namespace Staff.Tests
{
    /// <summary>
    /// Service Term Salary Range Tests
    /// </summary>
    [TestClass]
    public class ServiceTermSalaryRangeTests
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
        /// Chrome - add salary range to service term.
        /// </summary>
        //[TestMethod][ChromeUiTest("Chrome", "ServiceTermSalaryRangeTests", "P1", "Add_Salary_Range_To_Service_Term")]
        public void Add_Salary_Range_To_Service_Term()
        {
            AddSalaryRangeToServiceTerm();
        }

        /// <summary>
        /// Chrome - read salary range to service term.
        /// </summary>
        //[TestMethod][ChromeUiTest("Chrome", "ServiceTermSalaryRangeTests", "P1", "Read_Salary_Range_To_Service_Term")]
        public void Read_Salary_Range_To_Service_Term()
        {
            ReadSalaryRangeToServiceTerm();
        }

        /// <summary>
        /// Chrome - add neither a salary range or payscale to service term.
        /// </summary>
        //[TestMethod][ChromeUiTest("ServiceTermSalaryRangeTests", "P1", "Check_At_Least_One_PayLevel_Or_SalaryRange_Is_Required")]
        public void Check_At_Least_One_PayLevel_Or_SalaryRange_Is_Required()
        {
            string serviceTermDescription;
            string salaryRangeCode;
            string salaryRangeDescription;
            string payLevelCode;
            string payLevelDescription;
            string regionalWeightingDescription;
            Guid serviceTermId, salaryRangeId;

            DataPackage testData = SetupTestData(out serviceTermId, out salaryRangeId, out serviceTermDescription, out salaryRangeCode, out salaryRangeDescription, out payLevelCode, out payLevelDescription, out regionalWeightingDescription, omitPayScales: true);

            using (new DataSetup(testData))
            {
                InitialiseToServiceTermsPage();
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                var originalRowCount = serviceTermsPage.SalaryRangeTable.Rows.Count();

                serviceTermsPage = new ServiceTermPage();
                serviceTermsPage.ClickSave();
                List<string> validation = serviceTermsPage.Validation.ToList();

                //Assert
                Assert.IsTrue(validation.Contains("At least one Salary Range or one Pay Scale is required."));
            }
        }

        /// <summary>
        /// Check that the pay scale grid's Add Pay Scale button is disabled when a Salary Range entry is added to the SR grid
        /// </summary>
        //[TestMethod][ChromeUiTest("ServiceTermSalaryRangeTests", "P1", "Check_PayScale_Grid_Disabled_When_SalaryRange_Added")]
        public void Check_PayScale_Grid_Disabled_When_SalaryRange_Added()
        {
            string serviceTermDescription;
            string salaryRangeCode;
            string salaryRangeDescription;
            string payLevelCode;
            string payLevelDescription;
            string regionalWeightingDescription;
            Guid serviceTermId, salaryRangeId;

            DataPackage testData = SetupTestData(out serviceTermId, out salaryRangeId, out serviceTermDescription, out salaryRangeCode, out salaryRangeDescription, out payLevelCode, out payLevelDescription, out regionalWeightingDescription, true);

            using (new DataSetup(testData))
            {
                InitialiseToServiceTermsPage();
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                var originalRowCount = serviceTermsPage.SalaryRangeTable.Rows.Count();

                SalaryRangeTriple salaryRangeTriple = serviceTermsPage.AddSalaryRange();

                salaryRangeTriple.SearchCriteria.Code = salaryRangeCode;
                var tiles = salaryRangeTriple.SearchCriteria.Search();

                Assert.AreEqual(1, tiles.Count());

                var tile = tiles.Single(x => x.Name == salaryRangeCode);

                tile.Click();

                SalaryRangeDialog salaryRangeDialog = new SalaryRangeDialog();
                Assert.AreEqual(salaryRangeCode, salaryRangeDialog.SalaryRangeDetails.Code);
                Assert.AreEqual(salaryRangeDescription, salaryRangeDialog.SalaryRangeDetails.Description);
                Assert.AreEqual(payLevelDescription, salaryRangeDialog.SalaryRangeDetails.PayLevel);
                Assert.AreEqual(regionalWeightingDescription, salaryRangeDialog.SalaryRangeDetails.RegionalWeighting);

                salaryRangeDialog.ClickOk();

                ConfirmRequiredDialog confirmRequiredDialog = new ConfirmRequiredDialog();
                confirmRequiredDialog.ClickYes();

                serviceTermsPage = new ServiceTermPage();
                var salaryRangeTable = serviceTermsPage.SalaryRangeTable;
                Assert.AreEqual(originalRowCount + 1, salaryRangeTable.Rows.Count());

                bool isPayScaleGridEnabled = serviceTermsPage.PayScaleGridAddButtonEnabled;
                Assert.IsFalse(isPayScaleGridEnabled, "Add Pay Scale button is not disabled when Salary Range grid had at least one entry.");

            }
        }

        // <summary>
        /// Check that the pay scale grid's Add Pay Scale button is Enabled when a Salary Range grid is empty.
        /// </summary>
        //[TestMethod][ChromeUiTest("ServiceTermSalaryRangeTests", "P1", "Check_PayScale_Grid_Enabled_When_SalaryRange_Empty")]
        public void Check_PayScale_Grid_Enabled_When_SalaryRange_Empty()
        {
            string serviceTermDescription;
            string salaryRangeCode;
            string salaryRangeDescription;
            string payLevelCode;
            string payLevelDescription;
            string regionalWeightingDescription;
            Guid serviceTermId, salaryRangeId;

            DataPackage testData = SetupTestData(out serviceTermId, out salaryRangeId, out serviceTermDescription, out salaryRangeCode, out salaryRangeDescription, out payLevelCode, out payLevelDescription, out regionalWeightingDescription, true);

            using (new DataSetup(testData))
            {
                InitialiseToServiceTermsPage();
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                var originalRowCount = serviceTermsPage.SalaryRangeTable.Rows.Count();

                SalaryRangeTriple salaryRangeTriple = serviceTermsPage.AddSalaryRange();

                salaryRangeTriple.SearchCriteria.Code = salaryRangeCode;
                var tiles = salaryRangeTriple.SearchCriteria.Search();

                Assert.AreEqual(1, tiles.Count());

                var tile = tiles.Single(x => x.Name == salaryRangeCode);

                tile.Click();

                SalaryRangeDialog salaryRangeDialog = new SalaryRangeDialog();
                Assert.AreEqual(salaryRangeCode, salaryRangeDialog.SalaryRangeDetails.Code);
                Assert.AreEqual(salaryRangeDescription, salaryRangeDialog.SalaryRangeDetails.Description);
                Assert.AreEqual(payLevelDescription, salaryRangeDialog.SalaryRangeDetails.PayLevel);
                Assert.AreEqual(regionalWeightingDescription, salaryRangeDialog.SalaryRangeDetails.RegionalWeighting);

                salaryRangeDialog.ClickOk();

                ConfirmRequiredDialog confirmRequiredDialog = new ConfirmRequiredDialog();
                confirmRequiredDialog.ClickYes();

                serviceTermsPage = new ServiceTermPage();
                var salaryRangeTable = serviceTermsPage.SalaryRangeTable;
                Assert.AreEqual(originalRowCount + 1, salaryRangeTable.Rows.Count());

                bool isPayScaleGridEnabled = serviceTermsPage.PayScaleGridAddButtonEnabled;
                Assert.IsFalse(isPayScaleGridEnabled, "Add Pay Scale button is not disabled when Salary Range grid had at least one entry.");

                //Now remove the entry 
                salaryRangeTable.DeleteRowIfExist(salaryRangeTable.Rows.First());

                isPayScaleGridEnabled = serviceTermsPage.PayScaleGridAddButtonEnabled;
                Assert.IsTrue(isPayScaleGridEnabled, "Add Pay Scale button is not enabled when Salary Range grid is empty.");
            }
        }

        /// <summary>
        /// Chrome - Display affected service term(s) confirmation dialogue.
        /// </summary>
        // [TestMethod][ChromeUiTest("Chrome", "ServiceTermSalaryRangeTests", "P2", "Salary_Range_Check_Affected_Service_Terms")]
        public void Salary_Range_Check_Affected_Service_Terms()
        {

            string serviceTermDescription;
            string salaryRangeCode;
            string salaryRangeDescription;
            string payLevelCode;
            string payLevelDescription;
            string regionalWeightingDescription;
            Guid serviceTermId, salaryRangeId, serviceTermSalaryRangeId;

            DataPackage testData = SetupTestData(out serviceTermId, out salaryRangeId, out serviceTermDescription, out salaryRangeCode, out salaryRangeDescription, out payLevelCode, out payLevelDescription, out regionalWeightingDescription);
            testData.AddData("ServiceTermSalaryRange", DataPackageHelper.GenerateServiceTermSalaryRange(out serviceTermSalaryRangeId, serviceTermId, salaryRangeId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                var originalRowCount = serviceTermsPage.SalaryRangeTable.Rows.Count();

                var salaryRangeRow = serviceTermsPage.SalaryRangeTable.Rows[0];
                salaryRangeRow.ClickEdit();

                SalaryRangePage salaryRangePageReopened = new SalaryRangePage();
                var updatedCode = Utilities.GenerateRandomString(2);
                var updatedDesciption = Utilities.GenerateRandomString(5);
                salaryRangePageReopened.Code = updatedCode;
                salaryRangePageReopened.Description = updatedDesciption;
                salaryRangePageReopened.ClickOk();
                salaryRangePageReopened.EditConfirmationDialog();
                var UpdatedsalaryRangeRow = serviceTermsPage.SalaryRangeTable.Rows[0];
                UpdatedsalaryRangeRow.ClickEdit();

                Assert.AreEqual(updatedCode, salaryRangePageReopened.Code, "injected Code not present");
                Assert.AreEqual(updatedDesciption, salaryRangePageReopened.Description, "injected Description not present");

            }
        }

        /// <summary>
        /// Adds the salary range to service term.
        /// </summary>
        private void AddSalaryRangeToServiceTerm()
        {
            string serviceTermDescription;
            string salaryRangeCode;
            string salaryRangeDescription;
            string payLevelCode;
            string payLevelDescription;
            string regionalWeightingDescription;
            Guid serviceTermId, salaryRangeId;

            DataPackage testData = SetupTestData(out serviceTermId, out salaryRangeId, out serviceTermDescription, out salaryRangeCode, out salaryRangeDescription, out payLevelCode, out payLevelDescription, out regionalWeightingDescription);

            using (new DataSetup(testData))
            {
                InitialiseToServiceTermsPage();
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                var originalRowCount = serviceTermsPage.SalaryRangeTable.Rows.Count();

                SalaryRangeTriple salaryRangeTriple = serviceTermsPage.AddSalaryRange();

                salaryRangeTriple.SearchCriteria.Code = salaryRangeCode;
                var tiles = salaryRangeTriple.SearchCriteria.Search();

                Assert.AreEqual(1, tiles.Count());

                var tile = tiles.Single(x => x.Name == salaryRangeCode);

                tile.Click();

                SalaryRangeDialog salaryRangeDialog = new SalaryRangeDialog();
                Assert.AreEqual(salaryRangeCode, salaryRangeDialog.SalaryRangeDetails.Code);
                Assert.AreEqual(salaryRangeDescription, salaryRangeDialog.SalaryRangeDetails.Description);
                Assert.AreEqual(payLevelDescription, salaryRangeDialog.SalaryRangeDetails.PayLevel);
                Assert.AreEqual(regionalWeightingDescription, salaryRangeDialog.SalaryRangeDetails.RegionalWeighting);

                salaryRangeDialog.ClickOk();

                ConfirmRequiredDialog confirmRequiredDialog = new ConfirmRequiredDialog();
                confirmRequiredDialog.ClickYes();

                serviceTermsPage = new ServiceTermPage();
                var v = serviceTermsPage.SalaryRangeTable;
                Assert.AreEqual(originalRowCount + 1, v.Rows.Count());
            }
        }

        /// <summary>
        /// Reads the salary range to service term.
        /// </summary>
        private void ReadSalaryRangeToServiceTerm()
        {
            string serviceTermDescription;
            string salaryRangeCode;
            string salaryRangeDescription;
            string payLevelCode;
            string payLevelDescription;
            string regionalWeightingDescription;
            Guid serviceTermId, salaryRangeId, serviceTermSalaryRangeId;

            DataPackage testData = SetupTestData(out serviceTermId, out salaryRangeId, out serviceTermDescription, out salaryRangeCode, out salaryRangeDescription, out payLevelCode, out payLevelDescription, out regionalWeightingDescription);
            testData.AddData("ServiceTermSalaryRange", DataPackageHelper.GenerateServiceTermSalaryRange(out serviceTermSalaryRangeId, serviceTermId, salaryRangeId));

            using (new DataSetup(testData))
            {
                InitialiseToServiceTermsPage();
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                var originalRowCount = serviceTermsPage.SalaryRangeTable.Rows.Count();

                var salaryRangeRow = serviceTermsPage.SalaryRangeTable.Rows[0];
                salaryRangeRow.ClickEdit();

                SalaryRangePage salaryRangePageReopened = new SalaryRangePage();

                Assert.AreEqual(1, originalRowCount);
                Assert.AreEqual(salaryRangeCode, salaryRangePageReopened.Code);
                Assert.AreEqual(salaryRangeDescription, salaryRangePageReopened.Description);
                Assert.AreEqual(payLevelDescription, salaryRangePageReopened.PayLevel);
                Assert.AreEqual(regionalWeightingDescription, salaryRangePageReopened.RegionalWeighting);
            }
        }


        private void InitialiseToServiceTermsPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
            AutomationSugar.WaitForAjaxCompletion();
        }

        #region data setup helpers

        /// <summary>
        /// Setups the test data.
        /// </summary>
        /// <param name="serviceTermId">The service term identifier.</param>
        /// <param name="salaryRangeId">The salary range identifier.</param>
        /// <param name="serviceTermDescription">The service term description.</param>
        /// <param name="salaryRangeCode">The salary range code.</param>
        /// <param name="salaryRangeDescription">The salary range description.</param>
        /// <param name="payLevelCode">The pay level code.</param>
        /// <param name="payLevelDescription">The pay level description.</param>
        /// <returns></returns>
        private static DataPackage SetupTestData(out Guid serviceTermId,
                                                 out Guid salaryRangeId,
                                                 out string serviceTermDescription,
                                                 out string salaryRangeCode,
                                                 out string salaryRangeDescription,
                                                 out string payLevelCode,
                                                 out string payLevelDescription,
                                                 out string regionalWeightingDescription,
                                                bool omitPayScales = false)
        {
            DataPackage testData = new DataPackage();

            Guid paySpineId, payAwardId, payscaleId, postTypeId, statutoryPostTypeId, serviceTermsPostTypeId;
            string postTypeCode = Utilities.GenerateRandomString(4);
            string postTypeDescription = Utilities.GenerateRandomString(20);
            string paySpineCode = Utilities.GenerateRandomString(4);
            string serviceTermCode = Utilities.GenerateRandomString(2);
            serviceTermDescription = Utilities.GenerateRandomString(20);
            decimal weeksPerYear = 50m, hoursPerWeek = 40m;

            salaryRangeCode = Utilities.GenerateRandomString(8);
            salaryRangeDescription = Utilities.GenerateRandomString(100);
            DateTime awardDateTime = DateTime.Now.AddDays(1);
            string awardDate = awardDateTime.ToString("dd/MM/yyyy");
            decimal maximumAmount = 200, minimumAmount = 20;

            Guid payLevelId, regionalWeightingId, salaryAwardid;

            payLevelCode = Utilities.GenerateRandomString(20);
            payLevelDescription = Utilities.GenerateRandomString(20);
            string regionalWeightingCode = Utilities.GenerateRandomString(20);
            regionalWeightingDescription = Utilities.GenerateRandomString(20);

            testData = new DataPackage();

            testData.AddData("PayLevel", DataPackageHelper.GeneratePayLevel(out payLevelId, payLevelCode, payLevelDescription));
            testData.AddData("RegionalWeighting", DataPackageHelper.GenerateRegionalWeighting(out regionalWeightingId, regionalWeightingCode, regionalWeightingDescription));
            testData.AddData("SalaryRange", DataPackageHelper.GenerateSalaryRange(out salaryRangeId, salaryRangeCode, salaryRangeDescription, payLevelId, regionalWeightingId));
            testData.AddData("SalaryAward", DataPackageHelper.GenerateSalaryAward(out salaryAwardid, awardDateTime, maximumAmount, minimumAmount, salaryRangeId));
            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, serviceTermCode, serviceTermDescription, weeksPerYear, hoursPerWeek));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            if (!omitPayScales)
            {
                testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            }
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            return testData;
        }

        #endregion

    }
}
