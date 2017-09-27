using System;
using DataExchange.POM.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.CRUD;
using TestSettings;
using WebDriverRunner.webdriver;
using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using System.Collections.Generic;
using System.Linq;

namespace DataExchange.POM.Components.CTF.Export
{
    public class CtfExportAddPupil : AutoRefreshSeleniumComponents
    {
        private const string CreateButtonToFind = "create_button";
        private const string ColumnNameYearGroup = "Year Group";
        private const string ColumnNameClass = "Class";
        private const string ColumnNamePreviousDestination = "Previous Destination";
        private const string CtfExportPupilGridName = "CTFExportLearners";
        
        [FindsBy(How = How.Name, Using = CtfExportPupilGridName)]
        private readonly IWebElement _ctfPupilGrid = null;
        
        public CtfExportAddPupil()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1000));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public WebixComponent<CtfPupilCell> CtfPupilGrid
        {
            get { return new WebixComponent<CtfPupilCell>(_ctfPupilGrid); }
        }
        private IWebElement CreateButton
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(CreateButtonToFind));
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(CreateButtonToFind));
            }
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            return CtfPupilGrid.RowCount > 0;
        }

        /// <summary>
        /// This method returns true if there are records in grid
        /// </summary>
        /// <returns></returns>
        public bool HasNewColumnsAdded()
        {//Year Group, Class, Previous Destination
            return CtfPupilGrid[0][ColumnNameYearGroup].webElement.IsElementExists() &&
                   CtfPupilGrid[0][ColumnNameClass].webElement.IsElementExists() &&
                   CtfPupilGrid[0][ColumnNamePreviousDestination].webElement.IsElementExists();
        }

        /// <summary>
        /// generates the ctf export file
        /// </summary>
        public void GenerateCtfExport()
        {
            CreateButton.ClickByJS();
            WaitTillDetailPanelIsLoaded();
            WaitUntilDisplayed(By.CssSelector(DataExchangeElements.CtfExport.SearchCtfDestinationButton));
        }

        public void WaitTillDetailPanelIsLoaded()
        {
            string jsPredicate = "return $(\"#" + DataExchangeElements.CtfExport.DETAIL_PANEL_FORM_ID + "\").is(':visible')";
            Console.WriteLine("Waiting for Detail panel to load");
            DataExchange.POM.Helper.Wait.WaitTillConditionIsMet(jsPredicate, 60);
        }

        public void PupilselectorAdd()
        {
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.AddPupilLink);
            WaitElementToBeClickable(DataExchangeElements.CtfExport.SearchPupilButton);
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.SearchPupilButton);
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.SearchRecordsToFindtext);
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.AddButton);
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.PupilSelectorOkButton);
            DataExchange.POM.Helper.Wait.WaitUntilDisplayed(DataExchangeElements.CommonElements.PupilGridCheckBox);
        }


        /// <summary>
        /// CTFPupilCell webix Cell
        /// </summary>
        public class CtfPupilCell : WebixCell
        {
            public CtfPupilCell() { }

            public CtfPupilCell(IWebElement webElement)
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
