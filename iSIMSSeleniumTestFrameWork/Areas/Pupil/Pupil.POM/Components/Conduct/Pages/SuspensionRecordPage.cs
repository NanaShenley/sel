using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;
using SeSugar.Automation;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;


namespace POM.Components.Conduct
{
    public class SuspensionRecordPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("pupil_suspensions_detail"); }
        }


        #region Page propertise

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_suspension_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[title='Actions']")]
        private IWebElement _actionButton;

        public GridComponent<SuspensionExpulsion> SuspensionExpulsionGrid
        {
            get
            {
                GridComponent<SuspensionExpulsion> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SuspensionExpulsion>(By.CssSelector("[data-maintenance-container='Suspensions']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public static SuspensionRecordPage Create()
        {
            Wait.WaitUntilDisplayed(SimsBy.AutomationId("well_know_action_save"));
            return new SuspensionRecordPage();
        }


        public class SuspensionExpulsion
        {
            #region Properties
            [FindsBy(How = How.CssSelector, Using = "[name$= '.ExclusionTypeDisplay']")]
            private IWebElement _typeTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.ExclusionReasonDisplay']")]
            private IWebElement _reasonTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$= '.StartDateOnly']")]
            private IWebElement _startDateTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$= '.EndDateOnly']")]
            private IWebElement _endDateTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$= '.NumberOfSchoolDaysDisplay']")]
            private IWebElement _daysTextbox;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='remove_button']")]
            private IWebElement _removeButton;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _editButton;

            public string Type
            {
                get { return _typeTextbox.GetValue(); }
            }

            public string Reason
            {
                get { return _reasonTextbox.GetValue(); }
            }

            public string StartDate
            {
                get { return _startDateTextbox.GetValue(); }
            }

            public string EndDate
            {
                get { return _endDateTextbox.GetValue(); }
            }

            public string Days
            {
                get { return _daysTextbox.GetValue(); }
            }

            #endregion

            #region Action

            public AddSuspensionDialog ClickEditRecord()
            {

                _editButton.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                return new AddSuspensionDialog();
            }

            public bool isDeleteEnable()
            {
                return _removeButton.Enabled;
            }

            public bool isEditEnable()
            {
                return _editButton.Enabled;
            }

            public void DeleteRow()
            {
                if (_removeButton.IsExist())
                {
                    _removeButton.ClickByJS();
                    var _okButton = SeleniumHelper.Get(SimsBy.AutomationId("Yes_button"));
                    if (_okButton.IsExist())
                    {
                        _okButton.Click();
                    }
                }

            }

            public int IndexOfRow(GridComponent<SuspensionExpulsion> suspensionGrid)
            {
                IList<SuspensionExpulsion> listRow = suspensionGrid.Rows;
                foreach (SuspensionExpulsion rowItem in listRow)
                {
                    if (rowItem.Type.Trim().Equals(this.Type))
                    {
                        return listRow.IndexOf(rowItem);
                    }
                }
                return -1;
            }

            public void ClickDeleteRow()
            {
                _removeButton.Click();
            }

            #endregion

        }

        #endregion

        #region Page action

        public SuspensionRecordPage SaveValues()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SuspensionRecordPage();
        }

        public SuspensionRecordPage ClickDeleteRow(SuspensionExpulsion row)
        {
            if (row != null)
            {
                row.DeleteRow();
                _saveButton.Click();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new SuspensionRecordPage();
        }

        public SuspensionRecordPage ClickDeleteAllRow(List<SuspensionExpulsion> rows)
        {
            if (rows.Count > 0)
            {
                foreach (SuspensionExpulsion row in rows)
                {
                    row.DeleteRow();
                }
                _saveButton.Click();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }

            return new SuspensionRecordPage();
        }

        public AddSuspensionDialog ClickAddNewRecord()
        {
            _addButton.ClickByJS();
           // Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AddSuspensionDialog();
        }

        public bool IsSuccessMessageDisplay()
        {
            AutomationSugar.WaitForAjaxCompletion();
            return SeleniumHelper.IsElementExists(SimsBy.AutomationId("status_success"));
        }

        public bool IsYesNoDeleteButtonDisplayed()
        {
            return (SeleniumHelper.FindElement(SimsBy.AutomationId("Yes_button")).IsExist() && SeleniumHelper.FindElement(SimsBy.AutomationId("No_button")).IsExist());
        }

        public SuspensionRecordPage ConfirmDelete(SuspensionExpulsion row, bool isDelete = false)
        {
            IWebElement _cancelButton = SeleniumHelper.Get(SimsBy.AutomationId("No_button"));
            IWebElement _yesButton = SeleniumHelper.Get(SimsBy.AutomationId("Yes_button"));
            if (isDelete)
            {
                _yesButton.Click();
            }
            else
            {
                _cancelButton.Click();
            }
            return new SuspensionRecordPage();
        }

        public bool IsOverlapMessageDisplayed()
        {
            IList<IWebElement> elements = SeleniumHelper.FindElements(SimsBy.CssSelector(".validation-summary-errors li"));
            foreach (var element in elements)
            {
                if (element.GetText().Trim().Contains("Dates Overlap"))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
