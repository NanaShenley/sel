using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class NationalInsuranceRateTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("national_insurance_rate_triplet"); }
        }

        public NationalInsuranceRateTriplet()
        {
            _searchCriteria = new NationalInsuranceRatesSearchPage(this);
        }

        #region Search

        private readonly NationalInsuranceRatesSearchPage _searchCriteria;
        public NationalInsuranceRatesSearchPage SearchCriteria { get { return _searchCriteria; } }

        public class NationalInsuranceRatesSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        #endregion

        #region Properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='national_insurance_rate_create_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        public NationalInsuranceRatesPage Create()
        {
            _createButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new NationalInsuranceRatesPage();
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: click Delete button to delete an existing scheme
        /// </summary>
        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new DeleteConfirmationPage();
                confirmDialog.ConfirmDelete();
            }
        }

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool IsSuccessMessageDisplay()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_success']"));
            return SeleniumHelper.DoesWebElementExist(_messageSuccess);
        }

        public void SelectSearchTile(NationalInsuranceRatesSearchResultTile nationalInsuranceRateTile)
        {
            if (nationalInsuranceRateTile != null)
            {
                nationalInsuranceRateTile.Click();
            }
        }

        #endregion
    }

    public class NationalInsuranceRatesSearchPage : SearchCriteriaComponent<NationalInsuranceRateTriplet.NationalInsuranceRatesSearchResultTile>
    {
        public NationalInsuranceRatesSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties


        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _searchByStartDateTextBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _searchByEndDateTextBox;

        public string SearchByStartDate
        {
            get { return _searchByStartDateTextBox.GetAttribute("value"); }
            set { _searchByStartDateTextBox.SetDateTime(value); }
        }
        public string SearchByEndDate
        {
            get { return _searchByEndDateTextBox.GetAttribute("value"); }
            set { _searchByEndDateTextBox.SetDateTime(value); }
        }
        #endregion
    }
}
