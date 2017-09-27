using System;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class AdHocColumn : BaseSeleniumComponents
    {
        [FindsBy(How = How.CssSelector, Using = "input[name='adHocColumns_ColumnName']")]
        private IWebElement AdHocColumns_ColumnName;

        [FindsBy(How = How.CssSelector, Using = "button[data-value='AND']")]
        private IWebElement NumericType;

        [FindsBy(How = How.CssSelector, Using = "button[data-value='ADT']")]
        private IWebElement TextType;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='adhoc_Next']")]
        private IWebElement AdHoc_Next;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='adhoc_Done']")]
        private IWebElement AdHoc_Done;

        public AdHocColumn()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public AdHocColumn AddColumn(string ColumnName, ColumnType columnType)
        {
            AdHocColumns_ColumnName.WaitUntilState(ElementState.Displayed);
            AdHocColumns_ColumnName.SendKeys(ColumnName);
            if (columnType == ColumnType.Numeric)
                NumericType.Click();
            else
                TextType.Click();
            return this;
        }

        public AdHocColumn AddColumn_Done()
        {
            AdHoc_Done.Click();
            return this;
        }

        public AdHocColumn AdHoc_Column_LinkPeriod_Click()
        {
            AdHoc_Next.Click();
            return this;
        }
    }

    public enum ColumnType
    {
        Text,
        Numeric
    }
}
