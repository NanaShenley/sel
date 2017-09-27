using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using SeSugar.Automation;
using SimsBy = POM.Helper.SimsBy;

namespace POM.Components.Conduct.Pages
{
    public class ConductConfigurationPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("pupil_conduct_configuration_detail"); }
        }

        [FindsBy(How = How.Name, Using = "AchievementLabel")]
        private IWebElement _achievementLabel;

        [FindsBy(How = How.Name, Using = "BehaviourLabel")]
        private IWebElement _behaviourLabel;

        [FindsBy(How = How.Name, Using = "DefaultAchievementCategory.dropdownImitator")]
        private IWebElement _defaultAchievementType;

        [FindsBy(How = How.Name, Using = "DefaultBehaviourCategory.dropdownImitator")]
        private IWebElement _defaultBehaviourType;

        [FindsBy(How = How.Name, Using = "PointsIncrement")]
        private IWebElement _pointsIncrement;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        private By _behaviourUiConfigSelector = By.CssSelector("tr[data-row-name='BehaviourUiConfiguration']");

        private By _achievementUiConfigSelector = By.CssSelector("tr[data-row-name='AchievementUiConfiguration']");

        public string AchievementLabel
        {
            get { return _achievementLabel.GetText(); }
            set
            {
                _achievementLabel.SetText(value);
            }
        }

        public string BehaviourLabel
        {
            get { return _behaviourLabel.GetText(); }
            set
            {
                _behaviourLabel.SetText(value);
            }
        }

        public string PointsIncrement
        {
            get { return _pointsIncrement.GetText(); }
            set
            {
                _pointsIncrement.SetText(value);
            }
        }

        public string DefaultAchievementType
        {
            get { return _defaultAchievementType.GetValue(); }
            set
            {
                _defaultAchievementType.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public string DefaultBehaviourType
        {
            get { return _defaultBehaviourType.GetValue(); }
            set
            {
                _defaultBehaviourType.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public bool AchievementUiConfig
        {
            get
            {
                var firstRow = SeleniumHelper.FindElement(_achievementUiConfigSelector);
                return firstRow.IsCheckboxChecked(By.CssSelector("input[name$='IsVisible'][type='checkbox']"));
            }
            set
            {
                var firstRow = SeleniumHelper.FindElement(_achievementUiConfigSelector);
                firstRow.SetCheckBox(By.CssSelector("input[name$='IsVisible'][type='checkbox']"), value);
            }
        }

        public bool BehaviourUiConfig
        {
            get
            {
                var firstRow = SeleniumHelper.FindElement(_behaviourUiConfigSelector);
                return firstRow.IsCheckboxChecked(By.CssSelector("input[name$='IsVisible'][type='checkbox']"));
            }
            set
            {
                var firstRow = SeleniumHelper.FindElement(_behaviourUiConfigSelector);
                firstRow.SetCheckBox(By.CssSelector("input[name$='IsVisible'][type='checkbox']"), value);
            }
        }

        public bool DoesAchievementUiConfigExist
        {
            get { return SeleniumHelper.DoesWebElementExist(_achievementUiConfigSelector); }
        }

        public bool DoesBehaviourUiConfigExist
        {
            get { return SeleniumHelper.DoesWebElementExist(_behaviourUiConfigSelector); }
        }

        public void ClickSave()
        {
            _saveButton.ClickByJS();
            AutomationSugar.WaitForAjaxCompletion();
            Refresh();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Verify success message is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessMessageDisplayed()
        {
            AutomationSugar.WaitForAjaxCompletion();
            return SeleniumHelper.IsElementExists(SimsBy.AutomationId("status_success"));
        }
    }
}
