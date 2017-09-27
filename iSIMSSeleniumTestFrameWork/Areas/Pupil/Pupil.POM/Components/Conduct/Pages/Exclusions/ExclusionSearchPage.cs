using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
// ReSharper disable PrivateMembersMustHaveComments
#pragma warning disable 649

namespace POM.Components.Conduct.Pages.Exclusions
{
    /// <summary>
    /// Implements ExclusionSearchPage
    /// </summary>
    public class ExclusionSearchPage : SearchCriteriaComponent<ExclusionTriplet.ExclusionSearchResultTile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExclusionSearchPage"/> class.
        /// </summary>
        /// <param name="component">The component.</param>
        public ExclusionSearchPage(BaseComponent component) : base(component)
        {
        }

        #region Page properties

        /// <summary>
        /// The _pupil name text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "PupilName")]
        private IWebElement _pupilNameTextBox;

        /// <summary>
        /// The _current CheckBox
        /// </summary>
        [FindsBy(How = How.Name, Using = "StatusCurrentCriterion")]
        private IWebElement _currentCheckBox;

        /// <summary>
        /// The _future CheckBox
        /// </summary>
        [FindsBy(How = How.Name, Using = "StatusFutureCriterion")]
        private IWebElement _futureCheckBox;

        /// <summary>
        /// The _leaver CheckBox
        /// </summary>
        [FindsBy(How = How.Name, Using = "StatusFormerCriterion")]
        private IWebElement _leaverCheckBox;

        /// <summary>
        /// The _show more button
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_advanced']")]
        private IWebElement _showMoreButton;

        /// <summary>
        /// The _new exclusion checkbox
        /// </summary>
        [FindsBy(How = How.XPath, Using = "//label[text()='New Suspension']/preceding-sibling::input[position()=1]")]
        private IWebElement _newExclusionCheckbox;

        /// <summary>
        /// The _continue suspension checkbox
        /// </summary>
        [FindsBy(How = How.XPath, Using = "//label[text()='Continued Suspension']/preceding-sibling::input[position()=1]")]
        private IWebElement _continueSuspensionCheckbox;

        /// <summary>
        /// The _expulsion checkbox
        /// </summary>
        [FindsBy(How = How.XPath, Using = "//label[text()='Expulsion']/preceding-sibling::input[position()=1]")]
        private IWebElement _expulsionCheckbox;

        /// <summary>
        /// Gets or sets the name of the pupil.
        /// </summary>
        /// <value>
        /// The name of the pupil.
        /// </value>
        public string PupilName
        {
            set { _pupilNameTextBox.SetText(value); }
            get { return _pupilNameTextBox.GetValue(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is current.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is current; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrent
        {
            set { _currentCheckBox.Set(value); }
            get { return _currentCheckBox.IsChecked(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is future.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is future; otherwise, <c>false</c>.
        /// </value>
        public bool IsFuture
        {
            set { _futureCheckBox.Set(value); }
            get { return _futureCheckBox.IsChecked(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is leaver.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is leaver; otherwise, <c>false</c>.
        /// </value>
        public bool IsLeaver
        {
            set { _leaverCheckBox.Set(value); }
            get { return _leaverCheckBox.IsChecked(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is new exclusion.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is new exclusion; otherwise, <c>false</c>.
        /// </value>
        public bool IsNewExclusion
        {
            set { _newExclusionCheckbox.Set(value); }
            get { return _newExclusionCheckbox.IsChecked(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is continue exclusion.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is continue exclusion; otherwise, <c>false</c>.
        /// </value>
        public bool IsContinueExclusion
        {
            set { _continueSuspensionCheckbox.Set(value); }
            get { return _continueSuspensionCheckbox.IsChecked(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expulsion.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expulsion; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpulsion
        {
            set { _expulsionCheckbox.Set(value); }
            get { return _expulsionCheckbox.IsChecked(); }
        }

        #endregion Page properties

        #region Action

        /// <summary>
        /// Clicks the show more.
        /// </summary>
        public void ClickShowMore()
        {
            _showMoreButton.Click();
        }

        #endregion Action
    }
}