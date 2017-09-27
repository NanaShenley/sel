using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;
using System.Collections.Generic;
using System.Linq;

namespace DataExchange.POM.Components.Census
{
    public class EarlyYearsPupilPremium
    {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Early Years']")]
            private IWebElement _earlyYearsProvision;

            [FindsBy(How = How.Name, Using = "EarlyYearsSection.EarlyYearsPupilPremiums")]
            private IWebElement _earlyYearsProvisionGrid;
    
        
             public EarlyYearsPupilPremium()
            {
                PageFactory.InitElements(WebContext.WebDriver, this);
            }

            public WebixComponent<OnRollCell> EarlyYearsProvisionGrid
            {
                get { return new WebixComponent<OnRollCell>(_earlyYearsProvisionGrid); }
            }

            public IWebElement EarlyYearsProvision
            {
                get
                {
                    return _earlyYearsProvision;
                }
            }

            /// <summary>
            /// Check if Section Exist
            /// </summary>
            /// <returns></returns>
            public bool IsSectionExist()
            {
                return EarlyYearsProvision.IsElementDisplayed();
            }
            /// <summary>
            /// Open On roll section
            /// </summary>
            public void OpenSection()
            {
                if (EarlyYearsProvision.IsElementDisplayed())
                {
                    EarlyYearsProvision.Click();
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

