using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using System.Threading;

namespace Staff.Components.StaffRegression
{
    public class PaySpinesDialog : BaseDialogComponent
    {
        public class PaySpinesDetail : BaseComponent
        {
            public override By ComponentIdentifier
            {
                get { return SimsBy.AutomationId("pay_spines_detail"); }
            }

            #region Page properties
            [FindsBy(How = How.Name, Using = "Code")]
            private IWebElement _code;

            [FindsBy(How = How.Name, Using = "MinimumPoint")]
            private IWebElement _minimumPoint;

            [FindsBy(How = How.Name, Using = "MaximumPoint")]
            private IWebElement _maximumPoint;

            [FindsBy(How = How.Name, Using = "Interval")]
            private IWebElement _interval;

            [FindsBy(How = How.Name, Using = "AwardDate")]
            private IWebElement _awardDate;

            public string Code
            {
                set { _code.SetText(value); }
                get { return _code.GetValue(); }
            }
            public string MinimumPoint
            {
                set { _minimumPoint.SetText(value); }
                get { return _minimumPoint.GetValue(); }
            }
            public string MaximumPoint
            {
                set { _maximumPoint.SetText(value); }
                get { return _maximumPoint.GetValue(); }
            }
            public string Interval
            {
                set { _interval.SetText(value); }
                get { return _interval.GetValue(); }
            }
            public string AwardDate
            {
                set { _awardDate.SetText(value); }
                get { return _awardDate.GetValue(); }
            }
            #endregion

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_jobstep_button']")]
            private IWebElement _addScaleAwardsButton;

            public class ScaleAward
            {
                [FindsBy(How = How.CssSelector, Using = "[name$=Date]")]
                private IWebElement _awardDate;

                [FindsBy(How = How.CssSelector, Using = "[name$=ScalePoint]")]
                private IWebElement _scalePoint;

                [FindsBy(How = How.CssSelector, Using = "[name$=ScaleAmount]")]
                private IWebElement _scaleAmount;

                public string AwardDate
                {
                    set { _awardDate.SetText(value); }
                    get { return _awardDate.GetValue(); }
                }
                public string ScalePoint
                {
                    set { _scalePoint.SetText(value); }
                    get { return _scalePoint.GetValue(); }
                }
                public string ScaleAmount
                {
                    set { _scaleAmount.SetText(value); }
                    get { return _scaleAmount.GetValue(); }
                }
            }

            public GridComponent<ScaleAward> ScaleAwards
            {
                get
                {
                    GridComponent<ScaleAward> returnValue = null;
                    Retry.Do(() =>
                    {
                        returnValue = new GridComponent<ScaleAward>(By.CssSelector("[data-maintenance-container='PayAwards']"));
                    });
                    return returnValue;
                }
            }

            public void AddScaleAwards()
            {
               Retry.Do(_addScaleAwardsButton.Click);
            }
        }

        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_pay_scales_pay_spine_dialog"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id=\"save_button\"]")]
        private IWebElement _saveButton;

        public void ClickSave()
        {
            Thread.Sleep(250);
            _saveButton.Click();
            Thread.Sleep(250);
        }


        public PaySpinesDetail Create()
        {
            SeleniumHelper.ClickAndWaitFor(SimsBy.AutomationId("pay_spines_create_button"), By.CssSelector(".has-datamaintenance"));
            Thread.Sleep(500);
            return new PaySpinesDetail();
        }
    }

}
