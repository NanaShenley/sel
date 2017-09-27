using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.LoginPages;
using TestSettings;
using WebDriverRunner.webdriver;

namespace SharedComponents.HomePages
{
    public class HomePage : BaseSeleniumComponents
    {
        public static HomePage NavigateTo(string user, string password, int tenantId, string schoolName, string url, bool schoolSelection = true)
        {
            return NavigateTo(user, password, tenantId, schoolName, url, TestDefaults.Default.Path + "/Home/", schoolSelection);
        }

        public static HomePage NavigateTo(string user, string password, int tenantId, string schoolName, string url, string path, bool schoolSelection = true)
        {
            var signInPage = SignInPage.NavigateTo(tenantId, url, path);
            signInPage = signInPage.EnterUserId(user);
            signInPage = signInPage.EnterPassword(password);
            signInPage.SignIn();
            System.Threading.Thread.Sleep(500);
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
            WaitUntilDisplayed(By.Id("shell"));
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

        public static HomePage NavigateTo(string user, string password, int tenantId, string schoolName, string url, bool schoolSelection, params string[] enabledFeatures)
        {
            return NavigateTo(user, password, tenantId, schoolName, url, TestDefaults.Default.Path + "/Home/", schoolSelection, enabledFeatures);
        }

        public static HomePage NavigateTo(string user, string password, int tenantId, string schoolName, string url, string path, bool schoolSelection, string[] enabledFeatures)
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
    }
}