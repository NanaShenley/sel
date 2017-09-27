using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Admission
{
    public class LookupTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public LookupTriplet()
        {
            _searchCriteria = new LookupSearch(this);
        }

        #region Search

        private readonly LookupSearch _searchCriteria;
        public LookupSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_ApplicationStatus']")]
        private IWebElement _addButtonForApplicationStatus;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_AdmissionsAppealStatus']")]
        private IWebElement _addButtonForAppealOutcome;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_AdmissionsAppealHearingOutcome']")]
        private IWebElement _addButtonForAppealResult;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_ApplicationRejectionReason']")]
        private IWebElement _addButtonForReasonAdmissionRejected;
        #endregion

        #region Public methods

        public LookupWithCategoryPage AddRow(string screenName)
        {
            if (screenName == "ApplicationStatus")
                _addButtonForApplicationStatus.Click();
            else if (screenName == "AdmissionsAppealStatus")
                _addButtonForAppealOutcome.Click();
            else if (screenName == "AdmissionsAppealHearingOutcome")
                _addButtonForAppealResult.Click();

            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new LookupWithCategoryPage();
        }

        public LookupWithProviderPage AddLookupRow(string screenName)
        {
            if (screenName == "ApplicationRejectionReason")
                _addButtonForReasonAdmissionRejected.Click();

            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new LookupWithProviderPage();
        }
        #endregion
    }

    public class LookupSearch : SearchTableCriteriaComponent
    {
        public LookupSearch(BaseComponent parent) : base(parent) { }

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
