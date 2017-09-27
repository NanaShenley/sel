using System;
using DataExchange.POM.Helper;

namespace DataExchange.POM.Components.PLASC
{
    public class SupportSatffPanel : PlascSectionPanelBase
    {
        public override string AutomationId
        {
            get { return "section_menu_Support Staff"; }
        }

        public override string PanelName
        {
            get
            {
                return "Support Staff";
            }
        }

        public override bool CheckIfValidDataExist()
        {
            //Wait till section is rendered
            string jsVisibilityPredicate = "return $(\"[name='NAWSupportStaffSection.NAWSupportStaffs'] .webix_ss_header, .webix_ss_body\").is(':visible')";
            Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 30);

            //Visibility test - Check if all 10 rows are shown (base data)
            string script = "return $(\"[name='NAWSupportStaffSection.NAWSupportStaffs'] .webix_ss_center .webix_column.webix_first .webix_cell\").length == 10";
            Wait.WaitTillConditionIsMet(script, 30);

            return true;
        }
    }
}
