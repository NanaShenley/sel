using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using DataExchange.POM.Helper;
using System.Collections.Generic;
using System.Linq;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Base
{
    public class GridComponent<TrRow> where TrRow : new()
    {
        protected IWebElement _component;
        protected readonly By _componentIdentifier;
        protected readonly By _parentComponentIdentifier;
        protected readonly List<TrRow> _rows = new List<TrRow>();


        public bool WaitForNewRowAppear()
        {
            bool result = false;
            this.Refresh();
            int currentNumberRow = _rows.Count();
            int numberRowAdded = currentNumberRow;
            int timeout = TestSettings.BrowserDefaults.ObjectTimeOut;
            do
            {
                SeleniumHelper.Sleep(1);
                timeout--;
                this.Refresh();
                numberRowAdded = _rows.Count();
            }
            while (currentNumberRow == numberRowAdded && timeout > 0);
            return result;
        }

        public void WaitUntilRowAppear(Func<TrRow, bool> condition)
        {
            try
            {
                Wait.WaitUntil((d) =>
                {
                    Refresh();
                    return Rows.Any(condition);
                });
            }
            catch (WebDriverTimeoutException) { }
        }

        public TrRow this[int index]
        {
            get
            {
                var row = default(TrRow);
                try
                {
                    row = _rows[index];
                }
                catch
                {
                    Retry.Do(() =>
                    {
                        Initialise();
                        row = _rows[index];
                    });
                }

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
                _component = WebContext.WebDriver.FindElement(_parentComponentIdentifier).FindElement(_componentIdentifier);//.DeStaler(_componentIdentifier).WaitUntilState(ElementState.Displayed);
            }
            else
            {
                _component = WebContext.WebDriver.FindElement(_componentIdentifier);//.DeStaler(_componentIdentifier).WaitUntilState(ElementState.Displayed);
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
                _rows.Add(rowComponent);
            }
        }
    }

    public class Grid2Component<TrRow> : GridComponent<TrRow> where TrRow : new()
    {
        public Grid2Component(By componentIdentifier)
            : base(componentIdentifier)
        {
        }

        public Grid2Component(By componentIdentifier, By parentComponentIdentifier)
            : base(componentIdentifier, parentComponentIdentifier)
        {
        }

        protected override void Initialise()
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

            const string gridRowCss = "tbody tr";

            var listRows = _component.DeStaler(_componentIdentifier).FindElements(By.CssSelector(gridRowCss)).ToList();
            _rows.Clear();
            foreach (var row in listRows)
            {
                var rowComponent = new TrRow();
                PageFactory.InitElements(rowComponent, new ElementLocator(row));
                _rows.Add(rowComponent);
            }
        }
    }

    public class GridRow
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='remove_button']")]
        private IWebElement _deleteButton;

        public void ClickDelete()
        {
            _deleteButton.ClickByJS();
        }

        public void DeleteRow()
        {
            if (SeleniumHelper.IsExist(_deleteButton))
            {
                _deleteButton.ScrollToByAction();
                _deleteButton.ClickByJS();
                var _okButton = SeleniumHelper.Get(SimsBy.AutomationId("Yes_button"));
                if (SeleniumHelper.IsExist(_okButton))
                {
                    _okButton.Click();
                }
            }

        }
    }
}
