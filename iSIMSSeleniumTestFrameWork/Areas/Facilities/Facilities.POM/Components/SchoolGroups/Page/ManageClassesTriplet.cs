using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;

namespace POM.Components.SchoolGroups
{
    public class ManageClassesTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("manage_primary_class_triplet"); }
        }

        public ManageClassesTriplet()
        {
            _searchCriteria = new ManageClassesSearchPage(this);
        }

        #region Search

        private readonly ManageClassesSearchPage _searchCriteria;
        public ManageClassesSearchPage SearchCriteria { get { return _searchCriteria; } }

        public class ManageClassesSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Full Name']")]
            private IWebElement _classFullName;

            public string ClassFullName
            {
                get { return _classFullName.Text; }
            }
        }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _CreateButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        public ManageClassesPage Create()
        {
            //SeleniumHelper.Get(SimsBy.AutomationId("create_button")).ClickByJS();
            _CreateButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ManageClassesPage();
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Select Class if class is existing
        /// </summary>

        public void SelectSearchTile(ManageClassesSearchResultTile classesTile)
        {
            if (classesTile != null)
            {
                classesTile.Click();
                
            }
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new ConfirmDeleteDialog();
                confirmDialog.ClickContinueDelete();
            }
        }

      #endregion
    }

    public class ManageClassesSearchPage : SearchCriteriaComponent<ManageClassesTriplet.ManageClassesSearchResultTile>
    {
        public ManageClassesSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        private IWebElement _searchByAcademicYear;

        public string SearchByAcademicYear
        {
            get { return _searchByAcademicYear.GetValue(); }
            set { _searchByAcademicYear.EnterForDropDown(value); }
        }

        #endregion Search
    }

}
