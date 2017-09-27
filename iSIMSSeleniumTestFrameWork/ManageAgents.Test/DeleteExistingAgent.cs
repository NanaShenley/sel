using NUnit.Framework;
using SharedComponents.Helpers;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using SharedComponents.HomePages;
using Agents.Components;
using TestSettings;
using OpenQA.Selenium;
using Agents.Components.Utils;
using System;
using SharedComponents.BaseFolder;
using POM.Helper;

namespace ManageAgents.Test
{
    class DeleteExistingAgent
    {
        #region Story 13349 Delete Agents.
        [WebDriverTest(Enabled = true, Groups = new[] { "Agents" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void CanSelectAndDeleteExistingAgent()
        {
            //Can I select to delete any individual Agent record via existing Agent record screen?
            AgentSearchScreen ScreenObject = AgentScreenNavigation.NavigateToAgentMenuPage();
            AddNewAgentDialog dialogObj = ScreenObject.ClickAddNewAgent();
            string fname = AddNewAgentDialog.RandomString(10);
            string lname = AddNewAgentDialog.RandomString(10);
            AgentServicesProvided obj = dialogObj.EnterNames(fname, lname);
            AgentSearchScreen mainobj = obj.SelectAgentService();

            POM.Helper.SeleniumHelper.CloseTab("Agent Details");        
            AgentSearchScreen SObj = AgentScreenNavigation.NavigateToAgentMenuPage(false);
            ScreenObject.SelectAgentRecordFromSearch(fname, lname);
            
            //Delete the newly added record. 
            Assert.True(ScreenObject.DeleteAgentRecord());
        }
        #endregion 
    }
}
