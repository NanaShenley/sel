using System;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.PLASC
{
    public class PupilExclusionsPanel: PlascSectionPanelBase
    {
        public PupilExclusionsPanel()
        {

        }

        public override string AutomationId
        {
            get
            {
                return "section_menu_Pupil Exclusions";
            }
        }

        public override string PanelName
        {
            get
            {
                return "Pupil Exclusions";
            }
        }

        /// <summary>
        /// Checks if proper data is loaded
        /// </summary>
        /// <returns></returns>
        public override bool CheckIfValidDataExist()
        {
            try
            {
                //check if no data message is displayed
                string scriptNoDataCheck = "return $(\".form-row:contains('No pupil exclusion information applicable to the Return')\").length > 0";
                Wait.WaitTillConditionIsMet(scriptNoDataCheck, 5);
            }
            catch (WebDriverTimeoutException)
            {
                //Wait till section is rendered
                string jsVisibilityPredicate = "return $(\"[name='PLASCPupilExclusionSection.PLASCPupilExclusions'] .webix_ss_header, .webix_ss_body\").is(':visible')";
                Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 30);

                //Visibility test (row data check)
                string script = "return $(\"[name='PLASCPupilExclusionSection.PLASCPupilExclusions'] .webix_ss_center .webix_column.webix_first .webix_cell\").length > 0 ";
                Wait.WaitTillConditionIsMet(script, 30);
            }

            return true;
        }
    }
}
