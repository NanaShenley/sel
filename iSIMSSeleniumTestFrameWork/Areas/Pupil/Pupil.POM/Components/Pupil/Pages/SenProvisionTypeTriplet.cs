using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class SenProvisionTypeTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public SenProvisionTypeTriplet()
        {
            _searchCriteria = new SenProvisionTypeSearch(this);
        }

        #region Search

        private readonly SenProvisionTypeSearch _searchCriteria;
        public SenProvisionTypeSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_SENProvisionType']")]
        private IWebElement _addButton;

        #endregion

        #region Public methods

        public SenProvisionTypePage AddSenProvisionType()
        {
            _addButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SenProvisionTypePage();
        }

        #endregion
    }

    public class SenProvisionTypeSearch : SearchTableCriteriaComponent
    {
        public SenProvisionTypeSearch(BaseComponent parent) : base(parent) { }

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
