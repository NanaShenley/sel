using System;
using Attendance.Components.AttendancePages;
using SeSugar.Automation;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;

namespace Attendance.EditMarks.Tests.Lookups
{
    public class LookupTestsBase
    {
        protected bool CanAddLookupItems()
        {
            WebDriverWait wait = new WebDriverWait(SeSugar.Environment.WebContext.WebDriver, SeSugar.Environment.Settings.ElementRetrievalTimeout);
            wait.Until(x => ExpectedConditions.ElementExists(By.CssSelector("div[class=toolbar-bar]")));

            return WebContext.WebDriver.FindElements(new ByChained(By.CssSelector("div[class=toolbar-bar]"), By.CssSelector("[data-automation-id^='create_service_']"))).Count > 0;
        }

        protected void LoginAndNavigate(string to)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Lookups", "Attendance", to);
        }
    }
}
