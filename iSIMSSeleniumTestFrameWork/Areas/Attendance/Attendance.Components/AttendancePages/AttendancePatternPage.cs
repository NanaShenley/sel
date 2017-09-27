using Attendance.Components.Common;
using POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using WebDriverRunner.webdriver;
using SeSugar.Data;
using Attendance.POM.Entities;
using Attendance.POM.DataHelper;

namespace Attendance.Components.AttendancePages
{
    public class AttendancePatternPage : BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "task_menu_section_attendance_AttendancePattern-palette-editor")]
        public IWebElement AttendancePatternDialog;
        [FindsBy(How = How.Name, Using = "PreserveOverwrite")]
        public readonly IWebElement RadioButtons;
        [FindsBy(How = How.Name, Using = "StartDate")]
        public readonly IWebElement StartDate;
        [FindsBy(How = How.Name, Using = "EndDate")]
        public readonly IWebElement EndDate;
        [FindsBy(How = How.Id, Using = "select2-chosen-23")]
        public readonly IWebElement _thisweek;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='apply_pattern_button']")]
        public readonly IWebElement ApplyPattern;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='close_button']")]
        public IWebElement closeButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Selected Pupils*']")]
        public readonly IWebElement SelectedPupilSection;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pupils_button']")]
        public readonly IWebElement AddPupilLink;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='remove_button']")]
        public readonly IWebElement trashIcon;
        [FindsBy(How = How.CssSelector, Using = ".webix_table_checkbox")]
        public readonly IWebElement PupilGridCheckBox;
        [FindsBy(How = How.Name, Using = "AttendancePatternLearner")]
        public readonly IWebElement attendancePatternLearnerHeader;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Date_Range_Dropdown']")]
        public readonly IWebElement SelectDateRangeButtonDefaultValue;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        public readonly IWebElement ValidationWarning;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='confirmation_required_dialog']")]
        public readonly IWebElement Confirmationpopup;
        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        public IWebElement academicYearDropdown;


        private const string ConfirmationPopupDialogOkButton = "ok_button";

        public AttendancePatternPage()

        {
            PageFactory.InitElements(WebContext.WebDriver, this);
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

        public AttendanceSearchPanel ClickAddPupilLink()
        
        {
            WaitForElement(AttendanceElements.AddPupilPopUpElements.AddPupilLink);
            Retry.Do(AddPupilLink.ClickByAction);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AttendanceSearchPanel();
        }

        public void RemovePupilFromGrid()
        {
            //WaitForElement(AttendanceElements.AddPupilPopUpElements.AddPupilLink);
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


        public void ClickApplyPatternButton()
        {
            ApplyPattern.Click();
            Wait.WaitForDocumentReady();
            Wait.WaitLoading();
        }

        public bool IsConfirmationPopUpDisplayed()
        {
            WaitUntilDisplayed(AttendanceElements.AttendancePatternElements.Confirmationpopup);
            return WebContext.WebDriver.FindElement(AttendanceElements.AttendancePatternElements.Confirmationpopup).Displayed;
        }

        public void ClickCloseButton()
        {
            closeButton.Click();
        }

        public void SelectTerm(string p)
        {
            WaitForElement(By.CssSelector("[data-automation-id='" + p + "']"));
            TermDropDown(p).Click();
        }

        private IWebElement TermDropDown(string p)
        {
            return WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='" + p + "']"));
        }

        public void SelectCodeFromPatternDropdown()
        {
            var loc = By.CssSelector(".select2-arrow");
            WaitForElement(loc);
            var arrow = WebContext.WebDriver.FindElements(loc);
            arrow[1].Click();

            var Codes = WebContext.WebDriver.FindElements(By.CssSelector(".select2-results .select2-result-label"));
            Codes[1].Click();
            WaitForElement(AttendanceElements.AddPupilPopUpElements.AddPupilLink);
        }

        public bool PatternCodesForAMSession()
        {
            Queries.GetCodesAvailableOnAttendancePatternPage();         
            var arrow = WebContext.WebDriver.FindElements(By.CssSelector(".select2-arrow"));
            arrow[1].Click();

            var Codes = WebContext.WebDriver.FindElements(By.CssSelector(".select2-results .select2-result-label"));
            var match = false;
            foreach (var code in Codes)
            {
                //match = false;
                foreach (var ss in Queries.GetCodesAvailableOnAttendancePatternPage())
                {
                    if (code.Text.Equals(ss.Code))
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

        public bool LinksAbsent()
        {
            IReadOnlyCollection<IWebElement> addPupilLink = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='add_pupils_button']"));
            IReadOnlyCollection<IWebElement> removePupilLink = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='remove_pupils_button']"));
            bool val = (addPupilLink.Count == 0 && removePupilLink.Count==0);
            return val;
        }
    }
}
     