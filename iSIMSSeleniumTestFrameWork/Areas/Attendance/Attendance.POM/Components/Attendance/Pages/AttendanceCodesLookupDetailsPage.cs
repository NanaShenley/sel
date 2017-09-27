using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using SeSugar.Automation;
using POM.Helper;

namespace POM.Components.Attendance
{
    public class AttendanceCodesLookupDetailsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SeSugar.Automation.SimsBy.AutomationId("lookup_detail_basic"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _statusSuccess;

        public GridComponent<AttendanceCodesRow> AttendanceCodes
        {
            get
            {
                return new GridComponent<AttendanceCodesRow>(By.CssSelector("[data-maintenance-grid-id='LookupsGrid1']"), ComponentIdentifier);
            }
        }

        public class AttendanceCodesRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _Description;

            [FindsBy(How = How.CssSelector, Using = "[name$='StatisticalCategory']")]
            private IWebElement _StatisticalMeaning;

            [FindsBy(How = How.CssSelector, Using = "[name$='PhysicalCategory']")]
            private IWebElement _PhysicalMeaning;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsVisible']")]
            private IWebElement _AvailablefromTakeRegister;

            public string Code
            {
                //set { _code.SetText(value); }
                get { return _code.GetValue(); }
            }

            public string Description
            {
                //set { _Description.SetText(value); }
                get { return _Description.GetValue(); }
            }

            public string StatisticalMeaning
            {
                //set { _StatisticalMeaning.SetText(value); }
                get { return _StatisticalMeaning.GetValue(); }
            }

            public string PhysicalMeaning
            {
                //set { _PhysicalMeaning.SetText(value); }
                get { return _PhysicalMeaning.GetValue(); }
            }

            public bool AvailablefromTakeRegister
            {
                set { _AvailablefromTakeRegister.Set(value); }
                get { return _AvailablefromTakeRegister.IsChecked(); }
            }


        }

        #endregion
    }
}
