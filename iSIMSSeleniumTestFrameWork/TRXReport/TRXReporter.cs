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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mustache;
using WebDriverRunner.ReportPlugin;
using WebDriverRunner.results;

namespace TRXReport
{
    class TrxReporterBase : IReporter
    {
        public TrxReporterBase(string output)
        {
            Output = output;
        }

        public string Output { get; set; }


        public void GenerateReport(ISuiteResult results, Dictionary<string, List<ITestResult>> dictionary)
        {
            throw new NotImplementedException();
        }

        public void GenerateReport(ISuiteResult results)
        {

            var data = new Dictionary<string, object>();
            var compiler = new FormatCompiler();

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("WebDriverRunner.reporters.Trx.template.xml");
            var reader = new StreamReader(stream);
            var generator = compiler.Compile(reader.ReadToEnd());
            data["Name"] = results.Name;

            data["allCount"] = results.Results().Count;
            data["passedCount"] = results.PassedTests.Count;
            data["failedCount"] = results.FailedTests.Count;
            data["skippedCount"] = results.SkippedTests.Count;

            data["all"] = results.Results();


            var duration = string.Format(" total :{0:%m}min{0:%s}sec ", results.Total);
            data["summary"] = "(" + results.PassedTests.Count + "," +
                +results.FailedTests.Count + "," +
                +results.SkippedTests.Count + ")" + duration;

            var content = generator.Render(data);

            var file = new StreamWriter(Output + "/" + "template.trx");
            file.Write(content);
            file.Close();
        }

    }
}
