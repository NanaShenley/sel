using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class ApplySalaryUpdateSeachPage : SearchTableCriteriaComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("annual_increment_double"); }
        }

        public ApplySalaryUpdateSeachPage(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[title = 'Service Term'] + div")]
        private IWebElement _searchTermDropDown;

        [FindsBy(How = How.CssSelector, Using = "[title = 'Award Month'] + div")]
        private IWebElement _awardMonthDropDown;

        [FindsBy(How = How.CssSelector, Using = "[title = 'Award Year'] + div")]
        private IWebElement _awardYearDropDown;

        public string SearchTerm
        {
            set { _searchTermDropDown.EnterForDropDown(value); }
            get { return _searchTermDropDown.GetSelectedComboboxItemText(); }
        }

        public string AwardMonth
        {
            set { _awardMonthDropDown.EnterForDropDown(value); }
            get { return _awardMonthDropDown.GetSelectedComboboxItemText(); }
        }

        public string AwardYear
        {
            set { _awardYearDropDown.EnterForDropDown(value); }
            get { return _awardYearDropDown.GetSelectedComboboxItemText(); }
        }

        #endregion
    }
}
