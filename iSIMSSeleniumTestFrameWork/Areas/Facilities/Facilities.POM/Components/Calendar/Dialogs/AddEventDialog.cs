using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using OpenQA.Selenium.Support.UI;

namespace POM.Components.Calendar
{
    public class AddEventDialog: BaseDialogComponent
    {
        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");

        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "Calendar.dropdownImitator")]
        private IWebElement _calendarDropdown;

        [FindsBy(How = How.Name, Using = "EventName")]
        private IWebElement _eventNameTextBox;

        [FindsBy(How = How.Name, Using = "StartDateTime")]
        private IWebElement _startDateTimeTextBox;

        [FindsBy(How = How.Name, Using = "EndDateTime")]
        private IWebElement _endDateTimeTextBox;

        [FindsBy(How = How.Name, Using = "StartTime")]
        private IWebElement _startTimeTextBox;

        [FindsBy(How = How.Name, Using = "EndTime")]
        private IWebElement _endTimeTextBox;

        [FindsBy(How = How.Name, Using = "AllDay")]
        private IWebElement _allDay;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;
        
        [FindsBy(How = How.Name, Using = "DaysInterval")]
        private IWebElement _interval;

        [FindsBy(How = How.Name, Using = "RangeEndDate")]
        private IWebElement _endsOn;

        [FindsBy(How = How.Name, Using = "Occurrences")]
        private IWebElement _numberOfOccurrences;
        
        [FindsBy(How = How.CssSelector, Using = "[name='RepeatPattern'][value='Daily']")]
        private IWebElement _dailyButton;

        [FindsBy(How = How.CssSelector, Using = "[name='RepeatPattern'][value='Weekly']")]
        private IWebElement _weeklyButton;

        [FindsBy(How = How.Name, Using = "RepeatPattern")]
        private IWebElement _repeatpatternDropdown;
        [FindsBy(How = How.Name, Using = "RecurrenceRange")]
        private IWebElement _stoprepeatingafterDropdown;

        [FindsBy(How = How.Id, Using = "dialog-detailID")]
        private IWebElement _eventDialogId;

        #endregion

        #region Actions
        public string EventName
        {
            set { _eventNameTextBox.SetText(value); }
            get { return _eventNameTextBox.GetValue(); }
        }

        public string StartDate
        {
            get { return _startDateTimeTextBox.GetValue(); }
            set { _startDateTimeTextBox.SetDateTime(value); }
        }
        public void Clearstartdate()
        {
            _startDateTimeTextBox.Clear();
        }
        public string EndDate
        {
            get { return _endDateTimeTextBox.GetValue(); }
            set { _endDateTimeTextBox.SetDateTime(value); }
        }
        public void Clearenddate()
        {
            _endDateTimeTextBox.Clear();
        }

        public string Calendar
        {
            get { return _calendarDropdown.GetValue(); }
            set { _calendarDropdown.EnterForDropDown(value); }
        }

        public bool IsAllDay
        {
            set { _allDay.Set(value); }
            get { return _allDay.IsChecked(); }
        }

        public bool IsEndsOn
        {
            set
            {
                string javascript = "document.getElementsByName('RecurrenceRange')[0].checked = '{0}';";
                SeleniumHelper.ExecuteJavascript(String.Format(javascript, value));
            }
        }

        public bool IsNumberOfOccurences
        {
            set
            {
                string javascript = "document.getElementsByName('RecurrenceRange')[1].checked = '{0}';";
                SeleniumHelper.ExecuteJavascript(String.Format(javascript, value));
            }
        }

        public string EndsOn
        {
            get { return _endsOn.GetValue(); }
            set { _endsOn.SetDateTime(value); }
        }
        
        public string Interval
        {
            get { return _interval.GetValue(); }
            set { _interval.SetText(value); }
        }
        public string NumberOfOccurrences
        {
            get { return _numberOfOccurrences.GetValue(); }
            set { _numberOfOccurrences.SetText(value); }
        }
        //public virtual void ClickRepeatPattern(int sleep = 2)
        //{
        //    SelectElement dropdown = new SelectElement(_repeatpatternDropdown);
        //    dropdown.SelectByIndex(1);
        //    Wait.WaitForDocumentReady();
        //    SeleniumHelper.Sleep(sleep);
        //}
        public virtual void ClickDaily(int sleep = 2)
        {
            //_dailyButton.Click();
            SelectElement dropdown = new SelectElement(_repeatpatternDropdown);
            dropdown.SelectByIndex(1);
            Wait.WaitForDocumentReady();       
        }
        public virtual void ClickRepeatingAfter(int sleep = 2)
        {
            //_dailyButton.Click();
            SelectElement dropdown = new SelectElement(_stoprepeatingafterDropdown);
            dropdown.SelectByIndex(0);
            Wait.WaitForDocumentReady();           
        }
      public virtual void ClickWeekly(int sleep = 2)
        {
            //_weeklyButton.ClickByJS();
            SelectElement dropdown = new SelectElement(_repeatpatternDropdown);
            dropdown.SelectByIndex(2);
            Wait.WaitForDocumentReady();        
        }

        public string EventDialogId
        {
           get { return SeleniumHelper.ExecuteJavascript("return document.getElementById('dialog-detailID').getAttribute('value');"); }
       }

        #endregion
    }
}
