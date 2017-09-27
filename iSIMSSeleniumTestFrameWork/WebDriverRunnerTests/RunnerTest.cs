/*************************************************************************
* 
* Copyright © Capita Children's Services 2015
* All Rights Reserved.
* Proprietary and confidential
* Written by Steve Gray <steve.gray@capita.co.uk> and Francois Reynaud<Francois.Reynaud@capita.co.uk> 2015
* 
* NOTICE:  All Source Code and information contained herein remains
* the property of Capita Children's Services. The intellectual and technical concepts contained
* herein are proprietary to Capita Children's Services 2015 and may be covered by U.K, U.S and Foreign Patents,
* patents in process, and are protected by trade secret or copyright law.
* Dissemination of this information or reproduction of this material
* is strictly forbidden unless prior written permission is obtained
* from Capita Children's Services.
*
* Source Code distributed under the License is distributed on an
* "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied.  
*/
using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner;
using WebDriverRunner.Filters;
using WebDriverRunner.internals;

namespace WebDriverRunnerTests
{
    [TestClass]
    public class RunnerTest
    {
        private IFilter<MethodInfo> test1 = Helper.ByClass("WebDriverRunnerTests.mocktests.Tests1");
        private IFilter<MethodInfo> slows = Helper.ByClass("WebDriverRunnerTests.mocktests.SlowTests");
        private IFilter<MethodInfo> timeout = Helper.ByClass("WebDriverRunnerTests.mocktests.Timeout");
        private IFilter<MethodInfo> invoc = Helper.ByClass("WebDriverRunnerTests.mocktests.InvocationCountTests");
        private IFilter<MethodInfo> dp = Helper.ByClass("WebDriverRunnerTests.mocktests.DataProviderTests");
        private IFilter<MethodInfo> dpn = Helper.ByClass("WebDriverRunnerTests.mocktests.DataProviderNegTests");
        private IFilter<MethodInfo> _groups = Helper.ByClass("WebDriverRunnerTests.mocktests.GroupInclude");
                        readonly string _buildFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName) + "\\BuildOutput";
        [TestMethod]
        public void RunTests()
        {
            var config = Helper.Create(test1);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();

            Assert.AreEqual(1, results.FailedTests.Count);
            Assert.AreEqual(3, results.PassedTests.Count);
        }

        [TestMethod]
        public void RunSlowTestsSequentially()
        {
            var config = Helper.Create(slows);
            config.MaxThreads = 1;

            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();

            Assert.AreEqual(4, results.PassedTests.Count);
            Assert.IsTrue(results.Total > TimeSpan.FromSeconds(1), " results.Total:" + results.Total);
        }

        [TestMethod]
        public void RunSlowTestsParallel()
        {
            var config = Helper.Create(slows);
            config.MaxThreads = 2;

            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();

            Assert.AreEqual(4, results.PassedTests.Count);
            Assert.IsTrue(results.Total < TimeSpan.FromSeconds(1), "total is " + results.Total);
        }

        [TestMethod]
        public void TimeoutMethod()
        {
            var config = Helper.Create(timeout);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            Assert.AreEqual(1, results.FailedTests.Count);
        }

        [TestMethod]
        public void InvocationCountMethod()
        {
            var config = Helper.Create(invoc);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            Assert.AreEqual(52, results.PassedTests.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(RunnerException))]
        public void CannotGenerateReportBeforeTestsDone()
        {
            var runner = new Runner();
            runner.LoadTests();
            runner.GenerateReports();
        }


        [TestMethod]
        public void DataProviderTests()
        {
            var config = Helper.Create(dp);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            Assert.AreEqual(4, results.PassedTests.Count);
            Assert.AreEqual(1, results.FailedTests.Count);
        }

        [TestMethod]
        public void DataProviderNegTests()
        {
            var config = Helper.Create(dpn);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            runner.GenerateReports();
            Assert.AreEqual(3, results.PassedTests.Count);
            Assert.AreEqual(4, results.SkippedTests.Count);
        }

        [TestMethod]
        public void RunTestFromAnotherAssembly()
        {
            var config = new Configuration();
            config.Dlls.Add( _buildFolder+"\\SingleTestForTesting.dll");
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            runner.GenerateReports();
            Assert.AreEqual(1, results.PassedTests.Count);
            Assert.AreEqual(1, results.FailedTests.Count);
        }

        [TestMethod]
        public void Group1Group()
        {
            var args= "--include=group1 --dll=WebDriverRunnerTests.dll".Split(' ');
            var config = Configuration.Create(args);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            runner.GenerateReports();
            Assert.AreEqual(5, results.PassedTests.Count);
        }

        [TestMethod]
        public void Group2Group()
        {
            var args = "--include=group2 --dll=WebDriverRunnerTests.dll".Split(' ');
            var config = Configuration.Create(args);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            runner.GenerateReports();
            Assert.AreEqual(4, results.PassedTests.Count);
        }
        [TestMethod]
        public void GroupAAndB()
        {
            var args = "--include=A --include=B --dll=WebDriverRunnerTests.dll".Split(' ');
            var config = Configuration.Create(args);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            runner.GenerateReports();
            Assert.AreEqual(4, results.PassedTests.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(RunnerException))]
        public void GroupIsCaseSensitive()
        {
            var args = "--include=a --dll=WebDriverRunnerTests.dll".Split(' ');
            var config = Configuration.Create(args);
            var runner = new Runner(config);
            runner.LoadTests();
            runner.RunTests();
    
        }
    }
}
