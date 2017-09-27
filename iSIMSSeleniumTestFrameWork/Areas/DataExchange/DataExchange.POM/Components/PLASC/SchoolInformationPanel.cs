using DataExchange.POM.Helper;
using OpenQA.Selenium;
using System;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.PLASC
{
    public class SchoolInformationPanel : PlascSectionPanelBase
    {
        public SchoolInformationPanel()
        {
        }

        public override string AutomationId
        {
            get
            {
                return "section_menu_School Information";
            }
        }

        public override string PanelName
        {
            get
            {
                return "Census School Information";
            }
        }

        /// <summary>
        /// Checks if proper data is loaded
        /// </summary>
        /// <returns></returns>
        public override bool CheckIfValidDataExist()
        {
            //Wait till section is rendered
            string jsVisibilityPredicate = "return $(\"[name = 'SchoolSection.LEA']\").is(':visible')";
            Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 30);

            //Visibility test
            string script = "return $(\"input[name = 'SchoolSection.LEA']\").is(':visible') && ";
            script += " $(\"input[name = 'SchoolSection.Estab']\").is(':visible') && ";
            script += " $(\"input[name = 'SchoolSection.SchoolTypeDescription']\").is(':visible') && ";
            script += " $(\"input[name = 'SchoolSection.SchoolName']\").is(':visible') && ";
            script += " $(\"input[name = 'SchoolSection.IntakeTypeDescription']\").is(':visible') && ";
            script += " $(\"input[name = 'SchoolSection.GovernanceDescription']\").is(':visible') && ";
            script += " $(\"input[name = 'SchoolSection.SchoolPhaseDescription']\").is(':visible') && ";
            script += " $(\"input[name = 'SchoolSection.GenderMixDescription']\").is(':visible') && ";
            script += " $(\"input[name = 'SchoolSection.FederatedGoverningBody']\").is(':visible') && ";
            script += " $(\"input[name = 'SchoolSection.PhoneNo']\").is(':visible') && ";
            script += " $(\"input[name = 'SchoolSection.WelshMediumTypeDescription']\").is(':visible') ";
            
            //Data test - Not needed
            
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;
            bool hasAllData = (Boolean)jsExecutor.ExecuteScript(script);

            return hasAllData;
            
        }
    }
}
