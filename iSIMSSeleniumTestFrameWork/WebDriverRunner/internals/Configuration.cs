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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Options;
using WebDriverRunner.Filters;
using WebDriverRunner.ReportPlugin;
using Selene.Support.Attributes;

namespace WebDriverRunner.internals
{
    public class Configuration
    {
        private const string DefaultHub = "http://localhost:4444/wd/hub";
        private const int DefaultMaxThreads = 10;
        private const bool DefaulOverRideDisabled = false;
        private const bool DefaultPauseOnExit = false;
        private static readonly string[] DefaultBrowsers = { "internet explorer" };
        private static readonly TimeSpan DefaultSuiteTimeout = TimeSpan.FromMinutes(120);
        private static readonly string DefaultOutput = Path.GetFullPath(".");
        private static readonly string[] DefaultInclude = { };
        private static readonly string[] DefaultExclude = { "broken" };


        public static readonly Uri BedfordHub = new Uri("http://bedcsscmssl01:4444/wd/hub");

        private readonly OptionSet _options;

        public List<string> Dlls { get; private set; }
        public int MaxThreads { get; set; }
        public Uri Hub { get; set; }
        public List<string> Browsers { get; set; }
        public TimeSpan SuiteTimeout { get; set; }
        public string Output { get; set; }
        public List<string> Includes { get; private set; }
        public bool OverRideDisabled { get; set; }
        public string SeleneApiUrl { get; private set; }
        public string Team { get; private set; }
        public bool PauseOnExit { get; set; }
        public Guid AutomatedRunId { get; private set; }
        public Guid AssemblyRunId { get; private set; }
        public bool IncludeNotDoneTests { get; private set; }
        public string TestSuiteBuildNo { get; private set; }
        public bool ExcludeFromReports { get; private set; }
        public Variant VariantUnderTest { get; private set; }
        public bool SecurityScan { get; private set; }
        public string ProxyOverride { get; set; }
        public string SecurityScanReport { get; private set; }
        public TimeSpan BrowserResponseTimeout { get; private set; }

        private readonly List<Type> _reporters = new List<Type>();

        public ReadOnlyCollection<Type> Reporters
        {
            get
            {
                return new ReadOnlyCollection<Type>(_reporters);
            }
        }

        public IFilter<MethodInfo> MethodFilter;

        bool _help = false;
        int _verbose = 0;
        private static readonly string[] DefaultReporter = { @"..\\..\\..\\HtmlReport\\bin\\Debug\\HtmlReport.dll" };

        public void AddReporter(string typeName)
        {
            var loader = new PluginLoader(typeof(IReporter));
            // check in all loaded assemblies.
            var plugins = loader.GetPluginEntryPoints(typeName);
            _reporters.AddRange(plugins);
        }

        public void AddInclude(string group)
        {
            Includes.Add(group);
        }

        public Configuration()
        {
            Dlls = new List<string>();
            Browsers = new List<string>();
            MaxThreads = DefaultMaxThreads;
            Hub = new Uri(DefaultHub);
            SuiteTimeout = DefaultSuiteTimeout;
            Output = DefaultOutput;
            Includes = DefaultInclude.ToList();
            OverRideDisabled = DefaulOverRideDisabled;
            SeleneApiUrl = string.Empty;
            Team = string.Empty;
            PauseOnExit = false;
            AutomatedRunId = Guid.Empty;
            IncludeNotDoneTests = false;
            TestSuiteBuildNo = string.Empty;
            ExcludeFromReports = false;
            AssemblyRunId = Guid.NewGuid();
            SecurityScan = false;
            ProxyOverride = TestSettings.TestDefaults.Default.Proxy;
            BrowserResponseTimeout = TestSettings.TestDefaults.Default.BrowserResponseTimeout;
            SecurityScanReport = "";

            //NiStPri by default, as this is what the tests were originally tested against
            VariantUnderTest = Variant.NorthernIrelandStatePrimary;

            // ReSharper disable ConvertToLambdaExpression
            _options = new OptionSet()
            {
                    { "dll=","Set the dll to be tests. Can be passed multiple times.",      v => {Dlls.Add(v);} },
                    { "hub=", "Set the hub to be used. Defaults to "+DefaultHub ,    v => Hub = new Uri(v) },
                    { "maxThreads=", "Number of concurrent threads to run.Defaults to "+DefaultMaxThreads, (int v)=> MaxThreads = v},
                    { "browser=","Set a browser to test.Can be passed multiple times.If none specified, defaults to "+DefaultBrowsers,v => {Browsers.Add(v);} },
                    { "suiteTimeoutInMinutes=","Set a suite Timeout.If none specified, defaults to "+DefaultSuiteTimeout,(int v) => SuiteTimeout = TimeSpan.FromMinutes(v) },
                    { "reporter=","Add a reporter that will be called at the end of the suite run to produce a report.Can be called multiple times. Defaults to "+DefaultReporter, v => AddReporter(v) },
                    { "output=","Sets the base forder where all the artifacts will be produced.Reports, screenshots etc.Defaults to "+DefaultOutput, v => Output = v },
                    { "include=","Sets the group of test to run. Can be called multiple time."+DefaultInclude, AddInclude  },
                    { "OverRideDisabled=","Over rides disabled test."+DefaulOverRideDisabled, (bool v)=> OverRideDisabled = v},
                    { "sut=", "The system under test. DEFAULT: As per App.Config OPTIONS: \"lab-two\" | \"lab-five\" | \"lab-sims8\"", x=>{TestSettings.Configuration.SystemUnderTest = x.ToLowerInvariant();}},
                    { "team=", "The name of the team these tests belong to", x => {Team = x;}},
                    { "seleneApiUrl=", "The Selene API Url to report to", x => {SeleneApiUrl = x.ToLowerInvariant();}},
                    { "automatedRunId=", "The Guid of the automated run.", x => {AutomatedRunId = Guid.Parse(x);} },
                    { "testSuiteBuildNo=", "The build version of the current Test Suite (eg. the RollingSelenium build no.", x => {TestSuiteBuildNo = x.ToLowerInvariant();}},
                    { "t|forceTestUser" ,"Force login as TestUser.", x=>{ TestSettings.Configuration.ForceTestUserLogin = true; }},
                    { "v|verbose",  v => { ++_verbose; } },
                    { "h|?|help",   v => _help = v != null },
                    { "n|includeNotDone", "Includes tests marked with the [NotDone] attribute. [NotDone] tests are excluded by default.", x => IncludeNotDoneTests = true },
                    { "pauseOnExit=", "Determines whether the program will wait for keypress on exit." +DefaultPauseOnExit, (bool x) => PauseOnExit = x },
                    { "e|excludeFromReports", "Excludes results of this run from Selene Reports",  x => {ExcludeFromReports =true;}},
                    { "variant=", "Runs tests for a particlar variant", x => {VariantUnderTest = ParseVariant(x); }},
                    { "assemblyRunId=", "The Guid of the assembly run.", x => {AssemblyRunId = Guid.Parse(x);} },
                    { "securityScan" ,"Include security scan as part of run (will start and stop OWASP ZAP if not running).", x => { SecurityScan = true; } },
                    { "proxy=" ,"The address of the proxy to use for tests.", x => { ProxyOverride = x; } },
                    { "securityScanReport=" ,"Generate security scan report (using OWASP ZAP) on completion.", x => { SecurityScanReport = x; } },
            };
            // ReSharper restore ConvertToLambdaExpression

        }

        private List<string> Parse(string[] args)
        {
            var unknown = _options.Parse(args);

            if (Browsers.Count == 0)
            {
                Browsers.AddRange(DefaultBrowsers);
            }
            if (Reporters.Count == 0)
            {
                foreach (var reporter in DefaultReporter)
                {
                    AddReporter(reporter);
                }
            }

            if (Includes.Count == 0)
            {
                MethodFilter = new DefaultTestMethodFilter(this.IncludeNotDoneTests, this.VariantUnderTest, this.AutomatedRunId);
            }
            else
            {
                //TODO include variants
                var groups = Includes.Select(include => new GroupFilter(include, this.IncludeNotDoneTests, this.VariantUnderTest, this.AutomatedRunId)).Cast<IFilter<MethodInfo>>().ToList();
                MethodFilter = new Or<MethodInfo>(groups);
            }
            return unknown;
        }

        public Variant ParseVariant(string variant)
        {
            var loweredVariant = variant.ToLowerInvariant();

            switch(loweredVariant)
            {
                case "engstpri":
                    return Variant.EnglishStatePrimary;
                case "welstpri":
                    return Variant.WelshStatePrimary;
                case "nistpri":
                    return Variant.NorthernIrelandStatePrimary;
                default:
                    throw new ArgumentException(string.Format("The variant \"{0}\" is not supported.", variant));
            }
        }


        public IFilter<Assembly> AssemblyFilter
        {
            get
            {
                var all = Dlls.Select(dll => new SingleAssembly(dll)).Cast<IFilter<Assembly>>().ToList();
                return new Or<Assembly>(all);
            }
        }

        private void ShowHelp()
        {
            _options.WriteOptionDescriptions(Console.Out);
        }

        public void Validate()
        {
        }

        public static Configuration Create(string[] args)
        {
            var config = new Configuration();
            try
            {
                var unknown = config.Parse(args);

                if (unknown.Count != 0)
                {
                    throw new OptionException("Cannot parse parameter " + unknown[0], unknown[0]);
                }

                config.Validate();
                return config;
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                config.ShowHelp();
                throw e;

            }
        }
    }
}
