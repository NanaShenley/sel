using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Base;
using PageObjectModel.Helper;

namespace PageObjectModel.Components.Admission
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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_AdmissionsAppealHearingOutcome']")]
        private IWebElement _addButtonForAppealOutcome;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_AdmissionsAppealStatus']")]
        private IWebElement _addButtonForAppealResult;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_Tier4Category']")]
        private IWebElement _addButtonForTier4Category;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_Tier4Evidence']")]
        private IWebElement _addButtonForTier4Evidence;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_Tier4Reason']")]
        private IWebElement _addButtonForTier4Reason;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_Tier4Region']")]
        private IWebElement _addButtonForTier4Region;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_Tier4VisaStatus']")]
        private IWebElement _addButtonForTier4VisaStatus;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_Tier4VisaType']")]
        private IWebElement _addButtonForTier4VisaType;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_ApplicationRejectionReason']")]
        private IWebElement _addButtonForReasonAdmissionRejected;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_AdmissionTerm']")]
        private IWebElement _addButtonForAdmissionTerm;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_MarketingSource']")]
        private IWebElement _addButtonForMarketingSource;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_ReasonForEnquiry']")]
        private IWebElement _addButtonForEnquiryReason;

         [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_ReasonEnquiryWithdrawn']")]
        private IWebElement _addButtonForReasonEnquiryWithdrawn;

       #endregion

        #region Public methods

        public LookupWithCategoryPage AddRow(string screenName)
        {
            if (screenName == "ApplicationStatus")
                _addButtonForApplicationStatus.Click();
            else if (screenName == "AppealOutcome")
                _addButtonForAppealOutcome.Click();
            else if (screenName == "AppealStatus")
                _addButtonForAppealResult.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new LookupWithCategoryPage();
        }

        public LookupWithProviderPage AddLookupRow(string screenName)
        {
            switch (screenName)
            {
                case "AdmissionTerm":
                    _addButtonForAdmissionTerm.Click();
                    break;
                case "ReasonAdmissionRejected":
                    _addButtonForReasonAdmissionRejected.Click();
                    break;
                case "ApplicationStatus":
                    _addButtonForApplicationStatus.Click();
                    break;
                case "AppealOutcome":
                    _addButtonForAppealOutcome.Click();
                    break;
                case "AppealResult":
                    _addButtonForAppealResult.Click();
                    break;
                case "Tier4Category":
                    _addButtonForTier4Category.Click();
                    break;
                case "Tier4Evidence":
                    _addButtonForTier4Evidence.Click();
                    break;
                case "Tier4Reason":
                    _addButtonForTier4Reason.Click();
                    break;
                case "Tier4Region":
                    _addButtonForTier4Region.Click();
                    break;
                case "Tier4VisaType":
                    _addButtonForTier4VisaType.Click();
                    break;
                case "Tier4VisaStatus":
                    _addButtonForTier4VisaStatus.Click();
                    break;
                case "MarketingSource":
                    _addButtonForMarketingSource.Click();
                    break;
                case "EnquiryReason":
                    _addButtonForEnquiryReason.Click();
                    break;
                case "EnquiryWithdrawnReason":
                    _addButtonForReasonEnquiryWithdrawn.Click();
                    break;

                default:
                    break;
            }

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
