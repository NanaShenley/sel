﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class AllowanceTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public AllowanceTriplet()
        {
            _searchCriteria = new AllowanceSeach(this);
        }

        #region Search

        private readonly AllowanceSeach _searchCriteria;
        public AllowanceSeach SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Click Add button to view allowance detail page
        /// </summary>
        /// <returns></returns>
        public static NewAllowancePage Create()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("add_allowance_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new NewAllowancePage();
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Click "Delete" button on the record of category need to be deleted
        /// </summary>
        /// <param name="record">The row in allowances table need to be deleted</param>
        public void Delete(POM.Components.Staff.AllowanceDetailsPage.AllowancesRow record)
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

    public class AllowanceSeach : SearchTableCriteriaComponent
    {
        public AllowanceSeach(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _searchTextBox;

        public string CodeOrDecription
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion
    }
}
