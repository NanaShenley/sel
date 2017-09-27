using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Facilities.Components.Common
{
    public class AttendanceKPIElement
    {

        public static readonly By KPIMainPage = By.Id("editableData");
        //TODO : Need to replace by Automation ID.
        public static readonly By KPIGetTitle = By.CssSelector(".pane-header.section-header");
        public static readonly By KPITargetValue = By.Name("TargetValue");
        public static readonly By KPIMinThreshold = By.Name("MinThreshold");
        public static readonly By KPIMaxThreshold = By.Name("MaxThreshold");
        public static readonly By Save = By.CssSelector("[data-automation-id='well_know_action_save']"); 
        public static readonly By Savesuccessfuldisplayed = By.CssSelector("[data-automation-id='status_success']");
        public static readonly By ValidationScreen = By.CssSelector("[data-automation-id='status_error']"); 

    }
    public static class ManageKPIConstants
    {
        public const string ExpectedTitle = "Key Performance Indicators";
    }
}
