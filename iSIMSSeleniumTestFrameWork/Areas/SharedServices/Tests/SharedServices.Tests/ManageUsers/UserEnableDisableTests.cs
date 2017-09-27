using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using TestSettings;

namespace SharedServices.Tests.ManageUsers
{
    public class UserEnableDisableTests
    {
        private const int NotYetInvited = 0, Invited = 1, Reinvited = 2, Activated = 3, Disabled = 4;

        [WebDriverTest(Groups = new[] { TestGroups.EnableDisableUser }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void No_Status_Buttons_Are_Visible_When_Status_is_not_valid()
        {
            var userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(NotYetInvited);

            using (new DataSetup(false, true, CreateUserRecords(NotYetInvited, userName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                Assert.IsFalse(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.EnableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.DisableButton));
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.EnableDisableUser }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void Check_Disable_exists_for_Activated_user()
        {
            var userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Activated);

            using (new DataSetup(false, true, CreateUserRecords(Activated, userName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                Assert.IsTrue(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.DisableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.EnableButton));
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.EnableDisableUser }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void Check_Enable_exists_for_Disabled_user()
        {
            var userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Disabled);

            using (new DataSetup(false, true, CreateUserRecords(Disabled, userName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                Assert.IsTrue(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.EnableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.DisableButton));
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.EnableDisableUser }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void Check_Enable_exists_after_disabling_user()
        {
            var userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Activated);

            using (new DataSetup(false, true, CreateUserRecords(Activated, userName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                manageUsers.ClickButton(Components.UserNotification.ManageUsers.UserScreenElements.DisableButton);
                manageUsers.ClickButton(Components.UserNotification.ManageUsers.UserScreenElements.PopUpDisableButton);

                Assert.IsTrue(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.EnableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.DisableButton));
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.EnableDisableUser }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void Check_Disable_exists_after_enabling_user()
        {
            var userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Disabled);

            using (new DataSetup(false, true, CreateUserRecords(Disabled, userName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                manageUsers.ClickButton(Components.UserNotification.ManageUsers.UserScreenElements.EnableButton);
                manageUsers.ClickButton(Components.UserNotification.ManageUsers.UserScreenElements.PopUpEnableButton);

                Assert.IsTrue(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.DisableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.EnableButton));
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.EnableDisableUser }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void Check_Enable_exists_after_cancelling_enable()
        {
            var userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Disabled);

            using (new DataSetup(false, true, CreateUserRecords(Disabled, userName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                manageUsers.ClickButton(Components.UserNotification.ManageUsers.UserScreenElements.EnableButton);

                manageUsers.ClickButton(Components.UserNotification.ManageUsers.UserScreenElements.PopUpCancelButton);

                Assert.IsTrue(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.EnableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(Components.UserNotification.ManageUsers.UserScreenElements.DisableButton));
            }
        }

        private void Setup()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SystemManger);
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage Users");
        }

        private DataPackage CreateUserRecords(int? status, string userName)
        {
            const string query = "SELECT TOP 1 ID FROM app.DataAuthority WHERE TenantID = {0} AND DataAuthorityType in (SELECT ID FROM app.DataAuthorityType WHERE Name='School')";
            var dataAuthority = DataAccessHelpers.GetValue<Guid>(string.Format(query, SeSugar.Environment.Settings.TenantId));

            return this.BuildDataPackage()
                .AddData("app.AuthorisedUser", new
                {
                    Id = Guid.NewGuid(),
                    DefaultDataAuthority = dataAuthority,
                    UserName = userName,
                    EmailAddress = "jcoales@capita.com",
                    IsVisible = true,
                    TenantID = SeSugar.Environment.Settings.TenantId,
                    Status = status,
                    StatusChanged = DateTime.Now
                });
        }
    }
}