﻿using Agents.Components;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;

namespace ManageAgents.Test.Permissions
{
    class SeniorManagementTeam
    {
        #region SeniorManagementTeamAgentAccess()
        [WebDriverTest(Enabled = true, Groups = new[] { "AgentPermissions", "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AgentAccessPermissionForSeniorManagementTeam()
        {
            Assert.False(AgentScreenNavigation.NavigateToAgentMenuPage(SeleniumHelper.iSIMSUserType.SeniorManagementTeam));
        }
        #endregion
    }
}
