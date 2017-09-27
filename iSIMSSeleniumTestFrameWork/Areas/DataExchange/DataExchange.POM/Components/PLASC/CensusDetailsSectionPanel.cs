using DataExchange.POM.Helper;
using OpenQA.Selenium;
using System;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.PLASC
{
    public class CensusDetailsSectionPanel: PlascSectionPanelBase
    {
        public CensusDetailsSectionPanel()
        {
            
        }

        public override string AutomationId
        {
            get
            {
                return "section_menu_Census Details";
            }
        }

        public override string PanelName
        {
            get
            {
                return "Census Basic Details";
            }
        }

        /// <summary>
        /// Checks if proper data is loaded
        /// </summary>
        /// <returns></returns>
        public override bool CheckIfValidDataExist()
        {
            //Wait till section is rendered
            string jsVisibilityPredicate = "return $(\"[name = 'CensusDate']\").is(':visible')";
            Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 10);

            //Visibility test
            string script = "return $(\"input[name = 'CensusDate']\").is(':visible') && ";
            script += " $(\"input[name = 'ReturnDesc']\").is(':visible') && ";
            script += " $(\"input[name = 'AgeAtDate']\").is(':visible') && ";
            //Data test
            script += "  $(\"input[name = 'CensusDate']\").val().length > 0 && ";
            script += " $(\"input[name = 'ReturnDesc']\").val().length > 0 && ";
            script += " $(\"input[name = 'AgeAtDate']\").val().length > 0 ";


            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;
            bool hasAllData = (Boolean)jsExecutor.ExecuteScript(script);

            return hasAllData;
        }
        
    }
}
