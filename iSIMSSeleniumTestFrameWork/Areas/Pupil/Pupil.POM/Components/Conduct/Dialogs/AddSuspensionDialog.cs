using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Reflection;

namespace POM.Components.Conduct
{
    public class AddSuspensionDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("pupil_exclusion_detail"); }
        }

        #region Properties

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
        }

        public string EndDate
        {
            set { _endDateTextBox.SetDateTime(value); }
            get { return _endDateTextBox.GetDateTime(); }
        }

        public string StartTime
        {
            set { _startTimeTextBox.SetText(value); }
        }

        public string EndTime
        {
            set { _endTimeTextBox.SetText(value); }
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
                    returnValue = new GridComponent<StatusItem>(By.CssSelector("[data-maintenance-container='ExclusionStatus']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<Meeting> MeetingGrid
        {
            get
            {
                GridComponent<Meeting> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Meeting>(By.CssSelector("[data-maintenance-container='SuspensionsMeetings']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<NoteDocument> NoteDocumentGrid
        {
            get
            {
                GridComponent<NoteDocument> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<NoteDocument>(By.CssSelector("[data-maintenance-container='ExclusionNoteDocuments']"), DialogIdentifier);
                });
                return returnValue;
            }
        }


        public class StatusItem
        {
            #region Properties
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

            #endregion

        }

        public class Meeting
        {
            #region Properties
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
            #endregion

            #region Action

            public MeetingDialog EditMeeting()
            {
                _editButton.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                return new MeetingDialog();
            }

            #endregion

        }

        public class NoteDocument
        {
            #region Properties
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

            #endregion
        }

        #endregion

        #region Actions

        public SuspensionRecordPage SaveValue()
        {
            _OKButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new SuspensionRecordPage();
        }

        public SuspensionRecordPage Cancel()
        {
            _cancelButton.Click();
            return new SuspensionRecordPage();
        }

        public MeetingDialog AddMeeting()
        {
            _addMeetingButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new MeetingDialog();
        }


        public void  AddSuspensionStatus()
        {
            Action clickAction = () => _addStatusButton.Click();
            ClickWhenElementIsReady(clickAction, By.CssSelector("[data-automation-id='add_suspension_status_button']"));
        }

        public void AddSuspensionDocumentNote()
        {
            Action clickAction = () => _addDocumentNoteButton.Click();
            ClickWhenElementIsReady(clickAction, By.CssSelector("[data-automation-id='add_document_button']"));

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


        public bool IsAllElementDisable()
        {
            try
            {
                return (_typeDropdown.GetAttribute("disabled").Equals("true") && _reasonDropdown.GetAttribute("disabled").Equals("true")
                && _startDateTextBox.GetAttribute("readonly").Equals("true") && _endDateTextBox.GetAttribute("readonly").Equals("true")
                && _startTimeTextBox.GetAttribute("readonly").Equals("true") && _endTimeTextBox.GetAttribute("readonly").Equals("true")
                && _LengthTextBox.GetAttribute("readonly").Equals("true") && _noteTextArea.GetAttribute("readonly").Equals("true"));
            }
            catch (StaleElementReferenceException e) { return false; }

        }

        #endregion


    }
}
