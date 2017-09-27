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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using WebDriverRunner.internals;
using WebDriverRunner.results;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Selene.Support;
using Selene.ApiClient;


namespace WebDriverRunner.Listners
{
    public static class SeleneLock
    {
        public static readonly object WriterLock = new object();
    }
    internal class SeleneTestListener : TestlistenerBase
    {
        private readonly Dictionary<int, Stopwatch> timers = new Dictionary<int, Stopwatch>();
        private Stopwatch GetTimer()
        {

            int id = Thread.CurrentThread.ManagedThreadId;
            lock (timers)
            {
                if (timers.ContainsKey(id))
                {
                    return timers[id];
                }
                else
                {
                    Stopwatch timer = new Stopwatch();
                    timers.Add(id, timer);

                    return timer;
                }
            }
        }

        public static bool SeleneIsReachableSync(string seleneUrl)
        {
            return AsyncHelpers.RunSync<bool>(() => SeleneIsReachableAsync(seleneUrl));
        }

        public static async Task<bool> SeleneIsReachableAsync(string seleneUrl)
        {
            using (var client = new SeleneApiClient(seleneUrl))
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(SeleneApiEndpoints.Handshake, new { Hello = "ThisIsTheTestRunner" });

                return response.IsSuccessStatusCode;
            }
        }

        static void ReportToSeleneSync(TestMethodInstance instance, ITestResult result, string apiPath, TimeSpan timeTaken, Configuration config, bool reportException = false)
        {
            AsyncHelpers.RunSync(() => ReportToSeleneAsync(instance, result, apiPath, timeTaken, config, reportException));
        }

        static async Task ReportToSeleneAsync(TestMethodInstance instance, ITestResult result, string apiPath, TimeSpan timeTaken, Configuration config, bool reportException)
        {

            using (var client = new SeleneApiClient(config.SeleneApiUrl))
            {
                var browserObj = instance.GetMetadata("browser");
                var browser = browserObj != null ? browserObj.ToString() : string.Empty;

                TestInfo test = new TestInfo
                {
                    Assembly = instance.GetMethod().Module.Assembly.GetName().Name,
                    AssemblyRunId = config.AssemblyRunId,
                    AutomatedRunId = config.AutomatedRunId,
                    Team = config.Team,
                    Test = instance.GetMethod().Name,
                    Timestamp = DateTime.Now,
                    TimeTaken = timeTaken != null ? timeTaken : TimeSpan.FromMilliseconds(0),
                    Notification = apiPath.Split('/')[1],
                    Id = instance.TestId,
                    Browser = browser == "chrome" ? "Chrome" : browser == "internet explorer" ? "IE" : string.Empty
                };

                if (reportException && result.Exception != null)
                {
                    test.ExceptionName = result.Exception.GetType().Name;
                    test.ExceptionMessage = result.Exception.Message;
                    test.ExceptionSource = result.Exception.Source;
                }
                else
                {
                    test.ExceptionName = test.ExceptionMessage = test.ExceptionSource = string.Empty;
                }

                HttpResponseMessage response = await client.PostAsJsonAsync(apiPath, test);

                SeSugar.Environment.Logger.LogLine("Call to Selene {0} responded with {1} ({2}).", apiPath, response.StatusCode, (int)response.StatusCode);

            }
        }
        public override void OnTestStart(TestMethodInstance instance, ITestResult result)
        {
            var timer = GetTimer();
            timer.Reset();

            ReportToSeleneSync(instance, result, SeleneApiEndpoints.TestStarted, GetTimer().Elapsed, this.Configuration);

            timer.Start();
        }

        public override void OnTestFinished(TestMethodInstance instance, ITestResult result)
        {

        }

        public override void OnTestPassed(TestMethodInstance instance, ITestResult result)
        {
            GetTimer().Stop();
            ReportToSeleneSync(instance, result, SeleneApiEndpoints.TestPassed, GetTimer().Elapsed, this.Configuration);
        }

        public override void OnTestFailed(TestMethodInstance instance, ITestResult result)
        {
            GetTimer().Stop();
            ReportToSeleneSync(instance, result, SeleneApiEndpoints.TestFailed, GetTimer().Elapsed, this.Configuration, true);
        }

        public override void OnTestSkipped(TestMethodInstance instance, ITestResult result)
        {
            GetTimer().Stop();
            if (result.Exception != null)
            {
                if (result.Exception.GetType().Name == "InconclusiveException")
                {
                    ReportToSeleneSync(instance, result, SeleneApiEndpoints.TestInconclusive, GetTimer().Elapsed, this.Configuration, true);
                }
                else
                {
                    ReportToSeleneSync(instance, result, SeleneApiEndpoints.TestSkipped, GetTimer().Elapsed, this.Configuration, true);
                }
            }
            else
                ReportToSeleneSync(instance, result, SeleneApiEndpoints.TestSkipped, GetTimer().Elapsed, this.Configuration);
        }

        public SeleneTestListener(Configuration config)
            : base(config)
        {
        }

        private void WriteTestOutcome(TestMethodInstance instance, string message, ConsoleColor backgroundColour, ConsoleColor foregroundColour = ConsoleColor.Black, string additional = null)
        {
            lock (SeleneLock.WriterLock)
            {
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.BackgroundColor = backgroundColour;
                Console.ForegroundColor = foregroundColour;
                Console.Write(" {0} ", message);
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("{0} TEST:       {1}{0} ASSEMBLY:   {2}{0} TIME TAKEN: {3}", Environment.NewLine, instance, instance.GetMethod().Module.Assembly.GetName().Name, GetTimer().Elapsed);

                if (additional != null)
                    Console.WriteLine(additional);

                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine();
            }
        }
    }
    public static class AsyncHelpers
    {
        /// <summary>
        /// Execute's an async Task<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T> method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        /// <summary>
        /// Execute's an async Task<T> method which has a T return type synchronously
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            T ret = default(T);
            synch.Post(async _ =>
            {
                try
                {
                    ret = await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool done;
            public Exception InnerException { get; set; }
            readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
            readonly Queue<Tuple<SendOrPostCallback, object>> items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }
                workItemsWaiting.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (items)
                    {
                        if (items.Count > 0)
                        {
                            task = items.Dequeue();
                        }
                    }
                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null) // the method threw an exeption
                        {
                            throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                        }
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }
}
