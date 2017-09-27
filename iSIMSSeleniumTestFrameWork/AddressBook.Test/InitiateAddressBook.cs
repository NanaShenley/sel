using AddressBook.Components;
using AddressBook.Components.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;


namespace AddressBook.Test
{
    public class InitiateAddressBook 
    {

        #region Story 1231- Validate the existance of Address book link in menu bar
        public static bool TestAddressBookLinkPresence(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            return ElementRetriever.IsExist(SeleniumHelper.Get(AddressBookElements.TextSearch));
        }
        #endregion

        #region Story 1231- Validate the existance of Address book even if navigate to Room screen(other than home Page)
        public static bool TestAddressBookLinkPresenceForOtherScreen()
        {
            NavigateToOtherScreen.GoToRoomScreen();
            return ElementRetriever.IsExist(SeleniumHelper.Get(AddressBookElements.TextSearch));
        }
        #endregion
        #region Story 1231- Validate the existance of Address book even if navigate to Pupil screen(other than home Page)
        public static bool TestAddressBookLinkPresenceForPupilScreen(bool senFlag = false)
        {
            NavigateToOtherScreen.PupilScreenOnTaskMenu(senFlag);
            return ElementRetriever.IsExist(SeleniumHelper.Get(AddressBookElements.TextSearch));
        }
        #endregion

        #region Story 1231- Validate the existance of Address book even if navigate to Pupil screen(other than home Page)
        public static bool TestAddressBookLinkPresenceForPupilScreenViaQuickAccess()
        {
            NavigateToOtherScreen.GoToPupilRecordScreen();
            return ElementRetriever.IsExist(SeleniumHelper.Get(AddressBookElements.TextSearch));
        }
        #endregion

        #region Story 1231- Validate the existance of Address book even if navigate to School screen(other than home Page)
        public static bool TestAddressBookLinkPresenceForTGScreen()
        {
            NavigateToOtherScreen.GoToTGScreen();
            return ElementRetriever.IsExist(SeleniumHelper.Get(AddressBookElements.TextSearch));
        }
        #endregion


        #region Story 4517- Check if UI gets closed on clicking off
        //Returns true if UI gets closed, else false
        public static bool TestAddressBookLinkCloseOnClickingOff()
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)WebContext.WebDriver;
            executor.ExecuteScript("arguments[0].click();", SeleniumHelper.Get(AddressBookElements.LinkAddressBook));

            System.Threading.Thread.Sleep(500);
            SeleniumHelper.Get(AddressBookElements.HomeScreen).Click();
            System.Threading.Thread.Sleep(500);
            AddressSearchTextBox Page = new AddressSearchTextBox();
            bool value= !(Page.textSearch.Displayed); 
            return value;
        }
        #endregion

        #region Test focus of addressbox
        public static bool TestAddressBookLinkFocusDefault()
        {
            System.Threading.Thread.Sleep(500);

            SeleniumHelper.Get(AddressBookElements.LinkAddressBook).Click();
           
 
            AddressBookSearchPage searchBox = new AddressBookSearchPage();

            searchBox.ClearText();
            searchBox.textSearch.SendKeys("Able to send data immediately after Clicking on link");
            

            System.Threading.Thread.Sleep(500);

            bool enabledSearchTextBox = searchBox.textSearch.Enabled;

            System.Threading.Thread.Sleep(500);
            searchBox.ClearText();
            
            SeleniumHelper.Get(AddressBookElements.HomeScreen).Click();
        
            return enabledSearchTextBox;
        }
        #endregion

        #region Story 4517- Check the title for Link
        public static string TestAddressBooktoolTip()
        {
            string titleLink=  SeleniumHelper.Get(AddressBookElements.LinkAddressBook).GetAttribute("title");
            return titleLink;
        }
        #endregion

        #region Story 1231- Two characters are minimal required to get results
        //Returns true if popup is shown else false
        public static bool MinimumCharacterRequiredToGetResult(SeleniumHelper.iSIMSUserType userType, string charToSearch)
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationOnTab(userType);
            
            searchBox.ClearText();
            searchBox.textSearch.SendKeys(charToSearch);
            
            bool value = SeleniumHelper.Get(AddressBookElements.PlaceHolderForResults).Displayed;
            System.Threading.Thread.Sleep(500);
            SeleniumHelper.Get(AddressBookElements.HomeScreen).Click();
            return value;
        }
        #endregion

        #region Story 1231- Can we tab to QuickSearch control and on enter focus defaults to text box
      //Parul: On Chrome need to perform Action THRICE only
        public static bool CanTabToQuickSearch(SeleniumHelper.iSIMSUserType userType)
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationOnTab(userType);
            searchBox.ClearText();
            searchBox.textSearch.SendKeys("ad");
            string value = SeleniumHelper.Get(AddressBookElements.LinkAddressBook).GetAttribute("aria-expanded");
            return (value== "true");
        }
        #endregion

        #region Story 1231- Can we tab to QuickSearch control and on enter focus defaults to text box Specifically for Chrome
        public static bool CanTabToQuickSearchOnChrome(SeleniumHelper.iSIMSUserType userType)
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationOnTabChrome(userType);
            searchBox.ClearText();
            searchBox.textSearch.SendKeys("ad");
            string value = SeleniumHelper.Get(AddressBookElements.LinkAddressBook).GetAttribute("aria-expanded");
            return (value == "true");
        }
        #endregion
    }
}
