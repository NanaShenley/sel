using Selene.Support.Attributes;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner.internals;

namespace WebDriverRunnerTests.mocktests
{
    class SlowTests
    {

        [UnitTest(Groups = new[] { "all" })]
        public void TestPass()
        {
            Thread.Sleep(550);
            Assert.IsTrue(true);
        }
        [UnitTest(Groups = new[] { "all" })]
        public void TestPass2()
        {
            Thread.Sleep(550);
            Assert.IsTrue(true);
        }
    }
}
