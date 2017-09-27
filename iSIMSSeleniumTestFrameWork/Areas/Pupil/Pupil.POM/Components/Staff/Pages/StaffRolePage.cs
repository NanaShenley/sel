using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class StaffRolePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_role_record_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-grid-id='StaffRoleGrid1']")]
        private IWebElement _staffRoleTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_staff_role_button']")]
        private IWebElement _addStaffRoleButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;


        public GridComponent<StaffRoleRow> StaffRoleTable
        {
            get
            {
                GridComponent<StaffRoleRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffRoleRow>(By.CssSelector("[data-maintenance-grid-id='StaffRoleGrid1']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class StaffRoleRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[id$='StaffRole_dropdownImitator']")]
            private IWebElement _staffRole;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _staffStartDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _staffEndDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Description']")]
            private IWebElement _description;

            [FindsBy(How = How.CssSelector, Using = "[name$='.DisplayOrder']")]
            private IWebElement _displayOrder;

            [FindsBy(How = How.CssSelector, Using = "[name$='Parent.dropdownImitator']")]
            private IWebElement _category;

            public string StaffRole
            {
                set
                {
                    _staffRole.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _staffRole.GetAttribute("value"); }
            }

            public string StaffStartDate
            {
                set
                {
                    _staffStartDate.Click();

                    _staffStartDate.SetDateTimeByJS(value);

                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    Wait.WaitLoading();

                }
                get { return _staffStartDate.GetDateTime(); }
            }

            public string StaffEndDate
            {
                set
                {
                    if (value == null)
                    {
                        _staffEndDate.Click();
                        Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                        Wait.WaitLoading();
                        return;
                    }
                    
                    _staffEndDate.Click();
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

                    _staffEndDate.SetText(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _staffEndDate.GetValue();
                }
            }

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

            public string DisplayOrder
            {
                set { _displayOrder.SetText(value); }
                get { return _displayOrder.GetValue(); }
            }

            public string Category
            {
                set { _category.EnterForDropDown(value); }
                get { return _category.GetValue(); }
            }


            #endregion
        }

        #region Page Actions

        public bool IsMessgeSuccessDisplay()
        {
            return SeleniumHelper.DoesWebElementExist(_successMessage);
        }

        public void ClickDelete(StaffRoleRow row)
        {
            if (row != null)
            {
                row.DeleteRow();
            }
        }

        public AddStaffRoleDialog AddNewStaffRole()
        {
            _addStaffRoleButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddStaffRoleDialog();
        }

        public StaffRolePage SaveData()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new StaffRolePage();
        }

        #endregion

    }
}