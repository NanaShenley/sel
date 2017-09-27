using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff
{
    public class BankBuildingSocialDetailDialog : BaseDialogComponent
    {        
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_financial_account_dialog"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "BankAccountName")]
        private  IWebElement _bankAccountName;

        [FindsBy(How = How.Name, Using = "BankName")]
        private IWebElement _bankBuilindSocialName;

        [FindsBy(How = How.Name, Using = "BankSortCode")]
        private IWebElement _bankSortCode;

        [FindsBy(How = How.Name, Using = "BankAccountNumber")]
        private IWebElement _bankAccountNumber;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okbutton;

        public string BankAccountNameField
        {
            get { return _bankAccountName.GetText(); }
            set { _bankAccountName.SetText(value); }
        }

        public string BankBuilindSocialNameField
        {
            get { return _bankBuilindSocialName.GetText(); }
            set { _bankBuilindSocialName.SetText(value); }
        }

        public string BankSortCodeField
        {
            get { return _bankSortCode.GetText(); }
            set { _bankSortCode.SetText(value); }
        }

        public string BankAccountNumberField
        {
            get { return _bankAccountNumber.GetText(); }
            set { _bankAccountNumber.SetText(value); }
        }
        
        #endregion

        #region Methods

        public StaffRecordPage AddBankAccountDetails()
        {
            _okbutton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new StaffRecordPage();
        }

        #endregion

    }
}
