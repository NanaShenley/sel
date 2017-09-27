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
    public class ServiceAgreementLookupRegionalTests : LookupTestsBase
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
        //[TestMethod][ChromeUiTest(new[] { "Ensure_Service_Type_Lookup_Agreement_Is_Updateable", "NIStPri", "NIStSec", "NIStMult", "WelStPri", "WelStSec", "WelStMult", "IndPri", "IndSec", "IndMult" })]
        public void Ensure_Service_Type_Lookup_Agreement_Is_Updateable()
        {
            LoginAndNavigate("Service Agreement Type");
            Assert.IsTrue(CanAddLookupItems());
        }

        //[TestMethod][ChromeUiTest(new[] { "Ensure_Service_Type_Agreement_Lookup_Is_NOT_Updateable", "EngStPri", "EngStSec", "EngStMult" })]
        public void Ensure_Service_Type_Agreement_Lookup_Is_NOT_Updateable()
        {
            LoginAndNavigate("Service Agreement Type");
            Assert.IsFalse(CanAddLookupItems());
        }
    }
}
