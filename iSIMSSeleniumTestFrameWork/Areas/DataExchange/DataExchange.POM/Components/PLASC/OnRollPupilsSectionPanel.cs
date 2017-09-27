using System;
using DataExchange.POM.Helper;

namespace DataExchange.POM.Components.PLASC
{
    public class OnRollPupilsSectionPanel : PlascSectionPanelBase
    {
        public OnRollPupilsSectionPanel()
        {
            
        }

        public override string AutomationId
        {
            get
            {
                return "section_menu_On Roll Pupil";
            }
        }

        public override string PanelName
        {
            get
            {
                return "On-Roll Pupils";
            }
        }

        /// <summary>
        /// Checks if proper data is loaded
        /// </summary>
        /// <returns></returns>
        public override bool CheckIfValidDataExist()
        {
            //Wait till section is rendered
            string jsVisibilityPredicate = "return $(\"[name='OnRollPupilSection.OnRollPupils'] .webix_ss_header, .webix_ss_body\").is(':visible')";
            Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 30);

            //Visibility test (row data check)
            string script = "return $(\"[name='OnRollPupilSection.OnRollPupils'] .webix_ss_center .webix_column.webix_first .webix_cell\").length > 0 ";
            Wait.WaitTillConditionIsMet(script, 30);

            return true;
        }
    }
}
