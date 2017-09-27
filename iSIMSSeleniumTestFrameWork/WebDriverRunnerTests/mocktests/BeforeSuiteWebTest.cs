using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner.internals;
using Selene.Support.Attributes;
namespace WebDriverRunnerTests.mocktests
{
    class BeforeSuiteWebTest
    {
        [BeforeSuiteWebTest]
        public void Before()
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
