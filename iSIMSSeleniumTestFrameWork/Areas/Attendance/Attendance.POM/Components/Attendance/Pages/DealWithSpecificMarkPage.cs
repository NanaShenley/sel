using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using SeSugar.Automation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POM.Components.Attendance
{
    public class DealWithSpecificMarkPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SeSugar.Automation.SimsBy.AutomationId("DealWithSpecificMark"); }
        }

        #region Page properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='show/hide_filters_button']")]
        private IWebElement _showHideFilter;

        [FindsBy(How = How.Id, Using = "ModeOfTravel")]
        private IWebElement _floodFillCombobox;

        [FindsBy(How = How.CssSelector, Using = ".webix_dtable")]
        private IWebElement _marksTable;

        [FindsBy(How = How.CssSelector, Using = ".inline.alert.alert-warning")]
        private IWebElement _validationMessage;

        public DealWithSpecificMarksGrid DealWithSpecificMarkTable
        {
            get { return new DealWithSpecificMarksGrid(_marksTable); }
        }
        #endregion

        #region Page Actions

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

        public bool IsMessageSuccessAppear()
        {
            return _messageSuccess.IsElementExists();
        }

        public bool IsValidationMessageDisplay()
        {
            return _validationMessage.IsExist();
        }

        #endregion

        #region Deal With Specific Marks Grid
        public class DealWithSpecificMarksGrid : WebixComponent<WebixCell>
        {
            public DealWithSpecificMarksGrid(IWebElement _webElement)
                : base(_webElement) { }

            public override By ComponentIdentifier
            {
                get { return SeSugar.Automation.SimsBy.AutomationId("DealWithSpecificMark"); }
            }

            #region Properties

            [FindsBy(How = How.Id, Using = "Schoolcode")]
            private IWebElement _markCombobox;

            [FindsBy(How = How.CssSelector, Using = "[data-menu-column-id='_MinutesLate'] .group-body input[type='text']")]
            private IWebElement _minuteLateCombobox;

            [FindsBy(How = How.Name, Using = "Result.Comment-SpecificMarks")]
            private IWebElement _commentsCombobox;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='overwrite-existing-Schoolcode']")]
            private IWebElement _overrideModeOfMarksCheckbox;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='overwrite-existing-Comment']")]
            private IWebElement _overrideModeOfCommentsCheckbox;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='overwrite-existing-MinutesLate']")]
            private IWebElement _overrideModeOfMinuteLatesCheckbox;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='floodfill-selected-Schoolcode']")]
            private IWebElement _applySelectedModeOfMarksButton;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='floodfill-selected-MinutesLate']")]
            private IWebElement _applySelectedModeOfMinutesLateButton;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='floodfill-selected-Comment']")]
            private IWebElement _applySelectedModeOfCommentsButton;

            public bool OverrideMarks
            {
                set { _overrideModeOfMarksCheckbox.Set(value); }
            }

            public bool OverrideComments
            {
                set { _overrideModeOfCommentsCheckbox.Set(value); }
            }

            public bool OverrideMinuteLate
            {
                set { _overrideModeOfMinuteLatesCheckbox.Set(value); }
            }

            public string FloodFillMarks
            {
                set
                {
                    _markCombobox.SelectByText(value);
                }
            }

            public string FloodFillMinuteLate
            {
                set
                {
                    _minuteLateCombobox.SetText(value);
                }
            }

            public string FloodFillComments
            {
                set
                {
                    _commentsCombobox.SetText(value);
                }
            }

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
                lstColumns.Add(new Column(sectionElements[0], sectionElements[1]));

                sectionElements = tableElement.FindElements(By.CssSelector(".webix_ss_header .webix_hs_center [section='header']"));
                IList<IWebElement> lstHeaderElements = sectionElements[0].FindElements(By.CssSelector("td"));
                IList<IWebElement> lstActionElements = sectionElements[1].FindElements(By.CssSelector("td"));
                for (int i = 0; i < lstHeaderElements.Count; i++)
                {
                    lstColumns.Add(new Column(lstHeaderElements[i], lstActionElements[i]));
                }
                return lstColumns;
            }

            public void ApplySelectedModeOfMarks()
            {
                _applySelectedModeOfMarksButton.Click();
                AutomationSugar.WaitForAjaxCompletion();
            }

            public void ApplySelectedModeOfMinuteLate()
            {
                _applySelectedModeOfMinutesLateButton.Click();
            }

            public void ApplySelectedModeOfComments()
            {
                _applySelectedModeOfCommentsButton.Click();
                AutomationSugar.WaitForAjaxCompletion();
            }

            #endregion

            public class Column
            {
                IWebElement headerElement;
                IWebElement actionElement;

                public Column(IWebElement _headerElement, IWebElement _actionElement)
                {
                    headerElement = _headerElement;
                    actionElement = _actionElement;
                }

                public string HeaderText
                {
                    get { return headerElement.FindElement(By.CssSelector("span")).GetText(); }
                }

                public void Select()
                {
                    headerElement.Click();
                }

                public void ClickFilter()
                {
                    IWebElement filterButton = actionElement.FindElement(By.CssSelector("span"));
                    filterButton.Click();
                }

                public void ClickDownArrow()
                {
                    headerElement.FindElement(By.CssSelector(".fa-caret-down")).Click();
                }
            }

        }
        #endregion


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
        }
    }
}
