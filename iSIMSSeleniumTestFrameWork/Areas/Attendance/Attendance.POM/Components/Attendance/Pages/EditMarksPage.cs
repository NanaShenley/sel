using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Collections.Generic;
using WebDriverRunner.webdriver;


namespace POM.Components.Attendance
{
    public class EditMarksPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("EditMarks"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Extended_Dropdown']")]
        private IWebElement _modeCombobox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Keep']")]
        private IWebElement _preserveMode;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Replace']")]
        private IWebElement _overwriteMode;

        [FindsBy(How = How.CssSelector, Using = "[title='Automatically advance down the register']")]
        private IWebElement _orientationModeCombobox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Horizontal']")]
        private IWebElement _horizontalMode;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Vertical']")]
        private IWebElement _verticalMode;

        [FindsBy(How = How.CssSelector, Using = "table")]
        private IWebElement _markTimeTable;

        [FindsBy(How = How.CssSelector, Using = ".webix_dtable")]
        private IWebElement _marksTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_button']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]
        private IWebElement _allCodeButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = ".inline.alert.alert-warning")]
        private IWebElement _validationMessage;

        public MarkGrid Marks
        {
            get { return new MarkGrid(_marksTable); }
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

        public bool ModeHorizontal
        {
            set
            {
                _orientationModeCombobox.Click();

                if (value)
                {
                    _horizontalMode.WaitUntilState(ElementState.Displayed);
                    _horizontalMode.Click();
                }
                else
                {
                    _verticalMode.WaitUntilState(ElementState.Displayed);
                    _verticalMode.Click();
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

        public string CodeItemDropDown
        {
            set
            {
                try
                {
                    IList<IWebElement> lstCodes = SeleniumHelper.Get(SimsBy.CssSelector("[view_id='$suggest1_list']")).FindElements(SimsBy.CssSelector(".webix_list_item"));
                    foreach (var item in lstCodes)
                    {
                        if (item.Text.Equals(value))
                        {

                            item.Click();
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        #endregion

        #region Page Action

        public bool IsSuccessMessageDisplayed()
        {
            return _successMessage.IsExist();
        }

        public bool IsValidationMessageDisplay()
        {
            return _validationMessage.IsExist();
        }

        public IList<IWebElement> GetCodeList()
        {
 
             _allCodeButton.ClickByJS();
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='Button_Dropdown'][aria-expanded='true']"));

            IList<IWebElement> Codes = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='Dropdown_Menu']"));

            return Codes;

        }

        #endregion

        #region Marks Grid
        public class MarkGrid : WebixComponent<WebixCell>
        {
            public MarkGrid(IWebElement _webElement)
                : base(_webElement)
            { }

            public override By ComponentIdentifier
            {
                get { return SimsBy.CssSelector("[data-section-id='searchResults']"); }
            }

            #region Properties

            public IList<Column> Columns
            {
                get { return GetColumns(); }
            }

            #endregion

            #region Actions

            public IList<Column> GetColumns()
            {
                IList<Column> lstColumns = new List<Column>();
                IList<IWebElement> sectionElements = tableElement.FindElements(By.CssSelector(".webix_ss_header .webix_hs_left [section='header']"));
                lstColumns.Add(new Column(sectionElements[1], null, null));

                sectionElements = tableElement.FindElements(By.CssSelector(".webix_ss_header .webix_hs_center [section='header']"));
                IList<IWebElement> lstHeaderElements = sectionElements[0].FindElements(By.CssSelector("td"));
                IList<IWebElement> lstActionElements = sectionElements[1].FindElements(By.CssSelector("td"));
                int j = 0;

                for (int i = 0; i < lstHeaderElements.Count; i++)
                {
                    lstColumns.Add(new Column(lstHeaderElements[i], lstActionElements[j], lstActionElements[j + 1]));
                    j = j + 1;
                }
                return lstColumns;
            }

            #endregion
        }

        public class Column
        {
            IWebElement headerElement;
            IWebElement am;
            IWebElement pm;

            public Column(IWebElement _headerElement, IWebElement _am, IWebElement _pm)
            {
                headerElement = _headerElement;
                am = _am;
                pm = _pm;
            }

            public string HeaderText
            {
                get { return headerElement.GetText(); }
            }

            public string TimeIndicatorSelected
            {
                set
                {
                    if (value.Equals("AM"))
                    {

                        am.Click();
                    }
                    else
                    {

                        pm.Click();
                    }
                    Wait.WaitLoading();
                }
            }
        }

        #endregion
    }
}
