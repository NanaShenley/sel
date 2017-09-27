using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Selene.Support.Attributes;
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
    [TestClass]
    public class StaffReligionLookupRegionalTests : LookupTestsBase
    {
        #region MS Unit Testing support
        public TestContext TestContext { get; set; }
        [TestInitialize]
        public void Init()
        {
            TestRunner.VSSeleniumTest.Init(this, TestContext);
        }
        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.VSSeleniumTest.Cleanup(TestContext);
        }
        #endregion
        //[TestMethod][ChromeUiTest(new[] { "Staff_Religion_Lookup", "Ensure_Staff_Religion_Lookup_Regional_Tests_Is_Updateable", "EngStPri", "EngStSec", "EngStMult" })]
        public void Ensure_Staff_Religion_Lookup_Regional_Tests_Is_Updateable()
        {
            LoginAndNavigate("Staff Religion");
            Assert.IsTrue(CanAddLookupItems());
        }

        //[TestMethod][ChromeUiTest(new[] { "Staff_Religion_Lookup", "Ensure_Staff_Religion_Lookup_Regional_Tests_Is_NOT_Updateable", "WelStPri", "WelStSec", "WelStMult", "IndPri", "IndSec", "IndMult" })]
        public void Ensure_Staff_Religion_Lookup_Regional_Tests_Is_NOT_Updateable()
        {
            LoginAndNavigate("Staff Religion");
            Assert.IsFalse(CanAddLookupItems());
        }

       // [TestMethod][ChromeUiTest(new[] { "Staff_Religion_Lookup", "Ensure_Staff_Religion_Lookup_Regional_Tests_Is_NOT_Accessible", "NIStPri", "NIStSec", "NIStMult" })]
        public void Ensure_Staff_Religion_Lookup_Regional_Tests_Is_NOT_Accessible()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

            var staffReligionMenuItem = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='lookup_menu_staff_religion']")).Count == 1;

            Assert.IsFalse(staffReligionMenuItem);
        }
    }
}
