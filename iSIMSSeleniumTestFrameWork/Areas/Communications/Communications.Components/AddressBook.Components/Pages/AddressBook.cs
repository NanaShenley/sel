using AddressBook.Components.Pages;
using OpenQA.Selenium;
using SharedComponents;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using TestSettings;
using WebDriverRunner.webdriver;

namespace AddressBook.Test
{
    public  class AddressBookLink
    {
        private readonly static string Testuser = TestDefaults.Default.SchoolAdministrator;
        private readonly static string Password = TestDefaults.Default.SchoolAdministratorPassword;
        private static IWebElement _loadedQuickSearchlink = null;

        /// <summary>
        /// Waits for the Quick Search Link to load and returns the loaded link
        /// </summary>
        private static IWebElement LoadedQuickSearchLink
        {
            get
            {
                if (_loadedQuickSearchlink == null)
                {
                    _loadedQuickSearchlink = SeleniumHelper.Get(AddressBookElements.LinkAddressBook);
                }
                return _loadedQuickSearchlink;
            }
        }


        public static void ClickLink()
        {
            //if (LoadedQuickSearchLink.Displayed)
              //  LoadedQuickSearchLink.Click();
            SeleniumHelper.Get(AddressBookElements.LinkAddressBook).Click();

        }

        public static void LogLinkVisibility(string screenName)
        {
            if (LoadedQuickSearchLink.Displayed)
                TestResultReporter.Log("Quick Search Link is displayed on " + screenName);
            else
                TestResultReporter.Log("Quick Search Link is not displayed on " + screenName);
        }
    }
}