using AddressBook.Components;
using AddressBook.Test;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;

namespace AddressBook.CurrentPupil.Test.Permissions
{
    class SchAdminInitiateAddressBook
    {
        #region Story 1231 and Story 4517 - Validations for Global Search for AdmissionOfficer
        //Checks for global search validations. 
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome  })]
        public void GlobalSearchValidationsSchoolAdministrator()
        {
            //Check for global search menu validation after login [Home page]. 
            bool assert1 = InitiateAddressBook.TestAddressBookLinkPresence(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Check for global search menu validation after navigation to Go to Rooms page[Other than home page]
            bool assert6 = InitiateAddressBook.TestAddressBookLinkPresenceForOtherScreen();

            Assert.True(assert1 &&  assert6);
        }
        #endregion

        #region Story 1231 and Story 4517 - Validations for Global Search for AdmissionOfficer
        //Checks for global search validations. 
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void LiveSearchResultsSchoolAdmin()
        {
            Assert.False(InitiateAddressBook.MinimumCharacterRequiredToGetResult(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "a"));
        }
        #endregion

    }
}
