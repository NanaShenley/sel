using Attendance.Components.AttendancePages;
using NUnit.Framework;
using SharedComponents.CRUD;
using System;
using TestSettings;
using WebDriverRunner.internals;
using POM.Helper;
using Attendance.POM.DataHelper;
using SeSugar.Data;
using System.Collections.Generic;
using Selene.Support.Attributes;
using POM.Components.HomePages;
using System.Linq;
using POM.Components.Attendance;
using SeSugar.Automation;
using Attendance.Components.Common;
using System.Globalization;

namespace Attendance.EarlyYearSessionPattern.Tests
{
    public class EarlyYearSessionTests
    {
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome})]
        public void Exercise_ability_search_function_Early_Years_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Early Years Provisions");
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Early Years Session Pattern'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Early Years Session Pattern";

            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Attendance Pattern' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Early Years Session Pattern"));
            Assert.AreNotEqual(null, searchResult, "'Early Years Session Pattern' screen does not display on search result");
            Assert.AreEqual("Early Years Session Pattern", searchResult.TaskActionHighlight, "'Group' is not underline on 'Early Years Session Pattern'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Early Years Session Pattern'");

        }

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_01_Data")]
        public void CheckDefaultValueOfDatePicker(string academicYear, string dropdowntext)
        {
            EarlyYearsSessionPatternDialog page = AttendanceNavigations.NavigateToEarlyYearsSessionPatternFromTaskMenu();
            string defaultdropdownValue = page.SelectDateRangeButtonDefaultValue.Text.Trim();
            string defaultacademicyear = page.AcademicYear;
            DateTime dStartDate = Convert.ToDateTime(page._startDateTextbox.GetValue());
            DateTime dEndDate = Convert.ToDateTime(page._endDateTextbox.GetValue());
            Assert.IsTrue((defaultdropdownValue == dropdowntext) && defaultacademicyear == academicYear && dStartDate == page.weekstartdate() && dEndDate == page.weekEndDate());
        }

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_002_Data")]
        public void ShouldAddSelectedPupilsInGrid(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string dateOfBirth, string DateOfAdmission)
        {
            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.CreatePupil(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, yeargroup);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            EarlyYearsSessionPatternDialog earlyYearDialog = AttendanceNavigations.NavigateToEarlyYearsSessionPatternFromTaskMenu();
            earlyYearDialog.IsPreserve = true;
            earlyYearDialog.StartDate = startDate;
            earlyYearDialog.EndDate = endDate;

            //click on Add Pupils Link
            AddPupilsDialogTriplet addPupilsDialogTriplet = earlyYearDialog.AddPupil();

            //Select Pupil and Search
            addPupilsDialogTriplet.SearchCriteria.SearchByPupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var addPupilDialogSearchPage = addPupilsDialogTriplet.SearchCriteria;
            var addPupilsDetailsPage = addPupilDialogSearchPage.Search<AddPupilsDetailsPage>();
            addPupilsDetailsPage.AddAllPupils();

            // Click on OK button
            addPupilsDialogTriplet.ClickOk();

            var confirmRequiredDialog1 = earlyYearDialog.ClickApply();
            confirmRequiredDialog1.ClickApplyThisPattern();
            Assert.AreEqual(true, earlyYearDialog.IsSuccessMessageDisplayed(), "Success message doesn't display");
            earlyYearDialog.ClosePatternDialog();
        }

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_002_Data")]
        public void ShouldRemoveSelectedPupilsFromGrid(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string dateOfBirth, string DateOfAdmission)
        {
            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.CreatePupil(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, yeargroup);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            EarlyYearsSessionPatternDialog earlyYearDialog = AttendanceNavigations.NavigateToEarlyYearsSessionPatternFromTaskMenu();
            earlyYearDialog.IsOverwrite = true;
            earlyYearDialog.StartDate = startDate;
            earlyYearDialog.EndDate = endDate;

            //click on Add Pupils Link
            AddPupilsDialogTriplet addPupilsDialogTriplet = earlyYearDialog.AddPupil();

            //Select Pupil and Search
            addPupilsDialogTriplet.SearchCriteria.SearchByPupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var addPupilDialogSearchPage = addPupilsDialogTriplet.SearchCriteria;
            var addPupilsDetailsPage = addPupilDialogSearchPage.Search<AddPupilsDetailsPage>();
            addPupilsDetailsPage.AddAllPupils();

            // Click on OK button
            addPupilsDialogTriplet.ClickOk();

            earlyYearDialog.DeletePupil();
            earlyYearDialog.ApplyPattern();

            List<string> validation = earlyYearDialog.Validation.ToList();

            Assert.IsTrue(validation.Contains("Please select at least one Pupil"), "Validation not shown: 'Please select at least one Pupil'");            
        }

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyValidationWarningMessages()
        {
            EarlyYearsSessionPatternDialog page = AttendanceNavigations.NavigateToEarlyYearsSessionPatternFromTaskMenu();
            //page.AcademicYear = "";
            page._startDateTextbox.ClearText();
            page._endDateTextbox.ClearText();
            page.ApplyPattern();

            List<string> validation = page.Validation.ToList();

            Assert.IsTrue(validation.Contains("Start Date field is required."), "Validation not shown: 'Start Date field is required.'");
            Assert.IsTrue(validation.Contains("End Date field is required."), "Validation not shown: 'End Date field is required.'");
            Assert.IsTrue(validation.Contains("Early Year Session Pattern cannot span multiple academic years. Please review the start and end dates."), "Validation not shown: 'Early Year Session Pattern cannot span multiple academic years. Please review the start and end dates.'");
            Assert.IsTrue(validation.Contains("Please select at least one Pupil"), "Validation not shown: 'Please select at least one Pupil'");
        }

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT_002_Data")]
        public void EarlyYearSessionFloodfillFunctionality(string startDate, string endDate, string yeargroup, string pupilForeName, string pupilSurName,
              string pupilName, string dateOfBirth, string DateOfAdmission)
        {
            EarlyYearsSessionPatternDialog earlyYearDialog = AttendanceNavigations.NavigateToEarlyYearsSessionPatternFromTaskMenu();
            earlyYearDialog.IsOverwrite = true;
            earlyYearDialog.StartDate = startDate;
            earlyYearDialog.EndDate = endDate;

            //click on Add Pupils Link
            AddPupilsDialogTriplet addPupilsDialogTriplet = earlyYearDialog.AddPupil();

            //Select Pupil and Search
            addPupilsDialogTriplet.SearchCriteria.YearGroups.GetClassItem(yeargroup);
            var addPupilDialogSearchPage = addPupilsDialogTriplet.SearchCriteria;
            var addPupilsDetailsPage = addPupilDialogSearchPage.Search<AddPupilsDetailsPage>();
            addPupilsDetailsPage.AddAllPupils();

            // Click on OK button
            addPupilsDialogTriplet.ClickOk();

            var earlyYearsTable = earlyYearDialog.SelectPupilsTable;
            //tt.Rows
        }

        #region Data 
        public List<object[]> TC_ATT_01_Data()
        {
            string academicYear = String.Format("{0}", SeleniumHelper.GetAcademicYear(DateTime.Now));
            string dropdowntext = "This Week";

            var data = new List<Object[]>
            {
                new object[] {academicYear,dropdowntext}
            };
            return data;
        }

        public List<object[]> TC_ATT_002_Data()
        {
            string pattern = "M/d/yyyy";
            string pupilSurName = "AH_" + SeleniumHelper.GenerateRandomString(8);
            string pupilForeName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            string dateOfBirth = DateTime.ParseExact("10/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();
            string endDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).AddDays(3).ToShortDateString();

            var data = new List<Object[]>
            {                
                new object[] { startDate, endDate, "Year N", pupilForeName, pupilSurName, pupilName, dateOfBirth, DateOfAdmission}                
            };
            return data;
        }
        #endregion
    }
}