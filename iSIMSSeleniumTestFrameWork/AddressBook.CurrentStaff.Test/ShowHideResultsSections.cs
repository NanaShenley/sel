using AddressBook.Components;
using AddressBook.Components.Pages;
using AddressBook.Test;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;


namespace AddressBook.CurrentStaff.Test
{
    public class ShowHideResultsSections
    {
        private readonly string invalidtextForSearch = "rpKXTp";
    

        #region Story 7927-Check if Search Result Sections are dynamically Displayed/Hidden (Pupils and Pupil Contacts)
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentStaffQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void DynamicallyDisplayHideResultSection()  //For no results
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.textSearch.SendKeys(invalidtextForSearch);
            searchBox.waitForPupilResultstoAppear();
            String NoResultElementtileTitle = SeleniumHelper.Get("result_tile_scroll").Text;
            Assert.True(NoResultElementtileTitle == AddressBookConstants.TitleForNoResultsfound, "No results found text didn't appear"); //Assertion for title if records are not found
        }
        #endregion



    }
}
       