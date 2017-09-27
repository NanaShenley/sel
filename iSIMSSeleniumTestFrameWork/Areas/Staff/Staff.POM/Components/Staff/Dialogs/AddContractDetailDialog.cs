using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Base;
using Staff.POM.Helper;

using System.Threading;
using SeSugar.Automation;
using Staff.POM.Components.Staff.Dialogs;
namespace Staff.POM.Components.Staff
{
    public class AddContractDetailDialog : BaseDialogComponent
    {                
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_employment_contract_dialog"); }
        }

        #region Properties
        
        [FindsBy(How = How.Name, Using = "ServiceTerm.dropdownImitator")]
        private IWebElement _serviceTerm;

        [FindsBy(How = How.Name, Using = "EmploymentType.dropdownImitator")]
        private IWebElement _employmentType;

        [FindsBy(How = How.Name, Using = "PostType.dropdownImitator")]
        private IWebElement _postType;

        [FindsBy(How = How.Name, Using = "EmploymentContractOrigin.dropdownImitator")]
        private IWebElement _employmentContractOrigin;

        [FindsBy(How = How.Name, Using = "EmploymentContractDestination.dropdownImitator")]
        private IWebElement _employmentContractDestination;

        [FindsBy(How = How.Name, Using = "HoursPerWeek")]
        private IWebElement _hourPerWeek;

        [FindsBy(How = How.Name, Using = "WeeksPerYear")]
        private IWebElement _weekPerYear;

        [FindsBy(How = How.Name, Using = "AnnualLeaveEntitlementDays")]
        private IWebElement _annualLeaveEntitlementDays;

        [FindsBy(How = How.Name, Using = "FTE")]
        private IWebElement _FTE;

        [FindsBy(How = How.Name, Using = "ProRata")]
        private IWebElement _proRata;
        
        [FindsBy(How = How.Name, Using = "FinancialSubGroup.dropdownImitator")]
        private IWebElement _finacialSubGroup;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _OKButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pay_scale_button']")]
        private IWebElement _addPayScaleButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_salary_range_salary_record_button']")]
        private IWebElement _addSalaryRangeButton;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateField;

        public bool DoesSalaryRangeAddButtonExist()
        {
            AutomationSugar.WaitForAjaxCompletion();
            return SeleniumHelper.DoesElementExist(By.CssSelector("[data-automation-id='add_salary_range_salary_record_button']"));
        }

        public bool DoesPayScaleAddButtonExist()
        {
            AutomationSugar.WaitForAjaxCompletion();
            return SeleniumHelper.DoesElementExist(By.CssSelector("[data-automation-id='add_pay_scale_salary_record_button']"));
        }

        public bool IsPayScaleGridAddButtonEnabled
        {
            get 
            {
                string addPayScaleButton = _addPayScaleButton.GetAttribute("class").ToLower();
                return !addPayScaleButton.Contains("disabled"); 
            }
        }

        public bool IsSalaryRangeGridAddButtonEnabled
        {
            get
            {
                string addSalaryRangeButton = _addSalaryRangeButton.GetAttribute("class").ToLower();
                return !addSalaryRangeButton.Contains("disabled");
            }
        }

        public string StartDate
        {
            get { return _startDateField.GetText(); }
            set
            {
                _startDateField.SetText(value);
                AutomationSugar.WaitForAjaxCompletion();
            }
        }

        public string EmploymentContractOrigin
        {
            get { return _employmentContractOrigin.GetValue(); }
            set { _employmentContractOrigin.EnterForDropDown(value); }
        }

        public string EmploymentContractDestination
        {
            get { return _employmentContractDestination.GetValue(); }
            set { _employmentContractDestination.EnterForDropDown(value); }
        }

        public string ServiceTermCombobox
        {
            get { return _serviceTerm.GetText(); }
            set
            {
                _serviceTerm.EnterForDropDown(value);
                AutomationSugar.WaitUntilStale(SimsBy.AutomationId("section_menu_Contract Details"));
                Refresh();
            }
        }
        
        public string EmploymentTypeCombobox
        {
            get { return _employmentType.GetText(); }
            set
            {
                _employmentType.EnterForDropDown(value);
            }
        }

        public string PostTypeCombobox
        {
            get { return _postType.GetText(); }
            set { _postType.EnterForDropDown(value); }
        }

        public string HourPerWeek
        {
            get { return _hourPerWeek.GetText(); }
            set
            {
                _hourPerWeek.ClickByJS();
                AutomationSugar.WaitForAjaxCompletion();
                _hourPerWeek.SetText(value);
            }
        }
        
        public string WeekPerYear
        {
            get { return _weekPerYear.GetText(); }
            set 
            {
                _weekPerYear.ClickByJS();
                AutomationSugar.WaitForAjaxCompletion();
                _weekPerYear.SetText(value);
            }
        }
    
        public string AnnualLeaveEntitlementDays
        {
            get { return _annualLeaveEntitlementDays.GetText(); }
            set 
            {
                Retry.Do(_annualLeaveEntitlementDays.Click);
                AutomationSugar.WaitForAjaxCompletion();
                _annualLeaveEntitlementDays.SetText(value);
            }
        }

        public string FTE
        {
            get { return _FTE.GetText(); }
            set 
            {
                Retry.Do(_FTE.Click);
                AutomationSugar.WaitForAjaxCompletion();
                _FTE.SetText(value);
            }
        }

        public string ProRata
        {
            get { return _proRata.GetText(); }
            set 
            {
                Retry.Do(_proRata.Click);
                AutomationSugar.WaitForAjaxCompletion();
                _proRata.SetText(value);
            }
        }

        public void ClickFTEUpdate()
        {
            Retry.Do(_FTE.Click);
            AutomationSugar.WaitForAjaxCompletion();
        }

        public string FinacialSubGroupCombobox
        {
            get { return _finacialSubGroup.GetText(); }
            set { _finacialSubGroup.EnterForDropDown(value); }
        }

        #endregion

        #region Actions

        public StaffRecordPage AddContract()
        {
            _OKButton.Click();
            AutomationSugar.WaitForAjaxCompletion();
            return new StaffRecordPage();            
        }

        public SelectPayScaleDialog ClickAddPayScale()
        {
            _addPayScaleButton.ClickByJS();
            AutomationSugar.WaitForAjaxCompletion();
            return new SelectPayScaleDialog();
        }

        public SelectSalaryRangeDialog ClickAddSalaryRange()
        {
            _addSalaryRangeButton.ClickByJS();
            AutomationSugar.WaitForAjaxCompletion();
            return new SelectSalaryRangeDialog();
        }

        public void ClickAddStaffRole()
        {
            AutomationSugar.WaitFor(new ByChained(DialogIdentifier, SimsBy.AutomationId("add_staff_role_button")));
            AutomationSugar.ClickOn(new ByChained(DialogIdentifier, SimsBy.AutomationId("add_staff_role_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }     

        #endregion

        public GridComponent<StaffRoleGridRow> StaffRoles
        {
            get
            {
                return new GridComponent<StaffRoleGridRow>(By.CssSelector("[data-maintenance-container='EmploymentContractRoles']"), ComponentIdentifier);
            }
        }

        public class StaffRoleGridRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[id$='StaffRole_dropdownImitator']")]
            private IWebElement _staffRole;

            [FindsBy(How = How.CssSelector, Using = "[name$='.StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='.EndDate']")]
            private IWebElement _endDate;

            public string StaffRole
            {
                set
                {
                    _staffRole.EnterForDropDown(value);
                    AutomationSugar.WaitForAjaxCompletion();
                }
                get { return _staffRole.GetAttribute("value"); }
            }

            public string StartDate
            {
                set { _startDate.SetText(value); }
                get { return _startDate.GetValue(); }
            }

            public string EndDate
            {
                set { _endDate.SetText(value); }
                get { return _endDate.GetValue(); }
            }
        }
    }
}