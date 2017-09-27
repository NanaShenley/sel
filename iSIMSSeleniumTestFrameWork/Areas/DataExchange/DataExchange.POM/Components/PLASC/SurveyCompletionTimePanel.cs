using System;
using DataExchange.POM.Helper;

namespace DataExchange.POM.Components.PLASC
{
    public class SurveyCompletionTimePanel: PlascSectionPanelBase
    {
        public SurveyCompletionTimePanel()
        {

        }

        public override string AutomationId
        {
            get
            {
                return "section_menu_Survey Completion Time";
            }
        }

        public override string PanelName
        {
            get
            {
                return "Survey Completion Time";
            }
        }
        public override bool CheckIfValidDataExist()
        {
            //Wait till section is rendered
            string jsVisibilityPredicate = "return $(\"[name='SurveyCompletionSection.SurveyCompletions'] .webix_ss_header, .webix_ss_body\").is(':visible')";
            Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 30);

            //Visibility test - Check if all 8 rows are shown (base data)
            string script = "return $(\"[name='SurveyCompletionSection.SurveyCompletions'] .webix_ss_center .webix_column.webix_first .webix_cell\").length == 8";
            Wait.WaitTillConditionIsMet(script, 30);

            return true;

        }
    }
}
