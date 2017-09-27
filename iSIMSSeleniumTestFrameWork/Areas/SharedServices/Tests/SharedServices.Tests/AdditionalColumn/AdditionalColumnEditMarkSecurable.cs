using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedComponents.Helpers;
using SharedServices.Components.AdditionalColumn;
using SharedServices.Components.Common;
using TestSettings;
using SeSugar.Automation;
using Selene.Support.Attributes;

namespace SharedServices.Tests.AdditionalColumn
{
    public class AdditionalColumnEditMarkSecurable
    {
        /// <summary>
        /// Test to verify the count of the additional columns for class teacher user 
        /// Amit - Updated the expected result to 12 as we have not incorporated pre-requisite to restrict permission on one of the column.
        /// Obsolete - covered already by the Attendance team
        /// </summary>
        [WebDriverTest(Groups = new[] { Constants.TechServices, Constants.EditMarkSecurable }, Enabled = false, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        public void AdditionalColumnCountForClassTeacher()
        {
            // Arrange
            const int expected = 12;
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Take Register");

            var editMark = new AdditionalColumnEditMark();

            // Act
            var actual = editMark.GetDialogAdditionalColumnCount();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}