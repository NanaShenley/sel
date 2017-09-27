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
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using WebDriverRunner.Listners;
using WebDriverRunner.results;
using WebDriverRunner.webdriver;

namespace WebDriverRunner.internals
{
    class MethodInstanceRunner
    {
        private readonly List<ITestListner> _listeners;

        public MethodInstanceRunner(List<ITestListner> listeners)
        {
            _listeners = listeners;
        }

        private ITestResult RunOnce(TestMethodInstance instance)
        {
            ITestResult result = new TestResult(instance);
            TestResultReporter.Set(result);

            // the instance couldn't be constructed properly, and cannot be run.
            if (instance.RunnerException != null)
            {
                result.Exception = instance.RunnerException;
                result.Status = Status.Skipped;
                return result;
            }

            try
            {
                // TODO freynaud check parameters for missmatch before running beforeMethod.
                foreach (var listener in _listeners)
                {
                    listener.OnTestStart(instance, result);
                }
            }
            catch (Exception e)
            {
                result.Exception = e.InnerException;
                result.Status = Status.Skipped;
                foreach (var listener in _listeners)
                {
                    listener.OnTestSkipped(instance, result);
                }
                result.StopWatch();
                TestResultReporter.Clear();
                return result;
            }

            try
            {
                //instance.Invoke();
                InvokeWithTimeout(instance, result);
                result.Status = Status.Passed;

                foreach (var listener in _listeners)
                {
                    listener.OnTestPassed(instance, result);
                }
            }
            catch (Exception e)
            {
                //TestResultReporter.Log(e.Message);
                // reflection + another thread. Needs to go 2 level down.
                // except when it times out
                result.Exception = e.InnerException == null ? e : e.InnerException.InnerException;
                if (e.InnerException != null && e.InnerException.InnerException != null && e.InnerException.InnerException.GetType().Name == "InconclusiveException")
                {
                    result.Status = Status.Skipped;
                }
                else
                {
                    result.Status = Status.Failed;
                }
                foreach (var listener in _listeners)
                {
                    if (result.Status == Status.Skipped)
                    {
                        try
                        {
                            listener.OnTestSkipped(instance, result);
                        }
                        catch (Exception tle)
                        {
                            result.Log("WARNING. A OnTestSkipped() listener skipped with the following message :" + tle.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            listener.OnTestFailed(instance, result);
                        }
                        catch (Exception tle)
                        {
                            result.Log("WARNING. A OnTestFailed() listener failed with the following error :" + tle.Message);
                        }
                    }
                }
            }
            finally
            {


                // TODO freynaud the order needs to be reversed. First listener executed last makes more sense.
                foreach (var listener in _listeners)
                {
                    try
                    {
                        listener.OnTestFinished(instance, result);

                    }
                    catch (Exception e)
                    {
                        Runner.Log("OnTestFinished ERROR " + e);
                    }
                }

            }
            result.StopWatch();
            TestResultReporter.Clear();
            return result;
        }

        private void InvokeWithTimeout(TestMethodInstance instance, ITestResult result)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var timeOut = instance.Attr.TimeoutSeconds * 1000;

            var context = WebContext.GetThreadLocalContext();
            var task = Task.Factory.StartNew(() =>
            {
                WebContext.Set(context);
                TestResultReporter.Set(result);
                instance.Invoke();
            }, token);

            if (!task.Wait(timeOut, token))
            {
                throw new WebDriverTimeoutException("test timed out after " + instance.Attr.TimeoutSeconds + " sec");
            }
        }

        public ITestResult Run(TestMethodInstance instance)
        {
            var result = RunOnce(instance);
            while (RetryAnalyser(result))
            {
                Runner.Log("RETRYING");
                var newResult = RunOnce(instance);
                newResult.AddRetriedResult(result);
                result = newResult;
            }
            return result;
        }

        private Boolean RetryAnalyser(ITestResult result)
        {
            return false;
            //return result.Status != Status.Passed && result.RetriedResults.Count < 3;
        }
    }

}
