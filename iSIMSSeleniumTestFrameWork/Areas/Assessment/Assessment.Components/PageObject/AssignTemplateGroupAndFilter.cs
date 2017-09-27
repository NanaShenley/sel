using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using Assessment.Components.PageObject;
using SeSugar.Automation;
using Assessment.Components.Common;
using System.Collections.ObjectModel;
using System.Diagnostics;
using OpenQA.Selenium.Interactions;
using TestSettings;

namespace Assessment.Components
{
    public class AssignTemplateGroupAndFilter : MarksheetType
    {
        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
        public const string DetailName = "editableData";

        public static readonly int Timeout = 30;
        public static readonly By Result = By.CssSelector("[data-section-id='searchResults']");
        public static readonly By ResultCba = By.CssSelector("[data-section-id='searchResults']>div>div>span");
        public static readonly By SelectCba = By.CssSelector("[data-section-id='searchResults']>div>div:nth-child(2)>div>div>a");
        public static readonly By AssignGroupPopup = By.CssSelector("[data-automation-id='associate_group']");

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='assign_group(s)_to_create_new_marksheet(s)_button']")]
        private IWebElement _assignGroup;

        [FindsBy(How = How.CssSelector, Using = "button[title='Hide Search']")]
        private IWebElement _hideSearchPanel;

        //[FindsBy(How = How.CssSelector, Using = "div[data-automation-id='associate_group'] button[data-automation-id='ok_button']")]
        //private IWebElement _OkButton;

        [FindsBy(How = How.CssSelector, Using = "a[id='AssignTemplateGroupAndFilterSave']")]
        private IWebElement _SaveButton;

        private static By YearGroupsCheckBox = By.CssSelector("input[name='YearGroups.SelectedIds']");
        private static By ClassesCheckBox = By.CssSelector("input[name='Classes.SelectedIds']");

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='groups'] div.slider-header-title")]
        private IWebElement GroupHeader;

        [FindsBy(How = How.Name, Using = "Filter.dropdownImitator")]
        private IWebElement _assignFilterCombobox;

        [FindsBy(How = How.Name, Using = "groupFilterModel.NCYear.dropdownImitator")]
        private IWebElement _assignSubFilterCombobox;

        private static By MarksheetNameLocator = By.CssSelector("div[name='MarksheetDetails'] div[column='1'] div.webix_cell span.btn-text-afforded");
        private static By _OkButton = By.CssSelector("div[data-automation-id='associate_group'] button[data-automation-id='ok_button']");

        List<string> groupList = new List<string>();

        public string FilterCategory
        {
            set { _assignFilterCombobox.EnterForDropDown(value); }
            get { return _assignFilterCombobox.GetText(); }
        }

        public string SubFilterCategory
        {
            set { _assignSubFilterCombobox.EnterForDropDown(value); }
            get { return _assignSubFilterCombobox.GetText(); }
        }

        public AssignTemplateGroupAndFilter()
        {
            //Thread.Sleep(2000);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void OpenManageTemplateScreen()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            WebContext.WebDriver.FindElement(MarksheetConstants.AssignTemplateGroupAndFilter).Click();
        }

        public void SelectResult()
        {
            //wait until the result is loaded and has text
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(Timeout));
            waiter.Until(ExpectedConditions.ElementToBeClickable(Result));
            WaitUntilEnabled(Result);
            SeleniumHelper.WaitForElementClickableThenClick(Result);

            waiter.Until(ExpectedConditions.ElementIsVisible(ResultCba));
        }

        public AssignTemplateGroupAndFilter AssigGroupClick()
        {
            Thread.Sleep(4000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(_assignGroup)).Click();
            
            //wait logic
            return this;
        }

        public AssignTemplateGroupAndFilter HideSearchButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_hideSearchPanel)).Click();

            //wait logic
            return this;
        }

        public AssignTemplateGroupAndFilter SaveButton()
        {

            Thread.Sleep(4000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(_SaveButton)).Click();
            //wait logic
            return this;
        }

        public void OkButtonClick()
        {
            //waiter.Until(ExpectedConditions.ElementToBeClickable(_OkButton)).Click();
            Thread.Sleep(8000);
            WaitForAndClick(TimeSpan.FromSeconds(MarksheetConstants.Timeout), _OkButton);
            //wait logic
        }

        public List<string> AssignGroup()
        {
            SelectYearGroup("Year 1");
            groupList = GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);
            Thread.Sleep(4000);
            WaitForAndClick(TimeSpan.FromSeconds(MarksheetConstants.Timeout), _OkButton);
            return groupList;
        }

        public void SelectYearGroup(string YearGroup)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AssignGroupPopup));
            ReadOnlyCollection<IWebElement> YearGroupCheckboxlistYG = WebContext.WebDriver.FindElements(YearGroupsCheckBox);
            foreach (IWebElement eachelement in YearGroupCheckboxlistYG)
            {
                if (WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == YearGroup)
                {
                    eachelement.Click();
                    break;
                }
            }
        }

        public void SelectClass(string Class)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AssignGroupPopup));
            ReadOnlyCollection<IWebElement> ClassesList = WebContext.WebDriver.FindElements(ClassesCheckBox);
            foreach (IWebElement eachelement in ClassesList)
            {
                if (WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == Class)
                {
                    eachelement.Click();
                    break;
                }
            }
        }

        public List<string> GetSelectedGroupOrGroupfilter(By SelectGroup)
        {
            List<string> selectedGroup = new List<string>();
            ReadOnlyCollection<IWebElement> CheckboxlistGroup = WebContext.WebDriver.FindElements(SelectGroup);

            foreach (IWebElement groupfilterElem in CheckboxlistGroup)
            {
                if (groupfilterElem.Selected)
                {
                    // isSelected = true;
                    selectedGroup.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + groupfilterElem.GetAttribute("id") + "']")).Text);
                }
            }
            return selectedGroup;
        }

        public AssignTemplateGroupAndFilter Save()
        {
            Thread.Sleep(1000);
            _SaveButton.Click();
            WaitUntillAjaxRequestCompleted();
            return this;
        }

        public string GetValueFromMarksheetDetails(int columnNumber)
        {
            return GetAValue(MarksheetNameLocator, columnNumber);
        }

        /// <summary>
        /// Returs a Webelement list for the specifed column
        /// </summary>
        public ReadOnlyCollection<IWebElement> WebElementsList(By ColumnName)
        {
            waiter.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(ColumnName));
            return WebContext.WebDriver.FindElements(ColumnName);
        }

        /// <summary>
        /// Generate a list based on the Column Locator
        /// </summary>
        private List<string> GenerateAllValuesList(By colelementlocator)
        {
            List<string> ValueList = new List<string>();
            ReadOnlyCollection<IWebElement> ColumnElementList = WebElementsList(colelementlocator);
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

        public void clickElementAtPosition(int RowNumber)
        {
            int count = 1;
            List<string> ValueList = new List<string>();
            ReadOnlyCollection<IWebElement> ColumnElementList = WebElementsList(MarksheetNameLocator);
            foreach (IWebElement eachelement in ColumnElementList)
            {
                if (count == RowNumber)
                {
                    eachelement.Click();
                }
                count++;
            }
        }
    }

    public static class ElementAccessor
    {
        /// <summary>
        /// Au: Logigear
        /// Des: Enter a string into TextBox, TextArea... controls
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void EnterForDropDown(this IWebElement element, string value)
        {
            try
            {
                element.SendKeys(Keys.Enter);
                WaitForAjaxReady(By.CssSelector(".locking-mask"));
                //IWebElement inputElement = FindElement(SimsBy.CssSelector("#select2-drop input.select2-input"));
                var inputElement = ElementRetriever.GetOnceLoaded(SeSugar.Automation.SimsBy.CssSelector("#select2-drop input.select2-input"));
                inputElement.WaitUntilState(ElementState.Displayed);
                inputElement.SendKeys(value);
                Thread.Sleep(2000);
                inputElement.SendKeys(Keys.Enter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Have an exception happens when enter date for drop down: " + ex.ToString());
            }
        }
        /// <summary>
        /// Au: Logigear
        /// Des: Wait for an ajax request is completed
        /// Wait for progress bar is invisibled
        /// </summary>
        public static bool WaitForAjaxReady(By ajaxProgressElement)
        {
            Console.WriteLine("Waiting for Ajax request to complete");
            bool ajaxIsComplete = false;
            Stopwatch waiting = new Stopwatch();
            var timeOut = BrowserDefaults.TimeOut;
            waiting.Start();
            while (waiting.Elapsed <= timeOut)
            {
                string openConnections = (WebContext.WebDriver as IJavaScriptExecutor).ExecuteScript("return jQuery.active").ToString();

                ajaxIsComplete = openConnections == "0";
                if (ajaxIsComplete)
                    break;

                Thread.Sleep(10);
            }
            waiting.Stop();

            if (ajaxIsComplete)
                Console.WriteLine("Ajax request completed in approximately {0}ms.", waiting.ElapsedMilliseconds);
            else
                throw new WebDriverTimeoutException(string.Format("5. Waiting for connections to close timed out after {0}ms", timeOut.Milliseconds));

            return true;
        }
        /// <summary>
        /// Au: Huy Vo
        /// Des: GetText method uses to get text from element
        /// <param name="element">Element contais text</param>
        /// <returns>Return string value</returns>
        public static String GetText(this IWebElement element)
        {
            return element.Text;
        }
    }
}
