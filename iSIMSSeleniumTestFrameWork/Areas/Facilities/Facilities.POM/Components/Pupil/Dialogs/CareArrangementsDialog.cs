using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class CareArrangementsDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='PersonalEducationPlans']")]
        private IWebElement _PersonalEducationPlans;

        public GridComponent<PersonalEducationPlansRow> PersonalEducationPlans
        {
            get
            {
                GridComponent<PersonalEducationPlansRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PersonalEducationPlansRow>(By.CssSelector("[data-maintenance-container='PersonalEducationPlans']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class PersonalEducationPlansRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateField;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateField;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _columnEnd;

            public string StartDate
            {
                set
                {
                    _startDateField.SetDateTime(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    Retry.Do(_columnEnd.Click);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _startDateField.GetDateTime();
                }
            }

            public string EndDate
            {
                set
                {
                    _endDateField.SetDateTime(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    Retry.Do(_columnEnd.Click);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _endDateField.GetDateTime();
                }
            }
        }

        #endregion
    }
}
