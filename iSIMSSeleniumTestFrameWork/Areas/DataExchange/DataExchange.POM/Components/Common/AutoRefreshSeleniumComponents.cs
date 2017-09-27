using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using TestSettings;
using WebDriverRunner.webdriver;
using DataExchange.POM.Helper;

namespace DataExchange.POM.Components.Common
{
    public class AutoRefreshSeleniumComponents : BaseSeleniumComponents
    {
        /// <summary>
        /// wait for the generate button to be clickable
        /// </summary>
        public void WaitElementToBeClickable(By locator)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        /// <summary>
        /// wait untill the file name gets populated
        /// </summary>
        /// <param name="loc"></param>
        public void WaitUntilValueGetsPopulated(By loc)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(driver =>
            {
                try
                {
                    var findElement = driver.FindElement(loc);
                    return !string.IsNullOrEmpty(findElement.GetValue());
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// check if the file is getting processed
        /// </summary>
        public string CheckIsProcessing()
        {
            WaitUntilDisplayed(DataExchangeElements.CommonElements.ProcessingMessage);
            return SeleniumHelper.Get(DataExchangeElements.CommonElements.ProcessingMessage).Text;
        }

        /// <summary>
        /// verify the main screen is displayed after refresh
        /// </summary>
        /// <returns></returns>
        public string CheckAfterRefresh()
        {
            WaitUntilDisplayed(TimeSpan.FromMinutes(5d), Constants.MainScreen);
            WebContext.Screenshot();
            return SeleniumHelper.Get(Constants.MainScreen).Text;
        }

        /// <summary>
        /// Navigate to other screen
        /// </summary>
        public void ClickOnOtherScreen()
        {
            CheckIsProcessing();
            WaitElementToBeClickable(DataExchangeElements.QuickLinks.PupilRecord);
            SeleniumHelper.FindAndClick(DataExchangeElements.QuickLinks.PupilRecord);
        }

        /// <summary>
        /// Wait on other screen for the processing to completed 
        /// </summary>
        public void WaitForProcessingOnOtherScreen()
        {
            WaitUntilDisplayed(TimeSpan.FromMinutes(5d), DataExchangeElements.CommonElements.NotificationAlert);
        }

        /// <summary>
        /// Read the notification message header text
        /// </summary>
        /// <returns></returns>
        public string ReadAndReturnMessageText()
        {
            string message = WaitForAndGet(DataExchangeElements.CommonElements.NotificationAlert).FindChild(By.CssSelector("span[class=\"search-result h1-result\"]")).Text;
            WebContext.Screenshot();
            return message;
        }

        /// <summary>
        /// Add the document to sharepoint
        /// </summary>
        /// <param name="schoolId">SchoolId</param>
        /// <param name="propertyName">document store property name</param>
        /// <param name="filePath">file name with path</param>
        /// <returns></returns>
        public static string AddDocumentToSharepoint(Guid schoolId, string propertyName, string filePath)
        {
            var wsComponent = new ApplicationServerHelper(schoolId);

            var importPath = wsComponent.GetDocumentStoreUrl(propertyName, schoolId);

            string importFilePath = string.Concat(Directory.GetCurrentDirectory(), filePath);

            string fileName = Path.GetFileName(importFilePath);

            wsComponent.SaveFileToDocumentStore(Path.Combine(importPath, fileName), File.ReadAllBytes(importFilePath));

            return !string.IsNullOrEmpty(importPath) ? importPath : string.Empty;
        }
    }
}
