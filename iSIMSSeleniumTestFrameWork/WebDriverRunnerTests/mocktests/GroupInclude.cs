using Selene.Support.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner.internals;

namespace WebDriverRunnerTests.mocktests
{
    class GroupInclude
    {
        [UnitTest]
        public void TestNoGroup()
        {
            Assert.IsTrue(true);
        }
        [UnitTest(Groups = new[] { "group1" , "A" ,"all"})]
        public void TestGroup1()
        {
            Assert.IsTrue(true);
        }

        [UnitTest(Groups = new[] { "group1", "group2", "B" })]
        public void TestGroup1And2()
        {
            Assert.IsTrue(true);
        }

        [UnitTest(Groups = new[] { "group1", "group2","group3", "C" })]
        public void TestGroup1And2And3()
        {
            Assert.IsTrue(true);
        }
    }
}
