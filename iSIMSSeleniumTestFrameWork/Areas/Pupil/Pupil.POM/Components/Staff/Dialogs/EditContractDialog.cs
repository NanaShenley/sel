using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System.Threading;

namespace POM.Components.Staff
{
    public class EditContractDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_employment_contract_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'section_menu_Pay Scales']")]
        private IWebElement _payScalesLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'section_menu_Suspensions']")]
        private IWebElement _suspensionsLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'section_menu_Allowances']")]
        private IWebElement _allowancesLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_allowances_button']")]
        private IWebElement _addAllowancesButton;

        #endregion

        #region PayScales Grid

        public GridComponent<PayScales> PayScalesTable
        {
            get
            {
                GridComponent<PayScales> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PayScales>(By.CssSelector("[data-maintenance-container='EmploymentContractPayScales']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class PayScales
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='PayScaleDescription']")]
            private IWebElement _scale;

            [FindsBy(How = How.CssSelector, Using = "[name$='Point']")]
            private IWebElement _scalePoint;

            [FindsBy(How = How.CssSelector, Using = "data-automation-id='edit..._button']")]
            private IWebElement _editButton;

            public string StartDate
            {
                get
                {
                    return _startDate.GetValue();
                }
            }
            public string Scale
            {
                get
                {
                    return _scale.GetDateTime();
                }
            }
            public string ScalePoint
            {
                get
                {
                    return _scalePoint.GetDateTime();
                }
            }

            public PayScaleOnContractDialog EditOnContract()
            {
                _editButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                return new PayScaleOnContractDialog();
            }
        }

        #endregion

        #region Tables

        public GridComponent<AllowancesRow> AllowancesTable
        {
            get
            {
                GridComponent<AllowancesRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AllowancesRow>(By.CssSelector("[data-maintenance-container='EmploymentContractAllowances']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class AllowancesRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='AllowanceDescription']")]
            private IWebElement _allowance;


            public string Allowance
            {
                set { _allowance.SetText(value); }
                get { return _allowance.GetAttribute("value"); }
            }

        }

        #endregion

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

        public AddStaffContractAllowanceDialog ClickAddAllowance()
        {
            _addAllowancesButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddStaffContractAllowanceDialog();
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
