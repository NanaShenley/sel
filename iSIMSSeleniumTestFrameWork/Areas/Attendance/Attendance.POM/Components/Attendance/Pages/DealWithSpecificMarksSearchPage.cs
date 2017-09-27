using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System.Collections.Generic;
using Attendance.POM.DataHelper;
using WebDriverRunner.webdriver;

namespace POM.Components.Attendance
{
    public class DealWithSpecificMarksSearchPage : SearchTableCriteriaComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("search_criteria"); }
        }

        public DealWithSpecificMarksSearchPage(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "StartDate")]
        public IWebElement _searchStartDateTexBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        public IWebElement _searchEndDateTextBox;

        [FindsBy(How = How.Name, Using = "SchoolTerm.dropdownImitator")]
        public IWebElement _dateRangeDropdown;

        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        public IWebElement _searchAcademicYearDropdown;

        [FindsBy(How = How.Name, Using = "Mark.dropdownImitator")]
        public IWebElement _searchSelectMark;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='control_checkboxlist_rootnode_checkbox_Class']")]
        private IWebElement _classCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Class']")]
        private IWebElement _classGroupHeader;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='control_checkboxlist_rootnode_checkbox_Year_Group']")]
        private IWebElement _yearGroupCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Year Group']")]
        private IWebElement _yearGroupHeader;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Whole School']")]
        private IWebElement _wholeSchoolGroupHeader;

        [FindsBy(How = How.CssSelector, Using = ".checkboxlist")]
        private IList<IWebElement> _groupSections;

        public string SelectAcademicYear
        {
            set { _searchAcademicYearDropdown.EnterForDropDown(value); }
            get { return _searchAcademicYearDropdown.GetValue(); }
        }

        public string SelectDateRange
        {
            set { _dateRangeDropdown.EnterForDropDown(value); }
            get { return _dateRangeDropdown.GetValue(); }
        }

        public string StartDate
        {
            set
            {
                _searchStartDateTexBox.SetDateTime(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get { return _searchStartDateTexBox.GetDateTime(); }
        }

        public string EndDate
        {
            set
            {
                _searchEndDateTextBox.SetDateTime(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get { return _searchEndDateTextBox.GetDateTime(); }
        }

        public string SelectMark
        {
            set { _searchSelectMark.EnterForDropDown(value); }
            get { return _searchSelectMark.GetValue(); }
        }

        public bool AllClass
        {
            set { _classCheckbox.Set(value); }
            get { return _classCheckbox.IsChecked(); }
        }

        public bool AllYearGroup
        {
            set { _yearGroupCheckbox.Set(value); }
            get { return _yearGroupCheckbox.IsChecked(); }
        }

        public string Class
        {
            set
            {
                string[] values = value.Trim().Split(',');
                List<string> classParams = new List<string>(values);
                if (classParams.Contains("All"))
                {
                    _classCheckbox.Set(true);
                }
                else
                {
                    _classCheckbox.Set(false);

                    // Get Id of header
                    string headerId = _classCheckbox.GetAttribute("id");
                    string listId = string.Format("[data-parent-checkbox='{0}']", "#" + headerId);

                    // Check collapse
                    if (_classGroupHeader.IsCollapsed())
                    {
                        _classGroupHeader.Click();
                        Wait.WaitForElementDisplayed(By.CssSelector(listId));
                    }

                    // Find Checkbox list
                    IWebElement checkboxPanel = SeleniumHelper.FindElement(By.CssSelector(listId));
                    checkboxPanel.ScrollToByAction();
                    IList<IWebElement> checkboxList = checkboxPanel.FindElements(By.CssSelector(".checkboxlist-column"));

                    // Check
                    foreach (var item in checkboxList)
                    {
                        if (classParams.Count == 0)
                        {
                            break;
                        }

                        string className = item.GetText().Trim();
                        if (classParams.Contains(className))
                        {
                            item.FindElement(By.Name("Classes.SelectedIds")).Set(true);
                            classParams.Remove(className);
                        }
                    }
                }
            }
        }

        public string YearGroup
        {
            set
            {
                string[] values = value.Trim().Split(',');
                List<string> yearGroupParams = new List<string>(values);
                if (yearGroupParams.Contains("All"))
                {
                    _yearGroupCheckbox.Set(true);
                }
                else
                {
                    _yearGroupCheckbox.Set(false);

                    // Get Id of header
                    string headerId = _yearGroupCheckbox.GetAttribute("id");
                    string listId = string.Format("[data-parent-checkbox='{0}']", "#" + headerId);

                    // Check collapse
                    if (_yearGroupHeader.IsCollapsed())
                    {
                        _yearGroupHeader.Click();
                        Wait.WaitForElementDisplayed(By.CssSelector(listId));
                    }

                    // Find Checkbox list
                    IWebElement checkboxPanel = SeleniumHelper.FindElement(By.CssSelector(listId));
                    checkboxPanel.ScrollToByAction();
                    IList<IWebElement> checkboxList = checkboxPanel.FindElements(By.CssSelector(".checkboxlist-column"));

                    // Check
                    foreach (var item in checkboxList)
                    {
                        if (yearGroupParams.Count == 0)
                        {
                            break;
                        }

                        string yearGroup = item.GetText().Trim();
                        if (yearGroupParams.Contains(yearGroup))
                        {
                            item.FindElement(By.Name("YearGroups.SelectedIds")).Set(true);
                            yearGroupParams.Remove(yearGroup);
                        }
                    }
                }
            }
        }
        #region : Methods
        public void SelectWholeSchool()
        {
            _wholeSchoolGroupHeader.ClickByJS();
        }

        public void SelectYearGroup(string yearGroupName)
        {
            if (_yearGroupHeader.IsCollapsed())
            {
                _yearGroupHeader.Click();
            }

            // Get Id of header
            string headerId = _yearGroupCheckbox.GetAttribute("id");
            string listId = string.Format("[data-parent-checkbox='{0}']", "#" + headerId);
            var yearGroup = SeleniumHelper.FindElement(By.CssSelector(listId));

            IList<IWebElement> yearGroupLabels = yearGroup.FindElements(By.CssSelector(".checkboxlist-column label"));
            for (int i = 0; i < yearGroupLabels.Count; i++)
            {
                if (yearGroupLabels[i].GetText().Equals(yearGroupName))
                {
                    IWebElement input = yearGroup.FindElements(By.CssSelector(".checkboxlist-column label"))[i];
                    input.Set(true);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    break;
                }
            }
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public bool GetAllMarks()
        {
            Queries.GetCodesAvailableOnDealWithSpecificMarks();
            _searchSelectMark.Click();
            var Codes = WebContext.WebDriver.FindElements(By.CssSelector(".select2-results .select2-result-label"));

            var match = false;
            foreach (var codes in Codes)
            {
                foreach (var tt in Queries.GetCodesAvailableOnDealWithSpecificMarks())
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

        public bool VerifySpecificCodeInDropdown(string codeToFind)
        {
            _searchSelectMark.Click();
            var Codes = WebContext.WebDriver.FindElements(By.CssSelector(".select2-results .select2-result-label"));

            var match = false;
            foreach (var code in Codes)
            {
                if (code.Text.Contains(codeToFind))
                {
                    match = true;
                    break;
                }
            }

            return match;
        }
        #endregion

        #endregion
    }
}
