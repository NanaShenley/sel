using AddressBook.Test;
using OpenQA.Selenium.Interactions;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace AddressBook.Components
{
    public class QuickSearch
    {
        public static AddressBookSearchPage QuickSearchNavigation()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            return new AddressBookSearchPage();
        }

        public static AddressBookSearchPage QuickSearchNavigation(SeleniumHelper.iSIMSUserType user)
        {
            SeleniumHelper.Login(user);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
          //  AddressBookLink.ClickLink();
            return new AddressBookSearchPage();
        }

        public static AddressBookSearchPage QuickSearchNavigationOnTab(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(OpenQA.Selenium.Keys.Tab).Perform();
            action.SendKeys(OpenQA.Selenium.Keys.Tab).Perform();
            action.SendKeys(OpenQA.Selenium.Keys.Tab).Perform();
            action.SendKeys(OpenQA.Selenium.Keys.Enter).Perform();
            return new AddressBookSearchPage();
        }

        public static AddressBookSearchPage QuickSearchNavigationOnTabChrome(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(OpenQA.Selenium.Keys.Tab).Perform();
            action.SendKeys(OpenQA.Selenium.Keys.Tab).Perform();
            //     action.SendKeys(OpenQA.Selenium.Keys.Tab).Perform();
            //    action.SendKeys(OpenQA.Selenium.Keys.Enter).Perform();
            return new AddressBookSearchPage();
        }
        public static AddressBookSearchPage QuickSearchNavigationByUserType(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            return new AddressBookSearchPage();
        }

    }
}

