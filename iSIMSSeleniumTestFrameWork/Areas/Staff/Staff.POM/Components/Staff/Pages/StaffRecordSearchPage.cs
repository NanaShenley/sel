using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Base;
using Staff.POM.Helper;

namespace Staff.POM.Components.Staff
{
    public class StaffRecordSearchPage : SearchCriteriaComponent<StaffRecordTriplet.StaffRecordSearchResultTile>
    {
        public StaffRecordSearchPage(BaseComponent parent) : base(parent) { }

        #region Search roperties

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _staffNameTextBox;

        [FindsBy(How = How.Name, Using = "StatusCurrentCriterion")]
        private IWebElement _currentCheckBox;

        [FindsBy(How = How.Name, Using = "StatusFutureCriterion")]
        private IWebElement _futureCheckBox;

        [FindsBy(How = How.Name, Using = "StatusFormerCriterion")]
        private IWebElement _leaverCheckBox;

        public string StaffName
        {
            set { _staffNameTextBox.SetText(value, new ByChained(this.ComponentIdentifier, By.Name("LegalSurname"))); }
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
