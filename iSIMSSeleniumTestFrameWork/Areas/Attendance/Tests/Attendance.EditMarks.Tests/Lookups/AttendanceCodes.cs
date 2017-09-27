using Selene.Support.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Components.AttendancePages;
using POM.Helper;
using SeSugar.Automation;
using NUnit.Framework;
using POM.Components.Attendance;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;
using OpenQA.Selenium;
using Attendance.POM.Entities;
using Attendance.POM.DataHelper;
using TestSettings;
using System.Text.RegularExpressions;

namespace Attendance.EditMarks.Tests.Lookups
{
    public class AttendanceCodes : LookupTestsBase
    {
        [ChromeUiTest(Enabled = true, DataProvider = "TC_AT001_DATA")]
        public void Read_AttendanceCodes_Lookup_as_Admin(string code)
        {
            LoginAndNavigate("Attendance Codes");
  
            AttendanceCodesLookupTriplet searchPage = new AttendanceCodesLookupTriplet();
            searchPage.SearchCriteria.CodeOrDescription = code;
            AttendanceCodesLookupDetailsPage detailpage = searchPage.SearchCriteria.Search<AttendanceCodesLookupDetailsPage>();
            var gridRow = detailpage.AttendanceCodes.Rows.FirstOrDefault(x => x.Code == code);

            Assert.AreEqual(code, gridRow.Code);  
        }

        [ChromeUiTest(Enabled = true)]
        public void AttendanceCodes_Lookup_Is_Not_Updateable()
        {
            LoginAndNavigate("Attendance Codes");
            Assert.IsFalse(CanAddLookupItems());
        }

        [ChromeUiTest(Enabled = true)]
        public void Verify_AttendanceCodesCount_OnLookup()
        {
            LoginAndNavigate("Attendance Codes");

            AttendanceCodesLookupTriplet searchPage = new AttendanceCodesLookupTriplet();
            AttendanceCodesLookupDetailsPage detailpage = searchPage.SearchCriteria.Search<AttendanceCodesLookupDetailsPage>();

            var lookUpCodes = detailpage.AttendanceCodes.Rows.Select(x => x.Code).ToList<string>();          

            IEnumerable<SchoolAttendanceCode> allAttendanceCodes = Queries.GetAllAttendanceCodes();

            var results = allAttendanceCodes.Where(m => !lookUpCodes.Contains(m.Code)).ToList();

            Assert.IsTrue(results.Count==0);
        }

        [ChromeUiTest(Enabled = true, DataProvider="TC_AT001_DATA")]
        public void Should_ModifyAndSave_AttendanceCodes_OnLookup(string code)
        {
            LoginAndNavigate("Attendance Codes");

            AttendanceCodesLookupTriplet searchPage = new AttendanceCodesLookupTriplet();
            AttendanceCodesLookupDetailsPage detailpage = searchPage.SearchCriteria.Search<AttendanceCodesLookupDetailsPage>();

            var gridRow = detailpage.AttendanceCodes.Rows.Single(x => x.Code == code);

            gridRow.AvailablefromTakeRegister = false;

            searchPage.ClickSave();
            Assert.IsTrue(SharedComponents.CRUD.Detail.HasConfirmedSave()); 
        }

        [ChromeUiTest(Enabled = true)]
        public void Verify_CantUpdate_ANRCodes_OnLookup()
        {
            LoginAndNavigate("Attendance Codes");

            AttendanceCodesLookupTriplet searchPage = new AttendanceCodesLookupTriplet();

            AttendanceCodesLookupDetailsPage detailpage = searchPage.SearchCriteria.Search<AttendanceCodesLookupDetailsPage>();
            var lookUpCodes = detailpage.AttendanceCodes.Rows.Where(x => x.StatisticalMeaning == "Attendance not required" && x.AvailablefromTakeRegister == false).Select(x=> x.Code).ToList();

            Assert.IsTrue(lookUpCodes.Count == 4);
        }

        [ChromeUiTest(Enabled = true, DataProvider="TC_AT001_DATA")]
        public void UncheckedCodesOnLookup_ShouldNotVisible_OnTakeRegister_CodeDropdown(string code)
        {
            LoginAndNavigate("Attendance Codes");

            AttendanceCodesLookupTriplet searchPage = new AttendanceCodesLookupTriplet();
            AttendanceCodesLookupDetailsPage detailpage = searchPage.SearchCriteria.Search<AttendanceCodesLookupDetailsPage>();

            var gridRow = detailpage.AttendanceCodes.Rows.Single(x => x.Code == code);

            gridRow.AvailablefromTakeRegister = false;

            searchPage.ClickSave();

            SeleniumHelper.Logout();

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Take Register");

            var takeRegisterTriplet = new TakeRegisterTriplet();

            takeRegisterTriplet.SearchCriteria.StartDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();
            takeRegisterTriplet.SearchCriteria.Week = true;
            takeRegisterTriplet.SearchCriteria.SelectYearGroup("Year 1");
            var takeRegisterDetail = takeRegisterTriplet.SearchCriteria.Search<TakeRegisterDetailPage>();
            var takeRegisterTable = takeRegisterDetail.Marks;

            var codeListFromDropdwon = takeRegisterDetail.GetCodeList();

            bool notFound = true;
            foreach (var Expectedcode in codeListFromDropdwon)
            {
                string[] test = Regex.Split(Expectedcode.Text, "-");
                if (test.Length > 0)
                {
                    if (test[0].Trim().ToUpper() == code)
                        notFound = false;
                }
            }

            Assert.IsTrue(notFound);

            #region Post-Condition
            SeleniumHelper.Logout();
            LoginAndNavigate("Attendance Codes");

            AttendanceCodesLookupTriplet searchPage1 = new AttendanceCodesLookupTriplet();
            AttendanceCodesLookupDetailsPage detailpage1 = searchPage1.SearchCriteria.Search<AttendanceCodesLookupDetailsPage>();

            var gridRow1 = detailpage1.AttendanceCodes.Rows.Single(x => x.Code == code);

            gridRow1.AvailablefromTakeRegister = true;

            searchPage1.ClickSave();
            #endregion

        }

        [ChromeUiTest(Enabled = true, DataProvider="TC_AT001_DATA")]
        public void UncheckedCodesOnLookup_ShouldBeVisible_OnEditMarks_CodeDropdown(string code)
        {
            LoginAndNavigate("Attendance Codes");

            AttendanceCodesLookupTriplet searchPage = new AttendanceCodesLookupTriplet();
            AttendanceCodesLookupDetailsPage detailpage = searchPage.SearchCriteria.Search<AttendanceCodesLookupDetailsPage>();

            var gridRow = detailpage.AttendanceCodes.Rows.Single(x => x.Code == code);

            gridRow.AvailablefromTakeRegister = false;

            searchPage.ClickSave();

            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editMarksTripletPage = new EditMarksTriplet();

            editMarksTripletPage.SearchCriteria.StartDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();
            editMarksTripletPage.SearchCriteria.Week = true;
            editMarksTripletPage.SearchCriteria.SelectYearGroup("Year 1");
            var editMarksPage = editMarksTripletPage.SearchCriteria.Search<EditMarksPage>();
            var editmarkTable = editMarksPage.Marks;

            var codeListFromDropdwon = editMarksPage.GetCodeList();

            bool Found = true;
            foreach (var Expectedcode in codeListFromDropdwon)
            {
                string[] test = Regex.Split(Expectedcode.Text, "-");
                if (test.Length > 0)
                {
                    if (test[0].Trim().ToUpper() == code)
                        Found = false;
                }
            }

            Assert.IsTrue(Found);

            #region Post-Condition
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Lookups", "Attendance", "Attendance Codes");
            AttendanceCodesLookupTriplet searchPage1 = new AttendanceCodesLookupTriplet();
            AttendanceCodesLookupDetailsPage detailpage1 = searchPage1.SearchCriteria.Search<AttendanceCodesLookupDetailsPage>();

            var gridRow1 = detailpage1.AttendanceCodes.Rows.Single(x => x.Code == code);

            gridRow1.AvailablefromTakeRegister = true;

            searchPage1.ClickSave();
            #endregion

        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT002_DATA")]
        public void UncheckedCodesOnLookup_ShouldBeVisible_ForExistingMarks_OnTakeRegister(string dateSearch, string code, string YearGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateQuickLink("Edit Marks");

            var editMarksTriplet = new EditMarksTriplet();

            editMarksTriplet.SearchCriteria.StartDate = dateSearch;
            editMarksTriplet.SearchCriteria.Day = true;
            editMarksTriplet.SearchCriteria.SelectYearGroup(YearGroup);
            var editMarkPage = editMarksTriplet.SearchCriteria.Search<EditMarksPage>();

            var editmarkTable = editMarkPage.Marks;

            if (editMarkPage.IsValidationMessageDisplay())
            {
                Console.WriteLine("It's Non-Working Day");
                return;
            }

            IEnumerable<SchoolAttendanceCode> getANRs = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = getANRs.Select(x => x.Code).ToList<string>();

            if (codes.Contains(editmarkTable[1][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on AttendanceNotRequiredCodes");
                return;
            }
            editMarkPage.ModePreserve = false;
            editMarkPage.Marks[0][2].Text = code;
            editMarkPage.Marks[1][2].Text = code;
            editMarkPage.Marks[0][3].Text = code;
            editMarkPage.Marks[1][3].Text = code;

            editMarkPage.ClickSave();

            Wait.WaitForDocumentReady();

            AutomationSugar.NavigateMenu("Lookups", "Attendance", "Attendance Codes");

            AttendanceCodesLookupTriplet searchPage = new AttendanceCodesLookupTriplet();
            AttendanceCodesLookupDetailsPage detailpage = searchPage.SearchCriteria.Search<AttendanceCodesLookupDetailsPage>();

            var gridRow = detailpage.AttendanceCodes.Rows.Single(x => x.Code == code);

            gridRow.AvailablefromTakeRegister = false;

            searchPage.ClickSave();

            SeleniumHelper.Logout();

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Take Register");

            var takeRegisterTriplet = new TakeRegisterTriplet();

            takeRegisterTriplet.SearchCriteria.StartDate = dateSearch;
            takeRegisterTriplet.SearchCriteria.Day = true;
            takeRegisterTriplet.SearchCriteria.SelectYearGroup(YearGroup);
            var takeRegisterDetail = takeRegisterTriplet.SearchCriteria.Search<TakeRegisterDetailPage>();
            var takeRegisterTable = takeRegisterDetail.Marks;

            IEnumerable<SchoolAttendanceCode> ANRs = Queries.GetAttendanceNotRequiredCodes();
            List<string> Codes = ANRs.Select(x => x.Code).ToList<string>();

            if (Codes.Contains(takeRegisterTable[1][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on AttendanceNotRequiredCodes");
                return;
            }

            Assert.IsTrue(takeRegisterDetail.Marks[0][1].Text == code, "Codes are not Equal");
            Assert.IsTrue(takeRegisterDetail.Marks[0][2].Text == code, "Codes are not Equal");
            Assert.IsTrue(takeRegisterDetail.Marks[1][1].Text == code, "Codes are not Equal");
            Assert.IsTrue(takeRegisterDetail.Marks[1][2].Text == code, "Codes are not Equal");

            #region Post-Condition
            SeleniumHelper.Logout();
            LoginAndNavigate("Attendance Codes");

            AttendanceCodesLookupTriplet searchPage1 = new AttendanceCodesLookupTriplet();
            AttendanceCodesLookupDetailsPage detailpage1 = searchPage1.SearchCriteria.Search<AttendanceCodesLookupDetailsPage>();

            var gridRow1 = detailpage1.AttendanceCodes.Rows.Single(x => x.Code == code);

            gridRow1.AvailablefromTakeRegister = true;

            searchPage1.ClickSave();
            #endregion

        }

        #region Data Provider

        public List<object[]> TC_AT001_DATA()
        {
            string code = "L";
            string dateSearch = DateTime.Now.AddDays(-3).ToShortDateString();

            var data = new List<Object[]>
            {
                new object[] { code }

            };
            return data;
        }

        public List<object[]> TC_ATT002_DATA()
        {           
            string code = "L";
            string dateSearch = DateTime.Now.AddDays(-2).ToShortDateString();

            var data = new List<Object[]>
            {
                new object[] { dateSearch, code, "Year 2" }

            };
            return data;
        }
        #endregion

    }
}
