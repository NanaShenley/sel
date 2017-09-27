using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class CopyColumnDialog : BaseSeleniumComponents
    {
        public const string copyColumnPopupSelector = "[data-show-copy-columns-dialog]";
        public const string targetColumnsSelector = "columnCheckboxlist";
        public const string copycolumnSelector = "[data-copy-columns='']";
        public const string copycolumnoverwrite = "[data-copy-column-overwrite='']";
        public const string deletevalues = "[data-delete-floodfill-values='']";
        public const string deleteconfpopup = "[data-floodfill-delete='']";

        public const string floodfilloverwrite = "data-floodfill-overwrite";

        public void OpenHeaderMenu(string columnHeader)
        {
            string openHeaderId = "header_menu_" + columnHeader;
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-automation-id ='" + openHeaderId + "']")));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id ='" + openHeaderId + "']"));
        }

        public void DeletePreviousValues(string columnHeader)
        {
            OpenHeaderMenu(columnHeader);
            WaitForElement(By.CssSelector(deletevalues));
            if (WebContext.WebDriver.FindElements(By.CssSelector(deletevalues))
                .ToList()
                .Any(ele => ele.Displayed))
            {
                WebContext.WebDriver.FindElements(By.CssSelector(deletevalues))
                        .ToList()
                        .FirstOrDefault(ele => ele.Displayed)
                        .Click();


            }
            WaitForElement(By.CssSelector(deleteconfpopup));
            WebContext.WebDriver.FindElement(By.CssSelector(deleteconfpopup)).Click();
            Thread.Sleep(1000);

        }

        //public void OpenCopyColumnsPopover(string columnHeader)
        //{
        //    OpenHeaderMenu(columnHeader);
        //    System.Threading.Thread.Sleep(1000);
        //    WaitForElement(By.CssSelector(copyColumnPopupSelector));
        //    if (WebContext.WebDriver.FindElements(By.CssSelector(copyColumnPopupSelector))
        //        .ToList()
        //        .Any(ele => ele.Displayed))
        //        WebContext.WebDriver.FindElements(By.CssSelector(copyColumnPopupSelector))
        //            .ToList()
        //            .FirstOrDefault(ele => ele.Displayed)
        //            .Click();
        //}

        public void OpenCopyColumnsPopover(string columnHeader)
        {
            WaitForAndClick(BrowserDefaults.TimeOut, SeleniumHelper.SelectByDataAutomationID("header_menu_" + columnHeader));
            
            if (WebContext.WebDriver.FindElements(By.CssSelector(copyColumnPopupSelector))
                .ToList()
                .Any(ele => ele.Displayed))
                WebContext.WebDriver.FindElements(By.CssSelector(copyColumnPopupSelector))
                    .ToList()
                    .FirstOrDefault(ele => ele.Displayed)
                    .Click();
        }

        public CopyColumnDialog OpenMarksheet(string marksheetName)
        {
            WaitUntilDisplayed(By.LinkText(marksheetName));
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(marksheetName));
            WaitUntillAjaxRequestCompleted();
            return this;
        }

        public void SelectTargetColumn(bool overwrite, string type)
        {
            WaitUntilDisplayed(By.Id(targetColumnsSelector));
            WaitForElement(By.Id(targetColumnsSelector));

            if (type == "one")
            {
                IWebElement TargetColumn =
                    WebContext.WebDriver.FindElements(By.Id(targetColumnsSelector)).First();

                TargetColumn.Click();

                if (overwrite)
                {
                    WebContext.WebDriver.FindElement(By.CssSelector(copycolumnoverwrite)).Click();
                }
                WebContext.WebDriver.FindElement(By.CssSelector(copycolumnSelector)).Click();
            }
            else if (type == "multiple")
            {
                List<IWebElement> TargetColumnsList =
                   WebContext.WebDriver.FindElements(By.Id(targetColumnsSelector)).ToList();

                foreach (IWebElement TargetCol in TargetColumnsList)
                {
                    TargetCol.Click();
                }

                if (overwrite)
                {
                    WebContext.WebDriver.FindElement(By.CssSelector(copycolumnoverwrite)).Click();
                }
                WebContext.WebDriver.FindElement(By.CssSelector(copycolumnSelector)).Click();
            }
            WaitUntillAjaxRequestCompleted();
        }
    }
}
