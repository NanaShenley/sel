using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace Agents.Components
{
    public class AgentScreenNavigation : BaseSeleniumComponents
    {

        public AgentScreenNavigation()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        #region Agent Screen Navigation
        public static AgentSearchScreen NavigateToAgentMenuPage(bool loginFlag = true)
        {
            if (loginFlag)
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                POM.Helper.Wait.WaitLoading();
                ShellAction.OpenTaskMenu();
                TaskMenuActions.OpenMenuSection("section_menu_Communications");
                TaskMenuActions.ClickMenuItem("task_menu_section_communication_Agents");
                WaitUntilDisplayed(AgentElements.SearchPanel.SearchButton);
            }
            else
            {
                ShellAction.OpenTaskMenu();
                POM.Helper.Wait.WaitLoading();
                TaskMenuActions.ClickMenuItem("task_menu_section_communication_Agents");
                WaitUntilDisplayed(AgentElements.SearchPanel.SearchButton);
            }
            return new AgentSearchScreen();
        }
        #endregion
        
        //Method to login with specific user and navigate to agent add screen
        public static bool NavigateToAgentMenuPage(SeleniumHelper.iSIMSUserType userType)
        {
            //Accept the userType.
            //Switch case based on userType. To return different asserts

            SeleniumHelper.Login(userType);
            POM.Helper.Wait.WaitLoading();
            ShellAction.OpenTaskMenu();
            TaskMenuActions.OpenMenuSection("section_menu_Communications");

            switch (userType)
            {
                case SeleniumHelper.iSIMSUserType.AdmissionsOfficer:
                    try
                    {
                        //Can not view or add new agents.  [View -- In communications task menu entry]
                        //Returns true if agent gets listed in communication menu. Else false. 
                        IWebElement AgentEntry = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communication_Agents")));
                        return true;
                    }
                    catch (NoSuchElementException e)
                    {
                        return false;
                    }

                case SeleniumHelper.iSIMSUserType.ClassTeacher:
                    try
                    {
                        //Can view and add new agents. Find the add new agent option in taskbar
                        //Returns true if agent option gets listed in communication menu with Add new agent option on agent screen.
                        BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communication_Agents")));
                        IWebElement AgentEntry = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communication_Agents")));
                        AgentEntry.Click();

                        //Check for add new agent button
                        BaseSeleniumComponents.WaitUntilDisplayed(AgentElements.MainScreen.AddNewAgentButton);
                        WebContext.WebDriver.FindElement(AgentElements.MainScreen.AddNewAgentButton);
                        return true;
                    }
                    catch (NoSuchElementException e)
                    {
                        return false;
                    }
                                                           
                case SeleniumHelper.iSIMSUserType.CurricularManager: 
                    try
                    {
                        //Can not view or add new agents.  [View -- In communications task menu entry]
                        //Returns true if agent gets listed in communication menu. Else false. 
                        IWebElement AgentEntry = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communication_Agents")));
                        return true;
                    }
                    catch (NoSuchElementException e)
                    {
                        return false;
                    }
 
                    
                case SeleniumHelper.iSIMSUserType.SchoolAdministrator:
                    try
                    {
                        //Can view and add new agents. Find the add new agent option in taskbar
                        //Returns true if agent option gets listed in communication menu with Add new agent option on agent screen. Else false
                        BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communication_Agents")));
                        IWebElement AgentEntry = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communication_Agents")));                        

                        AgentEntry.Click();

                        BaseSeleniumComponents.WaitUntilDisplayed(AgentElements.MainScreen.AddNewAgentButton);
                        //Check for add new agent button
                        WebContext.WebDriver.FindElement(AgentElements.MainScreen.AddNewAgentButton);
                        return true;
                    }
                    catch (NoSuchElementException e)
                    {
                        return false;
                    }
                    
                case SeleniumHelper.iSIMSUserType.SENCoordinator: 
                    try
                    {
                        //Can view and add new agents. Find the add new agent option in taskbar
                        //Returns true if agent option gets listed in communication menu with Add new agent option on agent screen.Else false
                        BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communication_Agents")));
                        IWebElement AgentEntry = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communication_Agents")));
                        AgentEntry.Click();

                        BaseSeleniumComponents.WaitUntilDisplayed(AgentElements.MainScreen.AddNewAgentButton);
                        //Check for add new agent button
                        WebContext.WebDriver.FindElement(AgentElements.MainScreen.AddNewAgentButton);
                        return true;
                    }
                    catch (NoSuchElementException e)
                    {
                        return false;
                    }

                case SeleniumHelper.iSIMSUserType.SeniorManagementTeam:
                        //Can view and add new agents. Find the add new agent option in taskbar
                        //Returns true if agent option gets listed in communication menu with Add new agent option on agent screen.Else false
                        BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communication_Agents")));
                        WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_communication_Agents"))).Click();           
                        try
                        {
                            //Check for add new agent button
                            WebContext.WebDriver.FindElement(AgentElements.MainScreen.AddNewAgentButton);
                            return true;
                        }
                        catch (NoSuchElementException e)
                        {
                            return false;
                        }
                 
                default: return false;
            }//switch case
            
        }        



    }
}


