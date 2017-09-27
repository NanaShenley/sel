using DataExchange.POM.Components.Common;
using DataExchange.POM.Components.CTF.Export;
using DataExchange.POM.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using TestSettings;
using DataExchange.Components.Common;

namespace DataExchange.Tests.CTF.Export.AutoRefresh
{
    public class CtfExportTests
    {
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { Constants.CTFExport, BugPriority.P2 })]
        public void CtfExportNotify()
        {
            // Arrange
            const string expected = "CTF Export Process";
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, true, "CTFExportV16");
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "CTF Export");

            var export = new CtfExportRefresh();
            export.GenerateCtfExport();

            // Act
            export.Pupilselector();
            Wait.WaitTillAllAjaxCallsComplete();

            export.NavigateToOtherScreen();

            // Assert
            export.WaitForProcessingOnOtherScreen();
            var actual = export.ReadAndReturnMessageText();
            Assert.AreEqual(expected, actual);
        }

        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { Constants.CTFExport, BugPriority.P2 })]
        public void CtfExportRefresh()
        {
            // Arrange
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, true, "CTFExportV16");
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "CTF Export");

            var export = new CtfExportRefresh();
            export.GenerateCtfExport();

            // Act
            export.Pupilselector();

            // Assert
            var message = export.CheckIsProcessing();
            Assert.AreEqual("File Processing", message);

            var text = export.CheckAfterRefresh();
            Assert.AreEqual("CTF Export", text);
        }
    }
}