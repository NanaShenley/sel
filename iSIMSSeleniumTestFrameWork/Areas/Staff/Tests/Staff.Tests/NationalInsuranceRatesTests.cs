using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using SeSugar.Automation;
using SeSugar.Data;
using Selene.Support.Attributes;

namespace Staff.Tests
{
    [TestClass]
    public class NationalInsuranceRatesTests
    {
        #region MS Unit Testing support
        //public TestContext TestContext { get; set; }
        //[TestInitialize]
        //public void Init()
        //{
        //    TestRunner.VSSeleniumTest.Init(this, TestContext);
        //}
        //[TestCleanup]
        //public void Cleanup()
        //{
        //    TestRunner.VSSeleniumTest.Cleanup(TestContext);
        //}
        #endregion

        //[TestMethod]
        //[ChromeUiTest("StaffNationalInsuranceRates", "P1")]
        //public void Create_new_National_Insurance_Rates_record_as_TestUser()
        //{
        //    //Choose an abitrary date
        //    //Navigate to the NI Rates Page as a PO
        //    //Enter the date into the "Start Date" search criteria box
        //    //If no results, continue with steps, else choose a different date - the NI Rates entity must have unique values in the "Start Date" column

        //    //Add a few bands - This grid uses an AJAX call back when focus is lost from some of the cells, so wait as appropriate.
        //    //Assert "Lower Monthly Earnings" field is calculated correctly
        //    //Save
        //    //Assert save success and values retained
        //    DateTime startDateValue = DataAccessor.GetClosestAvailableDateBefore(DateTime.Today, "NationalInsuranceRate", "StartDate");
        //    DateTime endDateValue = startDateValue.AddYears(1).AddDays(-1);
        //    string startDate = startDateValue.ToShortDateString();
        //    string endDate = endDateValue.ToShortDateString();
        //    string upperMonthlyEarnings = "10.00";
        //    string rateOne = "2.0000";
        //    string rateTwo = "4.0000";

        //    //Login as School Test User and navigate to National Insurance Rates screen
        //    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);
        //    AutomationSugar.NavigateMenu("Tasks", "Staff", "National Insurance Rates");

        //    var nationalInsuranceRateTriplet = new NationalInsuranceRateTriplet();
        //    var nationalInsuranceRatesDetails = nationalInsuranceRateTriplet.Create();
        //    nationalInsuranceRatesDetails.StartDate = startDate;
        //    nationalInsuranceRatesDetails.EndDate = endDate;

        //    nationalInsuranceRatesDetails.ClickAddBand();
        //    var nationalInsuranceRateDetailsPage = new NationalInsuranceRatesPage();
        //    var gridRow = nationalInsuranceRateDetailsPage.BandCollections.Rows[0];
        //    gridRow.UpperMonthlyEarnings = upperMonthlyEarnings;
        //    gridRow.Refresh();
        //    gridRow.RateOne = rateOne;
        //    gridRow.Refresh();
        //    gridRow.RateTwo = rateTwo;

        //    nationalInsuranceRateDetailsPage.ClickSave();
        //    nationalInsuranceRateTriplet.SearchCriteria.SearchByStartDate = startDate;
        //    var nationalInsuranceRateSearchResults = nationalInsuranceRateTriplet.SearchCriteria.Search();
        //    var nationalInsuranceRateSearchTile = nationalInsuranceRateSearchResults.Single(t => t.Code.Equals("Start Date: " + startDate));
        //    var nationInsuranceRateDetails = nationalInsuranceRateSearchTile.Click<NationalInsuranceRatesPage>();

        //    Assert.AreEqual(startDate, nationalInsuranceRateDetailsPage.StartDate, "Create New National Insurance rates failed - StartDate doesn't match");
        //    Assert.AreEqual(endDate, nationalInsuranceRateDetailsPage.EndDate, "Create New National Insurance rates failed - EndDate doesn't match");
        //    Assert.AreEqual(upperMonthlyEarnings, gridRow.UpperMonthlyEarnings, "Create New National Insurance rates failed - UpperMonthlyEarnings doesn't match");
        //    Assert.AreEqual(rateOne, gridRow.RateOne, "Create New National Insurance rates failed - RateOne doesn't match");
        //    Assert.AreEqual(rateTwo, gridRow.RateTwo, "Create New National Insurance rates failed - RateTwo doesn't match");

        //}

        /// <summary>
        /// The NI Rate screen is now read only, so the only test that is needed is Read.
        /// </summary>
        [TestMethod]
        [ChromeUiTest("StaffNationalInsuranceRates", "P1")]
        public void Read_existing_National_Insurance_Rates_record_as_TestUser()
        {
            //Inject NI Rates Record with a unique Start Date, along with some valid NI Bands
            //Navigate to the NI Rates Page as a PO
            //Search for and load the injected NI record
            //Assert that the injected values are present        
            DateTime startDate = DataAccessor.GetClosestAvailableDateBefore(DateTime.Today, "NationalInsuranceRate", "StartDate");
            DateTime endDate = startDate.AddYears(1).AddDays(-1);

            string upperMonthlyEarnings = "10.00";
            string lowerMonthlyEarnings = "0";
            string rateOne = "2.0000";
            string rateTwo = "4.0000";

            Guid nationalInsuranceID = Guid.NewGuid();
            Guid bandCollectionID = Guid.NewGuid();

            var testdata = new DataPackage[]
            {
            GetNationalInsuranceRate(nationalInsuranceID, bandCollectionID, upperMonthlyEarnings, lowerMonthlyEarnings, rateOne, rateTwo, startDate, endDate),
            };

            using (new DataSetup(testdata))
            {
                //Login as School Test User and navigate to National Insurance Rates screen

                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "National Insurance Rates");

                // select existing national insurance rates

                var nationalInsuranceRateTriplet = new NationalInsuranceRateTriplet();
                nationalInsuranceRateTriplet.SearchCriteria.SearchByStartDate = startDate.ToShortDateString();
                var nationalInsuranceRateSearchResults = nationalInsuranceRateTriplet.SearchCriteria.Search();
                var nationalInsuranceRateSearchTile = nationalInsuranceRateSearchResults.Single(t => t.Code.Equals("Start Date: " + startDate.ToShortDateString()));
                var nationInsuranceRateDetails = nationalInsuranceRateSearchTile.Click<NationalInsuranceRatesPage>();
                var bandCollectionsResulsts = nationInsuranceRateDetails.BandCollections.Rows[0];

                Assert.AreEqual(startDate.ToShortDateString(), nationInsuranceRateDetails.StartDate, "injected StartDate not present");
                Assert.AreEqual(endDate.ToShortDateString(), nationInsuranceRateDetails.EndDate, "injected EndDate not present");
                Assert.AreEqual(upperMonthlyEarnings, bandCollectionsResulsts.UpperMonthlyEarnings, "injected UpperMonthlyEarnings not present");
                Assert.AreEqual(lowerMonthlyEarnings, bandCollectionsResulsts.LowerMonthlyEarnings, "injected LowerMonthlyEarnings not present");
                Assert.AreEqual(rateOne, bandCollectionsResulsts.RateOne, "injected RateOne not present");
                Assert.AreEqual(rateTwo, bandCollectionsResulsts.RateTwo, "injected RateTwo not present");

            }
        }

        //[TestMethod]
        //[ChromeUiTest("StaffNationalInsuranceRates", "P1")]
        //public void Update_existing_National_Insurance_Rates_record_as_TestUser()
        //{
        //    //Inject NI Rates Record with a unique Start Date, along with some valid NI Bands
        //    //Navigate to the NI Rates Page as a PO
        //    //Search for and load the injected NI record
        //    //Modify values, Save
        //    //Assert save success, assert changes retained
        //    DateTime startDate = DataAccessor.GetClosestAvailableDateBefore(DateTime.Today, "NationalInsuranceRate", "StartDate");
        //    DateTime endDate = startDate.AddYears(1).AddDays(-1);

        //    string upperMonthlyEarnings = "10.00";
        //    string lowerMonthlyEarnings = "0";
        //    string rateOne = "2.0000";
        //    string rateTwo = "4.0000";

        //    Guid nationalInsuranceID = Guid.NewGuid();
        //    Guid bandCollectionID = Guid.NewGuid();

        //    var testdata = new DataPackage[]
        //    {
        //    GetNationalInsuranceRate(nationalInsuranceID, bandCollectionID, upperMonthlyEarnings, lowerMonthlyEarnings, rateOne, rateTwo, startDate, endDate),
        //    };

        //    using (new DataSetup(testdata))
        //    {
        //        //Login as School Test User and navigate to National Insurance Rates screen

        //        SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);
        //        AutomationSugar.NavigateMenu("Tasks", "Staff", "National Insurance Rates");

        //        // select existing national insurance rates

        //        var nationalInsuranceRateTriplet = new NationalInsuranceRateTriplet();
        //        nationalInsuranceRateTriplet.SearchCriteria.SearchByStartDate = startDate.ToShortDateString();
        //        var nationalInsuranceRateSearchResults = nationalInsuranceRateTriplet.SearchCriteria.Search();
        //        var nationalInsuranceRateSearchTile = nationalInsuranceRateSearchResults.Single(t => t.Code.Equals("Start Date: " + startDate.ToShortDateString()));
        //        var nationInsuranceRateDetailsPage = nationalInsuranceRateSearchTile.Click<NationalInsuranceRatesPage>();
        //        var bandCollectionsResulsts = nationInsuranceRateDetailsPage.BandCollections.Rows[0];

        //        //Make some changes, Save

        //        upperMonthlyEarnings = bandCollectionsResulsts.UpperMonthlyEarnings = "20.00";
        //        bandCollectionsResulsts.Refresh();
        //        rateOne = bandCollectionsResulsts.RateOne = "6.0000";
        //        bandCollectionsResulsts.Refresh();
        //        rateTwo = bandCollectionsResulsts.RateTwo = "8.0000";

        //        nationInsuranceRateDetailsPage.ClickSave();
        //        nationInsuranceRateDetailsPage.Refresh();

        //        // load new results
        //        nationalInsuranceRateSearchTile = nationalInsuranceRateSearchResults.Single(t => t.Code.Equals("Start Date: " + startDate.ToShortDateString()));
        //        var editednationInsuranceRateDetailsPage = nationalInsuranceRateSearchTile.Click<NationalInsuranceRatesPage>();
        //        var editedbandCollectionsResulsts = editednationInsuranceRateDetailsPage.BandCollections.Rows[0];

        //        Assert.AreEqual(startDate.ToShortDateString(), editednationInsuranceRateDetailsPage.StartDate, "injected StartDate not present");
        //        Assert.AreEqual(endDate.ToShortDateString(), editednationInsuranceRateDetailsPage.EndDate, "injected EndDate not present");
        //        Assert.AreEqual(upperMonthlyEarnings, editedbandCollectionsResulsts.UpperMonthlyEarnings, "injected UpperMonthlyEarnings not present");
        //        Assert.AreEqual(lowerMonthlyEarnings, editedbandCollectionsResulsts.LowerMonthlyEarnings, "injected LowerMonthlyEarnings not present");
        //        Assert.AreEqual(rateOne, editedbandCollectionsResulsts.RateOne, "injected RateOne not present");
        //        Assert.AreEqual(rateTwo, editedbandCollectionsResulsts.RateTwo, "injected RateTwo not present");

        //    }
        //}

        //[TestMethod]
        //[ChromeUiTest("StaffNationalInsuranceRates", "P1")]
        //public void Delete_existing_National_Insurance_Rates_record_as_TestUser()
        //{
        //    //Inject NI Rates Record with a unique Start Date, along with some valid NI Bands
        //    //Navigate to the NI Rates Page as a PO
        //    //Search for and load the injected NI record
        //    //Delete record
        //    //Assert Delete success
        //    DateTime startDate = DataAccessor.GetClosestAvailableDateBefore(DateTime.Today, "NationalInsuranceRate", "StartDate");
        //    DateTime endDate = startDate.AddYears(1).AddDays(-1);

        //    string upperMonthlyEarnings = "10.00";
        //    string lowerMonthlyEarnings = "0";
        //    string rateOne = "2.0000";
        //    string rateTwo = "4.0000";

        //    Guid nationalInsuranceID = Guid.NewGuid();
        //    Guid bandCollectionID = Guid.NewGuid();

        //    var testdata = new DataPackage[]
        //    {
        //    GetNationalInsuranceRate(nationalInsuranceID, bandCollectionID, upperMonthlyEarnings, lowerMonthlyEarnings, rateOne, rateTwo, startDate, endDate),
        //    };

        //    using (new DataSetup(testdata))
        //    {
        //        //Login as School Test User and navigate to National Insurance Rates screen

        //        SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);
        //        AutomationSugar.NavigateMenu("Tasks", "Staff", "National Insurance Rates");

        //        // select existing national insurance rates

        //        var nationalInsuranceRateTriplet = new NationalInsuranceRateTriplet();
        //        nationalInsuranceRateTriplet.SearchCriteria.SearchByStartDate = startDate.ToShortDateString();
        //        var nationalInsuranceRateSearchResults = nationalInsuranceRateTriplet.SearchCriteria.Search();
        //        var nationalInsuranceRateSearchTile = nationalInsuranceRateSearchResults.Single(t => t.Code.Equals("Start Date: " + startDate.ToShortDateString()));
        //        var nationalInsuranceRateDetailsPage = nationalInsuranceRateSearchTile.Click<NationalInsuranceRatesPage>();
        //        nationalInsuranceRateDetailsPage.ClickDelete();
        //        nationalInsuranceRateDetailsPage.Refresh();
        //        nationalInsuranceRateTriplet.SearchCriteria.SearchByStartDate = startDate.ToShortDateString();
        //        nationalInsuranceRateSearchResults = nationalInsuranceRateTriplet.SearchCriteria.Search();

        //        nationalInsuranceRateSearchTile = nationalInsuranceRateSearchResults.SingleOrDefault(t => t.Code.Equals("Start Date: " + startDate.ToShortDateString()));

        //        Assert.IsNull(nationalInsuranceRateSearchTile, "Delete National Insurance Rates failed");
        //        new DataSetup(testdata);
        //    }
        //}

        #region Data
        private DataPackage GetNationalInsuranceRate(Guid nationalInsuranceID, Guid bandCollectionID, string upperMonthlyEarnings, string lowerMonthlyEarnings,
            string rateOne, string rateTwo, DateTime startDate, DateTime endDate)
        {

            return this.BuildDataPackage()
            .AddData("NationalInsuranceRate", new
            {
                ID = nationalInsuranceID,
                StartDate = startDate,
                EndDate = endDate,
                TenantID = SeSugar.Environment.Settings.TenantId
            })
             .AddData("BandCollection", new
             {
                 ID = bandCollectionID,
                 BandNumber = 1,
                 LowerMonthlyEarnings = 0,
                 UpperMonthlyEarnings = upperMonthlyEarnings,
                 RateOne = rateOne,
                 RateTwo = rateTwo,
                 NationalInsuranceRate = nationalInsuranceID,
                 TenantID = SeSugar.Environment.Settings.TenantId
             });
        }

        #endregion
    }
}
