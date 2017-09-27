// TODO NOT READY FOR REVIEW (PENDING TO REFACTOR THE CODE WITH STANDARD GUIDELINES)

using SharedComponents.LoginPages;
using TestSettings;
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

   /* public abstract class TestBase
    {
         [FindsBy(How = How.LinkText, Using = "Opens the task menu and task search drawer")]
        protected readonly static string Urlundertest = Configuration.GetSutUrl();
        protected readonly static string Testuser = TestDefaults.Default.User;
        protected readonly static string Password = TestDefaults.Default.Password;

    }
    */
    // TODO NOT READY FOR REVIEW (PENDING TO REFACTOR THE CODE WITH STANDARD GUIDELINES)
    public class SchoolDetailsTests //: TestBase
    {

#pragma warning disable 0649

        [FindsBy(How = How.LinkText, Using = "Opens the task menu and task search drawer")]
        protected readonly static string Urlundertest = TestSettings.Configuration.GetSutUrl();
        protected readonly static string Testuser = TestDefaults.Default.TestUser;
        protected readonly static string Password = TestDefaults.Default.TestUserPassword;
       

      // (Need to remove)
        private static void NavigateToSchoolDetails()
        {
            var page = SignInPage.NavigateTo("https://lab-three.sims8.com", "/iSIMSMVCClientFarm1/");
            page.EnterUserId(Testuser);
            page.EnterPassword(Password);
            page.SignIn();
            //Reporter.Log("testing with " + Testuser + " on " + Urlundertest);

            var tenantPage = new SelectTenantPage();
            tenantPage.ValidateElements();
            tenantPage.EnterTenant("1").Submit();
            var school = new SelectSchool();
            school.SelectBySchoolName("Abbey Hill Primary School");
            school.SignIn();

            var link = new TaskMenuBar();
            link.ClickTaskMenuBar();
            link .ClickSchoolManagementLink();
            link.ClickSchoolDetailsLink();
        }

        // [WebDriverTest(Groups = new[] { "All" }, Browsers = new String[] { "chrome"/*,"internet explorer"*/})]
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
            popup.clicOKBtn();
            WebContext.Screenshot();
            waitforSaveBtntoAppear();
            WebContext.Screenshot();

        }
         public void waitforSaveBtntoAppear()
         {
             // wait for the page to be fully loaded
             WebContext.Screenshot();
             WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
             IWebElement Save = waiter.Until<IWebElement>(d =>
             {
                 IWebElement el = WebContext.WebDriver.FindElement(By.LinkText("Save"));
                 if (el.Displayed)
                 {
                     return el;
                 }
                 return null;

             });

         }
    }

}



