using AddressBook.Components;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;

namespace AddressBook.CurrentPupil.Test.Permissions
{
    class SeniorManagement
    {
        private readonly string textForSearch = "Chris";

        #region SrMgmtQuickControlAccess()
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SrMgmtQuickControlAccess()
        {
            Assert.IsTrue(QuickControlAccess.isQuickControlAccess(SeleniumHelper.iSIMSUserType.SeniorManagementTeam));
        }
        #endregion

        #region SeniorManagementSearchCurrentPupilsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SrMgmtSearchCurrentPupilsAccess()
        {
            Assert.That(QuickControlAccess.hasPermissionToSearchCurrentPupil(SeleniumHelper.iSIMSUserType.SeniorManagementTeam, textForSearch) >= 0);
        }
        #endregion

        #region SeniorManagementPupilBasicDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SrMgmtPupilBasicDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewBasicDetailsCurrentPupil(SeleniumHelper.iSIMSUserType.SeniorManagementTeam, textForSearch));
        }
        #endregion

        #region SeniorManagementPupilTelephoneDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SrMgmtPupilTelephoneDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewPupilTelephoneDetails(SeleniumHelper.iSIMSUserType.SeniorManagementTeam, textForSearch));
        }
        #endregion

        #region SeniorManagementPupilEmailDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SrMgmtPupilEmailDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewPupilEmailDetails(SeleniumHelper.iSIMSUserType.SeniorManagementTeam, textForSearch));
        }
        #endregion

        #region SeniorManagementPupilAddressDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SrMgmtPupilAddressDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewPupilAddressDetails(SeleniumHelper.iSIMSUserType.SeniorManagementTeam, textForSearch));
        }
        #endregion

        #region SeniorManagementLinkToPupilRecordFromPupilInfoAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void SrMgmtLinkToPupilRecordFromPupilInfoAccess()
        {
            Assert.IsTrue(QuickControlAccess.hasAccessLinkToPupilRecordFromPupilInfo(SeleniumHelper.iSIMSUserType.SeniorManagementTeam, textForSearch));
        }
        #endregion
    }
}
