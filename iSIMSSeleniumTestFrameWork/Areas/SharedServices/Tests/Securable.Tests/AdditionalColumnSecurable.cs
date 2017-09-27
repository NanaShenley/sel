using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.HomePages;
using SharedComponents.LoginPages;
using SharedComponents.Utils;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace Securable.Tests
{
    public class AdditionalColumnSecurable
    {
        private readonly static string UrlUndertest = TestDefaults.Default.URL;
        private readonly static string Testuser = TestDefaults.Default.User;
        private readonly static string Password = TestDefaults.Default.Password;
        private readonly static string SchoolName = TestDefaults.Default.SchoolName;
        private static readonly int TenantId = TestDefaults.Default.TenantId;

        /// <summary>
        /// Test to sign into the application.
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "SignIn" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void SignIn()
        {
            var page = SignInPage.NavigateTo(UrlUndertest);
            page.EnterUserId(Testuser);
            page.EnterPassword(Password);
            page.SignIn();

            var tenantPage = new SelectTenantPage();
            tenantPage.ValidateElements();
            tenantPage.EnterTenant(TenantId.ToString());
            tenantPage.Submit();

            var selectSchool = new SelectSchool();
            selectSchool.SelectBySchoolName(SchoolName);
            selectSchool.SignIn();
            Thread.Sleep(2000);
            WebContext.Screenshot();

            Assert.IsTrue(true);
        }

    }
}



