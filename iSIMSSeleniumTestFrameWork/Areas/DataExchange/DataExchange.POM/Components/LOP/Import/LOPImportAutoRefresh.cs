using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using DataExchange.POM.Components.Common;

namespace DataExchange.POM.Components.LOP.Import
{
    public class LOPImportAutoRefresh : AutoRefreshSeleniumComponents
    {
        //import_file_button
        private const string ImportFileButton = "import_file_button";
        private const string UploadFileFromSharepointButton = "upload_file_from_sharepoint_button";
        private const string SaveButton = "well_know_action_save";

        public IWebElement ImportFileElement
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(ImportFileButton));
                return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID(ImportFileButton));
            }
        }

        //upload_file_from_sharepoint_button
        public IWebElement UploadFileFromSharepointElement
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(UploadFileFromSharepointButton));
                return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID(UploadFileFromSharepointButton));
            }
        }

        public IWebElement SaveElement
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(SaveButton));
                return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID(SaveButton));
            }
        }

        public LOPImportAutoRefresh()
        {
            Thread.Sleep(1000);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        /// <summary>
        /// method to import the LOP file
        /// </summary>
        public void LopImportFile()
        {
            //click the import button
            ImportFileElement.Click();
            Detail.WaitForDetail();

            //upload the file from sharepoint
            UploadFileFromSharepointElement.Click();

            SearchDocumentInSharepoint searchDoc = new SearchDocumentInSharepoint();
            searchDoc.ClickLopSearchButton();
            searchDoc.SelectFirstRecord();
            searchDoc.ClickOkButton();

            //wait untill the file name gets populated
            Detail.WaitForDetail();
            WaitUntilValueGetsPopulated(DataExchangeElements.CommonElements.FileName);

            SaveElement.Click();
            ////search for the lop file
            //WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.Import.UploadLOPSearchButton);
            ////wait for the search button to be enabled after searching
            //WaitUntilEnabled(DataExchangeElements.Import.SearchBtnEnabled);
            ////select the first record
            //WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.Import.FirstRecordSelect);
            ////click the Ok button
            //WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.Import.UploadLOPOkButton);
            ////wait untill the file name gets populated
            //WaitUntilValueGetsPopulated(DataExchangeElements.CommonElements.FileName);
            ////generate the import file
            //WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CommonElements.SaveButton);
        }

        public static string AddDocumentToSharepoint(Guid schoolId)
        {
            return AddDocumentToSharepoint(schoolId, "School.LoPIn", @"\TestDocuments\R1019999131.xml");
        }
    }
}
