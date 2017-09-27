using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using Staff.POM.Base;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace Staff.POM.Components.Staff.Dialogs
{
    public class SelectSalaryRangeDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("#dialog-dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "SalaryRange.dropdownImitator")]
        private IWebElement _salaryRange;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _addPayScaleButton;

        [FindsBy(How = How.Name, Using = "AnnualSalary")]
        private IWebElement _annualSalary;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDate;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDate;

        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _notes;

        [FindsBy(How = How.CssSelector, Using = "div[class='employmentcontractsalaryrangehistory'] button")]
        private IWebElement _viewSalaryRangeHistoryButton;

        public string SalaryRange
        {
            get { return _salaryRange.GetValue(); }
            set { 
                _salaryRange.EnterForDropDown(value);
                
                WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, SeSugar.Environment.Settings.ElementRetrievalTimeout);
                wait.Until(ExpectedConditions.StalenessOf(WebContext.WebDriver.FindElement(By.Name("MinimumAmount"))));
                wait.Until(ExpectedConditions.ElementExists(By.Name("MinimumAmount")));
            }
        }

        public string AnnualSalary
        {
            get { return _annualSalary.GetValue(); }
            set { _annualSalary.SetText(value); }
        }

        public string StartDate
        {
            set
            {
                _startDate.SetText(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            get { return _startDate.GetValue(); }
        }

        public string EndDate
        {
            set
            {
                _endDate.SetText(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            get { return _endDate.GetValue(); }
        }

        public string Notes
        {
            set { _notes.SetText(value); }
            get { return _notes.GetValue(); }
        }

        #endregion

        public void EditWarningDialog()
        {
            AutomationSugar.ClickOnAndWaitForUntilStale(new ByChained(By.CssSelector("[data-section-id='generic-confirm-dialog']"), SimsBy.AutomationId("ok_button")), By.CssSelector("[data-section-id='generic-confirm-dialog']"));
        }

        public SalaryRangeHistoryPopup ViewSalaryRangeHistory()
        {
            _viewSalaryRangeHistoryButton.ClickByJS();
            AutomationSugar.WaitForAjaxCompletion();

            return new SalaryRangeHistoryPopup();
        }
    }
}
