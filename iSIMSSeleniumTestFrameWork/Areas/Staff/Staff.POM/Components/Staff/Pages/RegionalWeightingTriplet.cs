using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class RegionalWeightingTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public RegionalWeightingTriplet()
        {
            _searchCriteria = new RegionalWeightingSearch(this);
        }

        #region Search

        private readonly RegionalWeightingSearch _searchCriteria;
        public RegionalWeightingSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Public actions
        public new void ClickCreate()
        {
            AutomationSugar.WaitFor(new ByChained(ComponentIdentifier, SimsBy.AutomationId("create_service_RegionalWeighting")));
            AutomationSugar.ClickOn(new ByChained(ComponentIdentifier, SimsBy.AutomationId("create_service_RegionalWeighting")));
            AutomationSugar.WaitForAjaxCompletion();
        }
        #endregion
    }

    public class RegionalWeightingSearch : SearchTableCriteriaComponent
    {
        public RegionalWeightingSearch(BaseComponent parent) : base(parent) { }

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
