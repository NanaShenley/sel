using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;


namespace POM.Components.SchoolGroups
{
    public class ManageYearGroupsTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("manage_year_group_triplet"); }
        }

        public ManageYearGroupsTriplet()
        {
            _searchCriteria = new ManageYearGroupsSearchPage(this);
        }

        #region Search

        public class YearGroupsSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Full Name']")]
            private IWebElement _fullName;

            public string FullName
            {
                get { return _fullName.GetText().Trim(); }
            }
        }

        private readonly ManageYearGroupsSearchPage _searchCriteria;

        public ManageYearGroupsSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _addButton;

     

        #endregion

        #region Actions

        public ManageYearGroupsPage AddYearGroup()
        {
            Retry.Do(_addButton.Click);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new ManageYearGroupsPage();
        }

     
        #endregion
    
    }

    public class ManageYearGroupsSearchPage : SearchCriteriaComponent<ManageYearGroupsTriplet.YearGroupsSearchResultTile>
    {
        public ManageYearGroupsSearchPage(BaseComponent component) : base(component) { }

        #region Properties

        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        private IWebElement _academicYearDropdown;

        public string AcademicYear
        {
            set { _academicYearDropdown.EnterForDropDown(value); }
            get { return _academicYearDropdown.GetValue(); }
        }

        #endregion

    }

}
