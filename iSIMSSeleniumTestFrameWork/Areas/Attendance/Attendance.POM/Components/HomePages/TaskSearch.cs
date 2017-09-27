using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POM.Components.HomePages
{
    public class TaskSearch : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='task-menu-search']"); }
        }

        #region Properties

        [FindsBy(How = How.Id, Using = "task-menu-search")]
        private IWebElement _searchTextBox;

        [FindsBy(How = How.CssSelector, Using = ".task-menu-suggestion")]
        private IList<IWebElement> _suggestionList;

        public string Search
        {
            set
            {
                _searchTextBox.Clear();
                _searchTextBox.Type(value);
                Refresh();
            }
            get { return _searchTextBox.GetValue(); }
        }

        public ListItemComponent<SearchResultItem> SearchResult
        {
            get { return new ListItemComponent<SearchResultItem>(By.CssSelector(".task-menu-suggestion")); }
        }

        #endregion

        public class SearchResultItem
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id^='Task_Menu_Search_Result_link']")]
            private IWebElement _navigateLink;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id^='Task_Menu_Search_Result_area']")]
            private IWebElement _functionalArea;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id^='Task_Menu_Search_Result_area'] .task-menu-highlight")]
            private IWebElement _functionalAreaHighlight;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id^='Task_Menu_Search_Result_title']")]
            private IWebElement _taskAction;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id^='Task_Menu_Search_Result_title'] .task-menu-highlight")]
            private IWebElement _taskActionHighlight;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id^='Task_Menu_Search_Result_description'] .task-icon")]
            private IWebElement _typeActionIcon;

            public string FuntionalArea
            {
                get { return _functionalArea.GetText().Trim(); }
            }

            public string TaskAction
            {
                get { return _taskAction.GetText().Trim(); }
            }

            public string FuntionalAreaHighlight
            {
                get { return _functionalAreaHighlight.GetText().Trim(); }
            }

            public string TaskActionHighlight
            {
                get { return _taskActionHighlight.GetText().Trim(); }
            }

            public bool IsTask()
            {
                return _typeActionIcon.GetAttribute("class").Contains("fa-rocket");
            }

            public bool IsLookUp()
            {
                return _typeActionIcon.GetAttribute("class").Contains("fa-list");
            }

            public void Click()
            {
                _navigateLink.Click() ;
                Wait.WaitForAjaxReady(By.Id("nprogress"));
            }
        }
    }
}
