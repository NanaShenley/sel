using System;
using DataExchange.POM.Components.Common;
using DataExchange.POM.Components.LOP.Import;
using Selene.Support.Attributes;
using SeSugar.Data;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataExchange.Tests.LOP.Import.AutoRefresh
{
    public class LopImportTests
    {
        private string _documentPath;

        [WebDriverTest(Groups = new[] { Constants.LopImportAutoRefresh }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, Enabled = false)]
        public void LopImportTest()
        {
            string sql = "select ID from dbo.School where name = '" + TestDefaults.Default.SchoolName.Trim().Replace("'", "''") + "'";
            Guid schoolId = DataAccessHelpers.GetValue<Guid>(sql);
            _documentPath = LOPImportAutoRefresh.AddDocumentToSharepoint(schoolId);

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Levels of Progression File Import");

            LOPImportAutoRefresh lopImport = new LOPImportAutoRefresh();
            lopImport.LopImportFile();
            //Verify Processing Screen
            string message = lopImport.CheckIsProcessing();
            Assert.AreEqual(message, "File Processing");
            WebContext.Screenshot();
            //Verify Screen Reload
            string refreshMsg = lopImport.CheckAfterRefresh();
            Assert.AreEqual("Levels of Progression Import File", refreshMsg);
            WebContext.Screenshot();
        }

        [WebDriverTest(Groups = new[] { Constants.LopImportAutoRefresh }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, Enabled = false)]
        public void LopImportGenerateNotificationWhenOnOtherScreen()
        {
            string sql = "select ID from dbo.School where name = '" + TestDefaults.Default.SchoolName.Trim().Replace("'", "''") + "'";
            Guid schoolId = DataAccessHelpers.GetValue<Guid>(sql);
            _documentPath = LOPImportAutoRefresh.AddDocumentToSharepoint(schoolId);

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Levels of Progression File Import");

            LOPImportAutoRefresh lopImport = new LOPImportAutoRefresh();
            lopImport.LopImportFile();
            //Verify Processing Screen
            string message = lopImport.CheckIsProcessing();
            Assert.AreEqual(message, "File Processing");
            lopImport.ClickOnOtherScreen();
            lopImport.WaitForProcessingOnOtherScreen();
            string messageText = lopImport.ReadAndReturnMessageText();
            Assert.AreEqual(messageText, "LOP Import Process");
        }

    }
}
