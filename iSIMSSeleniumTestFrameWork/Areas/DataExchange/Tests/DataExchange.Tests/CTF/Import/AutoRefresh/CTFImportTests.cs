using System;
using DataExchange.POM.Components.Common;
using DataExchange.POM.Components.CTF.Import;
using Selene.Support.Attributes;
using SeSugar.Data;
using TestSettings;
using WebDriverRunner.webdriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataExchange.Tests.CTF.Import.AutoRefresh
{
    public class CtfImportTests
    {
        private string _documentPath;

        [WebDriverTest(Groups = new[] { Constants.CTFImportAutoRefresh }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Enabled = false)]
        public void CtfImportTest()
        {
            string sql = "select ID from dbo.School where name = '" + TestDefaults.Default.SchoolName.Trim().Replace("'", "''") + "'";
            Guid schoolId = DataAccessHelpers.GetValue<Guid>(sql);
            _documentPath = CtfImportAutoRefresh.AddDocumentToSharepoint(schoolId);

            //login and import ctf doc
            CtfImportAutoRefresh ctfImport = new CtfImportAutoRefresh();
            ctfImport.CtfImportFile();
            string message = ctfImport.CheckIsProcessing();
            Assert.AreEqual(message, "File Processing");
            WebContext.Screenshot();

            string refreshMsg = ctfImport.CheckAfterRefresh();
            Assert.AreEqual("Import CTF File", refreshMsg);
            WebContext.Screenshot();
        }

        [WebDriverTest(Groups = new[] { Constants.CTFImportAutoRefresh }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Enabled = false)]
        public void CtfImportGenerateNotificationWhenOnOtherScreen()
        {
            string sql = "select ID from dbo.School where name = '" + TestDefaults.Default.SchoolName.Trim().Replace("'", "''") + "'";
            Guid schoolId = DataAccessHelpers.GetValue<Guid>(sql);
            _documentPath = CtfImportAutoRefresh.AddDocumentToSharepoint(schoolId);

            CtfImportAutoRefresh ctfImport = new CtfImportAutoRefresh();
            ctfImport.CtfImportFile();
            string message = ctfImport.CheckIsProcessing();
            Assert.AreEqual(message, "File Processing");
            ctfImport.ClickOnOtherScreen();
            ctfImport.WaitForProcessingOnOtherScreen();
            string messageText = ctfImport.ReadAndReturnMessageText();
            Assert.AreEqual(messageText, "CTF Import Process");
            WebContext.Screenshot();
        }
    }
}
