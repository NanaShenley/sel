using POM.Base;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Common;
using System;

namespace Facilities.POM.Components.ManageHouse
{
    public class ManageHouseDetailPage : BaseComponent
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

        [FindsBy(How = How.Name, Using = "FullName")]
        private IWebElement _houseFullName;

        [FindsBy(How = How.Name, Using = "ShortName")]
        private IWebElement _houseShortName;

        [FindsBy(How = How.Name, Using = "HouseColor.dropdownImitator")]
        private IWebElement _houseColourDropDown;
        

        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");

        #endregion

        #region Action

        public string HouseFullName
        {
            set { _houseFullName.SetText(value); }
            get { return _houseFullName.GetValue(); }
        }

        public string HouseShortName
        {
            set { _houseShortName.SetText(value); }
            get { return _houseShortName.GetValue(); }
        }

        public string HouseColour
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _houseColourDropDown.EnterForDropDown(value);
                }
                else
                {
                    IWebElement clearDropdown = SeleniumHelper.FindElement(SimsBy.CssSelector(".select2-search-choice-close"));
                    clearDropdown.Click();
                }

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

        public DeleteConfirmationDialog Delete()
        {
            _deleteButton.ClickByJS();
            return new DeleteConfirmationDialog();
        }

        #endregion
    }
}
