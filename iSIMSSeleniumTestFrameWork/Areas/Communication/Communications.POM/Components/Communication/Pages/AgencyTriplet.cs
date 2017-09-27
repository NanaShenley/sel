using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Communication
{
    public class AgencyTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public AgencyTriplet()
        {
            _searchCriteria = new AgencyDetailSearchPage(this);
        }

        #region Search
        public class AgencySearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Agency ']")]
            private IWebElement _agencyName;

            public string AgencyName
            {
                get { return _agencyName.Text; }
            }
        }

        private readonly AgencyDetailSearchPage _searchCriteria;

        public AgencyDetailSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _createButton;

        #endregion

        #region Actions
        
        public AgencyDetailPage CreateAgency()
        {
            //Retry.Do(_createButton.Click);
            SeleniumHelper.ClickByJS(_createButton);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            SeleniumHelper.Sleep(2);
            return AgencyDetailPage.Create();
        }

        #endregion
    }

    public class AgencyDetailSearchPage : SearchCriteriaComponent<AgencyTriplet.AgencySearchResultTile>
    {
        public AgencyDetailSearchPage(BaseComponent component) : base(component) { }

        #region Properties
        [FindsBy(How = How.Name, Using = "AgencyName")]
        private IWebElement _agencyNameTextbox;               

        [FindsBy(How = How.Name, Using = "ServiceType.dropdownImitator")]
        private IWebElement _serviceProvideDropdown;

        //[FindsBy(How = How.CssSelector, Using = SeleniumHelper.AutomationId("search_criteria"))]
        //private IWebElement _searchBoxTool;

        public string AgencyName
        {
            set { _agencyNameTextbox.SetText(value); }
            get { return _agencyNameTextbox.GetValue(); }
        }

        public string ServiceProvide
        {
            set { _serviceProvideDropdown.EnterForDropDown(value); }
            get { return _serviceProvideDropdown.GetValue(); }
        }

        #endregion

    }

}
