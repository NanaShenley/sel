using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace Assessment.Components.PageObject
{
    public class AssessmentPeriodLookupDataMaintainanceScreen
    {
        public AssessmentPeriodLookupDataMaintainanceScreen()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='create_button'][maintenance-container='AssessmentPeriods']")]
        private IWebElement CreateButton;

        [FindsBy(How = How.Id, Using = "AssessmentPeriodSave")]
        private IWebElement SaveButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success'] strong.inline-alert-title")]
        private IWebElement SaveSucess;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='reset_reference_dates_to_system_defaults_button']")]
        private IWebElement ResetReferenceDatesButton;
        [FindsBy(How = How.CssSelector, Using = "input[assessmentfrequencyname='Annual']")]
        private IWebElement SelectAnnualFrequency;

        [FindsBy(How = How.CssSelector, Using = "input[assessmentfrequencyname='Annual']")]
        private IWebElement Annual;

        By EditorCell = By.CssSelector("div[class='webix_dt_editor'] input[type='text']");

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        //private static By AssessmentPeriodList = By.CssSelector("div[data-grid-id='AssessmentPeriods'] tr[data-row-name='AssessmentPeriods']");
        //private static By DropdownListElements = By.CssSelector("div[id$='select2-result-label-']");

        private static By AssessmentPeriodName = By.CssSelector("div[name='AssessmentPeriods'] div[column='1'] div.webix_cell");
        private static By NCYear = By.CssSelector("div[name='AssessmentPeriods'] div[column='2'] div.webix_cell");
        private static By period = By.CssSelector("div[name='AssessmentPeriods'] div[column='3'] div.webix_cell");
        private static By ReferenceDateDay = By.CssSelector("div[name='AssessmentPeriods'] div[column='4'] div.webix_cell");
        private static By ReferenceDateMonth = By.CssSelector("div[name='AssessmentPeriods'] div[column='5'] div.webix_cell");
        private static By ResourceProvider = By.CssSelector("div[name='AssessmentPeriods'] div[column='6'] div.webix_cell");

        /// <summary>
        /// Get all the details based on the Row Number
        /// </summary>
        public List<string> GetAssessmentPeriodDetails(int RowNumber)
        {
            List<string> AssessmentPeriodDetailsList = new List<string>();
            for (int i = 2; i < 8; i++)
            {
                AssessmentPeriodDetailsList.Add(GetCellValue(i, RowNumber));
                i++;
            }
            return AssessmentPeriodDetailsList;
        }

        /// <summary>
        /// Get all the details based on the Row Number
        /// </summary>
        public AssessmentPeriodLookupDataMaintainanceScreen SetAssessmentPeriodDetails(int rowNumber, string APName, string ncYear)
        {
            //SetAssessmentPeriodName(RowNumber, APName);
            return SetCellValue(2, rowNumber, ncYear);
        }

        /// <summary>
        /// Click Create Button
        /// </summary>
        public AssessmentPeriodLookupDataMaintainanceScreen ClickCreateButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CreateButton)).Click();
            Thread.Sleep(2000);
            //while (true)
            //{
            //    if (CreateButton.GetAttribute("disabled") != "true")
            //        break;
            //}
            return new AssessmentPeriodLookupDataMaintainanceScreen();
        }

        /// <summary>
        /// Click Save Button
        /// </summary>
        public AssessmentPeriodLookupDataMaintainanceScreen ClickSaveButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SaveButton)).Click();
            //Thread.Sleep(2000);
            //waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSucess, "AssessmentPeriod record saved"));
            Thread.Sleep(4000);
            //while (true)
            //{
            //    if (SaveButton.GetAttribute("disabled") != "true")
            //        break;
            //}
            return new AssessmentPeriodLookupDataMaintainanceScreen();
        }

        /// <summary>
        /// Click Reset Reference Dates to System Default Button
        /// </summary>
        public AssessmentPeriodLookupDataMaintainanceScreen ClickResetReferenceDateButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(ResetReferenceDatesButton)).Click();
            Thread.Sleep(6000);
            while (true)
            {
                if (ResetReferenceDatesButton.GetAttribute("disabled") != "disabled")
                    break;
            }
            return new AssessmentPeriodLookupDataMaintainanceScreen();
        }

        /// <summary>
        /// Gets the Row Number of the Last Row or Newly Added Row
        /// </summary>
        public int GetLastRowNumber()
        {
            ReadOnlyCollection<IWebElement> APElementList = WebelementsList(NCYear);
            return APElementList.Count;
        }

        /// <summary>
        /// Returs a Webelement list for the specifed column
        /// </summary>
        public ReadOnlyCollection<IWebElement> WebelementsList(By ColumnName)
        {
            waiter.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(ColumnName));
            return WebContext.WebDriver.FindElements(ColumnName);
        }

        /// <summary>
        /// Get All the values for a particular column
        /// </summary>
        public List<string> GetAllValuesForAColumn(int columnnno)
        {
            List<string> ListofValues = new List<string>();
            switch (columnnno)
            {
                case 2:
                    ListofValues = GenerateAllValuesList(AssessmentPeriodName);
                    break;

                case 3:
                    ListofValues = GenerateAllValuesList(NCYear);
                    break;

                case 4:
                    ListofValues = GenerateAllValuesList(period);
                    break;

                case 5:
                    ListofValues = GenerateAllValuesList(ReferenceDateDay);
                    break;

                case 6:
                    ListofValues = GenerateAllValuesList(ReferenceDateMonth);
                    break;

                case 7:
                    ListofValues = GenerateAllValuesList(ResourceProvider);
                    break;
            }
            return ListofValues;
        }

        /// <summary>
        /// Get a value for a particular column based on the Row Number
        /// </summary>
        public string GetCellValue(int columnnno, int RowNumber)
        {
            Thread.Sleep(4000);
            string SingleValue = "";
            switch (columnnno)
            {
                case 2:
                    SingleValue = GetAValue(AssessmentPeriodName, RowNumber);
                    break;

                case 3:
                    SingleValue = GetAValue(NCYear, RowNumber);
                    break;

                case 4:
                    SingleValue = GetAValue(period, RowNumber);
                    break;

                case 5:
                    SingleValue = GetAValue(ReferenceDateDay, RowNumber);
                    break;

                case 6:
                    SingleValue = GetAValue(ReferenceDateMonth, RowNumber);
                    break;

                case 7:
                    SingleValue = GetAValue(ResourceProvider, RowNumber);
                    break;
            }
            return SingleValue;
        }

        /// <summary>
        /// Set Assessment Period Name based on the Row Number
        /// </summary>
        public AssessmentPeriodLookupDataMaintainanceScreen SetCellValue(int columnnno, int RowNumber, string cellvalue)
        {
            AssessmentPeriodLookupDataMaintainanceScreen assessmentperiodlookupdatamaintainancescreen = new AssessmentPeriodLookupDataMaintainanceScreen();
            switch (columnnno)
            {
                case 2:
                    assessmentperiodlookupdatamaintainancescreen = SetAValue(AssessmentPeriodName, RowNumber, cellvalue);
                    break;

                case 3:
                    assessmentperiodlookupdatamaintainancescreen = SetAValue(NCYear, RowNumber, cellvalue);
                    break;

                case 4:
                    assessmentperiodlookupdatamaintainancescreen = SetAValue(period, RowNumber, cellvalue);
                    break;

                case 5:
                    assessmentperiodlookupdatamaintainancescreen = SetAValue(ReferenceDateDay, RowNumber, cellvalue);
                    break;

                case 6:
                    assessmentperiodlookupdatamaintainancescreen = SetAValue(ReferenceDateMonth, RowNumber, cellvalue);
                    break;

                case 7:
                    assessmentperiodlookupdatamaintainancescreen = SetAValue(ResourceProvider, RowNumber, cellvalue);
                    break;
            }
            return assessmentperiodlookupdatamaintainancescreen;
        }



        /// <summary>
        /// Generate a list based on the Column Locator
        /// </summary>
        private List<string> GenerateAllValuesList(By colelementlocator)
        {
            List<string> ValueList = new List<string>();
            ReadOnlyCollection<IWebElement> ColumnElementList = WebelementsList(colelementlocator);
            foreach (IWebElement eachelement in ColumnElementList)
            {
                ValueList.Add(eachelement.Text);
            }
            return ValueList;
        }

        /// <summary>
        /// Get a specific value based on the Row Number and column Locator
        /// </summary>
        private string GetAValue(By colelementlocator, int RowNumber)
        {
            List<string> GetValueList = GenerateAllValuesList(colelementlocator);
            string SingleValue = "";
            int count = 1;
            foreach (string eachelement in GetValueList)
            {
                if (count == RowNumber)
                {
                    SingleValue = eachelement;
                    break;
                }
                count++;
            }
            return SingleValue;
        }

        /// <summary>
        /// Set a specific value based on the Row Number and column Locator
        /// </summary>
        private AssessmentPeriodLookupDataMaintainanceScreen SetAValue(By colelementlocator, int RowNumber, string cellvalue)
        {
            ReadOnlyCollection<IWebElement> ColumnElementList = WebelementsList(colelementlocator);
            int count = 1;
            foreach (IWebElement eachelement in ColumnElementList)
            {
                if (count == RowNumber)
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(eachelement));
                    eachelement.Click();
                    waiter.Until(ExpectedConditions.ElementExists(EditorCell)).SendKeys(cellvalue);
                    break;
                }
                count++;
            }
            return new AssessmentPeriodLookupDataMaintainanceScreen();
        }
    }
}
