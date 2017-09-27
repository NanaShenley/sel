using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Conduct.Pages
{
    public class AchievementEventSearchPage : SearchCriteriaComponent<AchievementEventTriplet.AchievementEventSearchResultTile>
    {
        public AchievementEventSearchPage(BaseComponent component) : base(component)
        {
        }

        [FindsBy(How = How.Name, Using = "AchievementEventCategories.dropdownImitator")]
        private IWebElement _achievementEventTypeDropDown;

        public string AchievementEventCategory
        {
            set
            {
                _achievementEventTypeDropDown.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get
            {
                return _achievementEventTypeDropDown.GetValue();
            }
        }
    }
}
