using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.LoginPages;
using POM.Helper;

using TestSettings;
using WebDriverRunner.webdriver;

namespace POM.Components.HomePages
{
    public class HomePage //: BaseSeleniumComponents
    {
        public static HomePage NavigateTo(string user, string password, int tenantId, string schoolName, string url, bool schoolSelection, params string[] enabledFeatures)
        {
            return NavigateTo(user, password, tenantId, schoolName, url, TestDefaults.Default.Path + "/Home/", schoolSelection, enabledFeatures);
        }

        public static HomePage NavigateTo(string user, string password, int tenantId, string schoolName, string url, string path, bool schoolSelection, params string[] enabledFeatures)
        {
            var signInPage = SignInPage.NavigateTo(tenantId, url, path, enabledFeatures);
            signInPage.EnterUserId(user);
            signInPage.EnterPassword(password);
            signInPage.SignIn();

            if (schoolSelection)
            {
                var selectSchool = new SelectSchool();
                selectSchool.SelectBySchoolName(schoolName);
                selectSchool.SignIn();
            }

            return new HomePage();
        }

        public HomePage()
        {
            //Wait.WaitUntilDisplayed(By.CssSelector(".navbar-left ul[role='menubar']"));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public QuickLinksBar MenuBar()
        {
            return new QuickLinksBar();
        }

        public TaskMenuBar TaskMenu()
        {
            return new TaskMenuBar();
        }

        public TaskSearch TaskSearch()
        {
            //SeleniumHelper.ClickByJS(By.CssSelector("[title=\"Task Menu\"]"));
            SeleniumHelper.Get(By.CssSelector("[data-automation-id='task_menu']")).ClickByJS();
            return new TaskSearch();
        }
        
         public static void SignOut()
        {
            SeleniumHelper.ClickAndWaitForByJS(By.CssSelector("[data-automation-id='TopMenuUserName']"),
                                               By.CssSelector("[data-automation-id='TopMenuSignOut']"));
            SeleniumHelper.Get(By.CssSelector("[data-automation-id='TopMenuSignOut']")).Click();
            Wait.WaitForDocumentReady();

            string URL = Configuration.GetSutUrl() + "/iSIMSMVCClientFarm1/Home/SignedOut?tenantID=" + TestDefaults.Default.TenantId;
            WebContext.WebDriver.Navigate().GoToUrl(URL);

            if (SeleniumHelper.DoesElementExist(By.CssSelector(SeleniumHelper.AutomationId("return-sign-out"))))
                SeleniumHelper.Get(By.CssSelector("[data-automation-id='return-sign-out']")).Click();
            Wait.WaitForDocumentReady();
        }
    }
}
