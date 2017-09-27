using POM.Base;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Common;

namespace Facilities.POM.Components.SchoolManagement.Page
{
    public class NewAcademicYearDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='next_button']")]
        private IWebElement _nextButton;
        
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='finish_button']")]
        private IWebElement _finishButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_school_terms_button']")]
        private IWebElement _addSchoolTermLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_public_holidays_button']")]
        private IWebElement _addPublicHolidayLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_inset_days_button']")]
        private IWebElement _addInsetLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_half_term_holiday_button']")]
        private IWebElement _addHalfTermHolidayLink;

        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");


        #endregion

        #region Action

        public void Next()
        {
            _nextButton.ClickByJS();
        }

        public void Finish()
        {
            _finishButton.ClickByJS();
        }

        #endregion

        public void ClickAddSchoolTermLink()
        {
            _addSchoolTermLink.ClickByJS();
            _addSchoolTermLink.WaitUntilState(ElementState.Enabled);
        }

        public void ClickHalfTermHolidayLink()
        {
            _addHalfTermHolidayLink.ClickByJS();
            _addHalfTermHolidayLink.WaitUntilState(ElementState.Enabled);
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

        public void DeleteRowIfExisted(GridRow row)
        {
            if (row != null)
            {
                row.DeleteRow();
            }
        }

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

        public GridComponent<HalfTermHoliday> HalfTermHolidayTable
        {
            get
            {
                GridComponent<HalfTermHoliday> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<HalfTermHoliday>(By.CssSelector("[data-maintenance-container='HalfTermHolidays']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<HalfTermName> HalfTermNameTable
        {
            get
            {
                GridComponent<HalfTermName> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<HalfTermName>(By.CssSelector("[data-maintenance-container='HalfTerms']"), ComponentIdentifier);
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

            [FindsBy(How = How.CssSelector, Using = "[name$='TermHolidayName']")]
            private IWebElement _holidayNameField;

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
            public string HolidayName
            {
                set
                {
                    _holidayNameField.SetText(value);
                    _holidayNameField.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _holidayNameField.GetAttribute("value");
                }
            }
        }

        public class HalfTermHoliday : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='Name']")]
            private IWebElement _halfTermHolidayName;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _halfTermStarDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _halfTermEndDate;

            public string HalfTermHolidayName
            {
                set
                {
                    _halfTermHolidayName.SetText(value);
                    _halfTermHolidayName.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _halfTermHolidayName.GetAttribute("value");
                }
            }

            public string HalfTermStartDate
            {
                set
                {
                    _halfTermStarDate.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _halfTermStarDate.GetDateTime();
                }
            }

            public string HalfTermEndDate
            {
                set
                {
                    _halfTermEndDate.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _halfTermEndDate.GetDateTime();
                }
            }
        }

        public class HalfTermName : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='Name']")]
            private IWebElement _halfTermName;

            public string HalfTermFullName
            {
                set
                {
                    _halfTermName.SetText(value);
                    _halfTermName.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _halfTermName.GetAttribute("value");
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
        #endregion
    }
}
