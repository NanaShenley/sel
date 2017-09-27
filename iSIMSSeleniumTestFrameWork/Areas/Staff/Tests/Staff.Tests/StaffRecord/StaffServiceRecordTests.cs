using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Base;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using WebDriverRunner.internals;

namespace Staff.Tests.StaffRecord
{
    [TestClass]
    public class StaffServiceRecordTests
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
        [ChromeUiTest("StaffServiceRecord", "P1", "Create_new_Service_Record_for_existing_Staff_as_PO")]
        public void Create_new_Service_Record_for_existing_Staff_as_PO()
        {
            Guid staffId, serviceRecordId;
            string staffSurname = Utilities.GenerateRandomString(5, "Staff");

            DataPackage testData = new DataPackage();

            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, DateTime.Today.AddDays(-1), null));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                staffRecordTriplet.SearchCriteria.IsCurrent = true;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();
                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                staffRecord.SaveStaff();
                staffRecord.Refresh();

                GridComponent<StaffRecordPage.ServiceRecord> serviceRecordGrid = staffRecord.ServiceRecordTable;

                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows);
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count);
                Assert.AreEqual(DateTime.Today.AddDays(-1).ToShortDateString(), staffRecord.ServiceRecordTable.Rows[0].DOA);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceRecord", "P1", "Read_existing_Service_Record_for_existing_Staff_as_PO")]
        public void Read_existing_Service_Record_for_existing_Staff_as_PO()
        {
            Guid staffId, serviceRecordId;
            string staffSurname = Utilities.GenerateRandomString(5, "Staff");

            DataPackage testData = new DataPackage();

            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, DateTime.Today.AddDays(-1), null));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                staffRecordTriplet.SearchCriteria.IsCurrent = true;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();
                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                GridComponent<StaffRecordPage.ServiceRecord> serviceRecordGrid = staffRecord.ServiceRecordTable;

                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows);
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count);
                Assert.AreEqual(DateTime.Today.AddDays(-1).ToShortDateString(), staffRecord.ServiceRecordTable.Rows[0].DOA);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceRecord", "P1", "Update_existing_Service_Record_for_existing_Staff_as_PO")]
        public void Update_existing_Service_Record_for_existing_Staff_as_PO()
        {
            Guid staffId, serviceRecordId;
            string staffSurname = Utilities.GenerateRandomString(5, "Staff");

            DataPackage testData = new DataPackage();

            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, DateTime.Today.AddDays(-1), null));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                staffRecordTriplet.SearchCriteria.IsCurrent = true;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();
                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();
                staffRecord.DateOfArrival = DateTime.Today.AddDays(-2).ToShortDateString();
                staffRecord.SaveStaff();

                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows);
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count);
                Assert.AreEqual(DateTime.Today.AddDays(-2).ToShortDateString(), staffRecord.ServiceRecordTable.Rows[0].DOA);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceRecord", "P1", "Delete_Service_Record_for_existing_Staff_as_PO")]
        public void Delete_Service_Record_for_existing_Staff_as_PO()
        {
            Guid staffId, serviceRecordId;
            string staffSurname = Utilities.GenerateRandomString(5, "Staff");

            DataPackage testData = new DataPackage();

            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, DateTime.Today.AddDays(-1), null));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, DateTime.Today.AddDays(-100), DateTime.Today.AddDays(-10)));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                staffRecordTriplet.SearchCriteria.IsCurrent = true;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();
                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows);
                Assert.AreEqual(2, staffRecord.ServiceRecordTable.Rows.Count);

                staffRecord.ServiceRecordTable.Rows[0].DeleteRow();

                ConfirmRequiredDialog confirmRequiredDialog = staffRecord.SaveStaffWithDeleteConfirmation();
                confirmRequiredDialog.ClickContinueWithDelete();
                staffRecord.Refresh();

                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows);
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count);
                Assert.IsFalse(staffRecord.ServiceRecordTable.Rows.Exists(x => new Guid(x.RowId).Equals(serviceRecordId)));
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceRecord", "P1", "Cannot_Delete_Only_Service_Record_as_PO")]
        public void Cannot_Delete_Only_Service_Record_as_PO()
        {
            Guid staffId, serviceRecordId;
            string staffSurname = Utilities.GenerateRandomString(5, "Staff");

            DataPackage testData = new DataPackage();

            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, DateTime.Today.AddDays(-1), null));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                staffRecordTriplet.SearchCriteria.IsCurrent = true;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();
                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                Assert.IsNotNull(staffRecord.ServiceRecordTable.Rows);
                Assert.AreEqual(1, staffRecord.ServiceRecordTable.Rows.Count);

                staffRecord.ServiceRecordTable.Rows[0].DeleteRow();
                staffRecord.SaveStaff();

                List<string> validationErrors = staffRecord.ValidationErrors.ToList();

                Assert.AreEqual(1, validationErrors.Count);
                Assert.IsTrue(validationErrors.Contains("At least one Service record should be present."));
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceRecord", "P2", "Date_of_Arrival_cannot_be_null_when_adding_Service_Record_as_PO")]
        public void Date_of_Arrival_cannot_be_null_when_adding_Service_Record_as_PO()
        {
            Guid staffId, serviceRecordId;
            string staffSurname = Utilities.GenerateRandomString(5, "Staff");

            DataPackage testData = new DataPackage();

            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, DateTime.Today.AddDays(-100), DateTime.Today.AddDays(10)));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                staffRecordTriplet.SearchCriteria.IsCurrent = true;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();
                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                ServiceRecordDialog serviceRecord = staffRecord.ClickAddServiceRecord();

                serviceRecord.ClickOk();

                List<string> validationErrors = serviceRecord.ValidationErrors.ToList();

                Assert.AreEqual(1, validationErrors.Count);
                Assert.IsTrue(validationErrors.Contains("Date of Arrival is required."));
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffServiceRecord", "P2", "Dates_cannot_overlap_when_adding_or_editing_Service_Record_as_PO")]
        public void Dates_cannot_overlap_when_adding_or_editing_Service_Record_as_PO()
        {
            Guid staffId, serviceRecordId;
            string staffSurname = Utilities.GenerateRandomString(5, "Staff");

            DataPackage testData = new DataPackage();

            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, staffSurname));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, DateTime.Today.AddDays(-100), null));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = staffSurname;
                staffRecordTriplet.SearchCriteria.IsCurrent = true;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                SearchResultsComponent<StaffRecordTriplet.StaffRecordSearchResultTile> searchResultTiles = staffRecordTriplet.SearchCriteria.Search();
                StaffRecordTriplet.StaffRecordSearchResultTile searchResult = searchResultTiles.Single();
                StaffRecordPage staffRecord = searchResult.Click<StaffRecordPage>();

                ServiceRecordDialog serviceRecord = staffRecord.ClickAddServiceRecord();

                serviceRecord.DOA = DateTime.Today.ToShortDateString();
                serviceRecord.ClickOk();

                staffRecord.SaveStaff();

                List<string> validationErrors = staffRecord.ValidationErrors.ToList();

                Assert.AreEqual(1, validationErrors.Count);
                Assert.IsTrue(validationErrors.Contains("Service Record dates cannot overlap. Please ensure that your existing entries have a Date of Leaving that precedes the Date of Arrival of your new entry."));
            }
        }
    }
}
