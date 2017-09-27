using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.SchoolGroups;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilities.POM.Components.SchoolGroups.Page
{
    public class CurriculumStructureTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("workspace"); }
        }

        public CurriculumStructureTriplet()
        {
            this.Refresh();
            _searchCriteria = new CurriculumStructureSearchPage(this);
        }

        #region Search
        
        public class CurriculumStructureSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly CurriculumStructureSearchPage _searchCriteria;
        public CurriculumStructureSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]
        private IWebElement _CreateSchemeDropdownButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='template_marksheet_filter']")]
        private IWebElement _CreateTeachingGroupSchemeButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='marksheet_with_level_new']")]
        private IWebElement _CreatebandSchemeButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        /// 
        public void Create()
        {
            _CreateSchemeDropdownButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }
        public CurriculumStructurePage CreateTeachingGroupSchemePage()
        {
            //SeleniumHelper.Get(SimsBy.AutomationId("create_button")).ClickByJS();
            _CreateTeachingGroupSchemeButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new CurriculumStructurePage();
        }

        public BandSchemePage CreateBandSchemePage()
        {
            //SeleniumHelper.Get(SimsBy.AutomationId("create_button")).ClickByJS();
            _CreatebandSchemeButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new BandSchemePage();
        }

        public void Save()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("well_know_action_save")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Select Class if class is existing
        /// </summary>

        public void SelectSearchTile(CurriculumStructureSearchResultTile schemeTile)
        {
            if (schemeTile != null)
            {
                schemeTile.Click();

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

    public class CurriculumStructureSearchPage : SearchCriteriaComponent<CurriculumStructureTriplet.CurriculumStructureSearchResultTile>
    {
        public CurriculumStructureSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _schemeName;

        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        private IWebElement _searchByAcademicYear;

        public string SearchByAcademicYear
        {
            get { return _searchByAcademicYear.GetValue(); }
            set { _searchByAcademicYear.EnterForDropDown(value); }
        }

        public string SearchBySchemeName
        {
            set { _schemeName.SetText(value); }
            get { return _schemeName.GetValue(); }
        }

        #endregion Search
    }

}

