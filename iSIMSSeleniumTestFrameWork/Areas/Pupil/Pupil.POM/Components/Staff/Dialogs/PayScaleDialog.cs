using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class PayScaleDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_pay_scale_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _CodeTextBox;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _descriptionTextBox;

        [FindsBy(How = How.Name, Using = "StatutoryPayScale.dropdownImitator")]
        private IWebElement _statutoryPayScaleDropDownList;

        [FindsBy(How = How.Name, Using = "MinimumPoint")]
        private IWebElement _minimumPointTextBox;

        [FindsBy(How = How.Name, Using = "MaximumPoint")]
        private IWebElement _maximumPointTextBox;

        [FindsBy(How = How.Name, Using = "Point")]
        private IWebElement _pointTextBox;

        [FindsBy(How = How.Name, Using = "Interval")]
        private IWebElement _intervalTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='pay_spine_button']")]
        private IWebElement _paySpineButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        [FindsBy(How = How.Name, Using = "PaySpineCode")]
        private IWebElement _paySpineTextBox;

        public string Code
        {
            set { _CodeTextBox.SetText(value); }
            get { return _CodeTextBox.GetValue(); }
        }

        public string Description
        {
            set { _descriptionTextBox.SetText(value); }
            get { return _descriptionTextBox.GetValue(); }
        }

        public string StatutoryPayScale
        {
            set { _statutoryPayScaleDropDownList.EnterForDropDown(value); }
            get { return _statutoryPayScaleDropDownList.GetSelectedComboboxItemText(); }
        }

        public string MinimumPoint
        {
            set { _minimumPointTextBox.SetText(value); }
            get { return _minimumPointTextBox.GetValue(); }
        }

        public string MaximumPoint
        {
            set
            {
                Wait.WaitLoading();
                _maximumPointTextBox.SetText(value);
            }
            get { return _maximumPointTextBox.GetValue(); }
        }

        public string InterVal
        {
            set { _intervalTextBox.SetText(value); }
            get { return _intervalTextBox.GetValue(); }
        }

        public string Point
        {
            set { _pointTextBox.SetText(value); }
            get { return _pointTextBox.GetValue(); }
        }

        public string PaySpineCode
        {
            get { return _paySpineTextBox.GetValue(); }
        }

        #endregion

        #region Page actions

        public PaySpineDialogTriplet ClickPaySpine()
        {
            _paySpineButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new PaySpineDialogTriplet();
        }

        public void OK()
        {
            Wait.WaitUntil((d) => { return !PaySpineCode.Equals(""); });
            _okButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion
    }
}
