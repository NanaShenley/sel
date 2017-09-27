using POM.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;


namespace POM.Components.Communication
{
    public class AgentDetailSearchPage : SearchCriteriaComponent<AgentDetailTriplet.AgentSearchResultTile>
    {
        public AgentDetailSearchPage(BaseComponent component) : base(component) { }

        #region Properties
        [FindsBy(How = How.Name, Using = "NameSearchText")]
        private IWebElement _agentNameTextbox;

        [FindsBy(How = How.Name, Using = "ServiceType.dropdownImitator")]
        private IWebElement _serviceProvideDropdown;

        public string AgentName
        {
            set { _agentNameTextbox.SetText(value); }
        }

        public string ServiceProvide
        {
            set { _serviceProvideDropdown.EnterForDropDown(value); }
        }

        #endregion

    }
}
