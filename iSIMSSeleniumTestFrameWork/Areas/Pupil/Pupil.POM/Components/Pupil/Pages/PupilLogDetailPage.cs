using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System.Collections.Generic;

namespace POM.Components.Pupil
{
    public class PupilLogDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("pupil_log_detail"); }
        }

        #region Page Properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='\"statsbar_widget_assessment_header\"']")]
        private IWebElement _assessmentContent;

        [FindsBy(How = How.Name, Using = "[data-automation-id='\"statsbar_widget_send_content\"']")]
        private IWebElement _senContent;

        [FindsBy(How = How.Name, Using = "[data-automation-id='service_navigation_contextual_link_SEN_Record']")]
        private IWebElement _senRecordButton;

        // luong.mai missing data-automation-id
        [FindsBy(How = How.CssSelector, Using = ".media .stats-avatar-detail-text")]
        private IWebElement _pupilStatus;

        // luong.mai missing data-automation-id
        [FindsBy(How = How.CssSelector, Using = ".stats-avatar-extra li:nth-child(1) .stats-avatar-detail-text")]
        private IWebElement _pupilYears;

        // luong.mai missing data-automation-id
        [FindsBy(How = How.CssSelector, Using = ".stats-avatar-extra li:nth-child(2) .stats-avatar-detail-text")]
        private IWebElement _pupilYearsPerClass;

        [FindsBy(How = How.CssSelector, Using = ".display-dl>dd")]
        private IWebElement _pupilFullName;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]
        private IWebElement _createNotesButton;

        [FindsBy(How = How.CssSelector, Using = "[data-ajax-url $= 'General']")]
        private IWebElement _generalNoteMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[data-ajax-url $= 'Assessment']")]
        private IWebElement _assessmentNoteMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[data-ajax-url $= 'Achievements']")]
        private IWebElement _achievementsNoteMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[data-ajax-url $= 'Attendance']")]
        private IWebElement _attendanceNoteMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[data-ajax-url $= 'SEN']")]
        private IWebElement _senNoteMenuItem;

        [FindsBy(How = How.Id, Using = "timeline-container")]
        private IWebElement _timeLine;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='popover_toolbar_action-Pupil-Summary']")]
        private IWebElement _summaryIcon;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='popover_toolbar_action-Medical-Details']")]
        private IWebElement _medicalDetailsIcon;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='popover_toolbar_action-Contact-Summary']")]
        private IWebElement _contactDetailsIcon;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='popover_toolbar_action-Linked-Pupils']")]
        private IWebElement _linkedPupilsIcon;

        [FindsBy(How = How.CssSelector, Using = ".no-information-text")]
        private IWebElement _noDataLabel;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='\"statsbar_widget_assessment_header\"']")]
        private IWebElement _assessmentWidget;

        public PupilLogTimeLine TimeLine
        {
            get { return new PupilLogTimeLine(_timeLine); }
        }

        public string PupilStatus
        {
            get { return _pupilStatus.GetText(); }
        }

        public string PupilYears
        {
            get { return _pupilYears.GetText(); }
        }

        public string PupilYearsPerClass
        {
            get { return _pupilYearsPerClass.GetText(); }
        }

        public string PupilFullName
        {
            get { return _pupilFullName.GetText(); }
        }

        public string PupilForename
        {
            get { return PupilFullName.Split(' ')[0]; }
        }

        public string PupilSurname
        {
            get { return PupilFullName.Split(' ')[1]; }
        }
        
        public string SendStage
        {
            get 
            {
                IWebElement SenPanel = SeleniumHelper.Get(SimsBy.AutomationId("\"statsbar_widget_send_content\""));
                return SenPanel.FindElements(By.CssSelector("dd"))[0].GetText();  
            }
        }

        public string SendNeeds
        {
            get
            {
                IWebElement SenPanel = SeleniumHelper.Get(SimsBy.AutomationId("\"statsbar_widget_send_content\""));
                return SenPanel.FindElements(By.CssSelector("dd"))[1].GetText(); 
            }
        }

        #endregion

        #region Page Actions
        public SenRecordDetailPage ClickSenRecord()
        {

            SeleniumHelper.Get(SimsBy.AutomationId("service_navigation_contextual_link_SEN_Record")).Click();
            Wait.WaitForAjaxReady(By.Id("nprogress"));
            return new SenRecordDetailPage();
        }

        public void ClickCreateNote()
        {
            _createNotesButton.ClickByJS();
        }

        public NoteDialog SelelectGeneralNoteType()
        {
            _generalNoteMenuItem.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new NoteDialog();
        }

        public NoteDialog SelelectAssessmentNoteType()
        {
            _assessmentNoteMenuItem.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new NoteDialog();
        }

        //public NoteDialog SelelectAchievementsNoteType()
        //{
        //    _achievementsNoteMenuItem.ClickByJS();
        //    Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        //    return new NoteDialog();
        //}

        public NoteDialog SelelectAttendanceNoteType()
        {
            _attendanceNoteMenuItem.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new NoteDialog();
        }

        public NoteDialog SelelectSENNoteType()
        {
            _senNoteMenuItem.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new NoteDialog();
        }

        public PupilSummaryDialog ClickPupilSummaryIcon()
        {
            _summaryIcon.ClickByJS();
            Wait.WaitForElementEnabled(SimsBy.AutomationId("popover_toolbar_action-Pupil-Summary"));
            return new PupilSummaryDialog();
        }

        public MedicalDetailsDialog ClickMedicalDetailsIcon(){
            _medicalDetailsIcon.ClickByJS();
            Wait.WaitForElementEnabled(SimsBy.AutomationId("popover_toolbar_action-Medical-Details"));
            return new MedicalDetailsDialog();
        }

        public ContactDetailsDialog ClickContactDetailsIcon()
        {
            _contactDetailsIcon.ClickByJS();
            Wait.WaitForElementEnabled(SimsBy.AutomationId("popover_toolbar_action-Contact-Summary"));
            return new ContactDetailsDialog();
        }

        public LinkedPupilsDialog ClickLinkedPupilsIcon()
        {
            _linkedPupilsIcon.ClickByJS();
            Wait.WaitForElementEnabled(SimsBy.AutomationId("popover_toolbar_action-Linked-Pupils"));
            return new LinkedPupilsDialog();
        }

        public bool IsAssessmentResultsDisplayed()
        {
            if (_assessmentWidget.IsExist())
            {
                return true;
            }
            return false;
        }

        #endregion

        public class PupilLogTimeLine
        {
            private IWebElement _timeLineElement;
            public PupilLogTimeLine(IWebElement _webElement) {
                _timeLineElement = _webElement;
            }

            public Note this[string noteName]
            {
                get { return GetNote(noteName); }
            }

            public Note GetNote(string noteName)
            {
                var _notes = _timeLineElement.FindElements(SimsBy.CssSelector(".event"));
                foreach (var note in _notes)
                {
                    var name = note.FindElement(SimsBy.AutomationId("log-event-heading")).GetText();
                    if (name.Trim().Equals(noteName))
                    {
                        return new Note(note);
                    }
                }

                return null;
            }

            public List<Note> GetNotes()
            {
                List<Note> lstNote = new List<Note>();
                var _notes = _timeLineElement.FindElements(SimsBy.CssSelector(".event"));
                foreach (var noteElement in _notes)
                {
                    var note = new Note(noteElement);
                    lstNote.Add(note);
                }
                return lstNote;
            }
        }
    }

    public class Note
    {
        private IWebElement noteElement;

        public Note(IWebElement webElement)
        {
            noteElement = webElement;
        }

        public PupilLogDetailPage Delete()
        {
            noteElement.FindElement(SimsBy.AutomationId("delete_button")).ClickByJS();
            SeleniumHelper.FindElement(SimsBy.AutomationId("Yes_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            return new PupilLogDetailPage();
        }

        public NoteDialog Edit()
        {
            noteElement.FindElement(SimsBy.AutomationId("edit_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new NoteDialog();
        }

        public string NoteName
        {
            get { return noteElement.FindElement(SimsBy.AutomationId("log-event-heading")).GetText(); }
        }
    }
}
