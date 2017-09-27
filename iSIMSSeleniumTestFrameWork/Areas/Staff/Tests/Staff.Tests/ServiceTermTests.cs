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
using System.Globalization;
using System.Linq;
using TestSettings;
using WebDriverRunner.internals;

namespace Staff.Tests
{
    [TestClass]
    public class ServiceTermTests
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
        [ChromeUiTest("StaffServiceTerms", "P1", "Create_new_Service_Term_by_selecting_an_existing_Pay_Spine_as_PO")]
        public void Create_new_Service_Term_by_selecting_an_existing_Pay_Spine_as_PO()
        {
            Guid paySpineId, postTypeId, statutoryPostTypeId, payAwardId;

            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string serviceTermsCode = CoreQueries.GetColumnUniqueString("ServiceTerm", "Code", 2);
            string serviceTermsDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string payScaleCode = CoreQueries.GetColumnUniqueString("PayScale", "Code", 4);
            string payScaleDescription = CoreQueries.GetColumnUniqueString("PayScale", "Description", 20);

            DataPackage testData = new DataPackage();

            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.Create();

                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                serviceTermsPage.Code = serviceTermsCode;
                serviceTermsPage.Description = serviceTermsDescription;
                serviceTermsPage.IncrementMonth = "April";
                serviceTermsPage.HoursWorkedPerWeek = "32.5";
                serviceTermsPage.WeeksWorkedPerYear = "52";

                PostTypeTripletDialog postTypeTripletDialog = serviceTermsPage.AddPostType();
                postTypeTripletDialog.SearchCriteria.SearchByDescription = postTypeDescription;

                SearchResultsComponent<PostTypeTripletDialog.PostTypeSearchResultTile> postTypeSearchResultTiles = postTypeTripletDialog.SearchCriteria.Search();
                PostTypeTripletDialog.PostTypeSearchResultTile postTypeResultTile = postTypeSearchResultTiles.Single();

                postTypeResultTile.Click();
                postTypeTripletDialog.ClickOk();

                // Ensure the row has been correctly added to the grid
                Assert.IsNotNull(serviceTermsPage.PostTypeTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PostTypeTable.Rows.Count);
                Assert.AreEqual(postTypeCode, serviceTermsPage.PostTypeTable.Rows[0].Code);
                Assert.AreEqual(postTypeDescription, serviceTermsPage.PostTypeTable.Rows[0].Description);

                PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();

                payScaleDialog.Code = payScaleCode;
                payScaleDialog.Description = payScaleDescription;
                payScaleDialog.StatutoryPayScale = "Other";
                payScaleDialog.MinimumPoint = "1.0";
                payScaleDialog.MaximumPoint = "2.0";

                PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();

                paySpineTripletDialog.SearchCriteria.SearchByCode = paySpineCode;

                SearchResultsComponent<PaySpineDialogTriplet.PaySpineDialogSearchResultTile> paySpineSearchResultTiles = paySpineTripletDialog.SearchCriteria.Search();
                PaySpineDialogTriplet.PaySpineDialogSearchResultTile paySpineResultTile = paySpineSearchResultTiles.Single();

                paySpineResultTile.Click();
                paySpineTripletDialog.ClickSavePaySpine();
                AutomationSugar.WaitForAjaxCompletion();
                payScaleDialog.ClickOk();

                // Ensure the row has been correctly added to the grid
                Assert.IsNotNull(serviceTermsPage.PayScaleTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PayScaleTable.Rows.Count);
                Assert.AreEqual(payScaleCode, serviceTermsPage.PayScaleTable.Rows[0].Code);
                Assert.AreEqual(payScaleDescription, serviceTermsPage.PayScaleTable.Rows[0].Description);

                serviceTermsPage.ClickSave();
                serviceTermsPage.Refresh();

                // Ensure the data was was saved
                Assert.IsTrue(serviceTermsPage.IsSuccessMessageDisplay());

                // Verify the post type data was saved
                Assert.IsNotNull(serviceTermsPage.PostTypeTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PostTypeTable.Rows.Count);
                Assert.AreEqual(postTypeCode, serviceTermsPage.PostTypeTable.Rows[0].Code);
                Assert.AreEqual(postTypeDescription, serviceTermsPage.PostTypeTable.Rows[0].Description);

                // Verify the pay scale data was saved
                Assert.IsNotNull(serviceTermsPage.PayScaleTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PayScaleTable.Rows.Count);
                Assert.AreEqual(payScaleCode, serviceTermsPage.PayScaleTable.Rows[0].Code);
                Assert.AreEqual(payScaleDescription, serviceTermsPage.PayScaleTable.Rows[0].Description);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceTerms", "P1", "Create_new_Service_Term_with_new_on_the_fly_Pay_Spine_as_PO")]
        public void Create_new_Service_Term_with_new_on_the_fly_Pay_Spine_as_PO()
        {
            Guid postTypeId, statutoryPostTypeId;
            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 4);
            string statutoryPostTypeCode = CoreQueries.GetColumnUniqueString("StatutoryPostType", "Code", 4);
            string statutoryPostTypeDescription = CoreQueries.GetColumnUniqueString("StatutoryPostType", "Description", 4);
            string serviceTermsCode = CoreQueries.GetColumnUniqueString("ServiceTerm", "Code", 2);
            string serviceTermsDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string payScaleCode = CoreQueries.GetColumnUniqueString("PayScale", "Code", 4);
            string payScaleDescription = CoreQueries.GetColumnUniqueString("PayScale", "Description", 20);

            DataPackage testData = new DataPackage();

            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.Create();

                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                serviceTermsPage.Code = serviceTermsCode;
                serviceTermsPage.Description = serviceTermsDescription;
                serviceTermsPage.IncrementMonth = "April";
                serviceTermsPage.HoursWorkedPerWeek = "32.5";
                serviceTermsPage.WeeksWorkedPerYear = "52";

                PostTypeTripletDialog postTypeTripletDialog = serviceTermsPage.AddPostType();
                postTypeTripletDialog.SearchCriteria.SearchByDescription = postTypeDescription;

                SearchResultsComponent<PostTypeTripletDialog.PostTypeSearchResultTile> postTypeSearchResultTiles = postTypeTripletDialog.SearchCriteria.Search();
                PostTypeTripletDialog.PostTypeSearchResultTile postTypeResultTile = postTypeSearchResultTiles.Single();

                postTypeResultTile.Click();
                postTypeTripletDialog.ClickOk();

                // Ensure the row has been correctly added to the grid
                Assert.IsNotNull(serviceTermsPage.PostTypeTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PostTypeTable.Rows.Count);
                Assert.AreEqual(postTypeCode, serviceTermsPage.PostTypeTable.Rows[0].Code);
                Assert.AreEqual(postTypeDescription, serviceTermsPage.PostTypeTable.Rows[0].Description);

                PayScaleDialog payScaleDialog = serviceTermsPage.AddPayScale();

                payScaleDialog.Code = payScaleCode;
                payScaleDialog.Description = payScaleDescription;
                payScaleDialog.StatutoryPayScale = "Other";
                payScaleDialog.MinimumPoint = "1.0";
                payScaleDialog.MaximumPoint = "2.0";

                PaySpineDialogTriplet paySpineTripletDialog = payScaleDialog.ClickPaySpine();
                PaySpineDialogTriplet.PaySpineDetail paySpineDetail = paySpineTripletDialog.ClickCreatePaySpine();

                paySpineDetail.Code = paySpineCode;
                paySpineDetail.MinimumPoint = "1";
                paySpineDetail.MaximumPoint = "2";
                paySpineDetail.InterVal = "1";
                paySpineDetail.AwardDate = DateTime.Today.AddDays(-10).ToShortDateString();
                paySpineDetail.ClickAddScaleAwards(2);

                paySpineDetail.ScaleAwards.Rows[0].ScaleAmount = "1000";
                paySpineDetail.ScaleAwards.Rows[1].ScaleAmount = "2000";

                paySpineTripletDialog.ClickSavePaySpine();
                payScaleDialog.ClickOk();

                // Ensure the row has been correctly added to the grid
                Assert.IsNotNull(serviceTermsPage.PayScaleTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PayScaleTable.Rows.Count);
                Assert.AreEqual(payScaleCode, serviceTermsPage.PayScaleTable.Rows[0].Code);
                Assert.AreEqual(payScaleDescription, serviceTermsPage.PayScaleTable.Rows[0].Description);

                serviceTermsPage.ClickSave();

                // Ensure the data was was saved
                Assert.IsTrue(serviceTermsPage.IsSuccessMessageDisplay());

                // Verify the post type data was saved
                Assert.IsNotNull(serviceTermsPage.PostTypeTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PostTypeTable.Rows.Count);
                Assert.AreEqual(postTypeCode, serviceTermsPage.PostTypeTable.Rows[0].Code);
                Assert.AreEqual(postTypeDescription, serviceTermsPage.PostTypeTable.Rows[0].Description);

                // Verify the pay scale data was saved
                Assert.IsNotNull(serviceTermsPage.PayScaleTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PayScaleTable.Rows.Count);
                Assert.AreEqual(payScaleCode, serviceTermsPage.PayScaleTable.Rows[0].Code);
                Assert.AreEqual(payScaleDescription, serviceTermsPage.PayScaleTable.Rows[0].Description);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceTerms", "P1", "Read_existing_Service_Term_as_PO")]
        public void Read_existing_Service_Term_as_PO()
        {
            Guid serviceTermId;
            Guid paySpineId, payAwardId, payscaleId;
            Guid postTypeId, statutoryPostTypeId, serviceTermsPostTypeId;
            Guid allowanceId, serviceTermsAllowanceId;
            Guid superannuationSchemeId, superannuationSchemeDetailId, serviceTermSuperannuationSchemeId;
            Guid financialSubGroupId;

            string paySpineCode = CoreQueries.GetColumnUniqueString("PaySpine", "Code", 4);
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string serviceTermsCode = CoreQueries.GetColumnUniqueString("ServiceTerm", "Code", 2);
            string serviceTermsDescription = CoreQueries.GetColumnUniqueString("ServiceTerm", "Description", 20);
            string payScaleCode = CoreQueries.GetColumnUniqueString("PayScale", "Code", 4);
            string payScaleDescription = CoreQueries.GetColumnUniqueString("PayScale", "Description", 20);

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, code: serviceTermsCode, description: serviceTermsDescription));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));
            testData.AddData("Allowance", DataPackageHelper.GenerateAllowance(out allowanceId));
            testData.AddData("ServiceTermAllowance", DataPackageHelper.GenerateServiceTermAllowance(out serviceTermsAllowanceId, allowanceId, serviceTermId));
            testData.AddData("SuperannuationScheme", DataPackageHelper.GenerateSuperannuationScheme(out superannuationSchemeId));
            testData.AddData("SuperannuationSchemeDetail", DataPackageHelper.GenerateSuperannuationSchemeDetail(out superannuationSchemeDetailId, superannuationSchemeId, DateTime.Today.AddDays(-10), 1000m));
            testData.AddData("ServiceTermSuperannuationScheme", DataPackageHelper.GenerateServiceTermSuperannuationScheme(out serviceTermSuperannuationSchemeId, superannuationSchemeId, serviceTermId));
            testData.AddData("FinancialSubGroup", DataPackageHelper.GenerateFinancialSubGroup(out financialSubGroupId, serviceTermId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermsDescription;

                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                Assert.IsNotNull(serviceTermsPage.PayScaleTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PayScaleTable.Rows.Count);

                Assert.IsNotNull(serviceTermsPage.AllowanceTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.AllowanceTable.Rows.Count);

                Assert.IsNotNull(serviceTermsPage.PostTypeTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PostTypeTable.Rows.Count);

                Assert.IsNotNull(serviceTermsPage.SuperannuationSchemesTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.SuperannuationSchemesTable.Rows.Count);

                Assert.IsNotNull(serviceTermsPage.FinalcialSubGroupTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.FinalcialSubGroupTable.Rows.Count);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceTerms", "P1", "Update_existing_Service_Term_as_PO")]
        public void Update_existing_Service_Term_as_PO()
        {
            Guid serviceTermId;
            Guid paySpineId, payAwardId, payscaleId;
            Guid postTypeId, statutoryPostTypeId, serviceTermsPostTypeId;
            string postTypeCode = Utilities.GenerateRandomString(4);
            string postTypeDescription = Utilities.GenerateRandomString(20);
            string paySpineCode = Utilities.GenerateRandomString(4);
            string serviceTermCode = Utilities.GenerateRandomString(2);
            string serviceTermDescription = Utilities.GenerateRandomString(20);
            string newServiceTermsDescription = Utilities.GenerateRandomString(20);
            string payScaleCode = Utilities.GenerateRandomString(4);
            string payScaleDescription = Utilities.GenerateRandomString(20);
            decimal weeksPerYear = 50m, hoursPerWeek = 40m;

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, serviceTermCode, serviceTermDescription, weeksPerYear, hoursPerWeek));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                serviceTermsPage.Description = newServiceTermsDescription;
                serviceTermsPage.ClickSave();
                Assert.IsTrue(serviceTermsPage.IsSuccessMessageDisplay());

                Assert.AreEqual(serviceTermCode, serviceTermsPage.Code);
                Assert.AreEqual(newServiceTermsDescription, serviceTermsPage.Description);
                Assert.AreEqual(weeksPerYear, Decimal.Parse(serviceTermsPage.WeeksWorkedPerYear));
                Assert.AreEqual(hoursPerWeek, Decimal.Parse(serviceTermsPage.HoursWorkedPerWeek));

                Assert.IsNotNull(serviceTermsPage.PayScaleTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PayScaleTable.Rows.Count);

                Assert.IsNotNull(serviceTermsPage.PostTypeTable.Rows);
                Assert.AreEqual(1, serviceTermsPage.PostTypeTable.Rows.Count);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceTerms", "P1", "Delete_existing_Service_Term_as_PO")]
        public void Delete_existing_Service_Term_as_PO()
        {
            Guid serviceTermId;
            Guid paySpineId, payAwardId, payscaleId;
            Guid postTypeId, statutoryPostTypeId, serviceTermsPostTypeId;
            string postTypeCode = Utilities.GenerateRandomString(4);
            string postTypeDescription = Utilities.GenerateRandomString(20);
            string paySpineCode = Utilities.GenerateRandomString(4);
            string serviceTermsCode = Utilities.GenerateRandomString(2);
            string serviceTermsDescription = Utilities.GenerateRandomString(20);
            string payScaleCode = Utilities.GenerateRandomString(4);
            string payScaleDescription = Utilities.GenerateRandomString(20);

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, serviceTermsCode, serviceTermsDescription));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, code: paySpineCode, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermsDescription;

                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();

                serviceTermsPage.ClickDelete();

                searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsFalse(searchResults.Any());
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceTerms", "P1", "Update_existing_Service_Term_Allowance_as_PO")]
        public void Update_existing_Service_Term_Allowance_as_PO()
        {
            Guid serviceTermId;
            Guid paySpineId, payAwardId, payscaleId, allowanceId;
            Guid postTypeId, serviceTermsPostTypeId, serviceTermsAllowanceId;

            string serviceTermCode = Utilities.GenerateRandomString(2);
            string serviceTermDescription = Utilities.GenerateRandomString(20);
            decimal weeksPerYear = 50m, hoursPerWeek = 40m;

            string newAllowanceDescription = Utilities.GenerateRandomString(5);

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, serviceTermCode, serviceTermDescription, weeksPerYear, hoursPerWeek));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            testData.AddData("Allowance", DataPackageHelper.GenerateAllowance(out allowanceId));
            testData.AddData("ServiceTermAllowance", DataPackageHelper.GenerateServiceTermAllowance(out serviceTermsAllowanceId, allowanceId, serviceTermId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();
                serviceTermsPage.SelectAllowanceTab();
                var gridRow = serviceTermsPage.AllowanceTable.Rows[0];
                gridRow.ClickEdit();

                ServiceTermAllowanceDialog allowanceDialog = new ServiceTermAllowanceDialog();
                allowanceDialog.Description = newAllowanceDescription;
                allowanceDialog.ClickOk();

                serviceTermsPage.ClickSave();

                gridRow = serviceTermsPage.AllowanceTable.Rows[0];
                gridRow.ClickEdit();

                allowanceDialog = new ServiceTermAllowanceDialog();

                Assert.AreEqual(newAllowanceDescription, allowanceDialog.Description);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceTerms", "P1", "Update_existing_Service_Term_Allowance_Unique_Description")]
        public void Update_existing_Service_Term_Allowance_Unique_Description()
        {
            Guid serviceTermId;
            Guid paySpineId, payAwardId, payscaleId, allowanceId, duplicateAllowanceId;
            Guid postTypeId, serviceTermsPostTypeId, serviceTermsAllowanceId;

            string serviceTermCode = Utilities.GenerateRandomString(2);
            string serviceTermDescription = Utilities.GenerateRandomString(20);
            decimal weeksPerYear = 50m, hoursPerWeek = 40m;

            string duplicateAllowanceDescription = Utilities.GenerateRandomString(5);

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, serviceTermCode, serviceTermDescription, weeksPerYear, hoursPerWeek));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            testData.AddData("Allowance", DataPackageHelper.GenerateAllowance(out duplicateAllowanceId, description: duplicateAllowanceDescription));
            testData.AddData("Allowance", DataPackageHelper.GenerateAllowance(out allowanceId));
            testData.AddData("ServiceTermAllowance", DataPackageHelper.GenerateServiceTermAllowance(out serviceTermsAllowanceId, allowanceId, serviceTermId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();
                serviceTermsPage.SelectAllowanceTab();
                var gridRow = serviceTermsPage.AllowanceTable.Rows[0];
                gridRow.ClickEdit();

                ServiceTermAllowanceDialog allowanceDialog = new ServiceTermAllowanceDialog();
                allowanceDialog.Description = duplicateAllowanceDescription;
                allowanceDialog.ClickOk();

                List<string> validation = allowanceDialog.Validation.ToList();

                Assert.IsTrue(validation.Contains("Description already exists. Description must be unique."));
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceTerms", "P1", "Update_existing_Service_Term_Post_Type_as_PO")]
        public void Update_existing_Service_Term_Post_Type_as_PO()
        {
            Guid serviceTermId;
            Guid paySpineId, payAwardId, payscaleId, statutoryPostTypeId;
            Guid postTypeId, serviceTermsPostTypeId;

            string serviceTermCode = Utilities.GenerateRandomString(2);
            string serviceTermDescription = Utilities.GenerateRandomString(20);
            decimal weeksPerYear = 50m, hoursPerWeek = 40m;

            string newPostTypeDescription = Utilities.GenerateRandomString(5);

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, serviceTermCode, serviceTermDescription, weeksPerYear, hoursPerWeek));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();
                serviceTermsPage.SelectPostTypesTab();
                var gridRow = serviceTermsPage.PostTypeTable.Rows[0];
                gridRow.ClickEdit();

                ServiceTermPostTypeDialog postTypeDialog = new ServiceTermPostTypeDialog();
                postTypeDialog.Description = newPostTypeDescription;
                postTypeDialog.ClickOk();

                serviceTermsPage.ClickSave();

                gridRow = serviceTermsPage.PostTypeTable.Rows[0];
                gridRow.ClickEdit();

                postTypeDialog = new ServiceTermPostTypeDialog();

                Assert.AreEqual(newPostTypeDescription, postTypeDialog.Description);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceTerms", "P1", "Update_existing_Service_Term_Superannuation_Scheme_as_PO")]
        public void Update_existing_Service_Term_Superannuation_Scheme_as_PO()
        {
            Guid serviceTermId;
            Guid paySpineId, payAwardId, payscaleId, statutoryPostTypeId, superannuationSchemeId;
            Guid postTypeId, serviceTermsPostTypeId, serviceTermSuperannuationSchemeId;

            string serviceTermCode = Utilities.GenerateRandomString(2);
            string serviceTermDescription = Utilities.GenerateRandomString(20);
            decimal weeksPerYear = 50m, hoursPerWeek = 40m;

            string newSuperannuationSchemeDescription = Utilities.GenerateRandomString(5);

            DataPackage testData = new DataPackage();

            testData.AddData("ServiceTerm", DataPackageHelper.GenerateServiceTerm(out serviceTermId, serviceTermCode, serviceTermDescription, weeksPerYear, hoursPerWeek));
            testData.AddData("PaySpine", DataPackageHelper.GeneratePaySpine(out paySpineId, minimumPoint: 1m, maximumPoint: 2m, interval: 1m));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 1m, 1000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayAward", DataPackageHelper.GeneratePayAward(out payAwardId, paySpineId, 2m, 2000m, DateTime.Today.AddDays(-10)));
            testData.AddData("PayScale", DataPackageHelper.GeneratePayScale(out payscaleId, serviceTermId, paySpineId));
            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, statutoryPostTypeId: statutoryPostTypeId));
            testData.AddData("ServiceTermPostType", DataPackageHelper.GenerateServiceTermPostType(out serviceTermsPostTypeId, postTypeId, serviceTermId));

            testData.AddData("SuperannuationScheme", DataPackageHelper.GenerateSuperannuationScheme(out superannuationSchemeId));
            testData.AddData("ServiceTermSuperannuationScheme", DataPackageHelper.GenerateServiceTermSuperannuationScheme(out serviceTermSuperannuationSchemeId, superannuationSchemeId, serviceTermId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet staffRecordTriplet = new ServiceTermTriplet();

                staffRecordTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = staffRecordTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();
                serviceTermsPage.SelectSupperannuationSchemeTab();
                var gridRow = serviceTermsPage.SuperannuationSchemesTable.Rows[0];
                gridRow.ClickEdit();

                SuperannuationSchemesDialog superannuationSchemeDialog = new SuperannuationSchemesDialog();
                superannuationSchemeDialog.Description = newSuperannuationSchemeDescription;
                superannuationSchemeDialog.ClickOk();

                serviceTermsPage.ClickSave();

                gridRow = serviceTermsPage.SuperannuationSchemesTable.Rows[0];
                gridRow.ClickEdit();

                superannuationSchemeDialog = new SuperannuationSchemesDialog();

                Assert.AreEqual(newSuperannuationSchemeDescription, superannuationSchemeDialog.Description);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceTerms", "P1", "Update_Service_Term_Allowance_Award_as_PO")]
        public void Update_Service_Term_Allowance_Award_as_PO()
        {
            Guid serviceTermId, employeeId, staffId, serviceRecordId, employmentContractId, employmentContractPayScaleId, employmentContractRoleId;
            Guid paySpineId, payAwardId, payscaleId, allowanceId, allowanceAwardId, ecAllowanceId;
            Guid postTypeId, serviceTermsPostTypeId, serviceTermsAllowanceId;

            string serviceTermCode = Utilities.GenerateRandomString(2);
            string serviceTermDescription = Utilities.GenerateRandomString(20);
            decimal weeksPerYear = 50m, hoursPerWeek = 40m;
            string staffSurname = Utilities.GenerateRandomString(20, "StaffSurname");
            string staffForename = Utilities.GenerateRandomString(20, "StaffForename");

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

            testData.AddData("Allowance", DataPackageHelper.GenerateFixedAllowance(out allowanceId));
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
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Service Terms");
                ServiceTermTriplet serviceTermTriplet = new ServiceTermTriplet();

                serviceTermTriplet.SearchCriteria.DescriptionSearch = serviceTermDescription;
                SearchResultsComponent<ServiceTermTriplet.ServiceTermSearchResultTile> searchResults = serviceTermTriplet.SearchCriteria.Search();
                Assert.IsTrue(searchResults.Any());

                searchResults.Single().Click();
                ServiceTermPage serviceTermsPage = new ServiceTermPage();
                serviceTermsPage.SelectAllowanceTab();
                var gridRow = serviceTermsPage.AllowanceTable.Rows[0];
                gridRow.ClickEdit();

                ServiceTermAllowanceDialog allowanceDialog = new ServiceTermAllowanceDialog();
                var gridRow2 = allowanceDialog.Awards.Rows[0];
                gridRow2.Amount = newAmount;
                allowanceDialog.ClickOk();
                serviceTermsPage.ClickSave();

                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");

                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();

                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                EditContractDialog editContractDialog = staffRecord.ContractsTable.Rows[0].Edit();
                var gridRow3 = editContractDialog.AllowancesTable.Rows[0];

                Assert.AreEqual(newAmount, gridRow3.Amount);
            }
        }
    }
}
