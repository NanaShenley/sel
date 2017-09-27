using System;
using System.Threading;
using DataExchange.POM.Components.Common;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.CRUD;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.CTF.Import
{
    public class CtfImportAutoRefresh : AutoRefreshSeleniumComponents
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

        public CtfImportAutoRefresh()
        {
            SeleniumHelper.Login();
            Thread.Sleep(1000);

            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "CTF Import");
            Thread.Sleep(1000);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        /// <summary>
        /// method to import ctf file
        /// </summary>
        public void CtfImportFile()
        {
            ImportFileElement.Click();
            Detail.WaitForDetail();
            UploadFileFromSharepointElement.Click();

            SearchDocumentInSharepoint searchDoc = new SearchDocumentInSharepoint();
            searchDoc.ClickCtfSearchButton();
            searchDoc.SelectFirstRecord();
            searchDoc.ClickOkButton();

            //wait untill the file name gets populated
            Detail.WaitForDetail();
            WaitUntilValueGetsPopulated(DataExchangeElements.CommonElements.FileName);

            SaveElement.Click();
        }

        public static string AddDocumentToSharepoint(Guid schoolId)
        {
            return AddDocumentToSharepoint(schoolId, "School.CTFIn", "/TestDocuments/0406201_CTF_0109999_001.xml");
        }
    }
}
