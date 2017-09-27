using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;

namespace POM.Components.Attendance
{
    public class EditMarksSearchPage : SearchTableCriteriaComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("search_criteria"); }
        }

        public EditMarksSearchPage(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _searchStartDateTexBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        private IWebElement _searchButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Class']")]
        private IWebElement _classGroupHeader;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Year Group']")]
        private IWebElement _yearGroupHeader;

        [FindsBy(How = How.Name, Using = "IsDaily")]
        private IList<IWebElement> _registerViews;

        [FindsBy(How = How.CssSelector, Using = ".checkboxlist")]
        private IList<IWebElement> _groupSections;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='control_checkboxlist_rootnode_checkbox_Class']")]
        private IWebElement _classCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='control_checkboxlist_rootnode_checkbox_Year_Group']")]
        private IWebElement _yearGroupCheckbox;

        public string StartDate
        {
            set
            {
                _searchStartDateTexBox.SetDateTime(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
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

        public void SelectClass(string className)
        {
            if (_classGroupHeader.IsCollapsed())
            {
                _classGroupHeader.Click();
            }
            IWebElement classGroup = _groupSections[0];
            IList<IWebElement> classLabels = classGroup.FindElements(By.CssSelector(".checkboxlist-column label"));
            for (int i = 0; i < classLabels.Count; i++)
            {
                if (classLabels[i].GetText().Equals(className))
                {
                    IWebElement input = classGroup.FindElements(By.CssSelector(".checkboxlist-column label"))[i];
                    input.Set(true);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    break;
                }
            }
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public void SelectYearGroup(string yearGroupName)
        {
            if (_yearGroupHeader.IsCollapsed())
            {
                _yearGroupHeader.Click();
            }
            IWebElement yearGroup = _groupSections[1];
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

        public CheckBoxGroupElement Class
        {
            get
            {
                // Get Id of header
                string headerId = _classCheckbox.GetAttribute("id");
                string listId = string.Format("[data-parent-checkbox='{0}']", "#" + headerId);

                // Check collapse
                if (_classGroupHeader.IsCollapsed())
                {
                    _classGroupHeader.ClickByJS();
                    Wait.WaitForElementDisplayed(By.CssSelector(listId));
                }

                var classPanel = SeleniumHelper.FindElement(By.CssSelector(listId));

                return new CheckBoxGroupElement(classPanel, _classCheckbox);
            }
        }

        public CheckBoxGroupElement YearGroups
        {
            get
            {

                // Get Id of header
                string headerId = _yearGroupCheckbox.GetAttribute("id");
                string listId = string.Format("[data-parent-checkbox='{0}']", "#" + headerId);

                // Check collapse
                if (_yearGroupHeader.IsCollapsed())
                {
                    _yearGroupHeader.ClickByJS();
                    Wait.WaitForElementDisplayed(By.CssSelector(listId));
                }

                var yearGroupPanel = SeleniumHelper.FindElement(By.CssSelector(listId));

                return new CheckBoxGroupElement(yearGroupPanel, _yearGroupCheckbox);
            }
        }


        #endregion
    }
}
