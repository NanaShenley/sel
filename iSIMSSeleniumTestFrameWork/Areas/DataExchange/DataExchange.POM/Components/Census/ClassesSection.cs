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
    public class ClassesSection
    {
      
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Classes Section']")]
        private IWebElement _classesSection;

        [FindsBy(How = How.Name, Using = "ClassesSection.Classess")]
        private IWebElement _classesGrid;
              
        [FindsBy(How = How.Name, Using = "ClassesSection.OnRollPupilsInClass")]
        private IWebElement _onRollPupilsInClass;

        [FindsBy(How = How.Name, Using = "ClassesSection.TotalPupils")]
        private IWebElement _totalPupils;

        [FindsBy(How = How.Name, Using = "ClassesSection.PupilsOnRoll")]
        private IWebElement _pupilsOnRoll;       
            

        public ClassesSection()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<OnRollCell> ClassesGrid
        {
            get { return new WebixComponent<OnRollCell>(_classesGrid); }
        }

        public IWebElement Classes
        {
            get
            {
                return _classesSection;
            }
        }

        public IWebElement OnRollPupilsInClass
        {
            get
            {
                return _onRollPupilsInClass;
            }
        }

        public IWebElement TotalPupils
        {
            get
            {
                return _totalPupils;
            }
        }

        public IWebElement PupilsOnRoll
        {
            get
            {
                return _pupilsOnRoll;
            }
        }
        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return Classes.IsElementDisplayed();
        }
        /// <summary>
        /// Open On roll section
        /// </summary>
        public void OpenSection()
        {
            if (Classes.IsElementDisplayed())
            {
                Classes.Click();
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            if (!(string.IsNullOrEmpty(OnRollPupilsInClass.GetValue())) &&
                !(string.IsNullOrEmpty(TotalPupils.GetValue())) &&
                !(string.IsNullOrEmpty(PupilsOnRoll.GetValue())) && ClassesGrid.RowCount > 0)
                return true;
            else
                return false;
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
