using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Pupil.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using System;
using OpenQA.Selenium.Interactions;
using SharedComponents;

namespace Pupil.Components.PupilRecord
{
    /// <summary>
    /// Page object for pupil record detail area
    /// </summary>
    public class PupilRecordDetail: BaseSeleniumComponents
    {
        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton = null;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-grid-id='mulitpleLearnerEnrolmentStatusGrid'] tbody tr:first-child td.grid-cell:nth-child(6) input")]
        private IWebElement _firstEnrolmentStatusHistoryRowEndDate = null;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-grid-id='mulitpleLearnerEnrolmentStatusGrid'] tbody tr:nth-child(2) td.grid-cell:nth-child(4) input")]
        private IWebElement _secondEnrolmentStatusHistoryRowEnrolmentStatus = null;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-grid-id='mulitpleLearnerEnrolmentStatusGrid'] tbody tr:nth-child(2) td.grid-cell:nth-child(5) input")]
        private IWebElement _secondEnrolmentStatusHistoryRowStartDate = null;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-grid-id='mulitpleLearnerEnrolmentStatusGrid'] tbody tr:nth-child(2) td.grid-cell:nth-child(3) button")]
        private IWebElement _secondEnrolmentStatusDeleteRowButton = null;

        [FindsBy(How = How.CssSelector, Using = "div.popover-content [data-automation-id='Yes_button']")]
        private IWebElement _rowDeleteConfirmationYesButton = null;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-grid-id='LearnerPreviousSchoolsGrid'] tbody tr:nth-child(1) td.grid-cell:nth-child(3) button")]
        private IWebElement _firstPreviousSchoolDeleteRowButton = null;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria'] [name='Name']")]
        private IWebElement _schoolName = null;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-grid-id='dialog-LearnerPreviousSchoolAttendanceSummariesGrid'] tbody tr:nth-child(1) td:nth-child(4) input")]
        private IWebElement _academicYear = null;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-grid-id='dialog-LearnerPreviousSchoolAttendanceSummariesGrid'] tbody tr:nth-child(1) td:nth-child(5) input")]
        private IWebElement _possibleSessions = null;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]        
        private IList<IWebElement> _status = null;

        public static readonly By _mulitpleLearnerEnrolmentStatusGrid = By.CssSelector("[data-maintenance-grid-id='mulitpleLearnerEnrolmentStatusGrid']");

        public static readonly By _learnerPreviousSchoolsGrid = By.CssSelector("[data-maintenance-grid-id='LearnerPreviousSchoolsGrid']");

        /// <summary>
        /// Constructor to initialise the page area
        /// </summary>
        public PupilRecordDetail()
        {            
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitForElement(PupilRecordElements.PupilRecord.Detail.LegalName);
        }

        public PupilRecordDetail Save()
        {
            Actions actions = new Actions(WebContext.WebDriver);            
            var saveBtnLocator = By.CssSelector("[data-automation-id='well_know_action_save']");
            actions.MoveToElement(SeleniumHelper.Get(saveBtnLocator)).Perform();
            SeleniumHelper.WaitForElementClickableThenClick(saveBtnLocator);

            var saveSuccessLocator = By.CssSelector(SeleniumHelper.AutomationId("status_success"));
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessLocator));            
            return this;
        }

        public PupilRecordDetail AddNewEnrolmentStatusHistory()
        {
            Thread.Sleep(3000);

            waiter.Until(ExpectedConditions.ElementExists(_mulitpleLearnerEnrolmentStatusGrid));

            _firstEnrolmentStatusHistoryRowEndDate.Clear();
            _firstEnrolmentStatusHistoryRowEndDate.SendKeys("09/09/2014");

            Thread.Sleep(1000);

            _secondEnrolmentStatusHistoryRowEnrolmentStatus.ChooseSelectorOption("Single Registration");
            _secondEnrolmentStatusHistoryRowStartDate.SendKeys("10/09/2014");

            return this;
        }

        public PupilRecordDetail DeleteNewEnrolmentStatusHistory()
        {
            Thread.Sleep(1000);

            waiter.Until(ExpectedConditions.ElementExists(_mulitpleLearnerEnrolmentStatusGrid));

            var loc = By.CssSelector("[data-maintenance-grid-id='mulitpleLearnerEnrolmentStatusGrid'] tbody tr:first-child td.grid-cell:nth-child(6) input");
            var firstEnrolmentStatusHistoryRowEndDate = SeleniumHelper.Get(loc);
            firstEnrolmentStatusHistoryRowEndDate.Clear();

            _secondEnrolmentStatusDeleteRowButton.Click();

            Thread.Sleep(1000);

            _rowDeleteConfirmationYesButton.Click();

            Thread.Sleep(1000);

            return this;
        }

        public PupilRecordDetail AddNewPreviousSchoolHistory()
        {
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("section_menu_School"));
            Thread.Sleep(TimeSpan.FromSeconds(2));
            SeleniumHelper.WaitForElementClickableThenClick(By.CssSelector(SeleniumHelper.AutomationId("add_school_button")));         
            IWebElement selectSchoolDialog = waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-prefix='dialog-']")));
            IWebElement schoolName =
                selectSchoolDialog.FindChild(By.CssSelector("[data-automation-id='search_criteria'] input[name='Name']"));
            schoolName.SendKeys("castletower");
            selectSchoolDialog.FindChild(By.CssSelector("[data-automation-id='search_criteria_submit']")).Click();
            selectSchoolDialog.FindChild(By.CssSelector("[data-automation-id='resultTile']")).Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            selectSchoolDialog.FindChild(By.CssSelector("[data-automation-id='ok_button']")).Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return AddNewPreviousSchoolAttendanceSummary();
        }

        public PupilRecordDetail AddNewPreviousSchoolAttendanceSummary()
        {            
            SeleniumHelper.WaitForElementClickableThenClick(By.CssSelector(SeleniumHelper.AutomationId("attendance_summaries_button")));
            IWebElement attendanceSummaryDialog = waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-prefix='dialog-']")));
            Thread.Sleep(TimeSpan.FromSeconds(2));
            IWebElement academicYear = attendanceSummaryDialog.FindChild(By.CssSelector("tr[data-role='gridRow'] td>div>input[name$='Year']"));
            academicYear.SendKeys("2014/2015");
            IWebElement possibleSessions = attendanceSummaryDialog.FindChild(By.CssSelector("tr[data-role='gridRow'] td>div>input[name$='PossibleSessions']"));
            possibleSessions.SendKeys("200");
            attendanceSummaryDialog.FindChild(By.CssSelector("[data-automation-id='ok_button']")).Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return this;
        }

        public PupilRecordDetail DeleteNewPreviousSchoolHistory()
        {
            waiter.Until(ExpectedConditions.ElementExists(_learnerPreviousSchoolsGrid));

           _firstPreviousSchoolDeleteRowButton.Click();

           _rowDeleteConfirmationYesButton.Click();

           return this;
        }

        public void CloseSuccessMessage()
        {
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("status_success"));
        }

        public bool HasSavedSuccessfully()
        {
            var loc = By.CssSelector(SeleniumHelper.AutomationId("status_success"));
            var status = SeleniumHelper.Get(loc);

            //TODO KAR OpenQA.Selenium.StaleElementReferenceException but only in IE
            return status.Text == "Pupil record saved";
        }
    }
}
