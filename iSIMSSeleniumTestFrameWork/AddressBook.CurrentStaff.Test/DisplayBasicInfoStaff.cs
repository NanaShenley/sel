using AddressBook.Components;
using AddressBook.Test;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System.Collections.Generic;
using TestSettings;
using WebDriverRunner.internals;
using System;
using OpenQA.Selenium;
using AddressBook.Components.Pages;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Interactions;
using POM.Components.Staff;
using SeSugar.Data;
using SeSugar.Automation;

namespace AddressBook.CurrentStaff.Test
{
    public class DisplayBasicInfoStaff
    {
        #region Data Provider for Current Pupil Name options(Legal Surname,Legal Forename,Preferred Surname,Preferred Forename,partial or full name)
        public List<object[]> OtherStaff()
        {
            var res = new List<object[]>
            {
                new object[] {"She"},
                new object[] {"Fortune"},
                new object[] {"Sheila Fortune"},
                new object[] {"Fort"},
                new object[] {"Sheila"}
            };
            return res;
        }
        #endregion

        #region  Story 1234- Check if Basic information of Current staff are displayed
        [WebDriverTest(DataProvider = "OtherStaff", Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)] 
        public void DisplayStaffBasicDetails(string textForSearch)
        {
            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = SeSugar.Utilities.GenerateRandomString(10, "Sheila");
            var surname = SeSugar.Utilities.GenerateRandomString(10, "Fortune");
            var startDate = DateTime.Today.AddDays(-1);

            var staffRecordData = StaffRecordPage.CreateStaffRecord(out staffId, forename, surname, startDate);
            DataSetup DataSetStaff = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: staffRecordData);

            #endregion


            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForStaff(textForSearch);
            AddressBookPopup popup= searchBox.ClickOnFirstStaffRecord();
            Assert.IsTrue(popup.GetStaffBasicDetails());
        }
        #endregion
    }
}
