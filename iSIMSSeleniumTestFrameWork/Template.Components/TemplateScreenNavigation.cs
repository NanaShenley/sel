using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using WebDriverRunner.webdriver;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using POM.Components.LoginPages;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Template.Components
{
    public class TemplateScreenNavigation : BaseSeleniumComponents
    {
        public TemplateScreenNavigation()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }


        public static TemplateSearchScreen NavigateToTemplateMenuPage(bool loginFlag = true)
        {
            if (loginFlag)
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);  // Will be changed to SchoolAdmin when permissions gets implemented 
                ShellAction.OpenTaskMenu();
                TaskMenuActions.OpenMenuSection("section_menu_Communications");
                TaskMenuActions.ClickMenuItem("task_menu_section_communications_ManageMessageTemplates");
            }
            else
            {
                ShellAction.OpenTaskMenu();
                TaskMenuActions.ClickMenuItem("task_menu_section_communications_ManageMessageTemplates");
            }

            WaitUntilDisplayed(TemplateElements.SearchPanel.SearchButton);            
            return new TemplateSearchScreen();
        }

        public static TemplateSearchScreen NavigateToTemplateMenuPageFeatureBee(string[] featureList, POM.Helper.SeleniumHelper.iSIMSUserType userType = POM.Helper.SeleniumHelper.iSIMSUserType.TestUser)
        {                             
            POM.Helper.SeleniumHelper.Login(userType, featureList);  // Feature list is a list of features which will be enable at login.

            POM.Helper.Wait.WaitForElementEnabled(By.CssSelector(SeleniumHelper.AutomationId("task_menu")));
            POM.Helper.SeleniumHelper.ClickByJS(POM.Helper.SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("task_menu"))));
            Console.WriteLine("Clicked on taskmenu");
            System.Threading.Thread.Sleep(500);
            //ShellAction.OpenTaskMenu();
            TaskMenuActions.OpenMenuSection("section_menu_Communications");
            TaskMenuActions.ClickMenuItem("task_menu_section_communications_ManageMessageTemplates");
            return new TemplateSearchScreen();
        }

        //Method to sign in through specific user and enable featureBee
        public static void LoginWithFeatureBee(string[] featureList, POM.Helper.SeleniumHelper.iSIMSUserType userType = POM.Helper.SeleniumHelper.iSIMSUserType.TestUser)
        {
            POM.Helper.SeleniumHelper.Login(userType, featureList);      
        }

        //Method to validate entry of Manage Message Template menu in taskmenu
        public static bool ValidateManageMessageTemplateMenu()
        {
            POM.Helper.Wait.WaitForElementEnabled(By.CssSelector(SeleniumHelper.AutomationId("task_menu")));
            POM.Helper.SeleniumHelper.ClickByJS(POM.Helper.SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("task_menu"))));
            System.Threading.Thread.Sleep(500);
            //ShellAction.OpenTaskMenu();
            TaskMenuActions.OpenMenuSection("section_menu_Communications");

            bool returnVal = POM.Helper.SeleniumHelper.DoesWebElementExist(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communications_ManageMessageTemplates")));
            if(returnVal == false)
            {
                return returnVal;
            }
            else
            {                
                TaskMenuActions.ClickMenuItem("task_menu_section_communications_ManageMessageTemplates");
                POM.Helper.Wait.WaitForElement(By.CssSelector(SeleniumHelper.AutomationId("add_button")));
                return POM.Helper.SeleniumHelper.DoesWebElementExist(By.CssSelector(SeleniumHelper.AutomationId("add_button")));
            }                       
        }

        public static void CloseManageMessageTemplateTab(string tabName = "Manage Message Templates")
        {
            IReadOnlyCollection<IWebElement> tabs = FindElements(By.CssSelector(".page-tabs [role='tab']"));
            foreach (var tab in tabs)
            {
                if (tab.GetAttribute("data-tab-name").Trim().Equals(tabName))
                {
                    IWebElement closeButton = tab.FindElement(By.CssSelector(".tab-close"));
                    Retry.Do(closeButton.Click);
                    POM.Helper.Wait.WaitForElement(By.CssSelector(SeleniumHelper.AutomationId("ignore_commit_dialog")));
                    IWebElement dontSaveButton = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("ignore_commit_dialog")));
                    POM.Helper.SeleniumHelper.ClickByJS(dontSaveButton);
                    POM.Helper.Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    break;
                }
            }

        }

        public static ReadOnlyCollection<IWebElement> FindElements(By element)
        {
            POM.Helper.Wait.WaitForElement(element);
            return WebContext.WebDriver.FindElements(element);
        }


    }
}
