using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using WebDriverRunner.webdriver;

namespace MessageSetting.Components
{
    public class MessageSettingScreenNavigation : BaseSeleniumComponents
    {
        public MessageSettingScreenNavigation()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public static MessageSettingScreen navigateToMessageSettingPage(SeleniumHelper.iSIMSUserType user = SeleniumHelper.iSIMSUserType.SystemManger)
        {
            SeleniumHelper.Login(user);
            POM.Helper.Wait.WaitLoading();          
            ShellAction.OpenTaskMenu();
            POM.Helper.SeleniumHelper.Sleep(1);
            TaskMenuActions.OpenMenuSection("section_menu_Communications");
            POM.Helper.SeleniumHelper.Sleep(1);
            TaskMenuActions.ClickMenuItem("task_menu_section_communication_Manage_Message_Settings");
            POM.Helper.Wait.WaitLoading();
            if (new MessageSettingScreen().isPageDisplayed())
                return new MessageSettingScreen();
            else
                throw new NoSuchElementException("Message Setting screen not displayed.");                            
        }

        public static bool configureEmailSettings(string serviceProvider, string emailID, string pwd)
        {
            MessageSettingScreen MSSObj = new MessageSettingScreen();
            MSSObj.clickEmailCheckBox();

            if(MSSObj.configEmail(serviceProvider, emailID, pwd))
            {
                MSSObj.saveButtonClick();
                POM.Helper.SeleniumHelper.Sleep(3);
                return true;
            }
            return false;          
        }

        public static bool testEmailConnection()
        {
            return new MessageSettingScreen().testEmailConnection("arron@example.com");
        }
        public static bool configureSMSSetting(string subID, string authCode)
        {
            MessageSettingScreen MSSObj = new MessageSettingScreen();
            MSSObj.clickSMSCheckBox();
            if (MSSObj.configSMS(subID, authCode))
            {
                MSSObj.saveButtonClick();
                POM.Helper.SeleniumHelper.Sleep(3);
                return true;
            }
            return false;
        }
        public static bool testSMSConnection()
        {
            return new MessageSettingScreen().testSMSConnection("12345678");
        }
    }
}
