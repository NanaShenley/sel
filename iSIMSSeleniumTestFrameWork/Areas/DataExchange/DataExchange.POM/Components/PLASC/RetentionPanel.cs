using System;
using DataExchange.POM.Helper;

namespace DataExchange.POM.Components.PLASC
{
    public class RetentionPanel : PlascSectionPanelBase
    {
        public RetentionPanel()
        {

        }

        public override string AutomationId
        {
            get
            {
                return "section_menu_Retention";
            }
        }

        public override string PanelName
        {
            get
            {
                return "Retention";
            }
        }

        public override bool CheckIfValidDataExist()
        {
            //Wait till section is rendered
            string jsVisibilityPredicate = "return $(\"[name='RetentionSection.Retentions'] .webix_ss_header, .webix_ss_body\").is(':visible')";
            Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 30);

            /*We can't test row visibility here. It is a manual entry grid, may have 0 rows initially*/

            return true;
        }
    }
}
