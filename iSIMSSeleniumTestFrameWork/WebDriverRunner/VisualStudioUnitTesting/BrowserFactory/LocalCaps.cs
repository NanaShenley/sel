using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Selene.Support.Attributes;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.results;
using WebDriverRunner.webdriver;
using Configuration = TestSettings.Configuration;
using Environment = SeSugar.Environment;

namespace WebDriverRunner.VisualStudioUnitTesting.BrowserFactory
{
    public class LocalCaps
    {
        private static internals.Configuration configuration = new internals.Configuration();
        private object instance;
        private TestContext testContext;

        public LocalCaps(object instance, TestContext testContext)
        {
            this.instance = instance;
            this.testContext = testContext;
        }

        public IWebDriver RunOnCurrentMachine()
        {
            // TODO: Take this setting from TestContext
            Configuration.SystemUnderTest = TestDefaults.Default.DefaultSystemUnderTest;// Configuration.lab_five;

            Environment.Initialise(new WebContext(), new SeSettings(), new SeLogger());
            string className = testContext.FullyQualifiedTestClassName;
            string testName = testContext.TestName;
            // NOTE: You might have to use AppDomain.CurrentDomain.GetAssemblies() and then call GetTypes on each assembly if this code
            //       resides in a baseclass in another assembly. 
            Type currentlyRunningClassType = instance.GetType().Assembly.GetTypes().FirstOrDefault(f => f.FullName == className);
            MethodInfo currentlyRunningMethod = currentlyRunningClassType.GetMethod(testName);
            // Replace WorkItemAttribute with whatever your attribute is called... 
            IEnumerable<WebDriverTestAttribute> webDriverAttributes = currentlyRunningMethod.GetCustomAttributes(typeof(WebDriverTestAttribute), true) as IEnumerable<WebDriverTestAttribute>;
            WebDriverTestAttribute webDriverAttr = webDriverAttributes.FirstOrDefault();
            string browserName = webDriverAttr.Browsers[0];
            if (String.IsNullOrEmpty(browserName))
            {
                if (webDriverAttr is ChromeUiTestAttribute)
                    browserName = BrowserDefaults.Chrome;
                else if (webDriverAttr is IeUiTestAttribute)
                    browserName = BrowserDefaults.Ie;
            }
            Assert.IsNotNull(browserName, "No browser name");

            // Set the user
            string domainuser = WindowsIdentity.GetCurrent() == null
                ? null
                : WindowsIdentity.GetCurrent().Name;

            char[] delimiterChars = { '\\' };
            var words = domainuser.Split(delimiterChars);
            var user = words[1];
            DesiredCapabilities caps = null;

            // Check the proxy settings
            Proxy useProxy = null;
            if (!String.IsNullOrEmpty(TestDefaults.Default.Proxy))
            {
                useProxy = new Proxy();
                string proxy = TestDefaults.Default.Proxy;
                useProxy.HttpProxy = proxy;
                useProxy.FtpProxy = proxy;
                useProxy.SslProxy = proxy;
                useProxy.IsAutoDetect = false;
                useProxy.Kind = ProxyKind.Manual;
            }
            
            BrowserManager manager = new BrowserManager(webDriverAttr, useProxy);
            caps = manager.ChooseRequiredBrowser(caps, browserName);
            // Set common capabilities
            caps.SetCapability("user", user);
            caps.SetCapability("method", testName);

            

            // Now sort out the driver
//            IEnumerable<object[]> param = FindParameters(instance, currentlyRunningMethod, webDriverAttr);
//
//            // do the param fit in the method ?
//            IEnumerable<object[]> enumerable = param as IList<object[]> ?? param.ToList();




            // Create the test method instance
            TestMethodInstance testMethodInstance = new TestMethodInstance(webDriverAttr,
                new Dictionary<string, object>(), null, instance, currentlyRunningMethod);

            // And a result
            ITestResult result = new UnitTestResult(testMethodInstance);

            // Now run the remote web driver based on all the parameters
            IWebDriver driver = new RemoteWebDriver(configuration.Hub, caps, configuration.BrowserResponseTimeout);
            {
                WebDriverContext webDriverContext = new WebDriverContext(driver, testMethodInstance, result,
                    configuration.Output);
                WebContext.Set(webDriverContext);
                testMethodInstance.SetMetadata(WebContext.BrowserKey, browserName);
            }
            return driver;
        }
    }
}
