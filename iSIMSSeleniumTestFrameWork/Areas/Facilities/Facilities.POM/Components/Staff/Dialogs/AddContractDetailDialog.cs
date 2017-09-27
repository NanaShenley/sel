using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pay_scales_button']")]
        private IWebElement _addPayScaleButton;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateField;


        public string StartDate
        {
            get { return _startDateField.GetText(); }
            set
            {
                _startDateField.SetDateTime(value);
                //_startDateField.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                //_startDateField.SetText(value);
                //Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
        }

        public string ServiceTermCombobox
        {
            get { return _serviceTerm.GetText(); }
            set
            {
                _serviceTerm.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public string EmploymentTypeCombobox
        {
            get { return _employmentType.GetText(); }
            set { _employmentType.EnterForDropDown(value); }
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
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                _hourPerWeek.SetText(value);
            }
        }

        public string WeekPerYear
        {
            get { return _weekPerYear.GetText(); }
            set
            {

                _weekPerYear.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                _weekPerYear.SetText(value);
            }
        }

        public string AnnualLeaveEntitlementDays
        {
            get { return _annualLeaveEntitlementDays.GetText(); }
            set
            {
                Retry.Do(_annualLeaveEntitlementDays.Click);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                _annualLeaveEntitlementDays.SetText(value);
            }
        }

        public string FTE
        {
            get { return _FTE.GetText(); }
            set
            {
                Retry.Do(_FTE.Click);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                _FTE.SetText(value);
            }
        }

        public string ProRata
        {
            get { return _proRata.GetText(); }
            set
            {
                Retry.Do(_proRata.Click);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                _proRata.SetText(value);
            }
        }



        public void ClickFTEUpdate()
        {
            Retry.Do(_FTE.Click);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
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
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new StaffRecordPage();
        }

        public SelectPayScaleDialog OpenSelectPayScaleDialog()
        {
            _addPayScaleButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new SelectPayScaleDialog();
        }

        #endregion


    }
}
