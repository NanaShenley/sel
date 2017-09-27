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
using System.Xml;
using WebDriverRunner.ReportPlugin;
using WebDriverRunner.results;

namespace SonarXMLReport
{
    public class SonarXmlReport : IReporter
    {
        public static readonly string ReportName = "e2e_report.xml";


        public SonarXmlReport(string output)
        {
            Output = output;
        }

        public string Output { get; set; }

        public void GenerateReport(ISuiteResult results)
        {
            var dictionary = new Dictionary<string, List<ITestResult>>();
            foreach (var result in results.Results())
            {
                var attr = result.Instance.Attr;
                if (!dictionary.ContainsKey(attr.File))
                {
                    dictionary[attr.File] = new List<ITestResult>();
                }
                dictionary[attr.File].Add(result);
            }

            var writer = new XmlTextWriter(Output + "/" + ReportName, null)
            {
                Formatting = Formatting.Indented
            };
            writer.WriteStartElement("unitTest");
            writer.WriteAttributeString("version", "1");


            foreach (var file in dictionary.Keys)
            {
                writer.WriteStartElement("file");
                writer.WriteAttributeString("path", file);

                foreach (var result in dictionary[file])
                {
                    writer.WriteStartElement("testCase");
                    writer.WriteAttributeString("name", result.Instance.ToString() + result.Uuid);
                    writer.WriteAttributeString("duration", Math.Ceiling(result.Total.TotalMilliseconds ).ToString());
                    if (result.Status != Status.Passed)
                    {
                        writer.WriteStartElement(MapToDesiredTag(result.Status));

                        if (result.Exception != null)
                        {
                            writer.WriteAttributeString("message", result.Exception.Message);
                            writer.WriteString(result.Exception.StackTrace);
                        }

                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.Close();
        }


        private string MapToDesiredTag(Status status)
        {
            switch (status)
            {
                case Status.Skipped:
                    return "skipped";
                case Status.Failed:
                    return "failure";
                default:
                    throw new Exception("Can't add status " + status + " in the xml doc. Not implemented.");
            }
        }
    }


}
