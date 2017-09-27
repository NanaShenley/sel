using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;
using POM.Components.Common;

using POM.Components.HomePages;

namespace POM.Components.SchoolManagement
{
    public class AcademicYearDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return By.CssSelector(".layout-two-column-detail"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _name;

        [FindsBy(How = How.Name, Using = "ReferenceAcademicYear.dropdownImitator")]
        private IWebElement _assessmentYear;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_school_terms_button']")]
        private IWebElement _addSchoolTermLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='calculate_school_holidays_jobstep_button']")]
        private IWebElement _calculateButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_public_holidays_button']")]
        private IWebElement _addPublicHolidayLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_inset_days_button']")]
        private IWebElement _addInsetLink;

        [FindsBy(How = How.Name, Using = "WeekDay.dropdownImitator")]
        private IWebElement _workingWeekDropDown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pattern_button']")]
        private IWebElement _addPatternsButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_school_holidays_button']")]
        private IWebElement _addSchoolHolidayLink;


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_School Terms']")]
        private IWebElement _schoolTermSection;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Working Week']")]
        private IWebElement _workingWeekAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Holidays']")]
        private IWebElement _holidaySection;

        public string Name
        {
            get { return _name.GetValue(); }
            set { _name.SetText(value); }
        }

        public string AssessmentYear
        {

            get { return _assessmentYear.GetValue(); }
            set { _assessmentYear.EnterForDropDown(value); }
        }

        public string FirstDayOfWorkingWeek
        {
            set { _workingWeekDropDown.EnterForDropDown(value); }
            get { return _workingWeekDropDown.GetValue(); }
        }

        #endregion

        #region Actions

        public AcademicYearDetailPage CreateAcademicYear()
        {
            if (_createButton.IsExist())
            {
                _createButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            return new AcademicYearDetailPage();
        }

        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool IsSuccessMessageDisplay()
        {
            return _messageSuccess.IsExist();
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                var confirmDeleteDialog = new DeleteConfirmationDialog();
                confirmDeleteDialog.ConfirmDelete();
            }
        }

        public void ClickAddSchoolTermLink()
        {
            _addSchoolTermLink.ClickByJS();
            _addSchoolTermLink.WaitUntilState(ElementState.Enabled);
        }

        public void ClickCalculateSchoolHoliday()
        {
            _calculateButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
        }

        public void ClickAddPublicHolidayLink()
        {
            _addPublicHolidayLink.ClickByJS();
            _addPublicHolidayLink.WaitUntilState(ElementState.Enabled);
        }

        public void ClickInsetDayLink()
        {
            _addInsetLink.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
        }

        public void ClickAddSchoolHolidayLink()
        {

            _addSchoolHolidayLink.ClickByJS();
            _addSchoolHolidayLink.WaitUntilState(ElementState.Enabled);
        }

        public void ScrollToSchoolTerms()
        {
            if (_schoolTermSection.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _schoolTermSection.ClickByJS();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-automation-id='add_school_terms_button']"));
        }

        public void ScrollToWorkingWeek()
        {
            if (_workingWeekAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _workingWeekAccordion.ClickByJS();
            }
            else
            {
                _workingWeekAccordion.ClickByJS();
                Wait.WaitLoading();
                _workingWeekAccordion.ClickByJS();
            }
            Wait.WaitForElementDisplayed(By.Name("WeekDay.dropdownImitator"));
        }

        public void ScrollToHoliday()
        {
            if (_holidaySection.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _holidaySection.ClickByJS();
            }
            else
            {
                _holidaySection.ClickByJS();
                Wait.WaitLoading();
                _holidaySection.ClickByJS();
            }
            Wait.WaitForElementDisplayed(By.Name("CalculateSchoolHolidays"));
        }

        public void DeleteRowIfExisted(GridRow row)
        {
            if (row != null)
            {
                row.DeleteRow();
            }
        }

        #endregion

        #region Grid

        public GridComponent<SchoolRow> SchoolTermsTable
        {
            get
            {
                GridComponent<SchoolRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SchoolRow>(By.CssSelector("[data-maintenance-container='SchoolTerms']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<SchoolRow> SchoolHolidayTable
        {
            get
            {
                GridComponent<SchoolRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SchoolRow>(By.CssSelector("[data-maintenance-container='SchoolHolidays']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<PublicHolidayRow> PublicHolidayTable
        {
            get
            {
                GridComponent<PublicHolidayRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PublicHolidayRow>(By.CssSelector("[data-maintenance-container='PublicHolidays']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<InsetRow> InsetDayTable
        {
            get
            {
                GridComponent<InsetRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<InsetRow>(By.CssSelector("[data-maintenance-container='InsetDays']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<WorkingWeek> WorkingWeekGrid
        {
            get
            {
                GridComponent<WorkingWeek> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<WorkingWeek>(By.CssSelector("[data-maintenance-container='WorkingWeekPatterns']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<Patterns> PatternsGrid
        {
            get
            {
                GridComponent<Patterns> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Patterns>(By.CssSelector("[data-maintenance-container='Patterns']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class Patterns
        {

            [FindsBy(How = How.CssSelector, Using = "[id$='__WorkingWeekSession_dropdownImitator']")]
            private IWebElement _nameTextBox;

            [FindsBy(How = How.CssSelector, Using = "[id$='Day1']")]
            private IWebElement _dayStartTimeTextBox;

            [FindsBy(How = How.CssSelector, Using = "[id$='Day2']")]
            private IWebElement _dayEndTimeTextBox;

            public string Name
            {
                set { _nameTextBox.SetText(value); }
                get { return _nameTextBox.GetValue(); }
            }

            public string DayStartTime
            {
                set { _dayStartTimeTextBox.SetText(value); }
                get { return _dayStartTimeTextBox.GetValue(); }
            }

            public string DayEndTime
            {
                set { _dayEndTimeTextBox.SetText(value); }
                get { return _dayEndTimeTextBox.GetValue(); }
            }

        }

        public class SchoolRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Name']")]
            private IWebElement _nameField;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _blankCell;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateField;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateField;

            public string Name
            {
                set
                {
                    _nameField.SetText(value);
                    _nameField.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _nameField.GetAttribute("value");
                }
            }

            public string StartDate
            {
                set
                {
                    _startDateField.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _startDateField.GetDateTime();
                }
            }

            public string EndDate
            {
                set
                {
                    _endDateField.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _endDateField.GetDateTime();
                }
            }


        }

        public class PublicHolidayRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Name']")]
            private IWebElement _nameField;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _blankCell;

            [FindsBy(How = How.CssSelector, Using = "[name$='Date']")]
            private IWebElement _startDateField;

            public string Name
            {
                set
                {
                    _nameField.SetText(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _nameField.GetAttribute("value");
                }
            }

            public string Date
            {
                set
                {
                    _startDateField.SetDateTime(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _startDateField.GetDateTime();
                }
            }
        }

        public class InsetRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Name']")]
            private IWebElement _nameField;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _blankCell;

            [FindsBy(How = How.CssSelector, Using = "[name$='Date']")]
            private IWebElement _dateField;

            [FindsBy(How = How.CssSelector, Using = "[name$='AMSession']")]
            private IWebElement _AMSession;

            [FindsBy(How = How.CssSelector, Using = "[name$='PMSession']")]
            private IWebElement _PMSession;

            public string Name
            {
                set
                {
                    _nameField.Click();
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    _nameField.SetText(value);
                    _nameField.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _nameField.GetAttribute("value");
                }
            }

            public string Date
            {
                set
                {
                    _dateField.SetDateTime(value);
                    //Retry.Do(_blankCell.Click);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _dateField.GetDateTime();
                }
            }

            public bool AM
            {
                set
                {
                    _AMSession.SetCheckBox(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _AMSession.IsCheckboxChecked();
                }
            }
            public bool PM
            {
                set
                {
                    _PMSession.SetCheckBox(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _PMSession.IsCheckboxChecked();
                }
            }
        }

        public class WorkingWeek
        {

            [FindsBy(How = How.CssSelector, Using = "[id$='__WorkingWeekSession_dropdownImitator']")]
            private IWebElement _workingWeekSession;

            [FindsBy(How = How.CssSelector, Using = "[name$='Day1']")]
            private IWebElement _monCheckBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='Day2']")]
            private IWebElement _tueCheckBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='Day3']")]
            private IWebElement _wedCheckBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='Day4']")]
            private IWebElement _thuCheckBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='Day5']")]
            private IWebElement _friCheckBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='Day6']")]
            private IWebElement _satCheckBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='Day7']")]
            private IWebElement _sunCheckBox;

            public string WorkingWeekSession
            {
                set { _workingWeekSession.EnterForDropDown(value); }
                get { return _workingWeekSession.GetValue(); }
            }

            public bool Monday
            {
                set { _monCheckBox.SetCheckBox(value); }
                get { return _monCheckBox.IsCheckboxChecked(); }
            }

            public bool Tuesday
            {
                set { _tueCheckBox.SetCheckBox(value); }
                get { return _tueCheckBox.IsCheckboxChecked(); }
            }

            public bool Wednesday
            {
                set { _wedCheckBox.SetCheckBox(value); }
                get { return _wedCheckBox.IsCheckboxChecked(); }
            }

            public bool Thurday
            {
                set { _thuCheckBox.SetCheckBox(value); }
                get { return _thuCheckBox.IsCheckboxChecked(); }
            }

            public bool Friday
            {
                set { _friCheckBox.SetCheckBox(value); }
                get { return _friCheckBox.IsCheckboxChecked(); }
            }

            public bool Saturday
            {
                set { _satCheckBox.SetCheckBox(value); }
                get { return _satCheckBox.IsCheckboxChecked(); }
            }

            public bool Sunday
            {
                set { _sunCheckBox.SetCheckBox(value); }
                get { return _sunCheckBox.IsCheckboxChecked(); }
            }

        }

        #endregion
    }
}
