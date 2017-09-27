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
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Options;
using WebDriverRunner.internals;

namespace WebDriverRunnerTests
{
    [TestClass]
    public class ConfigurationTests
    {
                        readonly string _buildFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName) + "\\BuildOutput";
        

        [TestMethod]
        public void CanSetDll()
        {
        var stringBuilder = "-dll=bla.dll_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            var config = Configuration.Create(args);
            Assert.AreEqual(1, config.Dlls.Count, 1);
            Assert.AreEqual("bla.dll", config.Dlls[0]);
        }

        [TestMethod]
        public void CanSetMultipleDlls()
        {
            var stringBuilder = "-dll=bla.dll_-dll=bla2.dll_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            var config = Configuration.Create(args);
            Assert.AreEqual(2, config.Dlls.Count);
            Assert.IsTrue(config.Dlls.Contains("bla.dll"));
            Assert.IsTrue(config.Dlls.Contains("bla2.dll"));
        }
        [TestMethod]
        public void CanSetMaxThread()
        {
            var stringBuilder = "-dll=bla.dll_-maxThreads=5_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            var config = Configuration.Create(args);
            Assert.AreEqual(5, config.MaxThreads);
        }

        [TestMethod]
        public void CanSetHub()
        {
            var stringBuilder = "-dll=bla.dll_-maxThreads=5_-hub=http://localhost:5555/wd/hub_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            var config = Configuration.Create(args);
            Assert.AreEqual("http://localhost:5555/wd/hub", config.Hub.ToString());
        }

        [TestMethod]
        public void CanSetBrowsers()
        {
            var stringBuilder = "-browser=chrome_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            var config = Configuration.Create(args);
            Assert.AreEqual(1, config.Browsers.Count);
            Assert.AreEqual("chrome", config.Browsers[0]);
        }

        [TestMethod]
        public void DefaultToIe()
        {
            var stringBuilder = "-dll=bla.dll_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            var config = Configuration.Create(args);
            Assert.AreEqual(1, config.Browsers.Count);
            Assert.AreEqual("internet explorer", config.Browsers[0]);
        }

        [TestMethod]
        public void CanSetIeAndChrome()
        {
            var stringBuilder = "-browser=chrome_-browser=internet explorer_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            
            var config = Configuration.Create(args);
            Assert.AreEqual(2, config.Browsers.Count);
            Assert.IsTrue(config.Browsers.Contains("internet explorer"));
            Assert.IsTrue(config.Browsers.Contains("chrome"));
        }

        [TestMethod]
        public void CanSetSuiteTimeout()
        {
            var stringBuilder = "--suiteTimeoutInMinutes=5_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            var config = Configuration.Create(args);
            Assert.AreEqual(TimeSpan.FromMinutes(5), config.SuiteTimeout);
        }

        [TestMethod]
        [ExpectedException(typeof(OptionException))]
        public void CanFindTypos()
        {
            var stringBuilder = "--SUITETIMEOUT=5_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            Configuration.Create(args);
        }

        [TestMethod]
        public void DefaultsToHtmlReporter()
        {
            string[] args = { };
            var config = Configuration.Create(args);
            Assert.AreEqual(1, config.Reporters.Count);
            Assert.AreEqual("JsonBackedHtmlReport", config.Reporters[0].Name);
        }

        [TestMethod]
        public void CanAddReporterInADifferentAssembly()
        {
            string[] args = { "-reporter="+ _buildFolder+"\\HtmlReport.dll" };
            var config = Configuration.Create(args);
            Assert.AreEqual(1, config.Reporters.Count);
            Assert.AreEqual("HtmlReport.JsonBackedHtmlReport", config.Reporters[0].FullName);
        }

        [TestMethod]
        public void CanSetOutput()
        {
            var stringBuilder = "-output=c:\\reports\\_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            var config = Configuration.Create(args);
            Assert.AreEqual("c:\\reports\\", config.Output);
        }
        [TestMethod]
        public void OutpuDefaultsToCurrentFolder()
        {
            var config = new Configuration();
            var p = Path.GetFullPath(".");
            Assert.AreEqual(p, config.Output);
        }

        [TestMethod]
        [ExpectedException(typeof(RunnerException))]
        public void DetectWrongReportersClassName()
        {
            string[] args = { "-reporter=WebDriverRunner.reporters.ConsoleReporter2" };
            Configuration.Create(args);
        }

        [TestMethod]
        [ExpectedException(typeof(RunnerException))]
        public void DetectWrongReportersDllName()
        {
            string[] args = { "-reporter=HtmlReport2.dll" };
            Configuration.Create(args);
        }

        [TestMethod]
        public void NoIncludeByDefault()
        {
            var config = new Configuration();
            Assert.AreEqual(0, config.Includes.Count);
        }

        [TestMethod]
        public void CanAddInclude()
        {
            var stringBuilder = "--include=bla_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            var config = Configuration.Create(args);
            Assert.AreEqual(1, config.Includes.Count);
            Assert.AreEqual("bla", config.Includes.First());
        }

        [TestMethod]
        public void CanAddMultipleIncludes()
        {
            var stringBuilder = "--include=bla_--include=test_-reporter=" + _buildFolder + "\\HtmlReport.dll";
            var args = stringBuilder.Split('_');
            var config = Configuration.Create(args);
            Assert.AreEqual(2, config.Includes.Count);
            Assert.IsTrue(config.Includes.Contains("bla"));
            Assert.IsTrue(config.Includes.Contains("test"));
        }

    }
}
