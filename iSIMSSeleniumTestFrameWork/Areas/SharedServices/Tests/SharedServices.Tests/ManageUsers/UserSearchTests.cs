using System;
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
    public class UserSearchTests
    {
        private const int NotYetInvited = 0, Invited = 1, Reinvited = 2, Activated = 3, Disabled = 4;
        private string Sql = "SELECT COUNT(1) FROM app.AuthorisedUser WHERE username = '{0}' AND Status = {1}";

        [WebDriverTest(Groups = new[] { TestGroups.UserSearch }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void All_users_returned_when_status_is_null()
        {
            string userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(true);
            manageUsers.Search();
            int expectedResults = manageUsers.GetNumberOfSearchResults() + 1;

            using (new DataSetup(false, true, CreateUserRecords(null, userName)))
            {
                manageUsers.Search();

                Assert.AreEqual(expectedResults, manageUsers.GetNumberOfSearchResults());
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.UserSearch }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void Return_invited_and_reinvited_users_on_selecting_invited()
        {
            string userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            string sql = "SELECT COUNT(1) FROM app.AuthorisedUser WHERE username = '{0}' AND (Status = {1} OR Status = {2})";

            int expectedResults = DataAccessHelpers.GetValue<int>(string.Format(sql, userName, Invited, Reinvited)) + 1;

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Invited);

            using (new DataSetup(false, true, CreateUserRecords(Invited, userName)))
            {
                manageUsers.Search();
                Assert.AreEqual(expectedResults, manageUsers.GetNumberOfSearchResults());
            }
            using (new DataSetup(false, true, CreateUserRecords(Reinvited, userName)))
            {
                manageUsers.Search();
                Assert.AreEqual(expectedResults, manageUsers.GetNumberOfSearchResults());
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.UserSearch }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void Return_NotYetInvited_users_on_selecting_notyetinvited()
        {
            string userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            int expectedResults = DataAccessHelpers.GetValue<int>(string.Format(Sql, userName, NotYetInvited)) + 1;

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(NotYetInvited);

            using (new DataSetup(false, true, CreateUserRecords(NotYetInvited, userName)))
            {
                manageUsers.Search();
                Assert.AreEqual(expectedResults, manageUsers.GetNumberOfSearchResults());
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.UserSearch }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void Return_Activated_users_on_selecting_activated()
        {
            string userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            int expectedResults = DataAccessHelpers.GetValue<int>(string.Format(Sql, userName, Activated)) + 1;

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Activated);

            using (new DataSetup(false, true, CreateUserRecords(Activated, userName)))
            {
                manageUsers.Search();
                Assert.AreEqual(expectedResults, manageUsers.GetNumberOfSearchResults());
            }
        }

        [WebDriverTest(Groups = new[] { TestGroups.UserSearch }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void Return_Disabled_users_on_selecting_activated()
        {
            string userName = Utilities.GenerateRandomString(15, "Selenium");
            Setup();

            int expectedResults = DataAccessHelpers.GetValue<int>(string.Format(Sql, userName, Disabled)) + 1;

            var manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.EnterSearchCriteria(userName);
            manageUsers.ShowMoreStatusFilters();
            manageUsers.SetAllStatusSearchBoxes(false);
            manageUsers.CheckStatusSearchBox(Disabled);

            using (new DataSetup(false, true, CreateUserRecords(Disabled, userName)))
            {
                manageUsers.Search();
                Assert.AreEqual(expectedResults, manageUsers.GetNumberOfSearchResults());
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
                    Status = status
                });
        }
    }
}