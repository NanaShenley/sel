using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Staff.Tests.FMS
{
    //NI Rates are now read only, meaning no users can make changes to the screen.
    [TestClass]
    public class FmsNationalInsuranceRates
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
        //With ref. to:
        //C:\WIP\Dev\iSIMSSeleniumTestFrameWork\Areas\Staff\Tests\Staff.StaffRecord.Tests\NationalInsuranceRateTests.cs
        //[TestMethod]
        //[ChromeUiTest("FMS", "P1")]
        //public void FMS_NationalInsuranceRatePositiveValuesTest()
        //{
        //    DateTime startDateValue = DataAccessor.GetClosestAvailableDateBefore(DateTime.Today, "NationalInsuranceRate", "StartDate");
        //    DateTime endDateValue = startDateValue.AddYears(1).AddDays(-1);
        //    string startDate = startDateValue.ToShortDateString();
        //    string endDate = endDateValue.ToShortDateString();
        //    string upperMonthlyEarnings = "99999.999";
        //    string rateOne = "99999.999";
        //    string rateTwo = "99999.999";

        //    //Login as School Personnel Officer and navigate to Training Course  screen
        //    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);
        //    AutomationSugar.NavigateMenu("Tasks", "Staff", "National Insurance Rates");

        //    //Create new full-time training course
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
        //    gridRow.Refresh();

        //    //Save training course
        //    nationalInsuranceRateDetailsPage.ClickSave();
        //    var validation = nationalInsuranceRatesDetails.Validation.ToList();

        //    Assert.IsTrue(validation.Contains("Upper Monthly Earnings value cannot be higher than 9999.99"));
        //    Assert.IsTrue(validation.Contains("National Insurance Rate Band Collection Upper Monthly Earnings may have only 2 figure(s) after the decimal point."));
        //    Assert.IsTrue(validation.Contains("Rate One has a maximum value of 99.9999"));
        //    Assert.IsTrue(validation.Contains("Rate Two has a maximum value of 99.9999"));

        //}

        //[TestMethod]
        //[ChromeUiTest("FMS", "P1")]
        //public void FMS_NationalInsuranceRateNegativeValuesTest()
        //{
        //    DateTime startDateValue = DataAccessor.GetClosestAvailableDateBefore(DateTime.Today, "NationalInsuranceRate", "StartDate");
        //    DateTime endDateValue = startDateValue.AddYears(1).AddDays(-1);
        //    string startDate = startDateValue.ToShortDateString();
        //    string endDate = endDateValue.ToShortDateString();
        //    string upperMonthlyEarnings = "-99999.999";
        //    string rateOne = "-99999.999";
        //    string rateTwo = "-99999.999";

        //    //Login as School Personnel Officer and navigate to Training Course  screen
        //    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);
        //    AutomationSugar.NavigateMenu("Tasks", "Staff", "National Insurance Rates");

        //    //Create new full-time training course
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
        //    gridRow.Refresh();

        //    //Save training course
        //    nationalInsuranceRateDetailsPage.ClickSave();

        //    var validation = nationalInsuranceRatesDetails.Validation.ToList();
        //    Assert.IsTrue(validation.Contains("Upper Monthly Earnings value cannot be lower than 0.00"));
        //    Assert.IsTrue(validation.Contains("National Insurance Rate Band Collection Upper Monthly Earnings may have only 2 figure(s) after the decimal point."));
        //    Assert.IsTrue(validation.Contains("Rate One has a minimum value of -99.9999"));
        //    Assert.IsTrue(validation.Contains("Rate Two has a minimum value of -99.9999"));
        //}
    }
}
