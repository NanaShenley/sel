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
    public class POSDataMaintainanceScreen : BaseSeleniumComponents
    {
        public POSDataMaintainanceScreen()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='summary_button']")]
        public IWebElement SummaryButton;

        [FindsBy(How = How.CssSelector, Using = "button[class='btn-show-panel']")]
        public IWebElement SearchFilterButton;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        private static By POSExpectationAchievedColumnHeader = By.CssSelector("div[id='cOverallPos']");
        private static By SchoolExpectationAchievedColumnHeader = By.CssSelector("div[id='cOverallSchool']");
        private static By OverallHeader = By.CssSelector("div[id='cOverall']");
        private static By StrengthsHeader = By.CssSelector("div[id='cStrengths']");
        private static By NextStepsHeader = By.CssSelector("div[id='cNextSteps']");
        private static By ColumnNameElement = By.CssSelector("td[column] div.column-wrapper.header-text");

        /// <summary>
        /// Clicks on the Summary Button
        /// </summary>
        public POSDataMaintainanceScreen ClickSummaryLink()
        {
            WaitUntilDisplayed(By.CssSelector("button[data-automation-id='summary_button']"));            
            SummaryButton.Click(); 
            return new POSDataMaintainanceScreen();
        }

        /// <summary>
        /// Clicks on the Summary Button
        /// </summary>
        public POSSummaryPannel SearchFilterButtonClick()
        {
            SearchFilterButton.Click();
            return new POSSummaryPannel();
        }

        /// <summary>
        /// Get all column names
        /// </summary>
        public List<string> GetAllMarksheetColumnNames()
        {
            ReadOnlyCollection<IWebElement> ColumnNameElementList = WebContext.WebDriver.FindElements(ColumnNameElement);
            List<string> ColumnName = new List<string>();
            int count = 0;
            foreach (IWebElement eachelement in ColumnNameElementList)
            {
                if (count > 0)
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(eachelement));
                    eachelement.Click();
                    Actions action = new Actions(WebContext.WebDriver);
                    action.SendKeys(Keys.Right).Perform();
                    if (eachelement.Displayed == true)
                    {
                        if (eachelement.Text.IndexOf(Environment.NewLine) > 0)
                        {
                            ColumnName.Add(eachelement.Text.Substring(0, eachelement.Text.IndexOf(Environment.NewLine)));
                        }
                        else
                        {
                            ColumnName.Add(eachelement.Text);
                        }

                    }
                }
                else if (eachelement.Displayed == true)
                {
                    ColumnName.Add(eachelement.Text);
                    count++;
                }
            }
            return ColumnName;
        }


        /// <summary>
        /// Get all column names
        /// </summary>
        public List<string> SelectMarksheetColumnName(String ColumnHeadertext)
        {
            ReadOnlyCollection<IWebElement> ColumnNameElementList = WebContext.WebDriver.FindElements(ColumnNameElement);
            
            List<string> ColumnName = new List<string>();
            int count = 0;
            Actions action = new Actions(WebContext.WebDriver);
            foreach (IWebElement eachelement in ColumnNameElementList)
            {
                if (count > 0)
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(eachelement));
                    eachelement.Click();
                    if (eachelement.Displayed == true)
                    {
                 //       if (eachelement.Text.IndexOf(Environment.NewLine) > 0)
                  //      {
                       //     ColumnName.Add(eachelement.Text.Substring(0, eachelement.Text.IndexOf(Environment.NewLine)));
                            if (eachelement.Text.Contains(ColumnHeadertext))
                                break;
                            action.SendKeys(Keys.Right).Perform();
                        action.SendKeys(Keys.ArrowUp).Perform();
                        //    }
                        //else
                        //{
                        //    ColumnName.Add(eachelement.Text);
                        //}

                    }
                }
                else if (eachelement.Displayed == true)
                {
                    ColumnName.Add(eachelement.Text);
                    count++;
                }

            }
            action.SendKeys(Keys.ArrowUp).Perform();
            return ColumnName;
        }

        /// <summary>
        /// Verifies if the column is present based on its Column Name
        /// </summary>
        public bool VerifyColumnPresent(string ColumnName)
        {
            bool iscolumnpresent = true;
            switch (ColumnName)
            {
                case "% of PoS Expectations Achieved":
                    iscolumnpresent = Verify(POSExpectationAchievedColumnHeader);
                    break;

                case "% of School Expectations Achieved":
                    iscolumnpresent = Verify(SchoolExpectationAchievedColumnHeader);
                    break;

                case "En Read Comp Overall":
                    iscolumnpresent = Verify(OverallHeader);
                    break;

                case "En Read Comp Strengths":
                    iscolumnpresent = Verify(StrengthsHeader);
                    break;

                case "En Read Comp NextSteps":
                    iscolumnpresent = Verify(NextStepsHeader);
                    break;

                case "En Read Overall":
                    iscolumnpresent = Verify(OverallHeader);
                    break;

                case "En Read Strengths":
                    iscolumnpresent = Verify(StrengthsHeader);
                    break;

                case "En Read NextSteps":
                    iscolumnpresent = Verify(NextStepsHeader);
                    break;

            }
            return iscolumnpresent;
        }

        /// <summary>
        /// Verifies if the % of POS Expectations column is present on the data maintainance screen
        /// </summary>
        private bool Verify(By ColumnWebelement)
        {
            try
            {
                waiter.Until(ExpectedConditions.ElementIsVisible(ColumnWebelement));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}