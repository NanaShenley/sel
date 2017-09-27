using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;
using System;
using System.Linq;
using SeSugar.Automation;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;

namespace POM.Components.Pupil
{
    public class PupilPremiumRecordPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("pupil_premium_detail"); }
        }

        public GridComponent<PupilPremiumGrants> PupilPremiumGrantsTable
        {
            get
            {
                GridComponent<PupilPremiumGrants> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PupilPremiumGrants>(By.CssSelector("[data-maintenance-container$='LearnerPupilPremiumGrants']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class PupilPremiumGrants
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            public string StartDate
            {
                set
                {
                    _startDate.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _startDate.GetDateTime(); }
            }

            public string EndDate
            {
                get { return _endDate.GetDateTime(); }
                set
                {
                    _endDate.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
            }
        }
    }
}