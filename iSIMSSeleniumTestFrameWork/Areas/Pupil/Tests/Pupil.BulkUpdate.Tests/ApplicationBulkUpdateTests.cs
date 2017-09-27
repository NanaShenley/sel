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
    public class ApplicationBulkUpdateTests
    {
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdatePermissions, PupilTestGroups.Priority.Priority2 })]
        public void Checks_BulkUpdate_MenuItems_For_SchoolAdministrator()
        {
            //Arrange
            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
            bulkUpdateNavigate.NavigateToPupilBulkUpdateMenuPage(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);

            //Act
            IWebElement applicantApplicationStatusMenuItem;
            try
            {
                applicantApplicationStatusMenuItem = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.MenuItems.ApplicantApplicationStatusMenuItem);
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format(@"Don't have enough permissions to browse to bulk update area or sub menus: {0}", exception.Message));
            }

            //Assert
            Assert.IsTrue(applicantApplicationStatusMenuItem.Text == "Application Status");
        }

        [WebDriverTest(Enabled = true,
            Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdatePermissions, PupilTestGroups.Priority.Priority2 })]
        public void Checks_BulkUpdate_MenuItems_For_AdmissionsOfficer()
        {
            //Arrange
            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
            bulkUpdateNavigate.NavigateToAdmissionsBulkUpdateMenuPage(SeleniumHelper.iSIMSUserType.AdmissionsOfficer, false);

            IWebElement applicantApplicationStatusMenuItem;
            //Act
            try
            {
                applicantApplicationStatusMenuItem = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.MenuItems.ApplicantApplicationStatusMenuItem);
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format(@"Don't have enough permissions to browse to bulk update area or sub menus: {0}", exception.Message));
            }

            //Assert
            string expected = "Application Status";
            string actual = applicantApplicationStatusMenuItem.Text;

            Assert.AreEqual(expected, actual);
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateMenu, PupilTestGroups.PupilBulkUpdate.Page, PupilTestGroups.Priority.Priority2 })]
        public void Verify_BulkUpdate_Applicant_ParentalSalutation_And_Addressee_Menu_Display()
        {
            //Arrange
            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
            bulkUpdateNavigate.NavigateToAdmissionsBulkUpdateMenuPage();

            //Act
            IWebElement bulkMenuItem = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.MenuItems.ApplicantSalutationAddresseeMenuItem);

            //Assert
            Assert.IsTrue(bulkMenuItem.Text == "Parental Salutation & Addressee");
        }
    }
}