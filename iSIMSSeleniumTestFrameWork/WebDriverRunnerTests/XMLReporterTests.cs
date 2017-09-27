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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner;
using WebDriverRunner.internals;

namespace WebDriverRunnerTests
{
    [TestClass]
    public class XmlReporterTests
    {
                        readonly string _buildFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName) + "\\BuildOutput";

        [TestMethod]
        public void TestReport()
        {
            var args = "--include=all --dll=WebDriverRunnerTests.dll".Split(' ');
            var config = Configuration.Create(args);
            config.AddReporter(_buildFolder+"\\SonarXMLReport.dll");
            config.Output = @"c:\report";
            var runner = new Runner(config);
            runner.LoadTests();
            runner.RunTests();
            runner.GenerateReports();

          

        }

       
    }
}
