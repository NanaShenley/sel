using System;
using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace DataExchange.POM.Components.Census
{
    public class CensusDataChecks
    {
        [FindsBy(How = How.Id, Using = "CensusDataCheckTab-label")]
        private IWebElement _censusDataChecksTab;

        [FindsBy(How = How.Id, Using = "GroupByIssueTab-label")]
        private IWebElement _showIssueGroupedbyTypeCheckBox;
               
        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _censusDataChecksGrid;

        [FindsBy(How = How.ClassName, Using = "badge-error")]
        private IWebElement _issuesCount;

        [FindsBy(How = How.ClassName, Using = "badge-warning")]
        private IWebElement _warningsCount;

        public CensusDataChecks()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<OnRollCell> CensusDataChecksGrid
        {
            get
            {                
                return new WebixComponent<OnRollCell>(_censusDataChecksGrid);
            }
        }

        public IWebElement CensusDataChecksTab
        {
            get
            {
                Wait.WaitTillAllAjaxCallsComplete();
                return _censusDataChecksTab;
            }
        }
        public IWebElement ShowIssueGroupedbyTypeCheckBox
        {
            get
            {
                Wait.WaitTillAllAjaxCallsComplete();
                return _showIssueGroupedbyTypeCheckBox;
            }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsTabExist()
        {
            return CensusDataChecksTab.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenCensusDataChecksTab()
        {
            if (CensusDataChecksTab.IsElementDisplayed())
            {
                CensusDataChecksTab.SendKeys(Keys.Enter);
            }
        }

        public void ClickShowIssueGroupedbyTypeCheckBox()
        {
            if (ShowIssueGroupedbyTypeCheckBox.IsElementDisplayed())
            {
                ShowIssueGroupedbyTypeCheckBox.SendKeys(Keys.Enter);
            }
        }        

        private int GetCount(IWebElement element,string partName)
        {
            string elementText = string.Empty;
            int charLength = 0, textCount = 0;

            if (IsTabExist())
            {
                if (element.IsElementDisplayed())
                {
                    elementText = element.Text;
                    charLength = elementText.Length - partName.Length;

                    if (charLength > 0)
                    {
                        elementText = elementText.Substring(0, charLength);
                    }
                }
                else
                {
                    return 0;
                }
                
            }
            int.TryParse(elementText, out textCount);
            return textCount;
        }         

        public bool IsCensusDataChecksCheckBoxExist()
        {
            Wait.WaitTillAllAjaxCallsComplete();
            return ShowIssueGroupedbyTypeCheckBox.IsElementDisplayed();
        }       

        public bool IsGridExist()
        {
            Wait.WaitTillAllAjaxCallsComplete();         
            return _censusDataChecksGrid.IsElementDisplayed();
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            bool isValid = false;
            if (GetCount(_issuesCount, " Issues") > 0 || GetCount(_warningsCount, " Queries") > 0)
            {
                if (IsGridExist() && CensusDataChecksGrid.RowCount > 0){
                    isValid = true;
                }
            }
            else {
                isValid = true;
            }

            return isValid;
        }

        /// <summary>
        /// On roll webix Cell
        /// </summary>
        public class OnRollCell : WebixCell
        {
            public OnRollCell() { }

            public OnRollCell(IWebElement webElement)
                : base(webElement)
            { }


            public string CellText
            {
                set
                {
                    if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
                    {
                        webElement.ClickByJS();
                    }
                    IWebElement input = SeleniumHelper.FindElement(By.CssSelector(".webix_dt_editor input"));
                    input.SetText(value);
                    IList<IWebElement> dropdown = SeleniumHelper.FindElements(By.CssSelector(".webix_list_item")).ToList();
                    foreach (var item in dropdown)
                    {
                        if (item.Text.Equals(value))
                        {
                            item.ClickByJS();
                            break;
                        }
                    }
                }
                get
                {
                    if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
                    {
                        webElement.ClickByJS();
                    }
                    IWebElement input = SeleniumHelper.FindElement(By.CssSelector(".webix_dt_editor input"));
                    return input.GetValue();
                }
            }
        }
    }
}
