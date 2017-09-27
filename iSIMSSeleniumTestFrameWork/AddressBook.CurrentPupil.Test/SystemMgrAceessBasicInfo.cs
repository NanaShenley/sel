using AddressBook.Components;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;

namespace AddressBook.CurrentPupil.Test
{
    class SystemMgrAccessBasicInfo
    {
        private readonly string textForSearch = "ad";

        #region Story 1234 - Check if basic details are displayed for Pupil - Name and DOB
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SysMgrCanAccessCurrentPupil_DOB_Name()
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationByUserType(SeleniumHelper.iSIMSUserType.SystemManger);
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsTrue(popup.GetPupilBasicDetailsNameDOB());
                 }
        #endregion

        #region Story 1234- Check if gender details are not displayed for Pupil
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SysMgrCanNotAccessPupilGenderDetails()
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationByUserType(SeleniumHelper.iSIMSUserType.SystemManger);
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsTrue(popup.CheckPupilGenderDetailsForBlank());
        }
        #endregion

        #region Story 1234- Check if Email field for pupil are blank
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SysMgrCannotAccessCurrentPupilEmail()
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationByUserType(SeleniumHelper.iSIMSUserType.SystemManger);
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsFalse(popup.IsEmailDisplayed());
        }
        #endregion

        #region Story 1234- Check if Telephone field for pupil are blank
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SysMgrCannotAccessCurrentPupilTelephone()
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationByUserType(SeleniumHelper.iSIMSUserType.SystemManger);
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsFalse(popup.IsPupilTelephoneDisplayed());
        }
        #endregion

        #region Story 1234- Check if Address field for pupil are blank
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SysMgrCanNotAccessCurrentPupilAddress()
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationByUserType(SeleniumHelper.iSIMSUserType.SystemManger);
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsFalse(popup.IsAddressDisplayed());
        }
        #endregion

      
        #region Story 1235 - Check if Pupil Associated  contacts are displayed
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SysMgrCannotAccessCurrentPupilAssociatedContacts()
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationByUserType(SeleniumHelper.iSIMSUserType.SystemManger);
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsFalse(popup.IsPupilAssociactedContactDisplayed());
        }
        #endregion

    }
}
