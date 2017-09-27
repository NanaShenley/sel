using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace Assessment.Components.PageObject
{
    public class POSSingleView
    {
        public POSSingleView()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "label[id='toolbar_singleview']")]
        private IWebElement SingleViewButton;

        [FindsBy(How = How.CssSelector, Using = "button[title='Choose Column']")]
        private IWebElement StatementsViewButton;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Clicks on the POSSingleView Button
        /// </summary>
        public POSSingleView ClickSingleViewButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SingleViewButton)).Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(StatementsViewButton));
            return new POSSingleView();
        }

        /// <summary>
        /// Clicks on the StatementsView Button
        /// </summary>
        public void ClickStatementsViewButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(StatementsViewButton));
            StatementsViewButton.Click();
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Verifies if the % of POS Expectations column is present on the data maintainance screen
        /// </summary>
        public bool VerifySummaryRowsSingleView(String ColumnWebelement)
        {

            ReadOnlyCollection<IWebElement> SummaryRows = WebContext.WebDriver.FindElements(By.CssSelector("div[class='datamaintenance-panel'] table[class='table table-condensed table-striped'] th[scope='row']"));
            foreach (IWebElement EachRow in SummaryRows)
            {
                if (EachRow.Text == ColumnWebelement)
                {
                    return true;
                }
                    
            }
            return false;
            //try
            //{
            //    waiter.Until(ExpectedConditions.ElementExists(ColumnWebelement));
            //    return true;
            //}
            //catch(Exception e)
            //{
            //    return false;
            //}
        }

    }
}
