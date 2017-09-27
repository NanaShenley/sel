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
    /// PupilsNotResidentInNI Section
    /// </summary>
    public class PupilsNotResidentInNISection
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Pupils Not Resident in Northern Ireland']")]
        private IWebElement _pupilsNotResidentInNISection;

        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _pupilsNotResidentInNIGrid;

        public PupilsNotResidentInNISection()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<PupilsNotResidentInNICell> PupilsNotResidentInNIGrid
        {
            get { return new WebixComponent<PupilsNotResidentInNICell>(_pupilsNotResidentInNIGrid); }
        }

        public IWebElement PupilsNotResidentInNIPupilSection
        {
            get
            {
                return _pupilsNotResidentInNISection;
            }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return PupilsNotResidentInNIPupilSection.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenSection()
        {
            if (PupilsNotResidentInNIPupilSection.IsElementDisplayed())
            {
                PupilsNotResidentInNIPupilSection.Click();
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            return PupilsNotResidentInNIGrid.RowCount > 0;
        }

        /// <summary>
        /// On roll webix Cell
        /// </summary>
        public class PupilsNotResidentInNICell : WebixCell
        {
            public PupilsNotResidentInNICell() { }

            public PupilsNotResidentInNICell(IWebElement webElement)
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