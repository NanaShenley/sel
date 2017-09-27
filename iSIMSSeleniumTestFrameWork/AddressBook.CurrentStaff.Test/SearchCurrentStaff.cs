using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using AddressBook.Components;
using AddressBook.Components.Pages;
using NUnit.Framework;
using TestSettings;
using WebDriverRunner.internals;
using SharedComponents.Helpers;
using AddressBook.Test;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Interactions;
using SharedComponents.HomePages;
using Selene.Support.Attributes;
using POM.Components.Staff;
using SeSugar.Data;

namespace AddressBook.CurrentStaff.Test
{
    public class SearchCurrentStaff
    {
        private readonly string textForSearch = "Oliver";


        //TODO: Use of Test suite for Test data creation
        #region Data Provider for Current Staff name options(Legal Surname,Legal Forename,Preferred Surname,Preferred Forename,partial or full name)
        public List<object[]> OtherStaff()
        {
            var res = new List<object[]>
            {
                new object[] {"Staff"},
                new object[] {"Staff_AddressBook"},
                new object[] {"Staff_AddressBook Staff_AddressBook"},
                new object[] {"Staf"}
            };
            return res;
        }
        #endregion

        #region  Story 7247- Search for Current Staff by Name
        [WebDriverTest(DataProvider = "OtherStaff", Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SearchCurrentStaffByName(string textForSearch)
        {
            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = SeSugar.Utilities.GenerateRandomString(10, "Staff_AddressBook");
            var surname = SeSugar.Utilities.GenerateRandomString(10, "Staff_AddressBook");
            var startDate = DateTime.Today.AddDays(-1);

            var staffRecordData = StaffRecordPage.CreateStaffRecord(out staffId, forename, surname, startDate);
            DataSetup DataSetStaff = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: staffRecordData);

            #endregion

            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForStaff(textForSearch);
            double millisecs = searchBox.SearchTimeInMillisecs;
            searchBox.Log("Results fetched in " + millisecs + " milliseconds");
            Assert.Less(millisecs, AddressBookConstants.MaxAcceptableSearchTimeInMillisecs);
        }
        #endregion

        #region Story 7247- Check Results appearing in ascending order
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CheckStaffResultsOrder()
        {
            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = SeSugar.Utilities.GenerateRandomString(10, "ResultOrder_Staff");
            var surname = SeSugar.Utilities.GenerateRandomString(10, "ResultOrder_Staff");
            var startDate = DateTime.Today.AddDays(-1);
            var staffRecordData = StaffRecordPage.CreateStaffRecord(out staffId, forename, surname, startDate);
            DataSetup DataSetStaff = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: staffRecordData);
            #endregion
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForStaff(surname);
            var selectedElements = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("search_result_tile_Staff")));
            Assert.IsTrue(selectedElements.Text.Contains("resultorder_staff"));
        }
        #endregion

        #region Story 7247- Check Results appearing in surname, forename format
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CheckStaffResultsFormat()
        {
            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = "ResultFormat_Staff_ForeName";
            var surname = "ResultFormat_Staff_Surname";
            var startDate = DateTime.Today.AddDays(-1);
            var staffRecordData = StaffRecordPage.CreateStaffRecord(out staffId, forename, surname, startDate);
            DataSetup DataSetStaff = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: staffRecordData);
            #endregion
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            POM.Helper.Wait.WaitLoading();
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForStaff(surname);
            var selectedElements = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("search_result_tile_Staff")));
            Assert.IsTrue(selectedElements.Text.Contains("ResultFormat_Staff_Surname, ResultFormat_Staff_ForeName"));
        }
        #endregion
    }
}
