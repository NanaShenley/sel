using Selene.Support.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner.internals;

namespace WebDriverRunnerTests.mocktests
{
    class AfterSuiteWebTest
    {
        [AfterSuiteWebTest]
        public void AfterSuite()
        {
            Assert.IsTrue(true);
        }

        [UnitTest]
        public void Test()
        {
            Assert.IsTrue(true);
        }
    }
}
