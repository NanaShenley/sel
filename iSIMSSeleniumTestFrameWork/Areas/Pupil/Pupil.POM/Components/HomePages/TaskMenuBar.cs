using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Threading;
using WebDriverRunner.webdriver;

namespace POM.Components.HomePages
{
    public class TaskMenuBar// : BaseSeleniumComponents
    {

        [FindsBy(How = How.LinkText, Using = "School Management")]
        private IWebElement schoolManagementLink;





        //[FindsBy(How = How.CssSelector, Using = "a[data-automation-id='task_menu']")]
        IWebElement taskManagerButton = WebContext.WebDriver.FindElement(By.CssSelector("#shell-menu > div > div.shell-menu > div:nth-child(1) > ul > li:nth-child(1) > a"));

        //Define a dictionary for all the Task menu items and their Children 

        private Dictionary<By, List<By>> tasklist = new Dictionary<By, List<By>>();

        private By L1Returns = By.LinkText("Statutory Return");
        private By L2ReturnsSettings = By.LinkText("Statutory Return Settings");
        private By L2ReturnsManage = By.LinkText("Manage Statutory Returns");


        private WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(60));



        public TaskMenuBar()
        {
            //e.g.Statutory Return Task menu has two children viz.Manage Statutory Returns and Statutory Returns Settings
            //So we tried to capture Main link as L1 and its children as L2 and add all these in a dictionary
            var children = new List<By>();
            children.Add(L2ReturnsSettings);
            children.Add(L2ReturnsManage);
            tasklist.Add(L1Returns, children);

            var menu = WebContext.WebDriver.FindElement(By.CssSelector(".shell-task-menu-section"));
            // wait for the page to load
            waiter.Until(
                d =>
                {
                    IJavaScriptExecutor js = d as IJavaScriptExecutor;
                    string state = (string)js.ExecuteScript("return document.readyState");
                    if ("complete".Equals(state))
                    {
                        return true;
                    }
                    return false;
                }
              );

            PageFactory.InitElements(menu, this);
        }

        public TaskMenuBar WaitForTaskMenuBarButton()
        {
            // the page takes some time to load. We use the document.ready state (the spinning wheel icon on the tab) as a proxy for the page is ready.
            // TODO could be improved and wait for the menu to be clickable rather thn wait for the page to be finished
            // a user could click even if the page is still loading
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            waiter.Until<Boolean>(d =>
            {
                IJavaScriptExecutor js = WebContext.WebDriver as IJavaScriptExecutor;
                string state = (String)js.ExecuteScript("return document.readyState");
                if ("complete".Equals(state))
                {
                    return true;
                }
                return false;
            });
            return this;
        }

        private IWebElement GetMenuBarButton()
        {
            return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationId("task_menu"));
        }

        public TaskMenuBar ClickTaskMenuBar()
        {
            WaitForTaskMenuBarButton();
            GetMenuBarButton().Click();
            // wait for the menu to be fully moved to the right
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            waiter.Until<Boolean>(d =>
            {
                int x = GetMenuBarButton().Location.X;
                if (x < 250)
                {
                    return false;
                }
                return true;
            });
            return this;
        }


        //public TaskMenuBar ClickTaskMenuBar()
        //{
        //   IWebElement taskMenu = WebContext.WebDriver.Get(By.LinkText("Opens the task menu and task search drawer"));
        //   taskMenu.Click();
        //    return this;
        //}

        public void ClickAssessmentLink()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            waiter.Until(d =>
            {
                IWebElement taskbar =
                    WebContext.WebDriver.FindElement(By.LinkText("Opens the task menu and task search drawer"));
                if (taskbar.Location.X > 250)
                    return true;
                return false;
            });

            IWebElement menu = WebContext.WebDriver.FindElement(By.LinkText("Assessment"));
            menu.Click();
        }

        public void ClickSystemManagerLink()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            waiter.Until(d =>
            {
                IWebElement taskbar =
                    WebContext.WebDriver.FindElement(By.LinkText("Opens the task menu and task search drawer"));
                if (taskbar.Location.X > 250)
                    return true;
                return false;
            });

            IWebElement menu = WebContext.WebDriver.FindElement(By.LinkText("System Manager"));
            menu.Click();
        }

        public void ClickSchoolGroupsLink()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            waiter.Until(d =>
            {
                IWebElement taskbar =
                    WebContext.WebDriver.FindElement(By.LinkText("Opens the task menu and task search drawer"));
                if (taskbar.Location.X > 250)
                    return true;
                return false;
            });

            IWebElement menu = WebContext.WebDriver.FindElement(By.LinkText("School Groups"));
            menu.Click();
        }

        //Click on Attendance Link
        public void ClickAttendanceLink()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            waiter.Until(d =>
            {
                IWebElement taskbar =
                    WebContext.WebDriver.FindElement(By.LinkText("Opens the task menu and task search drawer"));
                if (taskbar.Location.X > 250)
                    return true;
                return false;
            });

            IWebElement menu = WebContext.WebDriver.FindElement(By.LinkText("Attendance"));
            menu.Click();
        }


        //Click on AddressBook Link
        public void ClickAddressBookLink()
        {
            WaitForTaskMenuBarButton();
            IWebElement addressBookLink = WebContext.WebDriver.FindElement(By.CssSelector("a[title='Search People']"));
            addressBookLink.Click();
        }



        //Click on Exceptional Circumstances link
        public void ClickExceptionalCircumstancesLink()
        {
            WebContext.Screenshot();
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            IWebElement element = waiter.Until<IWebElement>(d =>
            {
                IWebElement link = d.FindElement(By.LinkText("Exceptional Circumstances"));
                return link;
            });

            element.Click();
            Thread.Sleep(4000);
            WebContext.Screenshot();
        }

        public void ClickSchoolManagementLink()
        {
            schoolManagementLink.Click();

        }

        public TaskMenuBar ClickSchoolDetailsLink()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            IWebElement element = waiter.Until<IWebElement>(d =>
             {
                 IWebElement link = d.FindElement(By.LinkText("My School Details"));
                 return link;
             });

            element.Click();
            return this;
        }

        public void ClickRoomLink()
        {
            WebContext.Screenshot();
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            IWebElement element = waiter.Until<IWebElement>(d =>
            {
                IWebElement link = d.FindElement(By.LinkText("Rooms"));
                return link;
            });


            element.Click();
            Thread.Sleep(4000);
            WebContext.Screenshot();
        }


        public TaskMenuBar WaitFor()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            waiter.Until(d =>
            {
                IJavaScriptExecutor executor = d as IJavaScriptExecutor;
                string output = (string)executor.ExecuteScript(" return document.readyState");
                if (output == "complete")
                    return true;
                return false;
            });
            return this;

        }

        public void ClickTaskManagerButton()
        {
            taskManagerButton.Click();

            waiter.Until(
                d =>
                {
                    int x = taskManagerButton.Location.X;
                    if (x < 250)
                    {
                        return false;
                    }
                    return true;
                }
              );
        }

        private void ClickL1Link(By link)
        {
            WebContext.WebDriver.FindElement(link).Click();
            foreach (By child in tasklist[link])
            {
                waiter.Until(ExpectedConditions.ElementIsVisible(child));
            }
        }

        private void ClickL2Link(By link)
        {
            WebContext.WebDriver.FindElement(link).Click();
            WaitForTheFollowingPageToLoad();
        }
        // private By Statutory
        public void ClickL1StatutoryReturnsLink()
        {
            ClickL1Link(L1Returns);
        }


        public void ClickL2ManageReturns()
        {
            ClickL2Link(L2ReturnsManage);
        }
        public void ClickL2ReturnSettings()
        {
            ClickL2Link(L2ReturnsSettings);
        }
        public void WaitForTheFollowingPageToLoad()
        {
            waiter.Until(
           d =>
           {
               waiter.Message = "failed witing for nprogress to disapear";
               string classes = d.FindElement(By.Id("html")).GetAttribute("class");
               if (classes.Contains("nprogress-busy"))
               {
                   return true;
               }
               return false;
           }
         );
            waiter.Until(
            d =>
            {
                string classes = d.FindElement(By.Id("html")).GetAttribute("class");
                if (!classes.Contains("nprogress-busy"))
                {
                    return true;
                }
                return false;
            }
          );
        }
    }
}
