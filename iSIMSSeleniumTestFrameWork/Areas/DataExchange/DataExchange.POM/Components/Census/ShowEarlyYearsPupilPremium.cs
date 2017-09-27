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
    public class ShowEarlyYearsPupilPremium
    {
        private const string _noRecordExistMessage = "No Early Years Pupil Premium Result information applicable to the Return";

        [FindsBy(How = How.Id, Using = "PupilInformationTab-label")]
        private IWebElement _pupilInformationTab;

        [FindsBy(How = How.Id, Using = "tab8-label")]
        private IWebElement _ShowEarlyYearsPupilPremiumCheckBox;

        [FindsBy(How = How.Id, Using = "EarlyYearPupilPremiumSection_EarlyYearsPupilPremiums")]
        private IWebElement _showEarlyYearsPupilPremiumGrid;

        [FindsBy(How = How.ClassName, Using = "message-control-text")]
        private IWebElement _noRecordMessage;

        public ShowEarlyYearsPupilPremium()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<OnRollCell> ShowEarlyYearsPupilPremiumGrid
        {
            get
            {               
                return new WebixComponent<OnRollCell>(_showEarlyYearsPupilPremiumGrid);
            }
        }

        public IWebElement PupilInformationTab
        {
            get
            {
                Wait.WaitTillAllAjaxCallsComplete();
                return _pupilInformationTab;
            }
        }
        public IWebElement ShowEarlyYearsPupilPremiumCheckBox
        {
            get
            {
                Wait.WaitTillAllAjaxCallsComplete();
                return _ShowEarlyYearsPupilPremiumCheckBox;
            }
        }        

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsTabExist()
        {
            return PupilInformationTab.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenPupilInformationTab()
        {
            if (PupilInformationTab.IsElementDisplayed())
            {
                PupilInformationTab.SendKeys(Keys.Enter);
            }
        }       

        public bool IsShowEarlyYearsPupilPremiumCheckBoxExist()
        {
            return _ShowEarlyYearsPupilPremiumCheckBox.IsElementDisplayed();
        }

        public void ClickShowEarlyYearsPupilPremiumCheckBox()
        {
            if (ShowEarlyYearsPupilPremiumCheckBox.IsElementDisplayed())
            {
                ShowEarlyYearsPupilPremiumCheckBox.SendKeys(Keys.Enter);
            }
        }

        public bool IsGridExist()
        {
            Wait.WaitTillAllAjaxCallsComplete();
            return _showEarlyYearsPupilPremiumGrid.IsElementDisplayed();
        }

        public bool IsNoRecordMessageLabelExist()
        {
            return _noRecordMessage.IsElementDisplayed();
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            bool isValid = false;
            if (IsGridExist())
            {
                isValid = ShowEarlyYearsPupilPremiumGrid.RowCount > 0;
            }
            else if (IsNoRecordMessageLabelExist())
            {
                if (_noRecordMessage.Text.Trim().Equals(_noRecordExistMessage))
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
