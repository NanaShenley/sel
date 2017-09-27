using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using Staff.POM.Base;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.POM.Components.Staff
{
    public class ServiceTermTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_triplet"); }
        }

        public class ServiceTermSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Code']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }

            [FindsBy(How = How.CssSelector, Using = "[title='Description']")]
            private IWebElement _description;

            public string Description
            {
                get { return _description.Text; }
            }
        }
        #region Search

        private readonly ServiceTermSearchPage _searchCriteria;
        public ServiceTermSearchPage SearchCriteria { get { return _searchCriteria; } }

        public ServiceTermTriplet()
        {
            _searchCriteria = new ServiceTermSearchPage(this);
        }
        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _continueWithDeleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='service_terms_create_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Yes_button']")]
        private IWebElement _confirmDeleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        #endregion

        #region Page Action
        public ServiceTermPage Create()
        {
            Retry.Do(_createButton.ClickByJS);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new ServiceTermPage();
        }

        public new void ClickCreate()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("service_terms_create_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("service_terms_create_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

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

        public void ConfirmDeleteTableRow()
        {
            if (_confirmDeleteButton.IsExist())
            {
                _confirmDeleteButton.ClickByJS();
            }
        }

        public void SelectSearchTile(ServiceTermSearchResultTile serviceTermTile)
        {
            if (serviceTermTile != null)
            {
                serviceTermTile.Click();
            }
        }

        public ServiceTermPage SelectServiceTerm(IWebElement serviceTerm)
        {
            ServiceTermPage serviceTermPage = null;
            if (serviceTerm != null)
            {
                serviceTerm.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                serviceTermPage = new ServiceTermPage();
            }
            return serviceTermPage;
        }

        #endregion
    }

    public class ServiceTermSearchPage : SearchCriteriaComponent<ServiceTermTriplet.ServiceTermSearchResultTile>
    {
        public ServiceTermSearchPage(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _searchByCodeTextBox;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _searchByDescriptionTextBox;

        public string SearchByCode
        {
            set { _searchByCodeTextBox.SetText(value); }
            get { return _searchByCodeTextBox.GetValue(); }
        }

        public string DescriptionSearch
        {
            set { _searchByDescriptionTextBox.SetText(value); }
            get { return _searchByDescriptionTextBox.GetValue(); }
        }

        #endregion
    }
}
