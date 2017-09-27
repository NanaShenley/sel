using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using DataExchange.POM.Components.LoginPages;
using DataExchange.POM.Helper;
using TestSettings;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.HomePages
{
    public class HomePage //: BaseSeleniumComponents
    {
        public static HomePage NavigateTo(string user, string password, int tenantId, string schoolName, string url, bool schoolSelection = true, params string[] enabledFeatures)
        {
            return NavigateTo(user, password, tenantId, schoolName, url, TestDefaults.Default.Path + "/Home/", schoolSelection, enabledFeatures);
        }

        public static HomePage NavigateTo(string user, string password, int tenantId, string schoolName, string url, string path, bool schoolSelection = true, params string[] enabledFeatures)
        {
            var signInPage = SignInPage.NavigateTo(tenantId,url, path, enabledFeatures);
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
            Wait.WaitUntilDisplayed(By.Id("shell"));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

      

        public TaskMenuBar TaskMenu()
        {
            return new TaskMenuBar();
        }

        public static void SignOut()
        {
            SeleniumHelper.ClickAndWaitForByJS(By.CssSelector("[data-automation-id='TopMenuUserName']"), 
                                               By.CssSelector("[data-automation-id='TopMenuSignOut']"));
            SeleniumHelper.Get(By.CssSelector("[data-automation-id='TopMenuSignOut']")).Click();
            Wait.WaitForDocumentReady();
            SeleniumHelper.Get(By.CssSelector("[data-automation-id='return-sign-out']")).Click();
            Wait.WaitForDocumentReady();
        }
    }
}
