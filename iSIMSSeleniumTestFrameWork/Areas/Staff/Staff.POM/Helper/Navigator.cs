using WebDriverRunner.webdriver;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Staff.POM.Helper
{
    public class Navigator
    {
        public static void Open(string url)
        {
            WebContext.WebDriver.Url = url;
            IeCertificateCheck();
        }


        public static void CloseBrowser()
        {
            IWebDriver _webDriver = WebContext.WebDriver;
            _webDriver.Quit();
        }

        public static void IeCertificateCheck()
        {
            if ("internet explorer".Equals(WebContext.Browser))
            {
                if (WebContext.WebDriver.Title.Equals("Certificate Error: Navigation Blocked"))
                {
                    WebContext.WebDriver.Url = "javascript:document.getElementById('overridelink').click();";
                }
            }
        }
    }
}
