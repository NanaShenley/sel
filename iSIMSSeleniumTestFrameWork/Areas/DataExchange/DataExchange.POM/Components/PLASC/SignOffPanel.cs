using System;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.PLASC
{
    public class SignOffPanel : PlascSectionPanelBase
    {
        public override string AutomationId
        {
            get { return "section_menu_Sign Off"; }
        }

        public override string PanelName
        {
            get { return "Sign Off"; }
        }
        public override bool CheckIfValidDataExist()
        {
            //Wait till section is rendered
            string jsVisibilityPredicate = "return $(\"[name = 'SignOffSection.AdditionalText']\").is(':visible')";
            Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 10);

            //Visibility test
            string script = "return $(\"input[name = 'SignOffSection.AdditionalText']\").is(':visible')";

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;
            bool hasAllData = (Boolean)jsExecutor.ExecuteScript(script);

            return hasAllData;
        }
    }
}
