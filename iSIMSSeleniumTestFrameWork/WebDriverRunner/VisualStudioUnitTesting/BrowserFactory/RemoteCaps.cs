using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
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
    public class RemoteCaps
    {

        private static internals.Configuration configuration = new internals.Configuration();
        private object instance;
        private TestContext testContext;

        public RemoteCaps(object instance, TestContext testContext)
        {
            this.instance = instance;
            this.testContext = testContext;
        }



        public IWebDriver RunOnRemoteMachine()
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
                //                else if (webDriverAttr is )
                //                    browserName = BrowserDefaults.Firefox;
                //                else if (webDriverAttr is )
                //                    browserName = BrowserDefaults.Firefox;
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

            BrowserManager manager = new BrowserManager(webDriverAttr);
            Proxy proxy = manager.CheckAndSetUpProxy();
            caps = manager.ChooseRequiredBrowser(caps, browserName, proxy);

            // Set common capabilities
            caps.SetCapability("user", user);
            caps.SetCapability("method", testName);
            //Set BrowserStack Details
            caps.SetCapability("os", BrowserStack.Default.OsName); //eg. "Windows"
            caps.SetCapability("os_version", BrowserStack.Default.OsVersion); // eg. "7"
            caps.SetCapability("browser", BrowserStack.Default.BrowserName);
            caps.SetCapability("browser_version", BrowserStack.Default.BrowserVersion);
            caps.SetCapability("project", BrowserStack.Default.ProjectName);
            caps.SetCapability("resolution", BrowserStack.Default.ScreenResolution); //1280x1024
            caps.SetCapability("browserstack.user", BrowserStack.Default.BrowserStackUser);
            caps.SetCapability("browserstack.key", BrowserStack.Default.BrowserStackKey);
            caps.SetCapability("browserstack.debug", "true");
            caps.SetCapability("build", "EdwinSmokeTest");
            caps = this.SetExtraBrowserStackCaps(caps, browserName);
            // Create the test method instance
            TestMethodInstance testMethodInstance = new TestMethodInstance(webDriverAttr,
                new Dictionary<string, object>(), null, instance, currentlyRunningMethod);
            // And a result
            ITestResult result = new UnitTestResult(testMethodInstance);
           
            var stackUrl = new Uri(BrowserStack.Default.BrowserStackHub);
            IWebDriver driver = new RemoteWebDriver(stackUrl, caps, configuration.BrowserResponseTimeout);
            {
                WebDriverContext webDriverContext = new WebDriverContext(driver, testMethodInstance, result, configuration.Output);
                WebContext.Set(webDriverContext);
                testMethodInstance.SetMetadata(WebContext.BrowserKey, browserName);
            }
            return driver;
        }


        private DesiredCapabilities SetExtraBrowserStackCaps(DesiredCapabilities caps, string browserName)
        {
            if (browserName.ToLower().Contains("ie"))
            {
                caps.SetCapability("browserstack.ie.enablePopups", "true");
                caps.SetCapability("browserstack.ie.noFlash", "true");
            }else if (browserName.ToLower().Contains("firefox"))
            {
                FirefoxProfile firefoxProfile = new FirefoxProfile();
                firefoxProfile.SetPreference("plugin.state.flash", 0);
                caps.SetCapability(FirefoxDriver.ProfileCapabilityName, firefoxProfile.ToBase64String());
            }
            else if (browserName.ToLower().Contains("safari"))
            {
                caps.SetCapability("browserstack.safari.enablePopups", "true");
            }
            caps.SetCapability("browserstack.video", "true");
            return caps;
        }
    }
}

