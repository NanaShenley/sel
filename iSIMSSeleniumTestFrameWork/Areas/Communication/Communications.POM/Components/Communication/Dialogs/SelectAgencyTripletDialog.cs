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
    public class SelectAgencyTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("Additional_select_agency_detail"); }
        }

        public SelectAgencyTripletDialog()
        {
            _searchCriteria = new SelectAgencySearchDialog(this);
        }

        #region Search

        public class SelectAgencySearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Agency Name']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly SelectAgencySearchDialog _searchCriteria;
        public SelectAgencySearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

    }

    public class SelectAgencySearchDialog : SearchCriteriaComponent<SelectAgencyTripletDialog.SelectAgencySearchResultTile>
    {
        public SelectAgencySearchDialog(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "AgencyName")]
        private IWebElement _agencyName;

        [FindsBy(How = How.Name, Using = "ServiceType.dropdownImitator")]
        private IWebElement _serviceProvided;

        public string AgencyName
        {
            set { _agencyName.SetText(value); }
            get { return _agencyName.Text; }
        }

        public string ServiceProvided
        {
            set { _serviceProvided.EnterForDropDown(value); }
            get { return _serviceProvided.GetValue(); }
        }

        #endregion
    }
}
