using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;
using System.Linq;
using POM.Components.Attendance;


namespace POM.Components.Attendance
{
    public class EditMarksPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector(".main.pane"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Extended_Dropdown']")]
        private IWebElement _modeCombobox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Keep']")]
        private IWebElement _preserveMode;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Replace']")]
        private IWebElement _overwriteMode;

        [FindsBy(How = How.CssSelector, Using = "table")]
        private IWebElement _markTimeTable;

        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _marksTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_button']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]
        private IWebElement _allCodeButton;

        public WebixComponent<WebixCell> Marks
        {
            get { return new WebixComponent<WebixCell>(_marksTable); }
        }

        public bool ModePreserve
        {
            set
            {
                _modeCombobox.Click();

                if (value)
                {
                    _preserveMode.WaitUntilState(ElementState.Displayed);
                    _preserveMode.Click();
                }
                else
                {
                    _overwriteMode.WaitUntilState(ElementState.Displayed);
                    _overwriteMode.Click();
                }
            }
        }

        public void Save(bool IsDialogDisplayed = false)
        {

            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            if (IsDialogDisplayed)
            {
                try
                {
                    POM.Components.Attendance.WarningConfirmDialog confirmDialog = new WarningConfirmDialog();
                    confirmDialog.ClickOk();
                }
                catch (Exception) { }
            }
            Refresh();
        }

        public void SaveConfirmDelete(bool IsDialogDisplayed = false)
        {

            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            if (IsDialogDisplayed)
            {
                try
                {
                    POM.Components.Attendance.WarningConfirmDialog confirmDialog = new WarningConfirmDialog();
                    confirmDialog.ClickContinueDelete();
                }
                catch (Exception) { }
            }
            Refresh();
        }

        public void ClickSave(bool isDialogDisplayed = false)
        {
            _saveButton.ClickByJS();
            if (isDialogDisplayed)
            {
              
                try
                {
                    IWebElement deleteButton = SeleniumHelper.FindElement(SimsBy.AutomationId("continue_with_delete_button"));
                    if (deleteButton.IsExist())
                    {
                        deleteButton.Click();
                    }

                }
                catch (Exception) { }
            }
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public void waitForWarningDialogDisplay()
        {
            Wait.WaitForAjaxReady(By.CssSelector("[data-section-id='custom-confirm-dialog']"));
        }

        public string CodeList
        {
            set
            {
                _allCodeButton.ClickByJS();
                Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='Button_Dropdown'][aria-expanded='true']"));
                IWebElement code = SeleniumHelper.Get(SimsBy.CssSelector(string.Format(".menu-item-tile [data-register-mark-id='{0}']", value)));
                code.ScrollToByAction();
                code.ClickByJS();
            }
        }

        #endregion
    }
}
