using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;
using System.Collections.Generic;
using System.Linq;

namespace POM.Components.Attendance
{
    public class DealWithChosenPage : BaseComponent
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

        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _marksTable;

        [FindsBy(How = How.Id, Using = "RegisterSave")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "#editableData >.inline.alert.alert-warning")]
        private IWebElement _alertWarning;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]
        private IWebElement _codesDropdown;

        [FindsBy(How = How.CssSelector, Using = ".dropdown-menu.toolbar-menu")]
        private IList<IWebElement> _toolBarDropdown;

        public WebixComponent<MarkCell> Marks
        {
            get { return new WebixComponent<MarkCell>(_marksTable); }
        }

        public class MarkCell : WebixCell
        {
            public MarkCell() { }

            public MarkCell(IWebElement webElement)
                : base(webElement)
            { }

            public string MarkDropDown
            {
                set
                {
                    if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
                    {
                        webElement.Click();
                    }
                    SeleniumHelper.FindElement(By.Name("AttendanceRecord.SchoolAttendanceCode")).Click();
                    IList<IWebElement> dropdown = SeleniumHelper.FindElements(By.CssSelector(".webix_list_item")).ToList();
                    foreach (var item in dropdown)
                    {
                        if (item.Text.Equals(value))
                        {
                            item.ClickByJS();
                            break;
                        }
                    }
                }
            }

            public string Marks
            {
                get { return webElement.FindElement(By.CssSelector("span")).GetText(); }
                set
                {
                    if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
                    {
                        webElement.Click();
                    }
                    SeleniumHelper.FindElement(By.Name("AttendanceRecord.SchoolAttendanceCode")).SendKeys(value);
                }
            }

            public AddCommentDialog AddComment()
            {
                if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
                {
                    webElement.Click();
                }
                SeleniumHelper.FindElement(By.CssSelector(".webix_dt_editor button")).Click();
                return new AddCommentDialog();
            }
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

        public bool ModeOverwrite
        {
            set
            {
                _modeCombobox.Click();

                if (value)
                {
                    _overwriteMode.WaitUntilState(ElementState.Displayed);
                    _overwriteMode.Click();
                }
                else
                {
                    _preserveMode.WaitUntilState(ElementState.Displayed);
                    _preserveMode.Click();
                }
            }
        }

        public string Code
        {
            set
            {
                string _codeId = _codesDropdown.GetAttribute("id");
                IWebElement _dropDown = _toolBarDropdown.First(t => t.GetAttribute("aria-labelledby").Equals(_codeId));
                if (_codesDropdown.GetAttribute("aria-expanded") == null || _codesDropdown.GetAttribute("aria-expanded").Equals("false"))
                {
                    _codesDropdown.Click();
                    _dropDown.WaitUntilState(ElementState.Displayed);
                }

                IWebElement _code = _dropDown.FindElement(By.CssSelector(String.Format("[data-register-mark-id='{0}']", value)));
                _code.ClickByJS();
            }
        }

        #endregion

        #region Page Actions

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool DoesNoMarkAmendDisplay()
        {
            if (_alertWarning.GetText().Equals("No Marks to Amend"))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
