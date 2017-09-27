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
    /// Projected Nursery Section
    /// </summary>
    public class ProjectedNurserySection
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Projected Nursery/Reception Numbers']")]
        private IWebElement _projectedNurserySection;

        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _projectedNurseryGrid;

        public ProjectedNurserySection()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }


        //public override By ComponentIdentifier
        //{
        //    get
        //    {               
        //        throw new NotImplementedException();
        //    }
        //}

        public WebixComponent<ProjectedNurseryCell> ProjectedNurseryGrid
        {
            get { return new WebixComponent<ProjectedNurseryCell>(_projectedNurseryGrid); }
        }

        public IWebElement ProjectedNurseryPupilSection
        {
            get
            {
                return _projectedNurserySection;
            }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return ProjectedNurseryPupilSection.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenSection()
        {
            if (ProjectedNurseryPupilSection.IsElementDisplayed())
            {
                ProjectedNurseryPupilSection.Click();
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            return ProjectedNurseryGrid.RowCount > 0;
        }

        /// <summary>
        /// On roll webix Cell
        /// </summary>
        public class ProjectedNurseryCell : WebixCell
        {
            public ProjectedNurseryCell() { }

            public ProjectedNurseryCell(IWebElement webElement)
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
