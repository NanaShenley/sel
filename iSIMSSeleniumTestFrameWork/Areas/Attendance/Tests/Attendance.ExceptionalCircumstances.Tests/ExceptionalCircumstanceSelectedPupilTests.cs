using System;
using System.Collections.Generic;
using WebDriverRunner.internals;
using System.Threading;
using NUnit.Framework;
using SharedComponents.CRUD;
using Attendance.Components.AttendancePages;
using Attendance.Components.Common;
using TestSettings;
using POM.Helper;
using Selene.Support.Attributes;

namespace Attendance.ExceptionalCircumstances.Tests
{
    public class ExceptionalCircumstanceSelectedPupilTests
    {

        #region Story 3947 : Mandatory Date Validations
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldHaveMandatorySelectedPupilDateField()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();

            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            ecSelectedPupilPage.EnterDescription("Test Exceptional Circumstances");
            ecSelectedPupilPage.mainPageStartDate.Clear();
            ecSelectedPupilPage.mainPageEndDate.Clear();

            AttendanceSearchPanel searchCriteria = ecSelectedPupilPage.ClickAddPupilLink();
            var checkbox = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            checkbox.Click(SeSugar.Automation.SimsBy.AutomationId("section_menu_Year Group"));
            checkbox.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
            PupilPickerAvailablePupilSection AvailablePupils = searchCriteria.PupilPickerSearchButton();
            AvailablePupils.GetAvailablePupils();
            PupilPickerSelectedPupilSection selectedPupil = AvailablePupils.AddSelectedPupil();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage1 = selectedPupil.ClickExceptionalCircumstances_PupilPickerOkButton();

            ecSelectedPupilPage1.Save();
            Assert.IsTrue(ecSelectedPupilPage1.IsDisplayedValidationWarning());
        }
        #endregion

        #region Story 3947 : Mandatory Session Field Validations
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldHaveMandatorySelectedPupilSessions()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            ecSelectedPupilPage.EnterDescription("Test Exceptional Circumstances");
            ecSelectedPupilPage.SessionStart.Clear();
            ecSelectedPupilPage.SessionEnd.Clear();
            ecSelectedPupilPage.Save();
            Assert.IsTrue(ecSelectedPupilPage.IsDisplayedValidationWarning());
        }
        #endregion

        #region Story 3947: Default Dates in Exceptional Circumstances
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DefaultStartAndEndDateRangeForSelectedPupil()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();

            DateTime DefaultStartDate = Convert.ToDateTime(ecSelectedPupilPage.mainPageStartDate.GetAttribute("value"));
            DateTime DefaultEndDate = Convert.ToDateTime(ecSelectedPupilPage.mainPageEndDate.GetAttribute("value"));

            Assert.That(DefaultStartDate, Is.EqualTo(DateTime.Now.Date));
            Assert.That(DefaultEndDate, Is.EqualTo(DateTime.Now.Date));
        }
        #endregion

        #region Story 3947: Default Sessions in Exceptional Circumstances
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DefaultStartAndEndSessionsForSelectedPupil()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();

            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ECSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();

            string SessionStart = ECSelectedPupilPage.SessionStart.GetValue();
            string SessionEnd = ECSelectedPupilPage.SessionEnd.GetValue();

            Assert.That(SessionStart == "AM" && SessionEnd == "PM");
        }
        #endregion

        #region Story 2333: Verify Controls in Selected Pupil Section
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifySelectedPupilSection()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            Assert.IsTrue(ecSelectedPupilPage.SelectedPupilSection.Displayed && ecSelectedPupilPage.AddPupilLink.Displayed);
        }


        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void PupilPickerSearchPanelShouldContain_Name_Year_Class_And_SearchButton()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            AttendanceSearchPanel searchcriteria = ecSelectedPupilPage.ClickAddPupilLink();
            Assert.IsTrue(searchcriteria.pupilNameFilter.Displayed && searchcriteria.classFilter.Displayed && searchcriteria.yeargroupFilter.Displayed && searchcriteria.searchButton.Displayed);
        }


        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyPupilNameSearchFilter()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            AttendanceSearchPanel searchcriteria = ecSelectedPupilPage.ClickAddPupilLink();
            SearchCriteria.SetCriteria("LegalSurname", "a");
            PupilPickerAvailablePupilSection availablePupils = searchcriteria.PupilPickerSearchButton();
            Assert.IsTrue(availablePupils.searchResults.Displayed);
        }


        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyClassSearchFilter()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            AttendanceSearchPanel searchcriteria = ecSelectedPupilPage.ClickAddPupilLink();        
            var searchCriteria = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            searchCriteria.Click(SeSugar.Automation.SimsBy.AutomationId("section_menu_Year Group"));
            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
            PupilPickerAvailablePupilSection availablePupils = searchcriteria.PupilPickerSearchButton();
            Assert.IsTrue(availablePupils.searchResults.Displayed);
        }


        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyYearGroupSearchFilter()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            AttendanceSearchPanel searchcriteria = ecSelectedPupilPage.ClickAddPupilLink();
            var searchCriteria = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            searchCriteria.Click(SeSugar.Automation.SimsBy.AutomationId("section_menu_Year Group"));
            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
            PupilPickerAvailablePupilSection availablePupils = searchcriteria.PupilPickerSearchButton();
            Assert.IsTrue(availablePupils.searchResults.Displayed);
        }


        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldAddSelectedPupilsInGrid()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            AttendanceSearchPanel searchCriteria = ecSelectedPupilPage.ClickAddPupilLink();
            var checkbox = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            checkbox.Click(SeSugar.Automation.SimsBy.AutomationId("section_menu_Year Group"));
            checkbox.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
            PupilPickerAvailablePupilSection AvailablePupils = searchCriteria.PupilPickerSearchButton();
            AvailablePupils.GetAvailablePupils();
            PupilPickerSelectedPupilSection selectedPupil = AvailablePupils.AddSelectedPupil();
            CreateExceptionalCircumstancesPage app1 = selectedPupil.ClickExceptionalCircumstances_PupilPickerOkButton();
            Assert.IsTrue(app1.Trashicon.Displayed);
        }

        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldRemoveSelectedPupilsFromGrid()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            AttendanceSearchPanel searchcriteria = ecSelectedPupilPage.ClickAddPupilLink();
            var checkbox = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            checkbox.Click(SeSugar.Automation.SimsBy.AutomationId("section_menu_Year Group"));
            checkbox.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
            PupilPickerAvailablePupilSection AvailablePupils = searchcriteria.PupilPickerSearchButton();
            AvailablePupils.GetAvailablePupils();
            PupilPickerSelectedPupilSection selectedPupil = AvailablePupils.AddSelectedPupil();
            CreateExceptionalCircumstancesPage app1 = selectedPupil.ClickExceptionalCircumstances_PupilPickerOkButton();
            app1.RemovePupilFromGrid();
            //Assert.IsFalse(app1.PupilGrid.Displayed);
        }
        #endregion

        #region Story 3947: Create ExceptionalCircumstance For SelectedPupil
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CreateExceptionalCircumstancesForSelectedPupil()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            CreateExceptionalCircumstanceForSelectedPupil(ecSelectedPupilPage);
            //Assert.IsTrue(ECSelectedPupilPage.HasConfirmedSave());
        }
        #endregion

        #region Story 3949,4050: Search and Edit ExceptionCircumstance For SelectedPupil
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void SearchAndEditExceptionalCircumstancesForSelectedPupil()
        {
            CreateExceptionalCircumstancesForSelectedPupil();

            ExceptionalCircumstanceSearchPage ecSearch = new ExceptionalCircumstanceSearchPage();
            string TodayDate = DateTime.Now.ToShortDateString();
            //Enter Dates
            ecSearch.EnterDate(TodayDate, ecSearch.searchCriteriaStartDate);
            ecSearch.EnterDate(TodayDate, ecSearch.searchCriteriaEndDate);

            //Click on Search Button
            ExceptionalCircumstanceSearchResultTile searchResults = ecSearch.ClickSearchButton();
            CreateExceptionalCircumstancesPage ecSelectedPupilUpdate = searchResults.ClickSearchResults();

            ecSelectedPupilUpdate.EnterDescription("Updated Exceptional Circumstances for Selected Pupil");
            ecSelectedPupilUpdate.Save();
            //Assert.IsTrue(ecSelectedPupilUpdate.HasConfirmedSave());
        }
        #endregion

        #region Story 3950: Delete ExceptionCircumstance For SelectedPupil 
        [WebDriverTest(Enabled =true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DeleteExceptionalCircumstancesForSelectedPupil()
        {
            ExceptionalCircumstancePage ecSelectedPupil = AttendanceNavigations.NavigateToExceptionalCircumstancePageFromTaskMenu1();
            ecSelectedPupil.ClickCreate();
            CreateExceptionalCircumstancesPage ecSelectedPupilPage = ecSelectedPupil.ClickSelectedPupilOption();
            CreateExceptionalCircumstanceForSelectedPupil(ecSelectedPupilPage);
            //Click on Delete button
            ecSelectedPupilPage.Delete();
            Assert.IsTrue(ecSelectedPupilPage.DeleteDialogDisappeared());
        }
        #endregion

        #region Common Stuffs
        private void CreateExceptionalCircumstanceForSelectedPupil(CreateExceptionalCircumstancesPage page)
        {
            page.EnterDescription("Test Exceptional Circumstances for Whole School");
            string TodayDate = DateTime.Now.ToShortDateString();
            page.EnterDate(TodayDate, page.mainPageStartDate);
            page.EnterDate(TodayDate, page.mainPageEndDate);
            page.EnterStartSession("AM");
            page.EnterEndSession("PM");
            page.Pupilselector();
            page.Save();
            Wait.WaitLoading();
        }
        #endregion

        #region Common Data
        public List<object[]> TC_001()
        {
            string description = SeleniumHelper.GenerateRandomString(10);
            string startdate = DateTime.Now.ToShortDateString();
            string enddate = DateTime.Now.ToShortDateString();

            var data = new List<Object[]>
            {
                new object[] { description, startdate, enddate}
            };
            return data;
        }
        #endregion

    }
}

