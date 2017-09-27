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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace WebDriverRunner.results
{
    [DataContract]
    class SuiteResult : ISuiteResult
    {
        private readonly ConcurrentBag<ITestResult> _results = new ConcurrentBag<ITestResult>();
        private readonly ConcurrentBag<MethodInfo> _methods = new ConcurrentBag<MethodInfo>();
        private readonly Stopwatch _watch = Stopwatch.StartNew();

        public SuiteResult(string name)
        {
            _watch.Start();
            Name = name;
        }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public double TimeMillis { get { return _watch.ElapsedMilliseconds; } }

        public void Add(ITestResult result)
        {
            _methods.Add(result.Method);
            _results.Add(result);
        }

        public TimeSpan Total { get { return _watch.Elapsed; } }


        public void StopWatch()
        {
            _watch.Stop();
        }

        public Status GetStatus()
        {
            if (FailedTests.Count != 0)
            {
                return Status.Failed; 
            }
            if (SkippedTests.Count != 0)
            {
                return Status.Failed; 
            }
            if (FailedConfiguration.Count != 0)
            {
                return Status.Failed;
            }
            if (SkippedConfiguration.Count != 0)
            {
                return Status.Failed;
            }
            return Status.Passed;
        }



        public List<ITestResult> Results()
        {
            return _results.OrderBy(result => result.Method.Name).ToList();
        }

        public List<ITestResult> Get(MethodInfo method)
        {
            return _results
                .Where(result => result.Method.Equals(method))
                .OrderBy(result => result.Method.Name)
                .ToList();
        }

        public List<ITestResult> GetFailedResults(bool configurationMethod)
        {
            return _results
                .Where(result => result.Status == Status.Failed && result.IsConfigurationMethod() == configurationMethod)
                .OrderBy(result => result.Method.Name)
                .ToList();

        }

        public List<ITestResult> GetPassedResults(bool configurationMethod)
        {
            return _results
                .Where(result => result.Status == Status.Passed && result.IsConfigurationMethod() == configurationMethod)
                .OrderBy(result => result.Method.Name)
                .ToList();

        }

        public List<ITestResult> GetSkippedResults(bool configurationMethod)
        {
            return _results
                .Where(result => result.Status == Status.Skipped && result.IsConfigurationMethod() == configurationMethod)
                .OrderBy(result => result.Method.Name)
                .ToList();

        }

        public List<MethodInfo> GetMethods()
        {
            return _methods.ToList();
        }

        [DataMember]
        public List<ITestResult> FailedTests
        {
            get { return GetFailedResults(false); }
        }

        [DataMember]
        public List<ITestResult> PassedTests
        {
            get { return GetPassedResults(false); }
        }

        [DataMember]
        public List<ITestResult> SkippedTests
        {
            get { return GetSkippedResults(false); }
        }

        [DataMember]
        public List<ITestResult> FailedConfiguration
        {
            get { return GetFailedResults(true); }
        }

        [DataMember]
        public List<ITestResult> PassedConfiguration
        {
            get { return GetPassedResults(true); }
        }

        [DataMember]
        public List<ITestResult> SkippedConfiguration
        {
            get { return GetSkippedResults(true); }
        }


        public int GetTotalTests()
        {
            return PassedTests.Count + FailedTests.Count + SkippedTests.Count;
        }

        public int GetTotalConfiguration()
        {
            return PassedConfiguration.Count + FailedConfiguration.Count + SkippedConfiguration.Count;
        }
    }
}
