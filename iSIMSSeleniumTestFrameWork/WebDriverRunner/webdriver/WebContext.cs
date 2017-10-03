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
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using WebDriverRunner.results;
using SeSugar.Interfaces;

namespace WebDriverRunner.webdriver
{
    public class WebContext : IWebContext
    {
        public static readonly ThreadLocal<WebDriverContext> ThreadLocal = new ThreadLocal<WebDriverContext>();



        public static string BrowserKey = "browser";

        public static void Set(WebDriverContext webDriverContext)
        {
            ThreadLocal.Value = webDriverContext;
        }

        public static WebDriverContext GetThreadLocalContext()
        {
            return ThreadLocal.Value;
        }

        public static IWebDriver WebDriver
        {
            get
            {
                if (ThreadLocal.Value != null && ThreadLocal.Value.Driver != null)
                {
                    return ThreadLocal.Value.Driver;

                }
                else
                {
                    throw new Exception("Couldn't create the driver.");
                }

            }
        }

        IWebDriver IWebContext.WebDriver
        {
            get { return WebDriver; }
        }

        public static string Browser
        {
            get { return (string)ThreadLocal.Value.Instance.GetMetadata(BrowserKey); }
            private set { }
        }

        public static void Screenshot(string relative)
        {
            var shot = ThreadLocal.Value.Driver.TakeScreenshot();
            var path = GetThreadLocalContext().Output +@"\"+ relative;
            shot.SaveAsFile(path, ImageFormat.Png);

            var url = "unknown";
            var title = "unknown";

            try
            {
                url = WebContext.WebDriver.Url;
                title = WebContext.WebDriver.Title;
            }
            catch (Exception)
            {
                // ignore;
            }
            TestResultReporter.Log(new ScreenshotLog(title, url, relative));
        }




        public static void Screenshot()
        {
            var folder = GetThreadLocalContext().Output + @"\screenshots";
            Directory.CreateDirectory(folder);
            var relative = @"screenshots\" + Guid.NewGuid() + ".png";

            Screenshot(relative);
        }

        public static void Log(Log log)
        {
            TestResultReporter.Log(log);
        }
    }
}
