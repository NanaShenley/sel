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
using WebDriverRunner.webdriver;

namespace WebDriverRunner.VisualStudioUnitTesting.BrowserFactory
{
    public class SeleniumGridManager
    {

        private Process _hubProcess;
        private Process _nodeProcess;
        private const string seleniumExistanceURL = "http://localhost:4444/selenium-server/";
        private const string seleniumHubShutDownEndpoint = "http://localhost:4444/lifecycle-manager/LifecycleServlet?action=shutdown";
        private const string seleniumNodeShutDownEndpoint = "http://localhost:5555/extra/LifecycleServlet?action=shutdown";

        /// <summary>
        /// Checks if selenium server is running locally
        /// </summary>
        private bool SeleniumRunning
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
            if (!SeleniumRunning)
            {
                StartSeleniumHubAndNode();
            }
        }


        public void CloseSeleniumGrid()
        {
            if (SeleniumRunning)
                StopSeleniumHubAndNode();
        }


        private string GetSolutionPath()
        {
            var directory = Directory.GetCurrentDirectory();
            var startDirectory = Directory.GetParent(directory).Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
            return startDirectory;
        }



        private string GetBatFilePath(string batFileName)
        {
            return GetSolutionPath() + $"\\WebDriverRunner\\SeleniumServer\\{batFileName}.bat";
        }

        private void StartSeleniumHubAndNode()
        {
            var hubPath = GetBatFilePath("SeleniumServerHub");
            var nodePath = GetBatFilePath("SeleniumServerNode");
            CreateAndWriteBatFile(hubPath, GetHubCommand("3.5.3"));
            CreateAndWriteBatFile(nodePath, GetNodeCommand());

            _hubProcess = Process.Start(hubPath);
            Thread.Sleep(TimeSpan.FromSeconds(2));

            _nodeProcess = Process.Start(nodePath);
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }


        private void StopSeleniumHubAndNode()
        {
            ShutdownSelenium(seleniumNodeShutDownEndpoint);
            ShutdownSelenium(seleniumHubShutDownEndpoint);
            KillTestProcesses("chrome");
        }

        private void ShutdownSelenium(string shutDownEndpoint) 
        {
            bool isShutDown = false;  
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(shutDownEndpoint);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                isShutDown = (response.StatusCode == HttpStatusCode.OK);
            }
            catch (Exception webEx)
            {
                isShutDown = false;
                Debug.WriteLine("Selenium not shutdown : " + webEx.Message);
            }
        }

        private void KillTestProcesses(string processName)
        {

            try
            {
                foreach (var process in HaltProcess(processName))
                process.Kill();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private Process[] HaltProcess(string localIisExpress)
        {
            return Process.GetProcessesByName(localIisExpress);
        }





        private void CreateAndWriteBatFile(string batFilePath, string command)
        {
            StreamWriter sw;
            if (!File.Exists(batFilePath)) sw = new StreamWriter(batFilePath);
            else
            {
                File.Delete(batFilePath);
                sw = new StreamWriter(batFilePath);
            }
            var seleniumServerPath = Path.GetDirectoryName(batFilePath);
            sw.WriteLine($"cd {seleniumServerPath}");
            sw.WriteLine(command);
            sw.Close();
            sw.Dispose();
        }




        private string GetHubCommand(string jarVersion)
        {
            return $"java -jar selenium-server-standalone-{jarVersion}.jar -role hub";
        }


        private string GetNodeCommand()
        {
            var packagePath = GetSolutionPath() + "\\packages\\";
            return $"java -Dwebdriver.ie.driver={packagePath}Selenium.WebDriver.IEDriver.3.6.0\\driver\\IEDriverServer.exe -Dwebdriver.chrome.driver={packagePath}Selenium.WebDriver.ChromeDriver.2.32.0\\driver\\win32\\chromedriver.exe -Dwebdriver.gecko.driver={packagePath}Selenium.WebDriver.GeckoDriver.Win64.0.19.0\\driver\\geckodriver.exe -jar selenium-server-standalone-3.5.3.jar -port 5555 -role node -hub http://localhost:4444/grid/register -servlet org.openqa.grid.web.servlet.LifecycleServlet -browser \"browserName=firefox,maxInstances=20,platform=WINDOWS,seleniumProtocol=WebDriver\" -browser \"browserName=internet explorer,version=11,platform=WINDOWS,maxInstances=20\" -browser \"browserName=chrome,version=ANY,maxInstances=20,platform=WINDOWS\"";
        }
    }
}
