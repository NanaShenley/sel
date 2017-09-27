using DataExchange.POM.Components.CBA;
using DataExchange.POM.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SharedComponents.Helpers;
using TestSettings;

namespace DataExchange.Tests.CBA.AutoRefresh
{
    public class CbaExportAutoRefresh
    {
        /// <summary>
        /// Obsolete - refresh tested using CTF Export
        /// </summary>
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { Constants.CBAAutoRefresh })]
        public void CbaExportTest()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "CBA Export");

            var cbaExport = new CbaExportRefresh();
            cbaExport.GenerateCbaExport();

            //Verify Processing Screen
            string message = cbaExport.CheckIsProcessing();
            Assert.AreEqual(message, "File Processing");
            //Verify Screen Reload
            string text = cbaExport.CheckAfterRefresh();
            Assert.AreEqual("CBA Export", text);
        }

        /// <summary>
        /// Obsolete - refresh tested using CTF Export
        /// </summary>
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { Constants.CBAAutoRefresh })]
        public void CbaExportGenerateNotificationWhenOnOtherScreen()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "CBA Export");

            var cbaExport = new CbaExportRefresh();
            cbaExport.GenerateCbaExport();

            cbaExport.ClickOnOtherScreen();
            cbaExport.WaitForProcessingOnOtherScreen();
            string messageText = cbaExport.ReadAndReturnMessageText();
            Assert.AreEqual(messageText, "CBA Export Process");
        }
    }
}
