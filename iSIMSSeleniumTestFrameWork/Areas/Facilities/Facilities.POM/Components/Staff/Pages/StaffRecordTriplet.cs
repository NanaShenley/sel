﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
{
    public class StaffRecordTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_record_triplet"); }
        }

        public StaffRecordTriplet()
        {
            _searchCriteria = new StaffRecordSearchPage(this);
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_staff_button']")]
        private IWebElement _addNewStaffButton;

        #region Search

        public class StaffRecordSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly StaffRecordSearchPage _searchCriteria;
        public StaffRecordSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Action


        public AddNewStaffDialog CreateNewStaff()
        {
            _addNewStaffButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddNewStaffDialog();
        }

        public AddNewStaffDialog CreateStaff()
        {
            _addNewStaffButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddNewStaffDialog();
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Yes_button']")]
        private IWebElement _confirmDeleteButton;

        /// <summary>
        /// Description: Click "Yes" button on confirm delete
        /// </summary>
        public void ConfirmDeleteTableRow()
        {
            if (_confirmDeleteButton.IsExist())
            {
                _confirmDeleteButton.ClickByJS();
            }
        }

        /// <summary>
        /// Description : Confirm Continue delete
        /// </summary>
        public void ConfirmDeleteTableRowDialog()
        {
            if (_confirmDeleteButton.IsExist())
            {
                _confirmDeleteButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                var confirmDeleteDialog = ConfirmDeleteDialog.Create();
                confirmDeleteDialog.Delete();
            }
        }

        #endregion

    }
}
