using System;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.PLASC
{
    public class GeneralSectionPanel : PlascSectionPanelBase
    {

        public override string AutomationId
        {
            get
            {
                return "section_menu_General";
            }
        }

        public override string PanelName
        {
            get
            {
                return "General Section";
                
            }
        }


        /// <summary>
        /// Checks if proper data is loaded
        /// </summary>
        /// <returns></returns>
        public override bool CheckIfValidDataExist()
        {
            //Wait till section is rendered
            string jsVisibilityPredicate = "return $(\"[name = 'GeneralSection.PupilsEligibleForFSM']\").is(':visible')";
            Wait.WaitTillConditionIsMet(jsVisibilityPredicate, 10);

            //Visibility test
            string script = "return $(\"input[name = 'GeneralSection.PupilsEligibleForFSM']\").is(':visible') &&";

            script += " $(\"input[name = 'GeneralSection.HeadTeachingIndicator.dropdownImitator']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.FreeSchoolMealsTaken']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.PaidSchoolMealsTaken']\").is(':visible') && ";

            //Free Breakfast
            script += " $(\"input[name = 'GeneralSection.FreeBreakfastsTaken']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.FreeBreakfastsTakenInWeekPrior']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.FreeBreakfastsForFSMEligible']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.FreeBreakfastsTakenInWeekPriorByFSMEligiblePupils']\").is(':visible') && ";

            ////School Milk
            script += " $(\"input[name = 'GeneralSection.FreeMilkTaken']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.SchoolMilkBought']\").is(':visible') && ";


            ////Attendance
            script += " $(\"input[name = 'GeneralSection.FullTimeAttendanceCensusDay']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.PartTimeAttendanceCensusDay']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.MorningAttendance']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.AfternoonAttendance']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.OtherAttendance']\").is(':visible') && ";

            //Governors

            script += " $(\"input[name = 'GeneralSection.HeadGovernor']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.MaleGovernor']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.FemaleGovernor']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.WelshGovernor']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.GovernorVacancy']\").is(':visible') && ";

            //LEA
            script += " $(\"input[name = 'GeneralSection.LEASpecialClass']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.PupilInLEASpecialClass']\").is(':visible') && ";
            script += " $(\"input[name = 'GeneralSection.PupilOSLEASpecialClass']\").is(':visible')";

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;
            bool hasAllData = (Boolean)jsExecutor.ExecuteScript(script);

            return hasAllData;
        }
    }


}
