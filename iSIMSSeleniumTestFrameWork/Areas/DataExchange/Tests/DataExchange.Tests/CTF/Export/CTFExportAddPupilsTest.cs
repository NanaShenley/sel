using DataExchange.Components.Common;
using DataExchange.POM.Components.Common;
using DataExchange.POM.Components.CTF.Export;
using DataExchange.POM.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using TestSettings;

namespace DataExchange.Tests.CTF.Export
{
    public class CtfExportAddPupilsTest
    {
        [Variant(Variant.EnglishStatePrimary)]
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome}, Groups = new[] { Constants.CTFExport, BugPriority.P2 })]
        public void CtfAddPupils()
        {
            // Arrange
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, true, "CTFExportV16");
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "CTF Export");

            var exportAddPupil = new CtfExportAddPupil();
            exportAddPupil.GenerateCtfExport();

            // Act
            exportAddPupil.PupilselectorAdd();

            Wait.WaitTillAllAjaxCallsComplete();

            // Assert
            Assert.IsTrue(exportAddPupil.HasNewColumnsAdded());
            Assert.IsTrue(exportAddPupil.HasRecords());
           
        }
    }
}
