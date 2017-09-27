using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using AddressBook.Components;
using AddressBook.Components.Pages;
using NUnit.Framework;
using TestSettings;
using WebDriverRunner.internals;
using SharedComponents.Helpers;
using AddressBook.Test;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SharedComponents.HomePages;
using Selene.Support.Attributes;

namespace AddressBook.CurrentPupil.Test.SchoolAdministrator
{
    public class SchoolAdminSearchCurrentPupil
    {


        #region Story 1232- Verify if Navigation using Down keys is possible
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SchAdminKeyDownNavigationOnPupilResults()
        {
            Assert.IsTrue(SearchCurrentPupil.KeyDownNavigationOnPupilResults(SeleniumHelper.iSIMSUserType.SchoolAdministrator));
        }
        #endregion
    }
}
