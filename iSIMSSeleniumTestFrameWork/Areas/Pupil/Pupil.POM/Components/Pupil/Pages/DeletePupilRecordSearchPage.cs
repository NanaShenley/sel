using POM.Base;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using POM.Components.Common;


namespace POM.Components.Pupil
{
    public class DeletePupilRecordSearchPage : SearchCriteriaComponent<PupilSearchTriplet.PupilSearchResultTile>
    {
        public DeletePupilRecordSearchPage(BaseComponent parent) : base(parent) { }

        #region Search roperties

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _pupilNameTextBox;
        
        [FindsBy(How = How.Name, Using = "StatusCurrentCriterion")]
        private IWebElement _currentCheckBox;

        [FindsBy(How = How.Name, Using = "StatusFutureCriterion")]
        private IWebElement _futureCheckBox;

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

        public string Class
        {
            set { _classDropDown.EnterForDropDown(value); }
            get { return _classDropDown.GetText(); }
        }

        public string YearGroup
        {
            set { _yearGroupDropDown.EnterForDropDown(value); }
            get { return _yearGroupDropDown.GetText(); }
        }

        public string EnrolmentStatus
        {
            set { _enrolmentDropDown.EnterForDropDown(value); }
            get { return _enrolmentDropDown.GetText(); }
        }

        public void ClickSearchAdvanced(bool showMore)
        {
            bool isExpanded = _searchAdvancedButton.GetText().Trim().Equals("Show Less");
            if((showMore && !isExpanded) || (!showMore && isExpanded))
            {
                _searchAdvancedButton.Click();
                Wait.WaitForControl(SimsBy.Name("PrimaryClass.dropdownImitator"));
            }
        }
        #endregion

    }
}
