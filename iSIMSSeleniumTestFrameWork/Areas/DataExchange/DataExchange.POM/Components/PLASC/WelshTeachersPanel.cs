using System;
using DataExchange.POM.Helper;

namespace DataExchange.POM.Components.PLASC
{
    public class WelshTeachersPanel: PlascSectionPanelBase
    {
        public WelshTeachersPanel()
        {

        }

        public override string AutomationId
        {
            get
            {
                return "section_menu_Teachers of Welsh";
            }
        }

        public override string PanelName
        {
            get
            {
                return "Welsh Teachers";
            }
        }
        public override bool CheckIfValidDataExist()
        {
            //Wait till section is rendered
            string jsVisibilityPredicate = "return $(\"[name='WelshTeacherSection.NAWWelshTeachers'] .webix_ss_header, .webix_ss_body\").is(':visible')";
            Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 30);

            //Visibility test - Check if all 5 rows are shown (base data)
            string script = "return $(\"[name='WelshTeacherSection.NAWWelshTeachers'] .webix_ss_center .webix_column.webix_first .webix_cell\").length == 5";
            Wait.WaitTillConditionIsMet(script, 30);

            return true;

        }
    }
}
