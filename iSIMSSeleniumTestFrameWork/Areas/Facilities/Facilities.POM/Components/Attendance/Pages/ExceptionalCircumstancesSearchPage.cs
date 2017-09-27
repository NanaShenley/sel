using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;

namespace POM.Components.Attendance
{
    public class ExceptionalCircumstancesSearchPage : SearchCriteriaComponent<ExceptionalCircumstancesTriplet.ExceptionalCircumstancesSearchResultTile>
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("search_criteria"); }
        }

        public ExceptionalCircumstancesSearchPage(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _searchStartDateTexBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _searchEndDateTexBox;

        public string StartDate
        {
            set
            {
                _searchStartDateTexBox.SetDateTime(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public string EndDate
        {
            set
            {
                _searchEndDateTexBox.SetDateTime(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }


        #endregion
    }
}
