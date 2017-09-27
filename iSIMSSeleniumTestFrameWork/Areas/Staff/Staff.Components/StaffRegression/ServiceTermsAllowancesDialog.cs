using System.Threading;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedComponents.Helpers;
using OpenQA.Selenium.Support.PageObjects;

namespace Staff.Components.StaffRegression
{
    public class ServiceTermsAllowancesDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_allowances_dialog"); }
        }

        public class ServiceTermsAllowancesDialogDetail : BaseComponent
        {
            public override By ComponentIdentifier
            {
                get { return SimsBy.AutomationId("service_terms_allowances_dialog_detail"); }
            }

            #region Page Properties
            [FindsBy(How = How.Name, Using = "Code")]
            private IWebElement _code;

            [FindsBy(How = How.Name, Using = "Description")]
            private IWebElement _description;

            [FindsBy(How = How.Name, Using = "DisplayOrder")]
            private IWebElement _displayOrder;

            [FindsBy(How = How.Name, Using = "AdditionalPaymentCategory.dropdownImitator")]
            private IWebElement _category;

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

            public string DisplayOrder
            {
                set { _displayOrder.SetText(value); }
                get { return _displayOrder.GetValue(); }
            }

            public string Category
            {
                set { _category.ChooseSelectorOption(value); }
                get { return _category.GetValue(); }
            }
            #endregion

            public class Award
            {
                [FindsBy(How = How.CssSelector, Using = "[name$=AwardDate]")]
                private IWebElement _awardDate;

                [FindsBy(How = How.CssSelector, Using = "[name$=Amount]")]
                private IWebElement _amount;

                public string AwardDate
                {
                    set { _awardDate.SetText(value); }
                    get { return _awardDate.GetValue(); }
                }

                public string Amount
                {
                    set { _amount.SetText(value); }
                    get { return _amount.GetValue(); }
                }
            }

            public GridComponent<Award> Awards
            {
                get
                {
                    GridComponent<Award> returnValue = null;
                    Retry.Do(() =>
                    {
                        returnValue = new GridComponent<Award>(By.CssSelector("[data-maintenance-container='AllowanceAwards']"));
                    });
                    return returnValue;
                }
            }
        }

        public ServiceTermsAllowancesDialogDetail Create()
        {
            SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("service_terms_allowances_dialog_create_button"), By.CssSelector("[data-automation-id='service_terms_allowances_dialog_detail'].has-datamaintenance"));
            Thread.Sleep(500);
            return new ServiceTermsAllowancesDialogDetail();
        }
    }
}
