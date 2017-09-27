using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class PupilLogTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public PupilLogTriplet()
        {
            _searchCriteria = new PupilLogSearchPage(this);
        }

        #region Search

        private readonly PupilLogSearchPage _searchCriteria;
        public PupilLogSearchPage SearchCriteria { get { return _searchCriteria; } }

        public class PupilLogSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Pupil Name']")]
            private IWebElement _name;

            [FindsBy(How = How.CssSelector, Using = "[title='Year/Class']")]
            private IWebElement _yearClass;

            public string Name
            {
                get { return _name.Text; }
            }

            public string YearGroup
            {
                get { return _yearClass.Text.Split('/')[0].Trim(); }
            }

            public string Class
            {
                get { return _yearClass.Text.Split('/')[1].Trim(); }
            }

            public string YearGroupClass
            {
                get { return _yearClass.GetText(); }
            }
        }
        #endregion
    }

    public class PupilLogSearchPage : SearchCriteriaComponent<PupilLogTriplet.PupilLogSearchResultTile>
    {
        public PupilLogSearchPage(BaseComponent parent) : base(parent) { }

        #region Search roperties

        [FindsBy(How = How.Name, Using = "PupilName")]
        private IWebElement _pupilNameTextBox;

        [FindsBy(How = How.Name, Using = "StatusCurrentCriterion")]
        private IWebElement _currentCheckBox;

        [FindsBy(How = How.Name, Using = "StatusFormerCriterion")]
        private IWebElement _leaverCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_advanced']")]
        private IWebElement _searchAdvancedButton;

        [FindsBy(How = How.Name, Using = "PrimaryClass.dropdownImitator")]
        private IWebElement _classDropDown;

        [FindsBy(How = How.Name, Using = "YearGroup.dropdownImitator")]
        private IWebElement _yearGroupDropDown;

        [FindsBy(How = How.Name, Using = "EnrolmentStatus.dropdownImitator")]
        private IWebElement _enrolmentDropDown;

        [FindsBy(How = How.Name, Using = "SENStatusCriterion")]
        private IWebElement _senCheckBox;

        public string PupilName
        {
            set { _pupilNameTextBox.SetText(value); }
        }

        public bool IsCurrent
        {
            set { _currentCheckBox.Set(value); }
        }

        public bool IsSen
        {
            set { _senCheckBox.Set(value); }
        }

        public bool IsLeaver
        {
            set { _leaverCheckBox.Set(value); }
            get { return _leaverCheckBox.IsChecked(); }
        }

        public string Class
        {
            set { _classDropDown.EnterForDropDown(value); }
        }

        public string YearGroup
        {
            set { _yearGroupDropDown.EnterForDropDown(value); }
        }

        public string EnrolmentStatus
        {
            set { _enrolmentDropDown.EnterForDropDown(value); }
        }

        public void ClickSearchAdvanced(bool showMore)
        {
            bool isExpanded = _searchAdvancedButton.GetText().Trim().Equals("Show Less");
            if ((showMore && !isExpanded) || (!showMore && isExpanded))
            {
                _searchAdvancedButton.Click();
                Wait.WaitForElementDisplayed(By.Name("PrimaryClass.dropdownImitator"));
            }
        }
        #endregion

    }
}
