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
    /// Onroll Pupil Attendance Section
    /// </summary>
    public class OnrollPupilAttendanceSection
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_On Roll Pupil Attendance']")]
        private IWebElement _onrollPupilAttendanceSection;

        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _onrollPupilAttendanceGrid;

        public OnrollPupilAttendanceSection()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<OnrollPupilAttendanceCell> OnrollPupilAttendanceGrid
        {
            get { return new WebixComponent<OnrollPupilAttendanceCell>(_onrollPupilAttendanceGrid); }
        }

        public IWebElement OnrollPupilAttendancePupilSection
        {
            get
            {
                return _onrollPupilAttendanceSection;
            }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return OnrollPupilAttendancePupilSection.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenSection()
        {
            if (OnrollPupilAttendancePupilSection.IsElementDisplayed())
            {
                OnrollPupilAttendancePupilSection.Click();
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            return OnrollPupilAttendanceGrid.RowCount > 0;
        }

        /// <summary>
        /// On roll webix Cell
        /// </summary>
        public class OnrollPupilAttendanceCell : WebixCell
        {
            public OnrollPupilAttendanceCell() { }

            public OnrollPupilAttendanceCell(IWebElement webElement)
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
