
using OpenQA.Selenium;
using PageObjectModel.Helper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PageObjectModel.Base
{
    public class TableElement
    {

        #region Properties

        public IWebElement tableElement;

        #endregion

        #region Methods

        public TableElement(IWebElement webElement)
        {
            tableElement = webElement;
        }

        public virtual List<IWebElement> GetRowElements()
        {
            ReadOnlyCollection<IWebElement> rows = tableElement.FindElements(SimsBy.CssSelector("tbody>tr"));
            return rows.ToList();
        }

        public virtual IWebElement GetRow(int index)
        {
            return GetRowElements()[index];
        }

        private List<IWebElement> GetCells(IWebElement row)
        {
            ReadOnlyCollection<IWebElement> cells = row.FindElements(SimsBy.CssSelector("td"));
            return cells.ToList();
        }

        #endregion
    }

}
