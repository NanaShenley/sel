using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using TestSettings;
using WebDriverRunner.webdriver;
using POM.Helper;
using System.Text;
namespace SharedComponents.LoginPages
{
    public class SignInPage : BaseSeleniumComponents
    {
       
#pragma warning disable 0649

        // ReSharper disable UnassignedField.Compiler
        [FindsBy(How = How.CssSelector, Using = "input[data-automation-id='email-account-login']")]
        private IWebElement _email;

        [FindsBy(How = How.CssSelector, Using = "input[data-automation-id='password-account-login']")]
        private IWebElement _password;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='sign-in-account-login']")]
        private readonly IWebElement _signInButton;


        [FindsBy(How = How.LinkText, Using = "Change your password")]
        private IWebElement _changePassword;

        [FindsBy(How = How.CssSelector, Using = ".text-danger")]
        private IWebElement _error;
        // ReSharper restore UnassignedField.Compiler
#pragma warning restore 0649

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));

        public SignInPage()
        {
            WaitForElement(SeleniumHelper.SelectByDataAutomationID("email-account-login"));
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

        public void SignIn()
        {
            SeleniumHelper.ClickByJS(_signInButton);
            //_signInButton.Click();
        }

        public void ValidateElements()
        {
            Assert.IsTrue(_email.Displayed);
            Assert.IsTrue(_password.Displayed);
            Assert.IsTrue(_signInButton.Displayed);
            Assert.IsTrue(_changePassword.Displayed);
        }
        
        public static readonly string DefaultPath = "/" + TestDefaults.Default.Path + "/";

        public static SignInPage NavigateTo(int tenantId, string domain, string path, params string[] enabledFeatures)
        {
            return NavigateTo(tenantId.ToString(CultureInfo.InvariantCulture), domain, path, enabledFeatures);
        }

        private static SignInPage NavigateTo(string tenantId, string domain, string path, params string[] enabledFeatures)
        {
            StringBuilder featureQuery = new StringBuilder();

            if (enabledFeatures != null && enabledFeatures.Length > 0)
            {
                if (tenantId != null)
                    featureQuery.Append("&");
                else
                    featureQuery.Append("?");

                featureQuery.Append("FeatureBee=");

                foreach (string feature in enabledFeatures)
                {
                    featureQuery.AppendFormat("%23{0}=true", feature.Replace(" ", "+"));
                }
            }

            if (tenantId != null)
            {
                var organisationId = CalculatePersistentUniqueIdentifierFromName(GetTenantIdString(tenantId));
                WebContext.WebDriver.Url = domain + path + "?OrganisationId=" + organisationId + featureQuery;
            }
            else
            {
                WebContext.WebDriver.Url = domain + path + featureQuery;
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

        public static SignInPage NavigateTo(int tenantId, string domain)
        {
            return NavigateTo(tenantId.ToString(CultureInfo.InvariantCulture), domain, DefaultPath);
        }

        public static SignInPage NavigateTo(int tenantId, string domain, string path)
        {
            return NavigateTo(tenantId.ToString(CultureInfo.InvariantCulture), domain, path);
        }

        public static SignInPage NavigateTo(string domain)
        {
            return NavigateTo(null, domain, DefaultPath);
        }

        public static SignInPage NavigateTo(string domain, string path)
        {
            return NavigateTo(null, domain, path);
        }

        private static SignInPage NavigateTo(string tenantId, string domain, string path)
        {
            if (tenantId != null)
            {
                var organisationId = CalculatePersistentUniqueIdentifierFromName(GetTenantIdString(tenantId));
                WebContext.WebDriver.Url = domain + path + "?OrganisationId=" + organisationId;
            }
            else
            {
                WebContext.WebDriver.Url = domain + path;
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
    }
}
