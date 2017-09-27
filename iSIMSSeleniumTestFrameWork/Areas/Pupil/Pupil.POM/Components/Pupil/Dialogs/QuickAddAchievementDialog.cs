using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using WebDriverRunner.webdriver;

namespace POM.Components.Pupil
{
    public class QuickAddAchievementDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "AchievementEventCategories.dropdownImitator")]
        private IWebElement _achievementTypeDropDown;

        [FindsBy(How = How.Name, Using = "Summary")]
        private IWebElement _comments;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_button']")]
        private IWebElement _saveButton;


        public string AchievementType
        {
            set { _achievementTypeDropDown.ChooseSelectorOption(value); }
            get { return _achievementTypeDropDown.GetValue(); }
        }

        public string Comments
        {
            set { _comments.SetText(value); }
            get { return _comments.Text; }
        }

        #endregion

        #region Public actions

        public void ClickPointsSliderUp()
        {
            SeleniumHelper.ClickLink("a[data-value='up']");
        }

        public void ClickPointsSliderDown()
        {
            SeleniumHelper.ClickLink("a[data-value='down']");
        }

        public void Save()
        {
            _saveButton.ClickByJS();
        }

        #endregion
    }
}
