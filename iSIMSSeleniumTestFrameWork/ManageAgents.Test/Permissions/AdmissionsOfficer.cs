using Agents.Components;
using Agents.Components.Utils;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;

namespace ManageAgents.Test.Permissions
{
    class AdmissionsOfficer
    {      

        #region AdmissionsOfficerAgentAccess()
        [WebDriverTest(Enabled = true, Groups = new[] {"AgentPermissions","All","P2"}, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AgentAccessPermissionForAdmissionOfficer()
        {
           Assert.True(AgentScreenNavigation.NavigateToAgentMenuPage(SeleniumHelper.iSIMSUserType.AdmissionsOfficer));
        }
        #endregion
        // Note- Not seeing Communication link at all
    }
}
