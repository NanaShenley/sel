using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;


namespace POM.Components.SchoolGroups
{
    public class AllocatePupilsToGroupsTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("allocate_future_pupil_main_id"); }
        }

        public AllocatePupilsToGroupsTriplet()
        {
            _searchCriteria = new AllocatePupilsToGroupsSearchPage(this);
        }

        #region Search

        private readonly AllocatePupilsToGroupsSearchPage _searchCriteria;

        public AllocatePupilsToGroupsSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion

    }

    public class AllocatePupilsToGroupsSearchPage : SearchTableCriteriaComponent
    {
        public AllocatePupilsToGroupsSearchPage(BaseComponent component) : base(component) { }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='control_checkboxlist_rootnode_checkbox_Classes']")]
        private IWebElement _classCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Classes']")]
        private IWebElement _classGroupHeader;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='control_checkboxlist_rootnode_checkbox_Year_Group']")]
        private IWebElement _yearGroupCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Year Group']")]
        private IWebElement _yearGroupHeader;

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

        public CheckBoxGroupElement Classes
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

        #endregion

    }

}
