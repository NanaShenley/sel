using OpenQA.Selenium.Support.UI;
using Selene.ApiClient;
using SeSugar.Automation;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Environment = SeSugar.Environment;

using Selene.Support.Attributes;

namespace BatchInitialise
{ 
    public class VariantAttributeTests
    {
        //[ChromeUiTest]
        [Variant(Variant.EnglishStatePrimary)]
        public void England_Pri_Only()
        {
            //This will run against just the English State Primary variant

            Thread.Sleep(10000);
        }

        //[ChromeUiTest]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void Ni_Pri_Only()
        {
            //This will run against just the Northern Ireland State Primary variant

            Thread.Sleep(10000);
        }

        //[ChromeUiTest]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        public void England_Pri_and_Welsh_Pri()
        {
            //This will run against the below variants:
            // - English State Primary 
            // - Welsh State Primary 

            Thread.Sleep(10000);
        }

        //[ChromeUiTest]
        [Variant(Variant.EnglishStatePrimary| Variant.WelshStatePrimary | Variant.IndependentPrimary)]
        public void England_Pri_and_Welsh_Pri_and_Independant_Pri()
        {
            //This will run against the below variants:
            // - English State Primary 
            // - Welsh State Primary 
            // - Independent Primary 

            Thread.Sleep(10000);
        }

        //[ChromeUiTest]
        [Variant(Variant.AllPrimary)]
        public void All_Primary()
        {
            //This will run against the below variants:
            // - English State Primary 
            // - Welsh State Primary 
            // - Northern Ireland State Primary 
            // - Independent Primary 

            Thread.Sleep(10000);
        }

        //[ChromeUiTest]
        public void No_variant_specified_should_default_to_NiStPri()
        {
            //This will run against just the Northern Ireland State Primary variant, even though there was no
            //[Variant] attribute as tests were originally written with this intent.

            Thread.Sleep(10000);
        }
    }



    public class Initialise
    {
        [ChromeUiTest]
        public void Build_number_registration()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            var taskMenuLocator = SimsBy.AutomationId("task_menu");
            var versionLocator = SimsBy.AutomationId("Shell_Brand_Popup_Version");

            WebDriverWait wait = new WebDriverWait(Environment.WebContext.WebDriver, Environment.Settings.ElementRetrievalTimeout);

            AutomationSugar.WaitFor(taskMenuLocator);
            AutomationSugar.ClickOn(taskMenuLocator);

            AutomationSugar.WaitFor(versionLocator);

            wait.Until(x => x.FindElement(versionLocator).Text != string.Empty);

            var version = Environment.WebContext.WebDriver.FindElement(versionLocator).Text.Trim();
            var automatedRunId = Environment.Settings.AutomatedRunId;

            if (Environment.Settings.SeleneApiUrl != string.Empty)
                ReportToSeleneSync(new BuildVersionInfo { AutomatedRunId = automatedRunId, SutBuildNo = version });

        }

        static void ReportToSeleneSync(BuildVersionInfo version)
        {
            AsyncHelpers.RunSync(() => ReportToSeleneAsync(version));
        }

        static async Task ReportToSeleneAsync(BuildVersionInfo version)
        {
            using (var client = new SeleneApiClient(Environment.Settings.SeleneApiUrl))
            {

                HttpResponseMessage response = await client.PostAsJsonAsync("testregistration/buildversion", version);
            }
        }

        public class BuildVersionInfo
        {
            public Guid AutomatedRunId { get; set; }
            public string SutBuildNo { get; set; }
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
