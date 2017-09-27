using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using WebDriverRunner.webdriver;
using POM.Helper;
using System.Threading;

namespace Attendance.Components.AttendancePages
{
   public class EditMarksPupilDetail : BaseSeleniumComponents
   {
#pragma warning disable 0649
       [FindsBy(How = How.CssSelector, Using = ".webix_ss_left .webix_cell.webix_cell_select")]
       public IWebElement pupilLink;
       [FindsBy(How = How.CssSelector, Using = "[data-automation-id='dialog-editableData']")]
       public IWebElement pupilLogPopup;
       [FindsBy(How = How.CssSelector, Using = ".grid-caption")]
       public IWebElement pupilLogPupilRecentAbsences;
       [FindsBy(How = How.CssSelector, Using = ".media-heading")]
       public IWebElement pupilLogPupilContact;
       [FindsBy(How = How.CssSelector, Using = "[data-automation-id='popover-custom-id']")]
       public IWebElement AttendanceLog;
       [FindsBy(How = How.CssSelector, Using = "[data-automation-id='view_pupil_log_button']")]
       public IWebElement viewPlogNote;
       [FindsBy(How = How.CssSelector, Using = "[title='Add a note for this pupil']")]
       public IWebElement attendanceNote;
       [FindsBy(How = How.Name, Using = "NoteText")]
       public IWebElement attendanceNoteTextArea;
       [FindsBy(How = How.Name, Using = "Title")]
       public IWebElement attendanceTitleArea;
       [FindsBy(How = How.Name, Using = "Pinned")]
       public IWebElement attendancePinNote;
       [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
       public IWebElement attendanceNotevalidationWarning;
       [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='save_button']")]
       public IWebElement attendanceNoteTextSave;
       [FindsBy(How = How.CssSelector, Using = "[data-automation-id='pupil_log_detail']")]
       public IWebElement pupilLogPage;
       [FindsBy(How = How.CssSelector, Using = "[data-automation-id='log-note-header-attendance']")]
       public IWebElement AttendanceNoteOnPupilLogPage;
       [FindsBy(How = How.CssSelector, Using = "button[title='Pinned Notes']")]
       public IWebElement pinNote;

        public static By loc = By.CssSelector("[data-automation-id='view_pupil_log_button']");
        public static By waitForLoc = By.Name("NoteText");


        public EditMarksPupilDetail()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void ClickViewPupilLogLink()
        {
            Wait.WaitForElementReady(loc);
            viewPlogNote.Click();
            Wait.WaitLoading();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        public void ClickOnAttendanceNoteButton()
        {
            WaitForElement(loc);
            attendanceNote.Click();
            Wait.WaitLoading();
            WaitForElement(waitForLoc);
        }

        public void EnterTextInAttendanceNoteTextArea(string text)
        {
            WaitForElement(waitForLoc);
            attendanceNoteTextArea.SendKeys(text);
        }

        public void EnterTitle(string title)
        {
            WaitForElement(waitForLoc);
            attendanceTitleArea.SendKeys(title);
        }

        public void PinNote()
        {
            WaitForElement(waitForLoc);
            attendancePinNote.Click();
        }


        public AttendanceDetails AttendanceNoteTextSave()
        {
            attendanceNoteTextSave.Click();
            Thread.Sleep(2000);
            return new AttendanceDetails();
        }    
    }
}
