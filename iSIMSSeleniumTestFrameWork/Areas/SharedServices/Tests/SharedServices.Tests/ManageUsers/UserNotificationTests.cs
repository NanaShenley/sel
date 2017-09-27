using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using TestSettings;

namespace SharedServices.Tests.ManageUsers
{
    public class UserNotificationTests
    {
        /// <summary>
        ///Create User and identify user having no account
        /// </summary>
        [WebDriverTest(Groups = new[] { TestGroups.UserNotification }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void CreateUserHavingNoAccount()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SystemManger);
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage Users");

            Components.UserNotification.ManageUsers manageUsers = new Components.UserNotification.ManageUsers();
            manageUsers.ClickCreateButton();
            manageUsers.SetName(manageUsers.TestUserName, SeSugar.Utilities.GenerateRandomString(5, "Test_"));
            manageUsers.SaveRecord();

            //Check notification when user has no account associated
            GetNotificationWhenNoAccount(manageUsers);
        }

        private void GetNotificationWhenNoAccount(Components.UserNotification.ManageUsers manageUsers)
        {           
            //Search record having no account
            AutomationSugarHelpers.WaitForAndClickOn("search_criteria_advanced");

            AutomationSugar.WaitForAjaxCompletion();

            manageUsers.SetAllStatusSearchBoxes(true);
            manageUsers.EnterSearchCriteria(manageUsers.TestUserName);
            manageUsers.Search();

            Assert.IsTrue(SearchResults.HasResults());

            SearchResults.SelectSearchResult(0);
            AutomationSugar.WaitForAjaxCompletion();

            Assert.AreEqual("This user has not yet been invited. Please review the Permission Groups and select 'Invite'.", manageUsers.GetUserAccountMessage());
        }
    }
}
