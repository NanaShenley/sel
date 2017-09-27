using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System.Linq;

namespace POM.Components.Staff
{
    public class StaffCheckTypePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_detail_provider"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_StaffCheckType']")]
        private IWebElement _addNewButton;


        public GridComponent<StaffCheckTypeRow> StaffCheckTypeTable
        {
            get
            {
                GridComponent<StaffCheckTypeRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffCheckTypeRow>(By.CssSelector("[data-maintenance-container='Rows']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class StaffCheckTypeRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='.Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Description']")]
            private IWebElement _description;

            [FindsBy(How = How.CssSelector, Using = "[name$='.DisplayOrder']")]
            private IWebElement _displayOrder;

            #region Properties


            public string Code
            {
                set
                {
                    _code.SetText(value);
                    _code.SetText(value);
                }
                get { return _code.GetValue(); }
            }

            public string Description
            {
                set { _description.SetText(value); }
                get { return _description.GetValue(); }
            }

            public string DisplayOrder
            {
                set { _displayOrder.SetText(value); }
                get { return _displayOrder.GetValue(); }
            }


            #endregion
        }

        #endregion

        #region Page actions

        public StaffCheckTypePage SaveRecord()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new StaffCheckTypePage();
        }

        public StaffCheckTypePage AddNewRecord()
        {
            _addNewButton.Click();
            Wait.WaitForElementEnabled(SimsBy.CssSelector("[data-automation-id='create_service_StaffCheckType']"));
            Wait.WaitLoading();
            return new StaffCheckTypePage();
        }

        public bool IsMessageSuccessDisplay()
        {
            return SeleniumHelper.Get(SimsBy.AutomationId("status_success")).IsElementDisplayed();
        }

        public bool IsMessageErrorDisplay()
        {
            bool messageExist = SeleniumHelper.FindElements(SimsBy.CssSelector("li")).Any(x => x.GetText().Contains("Staff Checks attached"))
                || SeleniumHelper.FindElements(SimsBy.CssSelector("li")).Any(x => x.GetText().Contains("be unique"));
            return messageExist;
        }

        #endregion
    }
}
