using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using POM.Components.Common;

namespace POM.Components.SchoolGroups
{
    public class ManageClassesPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("manage_primary_class_detail"); }
        }


        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _statusMessage;

        [FindsBy(How = How.Name, Using = "FullName")]
        private IWebElement _classFulnameTextBox;

        [FindsBy(How = How.Name, Using = "ShortName")]
        private IWebElement _classShortNameTextBox;

        [FindsBy(How = How.Name, Using = "DisplayOrder")]
        private IWebElement _displayOrderTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Associated Groups']")]
        private IWebElement _associatedGroup;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_active_history_button']")]
        private IWebElement _addactivehistory;
     
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_year_groups_button']")]
        private IWebElement _addyeargroup;
        
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_class_teacher_button']")]
        private IWebElement _addclassteacher;
        
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_staff_button']")]
        private IWebElement _addstaff;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Staff Details']")]
        private IWebElement _staffDetails;

        // Define Active History Table
        public GridComponent<ActiveHistory> ActiveHistoryTable
        {
            get
            {
                GridComponent<ActiveHistory> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ActiveHistory>(By.CssSelector("[data-maintenance-container='PrimaryClassSetMemberships']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }


        public void AddActivehistory()
        {
            _addactivehistory.ClickByJS();
        }

        public class ActiveHistory : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateTextBox;

       
            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateTextBox;
            public string StartDate
            {
                set { _startDateTextBox.SetDateTime(value); }
                get { return _startDateTextBox.GetDateTime(); }
            }
            public string EndDate
            {
                set { _endDateTextBox.SetDateTime(value); }
                get { return _endDateTextBox.GetDateTime(); }
            }

        }

        // Define Year Group Table
        public GridComponent<YearGroups> YearGroupsTable
        {
            get
            {
                GridComponent<YearGroups> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<YearGroups>(By.CssSelector("[data-maintenance-container='YearGroupPrimaryClassAssociations']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public void AddYearGroup()
        {
            _addyeargroup.Click();
        }

        public class YearGroups : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[id$='YearGroup_dropdownImitator']")]
            private IWebElement _yearGroupsDropDown;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateTextBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateTextBox;

            public string YearGroup
            {
                set { _yearGroupsDropDown.EnterForDropDown(value); }
                get { return _yearGroupsDropDown.GetValue(); }
            }
            public string StartDate
            {
                set { _startDateTextBox.SetDateTime(value); }
                get { return _startDateTextBox.GetDateTime(); }
            }
            public string EndDate
            {
                set { _endDateTextBox.SetDateTime(value); }
                get { return _endDateTextBox.GetDateTime(); }
            }

        }

        // Staff details sections
        // Define Class Teacher Table
        public GridComponent<ClassTeacher> ClassTeacherTable
        {
            get
            {
                GridComponent<ClassTeacher> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ClassTeacher>(By.CssSelector("[data-maintenance-container='PrimaryClassTeachers']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public void AddClassTeacher()
        {
            _addclassteacher.ClickByJS();
        }
        public class ClassTeacher : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[id$='Staff_dropdownImitator']")]
            private IWebElement _classTeacherDropDown;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateTextBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateTextBox;

            public string SelectClassTeacher
            {
                set { _classTeacherDropDown.EnterForDropDown(value); }
                get { return _classTeacherDropDown.GetValue(); }
            }

            public string StartDate
            {
                set { _startDateTextBox.SetDateTime(value); }
                get { return _startDateTextBox.GetDateTime(); }
            }
            public string EndDate
            {
                set { _endDateTextBox.SetDateTime(value); }
                get { return _endDateTextBox.GetDateTime(); }
            }

        }

        // Define Staff Table
        public GridComponent<Staff> StaffTable
        {
            get
            {
                GridComponent<Staff> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Staff>(By.CssSelector("[data-maintenance-container='PrimaryClassStaffs']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public void AddStaff()
        {
            _addstaff.ClickByJS();
        }

        public class Staff : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[id$='Staff_dropdownImitator']")]
            private IWebElement _staffDropDown;

            [FindsBy(How = How.CssSelector, Using = "[id$='StaffRole_dropdownImitator']")]
            private IWebElement _pastoralRoleDropDown;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateTextBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateTextBox;

            public string SelectStaff
            {
                set { _staffDropDown.EnterForDropDown(value); }
                get { return _staffDropDown.GetValue(); }
            }

            public string PastoralRoleDropDown
            {
                set { _pastoralRoleDropDown.EnterForDropDown(value); }
                get { return _pastoralRoleDropDown.GetValue(); }
            }

            public string StartDate
            {
                set { _startDateTextBox.SetDateTime(value); }
                get { return _startDateTextBox.GetDateTime(); }
            }

            public string EndDate
            {
                set { _endDateTextBox.SetDateTime(value); }
                get { return _endDateTextBox.GetDateTime(); }
            }

        }
        public string ClassFullName
        {
            set { _classFulnameTextBox.SetText(value); }
            get { return _classFulnameTextBox.GetValue(); }
        }

        public string ClassShortName
        {
            set { _classShortNameTextBox.SetText(value); }
            get { return _classShortNameTextBox.GetValue(); }
        }

        public string DisplayOrder
        {
            set { _displayOrderTextBox.SetText(value); }
            get { return _displayOrderTextBox.GetValue(); }
        }

        #endregion

        #region Actions

        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
        }

        public ConfirmRequiredDialog SaveToDeActive()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ConfirmRequiredDialog();
            Refresh();
        }

        public DeleteConfirmationDialog Delete()
        {
            _deleteButton.Click();
            return new DeleteConfirmationDialog();
        }

        public void DeleteRecord()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new ConfirmDeleteDialog();
                confirmDialog.ClickContinueDelete();
            }
        }


        public bool IsSuccessMessageDisplayed()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_success']"));
            return _statusMessage.IsExist();
        }

        public bool IsWarningMessageIsDisplayed()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_error']"));
            return SeleniumHelper.DoesWebElementExist(SimsBy.CssSelector("[data-automation-id='status_error']"));
        }

        public void ScrollToAssociatedGroup()
        {
            if (_associatedGroup.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _associatedGroup.Click();
            }
            else
            {
                _associatedGroup.ClickByJS();
                Wait.WaitLoading();
                _associatedGroup.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='YearGroupPrimaryClassAssociations']"));
        }
        public void ScrollToStaffDetails()
        {
            if (_staffDetails.GetAttribute("aria-expanded").Trim().Equals("false"))
            {
                _staffDetails.ClickByJS();
            }
            else
            {
                _staffDetails.ClickByJS();
                Wait.WaitLoading();
                _staffDetails.Click();
            }
            Wait.WaitForElementDisplayed(By.CssSelector("[data-maintenance-container='PrimaryClassTeachers']"));
        }

        #endregion

    }
}
