using AddressBook.Components;
using AddressBook.Components.Pages;
using Facilities.Components.FacilitiesPages;
using NUnit.Framework;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using AddressBook.Test;
using SharedComponents.HomePages;
using Selene.Support.Attributes;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using POM.Components.Staff;
using SeSugar.Data;

namespace AddressBook.CurrentStaff.Test
{
    public class LinksForCurrentStaff
    {
        private readonly string textForSearch = "Gill";

        #region Story 3762- Verify Link to Staff Record
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void LinkToStaffRecordFromStaffInfo()
        {
            

            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = "Andrew";
            var surname = "Gill";
            var startDate = DateTime.Today.AddDays(-1);

            var staffRecordData = StaffRecordPage.CreateStaffRecord(out staffId, forename, surname, startDate);
            DataSetup DataSetStaff = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: staffRecordData);

            #endregion
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForStaff(textForSearch);
            AddressBookPopup popup= searchBox.ClickOnFirstStaffRecord();
            popup.ClickStaffDetailsLink();
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementExists(AddressBookElements.OpenedStaffRecordTab));
            bool value = SeleniumHelper.Get(AddressBookElements.OpenedStaffRecordTab).Displayed;
            Assert.IsTrue(value);
        }
        #endregion



    }
}
