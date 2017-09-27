using System.Threading;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TestSettings;
using WebDriverRunner.webdriver;
using SeSugar.Automation;

namespace Assessment.Components
{
    public class FormulaBarPage : BaseSeleniumComponents
    {

        public const string FormulaBarSelector = "[data-input-formulabar='']";
        [FindsBy(How = How.CssSelector, Using = FormulaBarSelector)]
        public IWebElement _formulaBarElement;

        public const string CommentBarSelector = "[data-textarea-formulabar='']";
        [FindsBy(How = How.CssSelector, Using = CommentBarSelector)]
        public IWebElement _commentBarElement;

        [FindsBy(How = How.CssSelector, Using = "[data-expand-collapse-formula-bar='']")]
        public IWebElement _expandCommentBarElement;

        public FormulaBarPage(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.TestUser)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            SeleniumHelper.Login(userType);
            //SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Marksheets");
            CommonFunctions.GotToMarksheetMenu();
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public FormulaBarPage OpenMarksheet(string marksheetName)
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(marksheetName));
            return this;
        }

        public FormulaBarPage AddValue(string columnName, string value)
        {
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            IWebElement column = allColumns.FirstOrDefault(col => col.Text == columnName);
            string columnNo = column != null ? column.GetAttribute("column") : "1";

            List<IWebElement> editableCellsforColumn = MarksheetGridHelper.FindcellsForColumn(columnNo);

            if (editableCellsforColumn != null && editableCellsforColumn.Count > 0)
            {
                Actions action = new Actions(WebContext.WebDriver);

                editableCellsforColumn.First().Click();
                _formulaBarElement.Click();
                _formulaBarElement.Clear();
                _formulaBarElement.SendKeys(value);
                action.SendKeys(Keys.Enter).Perform();
            }

            return this;

        }

        //to multiple cells
        public FormulaBarPage AddValueToColumn(string columnName, string value)
        {
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            IWebElement column = allColumns.FirstOrDefault(col => col.Text == columnName);
            string columnNo = column != null ? column.GetAttribute("column") : "1";
            MarksheetGridHelper.FindColumnByColumnNumber(columnNo).Click();

            Actions action = new Actions(WebContext.WebDriver);

            _formulaBarElement.Click();
            _formulaBarElement.Clear();
            _formulaBarElement.SendKeys(value);
            action.SendKeys(Keys.Enter).Perform();

            return this;

        }

        public FormulaBarPage AddComment(string columnName, string value, bool haveToClick)
        {
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            IWebElement column = allColumns.FirstOrDefault(col => col.Text == columnName);
            string columnNo = column != null ? column.GetAttribute("column") : "1";

            List<IWebElement> editableCellsforColumn = MarksheetGridHelper.FindcellsForColumn(columnNo);

            if (editableCellsforColumn != null && editableCellsforColumn.Count > 0)
            {
                Actions action = new Actions(WebContext.WebDriver);
                editableCellsforColumn.First().WaitUntilState(ElementState.Enabled);
                
                if (haveToClick)
                {
                    editableCellsforColumn.First().Click();                    
                }

                _commentBarElement.Click();
                _commentBarElement.WaitUntilState(ElementState.Enabled);
                _commentBarElement.Clear();
                
                if (value.Length > 500)
                {
                    _commentBarElement.SendKeys(value.Substring(0, 500));
                    _commentBarElement.SendKeys(value.Substring(500, (value.Length - 500)));
                }
                else
                {
                    _commentBarElement.SendKeys(value);
                }
                action.SendKeys(Keys.Enter).Perform();
            }

            return this;
        }

        //to multiple cells
        public FormulaBarPage AddCommentToColumn(string columnName, string value)
        {
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            IWebElement column = allColumns.FirstOrDefault(col => col.Text == columnName);
            string columnNo = column != null ? column.GetAttribute("column") : "1";
            MarksheetGridHelper.FindColumnByColumnNumber(columnNo).Click();

            Actions action = new Actions(WebContext.WebDriver);

            _commentBarElement.Click();
            _commentBarElement.Clear();
            _commentBarElement.SendKeys(value);
            action.SendKeys(Keys.Enter).Perform();

            return this;

        }

        public FormulaBarPage ExpandCommantBar()
        {
            _expandCommentBarElement.Click();

            return this;

        }

    }
}
