using AddressBook.Components;
using AddressBook.Test;
using OpenQA.Selenium;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using WebDriverRunner.webdriver;


namespace AddressBook.CurrentPupil.Test.Permissions
{
    public class QuickControlAccess
    {
        #region IsQuickControlAccess(SeleniumHelper.iSIMSUserType userType)
        public static bool isQuickControlAccess(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            return searchBox.textSearch.Displayed;
        }
        #endregion

        #region hasPermissionToSearchCurrentPupil(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        public static int hasPermissionToSearchCurrentPupil(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        {
            SeleniumHelper.Login(userType);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            int resultCount = searchBox.CheckForResultsAvailability(textForSearch);
            return resultCount;
        }
        #endregion

        #region CanViewBasicDetailsCurrentPupil(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        public static bool canViewBasicDetailsCurrentPupil(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationByUserType(userType);

            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            WebContext.Screenshot();
            return popup.GetPupilBasicDetails();
        }
        #endregion

        #region CanViewPupilTelephoneDetails(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        public static bool canViewPupilTelephoneDetails(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        {
            SeleniumHelper.Login(userType);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            searchBox.ClickOnFirstPupilRecord();
            AddressBookPopup popup = new AddressBookPopup();
            popup.WaitForDialogToAppear();
            return popup.IsPupilTelephoneDisplayed();
        }
        #endregion

        #region CanViewPupilEmailDetails(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        public static bool canViewPupilEmailDetails(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        {
            SeleniumHelper.Login(userType);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            searchBox.ClickOnFirstPupilRecord();
            AddressBookPopup popup = new AddressBookPopup();
            popup.WaitForDialogToAppear();
            return popup.IsEmailDisplayed();
        }
        #endregion

        #region CanViewPupilAddressDetails(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        public static bool canViewPupilAddressDetails(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        {
            SeleniumHelper.Login(userType);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            searchBox.ClickOnFirstPupilRecord();
            AddressBookPopup popup = new AddressBookPopup();
            popup.WaitForDialogToAppear();
            return popup.IsAddressDisplayed();
        }
        #endregion

        #region hasAccessLinkToPupilRecordFromPupilInfo(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        public static bool hasAccessLinkToPupilRecordFromPupilInfo(SeleniumHelper.iSIMSUserType userType, string textForSearch)
        {
            SeleniumHelper.Login(userType);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(textForSearch);
            searchBox.ClickOnFirstPupilRecord();
            AddressBookPopup popup = new AddressBookPopup();
            popup.WaitForDialogToAppear();
            return popup.IsLinkDisplayed();
        }
        #endregion
    }
}
