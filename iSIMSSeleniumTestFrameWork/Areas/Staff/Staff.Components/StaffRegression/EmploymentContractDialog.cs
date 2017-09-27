using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using SharedComponents.Helpers;
using OpenQA.Selenium.Support.PageObjects;
using System.Threading;

namespace Staff.Components.StaffRegression
{
    public class EmploymentContractPayScaleDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_employment_contract_payscale_dialog"); }
        }

        #region Page properties
        [FindsBy(How = How.Name, Using = "PayScale.dropdownImitator")]
        private IWebElement _payScale;

        [FindsBy(How = How.Name, Using = "Point")]
        private IWebElement _point;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDate;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDate;

        public string PayScale
        {
            set { _payScale.ChooseSelectorOption(value); }
            get { return _payScale.GetValue(); }
        }

        public string Point
        {
            set { Retry.Do(() => { _point.SetText(value); }, catchAction: Refresh); }
            get { return _point.GetValue(); }
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
        #endregion
    }

    public class EmploymentContractAllowanceDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_employment_contract_allowance_dialog"); }
        }

        #region Page properties
        [FindsBy(How = How.Name, Using = "Allowance.dropdownImitator")]
        private IWebElement _allowance;

        [FindsBy(How = How.Name, Using = "AdditionalPaymentCategoryDescription")]
        private IWebElement _additionalPaymentCategoryDescription;

        [FindsBy(How = How.Name, Using = "AllowanceType.dropdownImitator")]
        private IWebElement _allowanceType;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDate;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDate;

        [FindsBy(How = How.Name, Using = "PayFactor")]
        private IWebElement _payFactor;

        [FindsBy(How = How.Name, Using = "Amount")]
        private IWebElement _amount;

        [FindsBy(How = How.Name, Using = "Reason")]
        private IWebElement _reason;

        public string Allowance
        {
            set
            {
                _allowance.ChooseSelectorOption(value);
                Thread.Sleep(1000);
                Refresh();
            }
            get { return _allowance.GetValue(); }
        }

        public string AdditionalPaymentCategoryDescription
        {
            set { _additionalPaymentCategoryDescription.SetText(value); }
            get { return _additionalPaymentCategoryDescription.GetValue(); }
        }

        public string AllowanceType
        {
            set { Retry.Do(() => { _allowanceType.ChooseSelectorOption(value); }, catchAction: Refresh); }
            get { return _allowanceType.GetValue(); }
        }

        public string StartDate
        {
            set { Retry.Do(() => { _startDate.SetText(value); }, catchAction: Refresh); }
            get { return _startDate.GetValue(); }
        }

        public string EndDate
        {
            set { Retry.Do(() => { _endDate.SetText(value); }, catchAction: Refresh); }
            get { return _endDate.GetValue(); }
        }

        public string PayFactor
        {
            set { _payFactor.SetText(value); }
            get { return _payFactor.GetValue(); }
        }
        public string Amount
        {
            set { _amount.SetText(value); }
            get { return _amount.GetValue(); }
        }

        public string Reason
        {
            set { _reason.SetText(value); }
            get { return _reason.GetValue(); }
        }
        #endregion
    }

    public class EmploymentContractDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_employment_contract_dialog"); }
        }

        #region Page properties
        [FindsBy(How = How.Name, Using = "ServiceTerm.dropdownImitator")]
        private IWebElement _serviceTerm;

        [FindsBy(How = How.Name, Using = "EmploymentType.dropdownImitator")]
        private IWebElement _employmentType;

        [FindsBy(How = How.Name, Using = "PostType.dropdownImitator")]
        private IWebElement _postType;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDate;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDate;

        public string ServiceTerm
        {
            set { _serviceTerm.ChooseSelectorOption(value); }
            get { return _serviceTerm.GetValue(); }
        }
        public string EmploymentType
        {
            set { _employmentType.ChooseSelectorOption(value); }
            get { return _employmentType.GetValue(); }
        }
        public string PostType
        {
            set { _postType.ChooseSelectorOption(value); }
            get { return _postType.GetValue(); }
        }

        public string StartDate
        {
            set
            {
                _startDate.SetText(value);
                Thread.Sleep(1000);
                Refresh();
            }
            get { return _startDate.GetValue(); }
        }

        public string EndDate
        {
            set
            {
                Retry.Do(() => { _endDate.SetText(value); });
                Thread.Sleep(1000);
                Refresh();
            }
            get { return _endDate.GetValue(); }
        }
        #endregion

        public class Role
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='StaffRole.dropdownImitator']")]
            private IWebElement _staffRole;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            public string StaffRole
            {
                set { _staffRole.ChooseSelectorOption(value); }

                get { return _staffRole.GetValue(); }
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

        public GridComponent<Role> Roles
        {
            get
            {
                GridComponent<Role> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Role>(By.CssSelector("[data-maintenance-container='EmploymentContractRoles']"));
                });
                return returnValue;
            }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pay_scales_button']")]
        private IWebElement _addPayScaleButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_allowances_button']")]
        private IWebElement _addAllowanceButton;

        public EmploymentContractPayScaleDialog AddPayScale()
        {
            Retry.Do(_addPayScaleButton.Click);
            return new EmploymentContractPayScaleDialog();
        }

        public EmploymentContractAllowanceDialog AddAllowance()
        {
            Retry.Do(_addAllowanceButton.Click);
            return new EmploymentContractAllowanceDialog();
        }
    }
}
