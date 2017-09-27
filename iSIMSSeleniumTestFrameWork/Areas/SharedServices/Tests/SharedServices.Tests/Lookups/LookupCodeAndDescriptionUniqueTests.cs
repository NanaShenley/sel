using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using SharedComponents.Popups;
using SharedServices.Components.Common;
using SharedServices.Components.PageObjects;
using TestSettings;
using SeSugar.Data;
using Selene.Support.Attributes;

namespace SharedServices.Tests.Lookups
{
    public class LookupCodeAndDescriptionUniqueTests : BaseSeleniumComponents
    {
        private const string DeleteRecord = "delete from ExclusionStatusChangeReason where Code = '{0}'";

        #region School Administrator Profile Tests

        [WebDriverTest(Groups = new[] { TestGroups.LookupCodeAndDescriptionUnique }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyCodeDuplicationForSchoolAdmin()
        {
            var rnd = new Random();
            var randomNumber = rnd.Next(0, 10);
            var code = string.Format("XX{0}", randomNumber);
            var description = string.Format("Test{0}", randomNumber);

            // Remove any old data
            DataAccessHelpers.Execute(string.Format(DeleteRecord, "XX%"));
            
            const string resourceProviderQuery = @"select * from resourceProvider where Code='deni' and tenantID=1019999";
            var resourceProvider = DataAccessHelpers.GetValue<Guid>(resourceProviderQuery);

            try
            {
                var insertQuery =
                    string.Format(
                        @"insert into exclusionstatuschangereason (Code, Description, IsVisible, ResourceProvider, TenantID) values ('{0}', 'Test', '1', '{1}', {2})",
                        code, resourceProvider, TestDefaults.Default.TenantId);
                DataAccessHelpers.Execute(insertQuery);

                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                AutomationSugar.NavigateMenu("Lookups", "Pupil Conduct", "Suspension Status Change Reason");

                var page = AddLookupRecord(code, description);
                Detail.WaitForDetail();

                // Verify popup message appears
                var popupDialog = new ConfirmationPopupDialogPage();
                popupDialog.ClickOkButton();

                // Verify save success message appears.
                Assert.IsNotNull(page.GetSuccessMessage());
            }
            finally
            {
                DataAccessHelpers.Execute(string.Format(DeleteRecord, code));
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.LookupCodeAndDescriptionUnique }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifySameResourceProviderDuplicationNotPermittedSchoolAdmin()
        {
            var rnd = new Random();
            var randomNumber = rnd.Next(20, 30);
            var code = string.Format("YY{0}", randomNumber);
            var description = string.Format("Description{0}", randomNumber);

            // Remove any records added from previous tests
            DataAccessHelpers.Execute(string.Format(DeleteRecord, "YY%"));

            try
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                AutomationSugar.NavigateMenu("Lookups", "Pupil Conduct", "Suspension Status Change Reason");

                // Add an initial record
                var page1 = AddLookupRecord(code, description);
                Detail.WaitForDetail();
                Assert.IsNotNull(page1.GetSuccessMessage());

                // Now add the same record again and check an error is shown
                var page2 = AddLookupRecord(code, description);
                Detail.WaitForDetail();
                Assert.IsNotNull(page2.GetErrorMessage());
            }
            finally
            {
                // Delete the added records
                DataAccessHelpers.Execute(string.Format(DeleteRecord, code));
            }
        }

        /// <summary>
        /// Adds an exclusion status chnage reason lookup record
        /// </summary>
        /// <param name="code">The status code to add</param>
        /// <param name="description">The random number to append to the code</param>
        /// <returns></returns>
        private static LookupPageObject AddLookupRecord(string code, string description)
        {
            // create page object
            var lookupPage = new LookupPageObject();

            // Do a random search
            lookupPage.ClickSearch();

            Detail.WaitForDetail();

            // Add a new lookup
            lookupPage.ClickCreateButton("create_service_ExclusionStatusChangeReason");

            // Hvae to put this in for Chrome only, otherwise the grid rows doesn't pick up the new "Create" row
            Detail.WaitForDetail();

            // Get last row and set values accordindgly
            var gridRows = SeleniumHelper.GetGridRows(By.CssSelector("table[data-maintenance-grid-id='LookupsWithProviderGrid1']"));

            var lastRow = gridRows.LastOrDefault();
            Assert.IsNotNull(lastRow);

            lookupPage.SetElementValue(lastRow, "input[name*='Code']", code);
            lookupPage.SetElementValue(lastRow, "input[name*='Description']", description);

            // Save the value
            lookupPage.ClickSaveButton();
            return lookupPage;
        }

        /// <summary>
        /// Adds an exclusion status chnage reason lookup record
        /// </summary>
        /// <param name="code">The status code to add</param>
        /// <param name="randomNumber">The random number to append to the code</param>
        /// <param name="messageToGet">The message name to return</param>
        /// <returns></returns>
        private static IWebElement AddLookupRecord(string code, int randomNumber, string messageToGet)
        {
            // create page object
            var lookupPage = new LookupPageObject();

            // Do a random search
            lookupPage.ClickSearch();

            Detail.WaitForDetail();

            // Add a new lookup
            lookupPage.ClickCreateButton("create_service_ExclusionStatusChangeReason");

            // Hvae to put this in for Chrome only, otherwise the grid rows doesn't pick up the new "Create" row
            Detail.WaitForDetail();

            // Get last row and set values accordindgly
            var gridRows = SeleniumHelper.GetGridRows(By.CssSelector("table[data-maintenance-grid-id='LookupsWithProviderGrid1']"));

            var lastRow = gridRows.LastOrDefault();
            Assert.IsNotNull(lastRow);

            lookupPage.SetElementValue(lastRow, "input[name*='Code']", code);
            lookupPage.SetElementValue(lastRow, "input[name*='Description']", string.Format("Description{0}", randomNumber));

            // Save the value
            lookupPage.ClickSaveButton();

            Detail.WaitForDetail();

            return lookupPage.GetMessage(messageToGet);           
        }

        #endregion
    }
}
