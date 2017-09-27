using Attendance.Components.AttendancePages;
using Attendance.Components.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;

namespace Attendance.ApplyMarkOverDateRange.Tests
{
    public class ApplyMarkOverDateRangeDetailTests
    {
        #region Story #4636 : Navigate to Enter Marks Over Date Range Page

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void VerifyApplyMarkOverDateRangeLink()
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            Assert.IsTrue(page.headerTitle.Displayed);
        }
        #endregion

        #region Story #4617 : RadioButton Default Behaviour
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void CheckDefaultBehaviourOfRadiobutton()
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            var preserve = page.ClickRadioButton("Preserve");
            Assert.False(preserve.GetAttribute("checked") == "checked");
            page.ClickRadioButton("Overwrite");
        }
        #endregion

        #region Story #4630: Verfiy Pupil Picker Search Panel
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void PupilPickerSearchPanelShouldContain_Year_Class_And_SearchButton()
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            page.ClickAddPupilButton();
            var searchCriteria = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1" });
            searchCriteria.FindCheckBoxAndClick("Class", new List<string> { "4A" });
            Assert.IsTrue(SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchButton).Text == "Search");
        }

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void ShouldRemoveSelectedPupilsFromGrid()
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            AttendanceSearchPanel searchcriteria = page.ClickAddPupilButton();
            var checkbox = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            checkbox.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
            PupilPickerAvailablePupilSection AvailablePupils = searchcriteria.PupilPickerSearchButton();
            AvailablePupils.GetAvailablePupils();
            PupilPickerSelectedPupilSection selectedPupil = AvailablePupils.AddSelectedPupil();
            ApplyMarkOverDateRangePage app1 = selectedPupil.ClickApplyMarkOverDateRange_PupilPickerOkButton();
            app1.RemovePupilFromGrid();
            Assert.IsTrue(app1.PupilGrid.Displayed);
        }

        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        public void ShouldAddSelectedPupilsInGrid()
        {
            ApplyMarkOverDateRangePage page = AttendanceNavigations.NavigateToApplyMarkOverDateRangeFromTaskMenu();
            AttendanceSearchPanel searchcriteria = page.ClickAddPupilButton();
            var checkbox = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            checkbox.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
            PupilPickerAvailablePupilSection AvailablePupils = searchcriteria.PupilPickerSearchButton();
            AvailablePupils.GetAvailablePupils();
            PupilPickerSelectedPupilSection selectedPupil = AvailablePupils.AddSelectedPupil();
            ApplyMarkOverDateRangePage app1 = selectedPupil.ClickApplyMarkOverDateRange_PupilPickerOkButton();
            Assert.IsTrue(app1.PupilGrid.Displayed);
        }
        #endregion  

    }
}