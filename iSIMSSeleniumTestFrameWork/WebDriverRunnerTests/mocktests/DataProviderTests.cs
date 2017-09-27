using Selene.Support.Attributes;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner.internals;
using WebDriverRunner.results;

namespace WebDriverRunnerTests.mocktests
{
    class DataProviderTests
    {

        public List<object[]> UserPass()
        {
            var res = new List<Object[]>
            {
                new object[] { Status.Passed},
                new object[] {Status.Passed},
                new object[] {Status.Skipped}
            };
            return res;
        }

        [UnitTest(DataProvider = "UserPass",Groups = new[] { "all"})]
        public void WebDriverDataProviderTest(Status status)
        {
            Assert.AreEqual(Status.Passed, status);
        }
    }
}
