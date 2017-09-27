using Attendance.Components.AttendancePages;
using NUnit.Framework;
using SharedComponents.CRUD;
using WebDriverRunner.internals;
using Attendance.Components.Common;
using System;
using TestSettings;
using Attendance.POM.DataHelper;
using SeSugar.Data;
using SeSugar.Interfaces;
using System.Collections.Generic;
using POM.Helper;
using POM.Components.Attendance;
using System.Threading;
using Selene.Support.Attributes;
using POM.Components.HomePages;
using System.Linq;

namespace Attendance.ExceptionalCircumstances.Tests
{

    public class ExceptionalCircumstanceWholeSchoolTests
    {
        [WebDriverTest(TimeoutSeconds = 400, Groups = new[] { "P2" }, Enabled =true, Browsers = new[] { BrowserDefaults.Chrome})]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void TC_HP001_Exercise_ability_search_function_ExceptionalCircumstances_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitLoading();
            var homePage = new HomePage();

            //Search with 'Exceptional Circumstances'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Exceptional";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Exceptional Circumstances' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Exceptional Circumstances"));
            Assert.AreNotEqual(null, searchResult, "'Exceptional Circumstances' screen does not display on search result");
            Assert.AreEqual("Exceptional", searchResult.TaskActionHighlight, "'Group' is not underline on 'Exceptional Circumstances'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Exceptional Circumstances'");
        }


        #region Story 3948, 3958 : Two options to create Exceptional Circumstances

        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] {BrowserDefaults.Chrome})]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldHaveTwoOptionsToCreateExceptionalCircumtances()
        {
            var exceptionalCircumstancesTriplet = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu();
            exceptionalCircumstancesTriplet.Create();
            Assert.That((exceptionalCircumstancesTriplet.wholeSchoolMenuItem.Displayed) && (exceptionalCircumstancesTriplet.selectedPupilMenuItem.Displayed));
        }
        #endregion

        #region Story 3959 : Max Character Length in Description Field
        [WebDriverTest(Enabled =true, Browsers = new[] { BrowserDefaults.Chrome})]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void VerifyMaxLengthOfDescriptionFieldOfWholeSchool()
        {
            var exceptionalCircumstancesTriplet = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu();
            exceptionalCircumstancesTriplet.Create();
            var ecWholeSchoolPage = exceptionalCircumstancesTriplet.SelectWholeSchool();
            Assert.IsTrue(ecWholeSchoolPage.descriptionTextBox.Displayed && ecWholeSchoolPage.descriptionTextBox.GetAttribute("maxlength") == "200");
        }
        #endregion

        #region Story 3959 : Mandatory Date Validations
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] {BrowserDefaults.Chrome}, DataProvider = "TC_001")]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldHaveMandatoryDateFieldOfWholeSchool(string description, string startdate, string enddate)
        {
            var exceptionalCircumstancesTriplet = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu();
            exceptionalCircumstancesTriplet.Create();
            var ecWholeSchoolPage = exceptionalCircumstancesTriplet.SelectWholeSchool();
            ecWholeSchoolPage.Description = description;
            ecWholeSchoolPage.startDateTextBox.Clear();
            ecWholeSchoolPage.endDateTextBox.Clear();
            exceptionalCircumstancesTriplet.Save();
            Assert.IsTrue(ecWholeSchoolPage.IsDisplayedValidationWarning());
        }
        #endregion

        #region Story 3959 : Default Dates in Exceptional Circumstances
        [WebDriverTest(Enabled =true, Browsers = new[] { BrowserDefaults.Chrome})]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void DefaultStartAndEndDateRangesOfWholeSchool()
        {
            var exceptionalCircumstancesTriplet = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu();
            exceptionalCircumstancesTriplet.Create();
            var ecWholeSchoolPage = exceptionalCircumstancesTriplet.SelectWholeSchool();
            DateTime DefaultStartDate = Convert.ToDateTime(ecWholeSchoolPage.startDateTextBox.GetValue());
            DateTime DefaultEndDate = Convert.ToDateTime(ecWholeSchoolPage.endDateTextBox.GetValue());

            Assert.That(DefaultStartDate, Is.EqualTo(DateTime.Now.Date));
            Assert.That(DefaultEndDate, Is.EqualTo(DateTime.Now.Date));
        }
        #endregion
         
        #region Story 3959 : Create ExceptionalCircumstances For Whole School
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome}, DataProvider = "TC_001")]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void CreateExceptionalCircumstanceForWholeSchool(string description, string startdate, string enddate)
        {
            var exceptionalCircumstancesTriplet = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu();
            CreateExceptionalCircumstance(description, startdate, enddate);
            Assert.IsTrue(exceptionalCircumstancesTriplet.HasConfirmedSave());

            #region Post-Condition: Delete Exceptional Circumstances if existed

            var exceptionalCircumstancesDetailsPage = new ExceptionalCircumstancesDetailPage();          
            exceptionalCircumstancesDetailsPage.Delete();

            #endregion
        }
        #endregion

        #region Story 3960, 4048 : Search and Edit ExceptionalCircumstances For Whole School
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_002")]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void SearchAndEditExceptionalCircumstancesForWholeSchool(string description, string startdate, string enddate)
        {
            var exceptionalCircumstancesTriplet = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu();

            // Create Exceptional Cirumstance
            CreateExceptionalCircumstance(description, startdate, enddate);

            //Search for existing Exceptional cirumstance
            exceptionalCircumstancesTriplet.SearchCriteria.StartDate = startdate;
            exceptionalCircumstancesTriplet.SearchCriteria.EndDate = enddate;
            var exCirResults = exceptionalCircumstancesTriplet.SearchCriteria.Search();

            var page = exCirResults.FirstOrDefault(x => x.Name.Trim().Equals(description)).Click<ExceptionalCircumstancesDetailPage>();
            Assert.AreNotEqual(null, exCirResults.FirstOrDefault(x => x.Name.Trim().Equals(description)));

            //Edit existing Exceptional cirumstance
            page.Description = description + SeleniumHelper.GenerateRandomString(10);
            page.StartDate = startdate;
            page.EndDate = enddate;
            var ecpage = new ExceptionalCircumstancesTriplet();
            ecpage.ConfirmAndSave();
            Assert.IsTrue(ecpage.HasConfirmedSave());

            //Delete Exceptional Circumstances if existed
            page.Delete();
        }
        #endregion

        #region Story 3961 : Delete ExceptionalCircumstances <Unique Constraint :Run Only Once in Grid>

        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_003")]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void DeleteExceptionalCircumstancesForWholeSchool(string description, string startdate, string enddate)
        {
            var exceptionalCircumstancesTriplet = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu();
            CreateExceptionalCircumstance(description, startdate, enddate);
            var exceptionalCircumstancesDetailsPage = new ExceptionalCircumstancesDetailPage();
            exceptionalCircumstancesDetailsPage.Delete();
            Assert.IsTrue(exceptionalCircumstancesDetailsPage.DeleteDialogDisappeared());        

        }
        #endregion

        #region Valiation Message should be there for overalapping whole school exceptional circumstance
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_001")]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void CreateOverlappingExceptionalCircumstancesForWholeSchool(string description, string startdate, string enddate)
        {
            var exceptionalCircumstancesTriplet = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu();
            CreateExceptionalCircumstance(description, startdate, enddate);
            Wait.WaitLoading();
            exceptionalCircumstancesTriplet.Create();
            ExceptionalCircumstancesDetailPage page = exceptionalCircumstancesTriplet.SelectWholeSchool();
            page.Description = description + SeleniumHelper.GenerateRandomString(10);
            page.StartDate = startdate;
            page.EndDate = enddate;
            page.SessionStart = "AM";
            page.SessionEnd = "PM";
            exceptionalCircumstancesTriplet.ConfirmAndSave();
            Assert.IsTrue(page.IsDisplayedValidationWarning());
        }
        #endregion

        #region SchoolAdministrator Permission Test
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldDisplayExceptionalCircumstanceMenuForSchoolAdministrator()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AttendanceNavigations navigationPage = new AttendanceNavigations();
            navigationPage.NavigateToAttendanceMenu();
            Assert.IsTrue(navigationPage.exceptionalCircumstanceSubMenu.Displayed);
        }
        #endregion

        #region ClassTeacher Permission Test
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldNotDisplayExceptionalCircumstanceMenuForClassTeacher()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            AttendanceNavigations navigationPage = new AttendanceNavigations();
            navigationPage.NavigateToAttendanceMenu();
            string loc = "[data-automation-id='task_menu_section_attendance_ExceptionalCircumstance']";
            Assert.True(navigationPage.SubmenuNotVisibleForClassTeacher(loc));
        }
        #endregion

        #region Common Stuffs

        private void CreateExceptionalCircumstance(string description, string startdate, string enddate)
        {
            //Delete any Existing Exceptional Circumstance
            ExceptionalCircumstancesTriplet exceptionalCircumstancesTriplet = new ExceptionalCircumstancesTriplet();
            exceptionalCircumstancesTriplet.SearchCriteria.StartDate = startdate;
            exceptionalCircumstancesTriplet.SearchCriteria.EndDate = enddate;
            var exCirResults = exceptionalCircumstancesTriplet.SearchCriteria.Search();
            if (SearchResults.SearchResultCount != 0)
            {
                SearchResults.SelectSearchResult(0);
                ExceptionalCircumstancesDetailPage page1 = new ExceptionalCircumstancesDetailPage();
                page1.Delete();
            }
            exceptionalCircumstancesTriplet.Create();
            ExceptionalCircumstancesDetailPage page = exceptionalCircumstancesTriplet.SelectWholeSchool();
            page.Description = description;
            page.StartDate = startdate;
            page.EndDate = enddate;
            page.SessionStart = "AM";
            page.SessionEnd = "PM";
            exceptionalCircumstancesTriplet.ConfirmAndSave();

        }
        #endregion

        #region Common Data
        public List<object[]> TC_001()
        {
            string description= SeleniumHelper.GenerateRandomString(10);
            string startdate= DateTime.Now.AddDays(1).ToShortDateString();
            string enddate = DateTime.Now.AddDays(1).ToShortDateString();

            var data = new List<Object[]>
            {
                new object[] { description, startdate, enddate}
            };
            return data;
        }

        public List<object[]> TC_002()
        {
            string description = SeleniumHelper.GenerateRandomString(10);
            string startdate = DateTime.Now.AddDays(2).ToShortDateString();
            string enddate = DateTime.Now.AddDays(2).ToShortDateString();

            var data = new List<Object[]>
            {
                new object[] { description, startdate, enddate}
            };
            return data;
        }

        public List<object[]> TC_003()
        {
            string description = SeleniumHelper.GenerateRandomString(10);
            string startdate = DateTime.Now.AddDays(3).ToShortDateString();
            string enddate = DateTime.Now.AddDays(3).ToShortDateString();

            var data = new List<Object[]>
            {
                new object[] { description, startdate, enddate}
            };
            return data;
        }
        #endregion
    }
}

