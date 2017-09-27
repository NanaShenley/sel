using AddressBook.Components;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;

namespace AddressBook.CurrentPupil.Test.Permissions
{
    class CurricularManager
    {
        private readonly string textForSearch = "Laura";
         
        #region CurricularManagerQuickControlAccess()
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CurricularManagerQuickControlAccess()
        {
            Assert.IsTrue(QuickControlAccess.isQuickControlAccess(SeleniumHelper.iSIMSUserType.CurricularManager));
        }
        #endregion

        #region CurricularManagerSearchCurrentPupilsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CurricularManagerSearchCurrentPupilsAccess()
        {
            Assert.That(QuickControlAccess.hasPermissionToSearchCurrentPupil(SeleniumHelper.iSIMSUserType.CurricularManager, textForSearch) >= 0);
        }
        #endregion

        #region CurricularManagerPupilBasicDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CurricularManagerPupilBasicDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewBasicDetailsCurrentPupil(SeleniumHelper.iSIMSUserType.CurricularManager, textForSearch));
        }
        #endregion

        #region CurricularManagerPupilTelephoneDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CurricularManagerPupilTelephoneDetailsAccess()
        {
            Assert.IsFalse(QuickControlAccess.canViewPupilTelephoneDetails(SeleniumHelper.iSIMSUserType.CurricularManager, textForSearch));
        }
        #endregion

        #region CurricularManagerPupilEmailDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CurricularManagerPupilEmailDetailsAccess()
        {
            Assert.IsFalse(QuickControlAccess.canViewPupilEmailDetails(SeleniumHelper.iSIMSUserType.CurricularManager, textForSearch));
        }
        #endregion

        #region CurricularManagerPupilAddressDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CurricularManagerPupilAddressDetailsAccess()
        {
            Assert.IsFalse(QuickControlAccess.canViewPupilAddressDetails(SeleniumHelper.iSIMSUserType.CurricularManager, textForSearch));
        }
        #endregion

        #region CurricularManagerLinkToPupilRecordFromPupilInfoAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CurricularManagerLinkToPupilRecordFromPupilInfoAccess()
        {
            Assert.IsFalse(QuickControlAccess.hasAccessLinkToPupilRecordFromPupilInfo(SeleniumHelper.iSIMSUserType.CurricularManager, textForSearch));
        }
        #endregion
    }
}
