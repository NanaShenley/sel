using NUnit.Framework;
using OpenQA.Selenium;
using Pupil.Components;
using Pupil.Components.Common;
using SharedComponents.Helpers;
using TestSettings;
using Selene.Support.Attributes;

namespace Pupil.BulkUpdate.Tests
{
    public class BulkUpdateMenuPageTests
    {
        [WebDriverTest(Enabled = true, 
            Browsers = new[] { BrowserDefaults.Chrome }, 
            Groups = new[] { PupilTestGroups.PupilBulkUpdate.Page, PupilTestGroups.Priority.Priority2 })]
        public void Verify_BulkUpdate_BasicDetails_Menu_Display()
        {
            //Arrange
            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
            bulkUpdateNavigate.NavigateToPupilBulkUpdateMenuPage();

            //Act
            IWebElement bulkMenuItem = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);

            //Assert
            Assert.AreEqual(bulkMenuItem.Text, "Basic Details");
        }

        [WebDriverTest(Enabled = true, 
            Browsers = new[] { BrowserDefaults.Chrome }, 
            Groups = new[] { PupilTestGroups.PupilBulkUpdate.Page, PupilTestGroups.Priority.Priority2 })]
        public void Verify_BulkUpdate_BasicDetails_SearchCriteria_Display()
        {
            //Arrange
            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
            bulkUpdateNavigate.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);

            //Act
            var searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);

            //Assert
            Assert.IsTrue(searchCriteria.Displayed, "Search Criteria displayed");
        }

       [WebDriverTest(Enabled = true, 
            Browsers = new[] { BrowserDefaults.Chrome }, 
            Groups = new[] { PupilTestGroups.PupilBulkUpdate.Page, PupilTestGroups.Priority.Priority2 })]
		public void Verify_BulkUpdate_Pupil_ParentalSalutation_And_Addressee_Menu_Display()
		{
            //Arrange
            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
			bulkUpdateNavigate.NavigateToPupilBulkUpdateMenuPage();

            //Act
			IWebElement bulkMenuItem = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilSalutationAddresseeMenuItem);

            //Assert
			Assert.IsTrue(bulkMenuItem.Text == "Parental Salutation & Addressee");
        }

          [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, 
            Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateMenu,
                PupilTestGroups.PupilBulkUpdate.Page,
                PupilTestGroups.Priority.Priority2 })]
        public void Can_Navigate_To_BulkUpdate_Pupil_Consents_SubMenu_As_SchoolAdministrator()
        {
            //Arrange
            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
            bulkUpdateNavigate.NavigateToPupilBulkUpdateMenuPage(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Act
            IWebElement bulkMenuItem = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilConsentsMenuItem);

            //Assert
            Assert.IsTrue(bulkMenuItem.Text == "Consents");
        }    
    }
}