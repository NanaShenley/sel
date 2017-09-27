
using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;
using System.Collections.Generic;
using System.Linq;

namespace DataExchange.POM.Components.Census
{
    public class SchoolLunchTakenSection
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_School Lunch Taken']")] //?
        private IWebElement _schoolLunchTaken;

        [FindsBy(How = How.CssSelector, Using = "#SchoolLunchTakenSection_SchoolLunchTakens")] //?
        private IWebElement _schoolLunchTakenGrid;

        public SchoolLunchTakenSection()
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

        public WebixComponent<SchoolLunchTakenCell> SchoolLunchTakenGrid
        {
            get { return new WebixComponent<SchoolLunchTakenCell>(_schoolLunchTakenGrid); }
        }

        public IWebElement SchoolLunchTaken
        {
            get { return _schoolLunchTaken; }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return SchoolLunchTaken.IsElementDisplayed();
        }

        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenSection()
        {
            if (SchoolLunchTaken.IsElementDisplayed())
            {
                SchoolLunchTaken.Click();
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            return SchoolLunchTakenGrid.RowCount > 0;
        }

        /// <summary>
        /// Open On roll section
        /// </summary>


        /// <summary>
        /// On roll webix Cell
        /// </summary>
        public class SchoolLunchTakenCell : WebixCell
        {
            public SchoolLunchTakenCell() { }

            public SchoolLunchTakenCell(IWebElement webElement)
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




