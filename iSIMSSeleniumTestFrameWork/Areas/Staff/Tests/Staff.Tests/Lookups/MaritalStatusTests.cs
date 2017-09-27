using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;

namespace Staff.Tests.Lookups
{
    [TestClass]
    public class MaritalStatusTests : LookupTestsBase
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
        //[TestMethod][ChromeUiTest(new[] { "22109", "MaritalStatus", "P1",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "NIStPri", "NIStSec", "NIStMult",
        //    "WelStPri", "WelStSec", "WelStMult",
        //    "IndPri", "IndSec", "IndMult"
        //})]
        public void Marital_Status_Lookup_Is_Not_Updateable()
        {
            LoginAndNavigate("Marital Status");
            Assert.IsFalse(CanAddLookupItems());
        }
    }
}
