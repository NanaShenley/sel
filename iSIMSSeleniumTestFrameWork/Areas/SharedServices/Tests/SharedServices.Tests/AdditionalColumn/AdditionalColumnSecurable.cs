using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.AdditionalColumn;
using SharedServices.Components.Common;
using TestSettings;

namespace SharedServices.Tests.AdditionalColumn
{
    public class AdditionalColumnSecurable : BaseSeleniumComponents
    {
        /// <summary>
        /// Test to verify the count of the additional columns on the Future Pupil screen
        /// </summary>
        [WebDriverTest(Groups = new[] { Constants.AdditionalColumnSecurables }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        public void FuturePupilAllAdditionalColumnCount()
        {
            // Arrange
            const int expected = 7;
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");  
            var allocateFuturePupil = new AllocateFuturePupil();

            // Act
            var actual = allocateFuturePupil.GetDialogAdditionalColumnCount();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Test to verify the count of the additional columns for class teacher user
        /// Obsolete - we are testing *functionality* not profiles
        /// </summary>
        [WebDriverTest(Enabled = false, Groups = new[] { Constants.AdditionalColumnSecurables }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        public void FuturePupilAdditionalColumnCountForClassTeacher()
        {
            // Arrange
            const int expected = 7;
            var allocateFuturePupil = new AllocateFuturePupil();
            allocateFuturePupil.SearchMenu("allocate future pupils");

            // Act
            var actual = allocateFuturePupil.GetDialogAdditionalColumnCount();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}