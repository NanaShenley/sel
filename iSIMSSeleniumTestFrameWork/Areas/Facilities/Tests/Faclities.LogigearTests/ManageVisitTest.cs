using System;
using POM.Helper;
using NUnit.Framework;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;
using Facilities.POM.Components.Visitor_Book;

namespace Faclities.LogigearTests
{
    public class ManageVisitTest
    {
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Visit_nonallday()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Visitor Book Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Visitors Book");
            Wait.WaitForDocumentReady();

            var visitTriplet = new ManageVisitTriplet();
            var visitDetailPage = visitTriplet.Create();
            visitDetailPage.VisitorName = "Visitor_" + SeleniumHelper.GenerateRandomString(10);
            visitDetailPage.VisitNote = "VisitNoteDescription_" + SeleniumHelper.GenerateRandomString(20);
            visitDetailPage.StartDate = DateTime.Now.ToString("M/d/yyyy");
            //visitDetailPage.StartTime = DateTime.Now.ToString("h:mm tt");
            //visitDetailPage.EndTime = DateTime.Now.AddHours(1).ToString("h:mm tt");
            visitDetailPage.Save();
            Assert.AreEqual(false, visitDetailPage.IsSuccessMessageDisplayed(), "Visit record saved");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Visit_allday()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Visitor Book Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Visitors Book");
            Wait.WaitForDocumentReady();
            //Adding a Visit
            var visitTriplet = new ManageVisitTriplet();
            var visitDetailPage = visitTriplet.Create();
            visitDetailPage.VisitorName = "Visitor_" + SeleniumHelper.GenerateRandomString(10);
            visitDetailPage.VisitNote = "VisitNoteDescription_" + SeleniumHelper.GenerateRandomString(20);
            visitDetailPage.StartDate = DateTime.Now.ToString("M/d/yyyy");
            visitDetailPage.IsAllday = true;
            //Saving
            visitDetailPage.Save();
            Assert.AreEqual(false, visitDetailPage.IsSuccessMessageDisplayed(), "Visit record saved");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Search_Visit()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Visitor Book Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Visitors Book");
            Wait.WaitForDocumentReady();
            //Search Visit
            var visitTriplet = new ManageVisitTriplet();
            visitTriplet.SearchCriteria.SearchByVisitorName = ("Visitor_");
            var searchResult = visitTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageVisitPage = searchResult.Click<ManageVisitDetail>();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Delete_Visit()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Visitor Book Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Visitors Book");
            Wait.WaitForDocumentReady();
            //Search Visit
            var visitTriplet = new ManageVisitTriplet();
            visitTriplet.SearchCriteria.SearchByVisitorName = ("Visitor_");
            var searchResult = visitTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageVisitPage = searchResult.Click<ManageVisitDetail>();
            //Delete Visit
            visitTriplet.Delete();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Visit_E2E()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Visitor Book Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Visitors Book");
            Wait.WaitForDocumentReady();
            //Adding a Visit
            var visitTriplet = new ManageVisitTriplet();
            var visitDetailPage = visitTriplet.Create();
            visitDetailPage.VisitorName = "Delete Visitor";
            visitDetailPage.VisitNote = "VisitNoteDescription";
            visitDetailPage.StartDate = DateTime.Now.ToString("M/d/yyyy");
            visitDetailPage.IsAllday = true;
            //Saving
            visitDetailPage.Save();
            Assert.AreEqual(false, visitDetailPage.IsSuccessMessageDisplayed(), "Visit record saved");
            //Visit Search
            visitTriplet.SearchCriteria.SearchByVisitorName = ("Delete Visitor");
            var searchResult = visitTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageVisitPage = searchResult.Click<ManageVisitDetail>();
            //Delete Visit
            visitTriplet.Delete();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Visit_FullName_Validation()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Visitor Book Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Visitors Book");
            Wait.WaitForDocumentReady();
            //Adding a Visit
            var visitTriplet = new ManageVisitTriplet();
            var visitDetailPage = visitTriplet.Create();
            visitDetailPage.VisitorName = "";
            visitDetailPage.StartDate = DateTime.Now.ToString("M/d/yyyy");
            visitDetailPage.IsAllday = true;
            //Saving
            visitDetailPage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageVisitDetail.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Visit_Date_Validation()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Visitor Book Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Visitors Book");
            Wait.WaitForDocumentReady();
            //Adding a Visit
            var visitTriplet = new ManageVisitTriplet();
            var visitDetailPage = visitTriplet.Create();
            visitDetailPage.VisitorName = "Visitor_" + SeleniumHelper.GenerateRandomString(10);
            visitDetailPage.IsAllday = false;
            //Saving
            visitDetailPage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageVisitDetail.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
    }
}
