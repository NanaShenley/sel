using OpenQA.Selenium;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POM.Base
{
    public class WebixComponent<Cell> : BaseComponent where Cell : WebixCell, new()
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public IWebElement tableElement;

        public WebixFooterComponent Summary
        {
            get { return new WebixFooterComponent(tableElement); }
        }

        public WebixComponent(IWebElement webElement)
        {
            tableElement = webElement;
        }

        public int RowCount
        {
            get
            {
                return tableElement.FindElements(By.CssSelector(".webix_ss_center .webix_column.webix_first .webix_cell")).Count;
            }
        }

        public WebixRow<Cell> this[string rowName]
        {
            get
            {
                return GetRow(rowName);
            }
        }

        public WebixRow<Cell> this[int rowIndex]
        {
            get
            {
                return GetRow(rowIndex);
            }
        }

        private WebixRow<Cell> GetRow(string rowName)
        {
            int rowIndex = GetRowIndex(rowName);
            return rowIndex == -1 ? null : new WebixRow<Cell>(tableElement, rowIndex);
        }

        private WebixRow<Cell> GetRow(int rowIndex)
        {
            return new WebixRow<Cell>(tableElement, rowIndex);
        }

        public int RowNumber()
        {
            int count = 0;
            IList<IWebElement> cols = tableElement.FindElements(By.CssSelector(".webix_ss_center .webix_column"));
            foreach (IWebElement col in cols)
            {
                count = col.FindElements(By.CssSelector(".webix_cell")).Count;
                break;
            }
            return count;
        }

        protected virtual int GetRowIndex(string rowName)
        {
            bool isExist = SeleniumHelper.ScrollDownUntilName(tableElement, By.CssSelector(".webix_ss_left .read-only .webix_cell"), rowName);
            IList<IWebElement> rows = tableElement.FindElements(By.CssSelector(".webix_ss_left .read-only .webix_cell"));
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].GetText().Trim().Equals(rowName))
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual IList<WebixRow<Cell>> Rows
        {
            get
            {
                IList<WebixRow<Cell>> listWebixRow = new List<WebixRow<Cell>>();
                IList<IWebElement> rows = tableElement.FindElements(By.CssSelector(".webix_ss_left .read-only .webix_cell"));

                if (rows.Count == 0)
                {
                    rows = tableElement.FindElements(By.CssSelector(".webix_ss_center .webix_column.webix_first .webix_cell"));
                }
                for (int i = 0; i < rows.Count; i++)
                {
                    listWebixRow.Add(new WebixRow<Cell>(tableElement, i));
                }
                return listWebixRow;
            }
        }

        public virtual WebixRow<Cell> FirstOrDefaut(Func<WebixRow<Cell>, bool> condition)
        {
            while (!Rows.Any(condition))
            {
                SeleniumHelper.ScrollBy(tableElement, 0, 100);
            }
            return Rows.FirstOrDefault(condition);
        }

        public void ScrollToTopLeft()
        {
            SeleniumHelper.ScrollTo(tableElement, 0, 0);
        }
    }

    /// <summary>
    /// Webix Row
    /// </summary>
    public class WebixRow<Cell> where Cell : WebixCell, new()
    {
        IWebElement _tableElement;
        int _index;
        Dictionary<int, Cell> _row = new Dictionary<int, Cell>();

        public Cell this[int i]
        {
            get
            {
                FindCell(i);
                return _row.ContainsKey(i) ? _row[i] : null;
            }
            set
            {
                _row[i] = value;
            }
        }

        public Cell this[string columnName]
        {
            get
            {
                int index = GetIndexByName(columnName);
                FindCell(index);
                return _row.ContainsKey(index) ? _row[index] : null;
            }
        }

        public int Index
        {
            get { return _index; }
        }

        public Dictionary<int, Cell> ListCells { get { return _row; } }

        public WebixRow(IWebElement tableElement, int rowIndex)
        {
            _tableElement = tableElement;
            _index = rowIndex;
            Initialise();
        }

        public void Initialise()
        {
            if (_index != -1)
            {
                IList<IWebElement> cols = _tableElement.FindElements(By.CssSelector(".webix_column"));
                foreach (IWebElement col in cols)
                {
                    int columnIndex = int.Parse(col.GetAttribute("column"));

                    Cell cell = Activator.CreateInstance(typeof(Cell), col.FindElements(By.CssSelector(".webix_cell"))[_index]) as Cell;
                    _row.Add(columnIndex, cell);
                }
            }
        }

        public virtual void FindCell(int columnIndex)
        {
            if (!_row.ContainsKey(columnIndex))
            {
                string cssSelector = ".webix_column[column='{0}']";
                bool isExist = SeleniumHelper.ScrollRightUntilExist(_tableElement, By.CssSelector(String.Format(cssSelector, columnIndex)));
                if (isExist)
                {
                    IWebElement column = _tableElement.FindElement(By.CssSelector(String.Format(cssSelector, columnIndex)));

                    Cell cell = Activator.CreateInstance(typeof(Cell), column.FindElements(By.CssSelector(".webix_cell"))[_index]) as Cell;
                    _row.Add(columnIndex, cell);
                }
            }
        }

        private int GetIndexByName(string columnName)
        {

            const string css = ".webix_ss_header td";
            int index = -1;
            IList<IWebElement> listColumnElement = _tableElement.FindElements(SimsBy.CssSelector(css));
            foreach (var element in listColumnElement)
            {
                if (element.GetText().Trim().Equals(columnName))
                {
                    index = int.Parse(element.GetAttribute("column"));
                    break;
                }
            }
            return index;
        }

        public void ClickCheckBox(int columnIndex)
        {
            IWebElement checkbox = _row[columnIndex].webElement.FindElement(By.CssSelector("[type='checkbox']"));
            checkbox.Set(true);
        }

        public bool IsBirthdayCakeDisplay()
        {
            try
            {
                IWebElement _birthdayCake = _row[0].webElement.FindElement(SimsBy.CssSelector("[title = 'Birthday']"));
                return _birthdayCake.IsExist();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ClickCellPupilName()
        {
            try
            {
                IWebElement _button = _row[0].webElement.FindElement(SimsBy.CssSelector("button"));
                if (_button.IsExist())
                {
                    _button.Click();
                    Wait.WaitLoading();
                }
            }
            catch (Exception) { }
        }
    }

    public class WebixCell
    {
        public IWebElement webElement;

        public WebixCell()
        {
        }

        public WebixCell(IWebElement _webElement)
        {
            webElement = _webElement;
        }

        public virtual string Text
        {
            get
            {
                try
                {
                    return webElement.FindElement(By.CssSelector("span")).GetText();
                }
                catch (Exception)
                {
                    return webElement.GetText();
                }
            }
            set
            {
                if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
                {
                    webElement.Click();
                }

                if (!value.Equals(string.Empty))
                {
                    SeleniumHelper.FindElement(By.Name("AttendanceRecord.SchoolAttendanceCode")).SendKeys(value);
                }
                else
                {
                    SeleniumHelper.FindElement(By.Name("AttendanceRecord.SchoolAttendanceCode")).SendKeys(Keys.Backspace);
                }
            }
        }

        public string Value
        {
            get
            {
                try
                {
                    return webElement.FindElement(By.CssSelector("span")).GetText();
                }
                catch (Exception)
                {
                    return webElement.GetText();
                }
            }
            set
            {
                try
                {
                    webElement.ClickUntilAppearElement(By.Name("AttendanceRecord.SchoolAttendanceCode"));
                    SeleniumHelper.Get(By.Name("AttendanceRecord.SchoolAttendanceCode")).SendKeys(value);
                }
                catch (Exception ex)
                {
                    // Handle issue Mode is preveted
                    return;
                }
            }
        }


        public void Focus()
        {
            try
            {
                webElement.ClickUntilAppearElement(By.Name("AttendanceRecord.SchoolAttendanceCode"));
            }
            catch (Exception)
            {

            }
        }

        public void Select()
        {
            if (!(webElement.GetAttribute("class").Contains("webix_cell_select")))
            {
                webElement.ClickByJS();
            }
        }


        public void DoubleClick()
        {
            if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
            {
                webElement.Click();
            }
            SeleniumHelper.FindElement(By.Name("AttendanceRecord.SchoolAttendanceCode")).Click();
        }

        public void OpenComment()
        {
            if (!(webElement.GetAttribute("class").Contains("webix_ss_center_scroll") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
            {
                //webElement.Click();
            }
            SeleniumHelper.FindElement(By.CssSelector("[title='Add/Edit Comments']")).Click();
            //SeleniumHelper.FindElement(By.CssSelector("[title='Add/Edit Note']")).ClickByJS();
        }

        public bool OpenSelectCodeDropDown()
        {

            IWebElement inputElement = null;
            try
            {
                inputElement = webElement.FindElement(By.Name("AttendanceRecord.SchoolAttendanceCode"));
            }
            catch (Exception)
            {
                inputElement = null;
            }

            if (inputElement != null)
            {
                webElement.FindElement(By.Name("AttendanceRecord.SchoolAttendanceCode")).ClickAndWaitFor(SimsBy.CssSelector("[view_id='$suggest1_list']"));
            }
            else
            {
                webElement.ClickAndWaitFor(By.Name("AttendanceRecord.SchoolAttendanceCode"));
                // Wait between two time click.
                SeleniumHelper.Sleep(2);
                SeleniumHelper.FindElement(By.Name("AttendanceRecord.SchoolAttendanceCode")).ClickByJS();
                SeleniumHelper.Sleep(2);

                try
                {
                    inputElement = SeleniumHelper.FindElement(SimsBy.CssSelector("[view_id='$suggest1_list']"));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                //SeleniumHelper.FindElement(By.Name("AttendanceRecord.SchoolAttendanceCode")).ClickAndWaitFor(SimsBy.CssSelector("[view_id='$suggest1_list']"));
            }
            return true;
        }

    }

    ////////////////// WEBIX FOOTER //////////////////

    public class WebixFooterComponent
    {

        public IWebElement tableElement;

        public WebixFooterComponent(IWebElement webElement)
        {
            tableElement = webElement;
        }

        private WebixFooterRow GetFooterRow(int rowIndex)
        {
            return new WebixFooterRow(tableElement, rowIndex);
        }

        public WebixFooterRow this[int rowIndex]
        {
            get
            {
                return GetFooterRow(rowIndex);
            }
        }

        public WebixFooterRow this[string rowName]
        {
            get
            {
                return GetFooterRow(rowName);
            }
        }

        private WebixFooterRow GetFooterRow(string rowName)
        {
            int rowIndex = GetRowIndex(rowName);
            return rowIndex == -1 ? null : new WebixFooterRow(tableElement, rowIndex);
        }

        int GetRowIndex(string rowName)
        {
            IList<IWebElement> rows = tableElement.FindElements(By.CssSelector(".webix_hs_left [section='footer'] .webix_hcell"));

            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].GetText().Trim().Equals(rowName))
                {
                    return i;
                }
            }
            return -1;
        }
    }

    /// <summary>
    /// Webix Row Footer
    /// </summary>
    public class WebixFooterRow
    {
        IWebElement _tableElement;
        int _index;
        Dictionary<int, IWebElement> _row = new Dictionary<int, IWebElement>();

        public IWebElement this[int i]
        {
            get
            {
                FindCell(i);
                return _row.ContainsKey(i) ? _row[i] : null;
            }
            set
            {
                _row[i] = value;
            }
        }

        public Dictionary<int, IWebElement> ListCells { get { return _row; } }

        public WebixFooterRow(IWebElement tableElement, int rowIndex)
        {
            _tableElement = tableElement;
            _index = rowIndex;
            Initialise();
        }

        public void Initialise()
        {
            if (_index != -1)
            {
                IList<IWebElement> rows = _tableElement.FindElements(By.CssSelector(".webix_ss_footer .webix_hs_center [section = 'footer']"));
                IList<IWebElement> cols = rows[_index].FindElements(By.TagName("td"));

                foreach (var col in cols)
                {
                    int columnIndex = int.Parse(col.GetAttribute("column"));

                    _row.Add(columnIndex, col.FindElement(By.CssSelector(".webix_hcell")));
                }

            }
        }

        public void FindCell(int columnIndex)
        {
            if (!_row.ContainsKey(columnIndex))
            {
                string cssSelector = ".webix_ss_footer .webix_hs_center [section = 'footer'] td[column='{0}']";
                bool isExist = SeleniumHelper.ScrollRightUntilExist(_tableElement, By.CssSelector(String.Format(cssSelector, columnIndex)));
                if (isExist)
                {
                    IWebElement column = _tableElement.FindElement(By.CssSelector(String.Format(cssSelector, columnIndex)));
                    _row.Add(columnIndex, column.FindElement(By.CssSelector(".webix_hcell")));
                }
            }
        }
    }
}
