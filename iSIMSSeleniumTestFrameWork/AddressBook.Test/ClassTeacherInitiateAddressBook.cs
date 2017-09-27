using AddressBook.Components;
using AddressBook.Test;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;

namespace AddressBook.Test
{
    class ClassTeacherInitiateAddressBook
    {
        #region Story 1231 and Story 4517 - Validations for Global Search for AdmissionOfficer
        //Checks for global search validations. 
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void GlobalSearchValidationsClassTeacher()
        {
            //Check for global search menu validation after login [Home page]. 
            bool assert1 = InitiateAddressBook.TestAddressBookLinkPresence(SeleniumHelper.iSIMSUserType.ClassTeacher);

            //Check for global search menu validation after navigation to Pupil screen [Other than home page]
            bool assert6 = InitiateAddressBook.TestAddressBookLinkPresenceForPupilScreen(); 
            //Lab 5- build 6.15.1

            Assert.True(assert1 && assert6);
        }
        #endregion

        #region Story 1231 and Story 4517 - Validations for Global Search for AdmissionOfficer
        //Checks for global search validations. 
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrPupilPermissions }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void LiveSearchResultsClassTeacher()
        {
            Assert.False(InitiateAddressBook.MinimumCharacterRequiredToGetResult(SeleniumHelper.iSIMSUserType.ClassTeacher, "a"));
        }
        #endregion

          }
}
