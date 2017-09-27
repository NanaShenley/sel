using System;
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
    /// Projected Pupil Numbers Section
    /// </summary>
    public class ProjectedPupilNumbersSection
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Projected Pupil Numbers']")]
        private IWebElement _projectedPupilNumbersSection;

        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _projectedPupilNumbersGrid;

        public ProjectedPupilNumbersSection()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<ProjectedPupilNumberCell> projectedPupilNumbersGrid
        {
            get { return new WebixComponent<ProjectedPupilNumberCell>(_projectedPupilNumbersGrid); }
        }

        public IWebElement ProjectedPupilNumberSection
        {
            get
            {
                return _projectedPupilNumbersSection;
            }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return ProjectedPupilNumberSection.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenSection()
        {
            if (ProjectedPupilNumberSection.IsElementDisplayed())
            {
                ProjectedPupilNumberSection.Click();
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            return projectedPupilNumbersGrid.RowCount > 0;
        }

        /// <summary>
        /// On roll webix Cell
        /// </summary>
        public class ProjectedPupilNumberCell : WebixCell
        {
            public ProjectedPupilNumberCell() { }

            public ProjectedPupilNumberCell(IWebElement webElement)
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
