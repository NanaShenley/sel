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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using WebDriverRunner.ReportPlugin;
using WebDriverRunner.results;

namespace HtmlReport
{
    public class JsonBackedHtmlReport : IReporter
    {
        private const string ReportName = "data.js";

        public JsonBackedHtmlReport(string output)
        {
            Output = output + "/htmlreport/";
            Directory.CreateDirectory(Output);
        }

        public string Output { get; set; }

        public void GenerateReport(ISuiteResult results)
        {
            // produce the json data the report will need.
            var json = JsonConvert.SerializeObject(results, Formatting.Indented);
            var jsonp = "var result = " + json;

            var file = new StreamWriter(Output + "/" + ReportName);
            file.Write(jsonp);
            file.Close();

            // copy the static files
            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames();
            foreach (var resource in resources)
            {
                CopyResource(resource, assembly);
            }

        }

        private void CopyResource(string resource, Assembly assembly)
        {
            var b = resource.Replace("HtmlReport.report.", "");
            var folders = new List<string> { "css", "fonts", "images", "js" };
            // sub folder ?
            var folder = "";
            var fileName = b;
            if (folders.Contains(b.Split('.')[0]))
            {
                folder = b.Split('.')[0];
                Directory.CreateDirectory(Output + "/" + folder);
                fileName = b.Replace(folder + ".", "");
                folder += "/";
            }
            var stream = assembly.GetManifestResourceStream(resource);
            Stream output = new FileStream(Output + "/" + folder + fileName,FileMode.Create);
            stream.CopyTo(output);
            output.Close();
        }
    }
}
