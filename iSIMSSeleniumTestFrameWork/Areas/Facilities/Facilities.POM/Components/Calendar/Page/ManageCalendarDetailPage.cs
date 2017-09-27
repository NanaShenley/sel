using POM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Common;
using Facilities.POM.Components.SchoolGroups.Dialog;

namespace Facilities.POM.Components.Calendar.Page
{
    public class ManageCalendarDetailPage : BaseComponent
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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        private IWebElement _validationError;

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _calendarName;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _calendarDescription;

        [FindsBy(How = How.Name, Using = "IsActive")]
        private IWebElement _calendarActiveCheckBox;

        [FindsBy(How = How.Name, Using = "CalendarGroup.dropdownImitator")]
        private IWebElement _calendarGroupDropdown;

        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");

        #endregion CalendarGroup.dropdownImitator

        #region Action

        public string CalendarName
        {
            set { _calendarName.SetText(value); }
            get { return _calendarName.GetValue(); }
        }

        public string CalendarDescription
        {
            set { _calendarDescription.SetText(value); }
            get { return _calendarDescription.GetValue(); }
        }
        public string CalendarGroup
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _calendarGroupDropdown.EnterForDropDown(value);
                }
                else
                {
                    IWebElement clearDropdown = SeleniumHelper.FindElement(SimsBy.CssSelector(".select2-search-choice-close"));
                    clearDropdown.Click();
                }

            }
        }

        public bool IsActive
        {
            get { return _calendarActiveCheckBox.IsChecked(); }
            set { _calendarActiveCheckBox.Set(value); }
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

        public DeleteConfirmationDialog Delete()
        {
            _deleteButton.ClickByJS();
            return new DeleteConfirmationDialog();
        }

        #endregion
    }
}
