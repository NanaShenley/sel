using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.Components
{
    public class AgentElements
    {
        public struct SearchPanel
        {
            public static By SearchCriteria = By.CssSelector("[data-automation-id='search_criteria']");
            public static readonly By SearchButton = By.CssSelector("[data-automation-id='search_criteria_submit']");
            public static readonly By RadioButton = By.Name("IsDaily");
            public static readonly By WholeSchoolChecbox = By.Name("tri_chkbox_WholeSchool");
        }
        public struct MainScreen
        {
            public static By AddNewAgentButton = By.CssSelector("[data-automation-id='add_new_agent_button']");
            public static By AgentForename = By.Name("LegalForename");
            public static By SuccessMessage = By.CssSelector("[data-automation-id='status_success']");
            public static By SaveButton = By.CssSelector("[data-automation-id='well_know_action_save']");
        }
        public struct MatchingAgentScreen
        {
            public static By New_Agent_Button = By.CssSelector("[data-automation-id='no,_this_is_a_new_agent_button']");     
        }

        public struct ServicesProvidedScreen
        {
            public static By CreateRecordButton = By.CssSelector("[data-automation-id='add_record_button']");
            public static By CheckBoxAudiometrist = By.CssSelector("[data-automation-id='Audiometrist']");
            public static By AlertMessage = By.CssSelector(".validation-summary-errors");
            public static By BackButton = By.CssSelector("[data-automation-id='back_button']");
            
        }
    }
}
