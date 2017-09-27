using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System.Collections.Generic;

namespace POM.Components.Attendance
{
    public class TakeRegisterSearchPage : SearchTableCriteriaComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("search_criteria"); }
        }

        public TakeRegisterSearchPage(BaseComponent parent) : base(parent) { }

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
        private IWebElement _allClass;

        public bool AllClass
        {
            set
            {
                if(value)
                {
                    while (_allClass.GetAttribute("checked") != "true")
                    {
                        _allClass.ClickByJS();
                    }
                }
                else
                {
                    while (_allClass.GetAttribute("checked") == "true")
                    {
                        _allClass.ClickByJS();
                    }
                }
            }
            get
            {
                return _allClass.IsCheckboxChecked();
            }
        }

        public string StartDate
        {
            set
            {
                _searchStartDateTexBox.SetDateTime(value);
                _searchStartDateTexBox.SendKeys(Keys.Enter);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            get
            {
                return _searchStartDateTexBox.GetDateTime();
            }
        }

        public bool Day
        {
            set
            {
                _registerViews[0].Set(value);
                //Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
        }

        public bool Week
        {
            set
            {
                _registerViews[1].Set(value);
                //Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
        }

        public string Class
        {
            set
            {
                if (_classGroupHeader.IsCollapsed())
                {
                    _classGroupHeader.Click();
                }
                IWebElement classGroup = _groupSections[0];
                IList<IWebElement> classLabels = classGroup.FindElements(By.CssSelector(".checkboxlist-column label"));
                for (int i = 0; i < classLabels.Count; i++)
                {
                    if (classLabels[i].GetText().Equals(value))
                    {
                        IWebElement input = classGroup.FindElements(By.CssSelector(".checkboxlist-column label"))[i];
                        input.Set(true);
                        Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                        break;
                    }
                }
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
        }

        public string YearGroup
        {
            set
            {
                if (_yearGroupHeader.IsCollapsed())
                {
                    _yearGroupHeader.ClickByJS();
                }
                IWebElement yearGroup = _groupSections[1];
                IList<IWebElement> yearGroupLabels = yearGroup.FindElements(By.CssSelector(".checkboxlist-column label"));
                for (int i = 0; i < yearGroupLabels.Count; i++)
                {
                    if (yearGroupLabels[i].GetText().Equals(value))
                    {
                        IWebElement input = yearGroup.FindElements(By.CssSelector(".checkboxlist-column label"))[i];
                        input.Set(true);
                        Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                        break;
                    }
                }
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
        }

        #endregion
    }
}
