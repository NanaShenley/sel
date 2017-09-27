using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SharedComponents.Helpers;
using OpenQA.Selenium.Support.PageObjects;

namespace Staff.Components.StaffRegression
{
    public class ServiceTerm : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_detail"); }
        }

        #region Page properties
        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _code;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _description;

        [FindsBy(How = How.Name, Using = "IncrementMonthSelector.dropdownImitator")]
        private IWebElement _incrementMonth;

        [FindsBy(How = How.Name, Using = "HoursWorkedPerWeek")]
        private IWebElement _hoursWorkedPerWeek;

        public string Code
        {
            set { _code.SetText(value); }
            get { return _code.GetValue(); }
        }
        public string Description
        {
            set { _description.SetText(value); }
            get { return _description.GetValue(); }
        }
        public string IncrementMonth
        {
            set { _incrementMonth.ChooseSelectorOption(value); }
            get { return _incrementMonth.GetValue(); }
        }
        public string HoursWorkedPerWeek
        {
            set { _hoursWorkedPerWeek.SetText(value); }
            get { return _hoursWorkedPerWeek.GetValue(); }
        }
        #endregion

        #region Grid add actions
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pay_scale_button']")]
        private IWebElement _addPayScaleButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_post_types_button']")]
        private IWebElement _addPostTypesButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_allowances_button']")]
        private IWebElement _addAllowancesButton;

        public PayScaleDialog AddPayScale()
        {
            Retry.Do(_addPayScaleButton.Click);
            return new PayScaleDialog();
        }

        public PostTypesDialog AddPostTypes()
        {
            Retry.Do(_addPostTypesButton.Click);
            return new PostTypesDialog();
        }

        public ServiceTermsAllowancesDialog AddAllowance()
        {
            Retry.Do(_addAllowancesButton.Click);
            return new ServiceTermsAllowancesDialog();
        }
        #endregion

        public static ServiceTerm Create()
        {
            SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("service_terms_create_button"), By.CssSelector(".has-datamaintenance"));
            return new ServiceTerm();
        }
        
        public ServiceTerm Save()
        {
            Action save = () => SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("well_know_action_save"), By.CssSelector("div.alert"));
            Retry.Do(save);
            return new ServiceTerm();
        }
    }
}
