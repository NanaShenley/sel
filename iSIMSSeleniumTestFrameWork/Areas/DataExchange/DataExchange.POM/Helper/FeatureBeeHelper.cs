using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Helper
{
    public class FeatureBeeHelper
    {
        public FeatureBeeHelper()
        {

        }

        /// <summary>
        /// Turns a feature ON
        /// Pre-condition: Feature tray is available in UI(featuretray=show)
        /// </summary>
        /// <param name="featureName">Feature Name</param>
        public void TurnFeatureOn(string featureName)
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;

            //Wait & ensure feature bee tray is loaded
            string script = "return $(\".feature-bee-loading\").is(':visible') == false";
            Wait.WaitTillConditionIsMet(script, 120);

            //Open the tray
            script = "$(\".feature-bee-show\").click()";
            jsExecutor.ExecuteScript(script);

            //Wait till feature is populated in tray
            script = "return $(\"button[data-toggle-target='" + featureName + "']\").length > 0";
            Wait.WaitTillConditionIsMet(script, 120);

            //Check current state and ON the feature if required
            script = "return $(\"button[data-toggle-target='" + featureName + "']\").attr('data-toogle-state');";

            string featuteState = (string)jsExecutor.ExecuteScript(script);

            if(featuteState.ToLower() == "off")
            {
                script = "return $(\"button[data-toggle-target='" + featureName + "']\").click();";
                jsExecutor.ExecuteScript(script);
            }

            //Hide the tray
            script = "$(\".feature-bee-hide\").click()";
            jsExecutor.ExecuteScript(script);
        }


    }
}
