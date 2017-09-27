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
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner;
using WebDriverRunner.Filters;

namespace WebDriverRunnerTests
{

    [TestClass]
    public class ConfigurationMethods
    {
        private IFilter<MethodInfo> basuite = Helper.ByClass("WebDriverRunnerTests.mocktests.BeforeSuite");
        private IFilter<MethodInfo> basuite2 = Helper.ByClass("WebDriverRunnerTests.mocktests.BeforeSuite2");
        private IFilter<MethodInfo> beforeSuiteWebTest = Helper.ByClass("WebDriverRunnerTests.mocktests.BeforeSuiteWebTest");
        private IFilter<MethodInfo> afterSuiteWebTest =  Helper.ByClass("WebDriverRunnerTests.mocktests.AferSuiteWebTest");

        [TestMethod]
        public void Runs()
        {
            var config = Helper.Create(basuite);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            runner.GenerateReports();
            Assert.AreEqual(3,results.PassedTests.Count);
            Assert.AreEqual(0,results.PassedConfiguration.Count);
        }

        [TestMethod]
        public void BeforeSuiteWebTestRuns()
        {
            var config = Helper.Create(beforeSuiteWebTest);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            runner.GenerateReports();
            Assert.AreEqual(3, results.PassedTests.Count);
        }

        [TestMethod]
        public void AfterSuiteWebTestRuns()
        {
            var config = Helper.Create(beforeSuiteWebTest);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            runner.GenerateReports();
            Assert.AreEqual(3, results.PassedTests.Count);

        }

        [TestMethod]
        public void SkippTestsWhenConfigFails()
        {
            var config = Helper.Create(basuite2);
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            runner.GenerateReports();
            Assert.AreEqual(3, results.PassedTests.Count);
            Assert.AreEqual(0, results.SkippedTests.Count);
            Assert.AreEqual(0, results.PassedConfiguration.Count);
            Assert.AreEqual(0, results.FailedConfiguration.Count);
        }
    }
}
