using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class SenStatusTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public SenStatusTriplet()
        {
            _searchCriteria = new SenStatusSearch(this);
        }

        #region Search

        private readonly SenStatusSearch _searchCriteria;
        public SenStatusSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_SENStatus']")]
        private IWebElement _addButton;

        #endregion

        #region Public methods

        public SenStatusPage AddSenStatus()
        {
            _addButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SenStatusPage();
        }

        #endregion
    }

    public class SenStatusSearch : SearchTableCriteriaComponent
    {
        public SenStatusSearch(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _searchTextBox;

        public string CodeOrDecription
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion
    }
}
