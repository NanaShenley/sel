using System;
using DataExchange.POM.Helper;

namespace DataExchange.POM.Components.PLASC
{
    public class AllTeachersPanel : PlascSectionPanelBase
    {
        public AllTeachersPanel()
        {
        }

        public override string AutomationId
        {
            get
            {
                return "section_menu_Teachers (All Teachers)";
            }
        }

        public override string PanelName
        {
            get
            {
                return "All Teachers";
            }
        }

        public override bool CheckIfValidDataExist()
        {
            //Wait till section is rendered
            string jsVisibilityPredicate = "return $(\"[name='AllTeachersSection.NAWTeachers'] .webix_ss_header, .webix_ss_body\").is(':visible')";
            Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 30);

            //Visibility test - Check if all 10 rows are shown (base data)
            string script = "return $(\"[name='AllTeachersSection.NAWTeachers'] .webix_ss_center .webix_column.webix_first .webix_cell\").length == 10";
            Wait.WaitTillConditionIsMet(script, 30);

            return true;

        }
    }
}
