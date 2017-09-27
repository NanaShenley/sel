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
    public class PayScaleOnContractDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_employment_contract_payscale_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "PayScale.dropdownImitator")]
        private IWebElement _payScaleDropDown;
        
        [FindsBy(How = How.Name, Using = "MinimumPoint")]
        private IWebElement _minimumPointTextBox;
        
        [FindsBy(How = How.Name, Using = "MaximumPoint")]
        private IWebElement _maximumPointTextBox;

        [FindsBy(How = How.Name, Using = "Point")]
        private IWebElement _pointTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        public string StatutoryPayScale 
        {
            set { _payScaleDropDown.EnterForDropDown(value); }
            get { return _payScaleDropDown.GetSelectedComboboxItemText(); }
        }

        public string MinimumPoint
        {
            set { _minimumPointTextBox.SetText(value); }
            get { return _minimumPointTextBox.GetValue(); }
        }

        public string MaximumPoint
        {
            set { _maximumPointTextBox.SetText(value); }
            get { return _maximumPointTextBox.GetValue(); }
        }

        public string Point
        {
            set { SeleniumHelper.SetTextInUpdatePanel(value, new ByChained(this.ComponentIdentifier, By.Name("Point"))); }
            get { return _pointTextBox.GetValue(); }
        }

        #endregion

        #region Actions

        public EditContractDialog ClickOk()
        {
            base.ClickOk();
            return new EditContractDialog();
        }

        #endregion
    }
}
