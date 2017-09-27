using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace Staff.Tests.Lookups
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
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Lookups", "Staff", to);
        }
    }
}
