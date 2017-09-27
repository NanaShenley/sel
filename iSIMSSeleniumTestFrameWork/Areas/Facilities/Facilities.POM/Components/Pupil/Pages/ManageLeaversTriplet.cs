using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class ManageLeaversTriplet: BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public ManageLeaversTriplet()
        {
            _searchCriteria = new ManageLeaversSearch(this);
        }

        #region Search

        private readonly ManageLeaversSearch _searchCriteria;
        public ManageLeaversSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion
    }

    public class ManageLeaversSearch : SearchTableCriteriaComponent
    {
        public ManageLeaversSearch(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "PrimaryClass.dropdownImitator")]
        private IWebElement _classDropdown;

        [FindsBy(How = How.Name, Using = "YearGroup.dropdownImitator")]
        private IWebElement _yearGroupDropdown;

        [FindsBy(How = How.Name, Using = "tri_chkbox_StatusFormerCriterion")]
        private IWebElement _leaverCheckBox;

        public string Class
        {
            get { return _classDropdown.GetValue(); }
            set { _classDropdown.EnterForDropDown(value); }
        }

        public string YearGroup
        {
            get { return _yearGroupDropdown.GetValue(); }
            set { _yearGroupDropdown.EnterForDropDown(value); }
        }

        public bool IsLeaver
        {
            get { return _leaverCheckBox.IsChecked(); }
            set { _leaverCheckBox.Set(value); }
        }

        #endregion
    }
}
