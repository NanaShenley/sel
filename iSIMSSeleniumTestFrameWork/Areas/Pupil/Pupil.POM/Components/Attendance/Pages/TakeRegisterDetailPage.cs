using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POM.Components.Attendance
{
    public class TakeRegisterDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("TakeRegister"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Extended_Dropdown']")]
        private IWebElement _modeCombobox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Keep']")]
        private IWebElement _preserveMode;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Replace']")]
        private IWebElement _overwriteMode;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]
        private IWebElement _allCodeButton;

        [FindsBy(How = How.CssSelector, Using = "table")]
        private IWebElement _markTimeTable;

        [FindsBy(How = How.CssSelector, Using = ".webix_dtable")]
        private IWebElement _marksTable;

        [FindsBy(How = How.Id, Using = "RegisterSave")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[view_id='$suggest1_list']")]
        private IWebElement _codeItemCombobox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        public MarkGrid Marks
        {
            get { return new MarkGrid(_marksTable); }
        }

        public bool ModePreserve
        {
            set
            {
                _modeCombobox.ClickUntilAppearElement(SimsBy.AutomationId("Keep"));

                if (value)
                {
                    _preserveMode.WaitUntilState(ElementState.Displayed);

                    Retry.Do(_preserveMode.Click);

                }
                else
                {
                    _overwriteMode.WaitUntilState(ElementState.Displayed);

                    Retry.Do(_overwriteMode.Click);

                }
            }
        }

        public WarningConfirmDialog SaveValue()
        {

            _saveButton.ClickByJS();
            return new WarningConfirmDialog();
        }


        public void ClickSave(bool isDialogDisplayed = false)
        {
            _saveButton.ClickByJS();
            if (isDialogDisplayed)
            {
                try
                {
                    IWebElement okButton = SeleniumHelper.FindElement(SimsBy.AutomationId("yes_button"));
                    if (okButton.IsExist())
                    {
                        okButton.Click();
                    }
                }
                catch (Exception) { }

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

        public bool IsSuccessMessageDisplayed()
        {
            return _successMessage.IsExist();
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

        public string CodeUI
        {
            set
            {
                SeleniumHelper.Get(SimsBy.CssSelector(string.Format("button[data-register-mark-id='{0}']", value))).ClickByJS();
            }
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

        #region Marks Grid
        public class MarkGrid : WebixComponent<WebixCell>
        {
            public MarkGrid(IWebElement _webElement)
                : base(_webElement) { }

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
                    j = j + 2;
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
