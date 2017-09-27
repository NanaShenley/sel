using DataExchange.POM.Helper;
using OpenQA.Selenium;

namespace DataExchange.POM.Components.PLASC
{
    public class OnRollPupilSENSection : PlascSectionPanelBase
    {
        public OnRollPupilSENSection()
        {

        }

        public override string AutomationId
        {
            get
            {
                return "section_menu_On Roll Pupil SEN";
            }
        }

        public override string PanelName
        {
            get
            {
                return "On Roll Pupil SEN";
            }
        }
        public override bool CheckIfValidDataExist()
        {
            try
            {
                //check if no data message is displayed
                string scriptNoDataCheck = "return $(\".form-row:contains('No On Roll Pupil SEN Result information applicable to the Return')\").length > 0";
                Wait.WaitTillConditionIsMet(scriptNoDataCheck, 5);
            }
            catch (WebDriverTimeoutException)
            {
                //Wait till section is rendered
                string jsVisibilityPredicate = "return $(\"[name='OnRollPupilSENSection.OnRollPupilSENDetails'] .webix_ss_header, .webix_ss_body\").is(':visible')";
                Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 30);

                //Visibility test (row data check)
                string script = "return $(\"[name='OnRollPupilSENSection.OnRollPupilSENDetails'] .webix_ss_center .webix_column.webix_first .webix_cell\").length > 0 ";
                Wait.WaitTillConditionIsMet(script, 30);
            }

            try
            {
                //check if no data message is displayed
                string scriptNoDataCheck = "return $(\".form-row:contains('No On Roll Pupil SEN Need Result information applicable to the Return')\").length > 0";
                Wait.WaitTillConditionIsMet(scriptNoDataCheck, 5);
            }
            catch (WebDriverTimeoutException)
            {
                //Wait till section is rendered
                string jsVisibilityPredicate = "return $(\"[name='OnRollPupilSENSection.OnRollPupilSENNeeds'] .webix_ss_header, .webix_ss_body\").is(':visible')";
                Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 30);

                //Visibility test (row data check)
                string script = "return $(\"[name='OnRollPupilSENSection.OnRollPupilSENNeeds'] .webix_ss_center .webix_column.webix_first .webix_cell\").length > 0 ";
                Wait.WaitTillConditionIsMet(script, 30);
            }

            return true;

        }
    }
}
