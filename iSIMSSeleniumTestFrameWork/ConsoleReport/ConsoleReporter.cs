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
using System.Linq;
using WebDriverRunner.ReportPlugin;
using WebDriverRunner.results;

namespace ConsoleReport
{
    public class ConsoleReporter : IReporter
    {

        public ConsoleReporter(string output)
        {
            Output = output;
        }

        public string Output { get; set; }

        public void GenerateReport(ISuiteResult results)
        {
            var totalConfigFailed = results.FailedConfiguration.Count;
            var totalConfigSkipped = results.SkippedConfiguration.Count;

            var totalTestPassed = results.PassedTests.Count;
            var totalTestFailed = results.FailedTests.Count;
            var totalTestSkipped = results.SkippedTests.Count;

            var total = results.Total;

            var summary = "-------------" + results.GetStatus() + "---------\n";
            summary += "Ran " + (totalTestPassed + totalTestFailed + totalTestSkipped) + " tests (" + DurationPretty(total) + ")\n";
            if ((totalTestFailed + totalTestSkipped) != 0)
            {
                summary += "\nTest Methods(" + results.GetTotalTests() + "):\n";
                summary += "\tFailed: " + totalTestFailed + "\n";
                summary += "\tSkipped: " + totalTestSkipped + "\n";
            }

            if ((totalConfigFailed + totalConfigSkipped) != 0)
            {
                summary += "\nConfiguration Methods(" + results.GetTotalConfiguration() + "):\n";
                summary += "\tFailed:" + totalConfigFailed + "\n";
                summary += "\tSkipped:" + totalConfigSkipped + "\n";
            }
            summary += "-----------------------------\n";


            summary = results.FailedTests.Aggregate(summary, (current, result) => current + ("FAILED : " + result.Method + " : " + (result.Exception == null ? null : result.Exception.Message) + "\n"));
            summary = results.SkippedTests.Aggregate(summary, (current, result) => current + ("SKIPPED : " + result.Method + " : " + (result.Exception == null ? null : result.Exception.Message) + "\n"));

            Console.WriteLine(summary);
        }



        private string DurationPretty(TimeSpan total)
        {
            if (total.TotalSeconds < 1)
            {
                return total.TotalMilliseconds + " milliseconds";
            }
            if (total.TotalMinutes < 1)
            {
                return string.Format("{0:ss}", total) + " seconds";
            }
            return string.Format("{0:mm\\m\\i\\nss}", total);
        }
    }
}
