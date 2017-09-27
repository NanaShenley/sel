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
using System.IO;
using System.Text;
using WebDriverRunner;
using WebDriverRunner.internals;
using WebDriverRunner.Listners;
using WebDriverRunner.OWASP;
using Environment = SeSugar.Environment;

namespace TestRunner
{
    class Program
    {
        /*         
         --dll="../../../iSIMSAddStudent/bin/debug/iSIMSSearchStudent.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="C:\WIP\SIMS 8 UK\Src\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll" --maxThreads=15 --output="c:/report"
         */
        static int Main(string[] args)
        {
            Environment.Initialise(new WebDriverRunner.webdriver.WebContext(), new TestRunner.SeSugar.SeSettings(), new TestRunner.SeSugar.SeLogger());

            Configuration config = null;
            Runner runner = null;

            try
            {
                config = Configuration.Create(args);

                Environment.Settings.AutomatedRunId = config.AutomatedRunId;
                Environment.Settings.SeleneApiUrl = config.SeleneApiUrl;

                bool scanWasAlreadyRunning = config.SecurityScan && Zap.IsRunning(config.ProxyOverride);
                if (!scanWasAlreadyRunning)
                    startSecurityScan(config);

                // Now run the tests
                runner = new Runner(config);
                runner.LoadTests();
                runner.RunTests();
                runner.GenerateReports();

                stopSecurityScanAndGenerateReport(config, scanWasAlreadyRunning);

                // Return the exit code
                return runner.GetExitCode();
            }
            catch (System.Exception e)
            {
                lock (ConsoleLock.WriterLock)
                {
                    if (config != null)
                    {
                        WriteErrorToConsole(e);
                        WriteErrorToFile(config, e);
                    }
                    else
                    {
                        WriteErrorToConsole(e);
                        WriteErrorToConsole(
                            new System.Exception("There was an issue with the configuration parameters. Please check them."));
                    }
                }
                return (int)WebDriverRunner.Runner.ExitCode.UnknownError;
            }
            finally
            {
                if (config != null && config.PauseOnExit)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
            }
        }

        private static void WriteErrorToConsole(System.Exception e)
        {
            System.Console.WriteLine();
            System.Console.ForegroundColor = System.ConsoleColor.Black;
            System.Console.BackgroundColor = System.ConsoleColor.Red;
            System.Console.Write("{0} {1}", e.Message, e.Source);
            System.Console.ResetColor();
            System.Console.WriteLine();
        }

        private static void WriteErrorToFile(Configuration config, System.Exception e)
        {
            string path = Path.Combine(config.Output, "TestRunnerLogs");
            string filePath = Path.Combine(path, "TestRunner_Errors.txt");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            StringBuilder errorLogEntry = new StringBuilder();

            errorLogEntry.AppendFormat("DATE/TIME : {0} - {1}", System.DateTime.Now.ToShortDateString(), System.DateTime.Now.ToLongTimeString());
            errorLogEntry.AppendLine();
            errorLogEntry.AppendFormat("EXCEPTION : {0}.{1}", e.GetType().Namespace, e.GetType().Name);
            errorLogEntry.AppendLine();
            errorLogEntry.AppendFormat("MESSAGE   : {0}", e.Message);
            errorLogEntry.AppendLine();
            errorLogEntry.AppendFormat("TRACE {0}{1}", System.Environment.NewLine, e.StackTrace);
            errorLogEntry.AppendLine();
            if (e.InnerException != null)
            {
                errorLogEntry.AppendFormat("INNEREXCEPTION : {0}.{1}", e.InnerException.GetType().Namespace, e.InnerException.GetType().Name);
                errorLogEntry.AppendLine();
                errorLogEntry.AppendFormat("INNER MESSAGE   : {0}", e.InnerException.Message);
                errorLogEntry.AppendLine();
            }
            errorLogEntry.AppendLine();
            errorLogEntry.AppendLine("****************************************************************************************************************************");
            errorLogEntry.AppendLine();

            File.AppendAllText(filePath, errorLogEntry.ToString());
        }
        private static void startSecurityScan(Configuration config)
        {
            if (config.SecurityScan)
            {
                try
                {
                    Console.WriteLine("Attempting to start security scanning...");
                    // Start the scanner
                    Zap.StartSecurityScanning(config.ProxyOverride, true);
                    Console.WriteLine("Started security scanning...");
                }
                catch (Exception e)
                {
                    WriteErrorToConsole(e);
                    WriteErrorToFile(config, e);
                }
                finally
                {
                    // Set back the proxy value in case we changed it and it didn't work
                    if (!Zap.IsRunning(config.ProxyOverride))
                    {
                        Console.WriteLine("Unable to verify if ZAP proxy was started. Test proxy is reset.");
                        // Reset the test proxy
                        config.ProxyOverride = "";
                    }
                }
            }
            else if (!string.IsNullOrEmpty(config.SecurityScanReport))
            {
                // We're generating a report, so assume ZAP is already running
                // Set back the proxy value in case we changed it and it didn't work
                if (!Zap.IsRunning(config.ProxyOverride))
                {
                    Console.WriteLine("ZAP not detected, but scan report requested. Test proxy has been reset.");
                    // Reset the test proxy
                    config.ProxyOverride = "";
                }
            }
        }
        private static void stopSecurityScanAndGenerateReport(Configuration config, bool scanWasAlreadyRunning)
        {
            Console.WriteLine("Generating security report and shutting scanner down");
            if (!string.IsNullOrEmpty(config.SecurityScanReport))
            {
                try
                {
                    // If we want a security scan report, generate it
                    Zap.GenerateReport(config.ProxyOverride, config.SecurityScanReport);
                }
                catch (Exception e)
                {
                    WriteErrorToConsole(e);
                    WriteErrorToFile(config, e);
                }
            }
            // Stop security scanner if it wasn't already running
            if (config.SecurityScan && !scanWasAlreadyRunning)
            {
                try
                {
                    Zap.StopSecurityScanning(config.ProxyOverride, true);
                }
                catch (Exception e)
                {
                    WriteErrorToConsole(e);
                    WriteErrorToFile(config, e);
                }
            }
        }
    }
}
