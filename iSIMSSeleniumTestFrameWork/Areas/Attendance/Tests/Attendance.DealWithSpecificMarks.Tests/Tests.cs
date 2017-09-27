using Attendance.Components.AttendancePages;
using Attendance.POM.DataHelper;
using NUnit.Framework;
using POM.Components.Attendance;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using Selene.Support.Attributes;
using SeSugar.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using TestSettings;
using SharedComponents.CRUD;
using Attendance.POM.Entities;
using System.Linq;
using SeSugar.Automation;
using OpenQA.Selenium;
using WebDriverRunner.webdriver;

namespace Attendance.DealWithSpecificMarks.Tests
{
    public class Tests
    {
        /// <summary>
        /// Author: Goyal, Gaurav
        /// Description: As School Administrator : Verify Default Values of Search Criteria.
        /// Status: PASS
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_001")]
        public void VerifyDefaultValuesOf_DealWithSpecificMarks_SearchPanel(string academicyear, string Term, string startdate, string endDate)
        {

            DealWithSpecifcMarksTriplet dealwithspecificmarktriplet = AttendanceNavigations.NavigateToDealWithSpecificMarksMenuPage();
            var searchpanel = dealwithspecificmarktriplet.SearchCriteria;
            Assert.IsTrue(searchpanel.SelectAcademicYear == academicyear && searchpanel.SelectDateRange == Term
                && searchpanel._searchStartDateTexBox.GetValue() == startdate && searchpanel._searchEndDateTextBox.GetValue() == endDate);
        }

        /// <summary>
        /// Author: Goyal, Gaurav
        /// Description: As School Administrator : Verify Codes in Mark Dropdown of Seach Criteria.
        /// Status: PASS
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyActiveCodesList_MarksDropdwon()
        {
            DealWithSpecifcMarksTriplet dealwithspecificmarktriplet = AttendanceNavigations.NavigateToDealWithSpecificMarksMenuPage();
            var searchpanel = dealwithspecificmarktriplet.SearchCriteria;
            Assert.IsTrue(searchpanel.GetAllMarks());
        }

        /// <summary>
        /// Author: Goyal, Gaurav
        /// Description: As School Administrator : Search For Whole School.
        /// Status: PASS
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_001")]
        public void SearchMarksForWholeSchool(string academicyear, string Term, string startdate, string endDate)
        {
            DealWithSpecifcMarksTriplet dealwithspecificmarktriplet = AttendanceNavigations.NavigateToDealWithSpecificMarksMenuPage();
            var searchpanel = dealwithspecificmarktriplet.SearchCriteria;
            searchpanel.SelectAcademicYear = academicyear;
            searchpanel.SelectDateRange = Term;
            searchpanel.SelectMark = "-";
            searchpanel.SelectWholeSchool();
            var dealwithpage = searchpanel.Search<DealWithSpecificMarkPage>();
            if (dealwithpage.IsValidationMessageDisplay())
            {
                Console.WriteLine("It's Non-working Day");
                return;
            }

            var headerTitle = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='deal_with_specific_marks_header']")).Text;

            Assert.IsTrue(headerTitle.Contains("Whole School"));
        }

        /// <summary>
        /// Author: Goyal, Gaurav
        /// Description: As School Administrator : Verify FloodFill Functionality of Mark and Comment Column on Detail Page.
        /// Status: PASS
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_002")]
        public void VerifyFloodFill_Of_MarksAndCommentsSection(string registerdate, string YearGroup, string pupilForeName, string pupilSurName,
            string gender, string dateOfBirth, string DateOfAdmission, string pupilName)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editmarksTriplet = new EditMarksTriplet();
            editmarksTriplet.SearchCriteria.StartDate = registerdate;
            editmarksTriplet.SearchCriteria.Week = true;
            editmarksTriplet.SearchCriteria.SelectYearGroup(YearGroup);
            var editmarkPage = editmarksTriplet.SearchCriteria.Search<EditMarksPage>();

            editmarkPage.ModePreserve = false;
            var editmarkTable = editmarkPage.Marks;
            IEnumerable<SchoolAttendanceCode> getANRs = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = getANRs.Select(x => x.Code).ToList<string>();

            if (codes.Contains(editmarkTable[1][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Holidays");
                return;
            }
            // FloodFill data
            var markGridColumns = editmarkPage.Marks.Columns;
            markGridColumns[2].TimeIndicatorSelected = "AM";
            editmarkPage.CodeList = "L";
            editmarkPage.ClickSave();

            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Deal with Specific Marks");
            AutomationSugar.WaitForAjaxCompletion();
            DealWithSpecifcMarksTriplet dealwithspecificmarktriplet = new DealWithSpecifcMarksTriplet();
            dealwithspecificmarktriplet.SearchCriteria.SelectDateRange = "Last 7 days";
            dealwithspecificmarktriplet.SearchCriteria.SelectMark = "L";
            dealwithspecificmarktriplet.SearchCriteria.SelectYearGroup(YearGroup);
            var dealwithpage = dealwithspecificmarktriplet.SearchCriteria.Search<DealWithSpecificMarkPage>();

            var dealwithTable = dealwithpage.DealWithSpecificMarkTable;

            var dealGridColumns = dealwithpage.DealWithSpecificMarkTable.Columns;

            //Apply Floodfill on Mark Column
            dealwithTable.Columns[3].Select();
            dealwithTable.Columns[3].ClickDownArrow();
            dealwithTable.FloodFillMarks = "N";
            dealwithTable.OverrideMarks = true;
            dealwithTable.ApplySelectedModeOfMarks();

            //Apply Floodfill on Comments Column
            dealwithTable.Columns[5].Select();
            dealwithTable.Columns[5].ClickDownArrow();
            dealwithTable.FloodFillComments = SeleniumHelper.GenerateRandomString(20);
            dealwithTable.OverrideComments = true;
            dealwithTable.ApplySelectedModeOfComments();

            dealwithTable.ClickSave();

            Assert.IsTrue(AutomationSugar.SuccessMessagePresent(dealwithpage.ComponentIdentifier));
        }

        /// <summary>
        /// Author: Goyal, Gaurav
        /// Description: As School Administrator : Verify minute late can only be entered for L or U code.
        /// Status: PASS
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_002")]
        [Variant(Variant.AllNI)]
        public void VerifyMinuteLates_CanOnly_BeEntered_For_L_OR_U_Code(string registerdate, string YearGroup, string pupilForeName, string pupilSurName,
            string gender, string dateOfBirth, string DateOfAdmission, string pupilName)
        {
            #region Pre-Condition

            //Add new Pupil
            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.CreatePupil(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, YearGroup);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: true, purgeAfterTest: true, packages: BuildPupilRecord);

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editmarksTriplet = new EditMarksTriplet();
            editmarksTriplet.SearchCriteria.StartDate = registerdate;
            editmarksTriplet.SearchCriteria.Week = true;
            editmarksTriplet.SearchCriteria.SelectYearGroup(YearGroup);
            var editmarkPage = editmarksTriplet.SearchCriteria.Search<EditMarksPage>();

            editmarkPage.ModePreserve = false;
            editmarkPage.ModeHorizontal = true;

            var editmarkTable = editmarkPage.Marks;

            IEnumerable<SchoolAttendanceCode> getANRs = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = getANRs.Select(x => x.Code).ToList<string>();

            if (codes.Contains(editmarkTable[1][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Holidays");
                return;
            }
            var BlankMarkAM = editmarkTable[pupilName][2].Text = "B";
            var BlankMarkPM = editmarkTable[pupilName][3].Text = "B";

            editmarkPage.ClickSave();

            #endregion
            
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Deal with Specific Marks");
            Wait.WaitLoading();
            DealWithSpecifcMarksTriplet dealwithspecificmarktriplet = new DealWithSpecifcMarksTriplet();
            dealwithspecificmarktriplet.SearchCriteria.SelectDateRange = "Last 7 days";
            dealwithspecificmarktriplet.SearchCriteria.SelectMark = "B";
            dealwithspecificmarktriplet.SearchCriteria.SelectYearGroup(YearGroup);
            var dealwithpage = dealwithspecificmarktriplet.SearchCriteria.Search<DealWithSpecificMarkPage>();

            var dealwithTable = dealwithpage.DealWithSpecificMarkTable;

            //dealwithTable[pupilName][4].Text = "10";
            //IWebElement el = WebContext.WebDriver.FindElement(By.CssSelector(".tooltip.fade.bottom.in"));
            //Assert.That(el.Text == String.Empty, "For Other Than L and U codes, Minute Late Can't be applied");

            dealwithTable.Columns[4].Select();

            dealwithTable.Columns[4].ClickDownArrow();
            dealwithTable.FloodFillMinuteLate = "10";
            dealwithTable.OverrideMinuteLate = true;
            dealwithTable.ApplySelectedModeOfMinuteLate();

            Assert.IsTrue(dealwithTable[2][4].Text == "Minutes Late can only be recorded against code L or U");

        }

        /// <summary>
        /// Author: Goyal, Gaurav
        /// Description: As School Administrator : Verify End to End functionality of Deal with Specific marks screen.
        /// Status: PASS
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_003")]
        public void Should_Deal_With_SpecificMarks(string registerdate, string YearGroup, string pupilForeName, string pupilSurName,
            string gender, string dateOfBirth, string DateOfAdmission, string pupilName)
        {
            #region Pre-Condition

            //Add new Pupil
            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.CreatePupil(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, YearGroup);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: true, purgeAfterTest: true, packages: BuildPupilRecord);

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editmarksTriplet = new EditMarksTriplet();
            editmarksTriplet.SearchCriteria.StartDate = registerdate;
            editmarksTriplet.SearchCriteria.Week = true;
            editmarksTriplet.SearchCriteria.SelectYearGroup(YearGroup);
            var editmarkPage = editmarksTriplet.SearchCriteria.Search<EditMarksPage>();

            editmarkPage.ModePreserve = false;
            editmarkPage.ModeHorizontal = true;

            var editmarkTable = editmarkPage.Marks;

            IEnumerable<SchoolAttendanceCode> getANRs = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = getANRs.Select(x => x.Code).ToList<string>();

            if (codes.Contains(editmarkTable[1][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Holidays");
                return;
            }
            var BlankMarkAM = editmarkTable[pupilName][2].Text = "L";
            var BlankMarkPM = editmarkTable[pupilName][3].Text = "L";

            editmarkPage.ClickSave();

            #endregion
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Deal with Specific Marks");
            Wait.WaitLoading();
            DealWithSpecifcMarksTriplet dealwithspecificmarktriplet = new DealWithSpecifcMarksTriplet();
            dealwithspecificmarktriplet.SearchCriteria.SelectDateRange = "Last 7 days";   
            dealwithspecificmarktriplet.SearchCriteria.SelectMark = "L";
            dealwithspecificmarktriplet.SearchCriteria.SelectYearGroup(YearGroup);
            var dealwithpage = dealwithspecificmarktriplet.SearchCriteria.Search<DealWithSpecificMarkPage>();

            var dealwithTable = dealwithpage.DealWithSpecificMarkTable;

            var dealGridColumns = dealwithpage.DealWithSpecificMarkTable.Columns;
            
            dealwithTable.Columns[3].Select();

            dealwithTable.Columns[3].ClickDownArrow();
            dealwithTable.FloodFillMarks = "N";
            dealwithTable.OverrideMarks = true;
            dealwithTable.ApplySelectedModeOfMarks();

            dealwithpage.Save();

            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");
            var editmarksTriplet1 = new EditMarksTriplet();
            //editmarksTriplet1.SearchCriteria.StartDate = registerdate;
            editmarksTriplet1.SearchCriteria.Week = true;
            //editmarksTriplet1.SearchCriteria.SelectYearGroup(YearGroup);
            var editmarkPage1 = editmarksTriplet1.SearchCriteria.Search<EditMarksPage>();

            var editmarkTable1 = editmarkPage1.Marks;
            Assert.AreEqual("N", editmarkTable1[pupilName][2].Value, "The selected cells cannot be upated from Deal With Specific Marks Screen");
            Assert.AreEqual("N", editmarkTable1[pupilName][3].Value, "The selected cells cannot be upated from Deal With Specific Marks Screen");
        }

        // <summary>
        /// Auth : Gazbare, Swapnil
        /// Desc : Verify the X code in Deal with specific mark screen for England
        /// </summary>
        [WebDriverTest(Enabled = false, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStateSecondary)]
        public void VerifyXcodeForEngland()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Deal with Specific Marks");
            DealWithSpecifcMarksTriplet dealwithSpecific = new DealWithSpecifcMarksTriplet();
            Assert.IsTrue(dealwithSpecific.SearchCriteria.VerifySpecificCodeInDropdown("X"));
        }

        // <summary>
        /// Auth : Gazbare, Swapnil
        /// Desc : Verify the X code in Deal with specific mark screen for Welsh
        /// </summary>
        [WebDriverTest(Enabled = false, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.AllWelsh)]
        public void VerifyXcodeForWelsh()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Deal with Specific Marks");
            DealWithSpecifcMarksTriplet dealwithSpecific = new DealWithSpecifcMarksTriplet();
            Assert.IsTrue(dealwithSpecific.SearchCriteria.VerifySpecificCodeInDropdown("X"));
        }

        // <summary>
        /// Auth : Gazbare, Swapnil
        /// Desc : Verify the ! code in Deal with specific mark screen for NI region
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.AllNI)]
        public void VerifyExclamationMarkInNI()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Deal with Specific Marks");
            DealWithSpecifcMarksTriplet dealwithSpecific = new DealWithSpecifcMarksTriplet();
            Assert.IsTrue(dealwithSpecific.SearchCriteria.VerifySpecificCodeInDropdown("!"));
        }

        
        // <summary>
        /// Auth : Gazbare, Swapnil
        /// Desc : Verify the class column on Deal with specific mark screen for ALL region
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.AllNI)]
        public void  VerifyClassColoumnasDefault()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Deal with Specific Marks");
            DealWithSpecifcMarksTriplet dealwithspecificmarktriplet = new DealWithSpecifcMarksTriplet();

            Wait.WaitForDocumentReady();
            dealwithspecificmarktriplet.SearchCriteria.SelectDateRange = "Current Term";
            dealwithspecificmarktriplet.SearchCriteria.SelectMark = "-";
            AttendanceNavigations.SelectClass("6A");
            dealwithspecificmarktriplet.SearchCriteria.Search<DealWithSpecificMarkPage>();
            Wait.WaitForDocumentReady();
            //  SeleniumHelper.Sleep(50);
            var classcolumn = SeleniumHelper.FindElement(By.XPath("//span[@class='header-text'][text()='Class']"));
            Assert.AreNotEqual(null, classcolumn);
            
        }

                

        #region Data Provider
        public List<object[]> TC_001()
        {
            string startdate = DateTime.Now.AddDays(-6).ToShortDateString();
            string endDate = DateTime.Now.ToShortDateString();
            string mark = "-";
            string academicyear = SeleniumHelper.GetAcademicYear(DateTime.Now);
            string Term = "Last 7 days";

            var data = new List<Object[]>
            {
                new object[] {academicyear, Term, startdate, endDate}
            };
            return data;
        }

        public List<object[]> TC_002()
        {
            string pattern = "dd/mm/yyyy";
            string pupilSurName = SeleniumHelper.GenerateRandomString(6);
            string pupilForeName = SeleniumHelper.GenerateRandomString(6);
            string pupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            string dateOfBirth = DateTime.ParseExact("18/10/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("19/10/2012", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string registerdate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();

            var data = new List<Object[]>
            {
                new object[] {registerdate, "Year 1", pupilForeName, pupilSurName,
             "Male", dateOfBirth,  DateOfAdmission,  pupilName}
            };
            return data;
        }

        public List<object[]> TC_003()
        {
            string pattern = "dd/mm/yyyy";
            string pupilSurName = SeleniumHelper.GenerateRandomString(6);
            string pupilForeName = SeleniumHelper.GenerateRandomString(6);
            string pupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            string dateOfBirth = DateTime.ParseExact("18/10/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("19/10/2012", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string registerdate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();

            var data = new List<Object[]>
            {
                new object[] {registerdate, "Year 2", pupilForeName, pupilSurName,
             "Male", dateOfBirth,  DateOfAdmission,  pupilName}
            };
            return data;
        }

        #endregion
    }

}
