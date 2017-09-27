using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assessment.Components.Common;
using OpenQA.Selenium.Support.UI;
using TestSettings;
using WebDriverRunner.webdriver;
using SeSugar.Automation;

namespace Assessment.Components
{
    public class MarsksheetPupilDetail : BaseSeleniumComponents
    {
        [FindsBy(How = How.CssSelector, Using = "[name='NoteText']")]
        private IWebElement _assessmentNoteTextArea;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='_export_button']")]
        public IWebElement _exportToExcell;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
        private readonly By _pupilAdditionalDetailSelector = By.CssSelector("[data-automation-id='pupil_additional_details']");

        public MarsksheetPupilDetail(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.TestUser)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            PageFactory.InitElements(WebContext.WebDriver, this);

            SeleniumHelper.Login(userType);

            AutomationSugar.WaitForAjaxCompletion();

            //SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Marksheets");
            CommonFunctions.GotToMarksheetMenu();
        }

        public MarsksheetPupilDetail OpenMarksheet(string marksheetName)
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(marksheetName));
            return this;
        }

        public MarsksheetPupilDetail PupilLink()
        {
            Thread.Sleep(1000);
            WaitUntilDisplayed(_pupilAdditionalDetailSelector);
            waiter.Until(ExpectedConditions.ElementToBeClickable(_pupilAdditionalDetailSelector));
            IWebElement pupilDetailButton = WebContext.WebDriver.FindElement(_pupilAdditionalDetailSelector);
            WaitForAndClick(new TimeSpan(0, 0, 20), By.CssSelector("[data-automation-id='pupil_additional_details']"));
            AutomationSugar.WaitForAjaxCompletion();
            return this;
        }


        public MarsksheetPupilDetail AssessmentNote()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[title='Add a note for this pupil']"));
            AutomationSugar.WaitForAjaxCompletion();
            return this;
        }

        public MarsksheetPupilDetail GeneralNote()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='plog-generalcategory-contextlink']"));
            return this;
        }

        public MarsksheetPupilDetail AchivementsNote()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText("Achievements"));
            return this;
        }

        public MarsksheetPupilDetail AttendanceNote()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText("Attendance"));
            return this;
        }

        public MarsksheetPupilDetail SENNote()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText("SEN"));
            return this;
        }

        public MarsksheetPupilDetail AssessmentNoteTextArea(string text)
        {
            Thread.Sleep(1000);
            WaitForElement(By.CssSelector("[name='NoteText']"));
            _assessmentNoteTextArea.SendKeys(text);
            return this;
        }
        public void AssessmentNoteTextSave()
        {
            IWebElement noteSaveElement =
                WebContext.WebDriver.FindElement(By.CssSelector("[data-ajax-update-detail='']"));
            noteSaveElement.Click();
            AutomationSugar.WaitForAjaxCompletion();
        }
        public MarsksheetPupilDetail AssessmentNoteToggle()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText("Add Pupil Log Assessment Note"));
            Thread.Sleep(1000);
            return this;
        }


        public bool IsNoteSaved(string text)
        {
            WaitForElement(By.CssSelector("[data-automation-id='log-event-heading']"));
            List<IWebElement> notesElements =
                WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='log-event-heading']")).ToList();

            return notesElements.Any(note => note.Text == text);

        }

        public MarsksheetPupilDetail ViewPupilLogNote()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='view_pupil_log_button']"));
            Thread.Sleep(1000);
            return this;
        }

        public MarsksheetPupilDetail ExportMarksheet()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_exportToExcell));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='_export_button']"));
            _exportToExcell.Click();
            return this;
        }
    }
}
