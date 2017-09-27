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
    public class ShowAllPupils
    {
        private const string _noRecordExistMessage = "No On Roll Pupil Result information applicable to the Return";
        [FindsBy(How = How.Id, Using = "PupilInformationTab-label")]
        private IWebElement _pupilInformationTab;

        [FindsBy(How = How.Id, Using = "tab6-label")]
        private IWebElement _showAllPupilCheckBox;

        [FindsBy(How = How.Id, Using = "OnRollPupilSection_OnRollPupils")]
        private IWebElement _showAllPupilGrid;

        [FindsBy(How = How.ClassName, Using = "message-control-text")]
        private IWebElement _noRecordMessage;

        public ShowAllPupils()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<OnRollCell> ShowAllPupilGrid
        {
            get
            {
                Wait.WaitTillAllAjaxCallsComplete();
                return new WebixComponent<OnRollCell>(_showAllPupilGrid);
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

        public IWebElement ShowAllPupilCheckBox
        {
            get
            {
                Wait.WaitTillAllAjaxCallsComplete();
                return _showAllPupilCheckBox;
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

        public bool IsShowAllPupilCheckBoxExist()
        {
            return _showAllPupilCheckBox.IsElementDisplayed();
        }

        public void ClickShowAllPupilCheckBox()
        {
            if (ShowAllPupilCheckBox.IsElementDisplayed())
            {
                ShowAllPupilCheckBox.SendKeys(Keys.Enter);
            }
        }

        public bool IsGridExist()
        {
            Wait.WaitTillAllAjaxCallsComplete();
            return _showAllPupilGrid.IsElementDisplayed();
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
                isValid = ShowAllPupilGrid.RowCount > 0;
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
