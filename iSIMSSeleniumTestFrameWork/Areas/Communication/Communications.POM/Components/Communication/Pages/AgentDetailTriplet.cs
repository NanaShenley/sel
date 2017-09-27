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
    public class AgentDetailTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public AgentDetailTriplet()
        {
            _searchCriteria = new AgentDetailSearchPage(this);
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'add_new_agent_button']")]
        private IWebElement _addNewButton;

        #endregion

        #region Actions

        public AddNewAgentDialog AddNewAgent()
        {
            Retry.Do(_addNewButton.ClickByJS);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddNewAgentDialog();
        }
        #endregion

        #region Search
        public class AgentSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Name']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }

            [FindsBy(How = How.Name, Using = "ServiceType.dropdownImitator")]
            private IWebElement _service;

            public string ServiceProvided
            {
                get { return _service.Text; }
            }

        }

        private readonly AgentDetailSearchPage _searchCriteria;

        public AgentDetailSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion
    }
}
