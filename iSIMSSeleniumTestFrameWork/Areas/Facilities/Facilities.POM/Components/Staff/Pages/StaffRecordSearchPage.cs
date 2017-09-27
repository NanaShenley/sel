using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class StaffRecordSearchPage : SearchCriteriaComponent<StaffRecordTriplet.StaffRecordSearchResultTile>
    {
        public StaffRecordSearchPage(BaseComponent parent) : base(parent) { }

        #region Search roperties

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _staffNameTextBox;

        [FindsBy(How = How.Id, Using = "tri_chkbox_StatusCurrentCriterion")]
        private IWebElement _currentCheckBox;

        [FindsBy(How = How.Id, Using = "tri_chkbox_StatusFutureCriterion")]
        private IWebElement _futureCheckBox;

        [FindsBy(How = How.Id, Using = "tri_chkbox_StatusFormerCriterion")]
        private IWebElement _leaverCheckBox;

        public string StaffName
        {
            set { _staffNameTextBox.SetText(value); }
            get { return _staffNameTextBox.GetValue(); }
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

        #endregion
    }
}
