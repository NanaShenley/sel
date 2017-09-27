using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Linq;
using WebDriverRunner.webdriver;
using Staff.POM.Helper;
using SeSugar.Automation;
using System;

namespace Staff.POM.Base
{
    public class GridComponent<TrRow> where TrRow : GridRow, new()
    {
        protected IWebElement _component;
        protected readonly By _componentIdentifier;
        protected readonly By _parentComponentIdentifier;
        protected readonly List<TrRow> _rows = new List<TrRow>();


        public TrRow this[int index]
        {
            get
            {
                var row = _rows[index];
                return row;
            }
        }

        public List<TrRow> Rows { get { return _rows; } }

        public TrRow LastInsertRow { get { return _rows.Count >= 2 ? _rows[_rows.Count - 2] : _rows.Last(); } }

        public GridComponent(By componentIdentifier)
        {
            _componentIdentifier = componentIdentifier;
            Initialise();
        }

        public GridComponent(By componentIdentifier, By parentComponentIdentifier)
        {
            _componentIdentifier = componentIdentifier;
            _parentComponentIdentifier = parentComponentIdentifier;
            Initialise();
        }

        public void Refresh()
        {
            Initialise();
        }

        public void RefreshRow(string rowId)
        {
            var row = this._rows.Single(x => x.RowId == rowId);
            PageFactory.InitElements(this._rows.Single(x => x.RowId == rowId), new ElementLocator(_rows.Single(x => x.RowId == rowId).RowSelector));
        }

        public TrRow GetLastRow()
        {
            this.Refresh();
            return _rows[_rows.Count - 1];
        }

        public TrRow GetLastInsertedRow()
        {
            this.Refresh();
            if (_rows.Count >= 2)
            {
                return _rows[_rows.Count - 2];
            }
            else
            {
                return _rows[_rows.Count - 1];
            }
        }

        public void DeleteRowIfExist(GridRow row)
        {
            if (row != null)
            {
                row.DeleteRow();
                this.Refresh();
            }
        }

        protected virtual void Initialise()
        {
            if (_parentComponentIdentifier != null)
            {
                _component = WebContext.WebDriver.FindElement(_parentComponentIdentifier).FindElement(_componentIdentifier);
            }
            else
            {
                _component = WebContext.WebDriver.FindElement(_componentIdentifier);
            }
            PageFactory.InitElements(this, new ElementLocator(_component));

            const string gridRowCss = "tbody tr[data-role=\"gridRow\"]";

            var rowIds = _component.DeStaler(_componentIdentifier).FindElements(By.CssSelector(gridRowCss)).Select(x => x.GetAttribute("data-row-id")).ToList();
            _rows.Clear();
            foreach (var rowId in rowIds)
            {
                var rowComponent = new TrRow();
                var rowCss = _component.DeStaler(_componentIdentifier).FindElement(By.CssSelector("[data-row-id='" + rowId + "']"));

                PageFactory.InitElements(rowComponent, new ElementLocator(rowCss));
                rowComponent.RefreshAction = RefreshRow;
                rowComponent.RowSelector = new ByChained(_componentIdentifier, By.CssSelector("[data-row-id='" + rowId + "']"));
                rowComponent.RowId = rowId;
                _rows.Add(rowComponent);
            }
        }
    }
       

    public class GridRow
    {        
        public Action<string> RefreshAction { get; set; }
        public By RowSelector { get; set; }
        public string RowId { get; set; }

        public void Refresh()
        {
            RefreshAction(this.RowId);
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='remove_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
        private IWebElement _editButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='more..._button']")]
        private IWebElement _moreButton;

        public bool IsRowDeleteButtonEnabled
        {
            get
            {
                string deleteBtnAttr = _deleteButton.GetAttribute("disabled");
                return string.IsNullOrEmpty(deleteBtnAttr) ? true : false;
            }

        }

        public bool IsRowEditButtonEnabled
        {
            get
            {
                string editBtnAttr = _editButton.GetAttribute("disabled");
                return string.IsNullOrEmpty(editBtnAttr) ? true : false;
            }

        }
        public void ClickDelete()
        {
            _deleteButton.ClickByJS();
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void ClickEdit()
        {
            _editButton.ClickByJS();
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void ClickMore()
        {
            _moreButton.ClickByJS();
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void DeleteRow()
        {
            AutomationSugar.WaitFor(new ByChained(RowSelector, SimsBy.AutomationId("remove_button")));
            AutomationSugar.ClickOn(new ByChained(RowSelector, SimsBy.AutomationId("remove_button")));
            AutomationSugar.WaitForAjaxCompletion();

            AutomationSugar.WaitFor(new ByChained(By.CssSelector(".popover"), SimsBy.AutomationId("Yes_button")));
            AutomationSugar.ClickOn(new ByChained(By.CssSelector(".popover"), SimsBy.AutomationId("Yes_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }
    }
}
