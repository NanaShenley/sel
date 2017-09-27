
using OpenQA.Selenium;
using POM.Helper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace POM.Base
{

    public class TableRowElement
    {
        #region Properties

        public IWebElement rowElement;
        public int rowIndex;

        #endregion

        #region Methods

        public TableRowElement(IWebElement webElement, int index)
        {
            rowElement = webElement;
            rowIndex = index;
        }

        public virtual List<IWebElement> GetCells()
        {
            ReadOnlyCollection<IWebElement> cells = rowElement.FindElements(SimsBy.CssSelector("td"));
            return cells.ToList();
        }

        public virtual IWebElement GetCell(int index)
        {
            return GetCells()[index];
        }

        public virtual List<IWebElement> GetGridCells()
        {
            ReadOnlyCollection<IWebElement> cells = rowElement.FindElements(SimsBy.CssSelector("td[class*='grid-cell']"));
            return cells.ToList();
        }

        public void ClickDelete()
        {
            rowElement.FindElement(SimsBy.CssSelector("[data-automation-id='remove_button']")).ClickByJS();
        }

        public void DeleteRow()
        {
            var removeElement = rowElement.FindElement(SimsBy.AutomationId("remove_button"));
            if (SeleniumHelper.IsExist(removeElement))
            {
                removeElement.ClickByJS();
                var _okButton = SeleniumHelper.Get(SimsBy.AutomationId("Yes_button"));
                if (SeleniumHelper.IsExist(_okButton))
                {
                    _okButton.Click();
                    SeleniumHelper.Sleep(3);
                }
            }

        }

        #endregion

    }
}
