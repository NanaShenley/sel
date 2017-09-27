using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class QTSRouteTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public QTSRouteTriplet()
        {
            _searchCriteria = new QTSRouteSearch(this);
        }

        #region Search

        private readonly QTSRouteSearch _searchCriteria;
        public QTSRouteSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Public actions

        public new void ClickCreate()
        {
            AutomationSugar.WaitFor(new ByChained(ComponentIdentifier, SimsBy.AutomationId("create_service_QTSRoute")));
            AutomationSugar.ClickOn(new ByChained(ComponentIdentifier, SimsBy.AutomationId("create_service_QTSRoute")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        #endregion
    }

    public class QTSRouteSearch : SearchTableCriteriaComponent
    {
        public QTSRouteSearch(BaseComponent parent) : base(parent) { }

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
