using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Admissions.Component;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;

namespace Admissions.BulkUpdate.Tests
{
    public class ApplicationBulkUpdateTests
    {
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "BulkUpdate", "Checks_BulkUpdate_MenuItems_For_SchoolAdministrator" })]
        public void Checks_BulkUpdate_MenuItems_For_SchoolAdministrator()
        {
            //Arrange
            var bulkUpdateNavigate = new AdmissionsBulkUpdateNavigation();
            bulkUpdateNavigate.NavigateToPupilBulkUpdateMenuPage(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);

            //Act
            IWebElement applicantApplicationStatusMenuItem;
            try
            {
                applicantApplicationStatusMenuItem = SeleniumHelper.Get(AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.ApplicantApplicationStatusMenuItem);
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format(@"Don't have enough permissions to browse to bulk update area or sub menus: {0}", exception.Message));
            }

            //Assert
            Assert.IsTrue(applicantApplicationStatusMenuItem.Text == "Application Status");
        }

        [WebDriverTest(Enabled = true,
            Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2","BulkUpdate", "Checks_BulkUpdate_MenuItems_For_AdmissionsOfficer" })]
        public void Checks_BulkUpdate_MenuItems_For_AdmissionsOfficer()
        {
            //Arrange
            var bulkUpdateNavigate = new AdmissionsBulkUpdateNavigation();
            bulkUpdateNavigate.NavigateToAdmissionsBulkUpdateMenuPage(SeleniumHelper.iSIMSUserType.AdmissionsOfficer, false);

            IWebElement applicantApplicationStatusMenuItem;
            //Act
            try
            {
                applicantApplicationStatusMenuItem = SeleniumHelper.Get(AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.ApplicantApplicationStatusMenuItem);
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
            Groups = new[] { "P2", "BulkUpdate", "Verify_BulkUpdate_Applicant_ParentalSalutation_And_Addressee_Menu_Display" })]
        public void Verify_BulkUpdate_Applicant_ParentalSalutation_And_Addressee_Menu_Display()
        {
            //Arrange
            var bulkUpdateNavigate = new AdmissionsBulkUpdateNavigation();
            bulkUpdateNavigate.NavigateToAdmissionsBulkUpdateMenuPage(SeleniumHelper.iSIMSUserType.AdmissionsOfficer, false);

            //Act
            IWebElement bulkMenuItem = SeleniumHelper.Get(AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.ApplicantSalutationAddresseeMenuItem);

            //Assert
            Assert.IsTrue(bulkMenuItem.Text == "Parental Salutation & Addressee");
        }
    }
}