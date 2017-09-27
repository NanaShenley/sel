using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;

namespace POM.Components.Attendance
{
    public class DealWithChosenSearchPage : SearchTableCriteriaComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("search_criteria"); }
        }

        public DealWithChosenSearchPage(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _searchStartDateTexBox;

        [FindsBy(How = How.Name, Using = "IsDaily")]
        private IList<IWebElement> _registerViews;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='N']")]
        private IWebElement _basicCodeN;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='-']")]
        private IWebElement _basicCodeMissing;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='control_checkboxlist_rootnode_checkbox_Other_Codes']")]
        private IWebElement _otherCodeCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Other Codes']")]
        private IWebElement _otherCodeHeader;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='control_checkboxlist_rootnode_checkbox_Class']")]
        private IWebElement _classCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Class']")]
        private IWebElement _classGroupHeader;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='control_checkboxlist_rootnode_checkbox_Year_Group']")]
        private IWebElement _yearGroupCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Year Group']")]
        private IWebElement _yearGroupHeader;

        public string StartDate
        {
            set
            {
                _searchStartDateTexBox.SetDateTime(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get { return _searchStartDateTexBox.GetDateTime(); }
        }

        public bool Day
        {
            set
            {
                _registerViews[0].Set(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public bool Week
        {
            set
            {
                _registerViews[1].Set(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public bool CodeN
        {
            set { _basicCodeN.Set(value); }
            get { return _basicCodeN.IsChecked(); }
        }

        public bool CodeMissing
        {
            set { _basicCodeMissing.Set(value); }
            get { return _basicCodeMissing.IsChecked(); }
        }

        public bool AllOtherCode
        {
            set { _otherCodeCheckbox.Set(value); }
            get { return _otherCodeCheckbox.IsChecked(); }
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

        public string OtherCode
        {
            set
            {
                string[] values = value.Trim().Split(',');
                List<string> codeParams = new List<string>(values);
                if (codeParams.Contains("All"))
                {
                    // Select all
                    _otherCodeCheckbox.Set(true);
                }
                else
                {
                    _otherCodeCheckbox.Set(false);

                    // Get Id of header
                    string headerId = _otherCodeCheckbox.GetAttribute("id");
                    string listId = string.Format("[data-parent-checkbox='{0}']", "#" + headerId);

                    // Check collapse
                    if (_otherCodeHeader.IsCollapsed())
                    {
                        _otherCodeHeader.Click();
                        Wait.WaitForElementDisplayed(By.CssSelector(listId));
                    }

                    // Find Checkbox list
                    IWebElement checkboxPanel = SeleniumHelper.FindElement(By.CssSelector(listId));
                    checkboxPanel.ScrollToByAction();
                    IList<IWebElement> checkboxList = checkboxPanel.FindElements(By.CssSelector(".checkboxlist-column"));

                    // Check
                    foreach (var item in checkboxList)
                    {
                        if (codeParams.Count == 0)
                        {
                            break;
                        }

                        string code = item.GetText().Trim();
                        if (codeParams.Contains(code))
                        {
                            item.FindElement(By.Name("OtherCodes.SelectedIds")).Set(true);
                            codeParams.Remove(code);
                        }
                    }
                }
            }
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

        #endregion
    }
}
