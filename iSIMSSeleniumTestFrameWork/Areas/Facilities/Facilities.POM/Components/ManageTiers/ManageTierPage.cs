using POM.Base;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;

namespace Facilities.POM.Components.ManageTier
{
    public class ManageTierPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("editableData"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;
        
        [FindsBy(How = How.Name, Using = "FullName")]
        private IWebElement _tierFullName;

        [FindsBy(How = How.Name, Using = "ShortName")]
        private IWebElement _tierShortName;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_active_history_button']")]
        private IWebElement _addActiveHistory;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_year_groups_button']")]
        private IWebElement _addYearGroup;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_tier_manager_button']")]
        private IWebElement _addTierManager;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_staff_button']")]
        private IWebElement _addstaff;



        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");

        #endregion

        #region Action

        public string TierFullName
        {
            set { _tierFullName.SetText(value); }
            get { return _tierFullName.GetValue(); }
        }

        public string TierShortName
        {
            set { _tierShortName.SetText(value); }
            get { return _tierShortName.GetValue(); }
        }

        // Define Active History Table
        public GridComponent<ActiveHistory> ActiveHistoryTable
        {
            get
            {
                GridComponent<ActiveHistory> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ActiveHistory>(By.CssSelector("[data-maintenance-container='TierSetMemberships']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }


        public void AddActivehistory()
        {
            _addActiveHistory.ClickByJS();
        }

        public class ActiveHistory : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateTextBox;


            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateTextBox;
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

        }

        // Define Year Group Table
        public GridComponent<YearGroups> YearGroupsTable
        {
            get
            {
                GridComponent<YearGroups> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<YearGroups>(By.CssSelector("[data-maintenance-container='TierYearGroupAssociations']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public void AddYearGroup()
        {
            _addYearGroup.Click();
        }

        public class YearGroups : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[id$='YearGroup_dropdownImitator']")]
            private IWebElement _yearGroupsDropDown;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateTextBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateTextBox;

            public string YearGroup
            {
                set { _yearGroupsDropDown.EnterForDropDown(value); }
                get { return _yearGroupsDropDown.GetValue(); }
            }
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

        }

        // Staff details sections
        public GridComponent<TierManagers> TierManagerTable
        {
            get
            {
                GridComponent<TierManagers> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TierManagers>(By.CssSelector("[data-maintenance-container='TierManagers']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public void AddTierManager()
        {
            _addTierManager.ClickByJS();
        }
        public class TierManagers : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[id$='Staff_dropdownImitator']")]
            private IWebElement _tierManagerDropDown;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateTextBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateTextBox;

            public string SelectTierManager
            {
                set { _tierManagerDropDown.EnterForDropDown(value); }
                get { return _tierManagerDropDown.GetValue(); }
            }

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

        }
        
        public bool IsSuccessMessageDisplayed()
        {
            return _successMessage.IsExist();
        }

        public void Save()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("well_know_action_save")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion
    }
}
