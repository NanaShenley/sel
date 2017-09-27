using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.Helpers;
using SharedServices.Components.UserNotification;
using TestSettings;
using System.Threading;

namespace SharedServices.Tests.ManageUser
{
    public class UserEnableDisableTests
    {
        private const string UserName = "yeriueiwiwiwuwuw";
        private const int NotYetInvited = 0, Invited = 1, Reinvited = 2, Activated = 3, Disabled = 4;

        [WebDriverTest("UserEnableDisableTests", "No_Status_Buttons_Are_Visible_When_Status_is_not_valid",
            Groups = new[] { "UserEnabled" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void No_Status_Buttons_Are_Visible_When_Status_is_not_valid()
        {
            Setup();

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.EnterSearchCriteria(UserName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(NotYetInvited);

            using (new DataSetup(false, true, CreateUserRecords(NotYetInvited, UserName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                Assert.IsFalse(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.EnableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.DisableButton));
            }
        }

        [WebDriverTest("UserEnableDisableTests", "Check_Disable_exists_for_Activated_user",
            Groups = new[] { "UserEnabled" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Check_Disable_exists_for_Activated_user()
        {
            Setup();

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.EnterSearchCriteria(UserName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Activated);

            using (new DataSetup(false, true, CreateUserRecords(Activated, UserName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                Assert.IsTrue(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.DisableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.EnableButton));
            }
        }

        [WebDriverTest("UserEnableDisableTests", "Check_Enable_exists_for_Disabled_user",
            Groups = new[] { "UserEnabled" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Check_Enable_exists_for_Disabled_user()
        {
            Setup();

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.EnterSearchCriteria(UserName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Disabled);

            using (new DataSetup(false, true, CreateUserRecords(Disabled, UserName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                Assert.IsTrue(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.EnableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.DisableButton));
            }
        }

        [WebDriverTest("UserEnableDisableTests", "Check_Enable_exists_after_disabling_user",
            Groups = new[] { "UserEnabled" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Check_Enable_exists_after_disabling_user()
        {
            Setup();

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.EnterSearchCriteria(UserName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Activated);

            using (new DataSetup(false, true, CreateUserRecords(Activated, UserName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                manageUsers.ClickButton(ManageUsers.UserScreenElements.DisableButton);

                manageUsers.ClickButton(ManageUsers.UserScreenElements.PopUpDisableButton);

                Assert.IsTrue(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.EnableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.DisableButton));
            }
        }

        [WebDriverTest("UserEnableDisableTests", "Check_Disable_exists_after_enabling_user",
            Groups = new[] { "UserEnabled" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Check_Disable_exists_after_enabling_user()
        {
            Setup();

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.EnterSearchCriteria(UserName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Disabled);

            using (new DataSetup(false, true, CreateUserRecords(Disabled, UserName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                manageUsers.ClickButton(ManageUsers.UserScreenElements.EnableButton);

                manageUsers.ClickButton(ManageUsers.UserScreenElements.PopUpEnableButton);

                Assert.IsTrue(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.DisableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.EnableButton));
            }
        }

        [WebDriverTest("UserEnableDisableTests", "Check_Enable_exists_after_cancelling_enable",
            Groups = new[] { "UserEnabled" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Check_Enable_exists_after_cancelling_enable()
        {
            Setup();

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.EnterSearchCriteria(UserName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Disabled);

            using (new DataSetup(false, true, CreateUserRecords(Disabled, UserName)))
            {
                manageUsers.Search();
                manageUsers.SelectFirstRecord();

                manageUsers.ClickButton(ManageUsers.UserScreenElements.EnableButton);

                manageUsers.ClickButton(ManageUsers.UserScreenElements.PopUpCancelButton);

                Assert.IsTrue(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.EnableButton));
                Assert.IsFalse(manageUsers.CheckIfElementExists(ManageUsers.UserScreenElements.DisableButton));
            }
        }

        private void Setup()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SystemManger, enabledFeatures: "ExternalIdentity");
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage Users");
        }

        private DataPackage CreateUserRecords(int? status)
        {
            return CreateUserRecords(status, "statustest");
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