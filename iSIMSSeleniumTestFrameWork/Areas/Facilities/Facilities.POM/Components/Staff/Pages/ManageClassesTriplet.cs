using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class ManageClassesTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("manage_primary_class_triplet"); }
        }

        public ManageClassesTriplet()
        {
            _searchCriteria = new ManageClassSearchPage(this);
        }

        #region Search

        private ManageClassSearchPage _searchCriteria;

        public ManageClassSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        public class ManageClassesSearchResultTitle : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        #endregion
    }

    public class ManageClassSearchPage : SearchCriteriaComponent<ManageClassesTriplet.ManageClassesSearchResultTitle>
    {
        public ManageClassSearchPage(BaseComponent context) : base(context) { }

        #region Page properties
        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        private IWebElement _academicYearDropdown;

        public string AcademicYear
        {
            get { return _academicYearDropdown.GetText(); }
            set { _academicYearDropdown.EnterForDropDown(value); }
        }
        #endregion
    }
}
