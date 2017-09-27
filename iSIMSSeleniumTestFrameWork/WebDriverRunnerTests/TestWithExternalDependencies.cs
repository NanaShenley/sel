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
using WebDriverRunner.internals;

namespace WebDriverRunnerTests
{
    /*
     * Those tests should not run in a CI as is, as they're assuming there is a selenium grid available as a dependency.
     */
    //[TestClass]
    public class TestWithExternalDependencies
    {
        private IFilter<MethodInfo> manual = Helper.ByClass("WebDriverRunnerTests.mocktests.ManualTest");

                        readonly string _buildFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName) + "\\BuildOutput";


        [TestMethod]
        public void SimpleWebDriver()
        {
            var config = Helper.Create(manual);
            config.Hub = Configuration.BedfordHub;
            config.MaxThreads = 50;
            config.Browsers.Add("chrome");
            config.Browsers.Add("internet explorer");
            config.AddReporter( _buildFolder+"\\HtmlReport.dll");
            config.Output = @"c:\report";
            var runner = new Runner(config);
            runner.LoadTests();
            var results = runner.RunTests();
            runner.GenerateReports();
            Assert.AreEqual(0, results.FailedTests.Count);
            Assert.AreEqual(1, results.PassedTests.Count);

        }
    }
}
