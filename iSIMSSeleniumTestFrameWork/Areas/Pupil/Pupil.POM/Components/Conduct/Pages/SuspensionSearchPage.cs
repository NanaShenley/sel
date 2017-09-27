using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Conduct
{
    public class SuspensionSearchPage : SearchCriteriaComponent<SuspensionTriplet.SuspensionSearchResultTile>
    {
        public SuspensionSearchPage(BaseComponent component) : base(component)
        {
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "PupilName")]
        private IWebElement _pupilNameTextBox;

        [FindsBy(How = How.Name, Using = "StatusCurrentCriterion")]
        private IWebElement _currentCheckBox;

        [FindsBy(How = How.Name, Using = "StatusFutureCriterion")]
        private IWebElement _futureCheckBox;

        [FindsBy(How = How.Name, Using = "StatusFormerCriterion")]
        private IWebElement _leaverCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_advanced']")]
        private IWebElement _showMoreButton;

        [FindsBy(How = How.XPath, Using = "//label[text()='New Suspension']/preceding-sibling::input[position()=1]")]
        private IWebElement _newSuspensionCheckbox;

        [FindsBy(How = How.XPath, Using = "//label[text()='Continued Suspension']/preceding-sibling::input[position()=1]")]
        private IWebElement _continueSuspentionCheckbox;

        [FindsBy(How = How.XPath, Using = "//label[text()='Expulsion']/preceding-sibling::input[position()=1]")]
        private IWebElement _expulsionCheckbox;

        public string PupilName
        {
            set { _pupilNameTextBox.SetText(value); }
            get { return _pupilNameTextBox.GetValue(); }
        }

        public bool IsCurrent
        {
            set { _currentCheckBox.Set(value); }
            get { return _currentCheckBox.IsChecked(); }
        }

        public bool IsFuture
        {
            set { _futureCheckBox.Set(value); }
            get { return _futureCheckBox.IsChecked(); }
        }

        public bool IsLeaver
        {
            set { _leaverCheckBox.Set(value); }
            get { return _leaverCheckBox.IsChecked(); }
        }

        public bool IsNewSuspension
        {
            set { _newSuspensionCheckbox.Set(value); }
            get { return _newSuspensionCheckbox.IsChecked(); }
        }

        public bool IsContinueSuspension
        {
            set { _continueSuspentionCheckbox.Set(value); }
            get { return _continueSuspentionCheckbox.IsChecked(); }
        }

        public bool IsExpulsion
        {
            set { _expulsionCheckbox.Set(value); }
            get { return _expulsionCheckbox.IsChecked(); }
        }

        #endregion Page properties

        #region Action

        public void ClickShowMore()
        {
            _showMoreButton.Click();
        }

        #endregion Action
    }
}