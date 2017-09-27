using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Collections.Generic;
using POM.Components.Pupil;
using WebDriverRunner.webdriver;

namespace POM.Components.Attendance
{
    public class EarlyYearsSessionPatternDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("task_menu_section_attendance_EarlyYear-palette-editor"); }
        }

        #region

        [FindsBy(How = How.Name, Using = "StartDate")]
        public IWebElement _startDateTextbox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        public IWebElement _endDateTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='apply_button']")]
        private IWebElement _applyButton;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='close_button']"))]
        private IWebElement _closeButton;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='add_pupils_button']"))]
        private IWebElement _addPupil;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='remove_pupils_button']"))]
        private IWebElement _removePupil;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='remove_button']"))]
        private IWebElement _deletePupil;

        [FindsBy(How = How.CssSelector, Using = ("[view_id='cxgridAttendancePatternLearner'] input"))]
        public IWebElement _selectCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[view_id='cxgridAttendancePatternLearner'] .webix_ss_body")]
        private IWebElement _selectPupilTable;

        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        private IWebElement _academicYearDropdown;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='Date_Range_Dropdown']"))]
        public IWebElement SelectDateRangeButtonDefaultValue;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = ".inline.alert.alert-warning")]
        private IWebElement _validationMessage;

        public string AcademicYear
        {
            set { _academicYearDropdown.EnterForDropDown(value); }
            get { return _academicYearDropdown.GetValue(); }
        }

        public string StartDate
        {
            set { _startDateTextbox.SetDateTime(value); }
        }

        public string EndDate
        {
            set { _endDateTextbox.SetDateTime(value); }
        }

        public bool SelectPupil
        {
            set { _selectCheckBox.Set(value); }
            get { return _selectCheckBox.IsChecked(); }
        }

        public WebixComponent<WebixCell> SelectPupilsTable
        {
            get { return new WebixComponent<WebixCell>(_selectPupilTable); }
        }

        public bool IsPreserve
        {
            set
            {
                string javascript = "document.getElementsByName('PreserveOverwrite')[0].checked = '{0}';";
                SeleniumHelper.ExecuteJavascript(String.Format(javascript, value));
            }
        }

        public bool IsOverwrite
        {
            set
            {
                string javascript = "document.getElementsByName('PreserveOverwrite')[1].checked = '{0}';";
                SeleniumHelper.ExecuteJavascript(String.Format(javascript, value));
            }
        }

        #endregion


        #region Page actions

        public POM.Components.Common.ConfirmRequiredDialog ClickApply()
        {
            _applyButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new POM.Components.Common.ConfirmRequiredDialog();
        }

        public void ApplyPattern()
        {
            _applyButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        public AddPupilsDialogTriplet AddPupil()
        {
            _addPupil.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddPupilsDialogTriplet();
        }

        public AddPupilsDialogTriplet RemovePupil()
        {
            _removePupil.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddPupilsDialogTriplet();
        }

        public PupilRecordPage ClickClose()
        {
            _closeButton.Click();

            return new PupilRecordPage();

        }
        public void ClosePatternDialog()
        {
            _closeButton.Click();
        }

        public void DeletePupil()
        {
            var element = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='remove_button']"));
            for (int i = 0; i < element.Count; i++)
            {
                element[i].Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
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

        public bool IsSuccessMessageDisplayed()
        {
            return _successMessage.IsExist();
        }

        #endregion

        public class AllocateSessionGrid : WebixComponent<WebixCell>
        {
            public AllocateSessionGrid(IWebElement _webElement)
                : base(_webElement)
            { }

            public override By ComponentIdentifier
            {
                get { return SimsBy.Id("AttendancePatternLearner"); }
            }

            #region Properties

            public IList<Column> Columns
            {
                get { return GetColumns(); }
            }

            #endregion

            #region Actions

            public IList<Column> GetColumns()
            {
                IList<Column> lstColumns = new List<Column>();
                IList<IWebElement> sectionElements = tableElement.FindElements(By.CssSelector(".webix_ss_header .webix_hs_left [section='header']"));
                lstColumns.Add(new Column(sectionElements[1], null, null));

                sectionElements = tableElement.FindElements(By.CssSelector(".webix_ss_header .webix_hs_center [section='header']"));
                IList<IWebElement> lstHeaderElements = sectionElements[0].FindElements(By.CssSelector("td"));
                IList<IWebElement> lstActionElements = sectionElements[1].FindElements(By.CssSelector("td"));
                int j = 0;

                for (int i = 0; i < lstHeaderElements.Count; i++)
                {
                    lstColumns.Add(new Column(lstHeaderElements[i], lstActionElements[j], lstActionElements[j + 1]));
                    j = j + 2;
                }
                return lstColumns;
            }

            #endregion
        }

        public class Column
        {
            IWebElement headerElement;
            IWebElement am;
            IWebElement pm;

            public Column(IWebElement _headerElement, IWebElement _am, IWebElement _pm)
            {
                headerElement = _headerElement;
                am = _am;
                pm = _pm;
            }

            public string HeaderText
            {
                get { return headerElement.GetText(); }
            }

            public string TimeIndicatorSelected
            {
                set
                {
                    if (value.Equals("AM"))
                    {

                        am.Click();
                    }
                    else
                    {

                        pm.Click();
                    }
                    Wait.WaitLoading();
                }
            }
        }
    }
}
