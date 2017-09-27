using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class SenNeedTypeTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public SenNeedTypeTriplet()
        {
            _searchCriteria = new SenNeedTypeSearch(this);
        }

        #region Search

        private readonly SenNeedTypeSearch _searchCriteria;
        public SenNeedTypeSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_SENNeedType']")]
        private IWebElement _addButton;

        #endregion

        #region Public methods

        public SenNeedTypePage AddSenNeedType()
        {
            _addButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SenNeedTypePage();
        }

        #endregion
    }

    public class SenNeedTypeSearch : SearchTableCriteriaComponent
    {
        public SenNeedTypeSearch(BaseComponent parent) : base(parent) { }

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
