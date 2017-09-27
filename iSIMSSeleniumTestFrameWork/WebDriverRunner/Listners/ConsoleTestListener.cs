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
using System.Text;
using System.Threading;
using WebDriverRunner.internals;
using WebDriverRunner.results;

namespace WebDriverRunner.Listners
{
    public static class ConsoleLock
    {
        public static readonly object WriterLock = new object();
    }
    internal class ConsoleTestListener : TestlistenerBase
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

        public override void OnTestStart(TestMethodInstance instance, ITestResult result)
        {
            lock (ConsoleLock.WriterLock)
            {
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" STARTED ");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("{0} TEST:       {1}{0} ASSEMBLY:   {2}", Environment.NewLine, instance, instance.GetMethod().Module.Assembly.GetName().Name);
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine();

            }

            var timer = GetTimer();
            timer.Reset();
            timer.Start();
        }

        public override void OnTestFinished(TestMethodInstance instance, ITestResult result)
        {

        }

        public override void OnTestPassed(TestMethodInstance instance, ITestResult result)
        {
            GetTimer().Stop();
            var md = instance.GetMetadata();
            WriteTestOutcome(instance, "PASSED", ConsoleColor.Green);
        }

        public override void OnTestFailed(TestMethodInstance instance, ITestResult result)
        {
            GetTimer().Stop();

            if (result.Exception != null)
            {
                StringBuilder exceptionText = new StringBuilder();
                exceptionText.AppendFormat("{1} EXCEPTION:  {0}", result.Exception.GetType().Name, Environment.NewLine);
                if (result.Exception.Message != null)
                {
                    exceptionText.AppendFormat("{1} MESSAGE:    {0}", result.Exception.Message, Environment.NewLine);
                }
                if (result.Exception.Source != null)
                {
                    exceptionText.AppendFormat("{1} SOURCE:     {0}", result.Exception.Source, Environment.NewLine);
                }
                WriteTestOutcome(instance, "FAILED", ConsoleColor.Red, additional: exceptionText.ToString());
            }
            else
                WriteTestOutcome(instance, "FAILED", ConsoleColor.Red);
        }

        public override void OnTestSkipped(TestMethodInstance instance, ITestResult result)
        {
            GetTimer().Stop();
            if (result.Exception != null)
            {
                StringBuilder exceptionText = new StringBuilder();
                exceptionText.AppendFormat("{1} EXCEPTION:  {0}", result.Exception.GetType().Name, Environment.NewLine);
                if (result.Exception.Message != null)
                {
                    exceptionText.AppendFormat("{1} MESSAGE:    {0}", result.Exception.Message, Environment.NewLine);
                }
                if (result.Exception.Source != null)
                {
                    exceptionText.AppendFormat("{1} SOURCE:     {0}", result.Exception.Source, Environment.NewLine);
                }

                if (result.Exception.GetType().Name == "InconclusiveException")
                {
                    WriteTestOutcome(instance, "INCONCLUSIVE", ConsoleColor.Yellow, additional: exceptionText.ToString());
                }
                else
                {
                    WriteTestOutcome(instance, "SKIPPED", ConsoleColor.DarkMagenta, additional: exceptionText.ToString());
                }
            }
            else
                WriteTestOutcome(instance, "SKIPPED", ConsoleColor.DarkMagenta);
        }

        public ConsoleTestListener(Configuration config)
            : base(config)
        {
        }

        private void WriteTestOutcome(TestMethodInstance instance, string message, ConsoleColor backgroundColour, ConsoleColor foregroundColour = ConsoleColor.Black, string additional = null)
        {
            lock (ConsoleLock.WriterLock)
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
}
