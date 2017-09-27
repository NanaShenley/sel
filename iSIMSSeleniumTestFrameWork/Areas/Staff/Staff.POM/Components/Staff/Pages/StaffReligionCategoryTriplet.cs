using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class StaffReligionCategoryTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public StaffReligionCategoryTriplet()
        {
            _searchCriteria = new StaffReligionCategorySearch(this);
        }

        #region Search

        private readonly StaffReligionCategorySearch _searchCriteria;
        public StaffReligionCategorySearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Public actions

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Init page Staff Religion Category Search
        /// </summary>
        /// <returns></returns>
        public static StaffReligionCategoryDetailsPage Create()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("create_service_StaffReligion")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new StaffReligionCategoryDetailsPage();
        }

        public void Delete(POM.Components.Staff.StaffReligionCategoryDetailsPage.StaffReligionCategoriesRow record)
        {
            if (record != null)
            {
                record.ClickDelete();
                SeleniumHelper.Get(SimsBy.AutomationId("Yes_button")).ClickByJS();
                SeleniumHelper.Get(SimsBy.AutomationId("well_know_action_save")).ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
        }

        #endregion
    }

    public class StaffReligionCategorySearch : SearchTableCriteriaComponent
    {
        public StaffReligionCategorySearch(BaseComponent parent) : base(parent) { }

        #region Properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _searchTextBox;

        public string CodeOrDescription
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion
    }

}
