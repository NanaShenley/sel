using Attendance.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using POM.Helper;
using SharedComponents.BaseFolder;
using System.Collections.Generic;
using System.Threading;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Attendance.Components.AttendancePages
{
    public class CreateExceptionalCircumstancesPage : BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.Name, Using = "Description")]
        public readonly IWebElement Description;
        [FindsBy(How = How.CssSelector, Using = "#editableData [name='StartDate']")]
        public readonly IWebElement mainPageStartDate;
        [FindsBy(How = How.CssSelector, Using = "#editableData [name='EndDate']")]
        public readonly IWebElement mainPageEndDate;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        public readonly IWebElement searchButton;
        [FindsBy(How = How.Name, Using = "StartSession.dropdownImitator")]
        public readonly IWebElement SessionStart;
        [FindsBy(How = How.Name, Using = "EndSession.dropdownImitator")]
        public readonly IWebElement SessionEnd;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        public readonly IWebElement ValidationWarning;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pupils_button']")]
        public readonly IWebElement AddPupilLink;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='remove_button']")]
        public readonly IWebElement Trashicon;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Selected Pupils*']")]
        public readonly IWebElement SelectedPupilSection;
        [FindsBy(How = How.CssSelector, Using = ".webix_table_checkbox")]
        public readonly IWebElement PupilGrid;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='exceptional_circumstance_Individual_pupil_dialog']")]
        public readonly IWebElement SelectedPupilOverlappingdialog;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='yes_button']")]
        public readonly IWebElement YesButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        public CreateExceptionalCircumstancesPage()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }


        public void EnterDescription(string description)
        {
            WaitForAndGet(AttendanceElements.ExceptionalCircumstanceElements.Description);
            Description.Clear();
            Description.SendKeys(description);
        }

        public void EnterDate(string date, IWebElement Date)
        {
            //var Date = WebContext.WebDriver.FindElement(DateSelector);
            Date.Clear();
            Date.SendKeys(date);
        }

        //Session Selector
        public void SessionSelector(string session, int number)
        {

            var Session = WebContext.WebDriver.FindElements(By.CssSelector("[class='select2-arrow']"));

            Session[number].Click();
            var AllDropDownList = WebContext.WebDriver.FindElements(By.ClassName("select2-result-label"));


            AllDropDownList[number].Click();

        }

        public void EnterStartSession(string startSession)
        {
            SessionSelector(startSession, 0);
        }


        public void EnterEndSession(string endSession)
        {
            SessionSelector(endSession, 1);
        }

        public void WaitElementToBeClickable(By locator)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public AttendanceSearchPanel ClickAddPupilLink()
        {
            WaitForElement(AttendanceElements.AddPupilPopUpElements.AddPupilLink);
            AddPupilLink.Click();
            WaitElementToBeClickable(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchButton);
            return new AttendanceSearchPanel();
        }

        public void Pupilselector()
        {
            WaitElementToBeClickable(AttendanceElements.AddPupilPopUpElements.AddPupilLink);
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.AddPupilPopUpElements.AddPupilLink);
            var searchCriteria = SeleniumHelper.Get(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchPanel);
            WaitElementToBeClickable(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchButton);
            searchCriteria.Click(SeSugar.Automation.SimsBy.AutomationId("section_menu_Year Group"));
            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1" });
            WaitElementToBeClickable(AttendanceElements.AddPupilPopUpElements.PupilPickerSearchButton);
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.AddPupilPopUpElements.PupilPickerSearchButton);
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.AddPupilPopUpElements.SearchRecordsToFindtext);
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.AddPupilPopUpElements.AddSelectedPupilButton);
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.AddPupilPopUpElements.PupilSelectorOkButton);
            WaitUntilDisplayed(AttendanceElements.AddPupilPopUpElements.TrashIcon);
        }

        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));   

            if (overlappingdialog()!=0)
            {
                YesButton.ClickByAction();         
            }
        }

        public bool IsDisplayedValidationWarning()
        {
            WaitUntilDisplayed(AttendanceElements.ValidationWarning);
            bool value = WebContext.WebDriver.FindElement(AttendanceElements.ValidationWarning).Displayed;
            return value;
        }

        public void Confirmation()
        {
            BaseSeleniumComponents.WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.ExceptionalCircumstanceElements.ConfirmationYes);
            //Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));        
        }

        public bool HasConfirmedSave()
        {
            var css = string.Format("{0}", SeleniumHelper.AutomationId("status_success"));
            Thread.Sleep(2000);// //TODO: Find better alternative for wait.
            bool value = WebContext.WebDriver.FindElement(By.CssSelector(css)).Displayed;
            return value;
        }
       

        public void Delete()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.ExceptionalCircumstanceElements.DeleteButton);
        }

        public void RemovePupilFromGrid()
        {
            var element = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='remove_button']"));
            for (int i = 0; i < element.Count; i++)
            {
                element[i].Click();
                Wait.WaitForDocumentReady();
            }
        }

        public bool DeleteDialogDisappeared()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.ExceptionalCircumstanceElements.ContinueWithDelete);
            Thread.Sleep(2000);
            IReadOnlyCollection<IWebElement> list = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='confirm_delete_dialog']"));
            bool val = (list.Count==0);
            return val;
        }

        public int overlappingdialog()
        {
            IReadOnlyCollection<IWebElement> list = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='exceptional_circumstance_Individual_pupil_dialog']"));
            var val = list.Count;
            return val;
        }

    }
}
