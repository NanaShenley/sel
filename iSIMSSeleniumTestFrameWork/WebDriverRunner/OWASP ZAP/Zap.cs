using System;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Threading;

namespace WebDriverRunner.OWASP
{
    /// <summary>
    /// Zap proxy class
    /// </summary>
    public static class Zap
    {
        private const string zapFileName = "C:\\Program Files (x86)\\OWASP\\Zed Attack Proxy\\ZAP.exe";
        /// <summary>
        /// Checks whether ZAP is running
        /// </summary>
        /// <returns>TRUE if it is</returns>
        public static bool IsRunning(string proxy)
        {
            bool running = false;
            try
            {
                if (!string.IsNullOrEmpty(proxy))
                {
                    WebRequest checkRequest = WebRequest.Create("http://zap/HTML/core/view/version/");
                    checkRequest.Proxy = new WebProxy(proxy);
                    HttpWebResponse response = checkRequest.GetResponse() as HttpWebResponse;
                    running = response != null && response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unable to contact ZAP proxy: " + e.Message);
                running = false;
            }
            return running;
        }
        /// <summary>
        /// Starts the security scanner
        /// </summary>
        /// <returns>TRUE if successful</returns>
        public static bool StartSecurityScanning(string proxy, bool rethrowExceptions = false)
        {
            bool started = false;
            if (!string.IsNullOrEmpty(proxy))
            { 
                started = IsRunning(proxy);
                if (!started && File.Exists(zapFileName))
                {
                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo(zapFileName);
                        startInfo.WorkingDirectory = Path.GetDirectoryName(zapFileName);
                        startInfo.Arguments = "-daemon";
                        Process p = Process.Start(startInfo);
                        started = true;
                        // Wait a bit...
                        Thread.Sleep(5000);
                    }
                    catch (Exception e)
                    {
                        System.Console.Write("Unable to start security scanner. Exception: {0} {1}", e.Message);
                        if (e.InnerException != null)
                            System.Console.Write("\t{0}", e.InnerException.Message);
                        if (rethrowExceptions)
                            throw;
                    }
                }
            }
            return started;
        }
        /// <summary>
        /// Generates the ZAP passive scan report to a file
        /// </summary>
        /// <param name="outputReportFile"></param>
        /// <returns>TRUE if successful</returns>
        public static bool GenerateReport(string proxy, string outputReportFile, bool rethrowExceptions = false)
        {
            bool generated = false;
            if (!string.IsNullOrEmpty(proxy))
            {
                try
                {
                    if (!string.IsNullOrEmpty(outputReportFile))
                    {
                        WebRequest reportRequest = WebRequest.Create("http://zap/OTHER/core/other/htmlreport/");
                        reportRequest.Proxy = new WebProxy(proxy);
                        HttpWebResponse reportResponse = reportRequest.GetResponse() as HttpWebResponse;
                        if (reportResponse.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream outputFile = File.OpenWrite(outputReportFile))
                            {
                                using (Stream input = reportResponse.GetResponseStream())
                                {
                                    input.CopyTo(outputFile);
                                    generated = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    if (rethrowExceptions)
                        throw;
                    else
                    {
                        Debug.WriteLine("Unable to generate security scan report: " + e.Message);
                        if (e.InnerException != null)
                            Debug.WriteLine("\t{0}", e.InnerException.Message);
                    }
                }
            }
            return generated;
        }

        /// <summary>
        /// Stops the security scanner
        /// </summary>
        /// <returns>TRUE if successful</returns>
        public static bool StopSecurityScanning(string proxy, bool rethrowExceptions = false)
        {
            bool stopped = false;
            if (!string.IsNullOrEmpty(proxy))
            {
                try
                {
                    // Now stop the scan
                    WebRequest stopRequest = WebRequest.Create("http://zap/HTML/core/action/shutdown/");
                    stopRequest.Proxy = new WebProxy(proxy);
                    HttpWebResponse response = stopRequest.GetResponse() as HttpWebResponse;
                    stopped = response != null && response.StatusCode == HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    if (rethrowExceptions)
                        throw;
                    else
                    {
                        Debug.WriteLine("Unable to stop security scanner: " + e.Message);
                        if (e.InnerException != null)
                            Debug.WriteLine(e.InnerException.Message);
                    }
                }
            }
            return stopped;
        }
    }
}
