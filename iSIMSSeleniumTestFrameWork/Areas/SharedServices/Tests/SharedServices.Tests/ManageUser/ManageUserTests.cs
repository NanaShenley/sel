using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SharedComponents.Helpers;
using SharedServices.Components.UserNotification;
using TestSettings;

namespace SharedServices.Tests.ManageUser
{
    public class ManageUserTests
    {
        private ManageUsers _manageUsers;

        [NotDone]
        [WebDriverTest(Groups = new[] { "ManageUser" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SendInvite()
        {
            var forename = CreateUser();
            SearchNewlyCreated(forename);
            Assert.AreEqual(_manageUsers.GetNumberOfSearchResults(), 1);
        }

        [NotDone]
        [WebDriverTest(Groups = new[] { "ManageUser" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ReInvite()
        {
            var forename = CreateUser();
            SearchNewlyCreated(forename);

            _manageUsers.SelectFirstRecord();
            _manageUsers.ClickReInviteButton();
            _manageUsers.SetInviteEmail(string.Format("{0}1@capita.co.uk", forename));
            _manageUsers.ClickSendInviteButton();

            var message = _manageUsers.ReadStatusMessage();
            Assert.AreEqual(string.Format("{0}1@capita.co.uk", forename), message);
        }

        private void SearchNewlyCreated(string forename)
        {
            _manageUsers.EnterSearchCriteria(forename);
            _manageUsers.Search();
        }

        private string CreateUser()
        {
            var forename = SeSugar.Utilities.GenerateRandomString(5, "Forename");
            NavigateToScreen();

            _manageUsers = new ManageUsers();
            _manageUsers.ClickCreateButton();
            _manageUsers.SetName(forename, SeSugar.Utilities.GenerateRandomString(5, "Surname"));
            _manageUsers.ClickInviteButton();
            _manageUsers.SetInviteEmail(string.Format("{0}@capita.co.uk", forename));
            _manageUsers.ClickSendInviteButton();

            return forename;
        }

        private static void NavigateToScreen()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SystemManger, enabledFeatures: "ExternalIdentity");
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.NavigateMenu("Tasks", "System Manager", "Manage Users");
        }
    }
}
