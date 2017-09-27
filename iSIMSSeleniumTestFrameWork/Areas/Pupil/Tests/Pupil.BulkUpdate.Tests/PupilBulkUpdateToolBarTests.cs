using NUnit.Framework;
using OpenQA.Selenium;
using Pupil.Components;
using Pupil.Components.Common;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SharedComponents.HomePages;
using System;
using System.Collections.Generic;
using POM.Helper;
using TestSettings;
using SeleniumHelper = SharedComponents.Helpers.SeleniumHelper;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;

namespace Pupil.BulkUpdate.Tests
{
    public class PupilBulkUpdateToolBarTests
    {
        private SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;

        #region BulkUpdatePupilDetails_SubMenu_Tests

        /// <summary>
        /// Test whether the tab label has changed on click of 'bulk update pupil details' menu item.
        /// </summary>
		[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateIdentifierColumns, PupilTestGroups.Priority.Priority2, "RSMB" })]
        public void TabUrl_ShouldChange_On_Return_To_BulkUpdatePupilDetailsTab_From_PupilRecordsTab()
        {
            //Arrange
            string expected = string.Format("/{0}/Pupils/BulkUpdatePupilBasicDetails/Details", TestDefaults.Default.Path);
            Checks_MaintainViewState_of_Bulkupdate_Pupil_Basic_Details();

            //Act
            var tabOpenbButton = SeleniumHelper.Get("tab_Bulk");
            var actual = tabOpenbButton.GetAttribute("data-ajax-url");

            //Assert
            Assert.AreEqual(expected, actual);
        }

        #endregion BulkUpdatePupilDetails_SubMenu_Tests

        #region Identifier Dialog

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateIdentifierColumns, PupilTestGroups.Priority.Priority2, "BU_SDICIG" })]
        public void ShouldDisplayIdentifierColumnsinGrid_OnClickOfPupilIdentifiersOKButton()
        {
            Select_Year_And_NavidateToBulkUpdateDetailScreen();

            SelectAllColumnsinIndentifierDialog();

            // Check the columns are added to the grid
            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.GridColumns.DateOfBirth);
            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.GridColumns.LegalName);
            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.GridColumns.Gender);

            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.GridColumns.AdmissionNumber);
            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.GridColumns.Class);
            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.GridColumns.DateOfAmission);
            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.GridColumns.YearGroup);
            Assert.IsTrue(true);
        }

        #endregion Identifier Dialog

        #region Common Stuffs

        private void Select_Year_And_NavidateToBulkUpdateDetailScreen()
        {
            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
            bulkUpdateNavigation.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);

            // Select Year 1 and Year 3 in the Search criteria
            var searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);
            Wait.WaitForAjaxReady(By.ClassName("locking_mask"));
            SeleniumHelper.FindCheckBoxAndClick(searchCriteria, "Year Group", new List<string> { "Year 1", "Year 3" });
            Wait.WaitForAjaxReady(By.ClassName("locking_mask"));

            // Click on the Search button
            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton, TimeSpan.FromSeconds(20));
            Wait.WaitForAjaxReady(By.ClassName("locking_mask"));
        }

        private void SelectAllColumnsinIndentifierDialog()
        {
            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierButton);
            Wait.WaitForAjaxReady(By.ClassName("locking_mask"));

            //Click on Personal details and registration details so that all the columns are included in the grid
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[webix_tm_id='Personal Details']:first-of-type + div.webix_tree_leaves  input[type='checkbox'][data-automation-id]")));

            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.PersonalDetails).Click();
            Wait.WaitForAjaxReady(By.ClassName("locking_mask"));
            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.RegistrationDetails).Click();
            Wait.WaitForAjaxReady(By.ClassName("locking_mask"));
            // Click on the ok button so that the columns are included in the grid
            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.OkButton);
            Wait.WaitForAjaxReady(By.ClassName("locking_mask"));
        }

        #endregion Common Stuffs

        private void Checks_MaintainViewState_of_Bulkupdate_Pupil_Basic_Details()
        {
            SeleniumHelper.Login(LoginAs);
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Bulk Update");
            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);
            Wait.WaitForAjaxReady(By.ClassName("locking_mask"));
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            Wait.WaitForAjaxReady(By.ClassName("locking_mask"));
            TabActions.ClickTabItem("tab_Bulk");
            Wait.WaitForAjaxReady(By.ClassName("locking_mask"));
        }
    }
}