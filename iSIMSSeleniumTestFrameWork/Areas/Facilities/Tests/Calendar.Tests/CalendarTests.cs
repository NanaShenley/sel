using Selene.Support.Attributes;
using TestSettings;
using POM.Helper;
using SeSugar.Automation;
using POM.Components.Calendar;
using System;
using NUnit.Framework;
using SeSugar;
using SeSugar.Data;
using Facilities.Data;
using Facilities.POM.Components.Calendar.Dialogs;

namespace Calendar.Tests
{
    public class CalendarTests
    {

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] {"P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Calendar_NonRecurring_Event01()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> School Management-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
            Wait.WaitForDocumentReady();
            DataPackage dataPackage = this.BuildDataPackage();
            //Calendar Data Injection
            var calendarId = Guid.NewGuid();
            var calendarName = Utilities.GenerateRandomString(3, "SNADD");
            var calendarDescription = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddCalendar(calendarId, calendarName, calendarDescription);
            //CalendarEvent new Data
            var newEventName = Utilities.GenerateRandomString(3, "SNADD");
            using (new DataSetup(purgeBeforeInsert: true, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps
                //Add Event
                CalendarDetailPage calendar = new CalendarDetailPage();
                AddEventDialog eventDialog = calendar.AddEvent();
                eventDialog.EventName = "Test_" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
                eventDialog.StartDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.EndDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.Calendar = "Generic event";
                string eventId = eventDialog.EventDialogId;
                eventDialog.ClickOk();
                Wait.WaitForDocumentReady();
                bool isValidationWarningExsist = SeleniumHelper.DoesWebElementExist(AddEventDialog.ValidationWarning);
                Assert.IsFalse(isValidationWarningExsist, "Validation Warning");

                // Need to add the data automation ID for School Event
                // Navigate to Tasks-> School Management-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //Wait.WaitForDocumentReady();
                //string automationID = "Event_" + eventId;
                //bool isEventTileExsist = SeleniumHelper.DoesWebElementExist(calendar.EventTileIdentifier(automationID));
                //Assert.IsTrue(isEventTileExsist, "Event not added on the Grid");
                ////Edit the Event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover1 = new EventPopoverCard();
                //eventDialog = eventPopover1.EditEvent();
                //eventDialog.EventName = newEventName;
                //eventDialog.ClickOk();
                //// Navigate to Tasks-> School Management-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //Wait.WaitForDocumentReady();
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover2 = new EventPopoverCard();
                //eventDialog = eventPopover2.EditEvent();
                //Assert.AreEqual(newEventName, eventDialog.EventName, "Event Edit Unsuccessful");
                //eventDialog.ClickCancel();
                ////Delete the event                
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover3 = new EventPopoverCard();
                //ConfirmDeleteDialog deleteDialog = eventPopover3.EventDeleteConfirmation();
                //deleteDialog.ContinueWithDelete();
                #endregion
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Calendar_NonRecurring_AllDay_Event02()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> School Management-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
            Wait.WaitForDocumentReady();
            DataPackage dataPackage = this.BuildDataPackage();
            //Calendar Data Injection
            var calendarId = Guid.NewGuid();
            var calendarName = Utilities.GenerateRandomString(3, "SNADD");
            var calendarDescription = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddCalendar(calendarId, calendarName, calendarDescription);
            //CalendarEvent new Data
            var newEventName = Utilities.GenerateRandomString(3, "SNADD");
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps
                CalendarDetailPage calendar = new CalendarDetailPage();
                AddEventDialog eventDialog = calendar.AddEvent();
                eventDialog.EventName = "Test_" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
                eventDialog.StartDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.EndDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.IsAllDay = true;
                eventDialog.Calendar = "Generic event";
                string eventId = eventDialog.EventDialogId;
                eventDialog.ClickOk();
                Wait.WaitForDocumentReady();
                //bool isValidationWarningExsist = SeleniumHelper.DoesWebElementExist(AddEventDialog.ValidationWarning);
                //Assert.IsFalse(isValidationWarningExsist, "Validation Warning");
                //// Navigate to Tasks-> School Management-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //Wait.WaitForDocumentReady();
                //string automationID = "Event_" + eventId;
                //bool isEventTileExsist = SeleniumHelper.DoesWebElementExist(calendar.EventTileIdentifier(automationID));
                //Assert.IsTrue(isEventTileExsist, "Event not added on the Grid");
                ////Edit the Event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover1 = new EventPopoverCard();
                //eventDialog = eventPopover1.EditEvent();
                //eventDialog.EventName = newEventName;
                //eventDialog.ClickOk();
                //// Navigate to Tasks-> School Management-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //Wait.WaitForDocumentReady();
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover2 = new EventPopoverCard();
                //eventDialog = eventPopover2.EditEvent();
                //Assert.AreEqual(newEventName, eventDialog.EventName, "Event Edit Unsuccessful");
                //eventDialog.ClickCancel();
                ////Delete the event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover3 = new EventPopoverCard();
                //ConfirmDeleteDialog deleteDialog = eventPopover3.EventDeleteConfirmation();
                //deleteDialog.ContinueWithDelete();
                #endregion
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Calendar_Daily_Recurring_Event_With_EndsOnDate03()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> School Management-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
            Wait.WaitForDocumentReady();
            DataPackage dataPackage = this.BuildDataPackage();
            //Calendar Data Injection
            var calendarId = Guid.NewGuid();
            var calendarName = Utilities.GenerateRandomString(3, "SNADD");
            var calendarDescription = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddCalendar(calendarId, calendarName, calendarDescription);
            //CalendarEvent new Data
            var newEventName = Utilities.GenerateRandomString(3, "SNADD");
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps
                CalendarDetailPage calendar = new CalendarDetailPage();
                AddEventDialog eventDialog = calendar.AddEvent();
                eventDialog.EventName = "Test_" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
                eventDialog.StartDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.EndDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.Calendar = "Generic event";
                //eventDialog.ClickRepeatPattern();
                eventDialog.ClickDaily();
                eventDialog.Interval = "1";
                eventDialog.IsEndsOn = true;
                eventDialog.EndsOn = DateTime.Now.AddDays(2).ToString("M/d/yyyy");
                string eventId = eventDialog.EventDialogId;
                eventDialog.ClickOk();
                Wait.WaitForDocumentReady();
                //bool isValidationWarningExsist = SeleniumHelper.DoesWebElementExist(AddEventDialog.ValidationWarning);
                //Assert.IsFalse(isValidationWarningExsist, "Validation Warning");

                //// Navigate to Tasks-> School Management-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //string automationID = "Event_" + eventId;
                //bool isEventTileExsist = SeleniumHelper.DoesWebElementExist(calendar.EventTileIdentifier(automationID));
                //Assert.IsTrue(isEventTileExsist, "Event not added on the Grid");

                ////Edit the Event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover1 = new EventPopoverCard();
                //eventDialog = eventPopover1.EditEvent();
                //eventDialog.EventName = newEventName;
                //eventDialog.ClickOk();
                //// Navigate to Tasks-> School Management-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover2 = new EventPopoverCard();
                //eventDialog = eventPopover2.EditEvent();
                //Assert.AreEqual(newEventName, eventDialog.EventName, "Event Edit Unsuccessful");
                //eventDialog.ClickCancel();

                ////Delete the event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover3 = new EventPopoverCard();
                //ConfirmDeleteDialog deleteDialog = eventPopover3.EventDeleteConfirmation();
                //deleteDialog.DeleteEventSeries = true;
                //deleteDialog.ContinueWithDelete();
                #endregion
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Calendar_Daily_Recurring_Event_With_NumberOfOccurrences04()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> School Management-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
            Wait.WaitForDocumentReady();
            DataPackage dataPackage = this.BuildDataPackage();
            //Calendar Data Injection
            var calendarId = Guid.NewGuid();
            var calendarName = Utilities.GenerateRandomString(3, "SNADD");
            var calendarDescription = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddCalendar(calendarId, calendarName, calendarDescription);
            //CalendarEvent new Data
            var newEventName = Utilities.GenerateRandomString(3, "SNADD");

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps
                CalendarDetailPage calendar = new CalendarDetailPage();
                AddEventDialog eventDialog = calendar.AddEvent();
                eventDialog.EventName = "Test_" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
                eventDialog.StartDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.EndDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.Calendar = "Generic event";
                eventDialog.ClickDaily();
                eventDialog.Interval = "1";
                eventDialog.ClickRepeatingAfter();
                //eventDialog.IsNumberOfOccurences = true;
                eventDialog.NumberOfOccurrences = "5";
                //string eventId = eventDialog.EventDialogId;
                eventDialog.ClickOk();
                //bool isValidationWarningExsist = SeleniumHelper.DoesWebElementExist(AddEventDialog.ValidationWarning);
                //Assert.IsFalse(isValidationWarningExsist, "Validation Warning");
                //// Navigate to Tasks-> School Management-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //Wait.WaitForDocumentReady();
                //string automationID = "Event_" + eventId;
                //bool isEventTileExsist = SeleniumHelper.DoesWebElementExist(calendar.EventTileIdentifier(automationID));
                //Assert.IsTrue(isEventTileExsist, "Event not added on the Grid");

                ////Edit the Event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover1 = new EventPopoverCard();
                //eventDialog = eventPopover1.EditEvent();
                //eventDialog.EventName = newEventName;
                //eventDialog.ClickOk();
                //// Navigate to Tasks-> School Management-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover2 = new EventPopoverCard();
                //eventDialog = eventPopover2.EditEvent();
                //Assert.AreEqual(newEventName, eventDialog.EventName, "Event Edit Unsuccessful");
                //eventDialog.ClickCancel();
                ////Delete the event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover3 = new EventPopoverCard();
                //ConfirmDeleteDialog deleteDialog = eventPopover3.EventDeleteConfirmation();
                //deleteDialog.DeleteEventSeries = true;
                //deleteDialog.ContinueWithDelete();
                #endregion
            }

        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Calendar_Weekly_Recurring_Event_With_EndsOnDate05()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> School Management-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
            Wait.WaitForDocumentReady();
            DataPackage dataPackage = this.BuildDataPackage();
            //Calendar Data Injection
            var calendarId = Guid.NewGuid();
            var calendarName = Utilities.GenerateRandomString(3, "SNADD");
            var calendarDescription = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddCalendar(calendarId, calendarName, calendarDescription);
            //CalendarEvent new Data
            var newEventName = Utilities.GenerateRandomString(3, "SNADD");
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps
                CalendarDetailPage calendar = new CalendarDetailPage();
                AddEventDialog eventDialog = calendar.AddEvent();
                eventDialog.EventName = "Test_" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
                eventDialog.StartDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.EndDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.Calendar = "Generic event";
                eventDialog.ClickWeekly();
                eventDialog.Interval = "1";
                eventDialog.IsEndsOn = true;
                eventDialog.EndsOn = DateTime.Now.AddDays(14).ToString("M/d/yyyy");
                string eventId = eventDialog.EventDialogId;
                eventDialog.ClickOk();
                Wait.WaitForDocumentReady();
                bool isValidationWarningExsist = SeleniumHelper.DoesWebElementExist(AddEventDialog.ValidationWarning);
                Assert.IsFalse(isValidationWarningExsist, "Validation Warning");
                //// Navigate to Tasks-> Calendar-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //Wait.WaitForDocumentReady();
                //string automationID = "Event_" + eventId;
                //bool isEventTileExsist = SeleniumHelper.DoesWebElementExist(calendar.EventTileIdentifier(automationID));
                //Assert.IsTrue(isEventTileExsist, "Event not added on the Grid");

                ////Edit the Event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover1 = new EventPopoverCard();
                //eventDialog = eventPopover1.EditEvent();
                //eventDialog.EventName = newEventName;
                //eventDialog.ClickOk();
                //// Navigate to Tasks-> Calendar-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover2 = new EventPopoverCard();
                //eventDialog = eventPopover2.EditEvent();
                //Assert.AreEqual(newEventName, eventDialog.EventName, "Event Edit Unsuccessful");
                //eventDialog.ClickCancel();

                ////Delete the event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover3 = new EventPopoverCard();
                //ConfirmDeleteDialog deleteDialog = eventPopover3.EventDeleteConfirmation();
                //deleteDialog.DeleteEventSeries = true;
                //deleteDialog.ContinueWithDelete();
                #endregion
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Calendar_Weekly_Recurring_Event_With_NumberOfOccurrences06()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> School Management-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
            Wait.WaitForDocumentReady();

            DataPackage dataPackage = this.BuildDataPackage();

            //Calendar Data Injection
            var calendarId = Guid.NewGuid();
            var calendarName = Utilities.GenerateRandomString(3, "SNADD");
            var calendarDescription = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddCalendar(calendarId, calendarName, calendarDescription);
            //CalendarEvent new Data
            var newEventName = Utilities.GenerateRandomString(3, "SNADD");
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps
                CalendarDetailPage calendar = new CalendarDetailPage();
                AddEventDialog eventDialog = calendar.AddEvent();
                eventDialog.EventName = "Test_" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
                eventDialog.StartDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.EndDate = DateTime.Now.ToString("M/d/yyyy");
                eventDialog.Calendar = "Generic event";
                eventDialog.ClickWeekly();
                eventDialog.ClickRepeatingAfter();
                eventDialog.Interval = "1";
                eventDialog.IsNumberOfOccurences = true;
                eventDialog.NumberOfOccurrences = "5";
                string eventId = eventDialog.EventDialogId;
                eventDialog.ClickOk();
                Wait.WaitForDocumentReady();
                bool isValidationWarningExsist = SeleniumHelper.DoesWebElementExist(AddEventDialog.ValidationWarning);
                Assert.IsFalse(isValidationWarningExsist, "Validation Warning");
                // Navigate to Tasks-> School Management-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //Wait.WaitForDocumentReady();
                //string automationID = "Event_" + eventId;
                //bool isEventTileExsist = SeleniumHelper.DoesWebElementExist(calendar.EventTileIdentifier(automationID));
                //Assert.IsTrue(isEventTileExsist, "Event not added on the Grid");
                ////Edit the Event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover1 = new EventPopoverCard();
                //eventDialog = eventPopover1.EditEvent();
                //eventDialog.EventName = newEventName;
                //eventDialog.ClickOk();
                //// Navigate to Tasks-> School Management-> School Calendar
                //AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
                //Wait.WaitForDocumentReady();
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover2 = new EventPopoverCard();
                //eventDialog = eventPopover2.EditEvent();
                //Assert.AreEqual(newEventName, eventDialog.EventName, "Event Edit Unsuccessful");
                //eventDialog.ClickCancel();
                ////Delete the event
                //SeleniumHelper.FindAndClick(calendar.EventTileIdentifier(automationID));
                //EventPopoverCard eventPopover3 = new EventPopoverCard();
                //ConfirmDeleteDialog deleteDialog = eventPopover3.EventDeleteConfirmation();
                //deleteDialog.DeleteEventSeries = true;
                //deleteDialog.ContinueWithDelete();
                #endregion
            }
        }
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P3" }, Browsers = new[] { BrowserDefaults.Chrome })]
     
        #region EventNameNullCheck
        public void EventNameNullCheck()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> School Management-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
            Wait.WaitForDocumentReady();
            DataPackage dataPackage = this.BuildDataPackage();
            //Calendar Data Injection
            var calendarId = Guid.NewGuid();
            var calendarName = Utilities.GenerateRandomString(3, "SNADD");
            var calendarDescription = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddCalendar(calendarId, calendarName, calendarDescription);
            //CalendarEvent new Data
            var newEventName = Utilities.GenerateRandomString(3, "SNADD");
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps
                CalendarDetailPage calendar = new CalendarDetailPage();
                AddEventDialog eventDialog = calendar.AddEvent();
                eventDialog.Calendar = calendarName;
                eventDialog.ClickOk();
                bool isValidationWarningExsist = SeleniumHelper.DoesWebElementExist(AddEventDialog.ValidationWarning);
                Assert.IsTrue(isValidationWarningExsist, "Validation Warning");
            }
            #endregion
        }
        #endregion

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P3" }, Browsers = new[] { BrowserDefaults.Chrome })]

        #region StartDateNullCheck
        public void StartDateNullCheck()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> School Management-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
            Wait.WaitForDocumentReady();
            DataPackage dataPackage = this.BuildDataPackage();
            //Calendar Data Injection
            var calendarId = Guid.NewGuid();
            var calendarName = Utilities.GenerateRandomString(3, "SNADD");
            var calendarDescription = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddCalendar(calendarId, calendarName, calendarDescription);
            //CalendarEvent new Data
            var newEventName = Utilities.GenerateRandomString(3, "SNADD");
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps
                CalendarDetailPage calendar = new CalendarDetailPage();
                AddEventDialog eventDialog = calendar.AddEvent();
                eventDialog.EventName = "Test_" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
                eventDialog.Clearstartdate();
                eventDialog.Calendar = calendarName;
                eventDialog.ClickOk();
                bool isValidationWarningExsist = SeleniumHelper.DoesWebElementExist(AddEventDialog.ValidationWarning);
                Assert.IsTrue(isValidationWarningExsist, "Validation Warning");
            }
            #endregion
        }
        #endregion

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P3" }, Browsers = new[] { BrowserDefaults.Chrome })]

        #region EndDateNullCheck
        public void EndDateNullCheck()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> School Management-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
            Wait.WaitForDocumentReady();
            DataPackage dataPackage = this.BuildDataPackage();
            //Calendar Data Injection
            var calendarId = Guid.NewGuid();
            var calendarName = Utilities.GenerateRandomString(3, "SNADD");
            var calendarDescription = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddCalendar(calendarId, calendarName, calendarDescription);
            //CalendarEvent new Data
            var newEventName = Utilities.GenerateRandomString(3, "SNADD");
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps
                CalendarDetailPage calendar = new CalendarDetailPage();
                AddEventDialog eventDialog = calendar.AddEvent();
                eventDialog.EventName = "Test_" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
                eventDialog.Clearenddate();
                eventDialog.Calendar = calendarName;
                eventDialog.ClickOk();
                bool isValidationWarningExsist = SeleniumHelper.DoesWebElementExist(AddEventDialog.ValidationWarning);
                Assert.IsTrue(isValidationWarningExsist, "Validation Warning");
            }
            #endregion
        }
        #endregion

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]

        #region CalendarNullCheck
        public void CalendarNullCheck()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> School Management-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "School Calendar");
            Wait.WaitForDocumentReady();
            DataPackage dataPackage = this.BuildDataPackage();
            //Calendar Data Injection
            var calendarId = Guid.NewGuid();
            var calendarName = Utilities.GenerateRandomString(3, "SNADD");
            var calendarDescription = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddCalendar(calendarId, calendarName, calendarDescription);
            //CalendarEvent new Data
            var newEventName = Utilities.GenerateRandomString(3, "SNADD");
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                #region Steps
                CalendarDetailPage calendar = new CalendarDetailPage();
                AddEventDialog eventDialog = calendar.AddEvent();
                eventDialog.EventName = "Test_" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
                eventDialog.ClickOk();
                bool isValidationWarningExsist = SeleniumHelper.DoesWebElementExist(AddEventDialog.ValidationWarning);
                Assert.IsTrue(isValidationWarningExsist, "Validation Warning");
            }
            #endregion
        }
        #endregion

    }
}
