using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Conduct.Pages
{
    public class ConductSummaryPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("pupil_conduct_summary_detail"); }
        }

        private static string _achievementLabel = "";

        private static string _behaviourLabel = "";

        public ConductSummaryPage(string achievementLabel, string behaviourLabel)
        {
            _achievementLabel = achievementLabel;
            _behaviourLabel = behaviourLabel;
        }

        // CSS Selector is used as same Id is using for tab content div
        [FindsBy(How = How.CssSelector, Using = "a#ConductSummary_Behaviours_Tab-label")]
        private IWebElement _behavioursTab;

        [FindsBy(How = How.CssSelector, Using = "a#ConductSummary_Achievements_Tab-label")]
        private IWebElement _achievementsTab;

        public bool CanSeeAddAchievement
        {
            get { return SeleniumHelper.DoesWebElementExist(SimsBy.AutomationId(string.Concat("add_new_", _achievementLabel.ToLower(), "_event_button"))); }
        }

        public bool CanSeeAddBehaviour
        {
            get { return SeleniumHelper.DoesWebElementExist(SimsBy.AutomationId(string.Concat("add_new_", _behaviourLabel.ToLower(), "_event_button"))); }
        }

        public void ClickAchievementsTab()
        {
            _achievementsTab.ClickByJS();

            // Wait for tab contents to be displayed.
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("div#Conduct_Summary_Tab_Content"));

            Refresh();
        }

        public void ClickBehavioursTab()
        {
            _behavioursTab.ClickByJS();

            // Wait for tab contents to be displayed.
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("div#Conduct_Summary_Tab_Content"));

            Refresh();
        }
    }
}
