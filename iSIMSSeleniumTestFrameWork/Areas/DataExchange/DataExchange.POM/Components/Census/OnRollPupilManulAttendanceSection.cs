using System;
using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;
using System.Collections.Generic;
using System.Linq;


namespace DataExchange.POM.Components.Census
{
    public class OnRollPupilManulAttendanceSection
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_On Roll Pupil Attendance']")]
        private IWebElement _onRollPupilManualAttendance;

        [FindsBy(How = How.Name, Using = "OnRollPupilAttendanceSection.OnRollPupilAttendances")]
        private IWebElement _onRollPupilManulAttendanceGrid;

        public OnRollPupilManulAttendanceSection()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<OnRollCell> OnRollPupilAttendanceBasicDetailsGrid
        {
            get { return new WebixComponent<OnRollCell>(_onRollPupilManulAttendanceGrid); }
        }

        public IWebElement OnRollPupilManualAttendance
        {
            get
            {
                return _onRollPupilManualAttendance;
            }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return OnRollPupilManualAttendance.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenSection()
        {
            if (OnRollPupilManualAttendance.IsElementDisplayed())
            {
                OnRollPupilManualAttendance.Click();
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            return OnRollPupilAttendanceBasicDetailsGrid.RowCount > 0;
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

