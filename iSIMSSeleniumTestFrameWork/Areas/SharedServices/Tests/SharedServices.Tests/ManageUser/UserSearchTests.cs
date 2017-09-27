using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.Helpers;
using SharedServices.Components.UserNotification;
using TestSettings;

namespace SharedServices.Tests
{
    public class UserSearchTests
    {
        private const string UserName = "yeriueiwiwiwuwuw";
        private const int NotYetInvited = 0, Invited = 1, Reinvited = 2, Activated = 3, Disabled = 4;
        private string Sql = "SELECT COUNT(1) FROM app.AuthorisedUser WHERE username = '{0}' AND Status = {1}";

        [WebDriverTest("UserSearchTests", "All_users_returned_when_status_is_null", 
            Groups = new[] { "UserSearch" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void All_users_returned_when_status_is_null()
        {
            Setup();

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(true);
            manageUsers.Search();
            int expectedResults = manageUsers.GetNumberOfSearchResults();

            using (new DataSetup(false, true, CreateUserRecords(null)))
            {
                manageUsers.Search();

                Assert.AreEqual(expectedResults + 1, manageUsers.GetNumberOfSearchResults());
            }
        }

        [WebDriverTest("UserSearchTests", "Return_invited_and_reinvited_users_on_selecting_invited", 
            Groups = new[] { "UserSearch" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Return_invited_and_reinvited_users_on_selecting_invited()
        {
            Setup();

            string sql = "SELECT COUNT(1) FROM app.AuthorisedUser WHERE username = '{0}' AND (Status = {1} OR Status = {2})";

            int expectedResults = DataAccessHelpers.GetValue<int>(string.Format(sql, UserName, Invited, Reinvited));

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.EnterSearchCriteria(UserName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Invited);

            using (new DataSetup(false, true, CreateUserRecords(Invited, UserName)))
            {
                manageUsers.Search();
                Assert.AreEqual(expectedResults + 1, manageUsers.GetNumberOfSearchResults());
            }
            using (new DataSetup(false, true, CreateUserRecords(Reinvited, UserName)))
            {
                manageUsers.Search();
                Assert.AreEqual(expectedResults + 1, manageUsers.GetNumberOfSearchResults());
            }
        }

        [WebDriverTest("UserSearchTests", "Return_NotYetInvited_users_on_selecting_notyetinvited",
            Groups = new[] { "UserSearch" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Return_NotYetInvited_users_on_selecting_notyetinvited()
        {
            Setup();

            int expectedResults = DataAccessHelpers.GetValue<int>(string.Format(Sql, UserName, NotYetInvited));

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.EnterSearchCriteria(UserName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(NotYetInvited);

            using (new DataSetup(false, true, CreateUserRecords(NotYetInvited, UserName)))
            {
                manageUsers.Search();
                Assert.AreEqual(expectedResults + 1, manageUsers.GetNumberOfSearchResults());
            }
        }

        [WebDriverTest("UserSearchTests", "Return_Activated_users_on_selecting_activated",
            Groups = new[] { "UserSearch" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Return_Activated_users_on_selecting_activated()
        {
            Setup();

            int expectedResults = DataAccessHelpers.GetValue<int>(string.Format(Sql, UserName, Activated));

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.EnterSearchCriteria(UserName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Activated);

            using (new DataSetup(false, true, CreateUserRecords(Activated, UserName)))
            {
                manageUsers.Search();
                Assert.AreEqual(expectedResults + 1, manageUsers.GetNumberOfSearchResults());
            }
        }

        [WebDriverTest("UserSearchTests", "Return_Disabled_users_on_selecting_activated",
            Groups = new[] { "UserSearch" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Return_Disabled_users_on_selecting_activated()
        {
            Setup();

            int expectedResults = DataAccessHelpers.GetValue<int>(string.Format(Sql, UserName, Disabled));

            ManageUsers manageUsers = new ManageUsers();
            manageUsers.EnterSearchCriteria(UserName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Disabled);

            using (new DataSetup(false, true, CreateUserRecords(Disabled, UserName)))
            {
                manageUsers.Search();
                Assert.AreEqual(expectedResults + 1, manageUsers.GetNumberOfSearchResults());
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
            return CreateUserRecords(status, "test1");
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
                    Status = status
                });
        }
    }
}