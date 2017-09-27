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

namespace Attendance.EarlyYearsProvisions.Tests
{
    public class EarlyYearsProvisionsTests
    {
        #region Story 12189 : Create Early Year Provisions
        [NotDone]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, DataProvider = "TC_001")]
        public void ShouldCreateEarlyYearProvisions(string provisonName, string shortName, string startdate, string endDate, string starttime, string endtime)
        {
            EarlyYearsProvisionsPage page = AttendanceNavigations.NavigateToEarlyYearsProvisionsFromTaskMenu();
            page.ClickCreate();
            page.EnterProvisionName(provisonName, page.MainPageProvisionName);
            page.EnterShortName(shortName, page.MainPageShortName);
            page.EnterNotes("Write Notes");
            page.EnterDate(startdate, page.startDate);
            page.EnterDate(endDate, page.endDate);
            page.EnterTime(starttime, page.startTime);
            page.EnterTime(endtime, page.endTime);
            Detail.Save();
            Assert.IsTrue(page.HasConfirmedSave());
        }
        #endregion

        #region Story 12189 : Default Dates in EarlyYearsProvisions
        [NotDone]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, DataProvider = "TC_001")]
        public void SearchProvisionByProvisionName(string provisonName, string shortName, string startdate, string endDate, string starttime, string endtime)
        {
            Guid provisionId = Guid.NewGuid();
            DataPackage provision = new DataPackage();
            provision.GenerateEarlyYearProvision(provisionId, provisonName, shortName, startdate, starttime, endtime);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: provision))
            {
                EarlyYearsProvisionsPage earlyYearPage = AttendanceNavigations.NavigateToEarlyYearsProvisionsFromTaskMenu();
                earlyYearPage.EnterProvisionName(provisonName, earlyYearPage.SearchPanelProvisionName);
                SearchCriteria.Search();
                SearchResults.WaitForResults();
                Assert.IsTrue(SearchResults.HasResults());
            }
        }
        #endregion

        #region Story 10993 : Search Early Year Provisions  by entering parameter Short Name only.
        [NotDone]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void SearchProvisionByShortName(string provisonName, string shortName, string startdate, string endDate, string starttime, string endtime)
        {
            Guid provisionId = Guid.NewGuid();
            DataPackage provision = new DataPackage();
            provision.GenerateEarlyYearProvision(provisionId, provisonName, shortName, startdate, starttime, endtime);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: provision))
            {
                EarlyYearsProvisionsPage earlyYearPage = AttendanceNavigations.NavigateToEarlyYearsProvisionsFromTaskMenu();
                earlyYearPage.EnterProvisionName(shortName, earlyYearPage.SearchPanelShortName);
                SearchCriteria.Search();
                SearchResults.WaitForResults();
                Assert.IsTrue(SearchResults.HasResults());
            }
        }
        #endregion

        #region Story 12189 : Delete Early Year Provisions Test
        [NotDone]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void DeleteProvision(string provisonName, string shortName, string startdate, string endDate, string starttime, string endtime)
        {
            Guid provisionId = Guid.NewGuid();
            DataPackage provision = new DataPackage();
            provision.GenerateEarlyYearProvision(provisionId, provisonName, shortName, startdate, starttime, endtime);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: provision))
            {
                EarlyYearsProvisionsPage earlyYearPage = AttendanceNavigations.NavigateToEarlyYearsProvisionsFromTaskMenu();
                earlyYearPage.EnterProvisionName(provisonName, earlyYearPage.SearchPanelProvisionName);
                SearchCriteria.Search();
                SearchResults.WaitForResults();
                SearchResults.SelectSearchResult(0);
                earlyYearPage.Delete();
                Assert.IsTrue(earlyYearPage.DeleteDialogDisappeared());
            }
        }
        #endregion

        #region Story 12191 : Mandatory ProvisionName Field Validations
        [NotDone]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, DataProvider = "TC_001")]
        public void ShouldHaveMandataoryProvisionNameField(string shortname, string startdate, string enddate, string starttime, string endtime)
        {
            EarlyYearsProvisionsPage earlyYearPage = AttendanceNavigations.NavigateToEarlyYearsProvisionsFromTaskMenu();
            earlyYearPage.ClickCreate();
            earlyYearPage.EnterShortName(shortname, earlyYearPage.MainPageShortName);
            earlyYearPage.EnterNotes("Write Notes");
            earlyYearPage.EnterDate(startdate, earlyYearPage.startDate);
            earlyYearPage.EnterDate(enddate, earlyYearPage.endDate);
            earlyYearPage.EnterTime(starttime, earlyYearPage.startTime);
            earlyYearPage.EnterTime(endtime, earlyYearPage.endTime);
            Detail.Save();
            Assert.IsTrue(earlyYearPage.IsDisplayedValidationWarning());
        }
        #endregion

        #region Story 12191 : Mandatory ShortName Field Validations
        [NotDone]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, DataProvider = "TC_001")]
        public void ShouldHaveMandataoryShortNameField(string provisionName, string startdate, string enddate, string starttime, string endtime)
        {
            EarlyYearsProvisionsPage earlyYearPage = AttendanceNavigations.NavigateToEarlyYearsProvisionsFromTaskMenu();
            earlyYearPage.ClickCreate();
            earlyYearPage.EnterProvisionName(provisionName, earlyYearPage.MainPageProvisionName);
            earlyYearPage.EnterNotes("Write Notes");
            earlyYearPage.EnterDate(startdate, earlyYearPage.startDate);
            earlyYearPage.EnterDate(enddate, earlyYearPage.endDate);
            earlyYearPage.EnterTime(starttime, earlyYearPage.startTime);
            earlyYearPage.EnterTime(endtime, earlyYearPage.endTime);
            Detail.Save();
            Assert.IsTrue(earlyYearPage.IsDisplayedValidationWarning());

        }
        #endregion

        #region Story 12191 : Mandatory Start Date & Time Fields Validations
        [NotDone]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie}, DataProvider = "TC_001")]
        public void ShouldHaveMandataoryStartDateField(string provisionName, string shortname, string startdate, string starttime, string endtime)
        {
            EarlyYearsProvisionsPage earlyYearPage = AttendanceNavigations.NavigateToEarlyYearsProvisionsFromTaskMenu();
            earlyYearPage.ClickCreate();
          
            earlyYearPage.EnterProvisionName(provisionName, earlyYearPage.MainPageProvisionName);
            earlyYearPage.EnterShortName(shortname, earlyYearPage.MainPageShortName);
            earlyYearPage.EnterNotes("Write Notes");
            earlyYearPage.startDate.Clear();
            earlyYearPage.EnterTime(starttime, earlyYearPage.startTime);
            earlyYearPage.EnterTime(endtime, earlyYearPage.endTime);
            Detail.Save();
            Assert.IsTrue(earlyYearPage.IsDisplayedValidationWarning());
        }
        #endregion

        #region Story 12189 : Max Character Length in various Fields on early year provisions page
        [NotDone]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void VerifyMaxLengthOfProvisionNameField()
        {

            EarlyYearsProvisionsPage earlyYearPage = AttendanceNavigations.NavigateToEarlyYearsProvisionsFromTaskMenu();
            earlyYearPage.ClickCreate();
            Assert.IsTrue((earlyYearPage.MainPageProvisionName.GetAttribute("maxlength") == "100") &&
                (earlyYearPage.MainPageShortName.GetAttribute("maxlength") == "10") && (earlyYearPage.notes.GetAttribute("maxlength") == "2000"));
        }
        #endregion

        #region SchoolAdministrator Permission Test
        [NotDone]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void ShouldDisplayEarlyYearsProvisionsMenuForSchoolAdministrator()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AttendanceNavigations navigationPage = new AttendanceNavigations();
            navigationPage.NavigateToAttendanceMenu();
            Assert.IsTrue(navigationPage.earlyYearsProvisionsSubMenu.Displayed);
        }
        #endregion

        #region ClassTeacher Permission Test
        [NotDone]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void ShouldNotDisplayEarlyYearsProvisionsMenuForClassTeacher()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            AttendanceNavigations navigationPage = new AttendanceNavigations();
            navigationPage.NavigateToAttendanceMenu();
            string loc = "[data-automation-id='task_menu_section_attendance_EarlyYearSetup']";
            Assert.True(navigationPage.SubmenuNotVisibleForClassTeacher(loc));
        }
        #endregion

        #region Common Stuffs
        public List<object[]> TC_001()
        {
            string provisonName = SeleniumHelper.GenerateRandomString(10);
            string shortName = SeleniumHelper.GenerateRandomString(5);
            string startdate = DateTime.Now.ToShortDateString();
            string endDate = DateTime.Now.AddDays(5).ToShortDateString();
            string starttime = DateTime.Now.ToShortTimeString();
            string endtime = DateTime.Now.ToShortTimeString();

            var data = new List<Object[]>
            {
                new object[] {provisonName, shortName, startdate, endDate, starttime, endtime }
            };
            return data;
        }
        #endregion

    }
}
