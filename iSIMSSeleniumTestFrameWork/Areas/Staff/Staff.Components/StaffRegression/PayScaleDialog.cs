using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;

namespace Staff.Components.StaffRegression
{
    public class PayScaleDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_pay_scale_dialog"); }
        }

        #region Page properties
        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _code;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _description;
        
        [FindsBy(How = How.Name, Using = "StatutoryPayScale.dropdownImitator")]
        private IWebElement _statutoryPayScale;

        [FindsBy(How = How.Name, Using = "MinimumPoint")]
        private IWebElement _minimumPoint;

        [FindsBy(How = How.Name, Using = "MaximumPoint")]
        private IWebElement _maximumPoint;

        [FindsBy(How = How.Name, Using = "tri_chkbox_IsVisible")]
        private IWebElement _isVisible;

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
        public string StatutoryPayScale
        {
            set { _statutoryPayScale.ChooseSelectorOption(value); }
            get { return _statutoryPayScale.GetValue(); }
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
        public bool IsVisible
        {
            set {  _isVisible.SetCheckBox(value); }
            get { return _isVisible.IsCheckboxChecked(); }
        }

        #endregion

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='pay_spine_button']")]
        private IWebElement _paySpineButton;
        
        public PaySpinesDialog SelectPaySpine()
        {
            _paySpineButton.Click();
            return new PaySpinesDialog();
        }
    }
}
