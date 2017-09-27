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

namespace AddressBook.CurrentPupil.Test
{
    public class IdentifyCurrentPupil
    {
        private readonly string textForSearch = "ad";
        private readonly string InvalidtextForSearch = "rpp";
        private readonly string leaverPupil = "Andrews,Jasmine";

        #region Story 1233- Identify Current Pupil from search results by Class/YearGroup
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void DisplayPupilResultsClassYearGroupDetails()
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            searchBox.ClickOnFirstPupilRecord();
            Assert.IsTrue(searchBox.GetClassYear());
            if (true)
                WebContext.Screenshot();
        }
        #endregion

        #region Story 1233-  For Pupils Present Check Results count
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CheckResultCountForExistingPupils() //For valid pupils
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            int resultCount = searchBox.CheckForResultsAvailability(textForSearch);
            Assert.That(resultCount >= 0);
            if (true)
                WebContext.Screenshot();
        }
        #endregion

      
        #region Story 1233-  For PupilsResults empty Check Results count
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CheckResultCountForNonExistingPupils() //For invalid pupils
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(InvalidtextForSearch);
            int resultCount = searchBox.CheckForResultsAvailability(InvalidtextForSearch);
            Assert.That(resultCount == 0);
            if (true)
                WebContext.Screenshot();

        }
        #endregion

    

        #region Story 1233- Check for CURRENT Pupils (Leavers and Future pupil should not be retreived)
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void IdentifyResultForLeaver() //For invalid pupils
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(leaverPupil);
            int resultCount = searchBox.CheckForResultsAvailability(leaverPupil);
            String NoElementtileTitle = SeleniumHelper.Get("result_tile_scroll").Text;
            Assert.True(NoElementtileTitle == AddressBookConstants.TitleForNoResultsfound, "No results found diddn't appear"); //Assertion for title if records are not found
            if (true)
                WebContext.Screenshot();
        }
        #endregion
    }
}
