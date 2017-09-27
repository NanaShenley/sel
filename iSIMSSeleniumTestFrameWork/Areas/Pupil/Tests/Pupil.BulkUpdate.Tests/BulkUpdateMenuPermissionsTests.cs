using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Pupil.Components;
using Pupil.Components.Common;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;

namespace Pupil.BulkUpdate.Tests
{
    public class BulkUpdateMenuPermissionsTests
    {
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdatePermissions, PupilTestGroups.Priority.Priority2 })]
        public void Checks_BulkUpdate_MenuItems_For_SchoolAdministrator()
        {
            //Arrange
            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
            bulkUpdateNavigate.NavigateToPupilBulkUpdateMenuPage(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);

            //Act
            IWebElement pupilBasicDetailsMenuItem;
            IWebElement pupilConsentsMenuItem;
            try
            {
                pupilBasicDetailsMenuItem = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);
                pupilConsentsMenuItem = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilConsentsMenuItem);
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format(@"Don't have enough permissions to browse to bulk update area or sub menus: {0}", exception.Message));
            }

            //Assert
            Assert.IsTrue(pupilBasicDetailsMenuItem.Text == "Basic Details");
            Assert.IsTrue(pupilConsentsMenuItem.Text == "Consents");
        }

    }
}