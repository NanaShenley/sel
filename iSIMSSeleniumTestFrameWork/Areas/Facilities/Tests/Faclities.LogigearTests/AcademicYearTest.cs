using System;
using POM.Helper;
using NUnit.Framework;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;
using System.Globalization;
using Facilities.POM.Components.SchoolManagement.Page;
using System.Threading;

namespace Faclities.LogigearTests
{
    public class AcademicYearTest
    {
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Academic_Year()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks","School Management", "Academic Years");
            Wait.WaitForDocumentReady();

            string pattern = "M/d/yyyy";
            var aYTriplet = new NewAcademicYearTriplet();
            var aYPage = aYTriplet.Create();
            aYPage.Next();
            Thread.Sleep(500);
            aYPage.SchoolTermsTable[0].StartDate = DateTime.ParseExact("01/01/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[0].EndDate = DateTime.ParseExact("04/04/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[1].StartDate = DateTime.ParseExact("05/05/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[1].EndDate = DateTime.ParseExact("08/08/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[2].StartDate = DateTime.ParseExact("09/09/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[2].EndDate = DateTime.ParseExact("12/12/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            Thread.Sleep(500);
            aYPage.Next();
            Wait.WaitForDocumentReady();
            Thread.Sleep(500);
            //aYPage.ClickHalfTermHolidayLink();
            //aYPage.HalfTermHolidayTable[0].HalfTermHolidayName = "Half Term 1 Holiday";
            //aYPage.HalfTermHolidayTable[0].HalfTermStartDate = DateTime.ParseExact("02/02/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //aYPage.HalfTermHolidayTable[0].HalfTermEndDate = DateTime.ParseExact("03/03/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //Thread.Sleep(500);
            //aYPage.ClickHalfTermHolidayLink();
            //aYPage.HalfTermHolidayTable[1].HalfTermHolidayName = "Half Term 2 Holiday";
            //aYPage.HalfTermHolidayTable[1].HalfTermStartDate = DateTime.ParseExact("06/06/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //aYPage.HalfTermHolidayTable[1].HalfTermEndDate = DateTime.ParseExact("07/07/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //Thread.Sleep(500);
            //aYPage.ClickHalfTermHolidayLink();
            //aYPage.HalfTermHolidayTable[2].HalfTermHolidayName = "Half Term 3 Holiday";
            //aYPage.HalfTermHolidayTable[2].HalfTermStartDate = DateTime.ParseExact("10/10/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //aYPage.HalfTermHolidayTable[2].HalfTermEndDate = DateTime.ParseExact("11/11/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //Thread.Sleep(500);
            aYPage.Next();
            Thread.Sleep(500);
            aYPage.Next();
            Thread.Sleep(500);
            //aYPage.ClickAddPublicHolidayLink();
            //aYPage.PublicHolidayTable[0].Name = "Public Holiday 1";
            //aYPage.PublicHolidayTable[0].Date = DateTime.ParseExact("10/12/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //Thread.Sleep(500);
            //aYPage.ClickAddPublicHolidayLink();
            //aYPage.PublicHolidayTable[1].Name = "Public Holiday 2";
            //aYPage.PublicHolidayTable[1].Date = DateTime.ParseExact("12/10/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //Thread.Sleep(500);
            //aYPage.ClickAddPublicHolidayLink();
            //aYPage.PublicHolidayTable[2].Name = "Public Holiday 3";
            //aYPage.PublicHolidayTable[2].Date = DateTime.ParseExact("05/09/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //Thread.Sleep(500);
            //aYPage.ClickAddPublicHolidayLink();
            //aYPage.PublicHolidayTable[3].Name = "Public Holiday 4";
            //aYPage.PublicHolidayTable[3].Date = DateTime.ParseExact("09/05/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //Thread.Sleep(500);
            //aYPage.ClickAddPublicHolidayLink();
            //aYPage.PublicHolidayTable[4].Name = "Public Holiday 5";
            //aYPage.PublicHolidayTable[4].Date = DateTime.ParseExact("02/10/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.Next();
            Thread.Sleep(500);
            //aYPage.ClickInsetDayLink();
            //aYPage.InsetDayTable[0].Name = "Inset day 1";
            //aYPage.InsetDayTable[0].Date = DateTime.ParseExact("05/12/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //aYPage.InsetDayTable[0].AM = true;
            //aYPage.InsetDayTable[0].PM = true;
            //Thread.Sleep(500);
            //aYPage.ClickInsetDayLink();
            //aYPage.InsetDayTable[1].Name = "Inset day 2";
            //aYPage.InsetDayTable[1].Date = DateTime.ParseExact("11/05/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            //aYPage.InsetDayTable[1].AM = true;
            aYPage.Next();
            Thread.Sleep(100);
            aYPage.Finish();
            Wait.WaitForDocumentReady();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Delete_Academic_Year()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            Wait.WaitForDocumentReady();

            //  string pattern = "M/d/yyyy";
            var aYTriplet = new NewAcademicYearTriplet();
            var academicyearresult = aYTriplet.SearchCriteria.Search().FirstOrDefault();
            academicyearresult.Click<CreatedAcademicYearPage>();
            Wait.WaitForDocumentReady();
            var deleteAY = aYTriplet.DeleteLatestAY();
            deleteAY.Delete();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Academic_Year_Term_Date_Required_validation()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            Wait.WaitForDocumentReady();

          //  string pattern = "M/d/yyyy";
            var aYTriplet = new NewAcademicYearTriplet();
            var aYPage = aYTriplet.Create();
            aYPage.Next();
            Thread.Sleep(500);
            aYPage.Next();
            var ValidationWarning = SeleniumHelper.Get(NewAcademicYearDetailPage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Academic_Year_Term_Name_Required_validation()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            Wait.WaitForDocumentReady();

            string pattern = "M/d/yyyy";
            var aYTriplet = new NewAcademicYearTriplet();
            var aYPage = aYTriplet.Create();
            aYPage.Next();
            Thread.Sleep(500);
            aYPage.SchoolTermsTable[0].Name = "";
            aYPage.SchoolTermsTable[0].StartDate = DateTime.ParseExact("01/01/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[0].EndDate = DateTime.ParseExact("02/02/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[1].Name = "";
            aYPage.SchoolTermsTable[1].StartDate = DateTime.ParseExact("03/03/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[1].EndDate = DateTime.ParseExact("06/06/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[2].Name = "";
            aYPage.SchoolTermsTable[2].StartDate = DateTime.ParseExact("08/08/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[2].EndDate = DateTime.ParseExact("11/11/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            Thread.Sleep(500);
            aYPage.Next();
            var ValidationWarning = SeleniumHelper.Get(NewAcademicYearDetailPage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Academic_Year_Term_Holiday_Name_Required_validation()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            Wait.WaitForDocumentReady();

            string pattern = "M/d/yyyy";
            var aYTriplet = new NewAcademicYearTriplet();
            var aYPage = aYTriplet.Create();
            aYPage.Next();
            Thread.Sleep(500);
            aYPage.SchoolTermsTable[0].StartDate = DateTime.ParseExact("01/01/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[0].EndDate = DateTime.ParseExact("04/04/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[0].HolidayName = "";
            aYPage.SchoolTermsTable[1].StartDate = DateTime.ParseExact("05/05/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[1].EndDate = DateTime.ParseExact("08/08/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[1].HolidayName = "";
            aYPage.SchoolTermsTable[2].StartDate = DateTime.ParseExact("09/09/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[2].EndDate = DateTime.ParseExact("12/12/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            aYPage.SchoolTermsTable[2].HolidayName = "";
            Thread.Sleep(500);
            aYPage.Next();
            var ValidationWarning = SeleniumHelper.Get(NewAcademicYearDetailPage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
    }
}
