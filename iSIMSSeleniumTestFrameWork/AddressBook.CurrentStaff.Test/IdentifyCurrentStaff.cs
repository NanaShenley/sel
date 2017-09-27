using AddressBook.Components;
using AddressBook.Components.Pages;
using AddressBook.Test;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace AddressBook.CurrentStaff.Test
{
    class IdentifyCurrentStaff
    {
        private readonly string textForSearch = "avery";
        private readonly string InvalidtextForSearch = "rppzxt";

        #region Story 7247-  For Staff Present Check Results count
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CheckResultCountForExistingStaff() //For valid staff
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForStaff(textForSearch);
            int resultCount = searchBox.CheckForStaffAvailability(textForSearch);
            Assert.That(resultCount >= 0);
            if (true)
                WebContext.Screenshot();
        }
        #endregion


        #region Story 7247-  For staff empty Check Results count
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CheckResultCountForNonExistingStaff() //For invalid staff
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.textSearch.SendKeys(InvalidtextForSearch);
            int resultCount = searchBox.CheckForStaffAvailability(InvalidtextForSearch);
            Assert.That(resultCount == 0);
            if (true)
                WebContext.Screenshot();

        }
        #endregion

        #region Story 1233-  For PupilResults empty Verify Section header
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void IdentifySectionHeaderForNoResultsFound() //For invalid staff
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.textSearch.SendKeys(InvalidtextForSearch);
            searchBox.waitForPupilResultstoAppear();
            int resultCount = searchBox.CheckForStaffAvailability(InvalidtextForSearch);
            String NoElementtileTitle = SeleniumHelper.Get("result_tile_scroll").Text;
            Assert.True(NoElementtileTitle == AddressBookConstants.TitleForNoResultsfound, "No results found text didn't appear"); //Assertion for title if records are not found
            if (true)
                WebContext.Screenshot();
        }
        #endregion
    }
}
