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
    public class CensusDaySnapshot
    {
        private const string _noRecordExistMessageForLunchTaken = "No School Lunch Taken information applicable to the Return";
        private const string _noRecordExistMessageForClasses = "No Classes and Pupil Reconciliation Result information applicable to the Return";

        [FindsBy(How = How.Id, Using = "CensusDaySnapshotTab-label")]
        private IWebElement _censusDaySnapshotTab;
                
        [FindsBy(How = How.Id, Using = "CensusDaySnapshotSection_Classess")]
        private IWebElement _classesGrid;

        [FindsBy(How = How.Id, Using = "CensusDaySnapshotSection_SchoolLunchTakens")]
        private IWebElement _schoolLunchTakenGrid;

        [FindsBy(How = How.ClassName, Using = "message-control-text")]
        private IWebElement _noRecordMessage;

        public CensusDaySnapshot()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
        public IWebElement CensusDaySnapshotTab
        {
            get
            {
                Wait.WaitTillAllAjaxCallsComplete();
                return _censusDaySnapshotTab;
            }
        }
        public WebixComponent<OnRollCell> ClassesGrid
        {
            get
            {                
                return new WebixComponent<OnRollCell>(_classesGrid);
            }
        }

        public WebixComponent<OnRollCell> SchoolLunchTakenGrid
        {
            get
            {
                Wait.WaitTillAllAjaxCallsComplete();
                return new WebixComponent<OnRollCell>(_schoolLunchTakenGrid);
            }
        }

        public IWebElement NoRecordExist
        {
            get
            {
                Wait.WaitTillAllAjaxCallsComplete();
                return _noRecordMessage;
            }
        }      

        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenCensusDataChecksTab()
        {
            if (CensusDaySnapshotTab.IsElementDisplayed())
            {
                CensusDaySnapshotTab.SendKeys(Keys.Enter);
            }
        }

        public bool IsTabExist()
        {
            return CensusDaySnapshotTab.IsElementDisplayed();
        }

        public bool IsClassesGridExist()
        {
            Wait.WaitTillAllAjaxCallsComplete();
            return _classesGrid.IsElementDisplayed();
        }

        public bool IsSchoolLunchTakenGridExist()
        {
            Wait.WaitTillAllAjaxCallsComplete();
            return _schoolLunchTakenGrid.IsElementDisplayed();
        }

        public List<string> GetAllNoRecordExistMsg()
        {
            Wait.WaitTillAllAjaxCallsComplete();
            List<string> lstNoRecordExistMessages =new List<string>();

            IList<IWebElement> lstNoRecords = WebContext.WebDriver.FindElements(By.ClassName("message-control-text")).ToList<IWebElement>();
            foreach(IWebElement element in lstNoRecords)
            {
                if(element!=null)
                {
                    lstNoRecordExistMessages.Add(element.Text.Trim());
                }
            }
            return lstNoRecordExistMessages;
        }

        public bool ClassesGridHasRecords()
        {
            bool isValid = false;
            if (IsClassesGridExist())
            {
                isValid = ClassesGrid.RowCount > 0;
            }
            else if (GetAllNoRecordExistMsg().Contains(_noRecordExistMessageForClasses))
            {
                isValid = true;
            }
            return isValid;            
        }

        public bool SchoolLunchTakenGridHasRecords()
        {
            bool isValid = false;
            if(IsSchoolLunchTakenGridExist())
            {
                isValid = SchoolLunchTakenGrid.RowCount > 0;
            }
            else if(GetAllNoRecordExistMsg().Contains(_noRecordExistMessageForLunchTaken))
            {
                isValid = true;
            }
            return isValid;
        }

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
