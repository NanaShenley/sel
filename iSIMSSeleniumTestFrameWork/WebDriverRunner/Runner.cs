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
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebDriverRunner.Exploders;
using WebDriverRunner.Filters;
using WebDriverRunner.internals;
using WebDriverRunner.Listners;
using WebDriverRunner.ReportPlugin;
using WebDriverRunner.results;
using WebDriverRunner.testfinder;

namespace WebDriverRunner
{
    public class Runner
    {
        private readonly int _maxThreads;
        private readonly TimeSpan _suiteTimeout;

        private readonly List<IReporter> _reporters = new List<IReporter>();
        private readonly ConcurrentQueue<TestMethodInstance> _queue = new ConcurrentQueue<TestMethodInstance>();
        private readonly MethodInstanceRunner _methodRunner;

        private readonly ISuiteResult _suiteResult;
        private readonly EventWaitHandle _event = new ManualResetEvent(false);
        private int _counter;
        private IEnumerable<MethodInfo> _befores;
        private IEnumerable<MethodInfo> _afters;
        private readonly Configuration _config;
        private Boolean _finished;

        private readonly ConcurrentQueue<TestMethodInstance> _beforesuiteWebTestMethodQueue =
            new ConcurrentQueue<TestMethodInstance>();
        private readonly ConcurrentQueue<TestMethodInstance> _afterSuiteWebTestMethodQueue =
            new ConcurrentQueue<TestMethodInstance>();

        public Runner()
            : this(new Configuration())
        {
        }

        public Runner(Configuration config)
        {
            _suiteTimeout = config.SuiteTimeout;
            _maxThreads = config.MaxThreads;
            _config = config;

            _suiteResult = new SuiteResult("Test Run, " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            var testListeners = new List<ITestListner>
            {
                new ConsoleTestListener(config),
                new WebDriverTestListener(config),
                new BeforesuiteWebTestListner(config),
                new AfterSuiteWebTestListner(config)
            };

            if (!config.ExcludeFromReports && !string.IsNullOrEmpty(config.SeleneApiUrl) && SeleneTestListener.SeleneIsReachableSync(config.SeleneApiUrl))
            {
                testListeners.Add(new SeleneTestListener(config));
                SeSugar.Environment.Logger.LogLine("Connected to Scrummy.");
            }
            else
            {                
                SeSugar.Environment.Logger.LogLine("Could not connect to Scrummy - web logging not enabled for this test run.");
            }

            _methodRunner = new MethodInstanceRunner(testListeners);

            foreach (var reporter in _config.Reporters)
            {
                var r = (IReporter)Activator.CreateInstance(reporter, _config.Output);
                _reporters.Add(r);
            }
        }

        public ISuiteResult GetResults
        {
            get
            {
                return _suiteResult;
            }
        }


        private void LoadTests(Assembly assembly, IFilter<MethodInfo> methodFilter)
        {
            var beforeSuiteWebTestFinder = new BeforeSuiteWebTestFinder(assembly, new BeforeSuiteWebTestMethodFilter());
            IEnumerable<MethodInfo> beforeSuiteWebTestMethods = beforeSuiteWebTestFinder.FindAllMethods();
            var finder = new TestFinder(assembly, methodFilter);
            IEnumerable<MethodInfo> methods = finder.FindAllMethods();
            var afterSuiteWebTestFinder = new AfterSuiteWebTestFinder(assembly, new AfterSuiteWebTestMethodFilter());
            IEnumerable<MethodInfo> afterSuiteWebTestMethods = afterSuiteWebTestFinder.FindAllMethods();

            //_befores = finder.GetSuiteMethods(methods, typeof(BeforeSuiteAttribute));
            //_afters = finder.GetSuiteMethods(methods, typeof(AfterSuiteAttribute));

            foreach (var exploder in beforeSuiteWebTestMethods.Select(beforesuiteWebTestMethod => new BeforeSuiteWebTestMethodExploder(beforesuiteWebTestMethod, _config)))
            {
                try
                {
                    var instances = exploder.ExplodeBeforeSuiteWebTest();
                    foreach (var instance in instances)
                    {
                        _beforesuiteWebTestMethodQueue.Enqueue(instance);
                    }
                }
                catch (RunnerException e)
                {
                    var instance = exploder.GetSkippedInstance(e);
                    _beforesuiteWebTestMethodQueue.Enqueue(instance);
                }
            }

            foreach (var exploder in methods.Select(method => new WebTestMethodExploder(method, _config)))
            {
                try
                {
                    var instances = exploder.Explode();
                    foreach (var instance in instances)
                    {
                        _queue.Enqueue(instance);
                    }
                }
                catch (RunnerException e)
                {
                    var instance = exploder.GetSkippedInstance(e);
                    _queue.Enqueue(instance);
                }
            }

            foreach (var exploder in afterSuiteWebTestMethods.Select(afterSuiteWebTestMethod => new AfterSuiteWebTestMethodExploder(afterSuiteWebTestMethod, _config)))
            {
                try
                {
                    var instances = exploder.ExplodeAfterSuiteWebTest();
                    foreach (var instance in instances)
                    {
                        _afterSuiteWebTestMethodQueue.Enqueue(instance);
                    }
                }
                catch (RunnerException e)
                {
                    var instance = exploder.GetSkippedInstance(e);
                    _afterSuiteWebTestMethodQueue.Enqueue(instance);
                }
            }
        }

        public void LoadTests()
        {
            foreach (var dll in _config.Dlls)
            {
                var a = Assembly.LoadFrom(dll);
                LoadTests(a, _config.MethodFilter);
                
                //TODO - do something with "Not Done" tests?
                //IEnumerable<TestMethodInstance> notDones = GetNotDoneTests(a, new NotDoneFilter(), _config);
                //var notDoneCount = notDones.Count();
            }
        }

        private IEnumerable<TestMethodInstance> GetNotDoneTests(Assembly a, NotDoneFilter notDoneFilter, Configuration config)
        {
            var finder = new TestFinder(a, notDoneFilter);
            var methodExploder = finder.FindAllMethods().Select(method => new WebTestMethodExploder(method, config));
            return methodExploder.SelectMany(x => x.Explode());
        }        

        public int GetQueueCount()
        {
            return _queue.Count;
        }

        public static void Log(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public ISuiteResult RunTests()
        {
            //bool configOk = ConfigSuiteMethods(_befores);
            bool configOk = BeforeSuiteWebTestMethods(_beforesuiteWebTestMethodQueue);

            if (configOk)
            {
                if (_queue.IsEmpty)
                {
                    if (_config.Dlls.Any())
                        throw new RunnerException(string.Format("No tests found in {0}.", _config.Dlls.First()));
                    else
                        throw new RunnerException("No tests found");
                }

                int workerThreads, complete;
                ThreadPool.GetMinThreads(out workerThreads, out complete);
                ThreadPool.SetMinThreads(Math.Min(_maxThreads, _queue.Count), complete);

                for (var i = 0; i < _maxThreads; i++)
                {

                    StartConsumer(_queue);
                }

                // TODO freynaud if time out , fail the suite.
                _event.WaitOne(_suiteTimeout);
            }
            else
            {
                // something went wrong in the setup. No point running anything
                while (!_queue.IsEmpty)
                {
                    TestMethodInstance instance;
                    _queue.TryDequeue(out instance);
                    var result = new TestResult(instance) { Status = Status.Skipped };
                    _suiteResult.Add(result);
                }
            }

            //  var afterOk = ConfigSuiteMethods(_afters);
            bool afterOk = AfterSuiteWebTestMethods(_afterSuiteWebTestMethodQueue);
            if (!afterOk)
            {
                // TODO freynaud : log the suite as failed ?  
            }
            _suiteResult.StopWatch();

            _finished = true;
            return _suiteResult;
        }

        private bool AfterSuiteWebTestMethods(ConcurrentQueue<TestMethodInstance> testMethodInstancesQueue)
        {
            //TODO Need to find a better way to build the test queue
            //if (testMethodInstancesQueue.Count > 1)
            //{
            //    throw new RunnerException("Please Ensure there is only 1 Before Suite WebTest Method");
            //}
            TestMethodInstance mi;
            testMethodInstancesQueue.TryDequeue(out mi);
            if (mi != null)
            {
                var result = _methodRunner.Run(mi);
                _suiteResult.Add(result);
            }
            return true;
        }


        private bool BeforeSuiteWebTestMethods(ConcurrentQueue<TestMethodInstance> testMethodInstancesQueue)
        {
            //if (testMethodInstancesQueue.Count > 1)
            //{
            //    throw new RunnerException("Please Ensure there is only 1 Before Suite WebTest Method");
            //}
            TestMethodInstance mi;
            testMethodInstancesQueue.TryDequeue(out mi);
            if (mi != null)
            {
                var result = _methodRunner.Run(mi);
                _suiteResult.Add(result);
            }
            return true;
        }

        private bool ConfigSuiteMethods(IEnumerable<MethodInfo> methodInfos)
        {
            foreach (var info in methodInfos)
            {
                var exploder = new MethodExploder(info, _config);
                var instance = exploder.Explode().First();
                var result = new TestResult(instance);
                try
                {
                    instance.Invoke();
                    result.Status = Status.Passed;
                    _suiteResult.Add(result);
                }
                catch (Exception e)
                {
                    result.Exception = e.InnerException;
                    result.Status = Status.Failed;
                    _suiteResult.Add(result);
                    return false;
                }
            }

            return true;

        }

        private void StartConsumer(ConcurrentQueue<TestMethodInstance> testMethodInstancesQueue)
        {
            Task.Factory.StartNew((() =>
            {
                while (!testMethodInstancesQueue.IsEmpty)
                {
                    TestMethodInstance mi;
                    testMethodInstancesQueue.TryDequeue(out mi);
                    if (mi != null)
                    {
                        Interlocked.Increment(ref _counter);
                        var result = _methodRunner.Run(mi);
                        _suiteResult.Add(result);
                        var left = Interlocked.Decrement(ref _counter);
                        if (left == 0 && testMethodInstancesQueue.IsEmpty)
                        {
                            _event.Set();
                        }
                    }
                }
            }));
        }

        public void GenerateReports()
        {
            if (!_finished)
            {
                throw new RunnerException("Cannot run reports if the tests are still running.");
            }
            // run the reports
            foreach (IReporter reporter in _reporters)
            {
                reporter.GenerateReport(_suiteResult);
            }
        }

        public enum ExitCode
        {
            Success = 0,
            TestsFailed = 1,
            TestSkipped = 2,
            UnknownError = 10
        }

        public int GetExitCode()
        {
            var exitCode = ExitCode.Success;
            if (_suiteResult.FailedTests.Count > 0)
            {
                exitCode = ExitCode.TestsFailed;
            }
            if (_suiteResult.SkippedTests.Count > 0)
            {
                exitCode = ExitCode.TestSkipped;
            }
            return (int)exitCode;
        }
    }
}
