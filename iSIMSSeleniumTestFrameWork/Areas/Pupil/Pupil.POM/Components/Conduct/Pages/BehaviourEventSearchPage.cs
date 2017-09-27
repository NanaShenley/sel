using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Conduct.Pages
{
    public class BehaviourEventSearchPage : SearchCriteriaComponent<BehaviourEventTriplet.BehaviourEventSearchResultTile>
    {
        public BehaviourEventSearchPage(BaseComponent component) : base(component)
        {
        }

        [FindsBy(How = How.Name, Using = "BehaviourEventCategories.dropdownImitator")]
        private IWebElement _behaviourEventTypeDropDown;

        public string BehaviourEventCategory
        {
            set
            {
                _behaviourEventTypeDropDown.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get
            {
                return _behaviourEventTypeDropDown.GetValue();
            }
        }
    }
}
