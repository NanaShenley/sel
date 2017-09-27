﻿using System;
using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;
using System.Collections.Generic;
using System.Linq;

namespace DataExchange.POM.Components.DENI
{
    /// <summary>
    /// Leaver Attendance Section
    /// </summary>
    public class LeaverAttendance
    {       
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Leavers Attendance']")]
        private IWebElement _leaverAttendanceSection;
       
        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _leaverAttendanceGrid;

        public LeaverAttendance()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
        
        public WebixComponent<LeaverAttendanceCell> LeaverAttendanceGrid
        {
            get { return new WebixComponent<LeaverAttendanceCell>(_leaverAttendanceGrid); }
        }

        public IWebElement LeaverAttendanceSection
        {
            get
            {
                return _leaverAttendanceSection;              
            }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return LeaverAttendanceSection.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenSection()
        {
            if (LeaverAttendanceSection.IsElementDisplayed())
            {
                LeaverAttendanceSection.Click();
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            return LeaverAttendanceGrid.RowCount > 0;
        }

        /// <summary>
        /// On roll webix Cell
        /// </summary>
        public class LeaverAttendanceCell : WebixCell
        {
            public LeaverAttendanceCell() { }

            public LeaverAttendanceCell(IWebElement webElement)
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
