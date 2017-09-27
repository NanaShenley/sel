using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Support.UI;
using Assessment.Components.Common;
using System;

namespace Assessment.Components
{
    public class ResultHistory : BaseSeleniumComponents
    {
        public const string EditorSelector = "//div[contains(@class,'webix_dt_editor')]/input[@type='text']";

        public const string Assessmentmenu = "//div[contains(@class,'webix_dt_editor')]/div[contains(@class,'btn-group')]/button[contains(@class,'btn btn-link dropdown-toggle')]";

        public const string historydialogid = "[data-grid-table='']";
        public const string historyresultinputlist = "//div[contains(@class,'grid-cell-control')]/input";

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_DropdownRadio']")] private IWebElement _assessmentYear;

        public ResultHistory(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.TestUser)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            PageFactory.InitElements(WebContext.WebDriver, this);

            SeleniumHelper.Login(userType);
        }

        public ResultHistory OpenMarksheet(string marksheetName)
        {
            //SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Marksheets");
            CommonFunctions.GotToMarksheetMenu();
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(marksheetName));
            return this;
        }

        public List<IWebElement> GetResultelements()
        {
            List<IWebElement> resultsList = WaitForAndGet(By.CssSelector(historydialogid)).FindElements(By.XPath(historyresultinputlist)).ToList();

            return resultsList;
        }

        public ResultHistory EnterValueInCell(string value)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-automation-id='save_button']")));
            
            var editableCells = MarksheetGridHelper.FindAllEditablecells();
            Thread.Sleep(1000);
            editableCells.First().FindElements(By.CssSelector(MarksheetGridHelper.CellSelector)).First().Click();            
            GetEditor().SendKeys(value);
            return this;
        }

        public void ClickOkButton()
        {
            WaitForElement(By.CssSelector("[data-automation-id='ok_button']"));
            WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='ok_button']")).Where(x => x.Displayed == true).FirstOrDefault().Click();


        }

        public ResultHistory ReEnterValueInCell(string value)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-automation-id='save_button']")));

            var editableCells = MarksheetGridHelper.FindAllEditablecells();
            Thread.Sleep(1000);
            editableCells.First().FindElements(By.CssSelector(MarksheetGridHelper.SelectedCellSelector)).First().Click();            
            GetEditor().SendKeys(value);
            return this;
        }

        public ResultHistory ClearValueInCell()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-automation-id='save_button']")));

            List<IWebElement> editableCells = MarksheetGridHelper.FindAllEditablecells();
            Thread.Sleep(1000);
            editableCells.First().FindElements(By.CssSelector(MarksheetGridHelper.SelectedCellSelector)).First().Click();            
            GetEditor().Clear();
            return this;
        }

        public static IWebElement GetEditor()
        {
            return WebContext.WebDriver.FindElement(By.XPath(EditorSelector));
        }

        public static IWebElement GetCellMenu()
        {
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.XPath(Assessmentmenu));
            return WebContext.WebDriver.FindElement(By.XPath(Assessmentmenu));
        }

        public ResultHistory SaveMarksheet()
        {
            WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='save_button']")).Click();
            WaitUntillAjaxRequestCompleted();
            return this;
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_DropdownRadio']")]
        private IWebElement _AssessmentYear;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='2014/2015']")]
        private IWebElement _SelectAssessmentYEar;
        public void OpenClickAssessmentYear()
        {
            WaitUntilDisplayed(By.CssSelector("[data-automation-id='Button_DropdownRadio']"));
            _AssessmentYear.Click();

            WaitUntilDisplayed(By.CssSelector("[data-automation-id='2014/2015']"));
            _SelectAssessmentYEar.Click();

        }

        public ResultHistory ClickCellMenu()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-automation-id='save_button']")));
            
            List<IWebElement> editableCells = MarksheetGridHelper.FindAllEditablecells();
            Thread.Sleep(1000);
            editableCells.First().FindElements(By.CssSelector(MarksheetGridHelper.CellSelector)).First().Click();
            GetCellMenu().Click();
            return this;
        }

        public ResultHistory ViewResultHistory()
        {
            List<IWebElement> resultHistoryElements = WebContext.WebDriver.FindElements(By.CssSelector("[data-show-result-history-dialog='']")).ToList();
            resultHistoryElements.FirstOrDefault(rh => rh.Displayed).Click();
            return this;
        }

        public ResultHistory CloseResultHistory()
        {             
            WaitUntilDisplayed(By.CssSelector("[data-automation-id='edit_result_history_popup_header_title']"));
            WebContext.WebDriver.FindElements(By.CssSelector("[data-cancel-dialog-button='']")).FirstOrDefault(rhd => rhd.Displayed).Click();
            return this;
        }

        public ResultHistory OpenAssessmentYear()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementToBeClickable(_assessmentYear));
            _assessmentYear.Click(); 
            return this;
        }

        public ResultHistory PickAssessmentYear()
        {
            WaitUntilDisplayed(By.CssSelector("[data-automation-id='2013/2014']"));
            WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='2013/2014']")).Click();
            return this;
        }
    }
}