using Facilities.Components;
using Facilities.Components.Common;
using Facilities.Components.Facilities_Pages;
using Facilities.Components.FacilitiesPages;
using NUnit.Framework;
using OpenQA.Selenium;
using Selene.Support.Attributes;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;


namespace Facilities.ManageKPI.Tests
{

   public class ManageAttendanceKeyPerformanceIndicators

    {
       #region Check if Current Academic Year Selected.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void CheckCurrentAcademicYearSelected()
        {
            var attendanceKpi = FacilitiesNavigation.NavigateToManageKPIPage();
            Assert.IsTrue(attendanceKpi.IsCurrentAcademicYearSelected());
        }
        #endregion

       #region Modify Academic Year
        [WebDriverTest(Enabled = true, Groups = new[] { "All" , "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ManageKPI()
        {
            var attendanceKpi = FacilitiesNavigation.NavigateToManageKPIPage();
            attendanceKpi.ModifyTargetValue("70.5");
            attendanceKpi.ModifyBenchMark("80.5");
            attendanceKpi.ModifyThresholdMinimum("90.5");
            attendanceKpi.ModifyThresholdMaximum("95.5");
            attendanceKpi.Save();
            Assert.IsTrue(attendanceKpi.HasConfirmedSave());

        }
        #endregion

       #region Check for mandatory validation for Target Value
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidateTargetValue()
        {
            var attendanceKpi = FacilitiesNavigation.NavigateToManageKPIPage();
            attendanceKpi.ClearTargetValue();
            attendanceKpi.ModifyBenchMark("70.55");
            attendanceKpi.Save();
            //TODO: change for Story ID 8720
            var arrWarnings = attendanceKpi.GetValidationWarningByCssSelector();
            Assert.Contains("'School target attendance' is required.", arrWarnings);         
            //Assert.Contains("Validation Warning", arrWarnings);         
        }
    #endregion

       #region Check for mandatory validation for Benchmark
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]

      public void ValidateBechmark()
      {
          var attendanceKpi = FacilitiesNavigation.NavigateToManageKPIPage();
          attendanceKpi.ClearBenchMark();
          attendanceKpi.ModifyTargetValue("70.55");
          attendanceKpi.Save();
          //TODO: change for Story ID 8720
          var arrWarnings = attendanceKpi.GetValidationWarningByCssSelector();
          Assert.Contains("'National average attendance' is required.", arrWarnings);
          //Assert.Contains("Validation Warning", arrWarnings);         
      }
    #endregion

       #region Check for BR Threshold Minimum cannot be larger than or equal to Threshold Maximum.
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
       public void BRValidation()
       {
           var attendanceKpi = FacilitiesNavigation.NavigateToManageKPIPage();
           attendanceKpi.ModifyTargetValue("70.5");
           attendanceKpi.ModifyBenchMark("80.5");
           attendanceKpi.ModifyThresholdMinimum("90");
           attendanceKpi.ModifyThresholdMaximum("80");
           attendanceKpi.Save();
           //TODO: change for Story ID 8720
           var arrWarnings = attendanceKpi.GetValidationWarningByCssSelector();
           Assert.Contains("'Display red for attendance below n' cannot be larger than 'Display green for attendance above or equal to n'.", arrWarnings);
           //Assert.Contains("Validation Warning", arrWarnings);         
       }
    #endregion
    }
}
