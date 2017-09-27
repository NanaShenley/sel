using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SharedComponents.Helpers;
using SharedServices.Components.AdditionalColumn;
using SharedServices.Components.Common;
using TestSettings;
using WebDriverRunner.webdriver;

namespace SharedServices.Tests.AdditionalColumn
{
    public class AllocatePupilAdditionalColumnsTests
    {
        [WebDriverTest(Groups = new[] { Constants.AllocatePupilAdditionalColumn }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        public void CanOpenAdditionalColumnsDialog()
        {
            // Arrange
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils to Groups");
            var dialog = new AllocatePupilAdditionalColumnsDialog();

            // Act
            dialog.OpenAdditionalColumnDialog();

            // Assert
            WebContext.Screenshot();
        }
    }
}