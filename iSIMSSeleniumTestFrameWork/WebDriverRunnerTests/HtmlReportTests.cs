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

using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner;
using WebDriverRunner.Filters;

namespace WebDriverRunnerTests
{
    [TestClass]
    public class HtmlReportTests
    {
        private IFilter<MethodInfo> testsRun = Helper.ByClass(
            "WebDriverRunnerTests.mocktests.Tests1",
           "WebDriverRunnerTests.mocktests.SlowTests",
            "WebDriverRunnerTests.mocktests.Timeout",
            "WebDriverRunnerTests.mocktests.BeforeAfterSuite",
            "WebDriverRunnerTests.mocktests.InvocationCountTests");   
        
        private IFilter<MethodInfo> testsSkipped = Helper.ByClass(
            "WebDriverRunnerTests.mocktests.Tests1",
            "WebDriverRunnerTests.mocktests.SlowTests",
            "WebDriverRunnerTests.mocktests.Timeout",
            "WebDriverRunnerTests.mocktests.BeforeAfterSuite",
            "WebDriverRunnerTests.mocktests.BeforeAfterSuite2",
            "WebDriverRunnerTests.mocktests.InvocationCountTests");

        private IFilter<MethodInfo> testfails = Helper.ByClass(
          "WebDriverRunnerTests.mocktests.ExceptionTests");

                        readonly string _buildFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName) + "\\BuildOutput";

        [TestMethod]
        public void TestReport()
        {
            var config = Helper.Create(testfails);
            config.AddReporter(_buildFolder + "\\HtmlReport.dll");
            config.Output = @"c:\report";
            var runner = new Runner(config);
            runner.LoadTests();
            runner.RunTests();
            runner.GenerateReports();

        }
    }
}
