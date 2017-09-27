using Selene.Support.Attributes;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner.internals;
using WebDriverRunner.results;

namespace WebDriverRunnerTests.mocktests
{
    class DataProviderNegTests
    {

        public List<object[]> UserPass()
        {
            var res = new List<Object[]>
            {
                new object[] { Status.Passed,Status.Passed}
            };
            return res;
        }

        [UnitTest(DataProvider = "UserPass",Groups = new[] { "all"})]
        public void WebDriverDataProviderMissingParamTest(Status status)
        {
            Assert.AreEqual(Status.Passed, status);
        }

        [UnitTest(DataProvider = "WrongName", Groups = new[] { "all" })]
        public void WebDriverDataProviderTypoTest(Status status)
        {
            Assert.AreEqual(Status.Passed, status);
        }
        [UnitTest(Groups = new[] { "all"})]
        public void WebDriverDataProviderForgotProviderTest(Status status)
        {
            Assert.AreEqual(Status.Passed, status);
        }

        [UnitTest(Groups = new[] { "all"})]
        public void NoDataprovider()
        {
            Assert.AreEqual(true, true);
        }

        [UnitTest(DataProvider = "UserPass", Groups = new[] { "all" })]
        public void DataProviderOnNoParamMethod()
        {
            Assert.AreEqual(true, true);
        }
    }
}
