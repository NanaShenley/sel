using OpenQA.Selenium;
using System;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.PLASC
{
    public abstract class PlascSectionPanelBase
    {
        public abstract string AutomationId { get; }

        public abstract string PanelName { get; }

        /// <summary>
        /// Checks if section Panel is open 
        /// </summary>
        /// <returns></returns>
        public bool IsSectionPanelOpen()
        {
            string script = "return $(\"[data-automation-id = '" + AutomationId + "']\").attr('aria-expanded');";

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;
            string isExpanded = (string)jsExecutor.ExecuteScript(script);

            return isExpanded.ToLower() == "true";
        }

        /// <summary>
        /// Toggles section panel state, (Open & Close)
        /// </summary>
        public void ToggleSectionPanel()
        {
            string script = "return $(\"[data-automation-id = '" + AutomationId + "']\").click();";

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;
            jsExecutor.ExecuteScript(script);
        }

        public abstract bool CheckIfValidDataExist();

        public bool IsSectionPanelExists()
        {
            string script = "return $(\"[data-automation-id = '" + AutomationId + "']\").length > 0;";

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;
            bool doesExist = (Boolean)jsExecutor.ExecuteScript(script);

            return doesExist;
        }
    }
}
