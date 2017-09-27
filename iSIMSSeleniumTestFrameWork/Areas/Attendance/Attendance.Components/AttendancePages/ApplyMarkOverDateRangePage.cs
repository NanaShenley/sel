using Attendance.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SeSugar;
using System;
using System.Collections.Generic;
using WebDriverRunner.webdriver;
using POM.Helper;
using SeSugar.Data;
using Attendance.POM.Entities;
using Attendance.POM.DataHelper;

namespace Attendance.Components.AttendancePages
{
    public class ApplyMarkOverDateRangePage : BaseSeleniumComponents
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='apply_mark_over_date_range_popup_header_title']")]
        public IWebElement headerTitle;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='apply_mark_button']")]
        public readonly IWebElement applyMarkButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='close_button']")]
        public readonly IWebElement closeButton;
        [FindsBy(How = How.Name, Using = "PreserveOverwrite")]
        public readonly IWebElement RadioButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pupils_button']")]
        public readonly IWebElement AddPupilButton;
        [FindsBy(How = How.CssSelector, Using = "[view_id='cxgridMarksOverDateRangeLearnerLearner']")]
        public readonly IWebElement PupilGrid;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='remove_button']")]
        public readonly IWebElement trashIcon;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Date_Range_Dropdown']")]
        public readonly IWebElement SelectDateRangeButtonDefaultValue;
        [FindsBy(How = How.Name, Using = "StartDate")]
        public readonly IWebElement StartDate;
        [FindsBy(How = How.Name, Using = "EndDate")]
        public readonly IWebElement EndDate;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        public readonly IWebElement ValidationWarning;
        [FindsBy(How = How.Name, Using = "Mark.dropdownImitator")]
        public IWebElement _searchSelectMark;
        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        public IWebElement academicYearDropdown;

        public ApplyMarkOverDateRangePage()
        {
            var loc = By.CssSelector("[data-automation-id='apply_mark_button']");
            WaitForElement(loc);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void WaitForElement()
        {
            throw new NotImplementedException();
        }

        public IWebElement ClickRadioButton(string labelText)
        {
            var radio = WebContext.WebDriver.FindElements(By.Name("PreserveOverwrite"));
            var radioText = WebContext.WebDriver.FindElements(By.CssSelector(".radio-text"));

            for (int i = 0; i < radioText.Count; i++)
            {
                if (radioText[i].Text == labelText)
                {
                    return radio[i];
                }
            }
            return null;
        }

        public AttendanceSearchPanel ClickAddPupilButton()
        {
            WaitForElement(AttendanceElements.AddPupilPopUpElements.AddPupilLink);
            AddPupilButton.Click();
            Wait.WaitLoading();
            return new AttendanceSearchPanel();
        }

        public void RemovePupilFromGrid()
        {
            var element = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='remove_button']"));
            for (int i = 0; i < element.Count; i++)
            {
                element[i].Click();
                Wait.WaitForDocumentReady();
            }
        }

        public DateTime weekstartdate()
        {
            DayOfWeek day = DateTime.Now.DayOfWeek;
            int days = day - DayOfWeek.Monday;
            return DateTime.Now.Date.AddDays(-days);
        }

        public DateTime weekEndDate()
        {
            DayOfWeek day = DateTime.Now.DayOfWeek;
            int days = day - DayOfWeek.Monday;
            DateTime startDate = DateTime.Now.Date.AddDays(-days);
            return startDate.AddDays(6);
        }

        public bool PatternCodesForMark()
        {

            Queries.GetCodesAvailableOnApplyMarkPage();
            _searchSelectMark.Click();

            var Codes = WebContext.WebDriver.FindElements(By.CssSelector(".select2-results .select2-result-label"));
            var match = false;
            foreach (var codes in Codes)
            {
                foreach (var tt in Queries.GetCodesAvailableOnApplyMarkPage())
                {
                    if (codes.Text.Equals(string.Format("{0} ({1})", tt.Code, tt.Description)))
                    {
                        match = true;
                        break;
                    }
                }

                if (match == false)
                    return false;
            }
            return true;
        }

        public void ClickApplyMarkButton()
        {
            applyMarkButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

    }
}
