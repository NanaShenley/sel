using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebDriverRunner.VisualStudioUnitTesting.BrowserFactory
{
    public class SeleniumGridManager
    {


        private const string seleniumExistanceURL = "http://localhost:4444/selenium-server/driver/?cmd=getLogMessages";
        private const string seleniumShutdownURL = "http://localhost:4444/selenium-server/driver/?cmd=shutDownSeleniumServer";
        /// <summary>
        /// Checks if selenium server is running locally
        /// </summary>
        private static bool SeleniumRunning
        {
            get
            {
                bool seleniumDetected = false;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(seleniumExistanceURL);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    seleniumDetected = (response.StatusCode == HttpStatusCode.OK);
                }
                catch (Exception webEx)
                {
                    seleniumDetected = false;
                    Debug.WriteLine("Selenium not detected: " + webEx.Message);
                }
                return seleniumDetected;
            }
        }
        /// <summary>
        /// Initialises a test run.
        /// </summary>
        public void InitialiseTestRun()
        {
            Debug.WriteLine("Test run starting. Current directory = " + Directory.GetCurrentDirectory());
            bool seleniumDetected = SeleniumRunning;
            // If it's not running already
            if (!seleniumDetected)
            {
                StartSeleniumHubAndNode();
            }
        }



        private static string GetSolutionPath()
        {
            var directory = Directory.GetCurrentDirectory();
            var startDirectory = Directory.GetParent(directory).Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
            return startDirectory;
        }



        private string GetBatFilePath(string batFileName)
        {
            var yourApplicationPath = GetSolutionPath() + $"\\WebDriverRunner\\SeleniumServer\\{batFileName}.bat";
            return yourApplicationPath;
        }

        private void StartSeleniumHubAndNode()
        {
            var hubPath = GetBatFilePath("SeleniumServerHub");
            var nodePath = GetBatFilePath("SeleniumServerNode");
            CreateAndWriteBatFile(nodePath);
                        Process batProcess = Process.Start(nodePath);

            //var process = new Process();
            //process.StartInfo.FileName = hubPath;
            //process.StartInfo.UseShellExecute = false;
            //process.StartInfo.WorkingDirectory = Path.GetDirectoryName(hubPath);
            //process.StartInfo.RedirectStandardInput = true;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.RedirectStandardError = true;
            //process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            //process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            //process.EnableRaisingEvents = true;
            //Process.Start(process.StartInfo);
            ////process.Start();
            //Thread.Sleep(TimeSpan.FromSeconds(2));
            //Process nodeProcess = Process.Start(nodePath);
            //Thread.Sleep(TimeSpan.FromSeconds(2));
        }



        public string GetNodeCommand()
        {
            var packagePath  = GetSolutionPath() + "\\packages\\";
            return $"java -Dwebdriver.ie.driver={packagePath}Selenium.WebDriver.IEDriver.3.6.0\\driver\\IEDriverServer.exe -Dwebdriver.chrome.driver={packagePath}Selenium.WebDriver.ChromeDriver.2.32.0.0\\driver\\chromedriver.exe -Dwebdriver.gecko.driver={packagePath}Selenium.WebDriver.GeckoDriver.Win64.0.19.0\\driver\\geckodriver.exe -jar selenium-server-standalone-3.5.3.jar -port 5555 -role node -debug -hub http://localhost:4444/grid/register -browser \"browserName=firefox,maxInstances=20,platform=WINDOWS,seleniumProtocol=WebDriver\" -browser \"browserName=internet explorer,version=11,platform=WINDOWS,maxInstances=20\" -browser \"browserName=chrome,version=ANY,maxInstances=20,platform=WINDOWS\"";
        }


        private  void CreateAndWriteBatFile(string nodeBatFilePath)
        {
            StreamWriter sw;
            if (!File.Exists(nodeBatFilePath)) sw = new StreamWriter(nodeBatFilePath);
            else
            {
                File.Delete(nodeBatFilePath);
                sw = new StreamWriter(nodeBatFilePath);
            }
            sw.WriteLine(GetNodeCommand());
            sw.Close();
            sw.Dispose();
        }
    }
}
