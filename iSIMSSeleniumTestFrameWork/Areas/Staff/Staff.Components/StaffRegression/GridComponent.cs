using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Collections;
using System.Linq;
using System.Threading;


namespace Staff.Components.StaffRegression
{
    public class GridComponent<TRow> where TRow : new()
    {
        private IWebElement _component;
        private readonly By _componentIdentifier;
        private readonly List<TRow> _rows = new List<TRow>();

        public TRow this[int index]
        {
            get
            {
                var row = default(TRow);
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

        public List<TRow> Rows { get { return _rows; } }

        public GridComponent(By componentIdentifier)
        {
            _componentIdentifier = componentIdentifier;
        }

        public void Refresh()
        {
            Initialise();
        }

        private void Initialise()
        {
            _component = WebContext.WebDriver.FindElement(_componentIdentifier);//.DeStaler(_componentIdentifier).WaitUntilState(ElementState.Displayed);
            PageFactory.InitElements(this, new ElementLocator(_component));

            const string gridRowCss = "tbody tr[data-role=\"gridRow\"]";

            var rowIds = _component.DeStaler(_componentIdentifier).FindElements(By.CssSelector(gridRowCss)).Select(x => x.GetAttribute("data-row-id")).ToList();
            _rows.Clear();
            foreach (var rowId in rowIds)
            {
                var rowComponent = new TRow();
                var rowCss = _component.DeStaler(_componentIdentifier).FindElement(By.CssSelector("[data-row-id='" + rowId + "']"));
                PageFactory.InitElements(rowComponent, new ElementLocator(rowCss));
                _rows.Add(rowComponent);
            }
        }
    }
}
