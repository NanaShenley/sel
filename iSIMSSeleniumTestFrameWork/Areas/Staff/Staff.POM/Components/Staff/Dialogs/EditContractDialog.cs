using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;
using Staff.POM.Components.Staff.Dialogs;

namespace Staff.POM.Components.Staff
{
    public class EditContractDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_employment_contract_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "EmploymentType.dropdownImitator")]
        private IWebElement _employmentType;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'section_menu_Pay Scales']")]
        private IWebElement _payScalesLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'section_menu_Suspensions']")]
        private IWebElement _suspensionsLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'section_menu_Allowances']")]
        private IWebElement _allowancesLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_allowances_button']")]
        private IWebElement _addAllowancesButton;

        [FindsBy(How = How.Name, Using = "AnnualLeaveEntitlementDays")]
        private IWebElement _annualLeaveEntitlement;

        [FindsBy(How = How.Name, Using = "HoursPerWeek")]
        private IWebElement _hoursPerWeek;

        [FindsBy(How = How.Name, Using = "WeeksPerYear")]
        private IWebElement _weeksPerYear;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateField;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateField;

        [FindsBy(How = How.Name, Using = "EmploymentContractOrigin.dropdownImitator")]
        private IWebElement _employmentContractOrigin;

        [FindsBy(How = How.Name, Using = "EmploymentContractDestination.dropdownImitator")]
        private IWebElement _employmentContractDestination;

        public string StartDate
        {
            get { return _startDateField.GetValue(); }
            set
            {
                _startDateField.SetText(value);
                AutomationSugar.WaitForAjaxCompletion();
            }
        }

        public string EndDate
        {
            get { return _endDateField.GetValue(); }
            set
            {
                _endDateField.SetText(value);
                AutomationSugar.WaitForAjaxCompletion();
            }
        }

        public string EmploymentTypeCombobox
        {
            get { return _employmentType.GetValue(); }
            set
            {
                _employmentType.EnterForDropDown(value);
            }
        }

        public string AnnualLeaveEntitlement
        {
            set { _annualLeaveEntitlement.SetText(value); }
            get { return _annualLeaveEntitlement.GetValue(); }
        }

        public string HoursPerWeek
        {
            set { _hoursPerWeek.SetText(value); }
            get { return _hoursPerWeek.GetValue(); }
        }

        public string WeeksPerYear
        {
            set { _weeksPerYear.SetText(value); }
            get { return _weeksPerYear.GetValue(); }
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

        #endregion

        #region PayScales Grid

        public GridComponent<PayScales> PayScalesTable
        {
            get
            {
                if(SeleniumHelper.DoesElementExist(By.CssSelector("[data-maintenance-container='EmploymentContractPayScales']")))
                {
                    return new GridComponent<PayScales>(By.CssSelector("[data-maintenance-container='EmploymentContractPayScales']"), DialogIdentifier);
                }
                else
                {
                    return null;
                }
            }
        }

        public class PayScales : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='PayScaleDescription']")]
            private IWebElement _scale;

            [FindsBy(How = How.CssSelector, Using = "[name$='Point']")]
            private IWebElement _scalePoint;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _editButton;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pay_scale_button']")]
            private IWebElement _addPayScaleButton;

            public string StartDate
            {
                get
                {
                    return _startDate.GetValue();
                }
            }

            public string EndDate
            {
                get
                {
                    return _endDate.GetValue();
                }
            }

            public string Scale
            {
                get
                {
                    return _scale.GetValue();
                }
            }

            public string ScalePoint
            {
                get
                {
                    return _scalePoint.GetValue();
                }
            }

            public PayScaleOnContractDialog EditOnContract()
            {
                _editButton.ClickByJS();
                AutomationSugar.WaitForAjaxCompletion();
                return new PayScaleOnContractDialog();
            }

            public bool DoesPayScaleAddButtonExist()
            {
                AutomationSugar.WaitForAjaxCompletion();
                return SeleniumHelper.DoesElementExist(By.CssSelector("[data-automation-id='add_pay_scale_button']"));
            }

            public bool IsAddNewRecordButtonEnabled
            {
                get
                {
                    AutomationSugar.WaitForAjaxCompletion();
                    string addPayScaleButton = SeleniumHelper.FindElement(By.CssSelector("[data-automation-id='add_pay_scale_button']")).GetAttribute("disabled");
                    return string.IsNullOrEmpty(addPayScaleButton) ? true : false;
                }
            }
        }

        #endregion

        #region SalaryRange grid

        public GridComponent<SalaryRanges> SalaryRangesTable
        {
            get
            {
                if (SeleniumHelper.DoesElementExist(By.CssSelector("[data-maintenance-container='EmploymentContractSalaryRanges']")))
                {
                    return new GridComponent<SalaryRanges>(By.CssSelector("[data-maintenance-container='EmploymentContractSalaryRanges']"), DialogIdentifier);
                }
                else
                {
                    return null;
                }
            }
        }

        public class SalaryRanges : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='SalaryRangeDescription']")]
            private IWebElement _salaryRangeDescription;

            [FindsBy(How = How.CssSelector, Using = "[name$='AnnualSalary']")]
            private IWebElement _annualSalary;

            [FindsBy(How = How.CssSelector, Using = "[name$='ActualSalary']")]
            private IWebElement _actualSalary;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _editButton;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_salary_range_button']")]
            private IWebElement _addSalaryRangeButton;


            public bool DoesSalaryRangeAddButtonExist()
            {
                AutomationSugar.WaitForAjaxCompletion();
                return SeleniumHelper.DoesElementExist(By.CssSelector("[data-automation-id='add_salary_range_button']"));
            }

            public bool IsAddNewRecordButtonEnabled
            {
                get
                {
                    AutomationSugar.WaitForAjaxCompletion();
                    string addSalaryRangeButton = SeleniumHelper.FindElement(By.CssSelector("[data-automation-id='add_salary_range_button']")).GetAttribute("disabled");
                    return string.IsNullOrEmpty(addSalaryRangeButton) ? true : false;
                }
            }

            public string StartDate
            {
                get
                {
                    return _startDate.GetValue();
                }
            }

            public string EndDate
            {
                get
                {
                    return _endDate.GetValue();
                }
            }

            public string SalaryRangeDescription
            {
                get
                {
                    return _salaryRangeDescription.GetValue();
                }
            }

            public string AnnualSalary
            {
                get
                {
                    return _annualSalary.GetValue();
                }
            }

            public SelectSalaryRangeDialog EditSalaryRange()
            {
                _editButton.ClickByJS();
                AutomationSugar.WaitForAjaxCompletion();
                return new SelectSalaryRangeDialog();
            }
        }


        #endregion

        #region Tables

        public GridComponent<AllowancesRow> AllowancesTable
        {
            get
            {
                return new GridComponent<AllowancesRow>(By.CssSelector("[data-maintenance-container='EmploymentContractAllowances']"), ComponentIdentifier);
            }
        }

        public class AllowancesRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='AllowanceDescription']")]
            private IWebElement _allowance;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='DisplayAmount']")]
            private IWebElement _amount;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _editButton;

            public string Allowance
            {
                set { _allowance.SetText(value); }
                get { return _allowance.GetAttribute("value"); }
            }

            public string StartDate
            {
                get
                {
                    return _startDate.GetValue();
                }
            }

            public string EndDate
            {
                get
                {
                    return _endDate.GetValue();
                }
            }

            public string Amount
            {
                get
                {
                    return _amount.GetValue();
                }
            }

            public StaffContractAllowanceDialog Edit()
            {
                _editButton.ClickByJS();
                AutomationSugar.WaitForAjaxCompletion();
                return new StaffContractAllowanceDialog();
            }
        }

        public void ClickAddStaffRole()
        {
            AutomationSugar.WaitFor(new ByChained(DialogIdentifier, SimsBy.AutomationId("add_staff_role_button")));
            AutomationSugar.ClickOn(new ByChained(DialogIdentifier, SimsBy.AutomationId("add_staff_role_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }     

        #endregion

        public GridComponent<AddContractDetailDialog.StaffRoleGridRow> StaffRolesTable
        {
            get
            {
                return new GridComponent<AddContractDetailDialog.StaffRoleGridRow>(By.CssSelector("[data-maintenance-container='EmploymentContractRoles']"), ComponentIdentifier);
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

        #region Actions

        public void ScrollToPayScalesPanel()
        {
            _payScalesLink.ScrollToByAction();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des: Scroll to allowances panel of staff contract dialog
        /// </summary>
        public void ScrollToAllowancesPanel()
        {
            // click if not open
            if (_allowancesLink.GetAttribute("aria-expanded").Trim().ToLower().Equals("false"))
            {
                _allowancesLink.Click();
            }
            else
            {
                _allowancesLink.Click();
                Wait.WaitLoading();
                _allowancesLink.Click();
            }
            Wait.WaitLoading();
        }

        public void ClickAddAllowance()
        {
            AutomationSugar.ClickOn(new ByChained(DialogIdentifier, SeSugar.Automation.SimsBy.AutomationId("add_allowance_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void DeleteTableRow(AllowancesRow row)
        {
            if (row != null)
            {
                row.ClickDelete();
                IWebElement confirmButton = SeleniumHelper.Get(By.CssSelector("[data-automation-id='Yes_button']"));
                Retry.Do(confirmButton.Click);
            }
        }
        #endregion
    }
}
