using AddressBook.Components;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;

namespace AddressBook.CurrentPupil.Test.Permissions
{
    class AssessmentCoordinator
    {
        private readonly string textForSearch = "Laura";

        #region AssessmentCoordinatorQuickControlAccess()
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void AssessmentCoordinatorQuickControlAccess()
        {
            Assert.IsTrue(QuickControlAccess.isQuickControlAccess(SeleniumHelper.iSIMSUserType.AssessmentCoordinator));
        }
        #endregion

        #region AssessmentCoordinatorSearchCurrentPupilsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void AssessmentCoordinatorSearchCurrentPupilsAccess()
        {
            Assert.That(QuickControlAccess.hasPermissionToSearchCurrentPupil(SeleniumHelper.iSIMSUserType.AssessmentCoordinator, textForSearch) >= 0);
        }
        #endregion

        #region AssessmentCoordinatorPupilBasicDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void AssessmentCoordinatorPupilBasicDetailsAccess()
        {
            Assert.IsTrue(QuickControlAccess.canViewBasicDetailsCurrentPupil(SeleniumHelper.iSIMSUserType.AssessmentCoordinator, textForSearch));
        }
        #endregion

        #region AssessmentCoordinatorPupilTelephoneDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void AssessmentCoordinatorPupilTelephoneDetailsAccess()
        {
            Assert.IsFalse(QuickControlAccess.canViewPupilTelephoneDetails(SeleniumHelper.iSIMSUserType.AssessmentCoordinator, textForSearch));
        }
        #endregion

        #region AssessmentCoordinatorPupilEmailDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void AssessmentCoordinatorPupilEmailDetailsAccess()
        {
            Assert.IsFalse(QuickControlAccess.canViewPupilEmailDetails(SeleniumHelper.iSIMSUserType.AssessmentCoordinator, textForSearch));
        }
        #endregion

        #region AssessmentCoordinatorPupilAddressDetailsAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void AssessmentCoordinatorPupilAddressDetailsAccess()
        {
            Assert.IsFalse(QuickControlAccess.canViewPupilAddressDetails(SeleniumHelper.iSIMSUserType.AssessmentCoordinator, textForSearch));
        }
        #endregion

        #region AssessmentCoordinatorLinkToPupilRecordFromPupilInfoAccess
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch, AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void AssessmentCoordinatorLinkToPupilRecordFromPupilInfoAccess()
        {
            Assert.IsTrue(QuickControlAccess.hasAccessLinkToPupilRecordFromPupilInfo(SeleniumHelper.iSIMSUserType.AssessmentCoordinator, textForSearch));
        }
        #endregion
    }
}
