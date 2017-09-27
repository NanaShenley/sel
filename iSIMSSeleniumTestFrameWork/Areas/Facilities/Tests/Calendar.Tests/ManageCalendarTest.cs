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
using Facilities.POM.Components.Calendar.Page;
using System.Collections.Generic;
using System.Linq;

namespace Calendar.Tests
{
    public class ManageCalendarTest
    {
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Active_Calendar01()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> Calendar-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Manage Calendars");
            Wait.WaitForDocumentReady();

            var manageCalendarTriplet = new ManageCalendarTriplet();
            var manageCalendarPage = manageCalendarTriplet.Create();
            manageCalendarPage.CalendarName = "Calendar_" + SeleniumHelper.GenerateRandomString(10);
            manageCalendarPage.CalendarDescription = "CalendarDescription_" + SeleniumHelper.GenerateRandomString(20);
            manageCalendarPage.Save();
            Assert.AreEqual(false, manageCalendarPage.IsSuccessMessageDisplayed(), "Calendar Record Saved");
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_InActive_Calendar02()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> Calendar-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Manage Calendars");
            Wait.WaitForDocumentReady();

            var manageCalendarTriplet = new ManageCalendarTriplet();
            var manageCalendarPage = manageCalendarTriplet.Create();
            manageCalendarPage.CalendarName = "Calendar_" + SeleniumHelper.GenerateRandomString(10);
            manageCalendarPage.CalendarDescription = "CalendarDescription_" + SeleniumHelper.GenerateRandomString(20);
            manageCalendarPage.IsActive = false;
            manageCalendarPage.Save();
            Assert.AreEqual(false, manageCalendarPage.IsSuccessMessageDisplayed(), "Calendar Record Saved");
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Search_Active_Calendar03()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> Calendar-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Manage Calendars");
            Wait.WaitForDocumentReady();

            var manageCalendarTriplet = new ManageCalendarTriplet();
            manageCalendarTriplet.SearchCriteria.SearchBySchemeName = ("");
            var searchResult = manageCalendarTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageCalendarPage = searchResult.Click<ManageCalendarDetailPage>();
        }

       [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Search_Include_InActive_Calendar04()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> Calendar-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Manage Calendars");
            Wait.WaitForDocumentReady();

            var manageCalendarTriplet = new ManageCalendarTriplet();
            manageCalendarTriplet.SearchCriteria.SearchBySchemeName = ("");
            manageCalendarTriplet.SearchCriteria.InactiveCalendarSearch = true;
            var searchResult = manageCalendarTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageCalendarPage = searchResult.Click<ManageCalendarDetailPage>();
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Delete_Calendar05()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> Calendar-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Manage Calendars");
            Wait.WaitForDocumentReady();

            var manageCalendarTriplet = new ManageCalendarTriplet();
            manageCalendarTriplet.SearchCriteria.SearchBySchemeName = ("");
            var searchResult = manageCalendarTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageCalendarPage = searchResult.Click<ManageCalendarDetailPage>();
            manageCalendarTriplet.Delete();
        }

       [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Manage_Calendar_Name_Validation06()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> Calendar-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Manage Calendars");
            Wait.WaitForDocumentReady();

            var manageCalendarTriplet = new ManageCalendarTriplet();
            var manageCalendarPage = manageCalendarTriplet.Create();
            manageCalendarPage.CalendarName = "";
            manageCalendarPage.CalendarDescription = "CalendarDescription_" + SeleniumHelper.GenerateRandomString(20);
            manageCalendarPage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageCalendarDetailPage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }

       [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Manage_Calendar_Description_Validation07()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> Calendar-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Manage Calendars");
            Wait.WaitForDocumentReady();

            var manageCalendarTriplet = new ManageCalendarTriplet();
            var manageCalendarPage = manageCalendarTriplet.Create();
            manageCalendarPage.CalendarName = "Calendar_" + SeleniumHelper.GenerateRandomString(5);
            manageCalendarPage.CalendarDescription = "";
            manageCalendarPage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageCalendarDetailPage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }

      [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Manage_Calendar_Calendar_Group_Validation08()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks-> Calendar-> School Calendar
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Manage Calendars");
            Wait.WaitForDocumentReady();

            var manageCalendarTriplet = new ManageCalendarTriplet();
            var manageCalendarPage = manageCalendarTriplet.Create();
            manageCalendarPage.CalendarName = "Calendar_" + SeleniumHelper.GenerateRandomString(5);
            manageCalendarPage.CalendarDescription = "CalendarDescription_" + SeleniumHelper.GenerateRandomString(20);
            manageCalendarPage.CalendarGroup = null;
            manageCalendarPage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageCalendarDetailPage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }



    }

}
