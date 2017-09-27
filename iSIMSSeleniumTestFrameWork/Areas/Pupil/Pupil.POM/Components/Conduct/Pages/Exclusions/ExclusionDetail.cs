using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace POM.Components.Conduct.Pages.Exclusions
{
    public class ExclusionDetail : BaseComponent
    {
        #region Page Properties

        [FindsBy(How = How.Name, Using = "ExclusionType.dropdownImitator")]
        private IWebElement _typeDropdown;

        [FindsBy(How = How.Name, Using = "ExclusionReason.dropdownImitator")]
        private IWebElement _reasonDropdown;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateTextBox;

        [FindsBy(How = How.Name, Using = "StartTime")]
        private IWebElement _startTimeTextBox;

        [FindsBy(How = How.Name, Using = "EndTime")]
        private IWebElement _endTimeTextBox;

        [FindsBy(How = How.Name, Using = "NumberOfSchoolDays")]
        private IWebElement _LengthTextBox;

        [FindsBy(How = How.Name, Using = "NumberOfSessions")]
        private IWebElement _SessionsMissed;

        [FindsBy(How = How.Name, Using = "Note")]
        private IWebElement _noteTextArea;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='ExclusionStatus']")]
        private IWebElement _statusTable;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='SuspensionsMeetings']")]
        private IWebElement _meetingTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_meeting_button']")]
        private IWebElement _addMeetingButton;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='ExclusionNoteDocuments']")]
        private IWebElement _noteDocumentTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _OKButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_suspension_status_button']")]
        private IWebElement _addStatusButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_document_button']")]
        private IWebElement _addDocumentNoteButton;




        #endregion

        #region Page Properties Setters Getters

        public string Type
        {
            set
            {
                _typeDropdown.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
        }

        public string Reason
        {
            set { _reasonDropdown.EnterForDropDown(value); }
        }

        public string StartDate
        {
            set { _startDateTextBox.SetDateTime(value); }
            get { return _startDateTextBox.GetDateTime(); }
        }

        public string EndDate
        {
            set { _endDateTextBox.SetDateTime(value); }
            get { return _endDateTextBox.GetDateTime(); }
        }

        public string StartTime
        {
            set { _startTimeTextBox.SetText(value); }
            get { return _startTimeTextBox.GetDateTime(true, true); }
        }

        public string EndTime
        {
            set { _endTimeTextBox.SetText(value); }
            get {return _endTimeTextBox.GetDateTime(true, true); }
        }

        public string Length
        {
            set { _LengthTextBox.SetText(value); }
            get { return _LengthTextBox.GetValue(); }
        }

        public string SessionsMissed
        {
            set { _SessionsMissed.SetText(value); }
            get { return _SessionsMissed.GetValue(); }
        }

        public string Note
        {
            set { _noteTextArea.SetText(value); }
        }

        public GridComponent<StatusItem> StatusGrid
        {
            get
            {
                GridComponent<StatusItem> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StatusItem>(By.CssSelector("[data-automation-id='add_exclusion_status_button']"));
                });
                return returnValue;
            }
        }

        public override By ComponentIdentifier
        {
            get
            {
                return SimsBy.AutomationId("pupil_exclusions_detail");
            }
        }

        #endregion


        #region Exclusion Status
        public class StatusItem
        {
           
            [FindsBy(How = How.CssSelector, Using = "[id $= 'ExclusionStatus_dropdownImitator']")]
            private IWebElement _statusDropdown;

            [FindsBy(How = How.CssSelector, Using = "[name$='.ChangeDate']")]
            private IWebElement _dateTextbox;

            [FindsBy(How = How.CssSelector, Using = "[id $='ChangeReason_dropdownImitator']")]
            private IWebElement _reasonDropdown;

            public string Status
            {
                set { _statusDropdown.EnterForDropDown(value); }
                get { return _statusDropdown.GetValue(); }
            }

            public string Date
            {
                set { _dateTextbox.SetDateTime(value); }
                get { return _dateTextbox.GetDateTime(); }
            }

            public string Reason
            {
                set { _reasonDropdown.EnterForDropDown(value); }
                get { return _reasonDropdown.GetValue(); }
            }

            

        }

        #endregion


        #region Exclusion Meetings

        public class Meeting
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='.ExclusionTypeDisplay']")]
            private IWebElement _typeDropdown;


            [FindsBy(How = How.CssSelector, Using = "[name$='.StartDateOnly']")]
            private IWebElement _startDateTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.StartTimeDisplay']")]
            private IWebElement _startTimeTextbox;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'edit..._button']")]
            private IWebElement _editButton;

            public string Type
            {
                get { return _typeDropdown.GetValue(); }
            }

            public string StartDate
            {
                get { return _startDateTextbox.GetDateTime(); }
            }


            public string StartTime
            {
                get { return _startTimeTextbox.GetValue(); }
            }        
          

            public MeetingDialog EditMeeting()
            {
                _editButton.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                return new MeetingDialog();
            }
        }

        #endregion


        #region Exclusion document

        public class NoteDocument
        {
           
            [FindsBy(How = How.CssSelector, Using = "[name$='.Summary']")]
            private IWebElement _summaryTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Notes']")]
            private IWebElement _noteTextbox;
            public string Summary
            {
                set { _summaryTextbox.SetText(value); }
                get { return _summaryTextbox.GetValue(); }
            }

            public string Note
            {
                set { _noteTextbox.SetText(value); }
                get { return _noteTextbox.GetValue(); }
            }

           
        }


        #endregion


        #region Page Actions
        public void AddSuspensionStatus()
        {
            Action clickAction = () => _addStatusButton.Click();
            ClickWhenElementIsReady(clickAction, By.CssSelector("[data-automation-id='add_suspension_status_button']"));
        }

        private void ClickWhenElementIsReady(Action action, By by)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException ex)
            {
                Console.WriteLine("Element is noy yet ready to be clicked. Please wait and try again later.", ex);
                Wait.WaitUntilEnabled(by);
                Wait.WaitForDocumentReady();
                action();
            }
            finally
            {
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
        }

        #endregion

    }
}
