using Selene.Support.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using TestSettings;
using WebDriverRunner.webdriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using OpenQA.Selenium;
using WebDriverRunner.VisualStudioUnitTesting.BrowserFactory;

namespace TestRunner
{
    /// <summary>
    /// Selenium support class for visual studio unit testing.
    /// This class contains the TestInitialize and TestCleanup implementations so that existing WebDriverTest methods
    /// can also be marked as [TestMethod] too and run from visual studio.
    /// </summary>
    public static class VSSeleniumTest
    {
//        private static IEnumerable<object[]> GetParamsFromDataProvider(object obj, MethodInfo method, TestMethodBaseAttribute attr)
//        {
//            // find the method.
//            var name = attr.DataProvider;
//            if (name == null)
//            {
//                throw new WebDriverRunner.internals.DataProviderException("expected a data provider for " + method);
//            }
//            var provider = obj.GetType().GetMethod(name);
//            if (provider == null)
//            {
//                throw new WebDriverRunner.internals.DataProviderException("No dataprovider method with name " + name);
//            }
//            return (List<object[]>)provider.Invoke(obj, null);
//        }
//
//
//
//        private static IEnumerable<object[]> FindParameters(object obj, MethodInfo method, TestMethodBaseAttribute attribute)
//        {
//            // get the method parameters
//            if (attribute.DataProvider != null)
//            {
//                return GetParamsFromDataProvider(obj, method, attribute);
//            }
//            return new List<object[]> { null };
//        }
        
        /// <summary>
        /// Initialises a unit test instance - typically called from TestInitialize, passing in the class instance and test context.
        /// </summary>
        /// <param name="instance">The instance of the test class</param>
        /// <param name="testContext">The test context</param>
        public static void Init(object instance, TestContext testContext)
        {
            var placeToExec = TestDefaults.Default.MethodOfExecution;
            IWebDriver driver = null;
            if (placeToExec.ToLower().Contains("local"))
            {
                SeleniumGridManager gridManager = new SeleniumGridManager();
                gridManager.InitialiseTestRun();
                driver = new LocalCaps(instance, testContext).RunOnCurrentMachine();
            }else if (placeToExec.ToLower().Contains("remote"))
            {
                driver = new RemoteCaps(instance, testContext).RunOnRemoteMachine();
            }
            else
            {
                throw new AmbiguousMatchException("The execution method is either local machine or remote machine");
            }
            driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Initialises a unit test instance - typically called from TestCleanup, passing in the test context.
        /// </summary>
        public static void Cleanup(TestContext testContext)
        {
            WebDriverContext context = WebContext.GetThreadLocalContext();
            context.Driver.Quit();
            context.Result.StopWatch();
            Debug.WriteLine("Test {0} completed. {1}", testContext.TestName, context.Result.ToString());
        }
    }
}
