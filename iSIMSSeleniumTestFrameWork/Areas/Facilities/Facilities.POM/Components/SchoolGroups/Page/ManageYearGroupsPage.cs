using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;
using POM.Components.Common;


namespace POM.Components.SchoolGroups
{
    public class ManageYearGroupsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("manage_year_group_detail"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.Name, Using = "FullName")]
        private IWebElement _fullNameTextBox;

        [FindsBy(How = How.Name, Using = "ShortName")]
        private IWebElement _shortNameTextBox;

        [FindsBy(How = How.Name, Using = "DisplayOrder")]
        private IWebElement _displayOrderTextBox;

        [FindsBy(How = How.Name, Using = "SchoolNCYear.dropdownImitator")]
        private IWebElement _curriculumDropdown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Year Group Details']")]
        private IWebElement _yearGroupDetailsAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Associated Groups']")]
        private IWebElement _associcatedGroupsAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Staff Details']")]
        private IWebElement _staffDetailsAccordion;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        private IWebElement _warningMessage;

        [FindsBy(How = How.CssSelector, Using = ".alert-warning.validation-summary-errors")]
        private IWebElement _warningDelete;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_staff_button']")]
        private IWebElement _addstaff;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_active_history_button']")]
        private IWebElement _addactivehistory;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_classes_button']")]
        private IWebElement _addClass;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_head_of_year_button']")]
        private IWebElement _addHeadOfYear;
        
        public string FullName
        {
            set { _fullNameTextBox.SetText(value); }
            get { return _fullNameTextBox.GetValue(); }
        }

        public string ShortName
        {
            set { _shortNameTextBox.SetText(value); }
            get { return _shortNameTextBox.GetValue(); }
        }

        public string DisplayOrder
        {
            set { _displayOrderTextBox.SetText(value); }
            get { return _displayOrderTextBox.GetValue(); }
        }

        public string CurriculumYear
        {
            set { _curriculumDropdown.EnterForDropDown(value); }
            get { return _curriculumDropdown.GetValue(); }
        }

        #endregion

        #region Actions

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public void ScrollToYearGroupDetails()
        {
            if (_yearGroupDetailsAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _yearGroupDetailsAccordion.ClickByJS();
            }
            else
            {
                _yearGroupDetailsAccordion.ClickByJS();
                Wait.WaitLoading();
                _yearGroupDetailsAccordion.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='YearGroupSetMemberships']"));
        }

        public void ScrollToAssociatedGroup()
        {
            if (_associcatedGroupsAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _associcatedGroupsAccordion.ClickByJS();
            }
            else
            {
                _associcatedGroupsAccordion.ClickByJS();
                Wait.WaitLoading();
                _associcatedGroupsAccordion.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='YearGroupPrimaryClassAssociations']"));
        }

        public void ScrollToStaffDetails()
        {
            if (_staffDetailsAccordion.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _staffDetailsAccordion.ClickByJS();
            }
            else
            {
                _staffDetailsAccordion.ClickByJS();
                Wait.WaitLoading();
                _staffDetailsAccordion.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='HeadOfYears']"));
        }

        public bool IsMessageSuccessAppear()
        {
            return _messageSuccess.IsExist();
        }

        public bool IsWarningMessageIsDisplayed()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_error']"));
            return SeleniumHelper.DoesWebElementExist(SimsBy.CssSelector("[data-automation-id='status_error']"));
        }

        public bool IsWarningDeleteAppear()
        {
            try
            {
                return SeleniumHelper.FindElement(SimsBy.CssSelector(".alert-warning.validation-summary-errors")).IsExist();
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                Retry.Do(_deleteButton.ClickByJS);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                var deleteDialog = new DeleteConfirmationDialog();
                deleteDialog.ConfirmDelete();
            }
        }

        public void AddActivehistory()
        {
            _addactivehistory.ClickByJS();
        }

        public void AddClass()
        {
            _addClass.ClickByJS();
        }

        public void AddHeadOfYear()
        {
            _addHeadOfYear.ClickByJS();
        }

        public void AddStaff()
        {
            _addstaff.ClickByJS();
        }

        #endregion

        #region ActiveHistory Grid

        public GridComponent<ActiveHistoryRow> ActiveHistory
        {
            get
            {
                GridComponent<ActiveHistoryRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ActiveHistoryRow>(By.CssSelector("[data-maintenance-container='YearGroupSetMemberships']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ActiveHistoryRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            public string StartDate
            {
                set { _startDate.SetDateTime(value); }
                get { return _startDate.GetDateTime(); }
            }

            public string EndDate
            {
                set { _endDate.SetDateTime(value); }
                get { return _endDate.GetDateTime(); }
            }
        }

        #endregion

        #region Classes Grid

        public GridComponent<ClassRow> Classes
        {
            get
            {
                GridComponent<ClassRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ClassRow>(By.CssSelector("[data-maintenance-container='YearGroupPrimaryClassAssociations']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ClassRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='PrimaryClass.dropdownImitator']")]
            private IWebElement _class;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            public string Class
            {
                set { _class.EnterForDropDown(value); }
                get { return _class.GetValue(); }
            }

            public string StartDate
            {
                set { _startDate.SetDateTime(value); }
                get { return _startDate.GetDateTime(); }
            }

            public string EndDate
            {
                set { _endDate.SetDateTime(value); }
                get { return _endDate.GetDateTime(); }
            }
        }

        #endregion

        #region Head of year

        public GridComponent<HeadOfYearRow> HeadOfYear
        {
            get
            {
                GridComponent<HeadOfYearRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<HeadOfYearRow>(By.CssSelector("[data-maintenance-container='HeadOfYears']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class HeadOfYearRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Staff.dropdownImitator']")]
            private IWebElement _headOfYear;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            public string HeadOfYear
            {
                set { _headOfYear.EnterForDropDown(value); }
                get { return _headOfYear.GetValue(); }
            }

            public string StartDate
            {
                set { _startDate.SetDateTime(value); }
                get { return _startDate.GetDateTime(); }
            }

            public string EndDate
            {
                set { _endDate.SetDateTime(value); }
                get { return _endDate.GetDateTime(); }
            }
        }

        #endregion

        #region Staff Grid

        public GridComponent<StaffRow> Staff
        {
            get
            {
                GridComponent<StaffRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffRow>(By.CssSelector("[data-maintenance-container='YearGroupStaffs']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class StaffRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Staff.dropdownImitator']")]
            private IWebElement _staff;

            [FindsBy(How = How.CssSelector, Using = "[name$='StaffRole.dropdownImitator']")]
            private IWebElement _pastoralRole;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            public string Staff
            {
                set { _staff.EnterForDropDown(value); }
                get { return _staff.GetValue(); }
            }

            public string PastoralRole
            {
                set { _pastoralRole.EnterForDropDown(value); }
                get { return _pastoralRole.GetValue(); }
            }

            public string StartDate
            {
                set { _startDate.SetDateTime(value); }
                get { return _startDate.GetDateTime(); }
            }

            public string EndDate
            {
                set { _endDate.SetDateTime(value); }
                get { return _endDate.GetDateTime(); }
            }
        }

        #endregion
    }
}
