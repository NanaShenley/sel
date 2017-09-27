using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff
{
    public class StaffCheckClearanceLevelPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_detail_provider"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "table[data-maintenance-container='Rows']")]
        private  IWebElement _staffCheckClearanceLevelTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        private IWebElement _errorMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_StaffCheckClearanceLevel']")]
        private IWebElement _addStaffCheckClearanceLevelButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        public GridComponent<ClearanceLevel> StaffClearanceLevelTable
        {
            get
            {
                GridComponent<ClearanceLevel> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ClearanceLevel>(By.CssSelector("[data-maintenance-container='Rows']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ClearanceLevel : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='.Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Description']")]
            private IWebElement _description;

            [FindsBy(How = How.CssSelector, Using = "[name$='.DisplayOrder']")]
            private IWebElement _displayOrder;

            public string Code
            {
                set
                {
                    _code.SetText(value);
                    Wait.WaitLoading();
                }
                get
                {
                    return _code.GetValue();
                }
            }

            public string Description
            {
                set
                {
                    _description.SetText(value);
                }
                get
                {
                    return _description.GetValue();
                }
            }

            public string DisplayOrder
            {
                set
                {
                    _displayOrder.SetText(value);
                }
                get
                {
                    return _displayOrder.GetValue();
                }
            }
        }

        #endregion

        #region Page action

        public bool IsMessageSuccessDisplay()
        {
            return _successMessage.IsElementDisplayed();
        }

        public bool IsMessageErrorDisplay()
        {
            bool messageExist = SeleniumHelper.FindElements(SimsBy.CssSelector("li")).Any(x => x.GetText()
                .Contains("Staff Checks attached"));
            return messageExist;
        }

        public StaffCheckClearanceLevelPage CreateNewStaffCheckClearance()
        {
            _addStaffCheckClearanceLevelButton.Click();
            Wait.WaitForElementEnabled(By.CssSelector("[data-automation-id='create_service_StaffCheckClearanceLevel']"));
            Wait.WaitLoading();
            return new StaffCheckClearanceLevelPage();
        }

        public StaffCheckClearanceLevelPage SaveData()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
            return new StaffCheckClearanceLevelPage();
        }

        #endregion
    }
}
