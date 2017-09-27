using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using WebDriverRunner.webdriver;
// ReSharper disable InconsistentNaming
#pragma warning disable 649

namespace SharedServices.Components.Common
{
    public sealed class GridComponent<TRRow> where TRRow : GridRow, new()
    {
        private IWebElement _component;
        private readonly By _componentIdentifier;
        private readonly By _parentComponentIdentifier;
        private readonly List<TRRow> _rows = new List<TRRow>();

        public TRRow this[int index]
        {
            get
            {
                var row = _rows[index];
                return row;
            }
        }

        public List<TRRow> Rows { get { return _rows; } }

        public TRRow LastInsertRow { get { return _rows.Count >= 2 ? _rows[_rows.Count - 2] : _rows.Last(); } }

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
            PageFactory.InitElements(_rows.Single(x => x.RowId == rowId), new ElementLocator(_rows.Single(x => x.RowId == rowId).RowSelector));
        }

        public TRRow GetLastRow()
        {
            Refresh();
            return _rows[_rows.Count - 1];
        }

        public TRRow GetLastInsertedRow()
        {
            Refresh();
            if (_rows.Count >= 2)
            {
                return _rows[_rows.Count - 2];
            }
            return _rows[_rows.Count - 1];
        }

        public void DeleteRowIfExist(GridRow row)
        {
            if (row != null)
            {
                row.DeleteRow();
                Refresh();
            }
        }

        private void Initialise()
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

            var rowIds = _component.FindElements(By.CssSelector(gridRowCss)).Select(x => x.GetAttribute("data-row-id")).ToList();
            _rows.Clear();
            foreach (var rowId in rowIds)
            {
                var rowComponent = new TRRow();
                var rowCss = _component.FindElement(By.CssSelector("[data-row-id='" + rowId + "']"));
                
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
            RefreshAction(RowId);
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='remove_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
        private IWebElement _editButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='more..._button']")]
        private IWebElement _moreButton;

        public void ClickDelete()
        {
            _deleteButton.ClickByJs();
        }

        public void ClickEdit()
        {
            _editButton.ClickByJs();
        }

        public void ClickMore()
        {
            _moreButton.ClickByJs();
        }

        public void DeleteRow()
        {
            AutomationSugar.WaitFor(new ByChained(RowSelector, SimsBy.AutomationId("remove_button")));
            AutomationSugar.ClickOn(new ByChained(RowSelector, SimsBy.AutomationId("remove_button")));
            AutomationSugar.WaitForAjaxCompletion();

            AutomationSugar.WaitFor(new ByChained(By.CssSelector(".popover-confirm"), SimsBy.AutomationId("Yes_button")));
            AutomationSugar.ClickOn(new ByChained(By.CssSelector(".popover-confirm"), SimsBy.AutomationId("Yes_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }
    }

    internal static class Extensions
    {
        public static void ClickByJs(this IWebElement element)
        {
            ((IJavaScriptExecutor) WebContext.WebDriver).ExecuteScript("arguments[0].click();", element);
        }
    }
}
