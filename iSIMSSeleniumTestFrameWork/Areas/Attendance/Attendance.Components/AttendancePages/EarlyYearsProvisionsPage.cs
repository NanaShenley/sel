using Attendance.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System.Collections.Generic;
using System.Threading;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Attendance.Components.AttendancePages
{
    public class EarlyYearsProvisionsPage : BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "editableData")]
        private readonly IWebElement _mainPage;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria']")]
        private readonly IWebElement _searchPage;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        public readonly IWebElement createButton;
        [FindsBy(How = How.Name, Using = "Notes")]
        public readonly IWebElement notes;
        [FindsBy(How = How.Name, Using = "StartDate")]
        public readonly IWebElement startDate;
        [FindsBy(How = How.Name, Using = "EndDate")]
        public readonly IWebElement endDate;
        [FindsBy(How = How.Name, Using = "StartTime")]
        public readonly IWebElement startTime;
        [FindsBy(How = How.Name, Using = "EndTime")]
        public readonly IWebElement endTime;
        [FindsBy(How = How.Name, Using = "ShortName")]
        public readonly IWebElement shortName;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        public readonly IWebElement searchButton;

        public IWebElement MainPageProvisionName
        {
            get
            {
                return _mainPage.FindElement(By.Name("ProvisionName"));
            }
        }

        public IWebElement MainPageShortName
        {
            get
            {
                return _mainPage.FindElement(By.Name("ShortName"));
            }
        }

        public IWebElement SearchPanelProvisionName
        {
            get
            {
                return _searchPage.FindElement(By.Name("ProvisionName"));
            }
        }

        public IWebElement SearchPanelShortName
        {
            get
            {
                return _searchPage.FindElement(By.Name("ShortName"));
            }
        }

        public EarlyYearsProvisionsPage()
        {
            WaitUntilDisplayed(AttendanceElements.EarlyYearProvisionsElements.createProvisions);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void ClickCreate()
        {
            //WaitUntilDisplayed(AttendanceElements.EarlyYearProvisionsElements.provisionName);
            createButton.Click();
            WaitUntilDisplayed(AttendanceElements.EarlyYearProvisionsElements.MainPageStartTime);
        }

        public void EnterProvisionName(string name, IWebElement provisionname)
        {
            WaitUntilDisplayed(AttendanceElements.EarlyYearProvisionsElements.provisionName);
            provisionname.Clear();
            provisionname.SendKeys(name);
        }
        public void EnterShortName(string name, IWebElement shortname)
        {
            shortname.Clear();
            shortname.SendKeys(name);
        }

        public void EnterNotes(string notes1)
        {
            notes.Clear();
            notes.SendKeys(notes1);
        }

        public void EnterDate(string date, IWebElement Date)
        {
            Date.Clear();
            Date.SendKeys(date);
        }

        public void EnterTime(string time, IWebElement Time)
        {
            Time.Clear();
            Time.SendKeys(time);
        }

        public bool HasConfirmedSave()
        {
            var css = string.Format("{0}", SeleniumHelper.AutomationId("status_success"));
            Thread.Sleep(1000);// //TODO: Find better alternative for wait.
            bool value = WebContext.WebDriver.FindElement(By.CssSelector(css)).Displayed;
            return value;
        }

        public void Delete()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.ExceptionalCircumstanceElements.DeleteButton);
        }

        public bool DeleteDialogDisappeared()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.ExceptionalCircumstanceElements.ContinueWithDelete);
            Thread.Sleep(2000);
            IReadOnlyCollection<IWebElement> list = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='confirm_delete_dialog']"));
            bool val = (list.Count == 0);
            return val;
        }

        public bool IsDisplayedValidationWarning()
        {
            WaitUntilDisplayed(AttendanceElements.ValidationWarning);
            bool value = WebContext.WebDriver.FindElement(AttendanceElements.ValidationWarning).Displayed;
            return value;
        }
    }
}
