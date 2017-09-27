using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using Staff.POM.Helper;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Staff.POM.Components.LoginPages
{
    public class SignInPage
    {

#pragma warning disable 0649

        // ReSharper disable UnassignedField.Compiler
        [FindsBy(How = How.CssSelector, Using = "input[data-automation-id='email-account-login']")]
        private IWebElement _email;

        [FindsBy(How = How.CssSelector, Using = "input[data-automation-id='password-account-login']")]
        private IWebElement _password;

        [FindsBy(How = How.TagName, Using = "button")]
        private IWebElement _signInButton;

        [FindsBy(How = How.LinkText, Using = "Change your password")]
        private IWebElement _changePassword;

        [FindsBy(How = How.CssSelector, Using = ".text-danger")]
        private IWebElement _error;
        // ReSharper restore UnassignedField.Compiler
#pragma warning restore 0649

        public SignInPage()
        {
            Wait.WaitForElement(SeleniumHelper.SelectByDataAutomationID("email-account-login"));
            PageFactory.InitElements(WebContext.WebDriver, this);

        }

        public string GetErrorText()
        {
            return _error.Text;
        }

        public SignInPage EnterUserId(string userId)
        {
            _email.Clear();
            _email.SendKeys(userId);
            return this;
        }

        public SignInPage EnterPassword(string password)
        {
            _password.Clear();
            _password.SendKeys(password);
            return this;
        }

        public ChangePasswordPage ChangePassword()
        {
            _changePassword.Click();
            return new ChangePasswordPage();
        }

        public void SignIn(bool schoolSelection)
        {
            AutomationSugar.WaitFor("sign-in-account-login");
            if (!schoolSelection)
            {
                AutomationSugar.ClickOnAndWaitFor(SimsBy.AutomationId("sign-in-account-login"), By.Id("shell"));
            }
            else
            {
                AutomationSugar.ClickOnAndWaitFor(SimsBy.AutomationId("sign-in-account-login"), By.Id("DomainID"));
            }
        }

        //public void ValidateElements()
        //{
        //    Assert.IsTrue(_email.Displayed);
        //    Assert.IsTrue(_password.Displayed);
        //    Assert.IsTrue(_signInButton.Displayed);
        //    Assert.IsTrue(_changePassword.Displayed);
        //}

        public static readonly string DefaultPath = "/" + TestDefaults.Default.Path + "/";

        public static SignInPage NavigateTo(int tenantId, string domain, params string[] enabledFeatures)
        {
            return NavigateTo(tenantId.ToString(CultureInfo.InvariantCulture), domain, DefaultPath, enabledFeatures);
        }

        public static SignInPage NavigateTo(int tenantId, string domain, string path, params string[] enabledFeatures)
        {
            return NavigateTo(tenantId.ToString(CultureInfo.InvariantCulture), domain, path, enabledFeatures);
        }

        public static SignInPage NavigateTo(string domain, params string[] enabledFeatures)
        {
            return NavigateTo(null, domain, DefaultPath, enabledFeatures);
        }

        public static SignInPage NavigateTo(string domain, string path, params string[] enabledFeatures)
        {
            return NavigateTo(null, domain, path, enabledFeatures);
        }

        private static SignInPage NavigateTo(string tenantId, string domain, string path, params string[] enabledFeatures)
        {
            StringBuilder cookieBuilder = new StringBuilder();

            if (enabledFeatures != null && enabledFeatures.Length > 0)
            {
                foreach (string feature in enabledFeatures)
                {
                    cookieBuilder.AppendFormat("%23{0}=true", feature.Replace(" ", "%20"));
                }
            }

            if (tenantId != null)
            {
                var organisationId = CalculatePersistentUniqueIdentifierFromName(GetTenantIdString(tenantId));
                WebContext.WebDriver.Url = domain + path + "?OrganisationId=" + organisationId;
            }
            else
            {
                WebContext.WebDriver.Url = domain + path;
            }

            if (!WebContext.Browser.Equals("internet explorer", StringComparison.OrdinalIgnoreCase) && enabledFeatures != null && enabledFeatures.Length > 0)
            {
                IOptions options = WebContext.WebDriver.Manage();

                cookieBuilder.Append("%23");

                options.Cookies.AddCookie(new Cookie("featureBee", cookieBuilder.ToString(), new Uri(domain).Host, "/", null));
            }

            if ("internet explorer".Equals(WebContext.Browser))
            {
                if (WebContext.WebDriver.Title.Equals("Certificate Error: Navigation Blocked"))
                {
                    WebContext.WebDriver.Url = "javascript:document.getElementById('overridelink').click();";
                }
            }
            return new SignInPage();
        }

        private static string GetTenantIdString(string tenantId)
        {
            return string.Format("TenantID{0}", tenantId);
        }

        /// 
        /// Calculates a hash of the string and copies the first 128 bits of the hash
        /// to a new Guid.
        /// 
        private static Guid CalculatePersistentUniqueIdentifierFromName(string baseName, string additionalName = null)
        {
            string data;
            data = baseName;
            if (!string.IsNullOrEmpty(additionalName))
            {
                data += "." + additionalName;
            }
            HashAlgorithm s_provider = new SHA1Managed();
            byte[] hash = s_provider.ComputeHash(System.Text.Encoding.Unicode.GetBytes(data));

            // Guid is always 16 bytes
            Debug.Assert(Guid.Empty.ToByteArray().Length == 16, "Expected Guid to be 16 bytes");

            byte[] toGuid = new byte[16];
            Array.Copy(hash, toGuid, 16);

            return new Guid(toGuid);
        }
    }
}
