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
    /// <summary>
    /// OnRoll Pupil Section
    /// </summary>
    public class OnRollPupilSection
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_On Roll Pupil']")]
        private IWebElement _onrollPupilSection;

        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _onrollPupilGrid;

        public OnRollPupilSection()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<OnRollCell> OnRollPupilGrid
        {
            get { return new WebixComponent<OnRollCell>(_onrollPupilGrid); }
        }

        public IWebElement OnrollPupilSection
        {
            get
            {
                return _onrollPupilSection;
            }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return OnrollPupilSection.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenSection()
        {
            if (OnrollPupilSection.IsElementDisplayed())
            {
                OnrollPupilSection.Click();
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            return OnRollPupilGrid.RowCount > 0;
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
