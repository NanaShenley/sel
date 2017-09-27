using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Selene.Support.Attributes;
using TestSettings;

namespace WebDriverRunner.VisualStudioUnitTesting.BrowserFactory
{
    public class BrowserManager
    {
        private WebDriverTestAttribute webDriverAttr;
        private Proxy useProxy;

        public BrowserManager(WebDriverTestAttribute webDriverAttr, Proxy useProxy)
        {
            this.webDriverAttr = webDriverAttr;
            this.useProxy = useProxy;
        }


        public DesiredCapabilities ChooseRequiredBrowser(DesiredCapabilities caps, string browserName)
        {

            switch (browserName)
            {
                case BrowserDefaults.Chrome:
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
                    if (webDriverAttr.UserDataDir != null)
                        options.AddArguments("--user-data-dir=" + webDriverAttr.UserDataDir);
                    if (useProxy != null)
                        options.Proxy = useProxy;
                    caps = options.ToCapabilities() as DesiredCapabilities;
                    break;
                case BrowserDefaults.Ie:
                    InternetExplorerOptions ieOptions = new InternetExplorerOptions();
                    ieOptions.UsePerProcessProxy = true;
                    ieOptions.EnsureCleanSession = true;
                    ieOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    ieOptions.IgnoreZoomLevel = true;
                    ieOptions.RequireWindowFocus = true;
                    ieOptions.EnableNativeEvents = true;
                    ieOptions.EnableFullPageScreenshot = true;
                    ieOptions.ForceCreateProcessApi = true;
                    ieOptions.BrowserCommandLineArguments = "-private";
                    if (useProxy != null)
                        ieOptions.Proxy = useProxy;
                    caps = ieOptions.ToCapabilities() as DesiredCapabilities;
                    caps.SetCapability("nativeEvents", true);
                    caps.SetCapability("ignoreZoomSetting", true);
                    caps.SetCapability("acceptSslCerts", "true");
                    caps.SetCapability(CapabilityType.AcceptSslCertificates, true);
                    caps.SetCapability(CapabilityType.SupportsFindingByCss, true);
                    caps.SetCapability(CapabilityType.HandlesAlerts, true);
                    caps.SetCapability(CapabilityType.TakesScreenshot, true);
                    caps.SetCapability(CapabilityType.SupportsLocationContext, true);
                    caps.SetCapability(CapabilityType.IsJavaScriptEnabled, true);
                    break;
                case BrowserDefaults.Firefox:
                    caps = DesiredCapabilities.Firefox();
                    caps.SetCapability("marionette", true);
                    break;
                default:
                    throw new Exception("Unsupported browser: " + browserName);
            }
            return caps;
        }



    }
}
