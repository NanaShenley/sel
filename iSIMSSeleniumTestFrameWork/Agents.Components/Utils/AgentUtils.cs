using NUnit.Framework;
using SharedComponents.Helpers;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using SharedComponents.HomePages;
using Agents.Components;
using OpenQA.Selenium;

namespace Agents.Components.Utils
{
    public class AgentUtils
    {
        public static AgentSearchScreen AddNewAgentForReuse(string s1, string s2)  //For No Match ( NEW)
        {
            AgentSearchScreen ScreenObject = AgentScreenNavigation.NavigateToAgentMenuPage();
            AddNewAgentDialog dialogObj = ScreenObject.ClickAddNewAgent();
            AgentServicesProvided obj = dialogObj.EnterNames(s1, s2);
            AgentSearchScreen mainobj = obj.SelectAgentService();
            return  new AgentSearchScreen();
        }

        public static AgentSearchScreen AddAgentForReuse(string s1, string s2)  //For Matching Result Found
        {
            AgentSearchScreen ScreenObject = AgentScreenNavigation.NavigateToAgentMenuPage();
            AddNewAgentDialog dialogObj = ScreenObject.ClickAddNewAgent();
            MatchingAgentScreen obj = dialogObj.PassingNames(s1, s2);
            AgentServicesProvided mainobj = obj.ClickOnNewAgentButton();
           mainobj.SelectAgentService();
           return new AgentSearchScreen();
         
        }
    }
}
