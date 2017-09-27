using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class NationalInsuranceRatesPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("national_insurance_rate_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "StartDate")]
        private  IWebElement _startDateTextBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private  IWebElement _endDateTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='BandCollections']")]
        private IWebElement _bandCollectionsTable;

        public string StartDate
        {
            set { _startDateTextBox.SetText(value); }
            get { return _startDateTextBox.GetValue(); }
        }

        public string EndDate
        {
            set { _endDateTextBox.SetText(value); }
            get { return _endDateTextBox.GetValue(); }
        }

    
        public GridComponent<BandCollectionsRow> BandCollections
        {
            get
            {
                GridComponent<BandCollectionsRow>  returnValue = new GridComponent<BandCollectionsRow>(By.CssSelector("[data-maintenance-container='BandCollections']"), ComponentIdentifier);
               
                return returnValue;
            }
        }

        public class BandCollectionsRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='BandNumber']")]
            private IWebElement _BandNumber;

            [FindsBy(How = How.CssSelector, Using = "[name$='UpperMonthlyEarnings']")]
            private IWebElement _UpperMonthlyEarnings;

            [FindsBy(How = How.CssSelector, Using = "[name$='LowerMonthlyEarnings']")]
            private IWebElement _LowerMonthlyEarnings;

            [FindsBy(How = How.CssSelector, Using = "[name$='RateOne']")]
            private IWebElement _RateOne;

            [FindsBy(How = How.CssSelector, Using = "[name$='RateTwo']")]
            private IWebElement _RateTwo;

            public string BandNumber
            {
                set { _BandNumber.SetText(value); }
                get { return _BandNumber.GetAttribute("Value"); }
            }
            public string UpperMonthlyEarnings
            {
                set 
                { 
                    _UpperMonthlyEarnings.SetText(value);
                    AutomationSugar.WaitForAjaxCompletion();
                }
                get { return _UpperMonthlyEarnings.GetAttribute("Value"); }
            }
            public string LowerMonthlyEarnings
            {
                set { _LowerMonthlyEarnings.SetText(value); }
                get { return _LowerMonthlyEarnings.GetAttribute("Value"); } 
            }
            public string RateOne
            {
                set 
                { 
                    _RateOne.SetText(value);
                    AutomationSugar.WaitForAjaxCompletion();
                }
                get { return _RateOne.GetAttribute("Value"); }
            }
            public string RateTwo
            {
                set 
                { 
                    _RateTwo.SetText(value);
                    AutomationSugar.WaitForAjaxCompletion();
                }
                get { return _RateTwo.GetAttribute("Value"); }
            }
        }

        #endregion

        #region Public methods

        public void ClickAddBand()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_band_jobstep_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_band_jobstep_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        #endregion
    }
}

