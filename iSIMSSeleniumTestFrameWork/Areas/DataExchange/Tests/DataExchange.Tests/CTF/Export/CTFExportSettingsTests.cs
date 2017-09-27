using DataExchange.POM.Components.CTF.Export;
using DataExchange.POM.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using TestSettings;

namespace DataExchange.Tests.CTF.Export
{
    public class CTFExportSettingsTests
    {              
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Groups = new[] { "CTF" })]        
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void OpenExportSettingsScreen()
        {
            string code = "NAALLLL";
            string description = "National Assessment Agency";
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "CTF Export  Settings");

            var exportSettingsPage = new ExportSettingsDetailPage();
            bool section=exportSettingsPage.IsSectionExist();
            Assert.IsTrue(section);

            exportSettingsPage.AddButton();
            exportSettingsPage.CtfAlternativeDestinationGrid[0].Code = code;
            exportSettingsPage.CtfAlternativeDestinationGrid[0].Description = description;

            exportSettingsPage.SaveRecords();
        }
    }
}
