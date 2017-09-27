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
    public class ManageUserDefinedPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("manage_user_defined_group_detail"); }
            
        }

        public ManageUserDefinedPage Create()
        {
            Wait.WaitUntilDisplayed(By.CssSelector("[data-automation-id='well_know_action_save']"));
            return new ManageUserDefinedPage();
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='select_effective_date_range_button']")]
        private IWebElement _selectDateRangeButton;

        [FindsBy(How = How.Name, Using = "FullName")]
        private IWebElement _groupFullNameTextbox;

        [FindsBy(How = How.Name, Using = "ShortName")]
        private IWebElement _groupShortNameTextbox;

        [FindsBy(How = How.Name, Using = "UserDefinedGroupPurpose.dropdownImitator")]
        private IWebElement _purposeDropdown;

        [FindsBy(How = How.Name, Using = "IsVisible")]
        private IWebElement _visibilityCheckbox;

        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _noteTextArea;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_supervisors_button']")]
        private IWebElement _addSupervisorButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pupils_button']")]
        private IWebElement _addPupilButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_members_button']")]
        private IWebElement _addMemberButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Group Details']")]
        private IWebElement _sectionGroupDetail;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Supervisors']")]
        private IWebElement _sectionSupervisor;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Members']")]
        private IWebElement _sectionMember;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_Cancel']")]
        private IWebElement _cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        private IWebElement _errorStatus;

        [FindsBy(How = How.CssSelector, Using = ".validation-summary-errors")]
        private IWebElement _errorMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _sucessMessage;

        [FindsBy(How = How.Name, Using = "EffectiveDate")]
        private IWebElement _effectiveDateRangeTextbox;

        public string FullName
        {
            get
            {
                return _groupFullNameTextbox.GetValue();
            }
            set
            {
                _groupFullNameTextbox.SetText(value);
            }
        }

        public string ShortName
        {
            get { return _groupShortNameTextbox.GetValue(); }
            set { _groupShortNameTextbox.SetText(value); }
        }

        public string Purpose
        {
            set
            {
                _purposeDropdown.EnterForDropDown(value);
            }
            get
            {
                return _purposeDropdown.GetValue();
            }
        }

        public bool Visibility
        {
            set { _visibilityCheckbox.Set(value); }
            get { return _visibilityCheckbox.IsChecked(); }
        }

        public string Notes
        {
            set { _noteTextArea.SetText(value); }
            get { return _noteTextArea.GetText(); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage.GetText(); }
        }

        public string EffectiveDateRange
        {
            get 
            {
                string effectiveDateRange = _effectiveDateRangeTextbox.GetValue();
                string startDate = effectiveDateRange.Split('-')[0].Trim();
                string endDate = effectiveDateRange.Split('-')[1].Trim();
                string dateFormat = "M/d/yyyy";
                if (SeleniumHelper.GetBrowserName().Contains("chrome"))
                {
                    dateFormat = "d/M/yyyy";
                }
                return String.Format("{0} - {1}", SeleniumHelper.Format(startDate, dateFormat), SeleniumHelper.Format(endDate, dateFormat));
            }
        }

        #endregion

        #region Table
        public GridComponent<Supervisor> SupervisorTable
        {
            get
            {
                GridComponent<Supervisor> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Supervisor>(By.CssSelector("[data-maintenance-container='GroupSupervisors']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<Member> MemberTable
        {
            get
            {
                GridComponent<Member> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Member>(By.CssSelector("[data-maintenance-container='GroupMembers']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class Supervisor : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='.PreferredListName']")]
            private IWebElement _nameTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='SupervisorRole.dropdownImitator']")]
            private IWebElement _roleDropdown;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Main']")]
            private IWebElement _mainCheckbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.StartDate']")]
            private IWebElement _fromTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.EndDate']")]
            private IWebElement _toTextbox;

            public string Name
            {
                get
                {
                    return _nameTextbox.GetValue().Replace("  ", " ");
                }
            }

            public string Role
            {
                set
                {
                    _roleDropdown.EnterForDropDown(value);
                }

                get
                {
                    return _roleDropdown.GetValue();
                }
            }

            public bool Main
            {
                set { _mainCheckbox.Set(value); }
                get { return _mainCheckbox.IsChecked(); }
            }


            public string From
            {
                set { _fromTextbox.SetDateTime(value); }
                get { return _fromTextbox.GetDateTime(); }
            }

            public string To
            {
                get { return _toTextbox.GetDateTime(); }
                set { _toTextbox.SetDateTime(value); }
            }

        }

        public class Member : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='.PreferredListName']")]
            private IWebElement _nameTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.StartDate']")]
            private IWebElement _fromTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.EndDate']")]
            private IWebElement _toTextbox;

            public string Name
            {
                get { return _nameTextbox.GetValue().Replace("  ", " "); }
            }

            public string From
            {
                set { _fromTextbox.SetDateTime(value); }
                get { return _fromTextbox.GetDateTime(); }
            }

            public string To
            {
                set { _toTextbox.SetDateTime(value); }
                get { return _toTextbox.GetDateTime(); }
            }

        }

        #endregion

        #region Actions

        public ManageUserDefinedPage SaveValue()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
            return new ManageUserDefinedPage();
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.Click();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                WarningConfirmationDialog deleteDialog = new WarningConfirmationDialog();
                deleteDialog.ConfirmDelete();
            }
        }

        public AddSupervisorsDialogTriplet ClickAddSupervisor()
        {
            _addSupervisorButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AddSupervisorsDialogTriplet();
        }

        public AddPupilsDialogTriplet ClickAddPupil()
        {
            _addPupilButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AddPupilsDialogTriplet();
        }

        public AddMemberDialogTriplet ClickAddMember()
        {
            Wait.WaitForElement(SimsBy.AutomationId("add_members_button"));
            _addMemberButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AddMemberDialogTriplet();
        }

        public void ScrollToGroupDetail()
        {
            if (_sectionGroupDetail.GetAttribute("class").Contains("collapsed"))
            {
                _sectionGroupDetail.ClickByJS();
            }
            else
            {
                _sectionGroupDetail.ClickByJS();
                Wait.WaitLoading();
                _sectionGroupDetail.ClickByJS();
            }
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("[name$='EffectiveDate']"));
        }

        public void ScrollToSupervisor()
        {
            if (_sectionSupervisor.GetAttribute("class").Contains("collapsed"))
            {
                _sectionSupervisor.ClickByJS();
            }
            else
            {
                _sectionSupervisor.ClickByJS();
                Wait.WaitLoading();
                _sectionSupervisor.Click();
            }
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='GroupSupervisors']"));
        }

        public void ScrollToMember()
        {
            if (_sectionMember.GetAttribute("class").Contains("collapsed"))
            {
                _sectionMember.ClickByJS();
            }
            else
            {
                _sectionMember.ClickByJS();
                Wait.WaitLoading();
                _sectionMember.Click();
            }
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='GroupMembers']"));
        }

        public void ClickCancel()
        {
            Wait.WaitUntilDisplayed(SimsBy.AutomationId("well_know_action_Cancel"));
            _cancelButton.ClickByJS();
            //var dialog = new WarningDialog();
            //dialog.DontSave();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public SelectEffectiveDateRangeDialog SelectEffectDateRange()
        {
            _selectDateRangeButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new SelectEffectiveDateRangeDialog();
        }

        public bool IsErrorMessageDisplay()
        {
            return _errorStatus.IsExist();
        }

        public bool IsSuccessMessageDisplayed()
        {
            return _sucessMessage.IsExist();
        }

        #endregion
    }
}
