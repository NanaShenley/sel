using SharedComponents.LoginPages;
using SharedComponents.Utils;
using WebDriverRunner.internals;
using SharedComponents.HomePages;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.IE;
using System.Drawing;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents;
using WebDriverRunner.webdriver;
using System.Collections.ObjectModel;
using Facilities.Components;

namespace Facilities.Tests
{


    public class SchoolDetailsTests
    {
#pragma warning disable 0649
        [FindsBy(How = How.LinkText, Using = "Opens the task menu and task search drawer")]


        private readonly static string Urlundertest = TestDefaults.Default.URL;
        private readonly static string Testuser = TestDefaults.Default.User;
        private readonly static string Password = TestDefaults.Default.Password;

        [WebDriverTest(Groups = new[] { "All" }, Browsers = new String[] { "chrome"/*,"internet explorer"*/})]
        public void SchoolDetailsSmokeTest()
        {
            NavigateToSchoolDetails();

             //school details section
            SchoolDetailsPage sdp = new SchoolDetailsPage();
            sdp.OpenSitesAndBuildings();
            sdp.AddSiteAndBuilding();

            AddSiteAndBuildindPopup popup = new AddSiteAndBuildindPopup();
            popup.EnterShortName("Rajesh");
            popup.EnterName("i m the name");
            WebContext.Screenshot();

        }

        private static void NavigateToSchoolDetails()
        {
            var page = SignInPage.NavigateTo(Urlundertest);
            page.EnterUserId(Testuser);
            page.EnterPassword(Password);
            page.SignIn();
            Reporter.Log("testing with " + Testuser + " on " + Urlundertest);

            var tenantPage = new SelectTenantPage();
            tenantPage.ValidateElements();
            tenantPage.EnterTenant("1").Submit();
            var school = new SelectSchool();
            school.SelectBySchoolName("Abbey Hill Primary School");
            school.SignIn();

             //menu
            var menu = new TaskMenuBar();
            menu.ClickTaskMenuBar();
            menu.ClickSchoolManagementLink();

            menu.ClickSchoolDetailsLink();
        }



        //public void ClickTaskMenu()
        //{
        //    Taskmenu.Click();

        //}


        //public void GoToSchoolSiteRoom()
        //{


        //    Taskmenu.Click();


        //    IWebElement TaskMenu = WebContext.WebDriver.FindElement(By.LinkText("Opens the task menu and task search drawer"));
        //    TaskMenu.Click();

        //    IWebElement SchoolMgmt = WebContext.WebDriver.FindElement(By.XPath("//div[12]/div/h4/a/span"));
        //    SchoolMgmt.Click();

        //    IWebElement MySchool = WebContext.WebDriver.FindElement(By.XPath("//div[12]/div[2]/div/ul/li[2]/a/span"));
        //    MySchool.Click();


        //    IWebElement SchoolSite = WebContext.WebDriver.FindElement(By.XPath("//form[@id='editableData']/div/div[4]/div/h4/a/span"));
        //    SchoolSite.Click();

        //    IWebElement BasicDetails = WebContext.WebDriver.FindElement(By.XPath(".//*[@id='editableData']/div/div[1]/div[1]/h4/a/span"));
        //    BasicDetails.Click();




        //}
    }

}



