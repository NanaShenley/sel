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

namespace Staff.POM.Components.Staff.Pages
{
    public class NationalIdentityTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public NationalIdentityTriplet()
        {
            _searchCriteria = new NationalIdentityCategorySearch(this);
        }

        #region Search

        private readonly NationalIdentityCategorySearch _searchCriteria;
        public NationalIdentityCategorySearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Public actions

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Init page National Identity Search
        /// </summary>
        /// <returns></returns>
        public static NationalIdentityDetailsPage Create()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("create_service_NationalIdentity")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new NationalIdentityDetailsPage();
        }


        public void Delete(POM.Components.Staff.Pages.NationalIdentityDetailsPage.NationalIdentitiesRow record)
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

     public class NationalIdentityCategorySearch : SearchTableCriteriaComponent
    {
        public NationalIdentityCategorySearch(BaseComponent parent) : base(parent) { }

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
