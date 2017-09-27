using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Threading;


namespace POM.Components.Conduct
{
    public class MeetingDialog : BaseDialogComponent
    {

        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("pupil_exclusion_meeting_detail"); }
        }

        #region Propertise

        [FindsBy(How = How.Name, Using = "ExclusionMeetingType.dropdownImitator")]
        private IWebElement _typeDropdown;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextbox;

        [FindsBy(How = How.Name, Using = "StartTime")]
        private IWebElement _startTimeTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _OKButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        public string Type
        {
            set { _typeDropdown.EnterForDropDown(value); }
        }

        public string StartDate
        {
            set { _startDateTextbox.SetDateTime(value); }
        }

        public string StartTime
        {
            set { _startTimeTextbox.SetText(value); }
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


        #region Action

        public AddSuspensionDialog SaveValue()
        {
            _OKButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Thread.Sleep(5000);
            return new AddSuspensionDialog();
        }


        #endregion


    }
}
