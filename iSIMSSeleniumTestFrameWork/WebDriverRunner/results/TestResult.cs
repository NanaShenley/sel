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
using Selene.Support.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace WebDriverRunner.results
{
    [DataContract]
    class TestResult : ITestResult
    {

        private readonly List<object> _messages = new List<object>();
        private readonly List<ITestResult> _retriedResults = new List<ITestResult>();
        private readonly Stopwatch _watch = Stopwatch.StartNew();
        private readonly string _uuid;

        private TestResult() { }
        public TestResult(TestMethodInstance instance)
        {
            _watch.Start();
            Instance = instance;
            _uuid = Guid.NewGuid().ToString();
        }

        [DataMember]
        public Dictionary<string, object> Metadata { get { return Instance.GetMetadata(); } }

        [DataMember]
        public object[] Parameters { get { return Instance.GetParameters(); } }


        [DataMember]
        public string Uuid
        {
            get { return _uuid; }
        }

        [DataMember]
        public string MethodName
        {
            get { return Method.Name; }
        }

        [DataMember]
        public string Type { get { return Method.ReflectedType.FullName; } }

        [DataMember]
        public Status Status { get; set; }

      
        public TimeSpan Total { get { return _watch.Elapsed; } }

        [DataMember]
        public double TimeMillis { get { return Total.TotalMilliseconds; } }

        public void StopWatch()
        {
            _watch.Stop();
        }

        [DataMember]
        public TestMethodBaseAttribute Attr
        {
            get { return Instance.Attr; }
        }

        public Exception Exception { get; set; }
        [DataMember]
        public ExceptionW Error
        {
            get
            {
                if (Exception == null)
                {
                    return null;
                }
                return new ExceptionW(Exception);

            }
        }

        public TestMethodInstance Instance { get; private set; }

        public class ExceptionW
        {
            private readonly Exception _e;
            public ExceptionW(Exception e)
            {
                _e = e;
            }

            public string Message { get { return _e.Message; } }
            public string Type { get { return _e.GetType().Name; } }
            public string StackTrace
            {
                get
                {
                    return _e.StackTrace;
                }
            }

        }





        public MethodInfo Method
        {
            get { return Instance.GetMethod(); }
        }


        public void Log(object msg)
        {
            _messages.Add(msg);
        }

        public void AddRetriedResult(ITestResult result)
        {
            _retriedResults.Add(result);
        }



        public bool IsConfigurationMethod()
        {
            var attr = Instance.Attr.GetType();
            var res = typeof(ConfigurationMethodAttribute).IsAssignableFrom(attr);
            return res;
        }

        public List<ITestResult> RetriedResults { get { return _retriedResults; }
        }

        [DataMember]
        public List<object> Logs
        {
            get { return _messages; }
        }


        public int CompareTo(ITestResult other)
        {
            if (other == null)
            {
                return 1;
            }
            return string.Compare(Method.Name, other.Method.Name, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            var duration = string.Format(" total :{0:%m}min{0:%s}sec ", Total);
            var type = Instance.Attr.ToString();

            var retry = "";
            if (_retriedResults.Count != 0)
            {
                retry = " ( after " + _retriedResults.Count + " retries)";
            }
            var res = type + "(" + duration + ")[" + Instance.GetMetadata(WebContext.BrowserKey) + "]" + Instance + retry;
            var ex = "";
            if (Exception != null)
            {
                ex = Exception.GetType() + ":" + Exception.Message;
            }
            return res + " : " + Status + " (" + ex + ")";
        }


    }
}
