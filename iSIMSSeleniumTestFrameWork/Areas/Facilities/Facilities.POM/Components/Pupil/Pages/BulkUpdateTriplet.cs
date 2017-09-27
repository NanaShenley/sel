using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;

namespace POM.Components.Pupil
{
    public class BulkUpdateTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public BulkUpdateTriplet()
        {
            _searchCriteria = new BulkSearch(this);
        }

        #region Search

        private readonly BulkSearch _searchCriteria;
        public BulkSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Actions
        public static BulkUpdateTriplet SelectBasicDetails()
        {
            IWebElement basicDetailButton = SeleniumHelper.Get(SimsBy.AutomationId("bulk_update_sub_menu_pupil_basic_details"));
            basicDetailButton.Click();
            Wait.WaitForControl(SimsBy.AutomationId("search_criteria"));
            return new BulkUpdateTriplet();
        }
        #endregion
    }

    public class BulkSearch : SearchTableCriteriaComponent
    {
        public BulkSearch(BaseComponent parent) : base(parent) { }

        #region Search Properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Class']")]
        private IWebElement _classGroupHeader;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Year Group']")]
        private IWebElement _yearGroupHeader;

        [FindsBy(How = How.CssSelector, Using = ".checkboxlist")]
        private IList<IWebElement> _groupSections;
        #endregion

        #region Search Actions

        public void Selector(string group, string value)
        {
            IWebElement groupElement;

            if (_classGroupHeader.IsCollapsed())
            {
                _classGroupHeader.Click();
            }
            
            if(group.Equals("Year Group"))
            {
                groupElement = _groupSections[0];
            }
            else
            {
                groupElement = _groupSections[1];
            }
            IList<IWebElement> classLabels = groupElement.FindElements(By.CssSelector(".checkboxlist-column label"));
            for (int i = 0; i < classLabels.Count; i++)
            {
                if (classLabels[i].GetText().Equals(value))
                {
                    IWebElement input = groupElement.FindElements(By.CssSelector(".checkboxlist-column label"))[i];
                    input.Set(true);
                    break;
                }
            }
        }

        #endregion
    }
}
