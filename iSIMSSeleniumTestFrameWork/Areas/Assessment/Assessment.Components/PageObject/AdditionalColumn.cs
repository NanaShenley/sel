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
    public class AdditionalColumn
    {
        public AdditionalColumn()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        //Additional Column Panel = ACP

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='additional'] div.slider-header-title")]
        private IWebElement ACPHeader;

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='additional'] button[data-automation-id='done-additionalColumn']")]
        private IWebElement ACPDoneButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='additional'] [additional-column-back]")]
        private IWebElement ACPBackButton;

        //[FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='additional'] [data-automation-id='additionalcolumnclosebutton']")]
        //private IWebElement ACPCloseButton;

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='search_results_counter'] > strong")]
        private IWebElement AdditionalColumnCount;

        [FindsBy(How = How.CssSelector, Using = "[view_id='fixedcolumntreenode']")]
        public IWebElement _additionalColumnsScreenRootElement;


        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        //Additional Column List
        private static By AdditionalColumnList = By.CssSelector("[class='webix_tree_item']");
        private static By AdditionalColumnCheckbox = By.CssSelector("input[type ='checkbox']");

        /// <summary>
        /// Clicks the Done Button and moves on to the Marksheet Builder page
        /// </summary>
        public MarksheetBuilder ClickDoneButton()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(ACPDoneButton, "Done"));
            ACPDoneButton.Click();
            return new MarksheetBuilder();
        }

        /// <summary>
        /// Clicks the Back Button and moves on to the Marksheet Builder page
        /// </summary>
        public MarksheetBuilder ClickBackButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(ACPBackButton));
            ACPBackButton.Click();
            return new MarksheetBuilder();
        }

      
        ///// <summary>
        ///// Clicks the Close Button and moves on to the Marksheet Builder page
        ///// </summary>
        //public MarksheetBuilder ClickCloseButton()
        //{
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(ACPCloseButton));
        //    ACPCloseButton.Click();
        //    return new MarksheetBuilder();
        //}

        /// <summary>
        /// Selects Additional Column\s based on the Column Count
        /// </summary>
        public void SelectNoOfAdditionalColumn(int NoOfAdditionalCol)
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(ACPDoneButton, "Done"));
            ReadOnlyCollection<IWebElement> AdditionalColNameList = WebContext.WebDriver.FindElements(AdditionalColumnList);

            int j = 0;

            foreach (IWebElement AdditionalColElement in AdditionalColNameList)
            {
                if (!AdditionalColumnParentHeaderList.Contains(AdditionalColElement.Text))
                {
                    if (NoOfAdditionalCol != 0)
                    {
                        AdditionalColElement.FindChild(AdditionalColumnCheckbox).Click();
                        j++;
                    }
                    if (j == NoOfAdditionalCol)
                        break;
                }
            }

        }

        private List<string> AdditionalColumnParentHeaderList = new List<string>() {"Personal Details", "Registration Details","Ethnic/Cultural Details","Attendance"};
        /// <summary>
        /// Selects a specific Additional Column
        /// </summary>
        public void SelectAddColByName(string AddColName)
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(ACPDoneButton, "Done"));
            IWebElement element = WebContext.WebDriver.FindElements(AdditionalColumnList)
                .FirstOrDefault(ele => ele.Text == AddColName);

            element.FindChild(AdditionalColumnCheckbox).Click();
        }

        /// <summary>
        /// Returns all the Additional Columns as a part of List
        /// </summary>
        public List<string> GetAdditionalColumnList()
        {
            List<string> ListOfAdditionalColumns = new List<string>();
            ReadOnlyCollection<IWebElement> AdditionalColNameList = WebContext.WebDriver.FindElements(AdditionalColumnList);
            foreach (IWebElement eachelement in AdditionalColNameList)
            {
                ListOfAdditionalColumns.Add(eachelement.Text);
            }
            return ListOfAdditionalColumns;
        }

        /// <summary>
        /// Returns all the Selected Additional Columns as a part of List
        /// </summary>
        public List<string> GetSelectedAdditionalColumnList()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(AdditionalColumnList));
            List<string> ListOfSelectedAdditionalColumns = new List<string>();
            ReadOnlyCollection<IWebElement> AdditionalColNameList = WebContext.WebDriver.FindElements(AdditionalColumnList);
            foreach (IWebElement eachelement in AdditionalColNameList)
            {
                if (eachelement.FindChild(AdditionalColumnCheckbox).Selected == true)
                    ListOfSelectedAdditionalColumns.Add(eachelement.Text);
            }
            return ListOfSelectedAdditionalColumns;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AdditionalColumnsAreUnSelected()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(AdditionalColumnList));
            ReadOnlyCollection<IWebElement> additionalColumnElements = WebContext.WebDriver.FindElements(AdditionalColumnList);
            foreach (IWebElement element in additionalColumnElements)
            {
                if (element.Text != "")
                {
                    bool CheckSelction = element.FindChild(AdditionalColumnCheckbox).Selected;
                    Assert.AreEqual(CheckSelction, false);
                }
            }
        }
    }
}
