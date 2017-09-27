using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using Staff.POM.Base;
using Staff.POM.Helper;

namespace Staff.POM.Components.Staff.Pages
{
    public class SalaryRangePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("SalaryRange_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
        private IWebElement _code;

        [FindsBy(How = How.CssSelector, Using = "[name$='Description")]
        private IWebElement _description;

        [FindsBy(How = How.CssSelector, Using = "[name$='PayLevel.dropdownImitator")]
        private IWebElement _payLevel;

        [FindsBy(How = How.CssSelector, Using = "[name$='RegionalWeighting.dropdownImitator")]
        private IWebElement _regionalWeighting;

        #endregion 

        public string Code
        {
            get { return _code.GetValue(); }
            set { _code.SetText(value); }
        }


        public string Description
        {
            get { return _description.GetValue(); }
            set { _description.SetText(value); }
        }

        public string PayLevel
        {
            get { return _payLevel.GetAttribute("value"); }
            set { _payLevel.EnterForDropDown(value); }
        }

        public string RegionalWeighting
        {
            get { return _regionalWeighting.GetAttribute("value"); }
            set { _regionalWeighting.EnterForDropDown(value); }
        }

        public GridComponent<SalaryAwardStandardRow> SalaryAwardStandardTable
        {
            get
            {
                return new GridComponent<SalaryAwardStandardRow>(By.CssSelector("[data-maintenance-container='SalaryAwards']"), ComponentIdentifier);
            }
        }

        public class SalaryAwardStandardRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='.AwardDate']")]
            private IWebElement _awardDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='.MinimumAmount']")]
            private IWebElement _minimumAmount;

            [FindsBy(How = How.CssSelector, Using = "[name$='.MaximumAmount']")]
            private IWebElement _maximumAmount;

            public string AwardDate
            {
                get { return _awardDate.GetValue(); }
                set { _awardDate.SetText(value); }
            }

            public string MinimumAmount
            {
                get { return _minimumAmount.GetValue(); }
                set { _minimumAmount.SetText(value); }
            }

            public string MaximumAmount
            {
                get { return _maximumAmount.GetValue(); }
                set { _maximumAmount.SetText(value); }
            }
        }


        public void AddSalaryAwardButtonClick()
        {
            AutomationSugar.WaitFor("add_salary_award_button");
            AutomationSugar.ClickOn("add_salary_award_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void AddSalaryRangeButtonClick()
        {
            AutomationSugar.WaitFor("add_button");
            AutomationSugar.ClickOn("add_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void ClickOk()
        {
            AutomationSugar.WaitFor("ok_button");
            AutomationSugar.ClickOn("ok_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void EditConfirmationDialog()
        {
            AutomationSugar.ClickOnAndWaitForUntilStale(new ByChained(By.CssSelector("[data-section-id='generic-confirm-dialog']"), SimsBy.AutomationId("yes_button")), By.CssSelector("[data-section-id='generic-confirm-dialog']"));
        }
    }
}
