using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class StaffRoleTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public StaffRoleTriplet()
        {
            _searchCriteria = new StaffRoleSearchPage(this);
        }

        #region Search

        private readonly StaffRoleSearchPage _searchCriteria;

        public StaffRoleSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion
    }

    public class StaffRoleSearchPage : SearchTableCriteriaComponent
    {
        public StaffRoleSearchPage(BaseComponent context) : base(context) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _searchTextBox;

        public string SearchCodeOrDescription
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion

    }
}
