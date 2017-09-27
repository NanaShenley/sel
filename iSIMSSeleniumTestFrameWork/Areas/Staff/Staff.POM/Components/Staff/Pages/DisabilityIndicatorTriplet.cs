using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class DisabilityIndicatorTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public DisabilityIndicatorTriplet()
        {
            _searchCriteria = new DisabilityIndicatorSearch(this);
        }

        #region Search

        private readonly DisabilityIndicatorSearch _searchCriteria;
        public DisabilityIndicatorSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion
    }

    public class DisabilityIndicatorSearch : SearchTableCriteriaComponent
    {
        public DisabilityIndicatorSearch(BaseComponent parent) : base(parent) { }

        #region Properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _searchTextBox;

        public string CodeOrDescription
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion
    }
}
