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
using System.Drawing;
using System.Security.Principal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using WebDriverRunner.internals;
using WebDriverRunner.results;
using WebDriverRunner.webdriver;

namespace WebDriverRunner.Listners
{
    public class TestlistenerBase : ITestListner
    {
        public Configuration Configuration { get; private set; }

        protected TestlistenerBase(Configuration config)
        {
            Configuration = config;
        }

        public virtual void OnTestStart(TestMethodInstance instance, ITestResult result)
        {
         
        }

        public virtual void OnTestFinished(TestMethodInstance instance, ITestResult result)
        {
         
        }

        public virtual void OnTestPassed(TestMethodInstance instance, ITestResult result)
        {
         
        }

        public virtual void OnTestFailed(TestMethodInstance instance, ITestResult result)
        {
         
        }

        public virtual void OnTestSkipped(TestMethodInstance instance, ITestResult result)
        {
         
        }

        public void SetUpAndStartBrowsers(TestMethodInstance instance, ITestResult result)
        {
            // Check the proxy settings
            Proxy useProxy = null;
            if (!string.IsNullOrEmpty(Configuration.ProxyOverride))
            {
                string proxy = Configuration.ProxyOverride;
                useProxy = new Proxy();
                useProxy.HttpProxy = proxy;
                useProxy.FtpProxy = proxy;
                useProxy.SslProxy = proxy;
                useProxy.IsAutoDetect = false;
                useProxy.Kind = ProxyKind.Manual;
            }
            // Now set the browser capabilities/options
            DesiredCapabilities cap;
            var browser = instance.GetMetadata(WebContext.BrowserKey) as string;
            string userDataDir = instance.Attr.UserDataDir;
            if (browser.Equals("chrome"))
            {
                ChromeOptions options = new ChromeOptions();
                options.SetLoggingPreference(LogType.Browser, LogLevel.All);
                options.AddArgument("--incognito");
                options.AddArgument("--explicitly-allowed-ports=95");
                options.AddArgument("--lang=en-GB");
                options.AddArgument("--disable-popup-blocking");
                options.AddArgument("--disable-extensions");
                options.AddArgument("--enable-javascript");
                options.AddArgument("--disable-web-security");
                options.AddArgument("--disable-plugins");
                options.AddArgument("--start-maximized");
                options.AddArgument("--enable-logging");
                options.AddArgument("--allow-running-insecure-content");
                options.AddArgument("--javascript-harmony");
                if (userDataDir != null)
                {
                    options.AddArguments("--user-data-dir=" + userDataDir);
                }
                if (useProxy != null)
                    options.Proxy = useProxy;
                cap = options.ToCapabilities() as DesiredCapabilities;
            }
            else
            {
                // Please think carefully before changeing this
                //https://code.google.com/p/selenium/wiki/InternetExplorerDriver
                InternetExplorerOptions IEOptions = new InternetExplorerOptions();
                IEOptions.UsePerProcessProxy = true;
                if (useProxy != null)
                    IEOptions.Proxy = useProxy;
                cap = IEOptions.ToCapabilities() as DesiredCapabilities;
                //cap = DesiredCapabilities.InternetExplorer();
                cap.SetCapability("nativeEvents", true);
                cap.SetCapability("ignoreZoomSetting", true);
            }


            var domainuser = WindowsIdentity.GetCurrent() == null
                ? null
                : WindowsIdentity.GetCurrent().Name;

            char[] delimiterChars = { '\\' };
            var words = domainuser.Split(delimiterChars);
            var user = words[1];
            cap.SetCapability("user", user);
            cap.SetCapability("method", instance.ToString());
            IWebDriver driver = new RemoteWebDriver(Configuration.Hub, cap, Configuration.BrowserResponseTimeout);
            {
                WebDriverContext webDriverContext = new WebDriverContext(driver, instance, result,
                    Configuration.Output);
                WebContext.Set(webDriverContext);
                instance.SetMetadata(WebContext.BrowserKey, browser);
            }

            driver.Manage().Window.Maximize();
        }

        public void CloseBrowser(IWebDriver webDriver)
        {
            ((IJavaScriptExecutor)webDriver).ExecuteScript("window.onbeforeunload = null;");
            webDriver.Quit();
        }


        public void Screenshot()
        {
            try
                {
                    WebContext.Screenshot();
                }
                catch (Exception e)
                {
                    TestResultReporter.Log("Couldn't take screenshot " + e.Message);
                }
            
        }

    }
}
