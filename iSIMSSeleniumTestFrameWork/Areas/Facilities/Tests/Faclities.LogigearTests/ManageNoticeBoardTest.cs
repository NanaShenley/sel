using System;
using POM.Helper;
using NUnit.Framework;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;
using Facilities.POM.Components.ManageNotice;

namespace Faclities.LogigearTests
{
    public class ManageNoticeBoardTest
    {
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Add_Notice()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Notices Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Manage Notices");
            Wait.WaitForDocumentReady();
            //Adding New Notice
            var noticeTriplet = new ManageNoticeTriplet();
            var noticeDetailPage = noticeTriplet.Create();
            noticeDetailPage.NoticeTitle = "Notice_" + SeleniumHelper.GenerateRandomString(10);
            noticeDetailPage.NoticeDescription = "Description_" + SeleniumHelper.GenerateRandomString(10);
            //Saving new Notice
            noticeDetailPage.Save();
            Assert.AreEqual(false, noticeDetailPage.IsSuccessMessageDisplayed(), "Notice record saved");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Search_Notice()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Notices Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Manage Notices");
            Wait.WaitForDocumentReady();
            //Adding New Notice
            var noticeTriplet = new ManageNoticeTriplet();
            //Searching the Notice
            noticeTriplet.SearchCriteria.SearchByNoticeName = ("Notice");
            var searchResult = noticeTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageNoticePage = searchResult.Click<ManageNoticePage>();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Delete_Notice()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Notices Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Manage Notices");
            Wait.WaitForDocumentReady();
            //Adding New Notice
            var noticeTriplet = new ManageNoticeTriplet();
            //Searching the Notice
            noticeTriplet.SearchCriteria.SearchByNoticeName = ("Notice");
            var searchResult = noticeTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageNoticePage = searchResult.Click<ManageNoticePage>();
            //Delete Notice
            noticeTriplet.Delete();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void E2E_Notice()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Notices Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Manage Notices");
            Wait.WaitForDocumentReady();
            //Adding New Notice
            var noticeTriplet = new ManageNoticeTriplet();
            var noticeDetailPage = noticeTriplet.Create();
            noticeDetailPage.NoticeTitle = "Notice_" + SeleniumHelper.GenerateRandomString(10);
            noticeDetailPage.NoticeDescription = "Description_" + SeleniumHelper.GenerateRandomString(10);
            //Saving new Notice
            noticeDetailPage.Save();
            Assert.AreEqual(false, noticeDetailPage.IsSuccessMessageDisplayed(), "Notice record saved");
            //Searching the Notice
            noticeTriplet.SearchCriteria.SearchByNoticeName = ("Notice");
            var searchResult = noticeTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageNoticePage = searchResult.Click<ManageNoticePage>();
            //Delete Notice
            noticeTriplet.Delete();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Notice_Title_Validation()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Notices Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Manage Notices");
            Wait.WaitForDocumentReady();
            //Adding New Notice
            var noticeTriplet = new ManageNoticeTriplet();
            var noticeDetailPage = noticeTriplet.Create();
            noticeDetailPage.NoticeTitle = "";
            //Saving new Notice
            noticeDetailPage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageNoticePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Notice_Date_Comparison_Validation()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Notices Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Manage Notices");
            Wait.WaitForDocumentReady();
            //Adding New Notice
            var noticeTriplet = new ManageNoticeTriplet();
            var noticeDetailPage = noticeTriplet.Create();
            noticeDetailPage.NoticeTitle = "Notice";
            noticeDetailPage.NoticePostingDate = DateTime.Now.AddDays(2).ToString("M/d/yyyy");
            noticeDetailPage.NoticePostingExpiryDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            //Saving new Notice
            noticeDetailPage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageNoticePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Notice_Expiry_Date_Validation()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Notices Screen
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Manage Notices");
            Wait.WaitForDocumentReady();
            //Adding New Notice
            var noticeTriplet = new ManageNoticeTriplet();
            var noticeDetailPage = noticeTriplet.Create();
            noticeDetailPage.NoticeTitle = "Notice";
            noticeDetailPage.NoticePostingDate = DateTime.Now.AddDays(-2).ToString("M/d/yyyy");
            noticeDetailPage.NoticePostingExpiryDate = DateTime.Now.AddDays(-1).ToString("M/d/yyyy");
            //Saving new Notice
            noticeDetailPage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageNoticePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
    }
}
