using System;
using System.Drawing;
using System.Security.Principal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using WebDriverRunner.internals;
using WebDriverRunner.results;

namespace WebDriverRunner.webdriver
{
    public class BeforesuiteWebTestListner : TestListener
    {
        public BeforesuiteWebTestListner(Configuration config) : base(config)
        {
        }

        public override void OnTestStart(TestMethodInstance instance, ITestResult result)
        {
            if (instance.Attr.GetType() == typeof(BeforeSuiteWebTestAttribute))
            {

                DesiredCapabilities cap;
                var browser = instance.GetMetadata(WebContext.BrowserKey) as string;
                string userDataDir = instance.Attr.UserDataDir;
                if (browser.Equals("chrome"))
                {
                    var options = new ChromeOptions();
                    options.AddArgument("--explicitly-allowed-ports=95");
                    options.AddArgument("--lang=en-gb");
                    if (userDataDir != null)
                    {
                        options.AddArguments("--user-data-dir=" + userDataDir);
                    }
                    cap = options.ToCapabilities() as DesiredCapabilities;
                }
                else
                {
                    cap = DesiredCapabilities.InternetExplorer();
                    cap.SetCapability("nativeEvents", false);
                }


                var domainuser = WindowsIdentity.GetCurrent() == null
                    ? null
                    : WindowsIdentity.GetCurrent().Name;

                char[] delimiterChars = { '\\' };
                var words = domainuser.Split(delimiterChars);
                var user = words[1];
                cap.SetCapability("user", user);
                cap.SetCapability("method", instance.ToString());
                IWebDriver driver = new RemoteWebDriver(Configuration.Hub, cap);
                if (driver != null)
                {
                    driver.Manage().Window.Size = new Size(1280, 1024);
                    var webDriverContext = new WebDriverContext(driver, instance, result,
                        Configuration.Output);
                    WebContext.Set(webDriverContext);
                    instance.SetMetadata(WebContext.BrowserKey, browser);
                }
            }

        }

        public override void OnTestFinished(TestMethodInstance instance, ITestResult result)
        {
            if (instance.Attr.GetType() == typeof(BeforeSuiteWebTestAttribute))
            {
                (WebContext.WebDriver as IJavaScriptExecutor).ExecuteScript("window.onbeforeunload = null;");
                WebContext.WebDriver.Quit();
            }

        }

        public override void OnTestPassed(TestMethodInstance instance, ITestResult result)
        {

        }

        public override void OnTestFailed(TestMethodInstance instance, ITestResult result)
        {
            if (instance.Attr.GetType() == typeof(BeforeSuiteWebTestAttribute))
            {
                WebContext.Screenshot();
            }
        }



        public override void OnTestSkipped(TestMethodInstance instance, ITestResult result)
        {

        }

        private void Screenshot(TestMethodInstance instance)
        {
            if (instance.Attr.GetType() == typeof(BeforeSuiteWebTestAttribute))
            {
                try
                {
                    WebContext.Screenshot();
                }
                catch (Exception e)
                {
                    Reporter.Log("Couldn't take screenshot " + e.Message);
                }
            }
        }
    }
}