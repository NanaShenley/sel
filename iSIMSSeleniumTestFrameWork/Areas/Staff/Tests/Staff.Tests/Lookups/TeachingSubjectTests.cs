using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;

namespace Staff.Tests.Lookups
{
    [TestClass]
    public class TeachingSubjectTests : LookupTestsBase
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
        //[TestMethod][ChromeUiTest(new[] { "22109", "TeachingSubject", "P1",
        //    "NIStPri", "NIStSec", "NIStMult",
        //    "WelStPri", "WelStSec", "WelStMult",
        //    "IndPri", "IndSec", "IndMult"
        //})]
        public void Teaching_Subject_Lookup_Is_Updateable()
        {
            LoginAndNavigate("Teaching Subject");
            Assert.IsTrue(CanAddLookupItems());
        }

        //[TestMethod][ChromeUiTest(new[] { "22109", "TeachingSubject", "P1",
        //   "EngStPri", "EngStSec", "EngStMult",
        //})]
        public void Teaching_Subject_Lookup_Is_Not_Updateable()
        {
            LoginAndNavigate("Teaching Subject");
            Assert.IsFalse(CanAddLookupItems());
        } 
    }
}
