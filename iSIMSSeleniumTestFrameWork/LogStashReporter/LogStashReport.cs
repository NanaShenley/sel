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
using System.Configuration;
using System.Net.Sockets;
using System.Text;
using WebDriverRunner.results;
using WebDriverRunner.ReportPlugin;


namespace LogStashReporter
{
    public class LogStashReport : IReporter
    {
        private string Output;
        private readonly string _serviceHostName;
        private readonly int _servicePort;

        public LogStashReport(string output)
        {
            Output = output;
            _serviceHostName = LogStashSettings.Default.HostName;
            _servicePort = LogStashSettings.Default.Port;
        }

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

            foreach (var file in dictionary.Keys)
            {
                foreach (var result in dictionary[file])
                {
                    TcpClient clientSocket = new TcpClient();
                    clientSocket.Connect(_serviceHostName, _servicePort);
                    NetworkStream serverStream = clientSocket.GetStream();
                    string dataToSend = "TestCaseName=" + result.Instance + ",TestId=" + result.Uuid + ",TestStatus=" + MapToDesiredTag(result.Status);
                    if (result.Status != Status.Passed)
                     {
                         dataToSend = dataToSend + ",TestMessage=" +
                                       result.Exception.Message + ",TestStacktrace=" + result.Exception.StackTrace;
                     }
                     else
                     {
                         dataToSend = dataToSend + ",TestMessage=NA,TestStacktrace=na";
                     }
                    dataToSend = dataToSend.Replace(Environment.NewLine, "");
                    byte[] outStream = Encoding.UTF8.GetBytes(dataToSend);
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                    clientSocket.Close();
                }
               
            } 
            
        }

        private string MapToDesiredTag(Status status)
        {
            switch (status)
            {
                case Status.Skipped:
                    return "skipped";
                case Status.Failed:
                    return "Failed";
                case Status.Passed:
                    return "Passed";
                default:
                    throw new Exception("Can't add status " + status + "Not implemented.");
            }
        }
    }
}
    
