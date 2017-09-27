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
    /// EarlyYears Provision Section
    /// </summary>
    public class EarlyYearsProvisionSection
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Early Years Provision']")]
        private IWebElement _earlyYearsProvisionSection;

        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _earlyYearsProvisionGrid;

        public EarlyYearsProvisionSection()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<EarlyYearsProvisionCell> EarlyYearsProvisionGrid
        {
            get { return new WebixComponent<EarlyYearsProvisionCell>(_earlyYearsProvisionGrid); }
        }

        public IWebElement EarlyYearsProvisionPupilSection
        {
            get
            {
                return _earlyYearsProvisionSection;
            }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return EarlyYearsProvisionPupilSection.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenSection()
        {
            if (EarlyYearsProvisionPupilSection.IsElementDisplayed())
            {
                EarlyYearsProvisionPupilSection.Click();
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            return EarlyYearsProvisionGrid.RowCount > 0;
        }

        /// <summary>
        /// On roll webix Cell
        /// </summary>
        public class EarlyYearsProvisionCell : WebixCell
        {
            public EarlyYearsProvisionCell() { }

            public EarlyYearsProvisionCell(IWebElement webElement)
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
