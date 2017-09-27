using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;
using System.Collections.Generic;

namespace POM.Components.Pupil
{
    public class BulkUpdatePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("bulk_update_pupil_basic_detail"); }
        }

        #region Page Properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='identifier_columns_button']")]
        private IWebElement _identifierColumnsButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='data_items_button']")]
        private IWebElement _dataItemsButton;

        [FindsBy(How = How.CssSelector, Using = ".webix_dtable")]
        private IWebElement _bulkUpdateDetailsTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        [FindsBy(How = How.Id, Using = "ModeOfTravel")]
        private IWebElement _floodFillCombobox;

        public BulkUpdateGrid BulkUpdateTable
        {
            get { return new BulkUpdateGrid(_bulkUpdateDetailsTable); }
        }

        #endregion

        #region Page Actions
        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
        }

        public PupilIdentifierColumnDialog IndentifierColumn()
        {
            _identifierColumnsButton.Click();
            return new PupilIdentifierColumnDialog();
        }

        public PupilDataItemDialog DataItem()
        {
            _dataItemsButton.Click();
            return new PupilDataItemDialog();
        }
        public bool IsMessageSuccessAppear()
        {
            return _messageSuccess.IsElementExists();
        }

        public bool VerifyModeOfTravel(List<string> modeOfTravels)
        {
            List<string> currentModeOfTravels = new List<string>();
            IList<IWebElement> optionElements = _floodFillCombobox.FindElements(SimsBy.CssSelector("option"));
            foreach(IWebElement el in optionElements)
            {
                currentModeOfTravels.Add(el.GetText());
            }
            currentModeOfTravels.Remove("");
            currentModeOfTravels.Sort();
            modeOfTravels.Sort();
            return SeleniumHelper.DoesListItemEqual(currentModeOfTravels, modeOfTravels);
        }
        #endregion

        #region BulkUpdate Grid
        public class BulkUpdateGrid : WebixComponent<WebixCell>
        {
            public BulkUpdateGrid(IWebElement _webElement)
                : base(_webElement) { }

            public override By ComponentIdentifier
            {
                get { return SimsBy.AutomationId("bulk_update_pupil_basic_detail"); }
            }

            #region Properties

            [FindsBy(How = How.Id, Using = "ModeOfTravel")]
            private IWebElement _modeOfTravelCombobox;

            [FindsBy(How = How.Id, Using = "ServiceChildren")]
            private IWebElement _serviceChildrenCombobox;

            [FindsBy(How = How.Id, Using = "ServiceChildrenSource")]
            private IWebElement _serviceChildrenSourceCombobox;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='overwrite-existing-ModeOfTravel']")]
            private IWebElement _overrideModeOfTravelCheckbox;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='floodfill-selected-ModeOfTravel']")]
            private IWebElement _applySelectedModeOfTravelButton;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='floodfill-selected-ServiceChildren']")]
            private IWebElement _applySelectedServiceChildrenButton;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='floodfill-selected-ServiceChildrenSource']")]
            private IWebElement _applySelectedServiceSourceButton;

            public bool OverrideModeOfTravel
            {
                set { _overrideModeOfTravelCheckbox.Set(value); }
            }

            public string FloodFillModeOfTravel
            {
                set
                {
                    _modeOfTravelCombobox.SelectByText(value);
                }
            }

            public string FloodFillServiceChildren
            {
                set
                {
                    _serviceChildrenCombobox.SelectByText(value);
                }
            }

            public IList<Column> Columns
            {
                get { return GetColumns(); }
            }

            public string FloodFillServiceSource
            {
                set
                {
                    _serviceChildrenSourceCombobox.SelectByText(value);
                }
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

            public string GenderFilter
            {
                set
                {
                    IWebElement genderElement = SeleniumHelper.Get(SimsBy.CssSelector(String.Format("[webix_l_id='{0}']", value)));
                    genderElement.Click();
                    IWebElement applyButton = SeleniumHelper.Get(SimsBy.Xpath(String.Format("//button[text()='Apply']", value)));
                    applyButton.Click();
                }
            }

            public void ApplySelectedModeOfTravel()
            {
                _applySelectedModeOfTravelButton.Click();
            }

            public void ApplySelectedServiceChildren()
            {
                _applySelectedServiceChildrenButton.Click();
            }

            public void ApplySelectedServiceSource()
            {
                _applySelectedServiceSourceButton.Click();
            }

            #endregion
        }

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
                headerElement.FindElement(By.CssSelector(".fa-angle-down")).Click();
            }
        }

        #endregion
    }
}
