using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;

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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_band_jobstep_button']")]
        private IWebElement _addBandButton;
        
        public string StartDate
        {
            set { _startDateTextBox.SetDateTime(value); }
            get { return _startDateTextBox.GetDateTime(); }
        }

        public string EndDate
        {
            set { _endDateTextBox.SetDateTime(value); }
            get { return _endDateTextBox.GetDateTime(); }
        }

      


        public GridComponent<BandCollectionsRow> BandCollections
        {
            get
            {
                GridComponent<BandCollectionsRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<BandCollectionsRow>(By.CssSelector("[data-maintenance-container='BandCollections']"), ComponentIdentifier);
                });
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
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
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
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _RateOne.GetAttribute("Value"); }
            }
            public string RateTwo
            {
                set
                {
                    _RateTwo.SetText(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _RateTwo.GetAttribute("Value"); }
            }
        }

        #endregion

        #region Public methods

        public void ClickAddBand()
        {
            _addBandButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion
    }
}

