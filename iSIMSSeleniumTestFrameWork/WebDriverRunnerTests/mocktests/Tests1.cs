using Selene.Support.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner.internals;

namespace WebDriverRunnerTests.mocktests
{
    class Tests1
    {
        [UnitTest(Groups = new[] { "all"})]
        public void TestPass()
        {
            Assert.IsTrue(true);
        }

        [UnitTest(Groups = new[] { "all" })]
        public void TestFail()
        {
            Assert.AreEqual("A","B");
        }
    }
}
