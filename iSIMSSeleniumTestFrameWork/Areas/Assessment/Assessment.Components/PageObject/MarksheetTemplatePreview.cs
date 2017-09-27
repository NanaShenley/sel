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
    public class MarksheetTemplatePreview
    {
        public MarksheetTemplatePreview()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }



        private static By ListOfthridlevelColumnHeaders = By.CssSelector("div[id='marksheetpreview'] td[class*='webix_last']");


        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        [FindsBy(How = How.CssSelector, Using = "button[data-createmarksheet-gradeset]")] //"button[data-ajax-url$='/Assessment/SIMS8GradesetSearchAssessmentGradeset/AddGradeset']")]
        private readonly IWebElement _chooseGradesetLink = null;

        /// <summary>
        /// Returns a New Page Object for the Marksheet Template Preview Page
        /// </summary>
        public MarksheetTemplatePreview NewMarksheetTemplatePreviewPageObject()
        {
            return new MarksheetTemplatePreview();
        }

        /// <summary>
        /// Returns the column headers for the mentioned column number
        /// </summary>
        public List<string> GetColumnHeaders(int ColumnNumber)
        {
            List<string> ListOfColumnHeaders = new List<string>();
            ColumnNumber = ColumnNumber - 1;
            ReadOnlyCollection<IWebElement> ColumnHeadersList = WebContext.WebDriver.FindElements(By.CssSelector("div[id='marksheetpreview'] td[column='" + ColumnNumber + "'] span.header-text"));
            foreach (IWebElement eachelement in ColumnHeadersList)
            {
                ListOfColumnHeaders.Add(eachelement.Text);
            }
            return ListOfColumnHeaders;
        }

        /// <summary>
        /// Gets the Count of the Column Present in the Preview
        /// </summary>
        public int GetColumnCount()
        {
            List<int> ColumnHeadersList = new List<int>();
            ReadOnlyCollection<IWebElement> ColumnList = WebContext.WebDriver.FindElements(ListOfthridlevelColumnHeaders);
            foreach (IWebElement element in ColumnList)
            {
                ColumnHeadersList.Add(int.Parse(element.GetAttribute("column")));
            }
            return ColumnHeadersList.Max() + 1;
        }

    }
}
