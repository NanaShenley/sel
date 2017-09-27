using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;

namespace POM.Components.Communication
{
    public class SelectAgentTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("Additional_select_agent_detail"); }
        }

        public SelectAgentTripletDialog()
        {
            _searchCriteria = new SelectAgentSearchDialog(this);
        }

        #region Search

        public class AgentSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Name']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly SelectAgentSearchDialog _searchCriteria;
        public SelectAgentSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

    }

    public class SelectAgentSearchDialog : SearchCriteriaComponent<SelectAgentTripletDialog.AgentSearchResultTile>
    {
        public SelectAgentSearchDialog(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "NameSearchText")]
        private IWebElement _agentName;

        [FindsBy(How = How.Name, Using = "ServiceType.dropdownImitator")]
        private IWebElement _serviceProvided;

        public string AgentName
        {
            set { _agentName.SetText(value); }
            get { return _agentName.Text; }
        }

        public string ServiceProvided
        {
            set { _serviceProvided.EnterForDropDown(value); }
            get { return _serviceProvided.GetValue(); }
        }

        #endregion
    }
}
