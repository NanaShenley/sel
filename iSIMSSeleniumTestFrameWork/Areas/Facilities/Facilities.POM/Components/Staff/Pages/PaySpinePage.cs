using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Helper;
using Staff.POM.Base;

namespace Staff.POM.Components.Staff
{
    public class PaySpinePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("payspine_record_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _statusSuccess;
        
        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _codeTextBox;

        [FindsBy(How = How.Name, Using = "Interval")]
        private IWebElement _intervalTextBox;

        [FindsBy(How = How.Name, Using = "MaximumPoint")]
        private IWebElement _maximumPointTextBox;

        [FindsBy(How = How.Name, Using = "MinimumPoint")]
        private IWebElement _minimumPointTextBox;

        [FindsBy(How = How.Name, Using = "AwardDate")]
        private IWebElement _awardDateTextBox;

        [FindsBy(How = How.Name, Using = "GeneratePayAwards")]
        private IWebElement _addPayAdwardsButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_jobstep_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='PayAwards']")]
        private IWebElement _scaleAwardTable;

        public string Code
        {
            set { _codeTextBox.SetText(value); }
            get { return _codeTextBox.GetValue(); }
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

        public string InterVal
        {
            set { _intervalTextBox.SetText(value); }
            get { return _intervalTextBox.GetValue(); }
        }

        public string AwardDate
        {
            set { _awardDateTextBox.SetDateTime(value); }
            get { return _awardDateTextBox.GetDateTime(); }
        }

        
        public GridComponent<POM.Components.Staff.NewAllowancePage.AwardsRow> AwardTable
        {
            get
            {
                GridComponent<POM.Components.Staff.NewAllowancePage.AwardsRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<POM.Components.Staff.NewAllowancePage.AwardsRow>(By.CssSelector("[data-maintenance-container='PayAwards']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        #endregion

        #region  ScaleAward Grid

        public GridComponent<ScaleAward> ScaleAwardTable
        {
            get
            {
                GridComponent<ScaleAward> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ScaleAward>(By.CssSelector("[data-maintenance-container='PayAwards']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ScaleAward
        {
            [FindsBy(How = How.CssSelector, Using = "[name $='ScaleAmount']")]
            private IWebElement _scaleAmount;

            [FindsBy(How = How.CssSelector, Using = "[name$='ScalePoint']")]
            private IWebElement _scalePoint;

            [FindsBy(How = How.CssSelector, Using = "[name$='Date']")]
            private IWebElement _awardDate;

            public string ScaleAmount
            {
                set { _scaleAmount.SetText(value); }
                get { return _scaleAmount.GetValue(); }
            }
            public string ScalePoint
            {
                set { _scalePoint.SetText(value); }
                get { return _scalePoint.GetValue(); }
            }
            public string AwardDate
            {
                set { _awardDate.SetDateTime(value); }
                get { return _awardDate.GetDateTime(); }
            }
        }

        #endregion

        #region Actions

        public void ClickAddPayAdwards()
        {
            _addPayAdwardsButton.ClickByAction();
            _addPayAdwardsButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask-loading-text"));
            Wait.WaitLoading();
        }
        public DeleteConfirmationPage ClickDelete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new DeleteConfirmationPage();
        }

        public void ClickSave()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool IsSuccessMessageIsDisplayed()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_success']"));
            return SeleniumHelper.DoesWebElementExist(_statusSuccess);
        }

        #endregion

    }
}
